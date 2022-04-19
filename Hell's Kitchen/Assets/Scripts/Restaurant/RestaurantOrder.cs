using Enums.Items;
using ExitGames.Client.Photon;

namespace Restaurant
{
    public class RestaurantOrder
    {
        public static int NextOrderID = 0;

        public int ID;
        public ItemInstance Item;
        public int Quantity;
        public bool Served;
        public float CashMoney;

        public RestaurantOrder(int ID)
        {
            this.ID = ID;
        }

        public RestaurantOrder()
        {
            ID = NextOrderID++;
        }

        public static object Deserialize(byte[] data)
        {
            var order = new RestaurantOrder(0);
            int index = 0;
            Protocol.Deserialize(out order.ID, data, ref index);
            Protocol.Deserialize(out int itemInstance, data, ref index);
            order.Item = (ItemInstance) itemInstance;
            Protocol.Deserialize(out order.Quantity, data, ref index);
            Protocol.Deserialize(out short served, data, ref index);
            order.Served = served != 0;
            Protocol.Deserialize(out order.CashMoney, data, ref index);
            return order;
        }

        public static byte[] Serialize(object obj)
        {
            var order = (RestaurantOrder)obj;
            byte[] bytes = new byte[18];
            int index = 0;
            Protocol.Serialize(order.ID, bytes, ref index);
            Protocol.Serialize((int)order.Item, bytes, ref index);
            Protocol.Serialize(order.Quantity, bytes, ref index);
            Protocol.Serialize((short) (order.Served ? 1 : 0), bytes, ref index);
            Protocol.Serialize(order.CashMoney, bytes, ref index);
            return bytes;
        }
    }
}
