using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAttackState : HealerBaseState
{
    float photonCastTime = 5f;
    float _photonCastTime = 0f;

    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Attack state");
        Debug.Log("@Attack charging spell... (" + photonCastTime + " seconds)");
        isAttackAnimationPlaying = false;
        _attackAnimationTime = 0f;
    }

    bool isAttackAnimationPlaying = false;
    float _attackAnimationTime = 0f;
    float attackAnimationTime = 3.75f;
    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.faceTargetEnemy();
        healer.sc.agent.standStill = true;

        if (healer.sc.targetEnemy == null && !isAttackAnimationPlaying) {
            healer.resetAttackCD();
            _photonCastTime = 0f;
            healer.magicCircle.gameObject.SetActive(false);
            healer.SwitchState(healer.moveToTarget);
            healer.sc.agent.standStill = false;
            return;
        }

        if (isAttackAnimationPlaying) {
            _attackAnimationTime += Time.deltaTime;
            if (_attackAnimationTime > attackAnimationTime) {
                healer.sc.agent.standStill = false;
                healer.SwitchState(healer.moveToTarget);
            }
        }

        if (healer.canAttack() && !isAttackAnimationPlaying) {
            //spell casting animation
            healer.animator.SetTrigger("CastSpell");
            healer.magicCircle.gameObject.SetActive(true);
            _photonCastTime += Time.deltaTime;
            if (_photonCastTime >= photonCastTime)
            {
                isAttackAnimationPlaying = true;
                healer.animator.SetTrigger("Attack");
                Debug.Log("@Attack attacking xD");
                //healer.sc.agent.standStill = false;
                //play attack animation
                healer.spells.HealerSpell_Photon(healer.sc.targetEnemy);
                healer.magicCircle.gameObject.SetActive(false);
                _photonCastTime = 0f;
                healer.sc.targetEnemy = null;
                healer.resetAttackCD();
                //healer.SwitchState(healer.moveToTarget);
            }
        }


    }
}
