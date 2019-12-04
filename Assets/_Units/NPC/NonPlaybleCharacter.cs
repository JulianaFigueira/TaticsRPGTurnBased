using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Unit))]
public class NonPlaybleCharacter : MonoBehaviour {

    
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public Unit character { get; private set; } // the movements we are controlling
    public Transform target;                                    // target to aim for


    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<Unit>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    private void Update()
    {
        if (character.CanMove && target != null)
        {
            agent.SetDestination(target.position);
        }

        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(true, false);
        else
        {
            character.Move(false, false);
            if (character.CanMove)
            {
                character.CanMove = false;
                character.AIAttackBehaviour();
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Attack()
    {
        character.Move(false, true);
    }

}
