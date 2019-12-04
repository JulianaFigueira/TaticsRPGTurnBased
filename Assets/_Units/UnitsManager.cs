using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour {

    private List<GameObject> Units;
    private List<Unit> UnitsFighters; //samme list as above, but easier to access Unit component

    public GameObject[] PCTypes; 
    public GameObject[] NPCTypes; 

    public Vector3 PositionOffset;

    public GameObject TileMap;
    private List<GameObject> TilePositions; //easy to access tiles

    private TaticsManager stateMachine;

    // Use this for initialization
    void Start()
    { 
        stateMachine = GetComponentInParent<TaticsManager>();
    }

    public void Generate()
    {
        CreateUnits();
        GenerateAttackOrder();
    }

    private void CreateUnits()
    {
        int team = 4;
        int enemies = Random.Range(4, 8);

        Units = new List<GameObject>(team + enemies);

        TilePositions = new List<GameObject>();
        foreach (Transform child in TileMap.transform)
        {
            if (child.tag == "Ground")
            {
                TilePositions.Add(child.gameObject);
            }
        }

        // one of each + a random
        Units.Add(Instantiate<GameObject>(PCTypes[0], TilePositions[0].transform.position + PositionOffset, Quaternion.identity, this.transform));
        Units.Add(Instantiate<GameObject>(PCTypes[1], TilePositions[1].transform.position + PositionOffset, Quaternion.identity, this.transform));
        Units.Add(Instantiate<GameObject>(PCTypes[2], TilePositions[2].transform.position + PositionOffset, Quaternion.identity, this.transform));
        Units.Add(Instantiate<GameObject>(PCTypes[Random.Range(0, PCTypes.Length)], TilePositions[3].transform.position + PositionOffset, Quaternion.identity, this.transform));

        //random enemies on the other side of the map
        for (int i = TilePositions.Count - 1; i >= TilePositions.Count - enemies; i--)
        {
            Units.Add(Instantiate<GameObject>(NPCTypes[Random.Range(0, NPCTypes.Length)], TilePositions[i].transform.position + PositionOffset, Quaternion.identity, this.transform));
        }
    }

    internal void StopGame()
    {
        //throw new NotImplementedException();
    }

    internal void ChangeCurrentPlayer()
    {
       // throw new NotImplementedException();
    }

    internal TaticsManager.GameState CheckGameState()
    {
        // throw new NotImplementedException();
        return TaticsManager.GameState.Lose;
    }

    internal bool CheckCurrentRound()
    {
        // throw new NotImplementedException();
        return false;
    }

    internal bool PrepareGame()
    {
        // throw new NotImplementedException();
        return false;
    }

    //Lottery
    void GenerateAttackOrder()
    {
        List<int> numbers = new List<int>(Units.Count);
        UnitsFighters = new List<Unit>(Units.Count);

        for (int i = 0; i < Units.Count; i++)
        {
            numbers.Add(i);
        }

        for (int i = 0; i < Units.Count; i++)
        {
            UnitsFighters.Add(Units[i].GetComponent<Unit>());
            UnitsFighters[i].AttackOrder = numbers[Random.Range(0, numbers.Count)];
            numbers.Remove(UnitsFighters[i].AttackOrder);
        }
    }

    internal void Delete()
    {
        for (int i = 0; i < Units.Count; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(Units[i]);
#else
            Destroy(Units[i]);
#endif

        }
    }
}
