using UnityEngine;

public class KnightFollowState : KnightBaseState
{
    public override void EnterState(KnightStateManager knight)
    {
        Debug.Log("@Follow state");
    }

    public override void UpdateState(KnightStateManager knight)
    {
        // make sure the pick up animation ends before moving
        if(knight.animator.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            knight.sc.agent.standStill = true;
        }
        else
        {
            knight.sc.agent.standStill = false;
        }

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
        }
        else if (knight.sc.targetLoot != null)
        {
            // enter loot state
            knight.SwitchState(knight.lootState);
        }
    }
}
