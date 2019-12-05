using System.Collections.Generic;
using UnityEngine;

/*
 Healer is a pacifist, protected by the divine
*/
public class HealerFighter : Unit
{
    protected override void CheckResult(Unit target, DefenseResult defense)
    {
        //nothing to do
    }

    protected override AttackResult PrepareAttack(Unit target)
    {
        return new AttackResult(-Power, SpecialStatus.None);
    }

    protected override DefenseResult ReceiveAttack(Unit attacker, AttackResult offense)
    {
        Health -= offense.Damage;

        if (Health <= 0)
        {
            if (Random.value >= 0.25f) // ~75% chance of divine intervention
            {
                Health = Power;
                Status = SpecialStatus.OK;
            }
            else
            {
                Status = SpecialStatus.Dead;
            }
        }
        else if (Status == SpecialStatus.OK && offense.Curse != SpecialStatus.None)
        {
            Status = offense.Curse;
        }

        return new DefenseResult(0);
    }

    protected override void AIMoveBehaviour(List<Unit> fighters)
    {
        Unit weakest = this;
        int minHealth = int.MaxValue;

        // find waekest adversary
        for (int i = 0; i < fighters.Count; i++)
        {
            if (fighters[i].tag == "Enemy") //healer works on their own kind
            {
                if (fighters[i].Health < minHealth)
                {
                    minHealth = fighters[i].Health;
                    weakest = fighters[i];
                }
            }
        }

        Vector3 target;
        if (Vector3.Distance(transform.position, weakest.transform.position) > (Speed + Range))
        {
            target = weakest.transform.position * (Speed / weakest.transform.position.magnitude);
        }
        else
        {
            target = weakest.transform.position;
        }

        CharacterAgent.SetTarget(target, weakest);
    }
}
