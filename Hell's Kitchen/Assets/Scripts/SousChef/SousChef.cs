using Common.Enums;
using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] AudioClip deathSound;

    public GameObject player;
    public float hitPoints;
    public float HitPoints => hitPoints;

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

    float takeDamageDelay = 1f;
    float _takeDamageDelay = 0f;
    void Update()
    {
        //player = FindClosestPlayer();
        bool enemyFound = false;

        // find new enemy target if none or out of range
        if (targetEnemy == null || (targetEnemy != null && Vector3.Distance(targetEnemy.transform.position, transform.position) > searchRange))
        {
            enemyFound = FindEnemy();
        }

        // if no ememy found around, find a new loot target
        // makes sure to deal with enemies nearby first
        if (!enemyFound)
        {
            //print("looking for loot...");
            if (targetLoot == null || (targetLoot != null && Vector3.Distance(targetLoot.transform.position, transform.position) > searchRange))
            {
                FindLoot();
            }
        }
        if (_takeDamageDelay < takeDamageDelay)
        {
            _takeDamageDelay += Time.deltaTime;
        }
    }

    // find closest enemy in range
    public bool FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange, 1 << 7);    //enemy layer mask
        colliders = colliders.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();
        if (colliders.Length != 0)
            targetEnemy = colliders[0].gameObject;
        //else
        //    targetEnemy = null;

        // enemy found, return true
        if (targetEnemy != null)
        {
            return true;
        }

        return false;
    }

    public void FindLoot()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange, 1 << 13);    //items layer mask
        colliders = colliders.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();
        bool found = false;
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Item")
            {
                targetLoot = collider.gameObject;
                found = true;
            }
        }
        if (!found)
            targetLoot = null;
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
        // Invulnerability after getting hit
        if (_takeDamageDelay < takeDamageDelay)
            return;

        _takeDamageDelay = 0;
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
        _photonView.RPC(nameof(PlayDeathSoundRPC), RpcTarget.All);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    private void PlayDeathSoundRPC()
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
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

    public float GetPlayerHP() {
        return player.GetComponent<Player.PlayerHealth>().internalHealth;
    }
    public bool IsPlayerLowHP()
    {
        return ((GetPlayerHP() / Common.GameStateData.playerMaxHitPoints) * 100) < 60;
    }

    public GameObject FindLowHealthPlayer() {
        GameObject[] players = GameObject.FindGameObjectsWithTag(Tags.Player);
        players = players.OrderBy(p => p.GetComponent<Player.PlayerHealth>().internalHealth).ToArray();
        if (players.Length == 0)
            return null;
        //Debug.Log("lowesthp:" + players[0].GetComponent<Player.PlayerHealth>().internalHealth + ", other: " + players[1].GetComponent<Player.PlayerHealth>().internalHealth);
        if ((players[0].GetComponent<Player.PlayerHealth>().internalHealth / Common.GameStateData.playerMaxHitPoints *100) >= 60)
            return null;
        return players[0];
    }
}
