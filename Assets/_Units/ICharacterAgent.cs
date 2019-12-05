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

        Target = null;
    }

    public void CheckRoundState()
    {
        if (Character.roundStage == Unit.RoundStage.Move && NextPostion != Vector3.up)
        {
            //Agent.SetDestination(NextPostion);
            NextPostion = Vector3.up;
        }

       //if (Agent.remainingDistance > Agent.stoppingDistance)
       //{
       //    Character.Move(true, false);
       //}
       //else
        {
            Agent.isStopped = true;
            Character.Move(false, false);

            if (Character.roundStage == Unit.RoundStage.Move && Character.unitType == Unit.UnitType.PC)
            {
                    Debug.Log("Click on a nearby unit to attack");
                    Character.roundStage = Unit.RoundStage.ChoseAttack;
                    Target = null;
            }
            else if (Character.roundStage == Unit.RoundStage.Move && Character.unitType == Unit.UnitType.NPC && Target == null)
            {
                Debug.Log("Click on a nearby unit to attack");
                Character.FinishRound();
            }

            if ((Character.roundStage == Unit.RoundStage.Move || Character.roundStage == Unit.RoundStage.Attack)
                    && Target != null)
            {
                Debug.Log(Character.name + "attacked!");
                Attack();
            }
        }
    }

    public void SetTarget(Vector3 newPosition, Unit target)
    {
        Character.roundStage = Unit.RoundStage.Move;
        Target = target;
        NextPostion = newPosition;
        Debug.Log(newPosition.ToString() + " " + target.name );
        Debug.DrawLine(newPosition, Character.gameObject.transform.position, Color.magenta);
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
