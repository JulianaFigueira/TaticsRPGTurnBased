using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class UnitMove : MonoBehaviour {

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;
    bool m_Attack;
    bool m_Moving;

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
}
