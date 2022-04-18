using UnityEditor;
using UnityEngine;

namespace Weapons.Models
{
    public class WeaponModel
    {
        public readonly GameObject Prefab;

        private WeaponModel(string path)
        {
            Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }

        // Guns
        public static readonly WeaponModel Pistol = new WeaponModel("Assets/Prefabs/Weapons/Pistol.prefab");
        public static readonly WeaponModel Shotgun = new WeaponModel("Assets/Prefabs/Weapons/Shotgun.prefab");
        public static readonly WeaponModel Revolver = new WeaponModel("Assets/Prefabs/Weapons/Revolver.prefab");
        
        // Blades
        public static readonly WeaponModel GreatSword = new WeaponModel("Assets/Prefabs/Weapons/GreatSword.prefab");
        public static readonly WeaponModel LongSword = new WeaponModel("Assets/Prefabs/Weapons/LongSword.prefab");
        public static readonly WeaponModel Scimitar = new WeaponModel("Assets/Prefabs/Weapons/Scimitar.prefab");
        public static readonly WeaponModel Axe = new WeaponModel("Assets/Prefabs/Weapons/Axe.prefab");
    }
}