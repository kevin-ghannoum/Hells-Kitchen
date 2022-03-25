using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PathfindingAgent))]
public class SousChef : MonoBehaviour
{
    [SerializeField] public float followDistance;
    [SerializeField] public float attackRange;
    [SerializeField] public float searchRange; // must be bigger than follow Distance and attackRange
    [SerializeField] public GameObject player;
    [SerializeField] public PathfindingAgent agent;
    public Character character { get; set; }
    [SerializeField] public Transform targetEnemy;
    // public Transform currentEnemyTarget { get; set; }
    [SerializeField] public Transform targetLoot;
    // public Transform currentLootTarget { get; set; }

    void Start()
    {
        character = gameObject.GetComponent<Character>();
        agent.Target = player.transform.position;
        agent.ArrivalRadius = followDistance;
    }

    void Update(){
        bool enemyFound = false;

        // if the souschef does not have an enemy target or if the former target is out of search range
        // find a new enemy target
        if(targetEnemy == null || (targetEnemy != null && Vector3.Distance(targetEnemy.position, transform.position) > searchRange)){
            enemyFound = FindEnemy();
        }

        // if no ememy found around, find a new loot target
        // makes sure to deal with enemies nearby first
        if(!enemyFound){
            if(targetLoot == null || (targetLoot != null && Vector3.Distance(targetLoot.position, transform.position) > searchRange)){
                //update loot target
                FindLoot();
            }          
        }
    }

    public void Follow()
    {
        if ((player.transform.position - transform.position).magnitude > followDistance)
        {
            transform.LookAt(player.transform);
            transform.position += transform.forward * character.speed * Time.deltaTime;
        }
    }

    public void Search()
    {
    }

    public void PickUp()
    {
    }

    public bool FindEnemy()
    {
        //code to loop through enemies within vision and find closest 1
        // currentEnemyTarget = exampleEnemy;
        //if no enemies within vision, set currentEnemyTarget = null;

        // cast rays around souschef to search for enemies
        float distance = Mathf.Infinity;
        for(int i = 0; i < 360; i += 5 ){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.zero + new Vector3(i, 0, 0), out hit, searchRange)){
                if(hit.transform.tag == "Enemy"){
                    float currentDistance = (targetEnemy.position - this.transform.position).magnitude;
                    if(currentDistance < distance){
                        // find the enemy with closest distance
                        targetEnemy = hit.transform;
                        distance = currentDistance;
                    }
                }
            }
        }

        // enemy found, return true
        if(targetEnemy != null){
            return true;
        }

        return false;
    }

    public void FindLoot() {
        // currentLootTarget = exampleLoot;

        // cast rays around souschef to search for loots
        float distance = Mathf.Infinity;
        for(int i = 0; i < 360; i += 5 ){
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.zero + new Vector3(i, 0, 0), out hit, searchRange)){
                if(hit.transform.tag == "Enemy"){
                    float currentDistance = (targetEnemy.position - this.transform.position).magnitude;
                    if(currentDistance < distance){
                        // find the loot with closest distance
                        targetEnemy = hit.transform;
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
            return (targetEnemy.position - transform.position).magnitude;
    }

    public void MoveToEnemy()
    {
        transform.LookAt(targetEnemy);
        transform.position += transform.forward * character.speed * Time.deltaTime;
    }

    public void Flee()
    {
        /*
         need flee() for:
         1. sous chefs will want to flee when low HP or if skills are on cooldown
         2. Healer, wants to go far before attacking (to lower chances of enemy interrupting spells)
        
        we can probably just make it run away from the closest enemy
         */
        transform.rotation = Quaternion.LookRotation(transform.position - targetEnemy.position);
        transform.position += transform.forward * character.speed * Time.deltaTime;
    }

    //these are for healer mostly, to be at a 
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
}
