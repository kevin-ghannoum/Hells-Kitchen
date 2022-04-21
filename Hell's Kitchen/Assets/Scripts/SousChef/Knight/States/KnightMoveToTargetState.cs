using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMoveToTargetState : KnightBaseState
{
    public override void EnterState(KnightStateManager knight)
    {
        Debug.Log("@MoveToTarget state");
    }

    public override void UpdateState(KnightStateManager knight)
    {
        if(knight.sc.GetDistanceToPlayer() <= knight.sc.searchRange){
            knight.sc.FindEnemy();
            knight.sc.FindLoot();
        }
        

        if(knight.sc.targetEnemy != null){
            if(knight.sc.GetDistanceToEnemy() > knight.sc.searchRange){
                knight.sc.targetEnemy = null;
                knight.SwitchState(knight.followState);
            }
            else{
                knight.weapon.GetComponent<Collider>().enabled = false;
                knight.SwitchState(knight.attackState);
            }
        }
        else if(knight.sc.targetLoot != null){
            knight.SwitchState(knight.lootState);
        }
    }
}
