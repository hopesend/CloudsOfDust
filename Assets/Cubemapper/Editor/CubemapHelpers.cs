/*------------------------------------------------
// CUBEMAPPER Version 1.4.2
// Created by: Rainer Liessem
// Website: http://www.spreadcamp.com
//
// PLEASE RESPECT THE LICENSE TERMS THAT YOU
// AGREED UPON WITH YOUR PURCHASE OF THIS ASSET
------------------------------------------------*/
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CubemapHelpers : MonoBehaviour
{
	#region GUI Helpers
	// A CUSTOM LAYER MASK FIELD (because Unity lacks them for Editor Scripts)
	// Based on the Code from "Aron Granberg" at Unity Answers (http://answers.unity3d.com/questions/60959/mask-field-in-the-editor.html)
	static public LayerMask LayerMaskField (string label, LayerMask selected, bool showSpecial = true)
	{	
	    List<string> layers = new List<string>();
	    List<int> layerNumbers = new List<int>();
	
	    string selectedLayers = "";
	
	    for (int i=0;i<32;i++)
		{	
			string layerName = LayerMask.LayerToName (i);
	
			if (layerName != "")
			{
				if (selected == (selected | (1 << i)))
				{
					if (selectedLayers == "")
						selectedLayers = layerName;
					else
						selectedLayers = "Mixed";
				}
			}
		}
	
	    //EventType lastEvent = Event.current.type; // Only debug
	
	    if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.ExecuteCommand)
		{
			if (selected.value == 0)
				layers.Add ("Nothing");
			else if (selected.value == -1)
				layers.Add ("Everything");
			else
				layers.Add (selectedLayers);
		
			layerNumbers.Add (-1);
	    }
	
	    if (showSpecial)
		{
	       layers.Add ((selected.value == 0 ? "[X] " : "     ") + "Nothing");
	       layerNumbers.Add (-2);
	
	       layers.Add ((selected.value == -1 ? "[X] " : "     ") + "Everything");
	       layerNumbers.Add (-3);
	    }
	
	    for (int i=0;i<32;i++)
		{
			string layerName = LayerMask.LayerToName (i);
	
			if (layerName != "")
			{
				if (selected == (selected | (1 << i)))
					layers.Add ("[X] "+layerName);
				else
					layers.Add ("     "+layerName);
				
				layerNumbers.Add (i);
			}
	    }
	
	    bool preChange = GUI.changed;
	
	    GUI.changed = false;
	
	    int newSelected = 0;
	
	    if(Event.current.type == EventType.MouseDown)
			newSelected = -1;
	
		newSelected = EditorGUILayout.Popup(label, newSelected, layers.ToArray(), EditorStyles.layerMaskField);
	
	    if (GUI.changed && newSelected >= 0)
		{
			//newSelected -= 1;
			//Debug.Log (lastEvent +" "+newSelected + " "+layerNumbers[newSelected]);
	
			if (showSpecial && newSelected == 0)
				selected = 0;
			else if (showSpecial && newSelected == 1)
				selected = -1;
			else
			{
				if (selected == (selected | (1 << layerNumbers[newSelected])))
				{
					selected &= ~(1 << layerNumbers[newSelected]);
					//Debug.Log ("Set Layer "+LayerMask.LayerToName (LayerNumbers[newSelected]) + " To False "+selected.value);
				}
				else
				{
					//Debug.Log ("Set Layer "+LayerMask.LayerToName (LayerNumbers[newSelected]) + " To True "+selected.value);
					selected = selected | (1 << layerNumbers[newSelected]);
				}
			}
	    } else {
			GUI.changed = preChange;
	    }
		
		return selected;
	}
	
	
	// Serialized Object Field for Cubemaps, stylizable (but no Labels)
	static public void SerializedCubemapField(SerializedProperty property, bool allowSceneObjects, GUILayoutOption[] layout)
	{
		Rect position = new Rect(0,0,0,0);
		GUIContent label = new GUIContent();
		label = EditorGUI.BeginProperty(position, label, property);
		
		EditorGUI.BeginChangeCheck();
				
		Cubemap newValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Cubemap), allowSceneObjects, layout) as Cubemap;
		
		// Only assign the value back if it was actually changed by the user.
	    // Otherwise a single value will be assigned to all objects when multi-object editing,
	    // even when the user didn't touch the control.
		if(EditorGUI.EndChangeCheck())
			property.objectReferenceValue = newValue;
		
		EditorGUI.EndProperty();
	}
	
	// SEPERATOR for GUI Elements
	static Texture2D seperatorTexture;
	
	static public void Separator(float padding = 5f)
	{
		GUILayout.Space(padding);

		if(Event.current.type == EventType.Repaint)
		{
			if(seperatorTexture == null)
				seperatorTexture = MakeTexture();
			
			Texture2D tex = seperatorTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.5f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 2f), tex);
			GUI.color = Color.white;
		}
		
		GUILayout.Space(padding);
	}
	static Texture2D MakeTexture()
	{
		Texture2D t = new Texture2D(1, 1);
		t.hideFlags = HideFlags.DontSave;
		t.SetPixel(0, 0, Color.white);
		t.Apply();		
		return t;
	}
	#endregion

    #region GUI Colors
    static public Color ColorGreen
    {
        get { return new Color(0.55f, 0.8f, 0.24f, 1f); }
    }

    static public Color ColorBlue
    {
        get { return new Color(0.43f, 0.78f, 1f, 1f); }
    }

    static public Color ColorRed
    {
        get { return new Color(0.8f, 0.3f, 0.2f, 1f); }
    }

    static public Color ColorOrange
    {
        get { return new Color(1f, 0.7f, 0.18f, 1f); }
    }
    #endregion

    #region File Helpers
    /// <summary>
	/// Transform a given path into something that Unity can read. Requires presence of "Assets" folder
	/// </summary>
	/// <returns>
	/// Sanitized relative path
	/// </returns>
	/// <param name='rawPath'>
	/// Raw path, like C:/MyProjects/MyGame/Unity/Assets/Subfolder
	/// Path needs to contain a folder called "Assets"
	/// </param>
	static public string MakeUnityPath(string rawPath, bool includeAssetsFolder = true)
	{
		// Split the path and reassemble into something Unity can read
		string[] splitPath = rawPath.Split(new char[] {'\\', '/'} );		
		string searchPath = null;		
		
		bool reassemblePath = false;
		
		for(int i=0; i<splitPath.Length; i++)
		{
			string name = splitPath[i];
			
			// Assets Folder found, tells us we should start reassembly of string from here
			if(name == "Assets")
				reassemblePath = true;
			
			if(reassemblePath)
			{
				// Skip Assets folder if defined
                if (name == "Assets" && !includeAssetsFolder)
                {
                    // do nothing
                }
                // Proceed as normal
                else
                    searchPath += splitPath[i] + "/";
			}
		}

        // Remove Last Slash     
        if (!string.IsNullOrEmpty(searchPath) && ( searchPath.Trim().EndsWith("/") || searchPath.Trim().EndsWith(@"\")))
        {
            //Debug.Log("Last slash found!");
            //searchPath = searchPath.Remove((searchPath.Length - 1), 1);
            searchPath = searchPath.TrimEnd(searchPath[searchPath.Length - 1]);
        }

		return searchPath;
	}
	
	// Verify a given path for existance
	static public string VerifyPath(string path)
	{
		if(string.IsNullOrEmpty(path))
			return "Could not verify Path";
			
		// Might be a little crude since it does not consider Unity's relative path format
		string getPath = Path.GetDirectoryName(path);
		//bool getDirectory = Directory.Exists(path); // can't use this because we deal in unity's relative paths
		
		if(string.IsNullOrEmpty(getPath))
			return "!! ERROR - PASS FAULTY !!";
		//else if(!getDirectory)
			//return "!! ERROR - DOES NOT EXIST !!";
		else
			return "PATH OK!";
	}
	#endregion
	
	#region Texture Helpers
	static public Texture2D Scale(Texture2D source, int targetWidth, int targetHeight)
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
	
	static public Color32[] MirrorColor32(Color[] source)
	{
		Color32[] mirroredColors = new Color32[source.Length];
		
		for(int m=0; m<source.Length; m++)
		{
			mirroredColors[m] = source[source.Length - 1 - m];
		}
		
		return mirroredColors;
	}
	#endregion

    #region Cubemap Generation Helpers
    static public void CreateCubemapWithPro(int resolution, TextureFormat textureFormat, bool useLinearSpace, bool useMipMaps, float mipMapBias, string outputPathCubemap, bool makePNG, string outputPathPNG, CameraClearFlags camClearFlags, Color camBGColor, float camFarClipPlane, int camCullingMask, bool smoothEdges, int smoothEdgeWidth)
    {
        CubemapNode[] nodes = FindObjectsOfType(typeof(CubemapNode)) as CubemapNode[];
        for (int a = 0; a < nodes.Length; a++)
        {
            // Make sure this Node is set to allow generation of either cubemaps or PNGs
            // Ignore the Allow parameter if we have no cubemap assigned yet at all
            if (nodes[a].allowCubemapGeneration || (!nodes[a].allowCubemapGeneration && nodes[a].cubemap == null))
            {
                // Does node request a custom resolution?
                if (nodes[a].overrideResolution)
                    resolution = nodes[a].resolution;

                // Create our Cubemap File that we will render into
                Cubemap cubemap = new Cubemap(resolution, textureFormat, useMipMaps);
                cubemap.mipMapBias = mipMapBias;
                string finalCubemapPath = outputPathCubemap + "/" + Path.GetFileNameWithoutExtension(EditorApplication.currentScene) + " - " + nodes[a].name + ".cubemap";
                if (finalCubemapPath.Contains("//"))
                    finalCubemapPath = finalCubemapPath.Replace("//", "/");

                AssetDatabase.CreateAsset(cubemap, finalCubemapPath);

                // create and position temporary camera for rendering
                var go = new GameObject("CubemapCamera", typeof(Camera));
                go.transform.position = nodes[a].transform.position;
                go.transform.rotation = Quaternion.identity;

                // Camera setup
                var cam = go.GetComponent<Camera>();
                cam.clearFlags = camClearFlags;
                cam.backgroundColor = camBGColor;
                cam.farClipPlane = camFarClipPlane;
                cam.cullingMask = camCullingMask;

                // render into cubemap		
                cam.RenderToCubemap(cubemap);

                // Smooth Edges on Unity 4.0+
                if (smoothEdges)
                {
                    cubemap.SmoothEdges(smoothEdgeWidth);
                    cubemap.Apply();
                }

                // Use Linear Space?
                SerializedObject serializedCubemap = new SerializedObject(cubemap);
                CubemapHelpers.setLinear(ref serializedCubemap, useLinearSpace);

                // Destroy temp camera
                DestroyImmediate(go);

                // Extract PNG if Allowed
                if (makePNG && nodes[a].allowGeneratePNG)
                {
                    CubemapHelpers.CubemapToPNG(cubemap, outputPathPNG);
                }

                AssetDatabase.Refresh();
                Selection.activeObject = cubemap;
            }
        }

        Debug.Log("CUBEMAPS GENERATED!");
        EditorUtility.DisplayDialog("Cubemaps generated!", "You can now proceed assigning your cubemaps to your objects.", "Yay!");
    }

    static public void CubemapToPNG(Cubemap sourceCubemap, string outputFolderPath, bool includePositiveX, bool includeNegativeX, bool includePositiveY, bool includeNegativeY, bool includePositiveZ, bool includeNegativeZ)
    {
        // Set up our local variables for later use
        int sizeX = sourceCubemap.width;
        int sizeY = sourceCubemap.height;
        CubemapFace face = CubemapFace.PositiveX;

        // Loop through all Directions
		for(int b=0; b<6; b++)
		{
			switch(b)
			{
				case 0:
                    if (includePositiveX)
                        face = CubemapFace.PositiveX;
					break;
				
				case 1:
                    if(includeNegativeX)
    					face = CubemapFace.NegativeX;
					break;
				
				case 2:
                    if(includePositiveY)
					    face = CubemapFace.PositiveY;
					break;
				
				case 3:
                    if(includeNegativeY)
					    face = CubemapFace.NegativeY;
					break;
				
				case 4:
                    if(includePositiveZ)
					    face = CubemapFace.PositiveZ;
					break;
				
				case 5:
                    if(includeNegativeZ)
					    face = CubemapFace.NegativeZ;
					break;
			}
			
            // If Face is +X but +X is not allowed then we just skip it
            // We have to do this because our initial Face variable is set to PositiveX,
            // which without this condition would cause PositiveX to always be created
            if (face == CubemapFace.PositiveX && !includePositiveX)
            {
                // Do Nothing
            }
            else
            {
                //Create the blank texture container
                Texture2D snapshot = new Texture2D(sizeX, sizeY, TextureFormat.RGB24, true);
                snapshot.wrapMode = TextureWrapMode.Clamp;

                Color[] cubemapColors = sourceCubemap.GetPixels(face);
                snapshot.SetPixels32(CubemapHelpers.MirrorColor32(cubemapColors)); // Mirror the snapshot image for our PNG in order to be identical with the cubemap faces
                snapshot.Apply();

                // Convert to PNG file
                byte[] bytes = snapshot.EncodeToPNG();

                // Save the file
                string path = Application.dataPath + "/" + outputFolderPath + "/" + sourceCubemap.name + " - " + face.ToString() + ".png";
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();

                // Fix compression state
                string finalImagePath = CubemapHelpers.MakeUnityPath(Application.dataPath + "/" + outputFolderPath + "/" + sourceCubemap.name + " - " + face.ToString() + ".png");
                if(finalImagePath.Contains("//"))
                    finalImagePath = finalImagePath.Replace("//", "/");

                AssetDatabase.Refresh(); // refresh necessary before we can use the textureimporter

                TextureImporter textureImporter = AssetImporter.GetAtPath(finalImagePath) as TextureImporter;
                if (textureImporter != null)
                {
                    textureImporter.textureFormat = TextureImporterFormat.RGB24;
                    AssetDatabase.ImportAsset(finalImagePath);
                }

                DestroyImmediate(snapshot);
            }
		}
    }
    
    static public void CubemapToPNG(Cubemap sourceCubemap, string outputFolderPath)
    { 
        CubemapHelpers.CubemapToPNG(sourceCubemap, outputFolderPath, true, true, true, true, true, true);
    }

    // Is this Cubemap using Mip Maps?
    public static bool usingMipMap(SerializedObject obj)
    {
        if (obj == null) return false;

        SerializedProperty prop = obj.FindProperty("m_MipMap");
        if (prop != null)
            return prop.boolValue;

        return false;
    }

    // Set to use Mip Maps
    public static void setMipMap(ref SerializedObject obj, bool useMipMap)
    {
        if (obj == null) return;

        SerializedProperty prop = obj.FindProperty("m_MipMap");
        if (prop != null)
        {
            prop.boolValue = useMipMap;
            obj.ApplyModifiedProperties();
        }
    }

    // Is this Cubemap in Linear space?
    public static bool isLinear(SerializedObject obj)
    {
        if (obj == null) return false;

        SerializedProperty prop = obj.FindProperty("m_ColorSpace");
        if (prop != null)
            return prop.intValue == (int)ColorSpace.Gamma;

        return false;
    }

    // Setting of Linear Space
    public static void setLinear(ref SerializedObject obj, bool linear)
    {
        if (obj == null) return;

        SerializedProperty prop = obj.FindProperty("m_ColorSpace");
        if (prop != null)
        {
            prop.intValue = linear ? (int)ColorSpace.Gamma : (int)ColorSpace.Linear;
            obj.ApplyModifiedProperties();
        }
    }
    #endregion

    #region Misc
    /// <summary>
    /// Returns the Clearflags based on a selected number. Useful for custom selection from a Popup List populated by a string array.
    /// It expects the numbers to represent a certain order, 0=Skybox, 1=Solid Color, 2=Depth Only, 3=Don't Clear
    /// </summary>
    /// <param name="selected"></param>
    /// <returns>CameraClearFlags</returns>
    static public CameraClearFlags SetClearFlagFromInt(int selected)
    {
        CameraClearFlags flags = CameraClearFlags.Skybox;

        switch(selected)
        {
            case 0: flags = CameraClearFlags.Skybox; break;
            case 1: flags = CameraClearFlags.SolidColor; break;
            case 2: flags = CameraClearFlags.Depth; break;
            case 3: flags = CameraClearFlags.Nothing; break;
        }

        return flags;
    }
    #endregion
}