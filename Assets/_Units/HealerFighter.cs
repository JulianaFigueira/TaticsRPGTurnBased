using UnityEngine;

/*
 Healer is a pacifist, protected by the divine
*/
public class HealerFighter : Unit
{
    public HealerFighter(int life, int speed, int power, int range) : base(life, speed, power, range)
    {
    }

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
}
