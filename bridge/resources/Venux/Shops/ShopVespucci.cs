using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;
using Venux.Items;

namespace Venux.Shops
{
    class ShopVespucci : Shop
    {
        public ShopVespucci()
        {
            this.id = 1;
            this.title = "Vespucci Shop";
            this.position = new Vector3(-707.8701, -913.9265, 18.115591);
            this.items = new List<BuyItem>()
              {
                new BuyItem(new Verbandskasten(), 500),
                new BuyItem(new Schutzweste(), 1000),
                new BuyItem(new Repairkit(), 4500)
              };
        }
    }
}
