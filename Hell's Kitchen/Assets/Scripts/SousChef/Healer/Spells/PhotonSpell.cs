using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSpell : MonoBehaviour
{
    float damage;

    float selfDestructTimer = 5f;

    public Transform spinners;
    public Transform explosions;
    public Transform AoE;
    public Transform lights;
    float spinStartDelay = 0.25f;
    float explosionDelay = 1f;

    float bigExplosionDelay = 1.5f;
    float centerSpeed = 12f;
    float arriveRadius = 0.75f;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

    }
    private void Update()
    {
        spinStartDelay -= Time.deltaTime;
        if (spinStartDelay < 0 && spinners != null) {
            spinners.gameObject.SetActive(true);
            foreach (Transform chile in spinners)
                if ((transform.position - chile.position).magnitude > arriveRadius)
                    chile.position = Vector3.MoveTowards(chile.position, transform.position, centerSpeed * Time.deltaTime);
        }

        explosionDelay -= Time.deltaTime;
        if (explosionDelay < 0) {
            explosions.gameObject.SetActive(true);
            spinners.gameObject.SetActive(false);
        }

        bigExplosionDelay -= Time.deltaTime;
        if (bigExplosionDelay < 0) {
            AoE.gameObject.SetActive(true);
            lights.gameObject.SetActive(false);
        }

        selfDestructTimer -= Time.deltaTime;
        if (selfDestructTimer <= 0)
            Destroy(gameObject);
        transform.Rotate(new Vector3(0, 5, 0));
    }
    private void OnCollisionEnter(Collision collision)
    {

        /*if (collision.gameObject.tag == "Player") {

        }*/
    }
}
