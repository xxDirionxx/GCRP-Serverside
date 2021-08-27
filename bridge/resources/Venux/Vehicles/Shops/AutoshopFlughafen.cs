using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;

namespace Venux.Vehicles.Shops
{
    internal class AutoshopFlughafen : AutoShop
    {
        public AutoshopFlughafen()
        {
            this.id = 1;
            this.name = "Autoshop Flughafen";
            this.position = new Vector3(-1034.931, -2678.675, 12.96781);
            this.ausparkPunkt = new Vector3(-965.3254, -2699.191, 13.40804);
            this.ausparkPunktRotation = 150.0593f;
            this.autoshopItems = new List<Buy.BuyCar>()
              {
                new BuyCar("zombiea", 35000),
                new BuyCar("Oracle", 10000),
                new BuyCar("baller3", 100000)
              };
        }
    }
}
