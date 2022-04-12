
using UnityEngine.InputSystem;

namespace Weapons
{
    public interface IWeapon
    {
        public void PickUpItem();
        public void Unequip(InputAction.CallbackContext callbackContext);
    }
}
