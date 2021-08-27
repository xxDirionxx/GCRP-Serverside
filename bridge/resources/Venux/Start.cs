using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Venux.Menus;
using Venux.XMenu;

namespace Venux
{
    class Start : Script
    {
        public static List<Client> adminPlayers = new List<Client>();
        public static DateTime startTime;
        public static readonly int port = 3306;
        public static string connectionString;
        public static Dictionary<Client, string> loggedInPlayers = new Dictionary<Client, string>();
        public static Dictionary<Client, int> deathTime = new Dictionary<Client, int>();
        public static Timer OnMinuteSpentTimer;
        public static Timer OnHourSpentTimer;
        public static Timer OnSecondSpentTimer;
        public static bool ReleaseBuild = true;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            Daten.setDatabaseData();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("German Classic");
            Log.Write("Starting German Classic in " + AppDomain.CurrentDomain.BaseDirectory + " ... (1.0)");

            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetCommandErrorMessage("");
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoSpawnOnConnect(false);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            connectionString = "Server=" + Daten.host + "; Database=" + Daten.database + "; UID=" + Daten.username + "; PASSWORD=" + Daten.password;
            Database.parkAllVehicles();

            startTime = DateTime.Now;

            OnMinuteSpentTimer = new Timer(60000.0);
            OnMinuteSpentTimer.AutoReset = true;
            OnMinuteSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                OnMinuteSpent(null);
            };
            OnMinuteSpentTimer.Start();

