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

    List<AudioSource> audios;
    [SerializeField] float AoE_delayBetweenTicks;

    [SerializeField] GameObject bulletExplosion;
    [SerializeField] public float aoeDamage;
    [SerializeField] public float singleTargetDamage;

    public GameObject target;

    float explosionSoundDelay = 0.5f;
    float _miniExplosionSfxDelay = 0f;
    float miniExplosionSfxDelay = 0.10f;
    private void Start()
    {
        audios = new List<AudioSource>(gameObject.GetComponents<AudioSource>());
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    bool stopFollowing = false;
    float aoeDmgDelayAfterExplosion = 0.25f;
    bool playedExplosionSound = false;
    float stopMiniSfx = 1f;
    bool playedBlazeSound = false;
    List<AudioSource> audioPlaying = new List<AudioSource>();
    private void Update()
    {
        if (target != null && !stopFollowing)
            transform.position = target.transform.position;
        
        spinStartDelay -= Time.deltaTime;
        
        if (spinStartDelay < 0 && spinners != null)
        {
            if (!playedBlazeSound)
            {
                playedBlazeSound = true;
                audios[audios.Count - 1].Play();
            }
            spinners.gameObject.SetActive(true);
            foreach (Transform chile in spinners)
                if ((transform.position - chile.position).magnitude > arriveRadius)
                    chile.position = Vector3.MoveTowards(chile.position, transform.position, centerSpeed * Time.deltaTime);
        }

        explosionDelay -= Time.deltaTime;
        if (explosionDelay < 0)
        {
            explosionSoundDelay -= Time.deltaTime;
            if (explosionSoundDelay <= 0 && !playedExplosionSound) {
                audios[0].Play();
                playedExplosionSound = true;
            }
            explosions.gameObject.SetActive(true);
            spinners.gameObject.SetActive(false);
        }

        bigExplosionDelay -= Time.deltaTime;

        _miniExplosionSfxDelay += Time.deltaTime;
        if (_miniExplosionSfxDelay >= miniExplosionSfxDelay && bigExplosionDelay < 0 && stopMiniSfx >= 0)
        {
            int randomindex = UnityEngine.Random.Range(1, audios.Count - 1);
            audioPlaying.Add(audios[randomindex]);
            audios[randomindex].Play();
            _miniExplosionSfxDelay = 0f;
            stopMiniSfx -= Time.deltaTime;
        }
        if (bigExplosionDelay < 0 && !stopFollowing)
        {

            AoE.gameObject.SetActive(true);
            lights.gameObject.SetActive(false);
            aoeDmgDelayAfterExplosion -= Time.deltaTime;
            if (aoeDmgDelayAfterExplosion < 0)
            {
                if (target != null && !hitList.Contains(target) && target.TryGetComponent(out IKillable killable) && !target.CompareTag(Tags.Player) && !target.CompareTag(Tags.SousChef))
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
        if (selfDestructTimer <= 0.5f)
        {
            foreach (var a in audioPlaying) {
                a.volume = a.volume / 1.05f;
            }
            AoE.gameObject.SetActive(false);
            lights.gameObject.SetActive(false);
            gameObject.GetComponent<SphereCollider>().enabled = false;
            magicCircle.gameObject.SetActive(false);
            explosions.gameObject.SetActive(false);
            spinners.gameObject.SetActive(false);
            Destroy(gameObject,3);
        }

        transform.Rotate(new Vector3(0, 5, 0));
    }

    IEnumerator ExecuteAfterTime(float time, GameObject objToRemove)
    {
        yield return new WaitForSeconds(time);
        try
        {
            hitList.Remove(objToRemove);
        }
        catch (Exception e)
        {
            Debug.LogError(e.StackTrace);
        }
    }

    List<GameObject> hitList = new List<GameObject> ();
    private void OnTriggerStay(Collider other)
    {
        if (!hitList.Contains(other.gameObject) && other.gameObject.TryGetComponent(out IKillable killable) && !other.CompareTag(Tags.Player) && !other.CompareTag(Tags.SousChef))
        {
            Destroy(Instantiate(bulletExplosion, other.transform.position, Quaternion.identity), 2);
            hitList.Add(other.gameObject);
            killable.TakeDamage(aoeDamage);
            StartCoroutine(ExecuteAfterTime(AoE_delayBetweenTicks, other.gameObject));
        }
    }
}
