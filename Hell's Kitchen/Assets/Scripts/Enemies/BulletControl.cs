using UnityEngine;
using Common.Enums;
using Common.Interfaces;

public class BulletControl : MonoBehaviour
{
    private float speed = 10f;
    [SerializeField] private float attackDamage = 10;
    public Vector3 direction;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(Tags.Player))
        {
            col.gameObject.GetComponent<IKillable>().TakeDamage(attackDamage);
            Destroy(this.gameObject);
        }
    }
}