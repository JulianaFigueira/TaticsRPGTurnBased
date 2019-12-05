using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Unit))]
public class PlayableCharacter : MonoBehaviour, ICharacterAgent
{
    Camera cam;
    CharacterAgent characterAgent;

    private void Start()
    {
        cam = Camera.main;
        characterAgent = new CharacterAgent(GetComponentInChildren<UnityEngine.AI.NavMeshAgent>(), GetComponent<Unit>());
    }

    private void Update()
    {
        Unit character = characterAgent.Character;

        if (character.roundStage == Unit.RoundStage.ChoseMove || character.roundStage == Unit.RoundStage.ChoseAttack)
        {
            CheckPlayerInput();
        }

        CheckRoundState();
    }

    private void CheckPlayerInput()
    {
        Unit character = characterAgent.Character;

        if (character.roundStage == Unit.RoundStage.ChoseMove && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //check if valid position
                float distance = Vector3.Distance(hit.point, character.transform.position);
                if (distance <= character.Speed && hit.collider.gameObject.tag == "Ground")
                {
                    SetTarget(hit.point, null);
                }
            }
        }
        else if (character.roundStage == Unit.RoundStage.ChoseAttack && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                float distance = Vector3.Distance(hit.point, character.transform.position);
                if (distance <= character.Range && (hit.collider.gameObject.tag == "AI" || hit.collider.gameObject.tag == "Player"))
                {
                    Unit target = hit.collider.gameObject.GetComponent<Unit>();
                    SetTarget(hit.point, target);
                    Attack();
                }
            }
        }
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
