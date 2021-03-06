using UnityEngine;

public class HealerLootState : HealerBaseState
{
    float pickUpTime = 1.5f;
    float _pickUpTime = 0f;
    bool pickUp = true;
    bool isAnimPlaying = false;

    public override void EnterState(HealerStateManager healer)
    {
        isAnimPlaying = false;
        pickUp = true;
        _pickUpTime = 0f;
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
                healer.sc.agent.standStill = false;
                healer.sc.agent.ArrivalRadius = 0.1f;
                healer.sc.agent.Target = healer.sc.targetLoot.transform.position;
            }
            else
            {
                // pick up + destroy loot: implemented in ItemDrop.cs
                healer.sc.agent.standStill = true;
                _pickUpTime += Time.deltaTime;
                if (pickUp)
                {
                    // play animation once
                    if(!isAnimPlaying)
                    {
                        healer.animator.SetTrigger("PickUp");
                        isAnimPlaying = true;
                    }
                }
                if (_pickUpTime >= pickUpTime)
                {
                    healer.sc.agent.standStill = false;
                    _pickUpTime = 0f;
                    isAnimPlaying = false;
                    pickUp = true;
                    healer.SwitchState(healer.followState);
                }
            }
        }
        else
        {
            healer.sc.agent.standStill = false;
            healer.SwitchState(healer.followState);
        }
    }
}
