using UnityEngine;
using UnityEngine.Events;

public class AnimationEventIntermediate : MonoBehaviour
{
    public UnityEvent fireGun = new UnityEvent();
    public UnityEvent meleeDamage = new UnityEvent();

    public void FireGun()
    {
        fireGun.Invoke();
    }

    public void MeleeDamage()
    {
        meleeDamage.Invoke();
    }
    
}
