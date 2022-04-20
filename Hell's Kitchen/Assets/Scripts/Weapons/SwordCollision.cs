using Common.Enums;
using Common.Interfaces;
using Player;
using UnityEngine;

namespace Weapons
{
    public class SwordCollision : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        private void OnCollisionEnter(Collision collision)
        {
            var animator = GameObject.FindWithTag(Tags.Player).GetComponentInChildren<Animator>();
            var isSwordAttack = animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimator.SwordAttack);
            if (isSwordAttack && collision.gameObject.TryGetComponent(out IKillable killable) && !collision.gameObject.CompareTag((Tags.SousChef)))
            {
                killable.TakeDamage(damage);
            }
        }
    }
}
