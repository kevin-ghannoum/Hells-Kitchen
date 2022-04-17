using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerStateManager : MonoBehaviour
{
    HealerBaseState currentState;
    public HealerAttackState attackState = new HealerAttackState();
    public HealerMoveToTargetState moveToTarget = new HealerMoveToTargetState();
    public HealerFollowState followState = new HealerFollowState();
    public HealerLootState lootState = new HealerLootState();
    public HealerHealState healState = new HealerHealState();
    public Animator animator;
    public Transform magicCircle;
    public Transform healCircle;
    public GameObject startTeleportPrefab;
    public GameObject endTeleportPrefab;
    public SousChef sc { get; set; }
    public SpellManager spells { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spells = gameObject.GetComponent<SpellManager>();
        sc = gameObject.GetComponent<SousChef>();
        currentState = followState;
        currentState.EnterState(this);
        transform.position = sc.player.transform.position;

    }


    float attackCooldown = 1.5f;
    float _attackCooldown = 0f;
    public bool canAttack() => _attackCooldown >= attackCooldown;
    public void resetAttackCD() => _attackCooldown = 0f;

    float maxTeleportDistance = 10f;
    bool beganTeleport = false;
    float delayBetweenTeleports = 0.75f;
    float _delayBetweenTeleports = 0f;
    bool canTeleport() => _delayBetweenTeleports >= delayBetweenTeleports;
    bool shouldTeleport() => !sc.agent.standStill && sc.agent.IsMoving() && sc.agent.Target != null && (Vector3.Distance(transform.position, sc.agent.Target) > 15);
    private void Update()
    {


        _attackCooldown += Time.deltaTime;
        gameObject.GetComponent<Animator>().SetBool("isWalking", sc.agent.IsMoving());
        gameObject.GetComponent<Animator>().SetBool("isRunning", sc.agent.IsMoving());
        currentState.UpdateState(this);

        _delayBetweenTeleports += Time.deltaTime;

        if (canTeleport() && shouldTeleport())// && !beganTeleport)
        {
            beganTeleport = true;
            var node = sc.agent.currentNode;
            Vector3 sourcePoint = transform.position;
            Vector3 targetPoint = sc.agent.Target;
            float teleportDistReduction = 0f;
            if (node != null && node.Next != null)
            {
                //if (Vector3.Distance(node.Next.Position, transform.position) < Vector3.Distance(transform.position, sc.agent.Target))
                {
                    targetPoint = node.Next.Position;
                }
                //cut corners (not teleport to a node if possible)

                /*if (Vector3.Distance(targetPoint, sourcePoint) < maxTeleportDistance - teleportDistReduction) {
                    teleportDistReduction = Vector3.Distance(targetPoint, sourcePoint);
                    if (node.Next.Next != null)
                    {
                        sourcePoint = node.Next.Position;
                        targetPoint = node.Next.Next.Position;
                    }
                    else {
                        Debug.Log("xD");
                        targetPoint = sc.agent.Target;
                    }
                }*/
            }
            float teleportDist = Mathf.Min(maxTeleportDistance, Vector3.Distance(targetPoint, sourcePoint)) - teleportDistReduction;
            Debug.DrawLine(sourcePoint, targetPoint, Color.magenta, 3f);
            Vector3 teleportDir = (targetPoint - sourcePoint).normalized;
            NavMeshHit endPos;
            if (!NavMesh.SamplePosition(transform.position + (teleportDir * teleportDist), out endPos, 1, NavMesh.AllAreas))
            {
                return;
            }
            Vector3 fxSpawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            Vector3 fxEndPos = transform.position + (teleportDir * teleportDist);
            fxEndPos = new Vector3(fxEndPos.x, fxEndPos.y + 2, fxEndPos.z);
            Instantiate(startTeleportPrefab, fxSpawnPos, Quaternion.identity);
            transform.position += teleportDir * teleportDist;
            _delayBetweenTeleports = 0f;
            Instantiate(endTeleportPrefab, fxEndPos, Quaternion.identity);

            //transform.position += teleportDir * teleportDist;

            _delayBetweenTeleports = 0f;
        }        
    }

    public void SwitchState(HealerBaseState state) {
        currentState = state;
        state.EnterState(this);
    }
}
