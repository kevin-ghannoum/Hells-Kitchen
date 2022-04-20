using UnityEngine;
using Common.Enums;
using Common.Interfaces;
using Photon.Pun;

namespace Enemies
{
    public class BulletControl : Enemy
    {
        private float speed = 10f;
        [SerializeField] private float attackDamage = 10;
        private float time;
        public Vector3 direction;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            time = Time.time;
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            transform.position += direction * speed * Time.deltaTime;

            if (Time.time - time > 5)
            {
                deleteBullet(this.gameObject);
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!photonView.IsMine)
                return;

            if (col.gameObject.CompareTag(Tags.Player))
            {
                col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, attackDamage);
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

        [PunRPC]
        private void deleteBullet(GameObject bullet)
        {
            PhotonView.Destroy(this.gameObject);
        }
    }
}