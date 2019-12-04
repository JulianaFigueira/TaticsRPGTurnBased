using System.Collections.Generic;
using UnityEngine;

/*
 Melee is a brute who can stun 
*/
public class MeleeFighter : Unit
{
    protected override void CheckResult(Unit target, DefenseResult defense)
    {
        Health -= defense.Damage;
    }

    protected override AttackResult PrepareAttack(Unit target)
    {
        // ~25% of provoking stun 
        return new AttackResult(Power, Random.value >= 0.75f ? SpecialStatus.Stun : SpecialStatus.OK);
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

        return new DefenseResult(Random.Range(0, Power) / 8);
    }

    public override void AIAttackBehaviour()
    {
        throw new System.NotImplementedException();
    }

    public override void AIMoveBehaviour(List<Unit> fighters)
    {
        throw new System.NotImplementedException();
    }

}
