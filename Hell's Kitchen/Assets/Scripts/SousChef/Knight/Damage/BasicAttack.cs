using Common.Enums;
using Common.Interfaces;
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
        if(other.gameObject.TryGetComponent(out IKillable killable) && !other.transform.CompareTag(Tags.Player) && !other.transform.CompareTag(Tags.SousChef))
        {
            Destroy(Instantiate(spark, other.transform.position + Vector3.up, Quaternion.identity), 1);
            killable.TakeDamage(damage);
        }
    }
}
