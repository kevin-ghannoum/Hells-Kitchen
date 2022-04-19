using UnityEditor;
using UnityEngine;

namespace Weapons.Models
{
    public class WeaponModel
    {
        public readonly GameObject Prefab;

        private WeaponModel(string path)
        {
            Prefab = Resources.Load<GameObject>(path);
        }

        // Guns
        public static readonly WeaponModel Pistol = new WeaponModel("Pistol");
        public static readonly WeaponModel Shotgun = new WeaponModel("Shotgun");
        public static readonly WeaponModel Revolver = new WeaponModel("Revolver");
        
        // Blades
        public static readonly WeaponModel GreatSword = new WeaponModel("GreatSword");
        public static readonly WeaponModel LongSword = new WeaponModel("LongSword");
        public static readonly WeaponModel Scimitar = new WeaponModel("Scimitar");
        public static readonly WeaponModel Axe = new WeaponModel("Axe");
    }
}
