using System.Collections;
using System.Collections.Generic;
using Common;
using Player;
using UnityEngine;

public class HealerHealState : HealerBaseState
{
    float healCastTime = 5f;
    float _healCastTime = 0f;
    public override void EnterState(HealerStateManager healer)
    {
        _healCastTime = 0f;
        Debug.Log("@Heal [spell] state");
        _delayBetweenCast = 2f;

    }

    //call this if enemy damages souschef while casting, onDamaged event maybe? Xd
    public void InterruptSpellCast(HealerStateManager healer) {
        healer.sc.agent.standStill = false;
        healer.healCircle.gameObject.SetActive(false);
        healer.SwitchState(healer.moveToTarget);
    }

    float delayBetweenCast = 2f;
    float _delayBetweenCast = 0f;
    public override void UpdateState(HealerStateManager healer)
    {
        _delayBetweenCast += Time.deltaTime;

        if (_delayBetweenCast >= delayBetweenCast)
        {
            healer.sc.agent.standStill = true;
            healer.healCircle.gameObject.SetActive(true);
            _healCastTime += Time.deltaTime;
            if (_healCastTime >= healCastTime)
            {
                healer.healCircle.gameObject.SetActive(false);
                //play heal animation
                //healer.sc.player.GetComponent<PlayerHealth>().HitPoints += 20;
                healer.spells.HealerSpell_Heal(healer.sc.hitPoints > GameStateManager.Instance.playerCurrentHitPoints ? healer.sc.player.transform.position : healer.transform.position);
                //GameStateManager.Instance.playerCurrentHitPoints += 20;
                //healer.sc.hitPoints += 20;
                _healCastTime = 0f;
                _delayBetweenCast = 0f;
            }
        }
        else {
            healer.sc.agent.standStill = false;
        }

        if (!healer.sc.isLowHP() && !GameStateManager.Instance.IsLowHP()) {
            healer.healCircle.gameObject.SetActive(false);
            healer.sc.agent.standStill = false;
            healer.SwitchState(healer.followState);
        }
    }
}
