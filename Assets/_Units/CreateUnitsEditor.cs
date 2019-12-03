using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateUnitsScript))]
public class CreateUnitsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateUnitsScript script = (CreateUnitsScript)target;
        if (GUILayout.Button("Generate Units"))
        {
            script.Generate();
        }

        if (GUILayout.Button("Delete Units"))
        {
            script.Delete();
        }
    }
}