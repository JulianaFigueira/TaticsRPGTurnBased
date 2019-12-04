using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class CreateMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapManager script = (MapManager)target;
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