            OnHourSpentTimer = new Timer(60 * 60000);
            OnHourSpentTimer.AutoReset = true;
            OnHourSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                OnHourSpend();
            };
            OnHourSpentTimer.Start();

            OnSecondSpentTimer = new Timer(500 + 500.0);
            OnSecondSpentTimer.AutoReset = true;
            OnSecondSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                OnSecondSpent(null);
            };
            OnSecondSpentTimer.Start();


            Players.AdminRanks.RegisterAdmins();

            foreach (Client p in NAPI.Pools.GetAllPlayers())
            {
                NAPI.Task.Run(() =>
                {
                    PlayerEvents.OnPlayerConnected(p);
                });

            }
        }

        public void OnSecondSpent(object unused)
        {
            try
            {

                foreach (String name in Functions.handcuffed)
                {
                    Client p = Database.getPlayerFromName(name);
                    if (p != null)
                    {
                        NAPI.Player.SetPlayerCurrentWeapon(p, WeaponHash.Unarmed);
                    }
                }

                foreach (KeyValuePair<Client, int> p in Start.deathTime)
                {
                    if (NAPI.Pools.GetAllPlayers().Contains(p.Key))
                    {
                        p.Key.Health = 200;
                    }
                }

                int seconds = DateTime.Now.Second;
                int minutes = DateTime.Now.Minute;
                int hours = DateTime.Now.Hour;
                NAPI.World.SetTime(hours, minutes, seconds);

                foreach (KeyValuePair<Client, int> deathPlayer in deathTime)
                {
                    NAPI.Task.Run(() =>
                    {
                        if (!NAPI.Pools.GetAllPlayers().Contains(deathPlayer.Key) || !Start.loggedInPlayers.ContainsKey(deathPlayer.Key))
                        {
                            deathTime.Remove(deathPlayer.Key);
                            return;
                        }

                        int ms = (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
                        deathPlayer.Key.Health = 200;
                        deathPlayer.Key.Armor = 5;

                        if (deathPlayer.Value < ms)
                        {
                            Database.setDeathStatus(Start.loggedInPlayers[deathPlayer.Key], false);
                            Notification.SendPlayerNotifcation(deathPlayer.Key, "Du wurdest Respawnt!", 5000, "red", "", "white");
                            deathTime.Remove(deathPlayer.Key);

                            if (deathPlayer.Key.GetSharedData("FRAKTION") == "Zivilist")
                            {
                                NAPI.Player.SpawnPlayer(deathPlayer.Key, new Vector3(-1082.94, -2840.031, 26.60876));
                            }
                            else
                            {
                                try
                                {
                                    NAPI.Player.SpawnPlayer(deathPlayer.Key, Database.getFraktionByName(deathPlayer.Key.GetSharedData("FRAKTION")).spawnPoint);
                                }
                                catch (Exception ex)
                                {
                                    Log.Write(ex.Message);
                                    NAPI.Player.SpawnPlayer(deathPlayer.Key, new Vector3(-1082.94, -2840.031, 26.60876));
                                }
                            }

                            deathPlayer.Key.StopAnimation();

                            deathPlayer.Key.TriggerEvent("stopScreenEffect", "DeathFailOut");
                            deathPlayer.Key.TriggerEvent("setInvincible", false);
                            Functions.disableAllPlayerControls(deathPlayer.Key, false);
                            deathPlayer.Key.Health = 200;
                            deathPlayer.Key.Armor = 0;

                            if (deathPlayer.Key.Dimension != Fraktionen.FraktionRegister.GangwarDimension)
                            {
                                Database.removeAllItems(Start.loggedInPlayers[deathPlayer.Key]);
                                deathPlayer.Key.RemoveAllWeapons();
                                foreach (WeaponHash weaponHash in Database.getLoadout(deathPlayer.Key.Name))
                                {
                                    Database.removeLoadout(deathPlayer.Key.Name, weaponHash);
                                }
                            }
                        }
                        else
                        {
                            Functions.disableAllPlayerControls(deathPlayer.Key, true);
                        }

                    });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        #region PRESS

        [RemoteEvent("Pressed_H")]
        public static void handsUp(Client p)
        {
            try
            {

                if (deathTime.ContainsKey(p) || (p.IsInVehicle))
                    return;

                if (p.HasData("handhoch"))
                {
                    NAPI.Player.PlayPlayerAnimation(p, 49, "mp_am_hold_up", "handsup_base");
                    p.ResetData("handhoch");
                }
                else
                {
                    NAPI.Player.StopPlayerAnimation(p);
                    p.SetData("handhoch", true);
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("Pressed_L")]
        public static void openCar(Client c)
        {
            VehicleMenu.REQUEST_VEHICLE_TOGGLE_LOCK_OUTSIDE(c);
        }

        [RemoteEvent("Pressed_J")]
        public static void salute(Client p)
        {
            try
            {

                if (deathTime.ContainsKey(p) || (p.IsInVehicle))
                    return;

                if (p.HasData("salute"))
                {
                    NAPI.Player.PlayPlayerAnimation(p, 49, "anim@mp_player_intincarsalutestd@ds@", "idle_a");
                    p.ResetData("salute");
                }
                else
                {
                    NAPI.Player.StopPlayerAnimation(p);
                    p.SetData("salute", true);
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
        #endregion 

        public static void OnMinuteSpent(object unused)
        {
            try
            {

                NativeManager.PosCheck();
                List<Client> keys = new List<Client>(loggedInPlayers.Keys);
                foreach (Client player in keys)
                {
                    NAPI.Task.Run(() =>
                    {
                        if (NAPI.Pools.GetAllPlayers().Contains(player))
                        {
                            if (player.Armor < 1)
                            {
                                player.SetClothes(9, 0, 0);
                            }
                            Database.SavePlayer(player);
                            player.SetSharedData("FRAKTION", Database.getPlayerFraktion(player.Name));
                            player.SetSharedData("FRAKTION_SHORT", Functions.getFraktionFromName(player.GetSharedData("FRAKTION")).shortName);
                            player.SetSharedData("FRAKTION_RANK", Database.getUserFraktionRank(player.Name));
                            player.SetSharedData("PLAYER_RANK", Database.getPlayerRank2(player.Name));
                            player.TriggerEvent("updateFraktion", player.GetSharedData("FRAKTION"));
                        }
                        else
                        {
                            loggedInPlayers.Remove(player);
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        public static void OnHourSpend()
        {
            try
            {
                List<Client> keys = new List<Client>(loggedInPlayers.Keys);
                foreach (Client player in keys)
                {
                    NAPI.Task.Run(() =>
                    {
                        if (Database.isPlayerTeam(player.Name))
                        {
                            Notification.SendPlayerNotifcation(player, "Sie haben ihren Payday erhalten. Schauen sie auf ihr Kontostand für mehr Information!", 7000, "green", "", "green");
                            Notification.SendPlayerNotifcation(player, "Glückwunsch! Du bist ein Level aufgestiegen.", 7000, "yellow", "", "green");
                            Notification.SendPlayerNotifcation(player, "Da du ein Teammitglied bist bekommst du 50.000$ mehr! Dein Rang: ( " + Database.getPlayerRank2(player.Name) + " )", 7000, "green", "", "green");
                            player.ResetData("IS_INVADER");
                            Database.changeMoney(loggedInPlayers[player], 125000, false);
                            Database.changePlaytime(player.SocialClubName, 1, false);
                        }
                        else
                        {
                            Notification.SendPlayerNotifcation(player, "Sie haben ihren Payday erhalten. Schauen sie auf ihr Kontostand für mehr Information!", 7000, "green", "", "green");
                            Notification.SendPlayerNotifcation(player, "Glückwunsch! Du bist ein Level aufgestiegen.", 7000, "yellow", "", "green");
                            player.ResetData("IS_INVADER");
                            Database.changeMoney(loggedInPlayers[player], 75000, false);
                            Database.changePlaytime(player.SocialClubName, 1, false);
                        }

                    });
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
    }
}

// Copyright© Test Server Script Dirion // --- Angemeldet 1-0 --- //
