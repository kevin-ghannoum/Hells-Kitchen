using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;    //can get rid of this parameter, use pathfindingAgent's max velocity instead xd

    public float health { get ; set; }
    private void Awake()
    {
        health = maxHealth;
    }
    public void TakeDamage(float dmg)
    {
        Debug.Log("'" + gameObject.name + "' took " + dmg + " damage");
        health -= dmg;
        if (health < 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("'" + gameObject.name + "' got kilt");
        Destroy(gameObject);
    }

    public bool isLowHP() {
        return ((health/maxHealth)*100) < 60;
    }
}
