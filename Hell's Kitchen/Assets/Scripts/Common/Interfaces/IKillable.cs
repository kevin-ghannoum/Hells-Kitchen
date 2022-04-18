using UnityEngine.Events;

namespace Common.Interfaces {
    public interface IKillable
    {
        public float HitPoints { get; }
        
        public void TakeDamage(float damage);
    }
}
