using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxHealth;
    private float health;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Death();

    }
    private void Death()
    {
        if (health > 0)
            return;

        anim.SetTrigger("Die");
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}
