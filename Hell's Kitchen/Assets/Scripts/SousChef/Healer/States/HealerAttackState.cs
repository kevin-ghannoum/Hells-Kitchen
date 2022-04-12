using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttackState : HealerBaseState
{
    float photonCastTime = 5f;
    float _photonCastTime = 0f;

    [SerializeField] float photonDamage = 50f;

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

        if (healer.sc.targetEnemy == null) {
            healer.resetAttackCD();
            _photonCastTime = 0f;
            healer.magicCircle.gameObject.SetActive(false);
            healer.SwitchState(healer.moveToTarget);
            healer.sc.agent.standStill = false;
            return;
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
                healer.magicCircle.gameObject.SetActive(false);
                healer.sc.targetEnemy.GetComponent<IKillable>().TakeDamage(photonDamage);
                _photonCastTime = 0f;
                healer.sc.targetEnemy = null;
                healer.resetAttackCD();
                healer.SwitchState(healer.moveToTarget);
            }
        }

    }
}
