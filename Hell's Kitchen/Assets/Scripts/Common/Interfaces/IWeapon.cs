
using UnityEngine.InputSystem;

namespace Common.Interfaces
{
    public interface IWeapon : IPickup
    {
        public void Use(InputAction.CallbackContext callbackContext);
    }
}
