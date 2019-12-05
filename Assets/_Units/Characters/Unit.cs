﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
This claas represents the basic data for the caracthers 
And its animation control
*/
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ICharacterAgent))]
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

    public enum RoundStage
    {
        Move,
        Attack,
        None
    }

    public enum UnitType
    {
        PlayableCharacter,
        NonPlayableCharacter
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
    public UnitType unitType;
    public SpecialStatus Status;
    public RoundStage roundStage;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private CapsuleCollider m_Capsule;
    private bool m_Attack;
    private bool m_Moving;

    protected ICharacterAgent CharacterAgent;
    protected Action OnRoundFinished;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        CharacterAgent = GetComponent<ICharacterAgent>();
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

    public float GetSqrSpeed()
    {
        return Speed * Speed;
    }

    public void Randevouz(Unit target)
    {
        CheckResult(target, target.ReceiveAttack(this, this.PrepareAttack(target)));
        FinishRound();
    }

    public void AIRoundBehaviour(List<Unit> unitsFighters, Action onRoundFinished)
    {
        OnRoundFinished = onRoundFinished;
        AIMoveBehaviour(unitsFighters);
    }

    public void FinishRound()
    {
        roundStage = RoundStage.None;
        OnRoundFinished();
    }

    protected abstract AttackResult PrepareAttack(Unit target);
    protected abstract DefenseResult ReceiveAttack(Unit attacker, AttackResult offense);
    protected abstract void CheckResult(Unit target, DefenseResult defense);
    protected abstract void AIMoveBehaviour(List<Unit> fighters);
    
}
