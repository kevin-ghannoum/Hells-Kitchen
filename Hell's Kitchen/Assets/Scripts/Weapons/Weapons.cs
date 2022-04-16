using System.Linq;
using System.Reflection;
using Common.Enums;

namespace Weapons
{
    public static class Weapons
    {
        public static readonly  Weapon Pistol = new Weapon("Pistol", 1.0f, WeaponModel.Pistol);
        public static readonly  Weapon Revolver = new  Weapon("Revolver", 1.0f, WeaponModel.Revolver);
        public static readonly  Weapon Shotgun = new  Weapon("Shotgun", 1.0f, WeaponModel.Shotgun);
        public static readonly  Weapon GreatSword = new  Weapon("BroadSword", 1.0f, WeaponModel.GreatSword);
        public static readonly  Weapon LongSword= new  Weapon("LongSword", 1.0f, WeaponModel.LongSword);
        public static readonly  Weapon Scimitar = new  Weapon("Scimitar", 1.0f, WeaponModel.Scimitar);
        public static readonly  Weapon Axe = new  Weapon("Axe", 1.0f, WeaponModel.Axe);

        public static Weapon GetItem(WeaponInstance weapon)
        {
            FieldInfo field = typeof(Weapons).GetFields().FirstOrDefault(f => f.Name == weapon.ToString());
            return (Weapon)field?.GetValue(null);
        }
    }
}