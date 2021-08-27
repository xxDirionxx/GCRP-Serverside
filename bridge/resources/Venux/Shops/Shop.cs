using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;

namespace Venux.Shops
{
    public class Shop
    {
        public int id { get; set; }

        public string title { get; set; }

        public uint customBlip { get; set; } = 0;

        public byte customBlipColor { get; set; }

        public Vector3 position { get; set; }

        public List<BuyItem> items { get; set; }
    }
}
