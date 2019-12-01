using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateMapScript))]
public class CreateMapEditor : Editor
{

    public GameObject[] TileTypes;
    public float NumberSides;
    public float CenterDistance;
    public float NumberTiles;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateMapScript script = (CreateMapScript)target;
        if (GUILayout.Button("Generate Map"))
        {
            script.Generate();
        }
    }
}