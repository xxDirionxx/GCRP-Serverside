using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.ClothingShops;
using Venux.Menus;

namespace Venux.Fraktionen
{
    public class FraktionRegister : Script
    {
        public static List<Fraktion> fraktionList = new List<Fraktion>();
        public static Vector3 fraklagerPos = new Vector3(-1550.136, -94.57054, -191.1857);
        public static Vector3 laborPos = new Vector3(996.879, -3200.482, -37.49374);
        public static int GangwarDimension = 8888;

        [ServerEvent(Event.ResourceStart)]
        public void registerFraktions()
        {
            if (fraktionList.Count < 1)
            {
                foreach (Fraktion fraktion in Database.getFraktionList())
                    fraktionList.Add(fraktion);
            }

            NAPI.Marker.CreateMarker(1, new Vector3(-1547.861, -100.145, -193.2261), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
            NAPI.Marker.CreateMarker(44, new Vector3(-1555.883, -108.5762, -192.2261), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
            NAPI.Marker.CreateMarker(22, new Vector3(-1558.609, -95.63198, -192.1858), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));

            ColShape val3 = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1555.883, -108.5762, -193.2261), 1.4f, 1.4f, uint.MaxValue);
            val3.SetData("COLSHAPE_FUNCTION", new FunctionModel("openWaffenschrank"));
            val3.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um den Waffenschrank zu öffnen.", "WAFFENSCHRANK", "white", 5000));

