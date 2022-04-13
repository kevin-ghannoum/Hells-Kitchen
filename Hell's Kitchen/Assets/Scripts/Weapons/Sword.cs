using Common.Interfaces;
using Player;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class Sword : WeaponPickup, IWeapon
    {
        public override void Use(InputAction.CallbackContext callbackContext)
        {
            base.Use(callbackContext);
            playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }
    }
}

