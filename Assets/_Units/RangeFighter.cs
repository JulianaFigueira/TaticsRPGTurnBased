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
}
