using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMapScript : MonoBehaviour {

    public GameObject[] TileTypes;
    public float CenterDistance;
    public int NumberTilesX;
    public int NumberTilesZ;

    GameObject[,] TileMap;

    public Vector3 RotateXZ(Vector3 original, float angle)
    {
        float x = original.x;
        float z = original.z;
        float sn = Mathf.Sin(Mathf.Deg2Rad * angle);
        float cs = Mathf.Cos(Mathf.Deg2Rad * angle);

        float px = x * cs - z * sn;
        float pz = x * sn + z * cs;

        return new Vector3(px, original.y, pz);
    }


    public void Generate()
    {
        TileMap = new GameObject[NumberTilesX, NumberTilesZ];
        Vector3 centerPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 nextDirection = new Vector3(0.0f, 0.0f, CenterDistance);

        for (int i = 0; i < NumberTilesX; i++)
        {
            //make line
            for (int k = 0; k < NumberTilesZ; k++)
            {
                TileMap[i,k] = Instantiate<GameObject>(TileTypes[Random.Range(0, TileTypes.Length)], centerPosition, Quaternion.identity, this.transform);
                centerPosition += nextDirection;
            }

            //prepare next line
            centerPosition = TileMap[i, 0].transform.position - RotateXZ(nextDirection, i%2 == 0 ? 60.0f : 120.0f);
        }
    }
}
