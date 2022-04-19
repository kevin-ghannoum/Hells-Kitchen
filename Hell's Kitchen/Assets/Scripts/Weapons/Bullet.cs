using System;
using Common.Enums;
using Common.Interfaces;
using Enemies;
using Photon.Pun;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject bulletHit;
        [SerializeField] private PhotonView photonView;
        public float Damage { get; set; }

        private void OnCollisionEnter(Collision collision)
        {
            var obj = collision.gameObject; // TODO Disable friendly fire with sous-chef
            if (!obj.CompareTag(Tags.Player))
            {
                var hitFx = Instantiate(bulletHit, collision.contacts[0].point, Quaternion.identity);
                Destroy(hitFx, 3.0f);
                
                if (photonView.IsMine)
                {
                    var enemy = obj.GetComponent<Enemy>();
                    enemy?.PhotonView.RPC(nameof(Enemy.TakeDamage), RpcTarget.All, Damage);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
