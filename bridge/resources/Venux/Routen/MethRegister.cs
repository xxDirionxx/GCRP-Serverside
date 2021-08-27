using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Timers;
using Venux.Menus;

namespace Venux.Routen
{
    class MethRegister : Script
    {
        public static Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();
        public static List<Client> farming = new List<Client>();
        public static List<Client> processing = new List<Client>();
        public static List<Client> selling = new List<Client>();
        public static int Methpreis = 0;
        public static int MethItem = 0;

        public static Timer OnFarmingSpentTimer;
        public static Timer OnProcessingSpentTimer;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            Methpreis = new Random().Next(8800, 15500);

            if (points.Count < 1)
            {
                points.Add("farmer", new Vector3(-500.5, 2977.22, 25.48));
                points.Add("verarbeiter", new Vector3(163.16, 2285.88, 92.95));
                points.Add("verkäufer", new Vector3(-676.69, -884.92, 23.35));

                foreach (KeyValuePair<string, Vector3> point in points)
                {
                    ColShape val = NAPI.ColShape.CreateCylinderColShape(point.Value, 30f, 2f, 0);
                    val.SetData("COLSHAPE_FUNCTION", new FunctionModel("changeFarming", point.Key, "Methkisten"));
                    val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um das Meth Farming zu starten/beenden.", "", "orange", 3500));

                    if (point.Key == "verkäufer")
                        NAPI.Blip.CreateBlip(277, point.Value, 1.0f, 0, "Methkisten" + point.Key, 255, 0, true, 0, 0);
                    else
                        NAPI.Blip.CreateBlip(164, point.Value, 1.0f, 0, "Meth" + point.Key, 255, 0, true, 0, 0);
                }
            }

            OnFarmingSpentTimer = new Timer(11000.0);
            OnFarmingSpentTimer.AutoReset = true;
            OnFarmingSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                OnFarmingSpent(null);
            };
            OnFarmingSpentTimer.Start();

