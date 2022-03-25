using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerMoveToTargetState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@MoveToTarget state");
    }

    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.FindEnemy();
        healer.sc.FindLoot();
        if (healer.sc.character.isLowHP() && healer.sc.currentEnemyTarget == null)
        {
            healer.SwitchState(healer.healState);
            return;
        }
        else if (healer.sc.currentEnemyTarget != null)
        {
            if (healer.sc.getDistanceToEnemy() > healer.sc.attackRange + 2)
            {
                healer.sc.MoveToEnemy();
            }
            else if (healer.sc.getDistanceToEnemy() < healer.sc.attackRange - 2)
            {
                healer.sc.Flee();
            }
            else if (healer.sc.character.isLowHP()) {
                healer.SwitchState(healer.healState);
                return;
            }
            else
            {
                healer.SwitchState(healer.attackState);
                return;
            }
        }
        else if (healer.sc.currentLootTarget != null)
        {
            healer.SwitchState(healer.lootState);
            return;
        }
        else {
            healer.SwitchState(healer.followState);
        }
    }
}
