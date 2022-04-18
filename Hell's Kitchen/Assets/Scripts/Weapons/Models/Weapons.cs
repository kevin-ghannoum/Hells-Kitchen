using System.Linq;
using System.Reflection;
using Common.Enums;

namespace Weapons.Models
{
    public static class Weapons
    {
        public static readonly  Weapon Pistol = new Weapon(WeaponModel.Pistol, "Pistol");
        public static readonly  Weapon Revolver = new  Weapon(WeaponModel.Revolver, "Revolver");
        public static readonly  Weapon Shotgun = new  Weapon(WeaponModel.Shotgun, "Shotgun");
        public static readonly  Weapon GreatSword = new  Weapon(WeaponModel.GreatSword, "BroadSword");
        public static readonly  Weapon LongSword= new  Weapon(WeaponModel.LongSword, "LongSword");
        public static readonly  Weapon Scimitar = new  Weapon(WeaponModel.Scimitar, "Scimitar");
        public static readonly  Weapon Axe = new  Weapon(WeaponModel.Axe, "Axe");

        public static Weapon GetItem(WeaponInstance weapon)
        {
            FieldInfo field = typeof(Weapons).GetFields().FirstOrDefault(f => f.Name == weapon.ToString());
            return (Weapon)field?.GetValue(null);
        }
    }
}