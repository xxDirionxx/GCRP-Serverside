using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.Menus;

namespace Venux.Dealer
{
    public class Verkäufer : Script
    {
        public static Vector3 KoksPos = new Vector3(814.1409, -491.8114, 29.52864);

        public static int KoksPreis = 0;

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            KoksPreis = new Random().Next(7900, 11000);

            ColShape val = NAPI.ColShape.CreateCylinderColShape(KoksPos, 2f, 2.5f, 0);
            val.SetData("COLSHAPE_FUNCTION", new FunctionModel("openKoksDealer"));
            val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um beim Dealer dein Zeug zu verkaufen!", "", "grey", 4500));

        }

        [RemoteEvent("openKoksDealer")]
        public void openKoksDealer(Client p)
        {

            try
            {
                NativeMenu nativeMenu = new NativeMenu("Dealer", "Angebote", new List<NativeItem>()
                {
                    new NativeItem("Koks - " + KoksPreis + "$", "koks"),
                });
                nativeMenu.showNativeMenu(p);

            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("nM-Dealer")]
        public void weedDealer(Client p, string selection)
        {

            if (selection == "koks")
            {
                if (Database.getItemCount(p.Name, "Koks") > 0)
                {
                    Database.changeInventoryItem(p.Name, "Koks", 1, true);
                    Database.changeBlackMoney(p.Name, KoksPreis, false);
                    Notification.SendPlayerNotifcation(p, "Du hast 1x Koks verkauft", 4500, "green", "DEALER", "");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Willst du mich verarschen? WO IST DAS ZEUG?!", 4500, "red", "DEALER", "");
                }
            }
        }
    }
}
