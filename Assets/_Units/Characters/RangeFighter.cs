using System.Collections.Generic;
using UnityEngine;

/*
 Range is not too strong but attacks from a distance, may trigger a killer rush when finishing a target
*/
public class RangeFighter : Unit
{
    protected override void CheckResult(Unit target, DefenseResult defense)
    {
        Health -= defense.Damage;

        if (target.Health - Power <= 0)
        {
            Status = SpecialStatus.GoAgain;
        }
    }

    protected override AttackResult PrepareAttack(Unit target)
    {
        return new AttackResult(Power, SpecialStatus.None);
    }

    protected override DefenseResult ReceiveAttack(Unit attacker, AttackResult offense)
    {
        Health -= offense.Damage;

        if (Health <= 0)
        {
            Status = SpecialStatus.Dead;
        }
        else if (Status == SpecialStatus.OK && offense.Curse != SpecialStatus.None)
        {
            Status = offense.Curse;
        }

        return new DefenseResult(Random.Range(0, Power) / 10);
    }
    
    protected override void AIMoveBehaviour(List<Unit> fighters)
    {
        Unit closest = this;
        float minDistance = float.MaxValue;
        float dif;

        // find closest adversary
        for (int i = 0; i < fighters.Count; i++)
        {
            if (fighters[i].tag == "Ally") //NPC Enemy seeks Ally PC
            {
                dif = Vector3.Distance(fighters[i].transform.position, this.transform.position);
                if (dif < minDistance)
                {
                    minDistance = dif;
                    closest = fighters[i];
                }
            }
        }

        Vector3 target;
        if (minDistance > (Speed + Range))
        {
            target = closest.transform.position * (Speed/ closest.transform.position.magnitude);
        }
        else
        {
            target = closest.transform.position;
        }

        CharacterAgent.SetTarget(target, closest);
    }
}
