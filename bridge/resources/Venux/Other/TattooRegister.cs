using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Other
{
    class TattooRegister : Script
    {

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            NAPI.Marker.CreateMarker(37, new Vector3(1324.009, -1652.691, 51.17553), new Vector3(), new Vector3(), 0.5f, new Color(255, 140, 0));
            ColShape col = NAPI.ColShape.CreateCylinderColShape(new Vector3(1324.009, -1652.691, 51.17553), 1.4f, 1.4f, uint.MaxValue);
            col.SetData("COLSHAPE_FUNCTION", new FunctionModel("TattooShopUI"));
            col.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um auf den TattooShop zuzugreifen", "", "grey", 4500));
        }

        [RemoteEvent("TattooShopUI")]
        public void Shop_TattooShopUI(Client p)
        {
            try
            {
                NAPI.Task.Run(() =>
                {
                    p.TriggerEvent("openWindow", new object[] { "TattooShop", "{\"ejectSeat\": \"target.Name\"}" });
                }, 500);

            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }


        public static List<Tattoos> Tattoo = new List<Tattoos>
        {
            new Tattoos("Abbrechen", "Abbrechen.png", 0),
            new Tattoos("Animation Bearbeiten", "Bearbeiten.png", 1),
            new Tattoos("Arme verschränken", "Animation.png", 2),
            new Tattoos("Kniehen", "Animation.png", 3),
            new Tattoos("Ergeben", "Animation.png", 4),
            new Tattoos("Phone", "Animation.png", 5),
            new Tattoos("Doggy", "Animation.png", 6),
            new Tattoos("Tanzen", "Animation.png", 7),
            new Tattoos("Angst", "Animation.png", 8),
            new Tattoos("Liegen", "Animation.png", 9),
            new Tattoos("Trinken", "Animation.png", 10),
            new Tattoos("Hämmern", "Animation.png", 11),
            new Tattoos("Liegestützen", "Animation.png", 12),
            new Tattoos("Salutieren", "Animation.png", 13),
            new Tattoos("Putzen", "Animation.png", 14),
        };

        [RemoteEvent("syncTattoo")]
        public static void syncTattoo(Client p, Client target)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }

            if (target != null)
            {
                //p.TriggerEvent("openWindow", new object[] { "GiveMoney", "{\"targetName\": \"target.Name\"}" });
            }
        }

        public class Tattoos
        {
            public string name { get; set; }
            public string icon { get; set; }
            public int slot { get; set; }

            public Tattoos(string i, string n, int f)
            {
                name = i;
                icon = n;
                slot = f;
            }
        }
    }
}
