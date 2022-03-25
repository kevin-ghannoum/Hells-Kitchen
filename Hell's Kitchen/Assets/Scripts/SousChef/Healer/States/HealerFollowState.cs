using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerFollowState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Follow state");
    }

    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.Follow();
        if (healer.sc.currentEnemyTarget == null)
        {
            healer.sc.FindEnemy();
        }
        else
        {
            healer.SwitchState(healer.moveToTarget);
            return;
        }

        if (healer.sc.currentLootTarget == null)
        {
            healer.sc.FindLoot();
        }
        else
        {
            healer.SwitchState(healer.moveToTarget);
            return;
        }

    }
}
