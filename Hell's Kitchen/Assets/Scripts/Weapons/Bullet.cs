using System;
using Common.Enums;
using Common.Interfaces;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject bulletHit;
        public float Damage { get; set; }

        private void OnCollisionEnter(Collision collision)
        {
            var obj = collision.gameObject;
            if (!obj.CompareTag(Tags.Player) && obj.TryGetComponent(out IKillable killable))
            {
                killable.TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
