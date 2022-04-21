using UnityEngine;

public class HealerFollowState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Follow state");
    }

    public override void UpdateState(HealerStateManager healer)
    {
        // make sure the pick up animation ends before movings
        if(healer.animator.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            healer.sc.agent.standStill = true;
        }
        else
        {
            healer.sc.agent.standStill = false;
        }
        
        if (healer.sc.isLowHP() || healer.sc.IsPlayerLowHP())
        {
            // heal
            healer.SwitchState(healer.healState);
        }
        else if (healer.sc.targetEnemy == null && healer.sc.targetLoot == null)
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
        }
        else if (healer.sc.targetEnemy != null)
        {
            // enter move to target
            healer.SwitchState(healer.moveToTarget);
        }
        else if (healer.sc.targetLoot != null)
        {
            // enter loot state
            healer.SwitchState(healer.lootState);
        }
    }
}
