using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnTriggerEnter(Collider other){
        Debug.Log("collide");
        if(other.gameObject.TryGetComponent(out IKillable killable) && other.tag != "Player" && other.tag != "SousChef"){
            Debug.Log(other.name);
            killable.TakeDamage(damage);
        }
    }
}
