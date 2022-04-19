using UnityEditor;
using UnityEngine;
using Weapons;

namespace Enums
{
    public static class WeaponSprites
    {
        public static Sprite Gun1 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_2872527.png"); // glock
        public static Sprite Gun2 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_448314.png"); // revolver
        public static Sprite Gun3 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_5322070.png"); // shotgun
        public static Sprite Axe = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_3755031.png"); // axe
        public static Sprite Sword1 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_2929708.png"); // scimitar
        public static Sprite Sword2 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_4155897.png"); // longsword
        public static Sprite Sword3 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/weapons/freepik_4666542.png") ; // golden sword
        
        public static Sprite GetSprite(string weaponName)
        {
            switch (weaponName)
            {
                case("Gun1"):
                    return Gun1;
                case("Gun2"):
                    return Gun2;
                case("Gun3"):
                    return Gun3;   
                case("Axe"):
                    return Axe;
                case("Sword1"):
                    return Sword1;
                case("Sword2"):
                    return Sword2;
                case("Sword3"):
                    return Sword3;
            }
            // default
            return null;
        }
    }
}