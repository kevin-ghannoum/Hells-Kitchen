using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonSpell : MonoBehaviour
{
    float damage;

    float selfDestructTimer = 4f;
    
    private void Update()
    {
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
