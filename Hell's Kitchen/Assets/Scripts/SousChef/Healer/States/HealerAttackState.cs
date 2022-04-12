using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttackState : HealerBaseState
{
    float photonCastTime = 5f;
    float _photonCastTime = 0f;

    float photonDamage = 40f;

    float _cooldownBetweenAttacks = 1f;
    float cooldownBetweenAttacks = 1f;
    bool castSpell = true;
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Attack state");
        Debug.Log("@Attack charging spell... (" + photonCastTime + " seconds)");
        healer.animator.SetBool("isWalking", false);
        healer.animator.SetBool("isRunning", false);
    }


    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.faceTargetEnemy();
        healer.sc.agent.standStill = true;
        _cooldownBetweenAttacks += Time.deltaTime;
        if(castSpell){
            healer.animator.SetTrigger("CastSpell");
            castSpell = false;
        }
        //do a reposition check while waaiting for cd (ie if enemy gets too close, run away instead of sittin there xd
        if (healer.canAttack()) {
            //spell casting animation
            healer.magicCircle.gameObject.SetActive(true);
            _photonCastTime += Time.deltaTime;
            if (_photonCastTime >= photonCastTime)
            {
                healer.animator.SetTrigger("Attack");
                Debug.Log("@Attack attacking xD");
                healer.sc.agent.standStill = false;
                //play attack animation
                healer.spells.HealerSpell_Photon(healer.sc.targetEnemy.transform.position);
                healer.sc.targetEnemy.GetComponent<Character>().TakeDamage(photonDamage);
                healer.magicCircle.gameObject.SetActive(false);
                _photonCastTime = 0f;
                healer.sc.targetEnemy = null;
                castSpell = true;
                healer.resetAttackCD();
                healer.SwitchState(healer.moveToTarget);
            }
        }

    }
}
