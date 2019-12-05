using UnityEngine;
using System.Collections;

public interface ICharacterAgent
{
    void SetTarget(Vector3 newPosition, Unit target);
    void Attack();
    void CheckRoundState();
}

public class CharacterAgent : ICharacterAgent
{
    public UnityEngine.AI.NavMeshAgent Agent;
    public Unit Character; // the movements we are controlling
    public Unit Target;                                    // target to aim for
    public Vector3 NextPostion;

    public CharacterAgent(UnityEngine.AI.NavMeshAgent navMeshAgent, Unit unit)
    {
        this.Agent = navMeshAgent;
        this.Character = unit;

        Agent.updateRotation = false;
        Agent.updatePosition = true;
    }

    public void CheckRoundState()
    {
        if (Character.roundStage == Unit.RoundStage.Move && Target != null)
        {
            Agent.SetDestination(NextPostion);
        }

        if (Agent.remainingDistance > Agent.stoppingDistance)
        {
            Character.Move(true, false);
        }
        else
        {
            Character.Move(false, false);
            if ((Character.roundStage == Unit.RoundStage.Move || Character.roundStage == Unit.RoundStage.Attack) 
                && Target != null)
            {
                Attack();
            }
        }
    }

    public void SetTarget(Vector3 newPosition, Unit target)
    {
        Target = target;
        NextPostion = newPosition;
    }

    public void Attack()
    {
        Character.roundStage = Unit.RoundStage.Attack;
        Character.Randevouz(Target);
        Character.Move(false, true);
        Target = null;
        NextPostion = Character.transform.position;
    }
}
