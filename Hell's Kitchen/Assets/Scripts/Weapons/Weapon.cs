using Common.Enums;

namespace Weapons
{
    public class Weapon
    {
        public readonly WeaponType WeaponType;
        public readonly ItemModel WeaponModel;
        public readonly float Damage;
        public readonly string Name;

        public Weapon(WeaponType weaponType, string name, float damage)
        {
            WeaponType = weaponType;
            Damage = damage;
            Name = name;
        }
    }
}