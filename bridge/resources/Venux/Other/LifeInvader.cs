using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Other
{

    class LifeInvader : Script
    {
        public static List<Other.openInvader> openInvader = new List<Other.openInvader>();

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            NAPI.Marker.CreateMarker(29, new Vector3(-1051.724, -236.824, 43.92112), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
            ColShape val = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1051.724, -236.824, 42.92112), 2f, 2f, 0);
            val.SetData("COLSHAPE_FUNCTION", new FunctionModel("openLifeInvader"));
            val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze `E` um eine Werbung fuer $5/Buchstabe zu schalten!", "WERBUNG", "lightblue", 4500));

            Log.Write("Liveinvader geladen.");
        }

        [RemoteEvent("requestAd")]
        public void requestAd(Client c)
        {
            c.TriggerEvent("componentServerEvent", "LifeInvaderApp", "updateLifeInvaderAds", NAPI.Util.ToJson(openInvader));
        }

        public static void addInvader(Client p, string content)
        {
            Random r = new Random();
            openInvader.Add(new Other.openInvader(p.Name, content, r.Next(1000, 9999)));
        }

        [RemoteEvent("openLifeInvader")]
        public void onopenLifeInvader(Client c)
        {
            c.TriggerEvent("openWindow", new object[3] {
                  "TextInputBox",
                  "{\"textBoxObject\":{\"Callback\":\"LifeInvaderPurchaseAd\"}}",
                  "Fuelstation",
                });
            c.TriggerEvent("componentReady", "TextInputBox");
        }

        [RemoteEvent("LifeInvaderPurchaseAd")]
        public void onopenLifeInvaderAd(Client p, string content)
        {
            if (p.HasData("IS_INVADER") == true)
            {
                Notification.SendPlayerNotifcation(p, "Du hast bereits eine Werbung geschaltet. Warte auf deinen nächsten PayDay!", 5000, "red", "", "");
                return;
            }
            Random r = new Random();
            openInvader.Add(new Other.openInvader(p.Name, content, r.Next(1000, 9999)));
            p.SetData("IS_INVADER", true);
            foreach (Client target in NAPI.Pools.GetAllPlayers())
            {
                if (Database.getPlayerRights(p.Name) > 5)
                {
                    target.SendNotification("AD | Werbung gesendet von (~y~" + target.Name + ")");
                }
            }
            NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendPlayerNotifcation(player, "Es gibt neue Werbung in der Lifeinvader App!", 5000, "yellow", "LIFEINVADER", "yellow"));
        }
    }
}

