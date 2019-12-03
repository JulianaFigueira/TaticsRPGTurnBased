using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitMove))]
public class NonPlaybleCharacter : MonoBehaviour {

    
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public UnitMove characterMove { get; private set; } // the movements we are controlling
    public Transform target;                                    // target to aim for


    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        characterMove = GetComponent<UnitMove>();

        agent.updateRotation = false;
        agent.updatePosition = true;
    }

    private void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
            characterMove.Move(true, false);
        else
            characterMove.Move(false, false);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Attack()
    {
        characterMove.Move(false, true);
    }

}
