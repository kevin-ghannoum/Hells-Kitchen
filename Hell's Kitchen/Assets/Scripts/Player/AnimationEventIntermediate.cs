using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class AnimationEventIntermediate : MonoBehaviour
    {
        public UnityEvent shootGun = new UnityEvent();

        public void FireGun()
        {
            shootGun.Invoke();
        }
    }
}
