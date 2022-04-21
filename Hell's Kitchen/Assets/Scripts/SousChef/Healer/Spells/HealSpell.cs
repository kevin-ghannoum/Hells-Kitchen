using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Enums;
using Player;
using UnityEngine;

public class HealSpell : MonoBehaviour
{
    [SerializeField] float healthIncrement;
    [SerializeField] float healTickDelay;
    [SerializeField] GameObject onHealAnimationPrefab;

    [SerializeField] float AoEDuration;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
            other.gameObject.GetComponent<PlayerHealth>().HitPoints += healthIncrement;
        }
        else if (other.gameObject.CompareTag(Tags.SousChef))
        {
            Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
            other.GetComponent<SousChef>().hitPoints += healthIncrement;
        }
    }

    float playerRegenTick = 0f;
    float ssRegenTick = 0f;
    private void Update()
    {
        playerRegenTick += Time.deltaTime;
        ssRegenTick += Time.deltaTime;
        AoEDuration -= Time.deltaTime;
        if (AoEDuration <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            if (playerRegenTick >= healTickDelay)
            {
                Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
                //other.gameObject.GetComponent<PlayerHealth>().HitPoints += healthIncrement;
                other.gameObject.GetComponent<PlayerHealth>().PhotonView.RPC("IncreaseMyHP", Photon.Pun.RpcTarget.All, healthIncrement);
                playerRegenTick = 0;
            }
        }
        else if (other.gameObject.CompareTag(Tags.SousChef))
        {
            if (ssRegenTick >= healTickDelay)
            {
                Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
                other.GetComponent<SousChef>().hitPoints += healthIncrement;
                ssRegenTick = 0;
            }
        }
    }
}
