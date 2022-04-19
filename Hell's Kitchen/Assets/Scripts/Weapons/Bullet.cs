using System;
using Common.Enums;
using Common.Interfaces;
using Photon.Pun;
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
                obj.GetComponent<IKillable>()?.TakeDamage(Damage);
                PhotonNetwork.Destroy(gameObject);
                var hitFx = Instantiate(bulletHit, collision.contacts[0].point, Quaternion.identity);
                Destroy(hitFx, 3.0f);
            }
        }
    }
}
