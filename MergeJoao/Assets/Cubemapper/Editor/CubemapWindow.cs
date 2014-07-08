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
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CubemapWindow : EditorWindow
{
	#region Variables
	Cubemapper manager			= null; 
	
    // Version related
    string version = "1.4.4";
    string assetURL = "https://www.assetstore.unity3d.com/#/content/3041";

	// Cubemap Resolution
	string[] resolutions		= {"32x32", "64x64", "128x128", "256x256", "512x512", "1024x1024", "2048x2048" };
	int[] resSizes				= {32, 64, 128, 256, 512, 1024, 2048};

    // Camera Clear Flags
    string[] m_camClearFlagOptions  = { "Skybox", "Solid Color", "Depth only", "Don't Clear" };
    int m_camClearFlagSelected      = 0;
	
	// Window Settings
	Vector2 scrollPos;
	bool generationInProgress	= false;
	
	bool showAdvancedSettings   = false;
	
	string currentMenu			= "Generate";
	float horizontalWidth		= 325f;
	
	string regPrefix			= "cubemapper_"; // for EditorPrefs

	// Images
	List<Texture> windowImages	= new List<Texture>();
	Texture img_bttn_generate	= null;
	Texture img_bttn_assignment	= null;
	Texture img_bttn_tools		= null;
	Texture img_bttn_settings	= null;
	Texture img_bttn_addNode	= null;
	
	// User Settings
	bool centerNodesOnScreen	= true;
	float nodeDistanceToCam		= 2f;
	
	// Variables to pass to Cubemap Manager
	string cm_pathCubemaps		= "Assets/Cubemapper/Generated Cubemaps";
	string cm_pathCubemapPNG	= "Cubemapper/Generated Cubemap Textures";
    bool cm_useLinearSpace      = true;
	bool cm_useMipMaps			= false;
	int cm_resolution			= 32;
	bool cm_makePNG				= false;
    float cm_mipmapBias         = 0.0f;
    bool cm_smoothEdges         = false;
    int cm_smoothEdgeWidth      = 1;

    CameraClearFlags cm_camClearFlags   = CameraClearFlags.Skybox;
    Color cm_camBGColor                 = new Color(0.192f, 0.302f, 0.475f, 0.020f);
	LayerMask cm_cullingMask	        = -1;
	float cm_farClipPlane	        	= 1000f;
    #endregion
	
    
	#region Initializing and Destroying of the Window
    [MenuItem ("Window/Cubemapper")]
    static void Init()
	{
        if (Path.GetFileNameWithoutExtension(EditorApplication.currentScene).Length == 0)
        {
            EditorUtility.DisplayDialog("Please save your Scene first!", "You can't use the Cubemapper on unsaved Scenes.", "OK");
            return;
        }
		
		// Window Set-Up
        CubemapWindow window = EditorWindow.GetWindow(typeof(CubemapWindow), false, "Cubemapper", true) as CubemapWindow;
		window.minSize = new Vector2(341, 250);
        window.autoRepaintOnSceneChange = true;
		window.Show();
    }
	
	// Window opens...
	void OnEnable()
	{
		// Destroy old instances of the manager
		DestroyManager();
		
		// Load Settings
		LoadPrefs();
		
		// Load our Images for later use throughout the window
		LoadImages();
	}
	
	// Window closes...
	void OnDestroy() {
		DestroyManager(); // Destroy old instances of the manager
		SavePrefs();
	}
	#endregion
	
	#region Editor GUI and Menus
    void OnGUI()
	{
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		// Don't display anything if we are generating
		if(generationInProgress)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox("Creating Cubemaps, please wait... do NOT stop playback!", MessageType.Warning);
			EditorGUILayout.EndHorizontal();
		}
		// Proceed with display
		else
		{
			MenuButtons();			
			//EditorGUILayout.Space();				
			ShowMenu(currentMenu);
		}
		
		EditorGUILayout.EndScrollView();
		
		if(GUI.changed)
			SavePrefs();
		
		Repaint();
    } 
		
	void MenuButtons()
	{
		GUILayoutOption[] buttonLayout = new GUILayoutOption[] { GUILayout.Height(55), GUILayout.Width(60) };
		
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal( );
		
		// Load Images (if they are null)
		if(img_bttn_generate == null)
			img_bttn_generate = GetImage("CIMG_BTTN_GENERATE");
	
		if(img_bttn_assignment == null)
			img_bttn_assignment = GetImage("CIMG_BTTN_ASSIGNMENT");
		
		if(img_bttn_addNode == null)
			img_bttn_addNode = GetImage("CIMG_BTTN_ADDNODE");
		
		if(img_bttn_tools == null)
			img_bttn_tools = GetImage("CIMG_BTTN_TOOLS");
		
		if(img_bttn_settings == null)
			img_bttn_settings = GetImage("CIMG_BTTN_SETTINGS");
		
		
		// BUTTONS
		Color buttonColorDefault	= Color.gray;
		Color buttonColorHover		= CubemapHelpers.ColorRed;
				
		// Generate Button
		GUI.backgroundColor = (currentMenu == "Generate") ? buttonColorHover : buttonColorDefault;		
		if(GUILayout.Button(img_bttn_generate, buttonLayout ))
		{
			currentMenu = "Generate";
		}
		
		// Assign Button
		GUI.backgroundColor = (currentMenu == "Assignment") ? buttonColorHover : buttonColorDefault;		
		if(GUILayout.Button(img_bttn_assignment, buttonLayout ))
		{
			currentMenu = "Assignment";
		}
		
		// Node Button
		GUI.backgroundColor = buttonColorDefault;		
		if(GUILayout.Button(img_bttn_addNode, buttonLayout ))
		{
			Undo.RegisterSceneUndo("Create Cubemap Node");
			MakeNode();
		}
		
		// Tools Button		
		GUI.backgroundColor = (currentMenu == "Tools") ? buttonColorHover : buttonColorDefault;		
		if(GUILayout.Button(img_bttn_tools, buttonLayout ))
		{
			currentMenu = "Tools";
		}	
		
		// Settings Button	
		GUI.backgroundColor = (currentMenu == "Settings") ? buttonColorHover : buttonColorDefault;
		if(GUILayout.Button(img_bttn_settings, buttonLayout ))
		{
			currentMenu = "Settings";
		}
		
		GUI.backgroundColor = Color.white;
		
		EditorGUILayout.EndHorizontal();
		
		CubemapHelpers.Separator();		
	}
	
	
	void ShowMenu(string menu)
	{
		// Make sure we have only one Node container in the Scene
		if(CountNodeContainers() > 1)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox("It looks like there is more than one Node Container present in the scene. Please make sure there is only one Container before proceeding!", MessageType.Error);
			EditorGUILayout.EndHorizontal();
		}
		// Check if we have made nodes in the scene
		else if(CountNodes() == 0)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox("Please add one or more Cubemap Nodes to your Scene. You can't generate Cubemaps until Nodes are present.\n\nNodes are used to indicate where in your Environment you'd like to take Cubemaps from. They also are used for the One-Click Assign of your Cubemaps to Cubemap Users (see Readme)", MessageType.Warning);
			EditorGUILayout.EndHorizontal();
		}		
		// We have Nodes, we can now proceed using the rest of the System...
		else
		{
			if(menu == null) return;
			
			switch(menu)
			{
				case "Generate":	Menu_Generate();	break;			
				case "Assignment":	Menu_Assignment();	break;
				case "Tools":		Menu_Tools(); 		break;
				case "Settings":	Menu_Settings();	break;
			}
		}
	}
	
	void Menu_Generate()
    {
        GUI.color = CubemapHelpers.ColorBlue;
        GUILayout.Label("Cubemap Generation", EditorStyles.boldLabel);
        GUI.color = Color.white;
        GUILayout.Label("Configure your settings, then press the button to generate.", EditorStyles.miniBoldLabel);
        
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(horizontalWidth));
        {
            cm_makePNG = EditorGUILayout.Toggle("Generate PNGs? (slow)", cm_makePNG);
            cm_resolution = EditorGUILayout.IntPopup("Resolution: ", cm_resolution, resolutions, resSizes);

            EditorGUILayout.Space();

            // Advanced Options Foldout
            GUI.color = CubemapHelpers.ColorOrange;
            showAdvancedSettings = EditorGUILayout.Foldout(showAdvancedSettings, "Advanced Settings");
            GUI.color = Color.white;
            if (showAdvancedSettings)
            {
                GUI.color = CubemapHelpers.ColorBlue;
                EditorGUILayout.LabelField("Misc. Settings", EditorStyles.miniBoldLabel);
                GUI.color = Color.white;

                cm_useMipMaps = EditorGUILayout.Toggle("Mip Maps?", cm_useMipMaps);
                cm_mipmapBias = EditorGUILayout.Slider("Mip Map Bias", cm_mipmapBias, -10f, 10f);

                cm_useLinearSpace = EditorGUILayout.Toggle("Use Linear Space", cm_useLinearSpace);

#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5
                cm_smoothEdges = EditorGUILayout.Toggle("Smooth Edges?", cm_smoothEdges);
                if (cm_smoothEdges)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(150));
                        {
                            EditorGUILayout.LabelField("Edge Smooth Width:", GUILayout.Width(150));
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(GUILayout.Width(30));
                        {
                            cm_smoothEdgeWidth = EditorGUILayout.IntField(cm_smoothEdgeWidth, GUILayout.Width(30));
                        }
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(GUILayout.Width(30));
                        {
                            EditorGUILayout.LabelField("px", GUILayout.Width(30));
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
#endif

                EditorGUILayout.Space();

                GUI.color = CubemapHelpers.ColorBlue;
                EditorGUILayout.LabelField("Advanced Camera Settings", EditorStyles.miniBoldLabel);
                GUI.color = Color.white;

                m_camClearFlagSelected = EditorGUILayout.Popup("Clear Flags", m_camClearFlagSelected, m_camClearFlagOptions);
                cm_camClearFlags = CubemapHelpers.SetClearFlagFromInt(m_camClearFlagSelected);
                if (cm_camClearFlags == CameraClearFlags.Color || cm_camClearFlags == CameraClearFlags.Skybox)
                {
                    cm_camBGColor = EditorGUILayout.ColorField("Background Color:", cm_camBGColor);
                }

                cm_cullingMask = CubemapHelpers.LayerMaskField("Culling Mask", cm_cullingMask, true);
                cm_farClipPlane = EditorGUILayout.FloatField("Far Clipping Plane", cm_farClipPlane);
            }

            EditorGUILayout.Space();

            CubemapHelpers.Separator();

            GUI.backgroundColor = CubemapHelpers.ColorGreen;
            if (GUILayout.Button("Generate Cubemaps!", GUILayout.Height(40)))
                InitGeneration();
            else
                DestroyManager();

            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndVertical();
    }
	
	
	void Menu_Assignment()
    {
        GUI.color = CubemapHelpers.ColorBlue;
        GUILayout.Label("Cubemap One-Click Assignment", EditorStyles.boldLabel);
        GUI.color = Color.white;
        GUILayout.Label("Quickly assign cubemaps to your objects (see Readme)", EditorStyles.miniBoldLabel);

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(horizontalWidth));
        {
            // Check if we have cubemap users in the scene
            if (CountUsers() == 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("You need to assign Cubemap Users if you want to make use of the automatic assign. Select a game object first, then press the button", MessageType.Error);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                // Button to add users (only when made Selections)
                if (Selection.transforms.Length != 0)
                {
                    if (GUILayout.Button("Convert Selection(s)\nto Cubemap User", GUILayout.Height(35), GUILayout.MinWidth(130)))
                    {
                        MakeTransformsToUser();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            // Everything is fine
            else
            {
                EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = CubemapHelpers.ColorBlue;
                if (GUILayout.Button("Assign Cubemaps", GUILayout.Height(40)))
                    AssignCubemaps();
                GUI.backgroundColor = Color.white;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                // Button to add users (only when made Selections)
                if (Selection.transforms.Length != 0)
                {
                    if (GUILayout.Button("Convert Selection(s)\nto Cubemap User", GUILayout.Height(35), GUILayout.MinWidth(130)))
                    {
                        MakeTransformsToUser();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            // Warning if some Nodes don't have Cubemaps
            if (!NodesHaveCubemaps())
            {
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Some Nodes don't seem to have a Cubemap assigned yet. Please generate some Cubemaps and press \"Build Cubemaps\" to assign them.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();

                // Show a Button to report which Nodes don't have Cubemaps
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Log Nodes with missing Cubemaps", GUILayout.Height(25)))
                {
                    ListNodesWithoutCubemaps();
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }
	
	void Menu_Tools()
    {
        GUI.color = CubemapHelpers.ColorBlue;
        GUILayout.Label("Tools", EditorStyles.boldLabel);
        GUI.color = Color.white;
        GUILayout.Label("You might find these additional tools useful when working with\nCubemaps. Tools marked as \"Experimental\" should be used at\nyour own risk and could produce unexpected results.", EditorStyles.miniBoldLabel);

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(horizontalWidth - 10f));
        {
            GUI.backgroundColor = CubemapHelpers.ColorBlue;

            GUILayout.Space(5);

            if (GUILayout.Button("Extract PNG from existing Cubemap", GUILayout.Height(35)))
            {
                CubemapToolPNGExtract pngWindow = EditorWindow.GetWindow(typeof(CubemapToolPNGExtract), false, "PNG Extract", true) as CubemapToolPNGExtract;
                pngWindow.outputPath = (EditorPrefs.HasKey(regPrefix + "PNGToolOutputPath"))
                    ? EditorPrefs.GetString(regPrefix + "PNGToolOutputPath") : cm_pathCubemapPNG;
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Property Applier for existing Cubemaps\n(Experimental)", GUILayout.Height(35)))
            {
                CubemapToolPropertyModifier propertyWindow = EditorWindow.GetWindow(typeof(CubemapToolPropertyModifier), false, "Property Applier", true) as CubemapToolPropertyModifier;
                propertyWindow.minSize = new Vector2(341, 400);
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Cubemap Users Overview\n(Experimental)", GUILayout.Height(35)))
            {
                EditorWindow.GetWindow(typeof(CubemapToolUserOverview), false, "C-User List", true);
            }

            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndVertical();
    }
	
	void Menu_Settings()
    {
        GUI.color = CubemapHelpers.ColorBlue;
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        GUI.color = Color.white;
        GUILayout.Label("Some general settings to improve your workflow.", EditorStyles.miniBoldLabel);

        CubemapHelpers.Separator();

        // Version Display
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            {
                GUILayout.Label("Version: " + version, EditorStyles.miniBoldLabel);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            {
                GUI.backgroundColor = CubemapHelpers.ColorGreen;
                if (GUILayout.Button("Check Version"))
                {
                    if (EditorUtility.DisplayDialog("Opens Browser", "The quickest way to compare your version is to go to the products page on the Asset Store. If you press OK, we will take you there. Are you sure you want to open it in your browser?", "OK, fine.", "No!"))
                    {
                        Application.OpenURL(assetURL);
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();

        CubemapHelpers.Separator();

        //==== Node Settings
        GUI.color = CubemapHelpers.ColorOrange;
        GUILayout.Label("Node Settings", EditorStyles.boldLabel);
        GUILayout.Label("Affects placement of new Nodes", EditorStyles.miniBoldLabel);
        GUI.color = Color.white;

        centerNodesOnScreen = EditorGUILayout.Toggle("Center to Viewport", centerNodesOnScreen);
        if (centerNodesOnScreen)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(50));
            nodeDistanceToCam = EditorGUILayout.FloatField("Node Distance to Cam", nodeDistanceToCam);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Units");
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        CubemapHelpers.Separator();

        GUI.color = CubemapHelpers.ColorOrange;
        GUILayout.Label("Output Folders", EditorStyles.boldLabel);
        GUILayout.Label("Path needs to be a folder inside your projects \"Assets\" folder.\n\nNOTE: The path is saved to the registry and used globally.\nIf you have multiple projects then please make sure they all\nuse the same folder structure in order to avoid errors!", EditorStyles.miniBoldLabel);
        GUI.color = Color.white;

        //==== Cubemap Output Folder
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        GUILayout.Label("Cubemap Output Folder", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.MinWidth(200), GUILayout.MaxWidth(200));
        GUILayout.Label(CubemapHelpers.VerifyPath(cm_pathCubemaps), EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(65));
        {
            GUI.backgroundColor = CubemapHelpers.ColorBlue;
            if (GUILayout.Button("Choose..", GUILayout.MaxWidth(65)))
            {
                string newCubemapPath = EditorUtility.OpenFolderPanel("Specify Output Folder for Cubemaps", "", "");
                string sanitizedCubemapPath = CubemapHelpers.MakeUnityPath(newCubemapPath, true);

                // Path seems ok, proceed assigning to variable
                if (!string.IsNullOrEmpty(sanitizedCubemapPath))
                {
                    cm_pathCubemaps = sanitizedCubemapPath;
                    //Debug.Log("Sanitized Path: " + sanitizedCubemapPath + " | Raw Path: " + newCubemapPath);
                }
                // Something went wrong, show error
                else
                    EditorUtility.DisplayDialog("Path NOT changed", "Path was not changed because selection of the new Output folder was either aborted or invalid. Please try again or select a different folder.", "Okay");
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        GUILayout.Label(cm_pathCubemaps);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space();


        //==== PNG Output Folder		
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(200));
        GUILayout.Label("PNG Output Folder", EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.MinWidth(200), GUILayout.MaxWidth(200));
        GUILayout.Label(CubemapHelpers.VerifyPath(cm_pathCubemapPNG), EditorStyles.boldLabel);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(65));
        {
            GUI.backgroundColor = CubemapHelpers.ColorBlue;
            if (GUILayout.Button("Choose..", GUILayout.MaxWidth(65)))
            {
                string newCubemapPNGPath = EditorUtility.OpenFolderPanel("Specify Output Folder for PNG Files", "", "");
                string sanitizedCubemapPNGPath = CubemapHelpers.MakeUnityPath(newCubemapPNGPath, false);

                // Path seems ok, proceed assigning to variable
                if (!string.IsNullOrEmpty(sanitizedCubemapPNGPath))
                {
                    cm_pathCubemapPNG = sanitizedCubemapPNGPath;
                    //Debug.Log("Sanitized Path: " + sanitizedCubemapPNGPath + " | Raw Path: " + newCubemapPNGPath);
                }
                // Something went wrong, show error
                else
                    EditorUtility.DisplayDialog("Path NOT changed", "Path was not changed because selection of the new Output folder was either aborted or invalid. Please try again or select a different folder.", "Okay");
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        GUILayout.Label("Assets/" + cm_pathCubemapPNG);
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
	#endregion
	
	#region Cubemapper Features
	// Set's manager up for generation and goes to playmode
	void InitGeneration()
	{
        // Destroy old instances of the manager
        DestroyManager();

        // UNITY PRO - Capturing Cubemap without playmode
        if (UnityEditorInternal.InternalEditorUtility.HasPro())
        {
            CubemapHelpers.CreateCubemapWithPro(cm_resolution, TextureFormat.RGB24, cm_useLinearSpace, cm_useMipMaps, cm_mipmapBias, cm_pathCubemaps, cm_makePNG, cm_pathCubemapPNG, cm_camClearFlags, cm_camBGColor, cm_farClipPlane, cm_cullingMask, cm_smoothEdges, cm_smoothEdgeWidth);
        }
        // UNITY BASIC - Capture Cubemap Images during playmode
        else
        {
            // Create and assign instance of a new Manager
            AddManager();
            manager = FindObjectOfType(typeof(Cubemapper)) as Cubemapper;

            // Configure Manager
            manager.pathCubemaps = cm_pathCubemaps;
            manager.pathCubemapPNG = cm_pathCubemapPNG;
            manager.makePNG = cm_makePNG;
            manager.useLinearSpace = cm_useLinearSpace;
            manager.useMipMaps = cm_useMipMaps;
            manager.resolution = cm_resolution;
            manager.cullingMask = cm_cullingMask;
            manager.farClipPlane = cm_farClipPlane;
            manager.camClearFlags = cm_camClearFlags;
            manager.camBGColor = cm_camBGColor;
            manager.mipMapBias = cm_mipmapBias;
            manager.smoothEdges = cm_smoothEdges;
            manager.smoothEdgesWidth = cm_smoothEdgeWidth;
            manager.generate = true;

            // Start game and inform this window about the generation
            generationInProgress = true;
            EditorApplication.isPlaying = true;
        }
	}
	
	// Track Progress
	void Update()
	{
		// Create Cubemap Manager once playing
		if(EditorApplication.isPlaying && !EditorApplication.isPaused && generationInProgress)
		{			
			manager = FindObjectOfType(typeof(Cubemapper)) as Cubemapper;
			
			if(manager != null && manager.completedTakingScreenshots)
			{
				EditorApplication.isPlaying = false;
				
				if(EditorUtility.DisplayDialog("Cubemaps generated!",
					"You can now proceed assigning your cubemaps to your objects.",
					"Yay!"))
				{
					generationInProgress = false;
					EditorApplication.isPlaying = false;
				}
			}
		}
	}
	
	void MakeNode()
	{
		// Find Cubemap Container, create one if there is none
		CubemapNodeContainer container = FindObjectOfType(typeof(CubemapNodeContainer)) as CubemapNodeContainer;
		if(container == null || !container)
		{
			GameObject newContainer = new GameObject("Cubemap Nodes");
			newContainer.AddComponent<CubemapNodeContainer>();
			container = newContainer.GetComponent<CubemapNodeContainer>();
		}
		
		GameObject go = new GameObject( "Cubemap Node " + ( CountNodes() + 1 ).ToString() );
		go.AddComponent<CubemapNode>();
		go.transform.parent = container.transform;
		
		// Position in center of screen if desired by User
		if(centerNodesOnScreen)
		{
			if(SceneView.currentDrawingSceneView.camera != null)
			{
				Vector3 centerScreen = SceneView.currentDrawingSceneView.camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, nodeDistanceToCam));
		        go.transform.position = centerScreen;
			}
		}
		
		// Select our new Node
		Selection.activeObject = go;
		
		Debug.Log("CUBEMAP NODE CREATED!");
	}
	
	
	void MakeTransformsToUser()
	{
		Undo.RegisterSceneUndo("Adding Cubemap User Component to Object(s)");
		
		foreach(Transform t in Selection.transforms)
		{
			t.gameObject.AddComponent<CubemapUser>();
		}
	}
	
	void AssignCubemaps()
	{
		Undo.RegisterSceneUndo("Assigning Cubemaps");
		
		CubemapNode[] nodes = FindObjectsOfType( typeof(CubemapNode) ) as CubemapNode[];
		CubemapUser[] users = FindObjectsOfType( typeof(CubemapUser) ) as CubemapUser[];
		
		//=======================================//
		// ASSIGN CUBEMAP TO NODES VARIABLE
		//=======================================//
		foreach(CubemapNode node in nodes)
		{
			// Let's find and assign the corresponding Cubemaps if no Cubemap is assigned yet and as long as we explicitly allow assignment
			if(node.cubemap == null || node.allowAssign)
			{
				// Find Cubemap File
				string sceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
				string filePath = FindCubemap("Assets/", "*.cubemap", sceneName + " - " + node.name + ".cubemap" );
				string errorNoPathMsg = "Could not find Cubemap for Node: " + node.name + ". Are you sure the file exists? The expected path was: " + filePath;
				
				Cubemap c = AssetDatabase.LoadAssetAtPath(filePath, typeof(Cubemap)) as Cubemap;
				
				// Don't proceed if there is no file found at path
				if(filePath == null || c == null) Debug.LogError(errorNoPathMsg);
				else {
					node.cubemap = c;
					Debug.Log("Assigned Cubemap successfully to Node " + node.name);
				}
			}
			//else Debug.LogWarning(node.name + " already has a Cubemap assigned, skipping assignment. Set Cubemap to None on this Node if this is not what you want.");
		}
		
		//=======================================//
		// ASSIGN NEAREST NODE'S CUBEMAP TO USER
		//=======================================//
		foreach(CubemapUser user in users)
		{
			CubemapNode nearestNode = null;
			float minDist = Mathf.Infinity;
			Vector3 pos = user.transform.position;		
			
			// Find nearest node
			for(int i=0; i<nodes.Length; i++)
			{
				float dist = Vector3.Distance(nodes[i].gameObject.transform.position, pos);
				
				if(dist < minDist)
				{
					nearestNode = nodes[i];
					minDist = dist;
					//Debug.Log("New nearest: " + nearestNode.name + "(Distance: " + nearestDistance + ")");
				}
			}
			
			// Assign Cubemap if we can
			if(nearestNode.cubemap == null) Debug.LogError("Trying to access non-existant Cubemap of Node " + nearestNode.name + " by User " + user.name);
			else {
				// Does this user have at least one material somewhere that supports cubemaps?
				if(VerifyCubemapSupport(user.gameObject))
				{
					Renderer[] renderers = user.gameObject.GetComponentsInChildren<Renderer>();
					
					if(renderers != null)
					{
						List<Material> materialsToEdit = new List<Material>();
						
					    foreach(Renderer r in renderers)
					    {
					         foreach(Material m in r.sharedMaterials)
					         {
								if(m.HasProperty("_Cube"))
									materialsToEdit.Add(m);
					         }
					    }
						
						Undo.RegisterUndo(materialsToEdit.ToArray(), "Assigning Cubemaps"); 
						
						// Change Materials
						foreach(Material m in materialsToEdit)
							m.SetTexture("_Cube", nearestNode.cubemap);
					}
					
					//user.renderer.sharedMaterial.SetTexture("_Cube", nearestNode.cubemap);
					Debug.Log("Assigned Cubemap successfully to \"" + user.name + "\" Shader!");
				}
				// no cubemap support anywhere on this object
				else Debug.LogError(user.gameObject.name + " is a Cubemap User, but no Materials have a _Cube property for attaching Cubemaps!");
			}
		}
	}
	
	void AddManager()
	{
		GameObject go = new GameObject("Cubemap Manager (temporary for Generation)");
		go.AddComponent<Cubemapper>();
	}
	
	/// <summary>
	/// Destroys all manager instances found in scene upon being called
	/// </summary>
	void DestroyManager()
	{
		Cubemapper[] managers = FindObjectsOfType(typeof(Cubemapper)) as Cubemapper[];
		
		foreach(Cubemapper obj in managers)
		{
			DestroyImmediate(obj.gameObject);
		}
	}
	#endregion

	
	
	#region Integrity Checks and Helpers
	void LoadImages()
	{
		// Path to current scripts location
		MonoScript script = MonoScript.FromScriptableObject(this);
		string assetPath = AssetDatabase.GetAssetPath(script);
		
		// Get Root path		
		DirectoryInfo dirInfo = new DirectoryInfo(assetPath);
		dirInfo = dirInfo.Parent.Parent; // 2 folders up from here
		
		// Split the path and reassemble into something Unity can read
		string searchPath = CubemapHelpers.MakeUnityPath(dirInfo.FullName, true);
		searchPath += "/Images/"; // Append our Image path (no need for useless searches in the other directories)

		string[] files = Directory.GetFiles(searchPath, "*.png", SearchOption.AllDirectories);
		
		if(files != null)
		{
			foreach(string file in files)
			{
				Object obj = AssetDatabase.LoadAssetAtPath(file, typeof(Object)) as Object;				
				Texture asset = obj as Texture;
				
				if(asset != null)
					windowImages.Add(asset);
			}
		}
	}
	
	Texture GetImage(string imageName)
	{
		Texture image = null;
		
		foreach(Texture t in windowImages)
		{
			if(Path.GetFileNameWithoutExtension(t.name) == imageName)
				image = t;
		}
		
		return (image != null) ? image : null;
	}
	
	// Looks through a directory for the file and returns the path when found
	string FindCubemap(string path, string extension, string targetFileName)
	{
		string[] files = Directory.GetFiles(path, extension, SearchOption.AllDirectories);
		
		string newFile = null;
		
		foreach(string file in files)
		{
			// Get only the Filename and see if this is the file we want
			string filename = Path.GetFileName(file);
			if(filename == targetFileName)
			{
				//Debug.Log("Match found! " + file);
				newFile = file;
			}
		}
		
		return newFile;
	}
	
	/// <summary>
	/// Verifies that at least one material has the "_Cube" property
	/// </summary>
	/// <returns>
	/// The cubemap support.
	/// </returns>
	/// <param name='obj'>
	/// The object to check
	/// </param>
	bool VerifyCubemapSupport(GameObject obj)
	{
		Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
		bool hasCubemapSupport = false;
		
		if(renderers != null)
		{
		    // Iterate through all renderers
		    foreach(Renderer r in renderers)
		    {
		         foreach(Material mat in r.sharedMaterials)
		         {
					if(mat.HasProperty("_Cube")) hasCubemapSupport = true;
		         }
		    }
		} else hasCubemapSupport = false;
		
		return hasCubemapSupport;
	}
	
	int CountNodeContainers()
	{
		CubemapNodeContainer[] containers = FindObjectsOfType( typeof(CubemapNodeContainer) ) as CubemapNodeContainer[];
		return containers.Length;
	}
		
	int CountNodes()
	{
		CubemapNode[] nodes = FindObjectsOfType( typeof(CubemapNode) ) as CubemapNode[];
		return nodes.Length;
	}
	
	int CountUsers()
	{
		CubemapUser[] users = FindObjectsOfType( typeof(CubemapUser) ) as CubemapUser[];
		return users.Length;
	}
	
	bool NodesHaveCubemaps()
	{
		CubemapNode[] nodes = FindObjectsOfType( typeof(CubemapNode) ) as CubemapNode[];
		
		if(nodes.Length == 0) return false;
		else {
			// Check our Nodes
			foreach(CubemapNode node in nodes)
			{
				// Abort and return false if we find one that doesn't have a Cubemap yet
				if(node.cubemap == null) return false;
			}
			
			// If we get here, we can assume every node had a cubemap
			return true;
		}
	}
	
	void ListNodesWithoutCubemaps()
	{
		CubemapNode[] nodes = FindObjectsOfType( typeof(CubemapNode) ) as CubemapNode[];
		
		if(nodes.Length == 0) Debug.LogError("There are no Nodes found in the Scene!");
		else {
			foreach(CubemapNode node in nodes)
			{
				if(node.cubemap == null) Debug.LogError("Node \"" + node.name + "\" has no Cubemap assigned!");
			}
		}
	}
	#endregion
	
	#region Manage Editor Prefs
	/// <summary>
	/// Saves the Cubemapper Editor Preferences.
	/// </summary>
	void SavePrefs()
	{
		EditorPrefs.SetFloat(regPrefix + "FarClipPlane", cm_farClipPlane);
        EditorPrefs.SetInt(regPrefix + "CullingMask", cm_cullingMask);
        EditorPrefs.SetInt(regPrefix + "CamClearFlag", m_camClearFlagSelected);
        EditorPrefs.SetFloat(regPrefix + "CamBGColor_R", cm_camBGColor.r);
        EditorPrefs.SetFloat(regPrefix + "CamBGColor_G", cm_camBGColor.g);
        EditorPrefs.SetFloat(regPrefix + "CamBGColor_B", cm_camBGColor.b);
        EditorPrefs.SetFloat(regPrefix + "CamBGColor_A", cm_camBGColor.a);
		EditorPrefs.SetBool(regPrefix + "MakePNG", cm_makePNG);
        EditorPrefs.SetBool(regPrefix + "SmoothEdges", cm_smoothEdges);
        EditorPrefs.SetInt(regPrefix + "SmoothEdgeWidth", cm_smoothEdgeWidth);
        EditorPrefs.SetFloat(regPrefix + "MipMapBias", cm_mipmapBias);
		EditorPrefs.SetInt(regPrefix + "Resolution", cm_resolution);
		EditorPrefs.SetBool(regPrefix + "UseMipMaps", cm_useMipMaps);
        EditorPrefs.SetBool(regPrefix + "UseLinearSpace", cm_useLinearSpace);
		EditorPrefs.SetString(regPrefix + "OutputPathCubemaps", cm_pathCubemaps);
		EditorPrefs.SetString(regPrefix + "OutputPathPNG", cm_pathCubemapPNG);
		EditorPrefs.SetString(regPrefix + "CurrentMenu", currentMenu);
		EditorPrefs.SetBool(regPrefix + "CenterNewNodes", centerNodesOnScreen);
		EditorPrefs.SetFloat(regPrefix + "NodeDistanceToCam", nodeDistanceToCam);
        EditorPrefs.SetBool(regPrefix + "ShowAdvancedGenerateSettings", showAdvancedSettings);
	}
	
	/// <summary>
	/// Loads the Cubemapper Editor Preferences
	/// </summary>
	void LoadPrefs()
	{
		if(EditorPrefs.HasKey(regPrefix + "FarClipPlane"))
			cm_farClipPlane = EditorPrefs.GetFloat(regPrefix + "FarClipPlane");

        if(EditorPrefs.HasKey(regPrefix + "CullingMask"))
            cm_cullingMask = EditorPrefs.GetInt(regPrefix + "CullingMask");

        if (EditorPrefs.HasKey(regPrefix + "CamClearFlag"))
            m_camClearFlagSelected = EditorPrefs.GetInt(regPrefix + "CamClearFlag");

        if(EditorPrefs.HasKey(regPrefix + "CamBGColor_R") && EditorPrefs.HasKey(regPrefix + "CamBGColor_G") && EditorPrefs.HasKey(regPrefix + "CamBGColor_B") && EditorPrefs.HasKey(regPrefix + "CamBGColor_A"))
            cm_camBGColor = new Color( EditorPrefs.GetFloat(regPrefix + "CamBGColor_R"), EditorPrefs.GetFloat(regPrefix + "CamBGColor_G"), EditorPrefs.GetFloat(regPrefix + "CamBGColor_B"), EditorPrefs.GetFloat(regPrefix + "CamBGColor_A") );

		if(EditorPrefs.HasKey(regPrefix + "MakePNG"))
			cm_makePNG		= EditorPrefs.GetBool(regPrefix + "MakePNG");

        if (EditorPrefs.HasKey(regPrefix + "SmoothEdges"))
            cm_smoothEdges = EditorPrefs.GetBool(regPrefix + "SmoothEdges");

        if (EditorPrefs.HasKey(regPrefix + "SmoothEdgeWidth"))
            cm_smoothEdgeWidth = EditorPrefs.GetInt(regPrefix + "SmoothEdgeWidth");

        if (EditorPrefs.HasKey(regPrefix + "MipMapBias"))
            cm_mipmapBias = EditorPrefs.GetFloat(regPrefix + "MipMapBias");

		if(EditorPrefs.HasKey(regPrefix + "Resolution"))
			cm_resolution	= EditorPrefs.GetInt(regPrefix + "Resolution");
		
		if(EditorPrefs.HasKey(regPrefix + "UseMipMaps"))
			cm_useMipMaps	= EditorPrefs.GetBool(regPrefix + "UseMipMaps");

        if (EditorPrefs.HasKey(regPrefix + "UseLinearSpace"))
            cm_useLinearSpace = EditorPrefs.GetBool(regPrefix + "UseLinearSpace");
		
		if(EditorPrefs.HasKey(regPrefix + "OutputPathCubemaps"))
			cm_pathCubemaps = EditorPrefs.GetString(regPrefix + "OutputPathCubemaps");
		
		if(EditorPrefs.HasKey(regPrefix + "OutputPathPNG"))
			cm_pathCubemapPNG = EditorPrefs.GetString(regPrefix + "OutputPathPNG");
		
		if(EditorPrefs.HasKey(regPrefix + "CurrentMenu"))
			currentMenu		= EditorPrefs.GetString(regPrefix + "CurrentMenu");
		
		if(EditorPrefs.HasKey(regPrefix + "CenterNewNodes"))
			centerNodesOnScreen = EditorPrefs.GetBool(regPrefix + "CenterNewNodes");
			
		if(EditorPrefs.HasKey(regPrefix + "NodeDistanceToCam"))
			nodeDistanceToCam = EditorPrefs.GetFloat(regPrefix + "NodeDistanceToCam");

        if(EditorPrefs.HasKey(regPrefix + "ShowAdvancedGenerateSettings"))
            showAdvancedSettings = EditorPrefs.GetBool(regPrefix + "ShowAdvancedGenerateSettings");
	}
	#endregion
}