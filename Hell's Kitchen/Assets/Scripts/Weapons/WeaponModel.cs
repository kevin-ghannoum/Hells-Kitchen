using System;
using UnityEditor;
using UnityEngine;

namespace Weapons
{
    public class WeaponModel
    {
        public readonly GameObject Prefab;

        private WeaponModel(string path)
        {
            Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }

        // Ingredients
        public static readonly WeaponModel Pistol = new WeaponModel("Assets/Prefabs/Weapons/Gun1.prefab");
        public static readonly WeaponModel Shotgun = new WeaponModel("Assets/Prefabs/Weapons/Gun3.prefab");
        public static readonly WeaponModel Revolver = new WeaponModel("Assets/Prefabs/Weapons/Gun2.prefab");
        public static readonly WeaponModel GreatSword = new WeaponModel("Assets/Prefabs/Weapons/Sword3.prefab");
        public static readonly WeaponModel LongSword = new WeaponModel("Assets/Prefabs/Weapons/Sword2.prefab");
        public static readonly WeaponModel Scimitar = new WeaponModel("Assets/Prefabs/Weapons/Sword1.prefab");
        public static readonly WeaponModel Axe = new WeaponModel("Assets/Prefabs/Weapons/Axe.prefab");
    }
}