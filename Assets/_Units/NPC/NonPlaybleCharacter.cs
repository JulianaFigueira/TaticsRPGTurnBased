using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Unit))]
public class NonPlaybleCharacter : MonoBehaviour, ICharacterAgent
{
    CharacterAgent characterAgent;

    private void Start()
    {
        characterAgent = new CharacterAgent(GetComponentInChildren<UnityEngine.AI.NavMeshAgent>(), GetComponent<Unit>());
    }

    private void Update()
    {
        CheckRoundState();
    }

    public void CheckRoundState()
    {
        characterAgent.CheckRoundState();
    }

    public void SetTarget(Vector3 newPosition, Unit target)
    {
        characterAgent.SetTarget(newPosition, target);
    }

    public void Attack()
    {
        characterAgent.Attack();
    }
}
