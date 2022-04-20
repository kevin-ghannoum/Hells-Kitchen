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
            var obj = collision.gameObject; // TODO Disable friendly fire with sous-chef
            if (!obj.CompareTag(Tags.Player))
            {
                Common.GameStateManager.Instance.playerCurrentStamina += 0.5f;
                obj.GetComponent<IKillable>()?.TakeDamage(Damage);
                Destroy(gameObject);
                var hitFx = Instantiate(bulletHit, collision.contacts[0].point, Quaternion.identity);
                Destroy(hitFx, 3.0f);
            }
        }
    }
}
