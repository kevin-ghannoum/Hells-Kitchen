using Photon.Pun;

namespace Common.Interfaces {
    public interface IKillable
    {
        public float HitPoints { get; }

        public PhotonView PhotonView { get; }
        
        public void TakeDamage(float damage);
    }
}
