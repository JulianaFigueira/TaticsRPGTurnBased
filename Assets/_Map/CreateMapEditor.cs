using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateMapScript))]
public class CreateMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateMapScript script = (CreateMapScript)target;
        if (GUILayout.Button("Generate Map"))
        {
            script.Generate();
        }
        if (GUILayout.Button("Delete Map"))
        {
            script.Delete();
        }
    }
}