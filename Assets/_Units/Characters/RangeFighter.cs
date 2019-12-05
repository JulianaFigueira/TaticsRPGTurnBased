using System.Collections.Generic;
using UnityEngine;

/*
 Range is not too strong but attacks from a distance, may trigger a adrenaline rush when finishing a target
*/
public class RangeFighter : Unit
{
    protected override void CheckResult(Unit target, DefenseResult defense)
    {
        //Health -= defense.Damage;

        if (target.Health - Power <= 0)
        {
            Debug.Log(name + " killed an enemy! Adrenaline rush!");
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
            Debug.Log(name + " died . . .");
            Status = SpecialStatus.Dead;
        }
        else if (Status == SpecialStatus.OK && offense.Curse != SpecialStatus.None)
        {
            Debug.Log(name + " is " + offense.Curse.ToString());
            Status = offense.Curse;
        }

        return new DefenseResult(Random.Range(0, Power) / 10);
    }
    
    protected override void AIMoveBehaviour(List<Unit> fighters)
    {
        Unit closest = this;
        float minDistance = float.MaxValue;
        float dif;

        // find closest adversary
        for (int i = 0; i < fighters.Count; i++)
        {
            if (fighters[i].tag == "Player") //NPC Enemy seeks PC
            {
                dif = Vector3.Distance(fighters[i].transform.position, this.transform.position);
                if (dif < minDistance)
                {
                    minDistance = dif;
                    closest = fighters[i];
                }
            }
        }

        Vector3 target;
        if (minDistance > (Speed + Range))
        {
            target = closest.transform.position * (Speed/ closest.transform.position.magnitude);
        }
        else
        {
            target = closest.transform.position;
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

        Debug.Log(name + " targeted " + closest.name);
        CharacterAgent.SetTarget(target, closest);
    }
}
