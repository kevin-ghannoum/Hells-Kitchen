using UnityEditor;
using UnityEngine;
using Weapons;

namespace Enums
{
    public static class WeaponSprites
    {
        public static Sprite Gun1 = Resources.Load<Sprite>("Pistol");
        public static Sprite Gun2 = Resources.Load<Sprite>("Revolver");
        public static Sprite Gun3 = Resources.Load<Sprite>("Shotgun");
        public static Sprite Axe = Resources.Load<Sprite>("Axe");
        public static Sprite Sword1 = Resources.Load<Sprite>("Scimitar");
        public static Sprite Sword2 = Resources.Load<Sprite>("LongSword");
        public static Sprite Sword3 = Resources.Load<Sprite>("GreatSword");
        
        public static Sprite GetSprite(string weaponName)
        {
            switch (weaponName)
            {
                case("Pistol"):
                    return Gun1;
                case("Revolver"):
                    return Gun2;
                case("Shotgun"):
                    return Gun3;   
                case("Axe"):
                    return Axe;
                case("Scimitar"):
                    return Sword1;
                case("LongSword"):
                    return Sword2;
                case("GreatSword"):
                    return Sword3;
            }
            // default
            return null;
        }
    }
}
