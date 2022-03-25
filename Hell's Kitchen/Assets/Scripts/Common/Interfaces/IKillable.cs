using UnityEngine.Events;

namespace Common.Interfaces {
    public interface IKillable
    {
        public float HitPoints { get; }

        public UnityEvent Killed { get; }

        public void TakeDamage(float damage);
    }
}
