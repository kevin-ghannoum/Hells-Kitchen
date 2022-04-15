using UnityEngine.InputSystem;

namespace Common.Interfaces
{
    public interface IPickup
    {
        public void PickUp();
        public void Drop(InputAction.CallbackContext callbackContext);
        public void Throw(InputAction.CallbackContext callbackContext);
    }
}
