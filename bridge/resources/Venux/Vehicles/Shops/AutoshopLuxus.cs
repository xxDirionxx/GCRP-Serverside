using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;

namespace Venux.Vehicles.Shops
{
    internal class AutoshopLuxus : AutoShop
    {
        public AutoshopLuxus()
        {
            this.id = 1;
            this.name = "Luxus Autoshop";
            this.position = new Vector3(-33.5088, -1101.968, 25.32236);
            this.ausparkPunkt = new Vector3(-31.9249, -1092.002, 25.3223);
            this.ausparkPunktRotation = 337;
            this.autoshopItems = new List<Buy.BuyCar>()
              {
                new BuyCar("Burrito", 75000),
                new BuyCar("neon", 3500000),
                new BuyCar("paragon", 5500000),
                new BuyCar("jugular", 5500000),
                new BuyCar("furia", 7500000),
                new BuyCar("t20", 7500000),
                new BuyCar("thrax", 8500000),
                new BuyCar("krieger", 9900000),
              };
        }
    }
}
