using System.Collections.Generic;
using UnityEngine;

/*
 Melee is a brute who can stun 
*/
public class MeleeFighter : Unit
{
    protected override void CheckResult(Unit target, DefenseResult defense)
    {
        //Health -= defense.Damage;
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
            Debug.Log(name + " died . . .");
            Status = SpecialStatus.Dead;
        }
        else if (Status == SpecialStatus.OK && offense.Curse != SpecialStatus.None)
        {
            Debug.Log(name + " is " + offense.Curse.ToString());
            Status = offense.Curse;
        }

        return new DefenseResult(Random.Range(0, Power) / 8);
    }
    
    protected override void AIMoveBehaviour(List<Unit> fighters)
    {
        Unit weakest = this;
        int minHealth = int.MaxValue;

        // find waekest adversary
        for (int i = 0; i < fighters.Count; i++)
        {
            if (fighters[i].tag == "Player")
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

        Ray ray = new Ray(target, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //check if valid position
            if (hit.collider.gameObject.tag == "Ground")
            {
                target = hit.collider.gameObject.transform.position;
            }
        }

        Debug.Log(name + " targeted " + weakest.name);
        CharacterAgent.SetTarget(target, weakest);
    }

}
