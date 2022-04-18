using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject knight;
    [SerializeField] private GameObject spark;

    private void Start() {
        Physics.IgnoreCollision(knight.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.TryGetComponent(out IKillable killable) && other.transform.tag != "Player" && other.transform.tag != "SousChef"){
            Destroy(Instantiate(spark, other.transform.position + Vector3.up, Quaternion.identity), 1);
            killable.TakeDamage(damage);
        }
    }
}
