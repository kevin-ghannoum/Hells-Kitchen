using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerMoveToTargetState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@MoveToTarget state");
        healer.animator.SetBool("isRunning", true);
    }

    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.FindEnemy();
        healer.sc.FindLoot();
        if (healer.sc.character.isLowHP() && healer.sc.targetEnemy == null)
        {
            healer.SwitchState(healer.healState);
            return;
        }
        else if (healer.sc.targetEnemy != null)
        {
            if (healer.sc.GetDistanceToEnemy() > healer.sc.attackRange + 2)
            {
                healer.sc.MoveToEnemy();
            }
            else if (healer.sc.GetDistanceToEnemy() < healer.sc.attackRange - 2)
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
        else if (healer.sc.targetLoot != null)
        {
            healer.SwitchState(healer.lootState);
            return;
        }
        else {
            healer.SwitchState(healer.followState);
        }
    }
}
