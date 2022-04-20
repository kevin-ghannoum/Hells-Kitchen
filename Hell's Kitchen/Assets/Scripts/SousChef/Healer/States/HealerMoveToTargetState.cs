using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerMoveToTargetState : HealerBaseState
{
    int fleeTile = -1;

    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@MoveToTarget state");
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;
                foundFleePoint = true;
                Debug.Log("flee point = " + result);
                return true;
            }
        }
        foundFleePoint = false;
        result = Vector3.zero;
        return false;
    }

    bool walkingBackToPlayer = false;
    bool foundFleePoint = false;
    Vector3 fleePoint;

    public override void UpdateState(HealerStateManager healer)
    {
        healer.sc.FindEnemy();
        healer.sc.FindLoot();
        if (healer.sc.character.isLowHP() && healer.sc.targetEnemy == null)
        {
            healer.SwitchState(healer.healState);
            return;
        }
        else if (healer.sc.targetEnemy != null)
        {
            if (healer.sc.GetDistanceToEnemy() > healer.sc.attackRange + 2)
            {
                //healer.sc.MoveToEnemy();
                if (healer.sc.agent.Target != healer.sc.targetEnemy.transform.position)
                    healer.sc.agent.Target = healer.sc.targetEnemy.transform.position;
                Debug.Log("implement moveToEnemy");

            }
            else if (healer.sc.GetDistanceToEnemy() < healer.sc.attackRange - 2)
            {
                //healer.sc.Flee();
                
                if (!walkingBackToPlayer && !foundFleePoint)
                {
                    RandomPoint(healer.transform.position, 90f, out fleePoint);
                }

                //Debug.Log("distToFleePoint=" + (healer.transform.position - fleePoint).magnitude + ", distToTargetInAgent=" + ((healer.transform.position - healer.sc.agent.Target).magnitude));
                if (foundFleePoint)
                {
                    //Debug.DrawRay(fleePoint, Vector3.up, Color.blue, 0.25f);
                    //Debug.DrawLine(healer.transform.position, fleePoint, Color.cyan, 0.25f);

                    if (healer.sc.agent.Target != fleePoint)
                        healer.sc.agent.Target = fleePoint;
                    if (healer.sc.agent.ArrivalRadius != healer.sc.followDistance)
                    {
                        healer.sc.agent.ArrivalRadius = healer.sc.followDistance;
                    }
                    if ((healer.transform.position - fleePoint).magnitude <= healer.sc.followDistance || healer.sc.agent.PathIsNull()) // || !isMoving
                    {
                        if (healer.sc.agent.PathIsNull()) {
                            Debug.Log("path was null, drawing bad path:");
                            Debug.DrawRay(healer.transform.position, Vector3.up, Color.green, 100f);
                            Debug.DrawRay(fleePoint, Vector3.up, Color.white, 100f);
                            Debug.DrawLine(healer.transform.position, fleePoint, Color.magenta, 100f);
                        }
                        Debug.Log("reset flee point");
                        foundFleePoint = false;
                    }
                }
                Debug.DrawRay(fleePoint, Vector3.up, Color.blue, 0.25f);
                Debug.DrawLine(healer.transform.position, fleePoint, Color.cyan, 0.25f);
                /*if (1 == 2 && (healer.transform.position - healer.sc.player.transform.position).magnitude > 20f) {
                    //walk back to player

                }
                else if () { 
                    
                }*/
            }
            else if (healer.sc.character.isLowHP()) {
                foundFleePoint = false;
                healer.SwitchState(healer.healState);
                return;
            }
            else
            {
                foundFleePoint = false;
                Debug.Log("distanceToEnemy=" + healer.sc.GetDistanceToEnemy() + ", healer.sc.attackRange=" + healer.sc.attackRange);
                healer.SwitchState(healer.attackState);
                return;
            }
        }
        else if (healer.sc.targetLoot != null)
        {
            foundFleePoint = false;
            healer.SwitchState(healer.lootState);
            return;
        }
        else {
            foundFleePoint = false;
            healer.SwitchState(healer.followState);
        }
    }
}
