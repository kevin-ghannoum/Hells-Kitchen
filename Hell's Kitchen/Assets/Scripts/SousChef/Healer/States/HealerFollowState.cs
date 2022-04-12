using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerFollowState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Follow state");
        healer.animator.SetBool("isWalking", true);
        healer.animator.SetBool("isRunning", true);
    }

    public override void UpdateState(HealerStateManager healer)
    {
        Debug.Log("@followState_UpdateState");
        if (healer.sc.targetEnemy == null && healer.sc.targetLoot == null)
        {
            // follow
            if (healer.sc.agent.Target != healer.sc.player.transform.position)
            {
                healer.sc.agent.Target = healer.sc.player.transform.position;
            }
            if (healer.sc.agent.ArrivalRadius != healer.sc.followDistance)
            {
                healer.sc.agent.ArrivalRadius = healer.sc.followDistance;
            }
            //if ((healer.transform.position - healer.sc.agent.Target).magnitude < healer.sc.followDistance) {
            if (!healer.sc.agent.IsMoving())
            {
                Debug.Log("arrived");
                //implement healer idle state
                healer.animator.SetBool("isWalking", false);
                healer.animator.SetBool("isRunning", false);
            }
            else
            {
                healer.animator.SetBool("isWalking", true);
                healer.animator.SetBool("isRunning", true);
            }
        }
        else if (healer.sc.targetEnemy != null)
        {
            // enter attack state
            healer.SwitchState(healer.attackState);
            return;
        }
        else if (healer.sc.character.isLowHP() || healer.sc.player.GetComponent<Character>().isLowHP())
        {
            // heal
            healer.SwitchState(healer.healState);
            return;
        }
        else if (healer.sc.targetLoot != null)
        {
            // enter loot state
            healer.SwitchState(healer.lootState);
            return;
        }

        // if (healer.sc.targetEnemy != null)
        // {
        //     healer.SwitchState(healer.moveToTarget);
        //     return;
        // }
        // else
        // {
        //     healer.SwitchState(healer.moveToTarget);
        //     return;
        // }

        // if (healer.sc.targetLoot == null)
        // {
        //     healer.sc.FindLoot();
        // }
        // else
        // {
        //     healer.SwitchState(healer.moveToTarget);
        //     return;
        // }
    }
}
