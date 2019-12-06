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
        characterAgent = new CharacterAgent(GetComponent<UnityEngine.AI.NavMeshAgent>(), GetComponent<Unit>());
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 10.0f, true);
            if (Physics.Raycast(ray, out hit))
            {
                float distance = Vector3.Distance(hit.point, character.transform.position);
                switch (character.roundStage)
                {
                    case Unit.RoundStage.ChoseMove:
                        if (distance <= character.Speed && hit.collider.gameObject.tag == "Ground")
                        {
                            SetTarget(hit.point, null);
                        }
                    break;
                    case Unit.RoundStage.ChoseAttack:
                        if (distance <= character.Range && (hit.collider.gameObject.tag == "AI" || hit.collider.gameObject.tag == "Player"))
                        {
                            Unit target = hit.collider.gameObject.GetComponent<Unit>();
                            SetTarget(hit.point, target);
                            Attack();
                        }
                    break;
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
