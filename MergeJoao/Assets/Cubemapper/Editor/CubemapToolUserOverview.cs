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

public class CubemapToolUserOverview : EditorWindow
{	
	Vector2 scrollPos;	
	CubemapUser[] users;
	
	private void OnGUI()
	{		
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		
		if(CountUsers() > 0)
		{
			EditorGUILayout.BeginVertical( GUILayout.Width(300) );
			
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Cubemap Users are created in the \"Assign\" Menu. Please refer to the Manual for more information about One-Click Assignments.", MessageType.Info);
			
			GUILayout.Label("Overview of Cubemap Users in Scene", EditorStyles.boldLabel);
			
			for(int i=0; i<users.Length; i++)
			{
				CubemapHelpers.Separator(5f);
				
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.BeginVertical( GUILayout.Width(220) );
				GUILayout.Label(users[i].name, GUILayout.Width(220), GUILayout.ExpandWidth(false));
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
				if(GUILayout.Button("Select", GUILayout.Width(70)))
				{
					Selection.activeGameObject = users[i].gameObject;
				}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.EndVertical();
		}
		else
			EditorGUILayout.HelpBox("No Cubemap Users found. Please use the Menu \"Assign\" to assign Cubemap Users. Refer to the Manual for more Information.", MessageType.Warning);
		
		EditorGUILayout.EndScrollView();
		
		Repaint();
	}
	
	
	int CountUsers()
	{
		users = FindObjectsOfType( typeof(CubemapUser) ) as CubemapUser[];
		return users.Length;
	}
}
