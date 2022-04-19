using Enums.Items;

namespace Restaurant
{
    public class RestaurantOrder
    {
        public ItemInstance Item;
        public int Quantity;
        public bool Served;
        public float CashMoney;
        public RestaurantOrderItem UIObject;
    }
}
