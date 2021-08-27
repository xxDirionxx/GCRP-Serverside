using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;
using Venux.Items;

namespace Venux.Shops
{
    class ShopVinewood : Shop
    {
        public ShopVinewood()
        {
            this.id = 1;
            this.title = "Vinewood Shop";
            this.position = new Vector3(374.30573, 326.5396, 102.46638);
            this.items = new List<BuyItem>()
              {
                new BuyItem(new Verbandskasten(), 500),
                new BuyItem(new Schutzweste(), 1000)
              };
        }
    }
}
