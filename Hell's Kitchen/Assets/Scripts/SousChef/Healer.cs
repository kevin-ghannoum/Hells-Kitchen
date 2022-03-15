using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : SousChef
{
    float cd_photon = 3f;
    float _cd_photon = 0f;
    float photonDamage = 40;
    private void Update()
    {
        _cd_photon += Time.deltaTime;
        if (currentEnemyTarget == null)
        {
            FindEnemy();
            Follow();
        }
        else
        {
            Attack();
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }


    public void Attack()
    {

        //later, need to account for situations where the current room size does not allow to go that far;
        //in that case, go as far away as possible n begin casting spell
        if (getDistanceToEnemy() > attackRange + 2)
        {
            //Debug.Log("move" + getDistanceToEnemy());
            MoveToEnemy();
        }
        else if (getDistanceToEnemy() < attackRange - 2)
        {
            //Debug.Log("flee" + getDistanceToEnemy());
            Flee();
            
        }
        else
        {
            if (_cd_photon >= cd_photon)
            {
                Spell_Photon();
                _cd_photon = 0;
            }
            
            //to do: probs want to separate spells into diff components/prefabs, will see later
        }
        
    }

    public void Teleport()
    {

    }
    public void Spell_Photon()
    {
        Debug.Log("attacking");
        Debug.DrawLine(transform.position, currentEnemyTarget.position, Color.red, 3);
        //temp example
        //spells will probs be prefabs and inflict damage if collider hits the enemy
        currentEnemyTarget.GetComponent<Character>().TakeDamage(photonDamage);
    }
    public void Spell_Heal()
    {

    }
}
