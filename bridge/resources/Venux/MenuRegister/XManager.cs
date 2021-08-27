using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.Menus;

namespace Venux.XMenu

{
    class XManager : Script
    {

        [RemoteEvent("nM-Kleidung")]
        public static void Kleidung(Client c, string arg)
        {
            try
            {
                if (arg == "hut")
                    c.SetAccessories(0, -1, 0);

                if (arg == "maske")
                {
                    c.SetData("player_mask", false);
                    c.SetClothes(1, 0, 0);
                }
                else if (c.GetData("player_mask") == false)
                {
                    Database.restoreMask(c);
                    c.SetData("player_mask", true);
                }

                if (arg == "hose")
                    Clothing.PlayerClothes.setClothes(c, 4, 0, 0);

                if (arg == "schuhe")
                    Clothing.PlayerClothes.setClothes(c, 6, 0, 0);

                if (arg == "anziehen")
                    Database.restoreMask(c);
                Items.Weapons.WeaponSync(c);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("m")]
        public static void nativeMenu(Client p, string id)
        {
            try
            {
                if (id == null)
                    return;

                if (!(id == "NaN"))
                {
                    NativeMenu nativeMenu = p.GetData("PLAYER_CURRENT_NATIVEMENU");
                    if (nativeMenu != null)
                    {
                        p.Eval("mp.events.callRemote('nM-" + nativeMenu.Title + "', '" + nativeMenu.Items[Convert.ToInt32(id)].selectionName + "');");
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("showKleidungsmenu")]
        public static void ShowAnimMenu1(Client c)
        {
            if (Database.isPlayerDeath(c.Name))
            {
                return;
            }

            try
            {
                new NativeMenu("Kleidung", "Menu", new List<NativeItem>()
                {
                    //new NativeItem("Hut", "hut"),
                    new NativeItem("Maske", "maske"),
                    new NativeItem("Aufsetzen", "anziehen"),
                    //new NativeItem("Hose", "hose"),
                    //new NativeItem("Schuhe", "schuhe")
                }).showNativeMenu(c);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
