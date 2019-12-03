using UnityEngine;
using System.Collections;
using System;

/*
This claas represents the basic data for the caracthers 
*/
public abstract class Unit : MonoBehaviour
{
    public enum SpecialStatus
    {
        OK,
        Stun,
        GoAgain,
        None,
        Dead
    }

    public struct AttackResult
    {
        public int Damage;
        public SpecialStatus Curse;

        public AttackResult(int damage, SpecialStatus curse)
        {
            Damage = damage;
            Curse = curse;
        }
    }

    public struct DefenseResult
    {
        public int Damage;

        public DefenseResult(int damage)
        {
            Damage = damage;
        }
    }

    public int Power;
    public int Range;
    public int Speed;
    public int Health;
    public SpecialStatus Status;

    public Unit(int life, int speed, int power, int range)
    {
        Power = power;
        Range = range;
        Health = life;
        Speed = speed;
    }

    public int GetPower()
    {
        return Power;
    }

    public int GetRange()
    {
        return Range;
    }

    public int GetSpeed()
    {
        return Speed;
    }

    internal float GetSqrSpeed()
    {
        return Speed * Speed;
    }

    public int GetHealth()
    {
        return Health;
    }

    public SpecialStatus GetStatus()
    {
        return Status;
    }

    public void Randevouz(Unit target)
    {
        CheckResult(target, target.ReceiveAttack(this, this.PrepareAttack(target)));
    }

    protected abstract AttackResult PrepareAttack(Unit target);
    protected abstract DefenseResult ReceiveAttack(Unit attacker, AttackResult offense);
    protected abstract void CheckResult(Unit target, DefenseResult defense);
}
