using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;
using Venux.Items;

namespace Venux.Shops
{
    class Shop247 : Shop
    {
        public Shop247()
        {
            this.id = 1;
            this.title = "24/7 Shop";
            this.position = new Vector3(25.7567, -1346.8448, 28.397045);
            this.items = new List<BuyItem>()
              {
                new BuyItem(new Verbandskasten(), 500),
                new BuyItem(new Schutzweste(), 1000),
                new BuyItem(new Repairkit(), 4500)
              };
        }
    }
}
