using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightLootState : KnightBaseState
{
    float pickUpTime = 1.5f;
    float _pickUpTime = 0f;
    bool pickUp = true;
    bool isAnimPlaying = false;

    public override void EnterState(KnightStateManager knight)
    {
        isAnimPlaying = false;
        pickUp = true;
        _pickUpTime = 0f;
        Debug.Log("@Loot state");
    }

    public override void UpdateState(KnightStateManager knight)
    {
        Debug.Log("@Loot state");
        if (knight.sc.targetLoot != null)
        {
            // pathfinding with loot as target and arrival radius as 0.5f
            if (Vector3.Distance(knight.transform.position, knight.sc.targetLoot.transform.position) > 0.6f)
            {
                // move to loot position
                Debug.Log("@PickUp moving to loot");
                Debug.Log("@PickUp distance to loot: " + Vector3.Distance(knight.transform.position, knight.sc.targetLoot.transform.position));
                knight.sc.agent.standStill = false;
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
                    if(!isAnimPlaying){
                        knight.animator.SetTrigger("PickUp");
                        isAnimPlaying = true;
                    }
                }
                if (_pickUpTime >= pickUpTime)
                {
                    knight.sc.agent.standStill = false;
                    _pickUpTime = 0f;
                    isAnimPlaying = false;
                    pickUp = true;
                    knight.SwitchState(knight.followState);
                }
            }
        }
        else
        {
            knight.sc.agent.standStill = false;
            knight.SwitchState(knight.followState);
        }
    }
}
