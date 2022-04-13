
using UnityEngine.InputSystem;

namespace Weapons
{
    public interface IWeapon
    {
        public void PickUpItem();
        public void Drop(InputAction.CallbackContext callbackContext);
        public void Throw(InputAction.CallbackContext callbackContext);
    }
}
