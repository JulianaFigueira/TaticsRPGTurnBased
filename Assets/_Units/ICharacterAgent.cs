using UnityEngine;
using System.Collections;
using UnityEngine.AI;

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

        Target = null;
    }

    public void CheckRoundState()
    {
        if (Character.roundStage == Unit.RoundStage.Move)
        {
            if (Agent.pathPending)
                return;

            if (Agent.remainingDistance > Agent.stoppingDistance)
            {
                Character.Move(true, false);
            }
            else
            {
                Agent.isStopped = true;
                Character.Move(false, false);
                switch(Character.unitType)
                {
                    case Unit.UnitType.NPC:
                        if (Target == null)
                        {
                            Character.FinishRound();
                        }
                        else
                        {
                            Attack();
                        }
                        break;
                    case Unit.UnitType.PC:
                        Debug.Log("Click on a nearby unit to attack");
                        Character.roundStage = Unit.RoundStage.ChoseAttack;
                        Target = null;
                        break;
                }
            }
        }
    }

    public void SetTarget(Vector3 newPosition, Unit target)
    {
        Character.roundStage = Unit.RoundStage.Move;
        Target = target;
        NextPostion = newPosition;
        Agent.isStopped = false;

            NavMeshHit navHit;
            NavMesh.SamplePosition(NextPostion, out navHit, Character.Speed, NavMesh.AllAreas); //now get a valid point inside the navmesh from the previous random position
            NextPostion = navHit.position; // set the new valid destination

        //Agent.SetDestination(NextPostion);
        Agent.Warp(NextPostion);

        Debug.DrawLine(newPosition, Character.gameObject.transform.position, Color.magenta, 10.0f, false);
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