            OnProcessingSpentTimer = new Timer(6600.0);
            OnProcessingSpentTimer.AutoReset = true;
            OnProcessingSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                OnProcessingSpent(null);
            };
            OnProcessingSpentTimer.Start();

            Log.Write("Meth-Route geladen.");
        }

        [RemoteEvent("changeFarming")]
        public void changeFarming(Client p, string arg1, string arg2)
        {
            if (arg1 == null || arg2 == null)
                return;

            try
            {
                if (arg2 == "Methkisten")
                {
                    if (arg1 == "verarbeiter")
                    {
                        if (p.HasData("IS_FARMS") == true)
                        {
                            return;
                        }
                        if (p.GetData("IS_FARMING"))
                        {
                            Notification.SendPlayerNotifcation(p, "Du verarbeitest nicht mehr!", 3500, "orange", "", "orange");
                            Routen.MethRegister.processing.Remove(p);
                            p.SetData("IS_FARMING", false);
                            NAPI.Player.StopPlayerAnimation(p);
                            p.TriggerEvent("disableAllPlayerActions", false);
                            p.Eval("mp.players.local.freezePosition(false);");
                        }
                        else
                        {
                            if (p.HasData("IS_FARMS") == true)
                            {
                                return;
                            }
                            p.Eval("mp.players.local.freezePosition(true);");
                            p.SetData("IS_FARMING", true);
                            p.TriggerEvent("disableAllPlayerActions", true);
                            NAPI.Player.StopPlayerAnimation(p);
                            Routen.MethRegister.processing.Add(p);
                            Notification.SendPlayerNotifcation(p, "Du verarbeitest nun!", 3500, "orange", "", "orange");
                        }
                    }
                    else if (arg1 == "farmer")
                    {
                        if (p.GetData("IS_FARMING"))
                        {
                            Notification.SendPlayerNotifcation(p, "Du hörst nun auf zu sammeln!", 3500, "gray", "", "orange");
                            Routen.MethRegister.farming.Remove(p);
                            p.SetData("IS_FARMING", false);
                            NAPI.Player.StopPlayerAnimation(p);
                            p.TriggerEvent("disableAllPlayerActions", false);
                            p.Eval("mp.players.local.freezePosition(false);");
                        }
                        else
                        {
                            //NAPI.Player.StopPlayerAnimation(p);
                            p.Eval("mp.players.local.freezePosition(true);");
                            NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                            Notification.SendPlayerNotifcation(p, "Du fängst nun an zu sammeln!", 3500, "gray", "", "orange");
                            Routen.MethRegister.farming.Add(p);
                            p.TriggerEvent("disableAllPlayerActions", true);
                            p.SetData("IS_FARMING", true);
                        }
                    }
                    else if (arg1 == "verkäufer")
                    {
                        NativeMenu nativeMenu = new NativeMenu("Methkisten", "Verkäufer", new List<NativeItem>()
                    {
                         new NativeItem("3x Methkisten - " + Routen.MethRegister.Methpreis + "$", "3")
                    });
                        nativeMenu.showNativeMenu(p);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void OnFarmingSpent(object unused)
        {
            try
            {
                MethItem = new Random().Next(1, 4);
                foreach (Client p in farming.ToArray())
                {
                    if (NAPI.Pools.GetAllPlayers().Contains(p))
                    {
                        p.SetData("IS_FARMING", true);
                        NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                        NAPI.Task.Run(delegate
                        {
                            Database.changeInventoryItem(p.Name, "Meth", MethItem, false);
                            Notification.SendPlayerNotifcation(p, "+" + MethItem + " Meth erhalten!", 3000, "orange", "", "orange");
                        }, 10000);
                    }
                    else
                    {
                        if (farming.Contains(p))
                            farming.Remove(p);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void OnProcessingSpent(object unused)
        {
            try
            {
                foreach (Client p in processing.ToArray())
                {
                    if (p.HasData("IS_FARMS") == true)
                    {
                        return;
                    }
                    if (NAPI.Pools.GetAllPlayers().Contains(p))
                    {
                        if (Database.getItemCount(p.Name, "Meth") > 9)
                        {
                            p.TriggerEvent("sendProgressbar", new object[1]
                            {
                        5000
                            });
                            p.SetData("IS_FARMS", true);
                            Functions.disableAllPlayerControls(p, true);
                            NAPI.Player.PlayPlayerAnimation(p, 33, "amb@prop_human_parking_meter@female@base", "base_female", 8f);
                            NAPI.Task.Run((() =>
                            {
                                p.TriggerEvent("componentServerEvent", new object[2]
                                {
                            "Progressbar",
                            "StopProgressbar"
                                });
                                p.SetData("IS_FARMING", true);
                                Database.changeInventoryItem(p.Name, "Methkisten", 1, false);
                                Database.changeInventoryItem(p.Name, "Meth", 10, true);
                                Notification.SendPlayerNotifcation(p, "+1 Methkiste", 3000, "orange", "", "orange");
                                p.StopAnimation();
                                Functions.disableAllPlayerControls(p, false);
                            }), 6000);
                            p.ResetData("IS_FARMS");
                        }
                        else
                        {
                            Notification.SendPlayerNotifcation(p, "Du hast zu wenig Meth dabei.", 3500, "red", "", "orange");
                            if (processing.Contains(p))
                                processing.Remove(p);
                        }
                    }
                    else
                    {
                        processing.Remove(p);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("nM-Methkisten")]
        public static void Methverkaufen(Client p, string countstring)
        {
            try
            {
                int count = int.Parse(countstring);

                if (Database.getItemCount(p.Name, "Methkisten") >= count)
                {
                    Database.changeInventoryItem(p.Name, "Methkisten", count, true);
                    Database.changeMoney(p.Name, count * Methpreis, false);
                    Notification.SendPlayerNotifcation(p, "-" + count + " Methkisten", 3000, "orange", "", "orange");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast zu wenig Methkisten dabei.", 3000, "red", "", "orange");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }


        }
    }
}
