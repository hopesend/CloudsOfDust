/*------------------------------------------------
// CUBEMAPPER Version 1.4.2
// Created by: Rainer Liessem
// Website: http://www.spreadcamp.com
//
// PLEASE RESPECT THE LICENSE TERMS THAT YOU
// AGREED UPON WITH YOUR PURCHASE OF THIS ASSET
------------------------------------------------*/
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class Cubemapper : MonoBehaviour
{	
	#region Variables
    // Generate Settings
	public bool generate = false;
	public bool makePNG = false; // makes editable PNG images if true
	
	// Output Folders
	public string pathCubemaps;
	public string pathCubemapPNG;
	
    // Cubemap Settings
    public bool useLinearSpace = true;
    public bool useMipMaps = false; // generates cubemaps with mipmaps when true
    public float mipMapBias = 0.0f;
    public bool smoothEdges = true;
    public int smoothEdgesWidth = 1;

	// Camera Settings
    public CameraClearFlags camClearFlags = CameraClearFlags.Skybox;
    public Color camBGColor = new Color(0.192f, 0.302f, 0.475f, 0.020f);
	public LayerMask cullingMask = -1;
	public float farClipPlane = 1000f;
	
	// Screenshot Settings
	public int resolution = 32;
	public int nodeResolution = 32; // this is used when nodes override

	// Status Stuff for the Custom Editor
	public bool isTakingScreenshots = false;
	public bool completedTakingScreenshots = false;	
	
	// Private members
	CubemapNode[] nodes = null;
	new Camera camera;
	enum DIR { Right, Left, Top, Bottom, Front, Back }
	DIR currentDir = DIR.Right;
	string sceneName;
	TextureFormat textureFormat = TextureFormat.RGB24;
	#endregion
	
	
	void OnDrawGizmos()	{
		Gizmos.DrawIcon(transform.position, "cManager.png"); 
	}

	void Start()
	{
		#if !UNITY_EDITOR
		Destroy(gameObject);
		#else
		// Are we to generate Cubemaps?
		if((generate && Application.isEditor && Application.isPlaying) && !isTakingScreenshots)
		{
			// Check for existing cameras and disable them to avoid interference
			DisableOtherCameras();
			
			// Create our Cubemapper camera
			GameObject newCam = new GameObject("Cubemap Camera (temporary during Generation)");
			newCam.AddComponent<Camera>();
			camera = newCam.GetComponent<Camera>();
			camera.nearClipPlane = 0.01f;
			camera.fieldOfView = 90;
			camera.aspect = 1.0f;
			camera.cullingMask = cullingMask;
			camera.farClipPlane = farClipPlane;
            camera.clearFlags = camClearFlags;
            camera.backgroundColor = camBGColor;
			
			// Find and assign all cubemap nodes in the scene, abort process if there are none
			FindNodes();
            			
			// Slow time to prevent weird things from happening in dynamic enviroments
			Time.timeScale = .0000001f;
			
			sceneName = Application.loadedLevelName;
			
			// Start taking Screenshots at our Nodes
			isTakingScreenshots = true;
			StartCoroutine("ScreenCapture");
		}
		else
			Destroy(gameObject);
		#endif
	}
	
	#if UNITY_EDITOR
	IEnumerator ScreenCapture()
	{
		Transform cam = camera.transform;
		
		while(isTakingScreenshots)
		{
			// Loop through each node
			for(int a=0; a<nodes.Length; a++)
			{
				// Make sure this Node is set to allow generation of either cubemaps or PNGs
				// Ignore the Allow parameter if we have no cubemap assigned yet at all
				if(nodes[a].allowCubemapGeneration || nodes[a].allowGeneratePNG || (!nodes[a].allowCubemapGeneration && nodes[a].cubemap == null) )
				{					
					// Set Camera
					cam.position = nodes[a].transform.position;				
					cam.rotation = Quaternion.identity;
					
					// Set resolution because Node may override
					SetNodeResolution(nodes[a]);
					
					// Make cubemap
					Cubemap cubemap = new Cubemap(nodeResolution, textureFormat, useMipMaps);
                    cubemap.mipMapBias = mipMapBias;

					// Loop through all Directions to take screenshots for this node
					for(int b=0; b<6; b++)
					{
						//Debug.Log("Processing Node " + nodes[a].name + " in Direction " + currentDir);
						
						switch(currentDir)
						{
							case DIR.Right:
								cam.rotation = Quaternion.Euler(0, 90, 0);						
								yield return StartCoroutine(MakeSnapshot(cubemap, CubemapFace.PositiveX, nodes[a]));
								currentDir = DIR.Left;
								break;
							
							case DIR.Left:
								cam.rotation = Quaternion.Euler(0, -90, 0);
								yield return StartCoroutine(MakeSnapshot(cubemap, CubemapFace.NegativeX, nodes[a]));
								currentDir = DIR.Top;
								break;
							
							case DIR.Top:
								cam.rotation = Quaternion.Euler(-90, 0, 0);
								yield return StartCoroutine(MakeSnapshot(cubemap, CubemapFace.PositiveY, nodes[a]));
								currentDir = DIR.Bottom;
								break;
							
							case DIR.Bottom:
								cam.rotation = Quaternion.Euler(90, 0, 0);
								yield return StartCoroutine(MakeSnapshot(cubemap, CubemapFace.NegativeY, nodes[a]));
								currentDir = DIR.Front;
								break;
							
							case DIR.Front:
								cam.rotation = Quaternion.Euler(0, 0, 0);
								yield return StartCoroutine(MakeSnapshot(cubemap, CubemapFace.PositiveZ, nodes[a]));
								currentDir = DIR.Back;
								break;
							
							case DIR.Back:
								cam.rotation = Quaternion.Euler(0, 180, 0);
								yield return StartCoroutine(MakeSnapshot(cubemap, CubemapFace.NegativeZ, nodes[a]));
								currentDir = DIR.Right; // back to the beginning (or else it gets stuck)
								break;
						}
                    }

                    // Smooth Edges on Unity 4+
                    if (smoothEdges)
                    {
                        cubemap.SmoothEdges(smoothEdgesWidth);
                        cubemap.Apply();
                    }					
			
					// Create Cubemap, but only if we are allowed to (unless there is no cubemap on the node yet)
                    if (nodes[a].allowCubemapGeneration || (!nodes[a].allowCubemapGeneration && nodes[a].cubemap == null))
                    {
                        string finalCubemapPath = pathCubemaps + "/" + sceneName + " - " + nodes[a].name + ".cubemap";
                        if (finalCubemapPath.Contains("//"))
                            finalCubemapPath = finalCubemapPath.Replace("//", "/");

                        AssetDatabase.CreateAsset(cubemap, finalCubemapPath);
                    }

                    // Set Linear Space if wanted
                    SerializedObject serializedCubemap = new SerializedObject(cubemap);
                    SetLinearSpace(ref serializedCubemap, useLinearSpace);
	
					// Free up memory to prevent memory leak
					Resources.UnloadUnusedAssets();
				}
			}
				
			AssetDatabase.Refresh();
			
			isTakingScreenshots = false;
			completedTakingScreenshots = true;
		}
		
		Debug.Log("CUBEMAPS GENERATED!");
	}
	
	
	void DisableOtherCameras()
	{
		Camera[] otherCams = FindObjectsOfType( typeof(Camera) ) as Camera[];
		
		foreach(Camera camera in otherCams)
		{
			camera.enabled = false;
		}
	}
	
	
	void SetNodeResolution(CubemapNode node)
	{
		nodeResolution = (node.overrideResolution) ? node.resolution : resolution;
	}
	
	
	void FindNodes()
	{
		nodes = FindObjectsOfType( typeof(CubemapNode) ) as CubemapNode[];
		//Debug.Log(nodes.Length + " | " + nodes);
		
		// Destroy process if htere are no nodes
		if(nodes.Length == 0)
		{
			Debug.LogError("There are no Cubemap Nodes in Scene to generate Cubemaps from! Please stop playback and add Cubemap Nodes to your Scene.");
			Destroy(gameObject);
		}
	}
	
	
	IEnumerator MakeSnapshot(Cubemap c, CubemapFace face, CubemapNode node)
	{
		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame();

		int width = Screen.width;
		int height = Screen.height;
	
		//Create the blank texture container
		Texture2D snapshot = new Texture2D(width, height, textureFormat, useMipMaps);
		snapshot.wrapMode = TextureWrapMode.Clamp;
	
		// Rectangle Area from the Camera
		Rect copyRect = new Rect((camera.pixelWidth * 0.5f) - (snapshot.width * 0.5f), (camera.pixelHeight * 0.5f) - (snapshot.height * 0.5f), snapshot.width, snapshot.height);
		
		//Read the current render into the texture container, snapshot
		snapshot.ReadPixels(copyRect, 0, 0, false);
		
		yield return null;
		
		snapshot.Apply();
		
		// Resize our Texture
		snapshot = Scale(snapshot, nodeResolution, nodeResolution);
		
		// Write mirrored pixels from our Texture to Cubemap
		Color cubemapColor;
		for (int y = 0; y<nodeResolution; y++)
        {
            for (int x = 0; x<nodeResolution; x++)
            {
				cubemapColor = snapshot.GetPixel(nodeResolution + x, (nodeResolution-1) - y);
				c.SetPixel(face, x, y, cubemapColor);
            }
        }
        c.Apply();	 
		 
		// Optional PNG generation. Double-check it with overriding Node setting
		if(makePNG && node.allowGeneratePNG)
		{
			// Mirror the snapshot image for our PNG in order to be identical with the cubemap faces
			snapshot.SetPixels32( MirrorColor32( c.GetPixels(face) ) );
			snapshot.Apply();
			
			// Convert to PNG file
			byte[] bytes = snapshot.EncodeToPNG();
			
			// Save the file			
			string path = Application.dataPath + "/" + pathCubemapPNG + "/" + sceneName + " - " + node.name + " - " + face.ToString() + ".png";
			//System.IO.File.WriteAllBytes(path, bytes); // deprecated because not available on Webplayer
			System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
			System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
			bw.Write(bytes);
			bw.Close();
			fs.Close();

            // Fix compression state
            string finalImagePath = "Assets/" + pathCubemapPNG + "/" + sceneName + " - " + node.name + " - " + face.ToString() + ".png";
            if (finalImagePath.Contains("//"))
                finalImagePath = finalImagePath.Replace("//", "/");

            AssetDatabase.Refresh(); // refresh necessary before we can use the textureimporter

            TextureImporter textureImporter = AssetImporter.GetAtPath(finalImagePath) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureFormat = TextureImporterFormat.RGB24;
                AssetDatabase.ImportAsset(finalImagePath);
            }
		}
		
		// Delete our screenshot texture as clean up
		DestroyImmediate(snapshot);
		
		yield return null;
	}
	
	Texture2D Scale(Texture2D source, int targetWidth, int targetHeight)
	{
		Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
		Color32[] rpixels = result.GetPixels32(0);
		float incX = ((float)1/source.width) * ((float)source.width/targetWidth);
		float incY = ((float)1/source.height) * ((float)source.height/targetHeight);

		for(int px=0; px<rpixels.Length; px++)
		{
			rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth)));
		}
		
		result.SetPixels32(rpixels, 0);
		result.Apply();
		
		return result;
	}	
	
	Color32[] MirrorColor32(Color[] source)
	{
		Color32[] mirroredColors = new Color32[source.Length];
		
		for(int m=0; m<source.Length; m++)
		{
			mirroredColors[m] = source[source.Length - 1 - m];
		}
		
		return mirroredColors;
	}

    // Setting of Linear Space
    void SetLinearSpace(ref SerializedObject obj, bool linear)
    {
        if (obj == null) return;

        SerializedProperty prop = obj.FindProperty("m_ColorSpace");
        if (prop != null)
        {
            prop.intValue = linear ? (int)ColorSpace.Gamma : (int)ColorSpace.Linear;
            obj.ApplyModifiedProperties();
        }
    }
	#endif
}
