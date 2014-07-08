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

public class CubemapToolPropertyModifier : EditorWindow
{
	private Vector2 m_scrollPos;
	private Cubemap m_Cubemap;
    private bool    m_useLinearSpace;
    private bool    m_useMipMaps;
    private float   m_mipmapBias = 0.0f;
#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5
    private bool    m_smoothEdges = false;
    private int     m_smoothEdgeWidth = 1;
#endif
    string[]        m_resolutions = { "32x32", "64x64", "128x128", "256x256", "512x512", "1024x1024", "2048x2048" };
    int[]           m_resSizes = { 32, 64, 128, 256, 512, 1024, 2048 };
    int             m_resolution = 64;
    
	private void OnGUI()
	{		
		m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(325));
            EditorGUILayout.HelpBox("This tool aims to help you when you want to change properties that are not covered by the standard Unity Inspector for Cubemaps, e.g. Mip Map Bias and Unity 4 Smooth Edges. This tool is experimental and should be used with care. I do not take responsibility if something goes wrong or produces undesirable results.", MessageType.Info);
        EditorGUILayout.EndHorizontal();

		GUILayout.Label("1. Define Cubemap", EditorStyles.boldLabel);			
		m_Cubemap = EditorGUILayout.ObjectField(m_Cubemap, typeof(Cubemap), false, GUILayout.Height(70), GUILayout.Width(70)) as Cubemap;	

