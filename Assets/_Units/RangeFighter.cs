using UnityEngine;

/*
 Range is not too strong but attacks from a distance, may trigger a killer rush when finishing a target
*/
public class RangeFighter : Unit
{
    public RangeFighter(int life, int speed, int power, int range) : base(life, speed, power, range)
    {
    }

    protected override void CheckResult(Unit target, DefenseResult defense)
    {
        Health -= defense.Damage;

        if (target.GetHealth() - Power <= 0)
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
