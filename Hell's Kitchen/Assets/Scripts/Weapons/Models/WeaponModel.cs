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
        public static readonly WeaponModel Pistol = new WeaponModel("Pistol.prefab");
        public static readonly WeaponModel Shotgun = new WeaponModel("Shotgun.prefab");
        public static readonly WeaponModel Revolver = new WeaponModel("Revolver.prefab");
        
        // Blades
        public static readonly WeaponModel GreatSword = new WeaponModel("GreatSword.prefab");
        public static readonly WeaponModel LongSword = new WeaponModel("LongSword.prefab");
        public static readonly WeaponModel Scimitar = new WeaponModel("Scimitar.prefab");
        public static readonly WeaponModel Axe = new WeaponModel("Axe.prefab");
    }
}
