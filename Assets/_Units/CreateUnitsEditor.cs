using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitsManager))]
public class CreateUnitsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UnitsManager script = (UnitsManager)target;
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