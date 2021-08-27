using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;
using Venux.Items;

namespace Venux.Shops
{
    class ShopDavis : Shop
    {
        public ShopDavis()
        {
            this.id = 1;
            this.title = "Davis Shop";
            this.position = new Vector3(-48.4442, -1756.734, 28.42101);
            this.items = new List<BuyItem>()
              {
                new BuyItem(new Verbandskasten(), 500),
                new BuyItem(new Schutzweste(), 1000),
                new BuyItem(new Repairkit(), 4500)
              };
        }
    }
}
