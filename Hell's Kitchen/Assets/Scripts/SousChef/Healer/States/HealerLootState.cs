using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerLootState : HealerBaseState
{
    float pickUpTime = 1f;
    float _pickUpTime = 0f;
    bool pickUp = true;

    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Loot state");
    }

    public override void UpdateState(HealerStateManager healer)
    {
        if (healer.sc.targetLoot != null)
        {
            // pathfinding with loot as target and arrival radius as 0.5f
            if (Vector3.Distance(healer.transform.position, healer.sc.targetLoot.transform.position) > 0.6f)
            {
                // move to loot position
                if (healer.sc.agent.Target != healer.sc.targetLoot.transform.position)
                {
                    healer.sc.agent.Target = healer.sc.targetLoot.transform.position;
                }
                if (healer.sc.agent.ArrivalRadius != 0.5f)
                {
                    healer.sc.agent.ArrivalRadius = 0.5f;
                }
            }
            else
            {
                // pick up + destroy loot: implemented in ItemDrop.cs
                healer.sc.agent.standStill = true;
                _pickUpTime += Time.deltaTime;
                if (pickUp)
                {
                    // play animation once
                    Debug.Log("@PickUp picking up xD");
                    healer.animator.SetTrigger("PickUp");
                    Debug.Log("(@healerLootState)need to implement looting functionality, replicate whatev player does when he loots here");
                    Debug.Log("(@healerLootState)dont do with colliders, just kill targetItem directly n put in bag");
                }
                if (_pickUpTime >= pickUpTime)
                {
                    healer.sc.agent.standStill = false;
                    _pickUpTime = 0f;
                    healer.SwitchState(healer.followState);
                }
            }
        }
        else
        {
            healer.SwitchState(healer.followState);
        }

        // throw new System.NotImplementedException();
    }
}
