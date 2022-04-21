using UnityEngine;
using Common.Enums;
using Common.Interfaces;
using Photon.Pun;

namespace Enemies
{
    public class BulletControl : MonoBehaviour
    {
        [SerializeField] private PhotonView photonView;
        [SerializeField] private float speed = 30f;
        [SerializeField] private float attackDamage = 10;
        
        private float time;
        public Vector3 direction;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            GetComponent<Rigidbody>().velocity = speed * direction;
            time = Time.time;
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;
            
            if (Time.time - time > 5)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!photonView.IsMine)
                return;

            if (col.gameObject.CompareTag(Tags.Player))
            {
                col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.AllBufferedViaServer, attackDamage);
            }
            
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