		if(m_Cubemap != null)
        {
            // Serialization helps determine and set Linear and Mip Map values
            SerializedObject serializedCubemap = new SerializedObject(m_Cubemap);

            // Display Name of the current Cubemap
            GUILayout.Label(m_Cubemap.name);

            GUILayout.Space(15);

            GUILayout.Label("2. Modify Settings", EditorStyles.boldLabel);

            EditorGUIUtility.LookLikeControls(125f);

            // SETTINGS VERTICAL AREA
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(325));
            {
                EditorGUILayout.HelpBox("Some settings require the Cubemap to be rebuilt, which means you need to re-assign it to your objects.", MessageType.Warning);

                // SETTINGS START HERE
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(250));
                    {
                        m_resolution = EditorGUILayout.IntPopup("New Resolution:", m_resolution, m_resolutions, m_resSizes);
                        EditorGUILayout.LabelField("Currently:", m_Cubemap.height.ToString() + "x" + m_Cubemap.width.ToString());
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50));
                    {
                        GUI.backgroundColor = CubemapHelpers.ColorGreen;
                        if (GUILayout.Button("Apply", GUILayout.Width(50), GUILayout.Height(20)))
                        {
                            RemakeCubemap(ref m_Cubemap);
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(250));
                    {
                        m_useLinearSpace = EditorGUILayout.Toggle("Linear Space:", m_useLinearSpace);
                        EditorGUILayout.LabelField("Currently: " + (CubemapHelpers.isLinear(serializedCubemap) ? "Yes":"No"));
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50));
                    {
                        GUI.backgroundColor = CubemapHelpers.ColorGreen;
                        if (GUILayout.Button("Apply", GUILayout.Width(50), GUILayout.Height(20)))
                        {
                            CubemapHelpers.setLinear(ref serializedCubemap, m_useLinearSpace);
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(250));
                    {
                        m_useMipMaps = EditorGUILayout.Toggle("Mip Maps:", m_useMipMaps);
                        EditorGUILayout.LabelField("Currently: " + (CubemapHelpers.usingMipMap(serializedCubemap) ? "Yes" : "No"));
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50));
                    {
                        GUI.backgroundColor = CubemapHelpers.ColorGreen;
                        if (GUILayout.Button("Apply", GUILayout.Width(50), GUILayout.Height(20)))
                        {
                            CubemapHelpers.setMipMap(ref serializedCubemap, m_useMipMaps);
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
                {
                    EditorGUILayout.BeginVertical(GUILayout.Width(250));
                    {
                        m_mipmapBias = EditorGUILayout.Slider("Mip Map Bias:", m_mipmapBias, -10f, 10f);
                        EditorGUILayout.LabelField("Currently:", m_Cubemap.mipMapBias.ToString());
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50f));
                    {
                        GUI.backgroundColor = CubemapHelpers.ColorGreen;
                        if (GUILayout.Button("Apply", GUILayout.Width(50), GUILayout.Height(20)))
                        {
                            m_Cubemap.mipMapBias = m_mipmapBias;
                            m_Cubemap.Apply();
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
            
                EditorGUILayout.Space();

#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5
                m_smoothEdges = EditorGUILayout.Toggle("Smooth Edges?", m_smoothEdges);
                if (m_smoothEdges)
                {
                    EditorGUILayout.HelpBox("CAREFUL: This effect can't be undone!", MessageType.Warning);

                    EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(250));
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
                                    m_smoothEdgeWidth = EditorGUILayout.IntField(m_smoothEdgeWidth, GUILayout.Width(30));
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
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(50f));
                        {
                            GUI.backgroundColor = CubemapHelpers.ColorGreen;
                            if (GUILayout.Button("Apply", GUILayout.Width(50), GUILayout.Height(20)))
                            {
                                m_Cubemap.SmoothEdges(m_smoothEdgeWidth);
                                m_Cubemap.Apply();
                            }
                            GUI.backgroundColor = Color.white;
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
#endif
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            GUI.backgroundColor = CubemapHelpers.ColorGreen;
            if (GUILayout.Button("Apply ALL (use carefully!)", GUILayout.Width(200), GUILayout.Height(40)))
            {
                ApplyChanges(m_Cubemap);
            }
            GUI.backgroundColor = Color.white;
        }
		
		EditorGUILayout.EndScrollView();
		
		Repaint();
	}

    private void ApplyChanges(Cubemap cubemap)
    {
        if (cubemap.height != m_resolution)
            RemakeCubemap(ref cubemap);
        else
        {
            cubemap.mipMapBias = m_mipmapBias;

#if !UNITY_2_6 && !UNITY_2_6_1 && !UNITY_3_0 && !UNITY_3_0_0 && !UNITY_3_1 && !UNITY_3_2 && !UNITY_3_3 && !UNITY_3_4 && !UNITY_3_5
            if (m_smoothEdges)
                cubemap.SmoothEdges(m_smoothEdgeWidth);
#endif

            cubemap.Apply();

            SerializedObject serializedCubemap = new SerializedObject(cubemap);
            CubemapHelpers.setMipMap(ref serializedCubemap, m_useMipMaps);
            CubemapHelpers.setLinear(ref serializedCubemap, m_useLinearSpace);
        }
    }

    private void RemakeCubemap(ref Cubemap originalCubemap)
    {
        Cubemap c = new Cubemap(m_resolution, originalCubemap.format, m_useMipMaps);

        CubemapFace face = CubemapFace.PositiveX;
        for (int i = 0; i < 6; i++)
        {
            switch (i)
            {
                case 0: face = CubemapFace.PositiveX; break;
                case 1: face = CubemapFace.PositiveY; break;
                case 2: face = CubemapFace.PositiveZ; break;
                case 3: face = CubemapFace.NegativeX; break;
                case 4: face = CubemapFace.NegativeY; break;
                case 5: face = CubemapFace.NegativeZ; break;
            }

            CopyCubemapFace(face, originalCubemap, ref c);
        }

        c.mipMapBias = originalCubemap.mipMapBias;
        c.Apply();

        string pathToOriginalCubemap = AssetDatabase.GetAssetPath(originalCubemap);
        AssetDatabase.CreateAsset(c, pathToOriginalCubemap);
        AssetDatabase.Refresh();

        // Assign the new Cubemap again for User convience
        m_Cubemap = c; 

        // Linear Space if wanted
        SerializedObject serializedCubemap = new SerializedObject(m_Cubemap);
        CubemapHelpers.setLinear(ref serializedCubemap, m_useLinearSpace);
    }

    private void CopyCubemapFace(CubemapFace face, Cubemap source, ref Cubemap target)
    {
        //Create the blank texture container
        Texture2D snapshot = new Texture2D(source.width, source.height, source.format, m_useMipMaps, false);
        snapshot.wrapMode = TextureWrapMode.Clamp;

        // Read Face Pixels into the Texture
        snapshot.SetPixels(source.GetPixels(face), 0);

        // Resize to new size
        snapshot = Scale(snapshot, m_resolution, m_resolution);

        // Finally write the contents to the new Cubemap
        target.SetPixels(snapshot.GetPixels(), face, 0);
        target.Apply();
    }

    Texture2D Scale(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color32[] rpixels = result.GetPixels32(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);

        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }

        result.SetPixels32(rpixels, 0);
        result.Apply();

        return result;
    }	
}