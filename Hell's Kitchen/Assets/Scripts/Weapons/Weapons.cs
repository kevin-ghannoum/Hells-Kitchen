namespace Weapons
{
    public static class Weapons
    {
        public static Weapon GetItem(WeaponInstance weapon)
        {
            FieldInfo field = typeof(Items).GetFields().FirstOrDefault(f => f.Name == item.ToString());
            return (Item)field?.GetValue(null);
        }
    }
}