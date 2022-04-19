using ExitGames.Client.Photon;

namespace Server
{
    public static class Serializer
    {
        public static byte[] SerializeInt2D(int[,] data)
        {
            byte[] bytes = new byte[8 + data.GetLength(0) * data.GetLength(1) * 4];
            int index = 0;
            Protocol.Serialize(data.GetLength(0), bytes, ref index);
            Protocol.Serialize(data.GetLength(1), bytes, ref index);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Protocol.Serialize(data[i, j], bytes, ref index);
                }
            }
            return bytes;
        }

        public static int[,] DeserializeInt2D(byte[] bytes)
        {
            int sizeX, sizeY, index = 0;
            Protocol.Deserialize(out sizeX, bytes, ref index);
            Protocol.Deserialize(out sizeY, bytes, ref index);
            int[,] data = new int[sizeX, sizeY];
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Protocol.Deserialize(out data[i, j], bytes, ref index);
                }
            }
            return data;
        }
    }
}
