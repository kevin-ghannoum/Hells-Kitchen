using Common.Enums;

namespace Weapons
{
    public class Weapon
    {
        // public readonly WeaponType WeaponType;
        public readonly WeaponModel WeaponModel;
        public readonly float Damage;
        public readonly string Name;

        public Weapon(string name, float damage, WeaponModel weaponModel)
        {
           // WeaponType = weaponType;
            Damage = damage;
            Name = name;
            WeaponModel = weaponModel;
        }
    }
}