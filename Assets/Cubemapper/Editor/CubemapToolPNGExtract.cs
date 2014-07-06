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

public class CubemapToolPNGExtract : EditorWindow
{
	public string outputPath;
	
	private Vector2 scrollPos;
	private Cubemap sourceCubemap;

    // Face Toggles
    private bool includePositiveX = true;
    private bool includePositiveY = true;
    private bool includePositiveZ = true;
    private bool includeNegativeX = true;
    private bool includeNegativeY = true;
    private bool includeNegativeZ = true;

	private void OnGUI()
	{		
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		if(string.IsNullOrEmpty(outputPath))
		{
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Oh no! Something went wrong while determining the Ouput Folder for PNG files. Please check the Cubemapper Settings and contact the developer if this error persists.", MessageType.Error);
			
			GUILayout.Label("PNG Output Folder", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal( GUILayout.Width(250) );
			EditorGUILayout.HelpBox("All Paths should point to a folder inside your projects \"Assets\" folder.", MessageType.Info);
			EditorGUILayout.EndHorizontal();
			
			OutputPathPreview();
		}
		else
		{
			GUILayout.Label("1. Define Cubemap", EditorStyles.boldLabel);			
			sourceCubemap = EditorGUILayout.ObjectField(sourceCubemap, typeof(Cubemap), false, GUILayout.Height(70), GUILayout.Width(70)) as Cubemap;
			
			if(sourceCubemap != null)
			{
				GUILayout.Label(sourceCubemap.name);

                GUILayout.Space(15);

                GUILayout.Label("2. Which Faces should be extracted?", EditorStyles.boldLabel);

                // Select/Deselect Toggle Buttons (Vertical Split)
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Select All", GUILayout.Width(90), GUILayout.Height(25)))
                    includePositiveX = includePositiveY = includePositiveZ = includeNegativeX = includeNegativeY = includeNegativeZ = true;

                if (GUILayout.Button("Select None", GUILayout.Width(90), GUILayout.Height(25)))
                    includePositiveX = includePositiveY = includePositiveZ = includeNegativeX = includeNegativeY = includeNegativeZ = false;                
                EditorGUILayout.EndHorizontal();
                
                // BEGIN Toggles Splitview
                EditorGUILayout.BeginHorizontal();

                // BEGIN Left Toggles
                EditorGUILayout.BeginVertical(GUILayout.Width(100));

                // +X Toggle
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("+X", GUILayout.MaxWidth(30));
                includePositiveX = EditorGUILayout.Toggle(includePositiveX);
                EditorGUILayout.EndHorizontal();

                // +Y Toggle
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("+Y", GUILayout.MaxWidth(30));
                includePositiveY = EditorGUILayout.Toggle(includePositiveY);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("+Z", GUILayout.MaxWidth(30));
                includePositiveZ = EditorGUILayout.Toggle(includePositiveZ);
                EditorGUILayout.EndHorizontal();

                // END Left Toggles
                EditorGUILayout.EndVertical();

                // BEGIN Right Toggles
                EditorGUILayout.BeginVertical(GUILayout.Width(100));

                // -X Toggle
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("-X", GUILayout.MaxWidth(30));
                includeNegativeX = EditorGUILayout.Toggle(includeNegativeX);
                EditorGUILayout.EndHorizontal();

                // -Y Toggle
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("-Y", GUILayout.MaxWidth(30));
                includeNegativeY = EditorGUILayout.Toggle(includeNegativeY);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("-Z", GUILayout.MaxWidth(30));
                includeNegativeZ = EditorGUILayout.Toggle(includeNegativeZ);
                EditorGUILayout.EndHorizontal();

                // END Right Toggles
                EditorGUILayout.EndVertical();

                // END Toggles Splitview
                EditorGUILayout.EndHorizontal();

                // Don't proceed if no Toggle selected
                if (includePositiveX || includePositiveY || includePositiveZ || includeNegativeX || includeNegativeY || includeNegativeZ)
                {
                    GUILayout.Space(15);

                    GUILayout.Label("3. PNG Output Folder", EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal(GUILayout.Width(250));
                    EditorGUILayout.HelpBox("All Paths should point to a folder inside your projects \"Assets\" folder.", MessageType.Info);
                    EditorGUILayout.EndHorizontal();

                    OutputPathPreview();

                    EditorGUILayout.Space();

                    if (!string.IsNullOrEmpty(outputPath))
                    {
                        GUILayout.Label("4. Click Button to extract PNG", EditorStyles.boldLabel);

                        GUI.backgroundColor = new Color(0.43f, 0.78f, 1f, 1f);
                        if (GUILayout.Button("Extract PNG from Cubemap", GUILayout.Width(200), GUILayout.Height(40)))
                        {
                            Debug.Log("Extracting PNG from Cubemap...");
                            CubemapHelpers.CubemapToPNG(sourceCubemap, outputPath, includePositiveX, includeNegativeX, includePositiveY, includeNegativeY, includePositiveZ, includeNegativeZ);
                            Debug.Log("Extraction complete!");
                        }
                        GUI.backgroundColor = Color.white;
                    }
                }
			}
		}
		
		EditorGUILayout.EndScrollView();
		
		Repaint();
	}
			
	private void OutputPathPreview()
	{		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Path: Assets/" + outputPath);
		EditorGUILayout.EndHorizontal();
		
		GUILayout.Label(CubemapHelpers.VerifyPath(outputPath));
		
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.BeginVertical( GUILayout.MaxWidth(120) );
		if(GUILayout.Button("Default Path", GUILayout.MaxWidth(120), GUILayout.Height(30)))
		{
			if(EditorPrefs.HasKey("cubemapper_OutputPathPNG"))
				outputPath = EditorPrefs.GetString("cubemapper_OutputPathPNG");
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical( GUILayout.MaxWidth(120) );
		
		if(GUILayout.Button("Choose Path...", GUILayout.MaxWidth(120), GUILayout.Height(30)))
		{
			string newCubemapPNGPath = EditorUtility.OpenFolderPanel("Specify Output Folder for PNG Files", "", "");
			string sanitizedCubemapPNGPath = CubemapHelpers.MakeUnityPath(newCubemapPNGPath, false);
			
			// Path seems ok, proceed assigning to variable
			if(!string.IsNullOrEmpty(sanitizedCubemapPNGPath))
			{
				outputPath = sanitizedCubemapPNGPath;
				EditorPrefs.SetString("cubemapper_PNGToolOutputPath", sanitizedCubemapPNGPath);
				//Debug.Log("Sanitized Path: " + sanitizedCubemapPNGPath + " | Raw Path: " + newCubemapPNGPath);
			}
			// Something went wrong, show error
			else
				EditorUtility.DisplayDialog("Path NOT changed", "Path was not changed because selection of the new Output folder was either aborted or invalid. Please try again or select a different folder.", "Okay");
		}
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.EndHorizontal();
	}
}