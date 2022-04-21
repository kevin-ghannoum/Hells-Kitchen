using UnityEngine;

public class HealerMoveToTargetState : HealerBaseState
{
    int fleeTile = -1;

    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@MoveToTarget state");
    }

    bool walkingBackToPlayer = false;
    bool foundFleePoint = false;
    Vector3 fleePoint;

    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.FindEnemy();
        healer.sc.FindLoot();
        if (healer.sc.isLowHP() || healer.sc.IsPlayerLowHP())
        {
            healer.SwitchState(healer.healState);
        }
        else if (healer.sc.targetEnemy != null)
        {
            if (healer.sc.GetDistanceToEnemy() > healer.sc.attackRange + 5)
            {
                if (healer.sc.agent.Target != healer.sc.targetEnemy.transform.position)
                    healer.sc.agent.Target = healer.sc.targetEnemy.transform.position;
            }
            else if (healer.sc.GetDistanceToEnemy() < healer.sc.attackRange - 2)
            {
                healer.sc.Flee();
            }
            else if (healer.sc.isLowHP())
            {
                foundFleePoint = false;
                healer.SwitchState(healer.healState);
            }
            else
            {
                foundFleePoint = false;
                Debug.Log("distanceToEnemy=" + healer.sc.GetDistanceToEnemy() + ", healer.sc.attackRange=" + healer.sc.attackRange);
                healer.SwitchState(healer.attackState);
            }
        }
        else if (healer.sc.targetLoot != null)
        {
            foundFleePoint = false;
            healer.SwitchState(healer.lootState);
        }
        else
        {
            foundFleePoint = false;
            healer.SwitchState(healer.followState);
        }
    }
}
