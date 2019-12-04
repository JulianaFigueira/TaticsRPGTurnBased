﻿using UnityEngine;
using UnityEngine.AI;
/*
 This class generates a map composed of regular hexagons

    TileTypes: hexagons prefabs, different terrains
    HexagonHeight: hexagon height is equal to the distance of the centers of two hexagons connected by the side, 0.8660246
    NumberTilesX: number of "columns"
    NumberTilesZ: number of "rows"
*/
[RequireComponent(typeof(NavMeshSurface))]
public class MapManager : MonoBehaviour {

    public GameObject[] TileTypes;
    public NavMeshSurface NavMesh;
    public float HexagonHeight;
    public int NumberTilesX;
    public int NumberTilesZ;

    public GameObject[,] TileMap;

    public void Awake()
    {
        NavMesh = GetComponentInChildren<UnityEngine.AI.NavMeshSurface>();
    }

    public void Generate()
    {
        CreateTiles();

        NavMesh.BuildNavMesh();
    }

    private void CreateTiles()
    {
        TileMap = new GameObject[NumberTilesX, NumberTilesZ];
        float centerDistance = HexagonHeight * this.transform.localScale.x; //xz should be uniform

        Vector3 centerPosition = this.transform.position;
        Vector3 nextDirection = new Vector3(0.0f, 0.0f, centerDistance);

        for (int i = 0; i < NumberTilesX; i++)
        {
            //make column
            for (int k = 0; k < NumberTilesZ; k++)
            {
                TileMap[i, k] = Instantiate<GameObject>(TileTypes[Random.Range(0, TileTypes.Length)], centerPosition, Quaternion.identity, this.transform);
                centerPosition += nextDirection;
            }

            //prepare next column
            centerPosition = TileMap[i, 0].transform.position - RotateXZ(nextDirection, i % 2 == 0 ? 60.0f : 120.0f);
        }
    }

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

    internal void Delete()
    {
        for (int i = 0; i < NumberTilesX; i++)
        {
            for (int k = 0; k < NumberTilesZ; k++)
            {
#if UNITY_EDITOR
                DestroyImmediate(TileMap[i, k]);
#else
                 Destroy(TileMap[i, k]);
#endif
            }
        }
    }
}