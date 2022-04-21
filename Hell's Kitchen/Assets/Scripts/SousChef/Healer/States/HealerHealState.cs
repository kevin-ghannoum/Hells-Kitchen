using Common;
using UnityEngine;

public class HealerHealState : HealerBaseState
{
    float healCastTime = 5f;
    float _healCastTime = 0f;
    public override void EnterState(HealerStateManager healer)
    {
        _healCastTime = 0f;
        Debug.Log("@Heal [spell] state");
        _delayBetweenCast = 0f;
        _attackAnimationTime = 0f;
        healer.sc.agent.standStill = false;
        isAttackAnimationPlaying = false;
        isCasting = false;
    }

    //call this if enemy damages souschef while casting, onDamaged event maybe? Xd
    public void InterruptSpellCast(HealerStateManager healer) {
        healer.sc.agent.standStill = false;
        healer.healCircle.gameObject.SetActive(false);
        healer.SwitchState(healer.moveToTarget);
    }

    float delayBetweenCast = 2f;
    float _delayBetweenCast = 0f;
    bool isAttackAnimationPlaying = false;
    float _attackAnimationTime = 0f;
    float attackAnimationTime = 3.75f;
    bool isCasting = false;
    public override void UpdateState(HealerStateManager healer)
    {

        if (isAttackAnimationPlaying)
        {
            _attackAnimationTime += Time.deltaTime;
            if (_attackAnimationTime > attackAnimationTime)
            {
                healer.sc.agent.standStill = false;
                healer.SwitchState(healer.moveToTarget);
                return;
            }
        }

        _delayBetweenCast += Time.deltaTime;

        if (_delayBetweenCast >= delayBetweenCast && !isAttackAnimationPlaying)
        {
            isCasting = true;
            healer.sc.facePlayer();
            healer.animator.SetTrigger("CastSpell");
            healer.sc.agent.standStill = true;
            healer.healCircle.gameObject.SetActive(true);
            _healCastTime += Time.deltaTime;
            if (_healCastTime >= healCastTime)
            {
                healer.animator.SetTrigger("CastSpell");
                healer.healCircle.gameObject.SetActive(false);
                healer.spells.HealerSpell_Heal(healer.sc.hitPoints > healer.sc.GetPlayerHP() ? healer.sc.player.transform.position : healer.transform.position);
                _healCastTime = 0f;
                _delayBetweenCast = 0f;
                isAttackAnimationPlaying = true;
                isCasting = false;
            }
        }

        if (!healer.sc.isLowHP() && !healer.sc.IsPlayerLowHP() && !isAttackAnimationPlaying && !isCasting) {
            healer.healCircle.gameObject.SetActive(false);
            healer.sc.agent.standStill = false;
            healer.SwitchState(healer.followState);
        }
    }
}
