using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
        CheckPlayerInput();

        CheckRoundState();
    }

    private void CheckPlayerInput()
    {
        Unit character = characterAgent.Character;

        if (character.roundStage == Unit.RoundStage.Move && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //check if valid position
                float distance = Vector3.Distance(hit.collider.transform.position, character.transform.position);
                if (distance <= character.Speed && hit.collider.gameObject.tag == "Ground")
                {
                    SetTarget(hit.point, null);
                }
            }
        }
        else if (character.roundStage == Unit.RoundStage.Attack && Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                float distance = Vector3.Distance(hit.collider.transform.position, character.transform.position);
                if (distance <= character.Range && (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Ally"))
                {
                    Unit target = hit.collider.gameObject.GetComponent<Unit>();
                    SetTarget(hit.point, target);
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
