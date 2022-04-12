using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class HealerHealState : HealerBaseState
{
    float healCastTime = 10f;
    float _healCastTime = 0f;
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Heal [spell] state");
        //character stands still while charging spell
        //play spell charging animation
        
    }

    //call this if enemy damages souschef while casting, onDamaged event maybe? Xd
    public void InterruptSpellCast(HealerStateManager healer) {
        _healCastTime = 0f;
        healer.SwitchState(healer.moveToTarget);
        
    }
    public override void UpdateState(HealerStateManager healer)
    {
        _healCastTime += Time.deltaTime;
        if (_healCastTime >= healCastTime) {
            //play heal animation
            healer.sc.player.GetComponent<PlayerHealth>().HitPoints += 20;
            _healCastTime = 0f;
        }
        if (!healer.sc.character.isLowHP()) {
            healer.SwitchState(healer.followState);
        }
    }
}
