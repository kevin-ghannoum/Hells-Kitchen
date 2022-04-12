using System;
using Common.Enums;
using Enemies.Enums;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyPig : Enemy
    {
        [Header("Parameters")]
        [SerializeField]
        private float attackRate = 0.5f;

        [SerializeField]
        private float aggroRadius = 20.0f;

        private PlayerController _player;
        private float _lastAttack;


        [SerializeField]
        private float chargeSpeed = 10;
        [SerializeField]
        private float chargeTime = 3;
        private float currentChargeTime = 0;

        private Vector3 chargeDirection;
        private Rigidbody rb;


        private void Start()
        {
            _player = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            rb = GetComponent<Rigidbody>();
        }

        public override void Update()
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < aggroRadius)
            {
                agent.Target = _player.transform.position;

                if (currentChargeTime >= chargeTime)
                {
                    animator.SetBool("Charge", false);
                    rb.velocity = Vector3.zero;
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, (_player.transform.position - transform.position).normalized, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            GetComponent<PathfindingAgent>().enabled = false;
                            chargeDirection = (_player.transform.position - transform.position).normalized;
                            currentChargeTime = -1;
                            animator.SetBool("Charge", true);
                            transform.rotation = Quaternion.LookRotation(chargeDirection);
                        }
                    }
                }
                else
                {
                    currentChargeTime += Time.deltaTime;
                    if (currentChargeTime < 0)
                    {
                        rb.velocity = chargeDirection * chargeSpeed;
                    }
                    else
                    {
                        GetComponent<PathfindingAgent>().enabled = true;
                    }
                }
            }
        }
    }
}
