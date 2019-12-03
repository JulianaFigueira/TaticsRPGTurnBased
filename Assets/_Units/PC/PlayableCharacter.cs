using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitMove))]
public class PlayableCharacter : MonoBehaviour {

    Camera cam;
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public UnitMove characterMove { get; private set; } // the movements we are controlling

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        cam = Camera.main;
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        characterMove = GetComponent<UnitMove>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Ground")
                    agent.SetDestination(hit.collider.gameObject.transform.position);
            }
        }

        if (agent.remainingDistance > agent.stoppingDistance)
            characterMove.Move(true, false);
        else
            characterMove.Move(false, false);
    }

    public void Attack()
    {
        characterMove.Move(false, true);
    }
}
