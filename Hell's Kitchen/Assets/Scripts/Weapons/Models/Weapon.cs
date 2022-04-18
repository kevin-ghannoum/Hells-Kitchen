
namespace Weapons.Models
{
    public class Weapon
    {
        public readonly WeaponModel WeaponModel;
        public readonly string Name;

        public Weapon(WeaponModel weaponModel, string name)
        {
            Name = name;
            WeaponModel = weaponModel;
        }
    }
}