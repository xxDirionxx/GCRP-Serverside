using GTANetworkAPI;
using System;

namespace Venux.Items
{
    class Marksmankiste : Item
    {

        public Marksmankiste()
        {
            Id = 19;
            Name = "Marksmankiste";
            ImagePath = "KisteMarksmanRifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            int random = new Random().Next(2);

            switch (random)
            {
                case 0:
                    Database.changeInventoryItem(p.Name, "Marksmanrifle", 1, false);
                    Notification.SendPlayerNotifcation(p, "+1 Marksmanrifle", 5000, "orange", "", "white");
                    break;

                case 1:
                    Database.changeInventoryItem(p.Name, "Advancedrifle", 3, false);
                    Notification.SendPlayerNotifcation(p, "+3 Advancedrifle", 5000, "red", "", "white");
                    break;
            }
            return true;
        }
    }
}
