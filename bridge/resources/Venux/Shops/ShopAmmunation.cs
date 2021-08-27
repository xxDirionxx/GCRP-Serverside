using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;
using Venux.Items;

namespace Venux.Shops
{
    class ShopAmmunation : Shop
    {
        public ShopAmmunation()
        {
            this.id = 1;
            this.title = "Ammunation Shop";
            this.position = new Vector3(21.150259, -1107.3888, 28.697025);
            this.items = new List<BuyItem>()
              {
                new BuyItem(new Heavypistol(), 15000),
                new BuyItem(new Compactrifle(), 55000)
              };
            this.customBlip = 110U;
            this.customBlipColor = (byte)4;
        }
    }
}
