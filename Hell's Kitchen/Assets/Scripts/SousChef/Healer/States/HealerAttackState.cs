using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttackState : HealerBaseState
{
    float photonCastTime = 5f;
    float _photonCastTime = 0f;

    float photonDamage = 40f;
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Attack state");
        Debug.Log("@Attack charging spell... (" + photonCastTime + " seconds)");
        //spell casting animation
        //stand still while charging spell
    }


    public override void UpdateState(HealerStateManager healer)
    {
        _photonCastTime += Time.deltaTime;
        if (_photonCastTime >= photonCastTime)
        {
            Debug.Log("@Attack attacking xD");
            //play attack animation
            Debug.DrawLine(healer.gameObject.transform.position, healer.sc.targetEnemy.transform.position, Color.red, 5);
            //temp example
            //spells will probs be prefabs and inflict damage if collider hits the enemy
            healer.spells.HealerSpell_Photon(healer.sc.targetEnemy.transform.position);
            healer.sc.targetEnemy.GetComponent<Character>().TakeDamage(photonDamage);
            _photonCastTime = 0f;
            healer.SwitchState(healer.followState);
        }
    }
}
