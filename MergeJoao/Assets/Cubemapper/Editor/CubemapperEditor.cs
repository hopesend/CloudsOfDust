using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Cubemapper))]
public class CubemapperEditor : Editor
{	
	public override void OnInspectorGUI()
	{		
		EditorGUILayout.HelpBox("The Cubemapper has been simplified into a dockable Window. You can open it by clicking on \"Window\" -> \"Cubemapper\"", MessageType.Warning);
		
		if(GUILayout.Button("Open Cubemapper Window", GUILayout.Height(40)))
		{
			// Window Set-Up
	        CubemapWindow window = EditorWindow.GetWindow(typeof(CubemapWindow), false, "Cubemapper", true) as CubemapWindow;
			window.Show();
		}
		
		EditorGUILayout.Space();
		
		EditorGUILayout.HelpBox("If you are unsure about how it all works please refer to the Manual.", MessageType.Info);
	}
}
