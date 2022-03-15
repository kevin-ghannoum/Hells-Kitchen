using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class SousChef : MonoBehaviour
{
    [SerializeField] protected float followDistance;
    [SerializeField] protected float attackRange;
    [SerializeField] protected GameObject player;
    protected Character character;
    void Start()
    {
        character = gameObject.GetComponent<Character>();
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

    [SerializeField] Transform exampleEnemy;
    protected Transform currentEnemyTarget;
    public void FindEnemy()
    {
        //code to loop through enemies within vision and find closest 1
        currentEnemyTarget = exampleEnemy;

        //if no enemies within vision, set currentEnemyTarget = null;
    }
    public float getDistanceToEnemy()
    {
        if (currentEnemyTarget == null)
            return float.NaN;
        else
            return (currentEnemyTarget.position - transform.position).magnitude;
    }

    public void MoveToEnemy()
    {
        transform.LookAt(currentEnemyTarget);
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
        transform.rotation = Quaternion.LookRotation(transform.position - currentEnemyTarget.position);
        transform.position += transform.forward * character.speed * Time.deltaTime;
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
