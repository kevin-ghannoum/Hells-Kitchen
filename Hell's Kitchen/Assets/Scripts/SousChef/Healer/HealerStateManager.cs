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
    //bool shouldTeleport() => !sc.agent.standStill && sc.agent.Target != null && (Vector3.Distance(transform.position, sc.agent.Target) > 15);

    //implemented from https://web.archive.org/web/20060909012810/http://local.wasp.uwa.edu.au/~pbourke/geometry/sphereline/
    public bool lineSphereIntesection(Vector3 p0, Vector3 p1, Vector3 center, float radius, out Vector3[] intersectionPoints)
    {
        Vector3 P = new Vector3(); //delta p

        //get delta p line segment
        P.x = p1.x - p0.x;
        P.z = p1.z - p0.z;

        //(x-center_x)^2+(y-center_y)^2+(z-center_z)^2=r^2
        float x = P.x * P.x + P.z * P.z;
        float y = 2 * (P.x * (p0.x - center.x) + P.z * (p0.z - center.z));
        float z = center.x * center.x + center.z * center.z;
        z += p0.x * p0.x + p0.z * p0.z;
        z -= 2 * (center.x * p0.x + center.z * p0.z);
        z -= radius * radius;

        //b^2 - 4*a*c
        float quadraticSqrtPart = Mathf.Pow(y, 2) - 4 * x * z;
        if (quadraticSqrtPart < 0 || Mathf.Abs(x) < float.Epsilon)
        {
            intersectionPoints = new Vector3[] { Vector3.zero, Vector3.zero };
            return false;
        }
        float u0 = (-y + Mathf.Sqrt(quadraticSqrtPart)) / (2 * x);
        float u1 = (-y - Mathf.Sqrt(quadraticSqrtPart)) / (2 * x);
        intersectionPoints = new Vector3[2];
        intersectionPoints[0] = new Vector3(p0.x + u0 * (p1.x - p0.x), 0, p0.z + u0 * (p1.z - p0.z));
        intersectionPoints[1] = new Vector3(p0.x + u1 * (p1.x - p0.x), 0, p0.z + u1 * (p1.z - p0.z));
        return true;
    }
    private void Update()
    {


        _attackCooldown += Time.deltaTime;
        gameObject.GetComponent<Animator>().SetBool("isWalking", sc.agent.IsMoving());
        gameObject.GetComponent<Animator>().SetBool("isRunning", sc.agent.IsMoving());
        currentState.UpdateState(this);

        _delayBetweenTeleports += Time.deltaTime;
        Pathfinding.PathNode currentNode = null;
        //return;
        if (canTeleport() && shouldTeleport())// && !beganTeleport)
        {
            beganTeleport = true;
            var node = sc.agent.currentNode;
            Vector3 fxSpawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            Vector3 pointOfIntersection = Vector3.zero;
            if (Vector3.Distance(transform.position, sc.agent.Target) < maxTeleportDistance)
            {
                pointOfIntersection = transform.position + (sc.agent.Target - transform.position).normalized * maxTeleportDistance;
            }
            else if (node != null && node.Next != null)
            {
                Vector3 p0 = node.Position;
                Vector3 p1 = node.Next.Position;
                if (Vector3.Distance(p0, transform.position) > maxTeleportDistance)
                {
                    pointOfIntersection = transform.position + (p0 - transform.position).normalized * maxTeleportDistance;
                }
                else
                {
                    while (node.Next != null)
                    {
                        p0 = node.Position;
                        p1 = node.Next.Position;
                        Debug.DrawLine(p0, p1, Color.cyan);
                        Vector3[] intersectionPoints;
                        if (lineSphereIntesection(p0, p1, transform.position, maxTeleportDistance, out intersectionPoints))
                        {
                            Debug.DrawLine(transform.position, intersectionPoints[0], Color.blue);
                            if (Vector3.Distance(p0, transform.position) < maxTeleportDistance && Vector3.Distance(p1, transform.position) > maxTeleportDistance)
                            {
                                pointOfIntersection = intersectionPoints[0];
                                currentNode = node.Next;

                                sc.agent.UpdatePath(node.Next);
                                break;

                            }
                        }
                        node = node.Next;
                    }
                }
                Debug.DrawRay(pointOfIntersection, Vector3.up * 100f, Color.magenta, 0.25f);
                /*Debug.Log("le point of intersect:" + pointOfIntersection);
                Debug.Log("transform.position:" + transform.position);*/
                //return;
            }
            else
            {
                if (node == null)
                {
                    Debug.Log("1xD");
                    return;
                }
                else
                {
                    Debug.Log("2xD");
                    pointOfIntersection = transform.position + (node.Position - transform.position).normalized * maxTeleportDistance;
                }
            }
            float teleportDist = Mathf.Min(maxTeleportDistance, Vector3.Distance(pointOfIntersection, transform.position));
            Debug.DrawLine(transform.position, pointOfIntersection, Color.magenta, 3f);
            NavMeshHit endPos;
            if (!NavMesh.SamplePosition(pointOfIntersection, out endPos, 1, NavMesh.AllAreas))
            {
                Debug.DrawRay(transform.position, Vector3.up * 100f, Color.blue, 20f);
                Debug.DrawRay(pointOfIntersection, Vector3.up * 100f, Color.red, 20f);
                Debug.DrawLine(transform.position, pointOfIntersection, Color.cyan, 20f);
                Debug.Log("oob, cancel tp xd");
                return;
            }
            if (currentNode != null)
            {
                ///xddddd
            }

            Vector3 fxEndPos = endPos.position;//transform.position + (teleportDir * teleportDist);
            fxEndPos = new Vector3(fxEndPos.x, fxEndPos.y + 2, fxEndPos.z);
            Instantiate(startTeleportPrefab, fxSpawnPos, Quaternion.identity);
            Vector3 endSpawnPos = new Vector3(endPos.position.x, transform.position.y, endPos.position.z);
            transform.position = endSpawnPos;//+= teleportDir * teleportDist;
            _delayBetweenTeleports = 0f;
            Instantiate(endTeleportPrefab, fxEndPos, Quaternion.identity);

            //transform.position += teleportDir * teleportDist;

            _delayBetweenTeleports = 0f;
        }
    }

    public void SwitchState(HealerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maxTeleportDistance);
    }
}
