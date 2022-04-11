
namespace Weapons
{
    public interface IWeapon
    {
        public void OnEquip();
        public void OnUnequip();
        public float Damage { get; }
    }
}
