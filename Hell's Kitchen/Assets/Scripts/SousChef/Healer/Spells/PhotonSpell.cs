using Common.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

public class PhotonSpell : MonoBehaviour
{
    float selfDestructTimer = 4f;

    public Transform spinners;
    public Transform explosions;
    public Transform AoE;
    public Transform lights;
    public Transform magicCircle;
    float spinStartDelay = 0.25f;
    float explosionDelay = 1f;

    float bigExplosionDelay = 1.5f;
    float centerSpeed = 12f;
    float arriveRadius = 0.75f;

    [SerializeField] float AoE_delayBetweenTicks;

    [SerializeField] GameObject bulletExplosion;
    [SerializeField] public float aoeDamage;
    [SerializeField] public float singleTargetDamage;

    public GameObject target;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    bool stopFollowing = false;
    float aoeDmgDelayAfterExplosion = 0.25f;
    private void Update()
    {
        if (target != null && !stopFollowing)
            transform.position = target.transform.position;
        
        spinStartDelay -= Time.deltaTime;
        
        if (spinStartDelay < 0 && spinners != null)
        {
            spinners.gameObject.SetActive(true);
            foreach (Transform chile in spinners)
                if ((transform.position - chile.position).magnitude > arriveRadius)
                    chile.position = Vector3.MoveTowards(chile.position, transform.position, centerSpeed * Time.deltaTime);
        }

        explosionDelay -= Time.deltaTime;
        if (explosionDelay < 0)
        {
            explosions.gameObject.SetActive(true);
            spinners.gameObject.SetActive(false);
        }

        bigExplosionDelay -= Time.deltaTime;
        if (bigExplosionDelay < 0 && !stopFollowing)
        {
            AoE.gameObject.SetActive(true);
            lights.gameObject.SetActive(false);
            aoeDmgDelayAfterExplosion -= Time.deltaTime;
            if (aoeDmgDelayAfterExplosion < 0)
            {
                if (!hitList.Contains(target) && target.TryGetComponent(out IKillable killable) && !target.CompareTag(Tags.Player) && !target.CompareTag(Tags.SousChef))
                {
                    hitList.Add(target);
                    killable.TakeDamage(singleTargetDamage);
                    StartCoroutine(ExecuteAfterTime(AoE_delayBetweenTicks, target));
                }
                stopFollowing = true;
                gameObject.GetComponent<SphereCollider>().enabled = true;
                magicCircle.gameObject.SetActive(true);
            }
        }

        selfDestructTimer -= Time.deltaTime;
        if (selfDestructTimer <= 0)
            Destroy(gameObject);

      //  if (!stopFollowing)
            transform.Rotate(new Vector3(0, 5, 0));
    }

    IEnumerator ExecuteAfterTime(float time, GameObject objToRemove)
    {
        yield return new WaitForSeconds(time);
        try
        {
            hitList.Remove(objToRemove);
        }
        catch (Exception e) {
            Debug.LogError(e.StackTrace);
        }
    }

    List<GameObject> hitList = new List<GameObject> ();
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collided with" + other.name);
        if (!hitList.Contains(other.gameObject) && other.gameObject.TryGetComponent(out IKillable killable) && !other.CompareTag(Tags.Player) && !other.CompareTag(Tags.SousChef)) {
            Destroy(Instantiate(bulletExplosion, other.transform.position, Quaternion.identity), 2);
            hitList.Add(other.gameObject);
            killable.TakeDamage(aoeDamage);
            StartCoroutine(ExecuteAfterTime(AoE_delayBetweenTicks, other.gameObject));
        }
    }
}
