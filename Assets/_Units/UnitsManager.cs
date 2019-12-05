using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour {

    public GameObject[] PCTypes; 
    public GameObject[] NPCTypes; 

    public Vector3 PositionOffset;

    public MapManager TileMap;
    private List<GameObject> TilePositions; //easy to access tiles

    public TaticsManager stateMachine;
    private List<GameObject> Units;
    private List<Unit> UnitsFighters; //samme list as above, but easier to access Unit component

    private int currentOrderIndex;
    private Unit currentUnit;

    // Use this for initialization
    void Start()
    { 
        //stateMachine = GetComponentInParent<TaticsManager>();
    }

    public void Generate()
    {
        CreateUnits();
        GenerateAttackOrder();
    }

    private void CreateUnits()
    {
        int team = 4;
        int enemies = UnityEngine.Random.Range(4, 8);

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
        Units.Add(Instantiate<GameObject>(PCTypes[UnityEngine.Random.Range(0, PCTypes.Length)], TilePositions[3].transform.position + PositionOffset, Quaternion.identity, this.transform));

        //random enemies on the other side of the map
        for (int i = TilePositions.Count - 1; i >= TilePositions.Count - enemies; i--)
        {
            Units.Add(Instantiate<GameObject>(NPCTypes[UnityEngine.Random.Range(0, NPCTypes.Length)], TilePositions[i].transform.position + PositionOffset, Quaternion.identity, this.transform));
        }
    }

    internal void StopGame()
    {
        Delete(); //no units, no game
    }

    internal void ChangeCurrentPlayer()
    {
        if (currentOrderIndex < Units.Count)
            currentOrderIndex++;
        else
            currentOrderIndex = 0;
        SetNextPlayer();
    }

    internal TaticsManager.GameState CheckGameState()
    {
        int enemies = 0;
        int allies = 0;
        TaticsManager.GameState state = TaticsManager.GameState.Play;

        CleanDefeatedUnits();    

        foreach (Unit fighter in UnitsFighters)
        {
            if (fighter.unitType == Unit.UnitType.NonPlayableCharacter)
                enemies++;
            else if (fighter.unitType == Unit.UnitType.PlayableCharacter)
                allies++;
        }

        if (enemies == 0)
            state = TaticsManager.GameState.Win;
        else if (allies == 0)
            state = TaticsManager.GameState.Lose;

        return state;
    }

    private void CleanDefeatedUnits()
    {
        for(int i = UnitsFighters.Count - 1; i >= 0; i--)
        {
            Unit fighter = UnitsFighters[i];
            if (fighter.Health <= 0 || fighter.Status == Unit.SpecialStatus.Dead)
            {
                Units.Remove(fighter.gameObject);
                UnitsFighters.Remove(fighter);
                Destroy(fighter.gameObject);
            }
        }
    }

    internal bool CheckCurrentRoundFinished()
    {
        if (currentUnit.Status == Unit.SpecialStatus.GoAgain)
        {
            SetNextPlayer();
            return false;
        }
        else
            return true;
    }

    internal bool PrepareGame()
    {
        bool res = Units.Count > 0 && Units.Count == UnitsFighters.Count; //units generated, may play

        if (res)
        {
            currentOrderIndex = 0;
            SetNextPlayer();
        }

        return res;
    }

    private void SetNextPlayer()
    {
        foreach(Unit fighter in UnitsFighters)
        {
            if(fighter.AttackOrder == currentOrderIndex)
            {
                currentUnit = fighter;
                if (fighter.Status == Unit.SpecialStatus.OK)
                {
                    fighter.roundStage = Unit.RoundStage.Move;
                    switch (fighter.unitType)
                    {
                        case Unit.UnitType.NonPlayableCharacter:
                            //it has to notify state machine when it finishes
                            fighter.AIRoundBehaviour(UnitsFighters, stateMachine.UpdateStateMachine);
                            break;
                        case Unit.UnitType.PlayableCharacter:
                            //liberate input
                            break;
                    }
                }
                else if (fighter.Status == Unit.SpecialStatus.Stun) //stun effect only lasts one round
                {
                    fighter.Status = Unit.SpecialStatus.OK;
                }

                break;
            }
        }
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
            UnitsFighters[i].AttackOrder = numbers[UnityEngine.Random.Range(0, numbers.Count)];
            numbers.Remove(UnitsFighters[i].AttackOrder);
        }
    }

    internal void Delete()
    {
        for (int i = 0; i < Units.Count; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(Units[i]);
            DestroyImmediate(UnitsFighters[i]);
#else
            Destroy(Units[i]);
            Destroy(UnitsFighters[i]);
#endif

        }
    }
}
