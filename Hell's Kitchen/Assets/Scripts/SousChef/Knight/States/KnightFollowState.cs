using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightFollowState : KnightBaseState
{
    public override void EnterState(KnightStateManager knight)
    {
        Debug.Log("@Follow state");
    }

    public override void UpdateState(KnightStateManager knight)
    {
        if (knight.sc.targetEnemy == null && knight.sc.targetLoot == null)
        {
            // follow
            if (knight.sc.agent.Target != knight.sc.player.transform.position)
            {
                knight.sc.agent.Target = knight.sc.player.transform.position;
            }
            if (knight.sc.agent.ArrivalRadius != knight.sc.followDistance)
            {
                knight.sc.agent.ArrivalRadius = knight.sc.followDistance;
            }
        }
        else if (knight.sc.targetEnemy != null)
        {
            // enter move to target
            knight.SwitchState(knight.moveToTarget);
            return;
        }
        else if (knight.sc.targetLoot != null)
        {
            // enter loot state
            knight.SwitchState(knight.lootState);
            return;
        }
    }
}
