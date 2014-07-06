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

[CustomEditor(typeof(CubemapNode)), CanEditMultipleObjects]
public class CubemapNodeEditor : Editor
{
	#region Variables
	// Overriding Resolution for Cubemaps generated from this Node
	GUIContent[] availableResolutions = new GUIContent[]
	{
		new GUIContent("32x32"),
		new GUIContent("64x64"),
		new GUIContent("128x128"),
		new GUIContent("256x256"),
		new GUIContent("512x512"),
		new GUIContent("1024x1024"),
		new GUIContent("2048x2048")
	};
	int[] resSizes = {32, 64, 128, 256, 512, 1024, 2048};
	
	// NEW Serialized Properties
	private SerializedProperty m_allowGeneratePNG;
	private SerializedProperty m_allowCubemapGeneration;
	private SerializedProperty m_allowAssign;
	private SerializedProperty m_overrideResolution;
	private SerializedProperty m_resolution;
	private SerializedProperty m_cubemap;
	#endregion
	
	private void OnEnable()
	{
		// Find and Load Serialized Properties
		m_allowGeneratePNG		 = serializedObject.FindProperty("allowGeneratePNG");
		m_allowCubemapGeneration = serializedObject.FindProperty("allowCubemapGeneration");
		m_allowAssign			 = serializedObject.FindProperty("allowAssign");
		m_overrideResolution	 = serializedObject.FindProperty("overrideResolution");
		m_resolution			 = serializedObject.FindProperty("resolution");
		m_cubemap				 = serializedObject.FindProperty("cubemap");
	}
	
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		
		EditorGUILayout.HelpBox("If you customize your Cubemaps you will find the settings here can aid you greatly. Please refer to the Manual for more Information.", MessageType.Info);
		
		EditorGUILayout.Space();
		
		// Overriding Settings
		GUILayout.Label("Overriding Options (use with care!)", EditorStyles.boldLabel);
		
		EditorGUILayout.PropertyField(m_allowGeneratePNG, 			new GUIContent("Generate PNGs"));
		EditorGUILayout.PropertyField(m_allowCubemapGeneration, 	new GUIContent("Generate Cubemaps"));
		EditorGUILayout.PropertyField(m_allowAssign,		 		new GUIContent("Allow One-Click Assign"));
		EditorGUILayout.PropertyField(m_overrideResolution,		 	new GUIContent("Override Resolution?"));
		
		if(m_overrideResolution.boolValue)
			EditorGUILayout.IntPopup(m_resolution, availableResolutions, resSizes, new GUIContent("Resolution"));
		
		EditorGUILayout.Space();
		
		// Stored Cubemap used by this Node
		GUILayout.Label("Cubemap assigned to this Node:", EditorStyles.boldLabel);
				
		EditorGUILayout.BeginHorizontal();
		
		EditorGUILayout.BeginVertical(GUILayout.Width(100));
		CubemapHelpers.SerializedCubemapField(m_cubemap, false, new GUILayoutOption[] { GUILayout.Height(100), GUILayout.Width(100) } );
		EditorGUILayout.EndVertical();
	
		if(m_cubemap.objectReferenceValue == null)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.HelpBox("If you customize Cubemaps or want to use realtime-switching between Nodes, you will have to assign a Cubemap here either manually or by letting the Cubemapper do it with the One-Click Assign. Refer to the Manual for more Information.", MessageType.Info);
			EditorGUILayout.EndVertical();
		}
		
		EditorGUILayout.EndHorizontal();
		
		if(m_cubemap.objectReferenceValue != null)
		{
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("\"" + m_cubemap.objectReferenceValue.name + "\"", EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();	
		}
		
		// Save Changes
		serializedObject.ApplyModifiedProperties();
	}
}