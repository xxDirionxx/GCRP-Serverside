using GTANetworkAPI;
using System;

namespace Venux.Fraktionen
{
    class FraktionsLabor : Script
    {


        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            NAPI.Marker.CreateMarker(1, new Vector3(), new Vector3(), new Vector3(), 1f, new Color(0, 0, 0), false, uint.MaxValue);
            ColShape col = NAPI.ColShape.CreateCylinderColShape(new Vector3(1010.55, -3196.363, -40.09316), 1.4f, 1.4f, uint.MaxValue);
            col.SetData("COLSHAPE_FUNCTION", new FunctionModel("openVenuxLab"));
            col.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um auf das Labor zuzugreifen", "", "grey", 4500));

            // Schwarzgeld wäsche
            NAPI.Marker.CreateMarker(29, new Vector3(1007.817, -3201.09, -39.09316), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
            ColShape col1 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1007.817, -3201.09, -40.09316), 1.4f, 1.4f, uint.MaxValue);
            col1.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterBlackMoney"));
            col1.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um auf dein Schwarzgeld zu waschen!", "", "grey", 4500));

            // Schwarzgeld wäsche austreten
            NAPI.Marker.CreateMarker(29, new Vector3(1118.654, -3193.622, -40.49331), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
            ColShape col2 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1118.654, -3193.622, -41.49331), 1.4f, 1.4f, uint.MaxValue);
            col2.SetData("COLSHAPE_FUNCTION", new FunctionModel("exitBlackMoney"));
            col2.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um die Schwarzgeldwäscherei zu verlassen", "", "grey", 4500));
        }

        [RemoteEvent("enterBlackMoney")]
        public void enterBlackMoney(Client p)
        {

            try
            {
                {
                    Anticheat.Wait(p); p.Position = new Vector3(1138.132, -3199.072, -40.76569).Add(new Vector3(0, 0, 1.5));
                    p.Dimension = 6666;
                    p.TriggerEvent("loadblackmoneyInterior");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Failed to get in Fraktions Lagerhalle:" + ex.Message);
            }
        }

        [RemoteEvent("exitBlackMoney")]
        public void exitBlackMoney(Client p)
        {
            try
            {
                p.Position = new Vector3(1007.817, -3201.09, -40.09316).Add(new Vector3(0, 0, 1.5));
                p.Dimension = 11;
                p.TriggerEvent("loadMethInterior", 1, 2, 1);
                return;

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [RemoteEvent("openVenuxLab")]
        public void openVenuxLab(Client p)
        {
            try
            {
                if (Database.getItemCount(p.Name, "Koksblätter") > 0)
                {

                    if (p.HasData("IS_DOINGMETH") == true)
                    {
                        Notification.SendPlayerNotifcation(p, "Du kannst erst in 15 Sekunden wieder Koks herstellen", 3500, "grey", "LABOR", "");
                        return;
                    }
                    else
                    {

                        NAPI.Player.PlayPlayerAnimation(p, 33, "mini@repair", "fixing_a_player", 8f);
                        p.TriggerEvent("disableAllPlayerActions", new object[1]
                        {
                        true
                        });
                        Functions.disableAllPlayerControls(p, true);
                        p.TriggerEvent("sendProgressbar", new object[1]
                        {
                        15000
                        });
                        p.SetData("IS_DOINGMETH", true);
                        NAPI.Task.Run(delegate
                        {
                            p.TriggerEvent("disableAllPlayerActions", new object[1]
                            {
                            false
                            });
                            p.ResetData("IS_DOINGMETH");
                            Functions.disableAllPlayerControls(p, false);
                            NAPI.Player.StopPlayerAnimation(p);
                            Database.changeInventoryItem(p.Name, "Koksblätter", 40, true);
                            Database.changeInventoryItem(p.Name, "Koks", 4, false);

                        }, 15000);
                    }

                }
                else if (Database.getItemCount(p.Name, "Cannabis") > 0)
                {
                    if (p.HasData("IS_DOINGWEED") == true)
                    {
                        Notification.SendPlayerNotifcation(p, "Du kannst erst in 15 Sekunden wieder Weed herstellen", 2500, "grey", "LABOR", "");
                    }
                    else
                    {

                        NAPI.Player.PlayPlayerAnimation(p, 33, "mini@repair", "fixing_a_player", 8f);
                        p.TriggerEvent("disableAllPlayerActions", new object[1]
                        {
                        true
                        });
                        Functions.disableAllPlayerControls(p, true);
                        p.TriggerEvent("sendProgressbar", new object[1]
                        {
                        15000
                        });
                        p.SetData("IS_DOINGMETH", true);
                        NAPI.Task.Run(delegate
                        {
                            p.TriggerEvent("disableAllPlayerActions", new object[1]
                            {
                            false
                            });
                            p.ResetData("IS_DOINGWEED");
                            Functions.disableAllPlayerControls(p, false);
                            NAPI.Player.StopPlayerAnimation(p);
                            Database.changeInventoryItem(p.Name, "Cannabis", 40, true);
                            Database.changeInventoryItem(p.Name, "Weed", 5, false);

                        }, 15000);
                    }
                    { // WEED
                        Notification.SendPlayerNotifcation(p, "Hast du wirklich das Zeug dabei?", 1500, "red", "LABOR", "");
                    }
                }
                { // KOKS
                    Notification.SendPlayerNotifcation(p, "Hast du wirklich das Zeug dabei?", 2500, "red", "LABOR", "");
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //




