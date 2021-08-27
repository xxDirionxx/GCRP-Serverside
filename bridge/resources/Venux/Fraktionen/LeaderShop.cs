using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.Menus;
using Venux.Other;

namespace Venux.Fraktionen
{
    class LeaderShop : Script
    {

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            NAPI.Marker.CreateMarker(37, new Vector3(-1536.667, -95.49875, -192.1894), new Vector3(), new Vector3(), 0.5f, new Color(255, 140, 0));
            ColShape col = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1536.667, -95.49875, -192.1894), 1.4f, 1.4f, uint.MaxValue);
            col.SetData("COLSHAPE_FUNCTION", new FunctionModel("LeaderUI"));
            col.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um auf den Leadershop zuzugreifen", "", "grey", 4500));
        }

        [RemoteEvent("LeaderUI")]
        public void CMD_Leadershop(Client p)
        {
            try
            {
                NAPI.Task.Run(() =>
                {
                    if (p.GetSharedData("FRAKTION") != "Zivilist" && p.GetSharedData("FRAKTION_RANK") > 9)
                    {
                        List<Buy.BuyCar> Cars = new List<Buy.BuyCar>();

                        if (!Database.hasFraktionVehicle(p.GetSharedData("FRAKTION"), "jugular"))
                        {
                            Cars.Add(new Buy.BuyCar("jugular", 23000000));
                        }
                        if (!Database.hasFraktionVehicle(p.GetSharedData("FRAKTION"), "drafter"))
                        {
                            Cars.Add(new Buy.BuyCar("drafter", 12000000));
                        }
                        if (!Database.hasFraktionVehicle(p.GetSharedData("FRAKTION"), "supervolito2"))
                        {
                            Cars.Add(new Buy.BuyCar("supervolito2", 60000000));
                        }
                        /*if (!Database.hasFraktionVehicle(p.GetSharedData("FRAKTION"), "schafter6"))
                        {
                            Cars.Add(new Buy.BuyCar("schafter6", 20000000));
                        }*/

                        List<NativeItem> Items = new List<NativeItem>();

                        foreach (Buy.BuyCar buyCar in Cars)
                        {
                            {
                                Items.Add(new NativeItem(Utils.FirstletterUpper(buyCar.Vehicle_Name) + " - " + buyCar.Price + "$", buyCar.Vehicle_Name.ToLower() + "-" + buyCar.Price));
                            }
                        }

                        NativeMenu nativeMenu = new NativeMenu("Leadershop", Database.getFraktionByName(p.GetSharedData("FRAKTION")).shortName, Items);
                        nativeMenu.showNativeMenu(p);
                    }
                }, 1000);

            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [RemoteEvent("nM-Leadershop")]
        public void buyFraktionsCar(Client p, string value)
        {
            if (value == null) { return; }

            try
            {
                string[] splitted = value.Split("-");
                string name = splitted[0];
                int price = int.Parse(splitted[1]);

                if (Database.getFrakBank(p.GetSharedData("FRAKTION")) >= price)
                {
                    NativeMenu.closeNativeMenu(p);
                    Database.changeFraktionMoney(p.GetSharedData("FRAKTION"), price, true);
                    Database.giveFraktionVehicle(p.GetSharedData("FRAKTION"), name);
                    Notification.SendPlayerNotifcation(p, "Du hast das Fahrzeug " + name + " erfolgreich für deine Fraktion gekauft. (Nach Neustart in der Garage)", 5000, "white", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Es ist zu wenig Geld auf der Fraktionsbank.", 5000, "red", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

    }
}
// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //