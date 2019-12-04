using UnityEngine;
using System.Collections;
using System;

/*
This claas represents the basic data for the caracthers 
And its animation control
*/
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
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
    public int AttackOrder;
    public SpecialStatus Status;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private CapsuleCollider m_Capsule;
    private bool m_Attack;
    private bool m_Moving;
    public bool CanMove;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }


    public void Move(bool moving, bool attacking)
    {
        // send input and other state parameters to the animator
        m_Moving = moving;
        m_Attack = attacking;
        UpdateAnimator();
    }


    void UpdateAnimator()
    {
        // update the animator parameters
        if (m_Attack)
        {
            m_Animator.SetTrigger("Attack1Trigger");
            m_Attack = false;
        }
        else
        {
            m_Animator.ResetTrigger("Attack1Trigger");
        }

        m_Animator.SetBool("Moving", m_Moving);
    }

    internal float GetSqrSpeed()
    {
        return Speed * Speed;
    }

    public void Randevouz(Unit target)
    {
        CheckResult(target, target.ReceiveAttack(this, this.PrepareAttack(target)));
    }

    protected abstract AttackResult PrepareAttack(Unit target);
    protected abstract DefenseResult ReceiveAttack(Unit attacker, AttackResult offense);
    protected abstract void CheckResult(Unit target, DefenseResult defense);
}
