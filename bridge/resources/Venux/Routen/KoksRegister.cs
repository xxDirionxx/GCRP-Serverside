using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Venux.Routen
{
    class KoksRegister : Script
    {
        public static Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();
        public static List<Client> farming = new List<Client>();
        public static List<Client> selling = new List<Client>();
        public static int Kokspreis = 0;
        public static int KoksItem = 0;

        public static Timer OnFarmingSpentTimer;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            Kokspreis = new Random().Next(9800, 17500);

            if (points.Count < 1)
            {
                points.Add("farmer", new Vector3(1874.082, 284.7344, 161.7287));

                foreach (KeyValuePair<string, Vector3> point in points)
                {
                    ColShape val = NAPI.ColShape.CreateCylinderColShape(point.Value, 30f, 2f, 0);
                    val.SetData("COLSHAPE_FUNCTION", new FunctionModel("changeFarmingST", point.Key, "Koks"));
                    val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um die Koksblätter zu Farmen.", "", "orange", 3500));

                    if (point.Key == "farmer")
                        NAPI.Blip.CreateBlip(164, point.Value, 1.0f, 0, "Koksblätter" + point.Key, 255, 0, true, 0, 0);
                }
            }

            OnFarmingSpentTimer = new Timer(11000.0);
            OnFarmingSpentTimer.AutoReset = true;
            OnFarmingSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                OnFarmingSpent(null);
            };
            OnFarmingSpentTimer.Start();

            Log.Write("Koks-Route geladen.");
        }

        [RemoteEvent("changeFarmingST")]
        public void changeFarmingST(Client p, string arg1, string arg2)
        {
            if (arg1 == null || arg2 == null)
                return;

            try
            {
                {
                    if (arg1 == "farmer")
                    {
                        if (p.GetData("IS_FARMING"))
                        {
                            Notification.SendPlayerNotifcation(p, "Du hörst nun auf zu sammeln!", 3500, "gray", "", "orange");
                            farming.Remove(p);
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
                            farming.Add(p);
                            p.TriggerEvent("disableAllPlayerActions", true);
                            p.SetData("IS_FARMING", true);
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void OnFarmingSpent(object unused)
        {
            try
            {
                KoksItem = new Random().Next(1, 4);
                foreach (Client p in farming.ToArray())
                {
                    if (NAPI.Pools.GetAllPlayers().Contains(p))
                    {
                        p.SetData("IS_FARMING", true);
                        NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                        NAPI.Task.Run(delegate
                        {
                            Database.changeInventoryItem(p.Name, "Koksblätter", KoksItem, false);
                            Notification.SendPlayerNotifcation(p, "+" + KoksItem + " Koksblätter erhalten!", 3000, "orange", "", "orange");
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
    }
}
