using Common.Interfaces;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class Sword : WeaponPickup, IWeapon
    {
        [SerializeField] private float price =  10f;
        public override float Price { get => price; }
        public override void Use(InputAction.CallbackContext callbackContext)
        {
            base.Use(callbackContext);
            playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }
    }
}

