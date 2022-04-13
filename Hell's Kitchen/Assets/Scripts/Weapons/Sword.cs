using Player;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class Sword : WeaponPickup, IWeapon
    {
        protected override void Use(InputAction.CallbackContext callbackContext)
        {
            base.Use(callbackContext);
            playerAnimator.SetTrigger(PlayerAnimator.SwordAttack);
        }
    }
}

