using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightLootState : KnightBaseState
{
    float pickUpTime = 1f;
    float _pickUpTime = 0f;
    bool pickUp = true;

    public override void EnterState(KnightStateManager knight)
    {
        Debug.Log("@Loot state");
    }

    public override void UpdateState(KnightStateManager knight)
    {
        if (knight.sc.targetLoot != null)
        {
            // pathfinding with loot as target and arrival radius as 0.5f
            if (Vector3.Distance(knight.transform.position, knight.sc.targetLoot.transform.position) > 0.6f)
            {
                // move to loot position
                if (knight.sc.agent.Target != knight.sc.targetLoot.transform.position)
                {
                    knight.sc.agent.Target = knight.sc.targetLoot.transform.position;
                }
                if (knight.sc.agent.ArrivalRadius != 0.5f)
                {
                    knight.sc.agent.ArrivalRadius = 0.5f;
                }
            }
            else
            {
                // pick up + destroy loot: implemented in ItemDrop.cs
                knight.sc.agent.standStill = true;
                _pickUpTime += Time.deltaTime;
                if (pickUp)
                {
                    // play animation once
                    Debug.Log("@PickUp picking up xD");
                    knight.animator.SetTrigger("PickUp");
                    Debug.Log("(@knightLootState)need to implement looting functionality, replicate whatev player does when he loots here");
                    Debug.Log("(@knightLootState)dont do with colliders, just kill targetItem directly n put in bag");
                }
                if (_pickUpTime >= pickUpTime)
                {
                    knight.sc.agent.standStill = false;
                    _pickUpTime = 0f;
                    knight.SwitchState(knight.followState);
                }
            }
        }
        else
        {
            knight.SwitchState(knight.followState);
        }
    }
}
