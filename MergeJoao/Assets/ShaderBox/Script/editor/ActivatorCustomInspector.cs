using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Activator))]
public class ActivatorCustomInspector : Editor
{
    Activator targetObj;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("This is a Label in a Custom Editor");

        if (GUILayout.Button("ON"))
        {
            targetObj = (Activator)target;
            targetObj.TurnOn();
        }
        if (GUILayout.Button("OFF"))
        {
            targetObj = (Activator)target;
            targetObj.TurnOff();
        }

        // Show default inspector property editor
        DrawDefaultInspector();
    }
}
