using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerLootState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Loot state");
    }

    public override void UpdateState(HealerStateManager healer)
    {
        if(healer.sc.targetLoot != null){
            // pathfinding with loot as target and arrival radius as 0.5f
            if(Vector3.Distance(healer.transform.position, healer.sc.player.transform.position) > 0.6f){
                // move to loot position
                if(healer.sc.agent.Target != healer.sc.player.transform.position){
                    healer.sc.agent.Target = healer.sc.player.transform.position;
                }
                if(healer.sc.agent.ArrivalRadius != 0.5f){
                    healer.sc.agent.ArrivalRadius = 0.5f;
                }
            }
            else{
                // pick up
                // destroy loot
                // play animation
            }
        }
        else{
            healer.SwitchState(healer.followState);
        }
        
        // throw new System.NotImplementedException();
    }
}
