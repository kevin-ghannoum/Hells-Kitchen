using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Transform))]
public class HealerStateManager : MonoBehaviour
{
    public HealerBaseState currentState;
    public HealerAttackState attackState = new HealerAttackState();
    public HealerMoveToTargetState moveToTarget = new HealerMoveToTargetState();
    public HealerFollowState followState = new HealerFollowState();
    public HealerLootState lootState = new HealerLootState();
    public HealerHealState healState = new HealerHealState();

    public Animator animator;
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
    float delayBetweenTeleports = 0.75f;
    float _delayBetweenTeleports = 0f;
    bool canTeleport() => _delayBetweenTeleports >= delayBetweenTeleports;
    bool shouldTeleport() => !sc.agent.standStill && sc.agent.IsMoving && sc.agent.Target != null &&
                             (Vector3.Distance(transform.position, sc.agent.Target) > 15);

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
            intersectionPoints = new Vector3[] {Vector3.zero, Vector3.zero};
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
        var lowhpPlayer = sc.FindLowHealthPlayer();
        if (lowhpPlayer != null)
            sc.player = lowhpPlayer;
        else
            sc.player = sc.FindClosestPlayer();

        var photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
            return;

        _attackCooldown += Time.deltaTime;
        animator.SetBool("isWalking", sc.agent.IsMoving);
        animator.SetBool("isRunning", sc.agent.IsMoving);
        currentState.UpdateState(this);

        _delayBetweenTeleports += Time.deltaTime;
        Pathfinding.PathNode currentNode = null;
        
        if (canTeleport() && shouldTeleport())
        {
            var node = sc.agent.CurrentNode;
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
                            if (Vector3.Distance(p0, transform.position) < maxTeleportDistance &&
                                Vector3.Distance(p1, transform.position) > maxTeleportDistance)
                            {
                                pointOfIntersection = intersectionPoints[0];
                                currentNode = node.Next;

                                sc.agent.RecalculatePath();
                                break;

                            }
                        }
                        node = node.Next;
                    }
                }
                Debug.DrawRay(pointOfIntersection, Vector3.up * 100f, Color.magenta, 0.25f);
            }
            else
            {
                if (node == null)
                {
                    return;
                }
                
                pointOfIntersection = transform.position + (node.Position - transform.position).normalized * maxTeleportDistance;
            }

            Debug.DrawLine(transform.position, pointOfIntersection, Color.magenta, 3f);
            NavMeshHit endPos;
            if (!NavMesh.SamplePosition(pointOfIntersection, out endPos, 1, NavMesh.AllAreas))
            {
                Debug.DrawRay(transform.position, Vector3.up * 100f, Color.blue, 20f);
                Debug.DrawRay(pointOfIntersection, Vector3.up * 100f, Color.red, 20f);
                Debug.DrawLine(transform.position, pointOfIntersection, Color.cyan, 20f);
                return;
            }

            Vector3 fxEndPos = endPos.position; //transform.position + (teleportDir * teleportDist);
            fxEndPos = new Vector3(fxEndPos.x, fxEndPos.y + 2, fxEndPos.z);
            Instantiate(startTeleportPrefab, fxSpawnPos, Quaternion.identity);
            Vector3 endSpawnPos = new Vector3(endPos.position.x, transform.position.y, endPos.position.z);
            transform.position = endSpawnPos; //+= teleportDir * teleportDist;
            _delayBetweenTeleports = 0f;

            photonView.RPC(nameof(SpawnTeleportMagic), RpcTarget.All, fxEndPos);

            _delayBetweenTeleports = 0f;
        }
    }

    public void SwitchState(HealerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    [PunRPC]
    public void SpawnTeleportMagic(Vector3 position)
    {
        Instantiate(endTeleportPrefab, position, Quaternion.identity);
    }

    public bool IsInLootState(){
        return currentState == lootState;
    }

    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(shouldShowMagicCircle);
        }
        else if (stream.IsReading)
        {
            shouldShowMagicCircle = (bool) stream.ReceiveNext();
        }
    }*/
}
