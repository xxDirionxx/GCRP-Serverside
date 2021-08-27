using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;

namespace Venux.Vehicles.Shops
{
    internal class AutoshopDavis : AutoShop
    {
        public AutoshopDavis()
        {
            this.id = 1;
            this.name = "Autoshop Arena";
            this.position = new Vector3(-232.0067, -1997.24, 28.84606);
            this.ausparkPunkt = new Vector3(-217.2852, -1999.039, 26.65544);
            this.ausparkPunktRotation = 261.7945f;
            this.autoshopItems = new List<BuyCar>()
              {
                new BuyCar("virgo3", 75000),
                new BuyCar("buffalo", 100000),
                new BuyCar("vamos", 150000),
                new BuyCar("ruiner", 200000),
                new BuyCar("gauntlet5", 340000),
                new BuyCar("ellie", 355000)
              };
        }
    }
}
