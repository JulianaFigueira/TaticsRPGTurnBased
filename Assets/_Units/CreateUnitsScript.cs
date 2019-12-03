using System.Collections.Generic;
using UnityEngine;

public class CreateUnitsScript : MonoBehaviour {

    public GameObject[] Units;
    public GameObject[] PCTypes; //must be unit
    public GameObject[] NPCTypes;
    public GameObject TileMap;
    public Vector3 PositionOffset;
    private List<GameObject> TilePositions;

    public void Generate()
    {
        int team = 4;
        int enemies = Random.Range(4, 8);

        Units = new GameObject[team + enemies];

        TilePositions = new List<GameObject>();
        foreach (Transform child in TileMap.transform)
        {
            if (child.tag == "Ground")
            {
                TilePositions.Add(child.gameObject);
            }
        }

        // 3 musketeers + a random
        Units[0] = Instantiate<GameObject>(PCTypes[0], TilePositions[0].transform.position + PositionOffset, Quaternion.identity, this.transform);
        Units[1] = Instantiate<GameObject>(PCTypes[1], TilePositions[1].transform.position + PositionOffset, Quaternion.identity, this.transform);
        Units[2] = Instantiate<GameObject>(PCTypes[2], TilePositions[2].transform.position + PositionOffset, Quaternion.identity, this.transform);
        Units[3] = Instantiate<GameObject>(PCTypes[Random.Range(0, PCTypes.Length)], TilePositions[3].transform.position + PositionOffset, Quaternion.identity, this.transform);

        //random enemies
        int j = 0;
        for (int i = TilePositions.Count - 1; i >= TilePositions.Count - enemies; i--)
        {
            Units[team + j++] = Instantiate<GameObject>(NPCTypes[Random.Range(0, NPCTypes.Length)], TilePositions[i].transform.position + PositionOffset, Quaternion.identity, this.transform);
        }

    }

    internal void Delete()
    {
        for (int i = 0; i < Units.Length; i++)
        {
            DestroyImmediate(Units[i]);
        }
    }
}