            ColShape val4 = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1558.609, -95.63198, -193.1858), 1.4f, 1.4f, uint.MaxValue);
            val4.SetData("COLSHAPE_FUNCTION", new FunctionModel("openFraktionsKleiderschrank"));
            val4.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um den Fraktionskleiderschrank zu öffnen.", "KLEIDERSCHRANK", "white", 5000));

            ColShape val2 = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1547.861, -100.145, -193.2261), 1.4f, 1.4f, uint.MaxValue);
            val2.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterGangwarDimension"));
            val2.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um die Gangwar Dimension zu betreten", "GANGWAR", "white", 5000));

            ColShape val = NAPI.ColShape.CreateCylinderColShape(fraklagerPos, 2.4f, 2.4f, uint.MaxValue);
            val.SetData("COLSHAPE_FUNCTION", new FunctionModel("exitFraklager"));

            foreach (Fraktion fraktion in fraktionList)
                loadFraktion(fraktion);

        }

        public void loadFraktion(Fraktion fraktion)
        {
            NAPI.Marker.CreateMarker(1, fraktion.fraktionsLagerPoint, new Vector3(), new Vector3(), 1f, new Color(0, 0, 0));
            NAPI.Marker.CreateMarker(21, fraktion.garagePoint, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0));

            ColShape val = NAPI.ColShape.CreateCylinderColShape(fraktion.garagePoint, 1.4f, 1.4f, uint.MaxValue);
            val.SetData("COLSHAPE_FUNCTION", new FunctionModel("openFraktionsGarage", fraktion.fraktionName));
            val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um die Fraktionsgarage zu öffnen.", fraktion.fraktionName, "green", 5000));


            ColShape val2 = NAPI.ColShape.CreateCylinderColShape(fraktion.fraktionsLagerPoint, 1.4f, 1.4f, uint.MaxValue);
            val2.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterFraklager", fraktion.fraktionName));
            val2.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um das Fraklager zu betreten.", fraktion.fraktionName, "green", 5000));

            //                     ------------------   Fraktion Labore VenuxC  ------------------------    //

            ColShape val3 = NAPI.ColShape.CreateCylinderColShape(fraktion.frakLaborPoint, 1.4f, 1.4f, uint.MaxValue);
            val3.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterLabor", fraktion.fraktionName));
            val3.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um das Labor zu betreten", fraktion.fraktionName, "white", 5000));

            ColShape val5 = NAPI.ColShape.CreateCylinderColShape(new Vector3(996.879, -3200.482, -37.49374), 2.4f, 2.4f, uint.MaxValue);
            val5.SetData("COLSHAPE_FUNCTION", new FunctionModel("exitFrakLabor"));

            FraktionsVehicles.list.Add(fraktion.fraktionName, Database.getFraktionVehicles2(fraktion.fraktionName));
        }


        [RemoteEvent("enterLabor")]
        public void enterLabor(Client p, string fraktionname)
        {
            if (fraktionname == null) { return; }

            try
            {
                if (Database.isPlayerInFrak(p, fraktionname))
                {
                    Fraktion fraktion = Database.getFraktionByName(fraktionname);
                    Anticheat.Wait(p); p.Position = new Vector3(996.879, -3200.482, -37.49374).Add(new Vector3(0, 0, 1.5));
                    p.Dimension = fraktion.fraktionsDimension;
                    p.TriggerEvent("loadMethInterior", 1, 2, 1);
                    return;
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht in der Fraktion.", 5000, "red", fraktionname, "");
                }
            }
            catch (Exception ex)
            {
                Log.Write("Failed to get in Fraktions Labor:" + ex.Message);
            }
        }

        [RemoteEvent("exitFrakLabor")]
        public void exitFrakLabor(Client p)
        {
            try
            {
                foreach (Fraktion fraktion in fraktionList)
                {
                    if (Database.isPlayerInFrak(p, fraktion.fraktionName))
                    {
                        Anticheat.Wait(p); p.Position = fraktion.frakLaborPoint.Add(new Vector3(0, 0, 1.5));

                        if (p.Dimension == fraktion.fraktionsDimension)
                            p.Dimension = 0;

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
        //   -------  Fraktion Labore ENDE  -------- //

        [RemoteEvent("exitFraklager")]
        public void exitFraklager(Client p)
        {
            try
            {
                foreach (Fraktion fraktion in fraktionList)
                {
                    if (Database.isPlayerInFrak(p, fraktion.fraktionName))
                    {
                        Anticheat.Wait(p); p.Position = fraktion.fraktionsLagerPoint.Add(new Vector3(0, 0, 1.5));

                        if (p.Dimension == fraktion.fraktionsDimension)
                            p.Dimension = 0;

                        return;
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("enterFraklager")]
        public void enterFraklager(Client p, string fraktionname)
        {
            if (fraktionname == null) { return; }

            try
            {
                if (Database.isPlayerInFrak(p, fraktionname))
                {
                    Fraktion fraktion = Database.getFraktionByName(fraktionname);
                    if (p.Position.DistanceTo(fraktion.fraktionsLagerPoint) < 2.0f)
                    {
                        Anticheat.Wait(p); p.Position = fraklagerPos;
                        p.Dimension = fraktion.fraktionsDimension;
                        return;
                    }
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht in der Fraktion.", 5000, "red", fraktionname, "");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("enterGangwarDimension")]
        public void enterGangwarDimension(Client p)
        {
            try
            {
                if (p.GetSharedData("FRAKTION") == "Zivilist") { return; }

                if (p.Dimension != GangwarDimension)
                {
                    p.Dimension = (uint)GangwarDimension;
                    Notification.SendPlayerNotifcation(p, "Du hast die Gangwar Dimension betreten.", 5000, "orange", "", "orange");
                    NAPI.Player.SpawnPlayer(p, Database.getFraktionByName(p.GetSharedData("FRAKTION")).spawnPoint);
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("openWaffenschrank")]
        public void openWaffenschrank(Client p)
        {
            try
            {
                foreach (Fraktion fraktion in fraktionList)
                {
                    NAPI.Task.Run(() =>
                    {
                        if (Database.isPlayerInFrak(p, fraktion.fraktionName))
                        {
                            if (fraktion.isBadFraktion)
                            {
                                NativeMenu nativeMenu = new NativeMenu("Waffenschrank", fraktion.shortName, new List<NativeItem>(){
                                new NativeItem("5x Schutzweste - 3500$", "Schutzweste-3500-5"),
                                new NativeItem("5x Verbandskasten - 3500$", "Verbandskasten-3500-5"),
                                new NativeItem("1x Advancedrifle - 80000$", "Advancedrifle-80000-1"),
                                new NativeItem("1x Bullpuprifle - 75000$", "Bullpuprifle-75000-1"),
                                new NativeItem("1x Compactrifle - 50000$", "Compactrifle-50000-1"),
                                new NativeItem("1x Gusenberg - 90000$", "Gusenberg-90000-1"),
                                new NativeItem("1x Heavypistol - 10000$", "Heavypistol-10000-1"),
                            });
                                nativeMenu.showNativeMenu(p);

                                return;
                            }
                        }
                    });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [RemoteEvent("openFraktionsKleiderschrank")]
        public void openFraktionsKleiderschrank(Client p)
        {
            try
            {
                foreach (Fraktion fraktion in fraktionList)
                {
                    if (Database.isPlayerInFrak(p, fraktion.fraktionName))
                    {
                        new NativeMenu("Fraktionskleiderschrank", fraktion.shortName, new List<NativeItem>()
                          {
                            new NativeItem("Maske", "Maske"),
                            new NativeItem("Oberteil", "Oberteil"),
                            new NativeItem("Unterteil", "Unterteil"),
                            new NativeItem("Körper", "Körper"),
                            new NativeItem("Hose", "Hose"),
                            new NativeItem("Schuhe", "Schuhe"),
                            new NativeItem("Fraktionsbank", "Frakbank")
                          }).showNativeMenu(p);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("nM-Fraktionskleiderschrank")]
        public void Fraktionskleiderschrank(Client p, string selection)
        {
            if (selection == null) { return; }

            try
            {
                foreach (Fraktion fraktion in fraktionList)
                {
                    NAPI.Task.Run(() =>
                    {
                        if (Database.isPlayerInFrak(p, fraktion.fraktionName))
                        {
                            List<NativeItem> Items = new List<NativeItem>();
                            foreach (ClothingModel fraktionsClothe in fraktion.fraktionsClothes)
                            {
                                if (fraktionsClothe.category == selection)
                                    Items.Add(new NativeItem(fraktionsClothe.name, selection + "-" + fraktionsClothe.component.ToString() + "-" + fraktionsClothe.drawable.ToString() + "-" + fraktionsClothe.texture.ToString()));
                                if (selection == "Frakbank")
                                    Fraktionen.FraktionsBank.openFrakBank(p);
                                Menus.NativeMenu.closeNativeMenu(p);
                            }
                            NativeMenu.closeNativeMenu(p);
                            new NativeMenu("Kleidungsauswahl", fraktion.fraktionName + " | " + selection, Items).showNativeMenu(p);
                        }
                    });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }


        [RemoteEvent("nM-Waffenschrank")]
        public void FraktionsWaffenschrank(Client p, string selection)
        {
            if (selection == null) { return; }

            try
            {
                {
                    NAPI.Task.Run(() =>
                    {
                        string[] strArray = selection.Split("-");

                        string item = strArray[0];
                        int price = int.Parse(strArray[1]);
                        int count = int.Parse(strArray[2]);

                        if (Database.getMoney(p.Name) >= price)
                        {
                            Database.changeInventoryItem(p.Name, item, count, false);
                            Database.changeMoney(p.Name, price, true);
                            Notification.SendPlayerNotifcation(p, "+" + count + " " + item, 5000, "white", "", "white");
                        }
                        else
                        {
                            Notification.SendPlayerNotifcation(p, "Du bist derzeit nicht in der Lage etwas zu kaufen. Schau auf dein Kontostand!", 5000, "red", "FEHLER", "red");
                        }
                    });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //

