using Common.Enums;
using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(PathfindingAgent))]
public class SousChef : MonoBehaviour, IKillable
{
    [SerializeField] public PathfindingAgent agent;
    [SerializeField] public float maxHealth;
    [SerializeField] public GameObject targetEnemy;
    [SerializeField] public GameObject targetLoot;
    
    [SerializeField] public float followDistance;
    [SerializeField] public float attackRange;
    [SerializeField] public float searchRange; // must be bigger than follow Distance and attackRange
    
    public GameObject player;
    public float hitPoints;
    public float HitPoints => hitPoints;
    
    public UnityEvent Killed => throw new System.NotImplementedException();

    private PhotonView _photonView;
    
    public PhotonView PhotonView { get => _photonView; }

    GameObject gameStateManager;
    void Awake()
    {
        player = FindClosestPlayer();
        hitPoints = maxHealth;
        agent.Target = player.transform.position;
        agent.ArrivalRadius = followDistance;
        targetEnemy = null;
        targetLoot = null;
        _photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        player = FindClosestPlayer();
        bool enemyFound = false;

        // find new enemy target if none or out of range
        if(targetEnemy == null || (targetEnemy != null && Vector3.Distance(targetEnemy.transform.position, transform.position) > searchRange))
        {
            enemyFound = FindEnemy();
        }

        // if no ememy found around, find a new loot target
        // makes sure to deal with enemies nearby first
        if(!enemyFound)
        {
            print("looking for loot...");
            if(targetLoot == null || (targetLoot != null && Vector3.Distance(targetLoot.transform.position, transform.position) > searchRange))
            {
                FindLoot();
            }          
        }
    }

    // find closest enemy in range
    public bool FindEnemy()
    {
        // cast rays around souschef to search for enemies
        float distance = Mathf.Infinity;
        int numOfRays = 20;

        for(float i = 0; i < 360; i += 2 * Mathf.PI / numOfRays)
        {
            RaycastHit hit;
            Vector3 direction = new Vector3 (Mathf.Cos (i), 0, Mathf.Sin (i)).normalized;

            //Debug.DrawRay(transform.position + Vector3.up / 2, direction * searchRange, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up / 2, direction, out hit, searchRange))
            {
                if(hit.transform.gameObject.layer == Layers.Enemies)
                {
                    float currentDistance = (hit.transform.position - this.transform.position).magnitude;
                    if(currentDistance < distance) // closets enemy
                    {
                        targetEnemy = hit.transform.gameObject;
                        distance = currentDistance;
                    }
                }
            }
        }

        return (targetEnemy != null); // TRUE if found
    }

    public void FindLoot()
    {
        // cast rays around souschef to search for loot
        float distance = Mathf.Infinity;
        int numOfRays = 10;

        for(float i = 0; i < 360; i += Mathf.PI / numOfRays )
        {
            RaycastHit hit;
            Vector3 direction = new Vector3 (Mathf.Cos (i), 0, Mathf.Sin (i)).normalized * searchRange;

            //Debug.DrawRay(transform.position + Vector3.up / 2, direction, Color.yellow);
            if(Physics.Raycast(transform.position + Vector3.up / 2, direction, out hit, searchRange))
            {
                if(hit.transform.gameObject.CompareTag(Tags.Item))
                {
                    float currentDistance = (hit.transform.position - this.transform.position).magnitude;
                    if(currentDistance < distance) // closest loot
                    {
                        targetLoot = hit.transform.gameObject;
                        distance = currentDistance;
                    }
                }
            }
        }
    }

    public float GetDistanceToEnemy()
    {
        if (targetEnemy == null)
            return float.NaN;
        else
            return (targetEnemy.transform.position - transform.position).magnitude;
    }

    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;
                Debug.Log("flee point = " + result);
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public Vector3 fleePoint;
    public bool foundFleePoint = false;
    public void Flee()
    {
        if (!foundFleePoint)
        {
            foundFleePoint = RandomPoint(transform.position, 90f, out fleePoint);
        }
        else if (foundFleePoint)
        {
            if ((transform.position - fleePoint).magnitude <= followDistance)
            {
                Debug.Log("reset flee point");
                foundFleePoint = false;
                return;
            }
            if (agent.Target != fleePoint)
                agent.Target = fleePoint;
            if (agent.ArrivalRadius != followDistance)
            {
                agent.ArrivalRadius = followDistance;
            }
        }
    }

    public void faceTargetEnemy()
    {
        if (targetEnemy != null)
            transform.rotation = Quaternion.LookRotation((targetEnemy.transform.position - transform.position).normalized);
    }

    public void facePlayer()
    {
        if (player != null)
            transform.rotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        //gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation((targetEnemy.transform.position - transform.position).normalized));
        //transform.LookAt(targetEnemy.transform.position);
    }
    public float GetDistanceToPlayer()
    {
        return (player.transform.position - transform.position).magnitude;
    }

    public void BasicAttack()
    {
        /*
         *  Healer fight style will be flee+longrange spells only, it probably won't use this method.
         *  a simple melee, single-target, low-cooldown, high damage attack (enough to kill within 1-3 hits)
         *  this could be used by Fighter and Assassin when their more powerful spells are on cooldown.
         *  we will see what fits better as the game evolves
        */
    }

    public bool isLowHP()
    {
        return ((hitPoints / maxHealth) * 100) < 60;
    }
    
    [PunRPC]
    public void TakeDamage(float dmg)
    {
        Debug.Log("'" + gameObject.name + "' took " + dmg + " damage");
        hitPoints -= dmg;
        if (_photonView.IsMine)
        {
            if (hitPoints < 0)
                Die();
        }
    }
    
    public void Die()
    {
        Debug.Log("'" + gameObject.name + "' got kilt");
        PhotonNetwork.Destroy(gameObject);
    }
    
    public GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.Player);
        float closestDist = float.MaxValue;
        GameObject closestPlayer = null;
        foreach (var player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestPlayer = player;
            }
        }
        return closestPlayer;
    }
}
