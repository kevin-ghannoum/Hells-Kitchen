using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : MonoBehaviour
{
    [SerializeField] float healthIncrement;
    [SerializeField] float healTickDelay;
    [SerializeField] GameObject onHealAnimationPrefab;

    [SerializeField] float AoEDuration;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
            Common.GameStateManager.Instance.playerCurrentHitPoints += healthIncrement;
        }
        else if (other.gameObject.tag == "SousChef")
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

        if (other.gameObject.tag == "Player") {
            if (playerRegenTick >= healTickDelay) {
                Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
                Common.GameStateManager.Instance.playerCurrentHitPoints += healthIncrement;
                playerRegenTick = 0;
            }
            
        }
        else if (other.gameObject.tag == "SousChef") {
            if (ssRegenTick >= healTickDelay)
            {
                Destroy(Instantiate(onHealAnimationPrefab, other.transform.position, Quaternion.identity), 2);
                other.GetComponent<SousChef>().hitPoints += healthIncrement;
                ssRegenTick = 0;
            }
            
        }
    }
}
