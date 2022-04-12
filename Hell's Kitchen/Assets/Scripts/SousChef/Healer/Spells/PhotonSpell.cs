using Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSpell : MonoBehaviour
{
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

    public float aoeDamage;

    public Transform target;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

    }

    float aoeDmgDelayAfterExplosion = 0.25f;
    private void Update()
    {
        if (target != null)
            transform.position = target.position;
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
            aoeDmgDelayAfterExplosion -= Time.deltaTime;
            if (aoeDmgDelayAfterExplosion < 0)
                gameObject.GetComponent<SphereCollider>().enabled = true;
        }

        selfDestructTimer -= Time.deltaTime;
        if (selfDestructTimer <= 0)
            Destroy(gameObject);
        transform.Rotate(new Vector3(0, 5, 0));
    }

    List<GameObject> hitList = new List<GameObject> ();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided with" + other.name);
        if (other.gameObject.TryGetComponent(out IKillable killable) && other.tag != "Player" && !hitList.Contains(other.gameObject)) {
            hitList.Add(other.gameObject);
            killable.TakeDamage(aoeDamage);
        }
    }
}
