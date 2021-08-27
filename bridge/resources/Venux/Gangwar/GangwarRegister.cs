using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Venux.Gangwar
{
    class GebietRegister : Script
    {
        public static List<Gebiet> gebietList = new List<Gebiet>();
        public static Dictionary<string, Blip> blipList = new Dictionary<string, Blip>();
        public static Gebiet currentRunningGangwarGebiet = null;
        public static DateTime gangwarStartTime = DateTime.Now;
        public static Marker currentGangwarMarker = null;
        public static List<Client> InGangwarMarker = new List<Client>();
        public static List<Gebiet> BlockedGebiete = new List<Gebiet>();
        public static Fraktionen.Fraktion fraktionOne = null;
        public static Fraktionen.Fraktion fraktionTwo = null;
        public static int pointsFraktionOne = 0;
        public static int pointsFraktionTwo = 0;
        public static Timer OnSecondSpentTimer;
        public static Timer OnTwentySecondSpentTimer;
        public static List<Client> InFlag = new List<Client>();
        public static int gangwarTime = 1950;

        [ServerEvent(Event.ResourceStart)]
        public void registerGebiete()
        {
            NAPI.Task.Run(() =>
            {
                if (gebietList.Count < 1)
                {
                    foreach (Gebiet gebiet in Database.getGebietList().ToArray())
                        gebietList.Add(gebiet);
                }

                foreach (Gebiet gebiet in gebietList)
                    loadGebiet(gebiet);

                OnSecondSpentTimer = new Timer(1000.0);
                OnSecondSpentTimer.AutoReset = true;
                OnSecondSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
                {
                    OnSecondSpent(null);
                };
                OnSecondSpentTimer.Start();

                OnTwentySecondSpentTimer = new Timer(25000.0);
                OnTwentySecondSpentTimer.AutoReset = true;
                OnTwentySecondSpentTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
                {
                    OnTwentySecondsSpent();
                };
                OnTwentySecondSpentTimer.Start();

                Log.Write("----> German Classic <----");
            }, 1000);

        }

        public void loadGebiet(Gebiet gebiet)
        {
            if (gebiet == null) { return; }

            if (Database.getGebietFraktion(gebiet.name).fraktionName == "Keine")
            {
                NAPI.Marker.CreateMarker(1, gebiet.position, new Vector3(), new Vector3(), 1f, new Color(255, 255, 255));
                blipList.Add(gebiet.name, NAPI.Blip.CreateBlip(543, gebiet.position, 1f, 0, gebiet.name, 255, 0, true, 0));
            }
            else
            {
                NAPI.Marker.CreateMarker(1, gebiet.position, new Vector3(), new Vector3(), 1f, Database.getFraktionByName(gebiet.fraktion).rgbColor);
                blipList.Add(gebiet.name, NAPI.Blip.CreateBlip(543, gebiet.position, 1f, (byte)Database.getFraktionByName(gebiet.fraktion).blipColor, gebiet.name + " - " + gebiet.fraktion, 255, 0, true, 0));
            }

            ColShape val = NAPI.ColShape.CreateCylinderColShape(gebiet.position, 1.4f, 1.4f, 0);
            val.SetData("COLSHAPE_FUNCTION", new FunctionModel("showGangwarMenu", gebiet.name));
            val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um den Gangwar zu starten.", "GANGWAR", "orange", 5000));

            NAPI.Marker.CreateMarker(4, gebiet.flagOne.Add(new Vector3(0.0f, 0.0f, 1.2f)), new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            NAPI.Marker.CreateMarker(4, gebiet.flagTwo.Add(new Vector3(0.0f, 0.0f, 1.2f)), new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            NAPI.Marker.CreateMarker(4, gebiet.flagThree.Add(new Vector3(0.0f, 0.0f, 1.2f)), new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            NAPI.Marker.CreateMarker(4, gebiet.flagFour.Add(new Vector3(0.0f, 0.0f, 1.2f)), new Vector3(), new Vector3(), 1f, new Color(255, 140, 0), false, (uint)Fraktionen.FraktionRegister.GangwarDimension);

            ColShape val2 = NAPI.ColShape.CreateCylinderColShape(gebiet.flagOne, 1.4f, 1.4f, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            val2.SetData("IS_FLAG", true);

            ColShape val3 = NAPI.ColShape.CreateCylinderColShape(gebiet.flagTwo, 1.4f, 1.4f, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            val3.SetData("IS_FLAG", true);

            ColShape val4 = NAPI.ColShape.CreateCylinderColShape(gebiet.flagThree, 1.4f, 1.4f, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            val4.SetData("IS_FLAG", true);

            ColShape val5 = NAPI.ColShape.CreateCylinderColShape(gebiet.flagFour, 1.4f, 1.4f, (uint)Fraktionen.FraktionRegister.GangwarDimension);
            val5.SetData("IS_FLAG", true);
        }

        [RemoteEvent("showGangwarMenu")]
        public void showGangwarMenu(Client p, string arg1)
        {
            if (arg1 == null) { return; }

            try
            {
                if (!Database.isPlayerInFrak(p, "Zivilist"))
                {
                    Gebiet gebiet = Database.getGebietByName(arg1);
                    p.TriggerEvent("openWindow", new object[2]
                        {
                                            "Confirmation",
                                            "{\"confirmationObject\":{\"Title\":\"Gangwar\",\"Message\":\"Möchtest du den Gangwar starten?\",\"Callback\":\"Gangwar\",\"Arg1\":\"" + arg1 + "\",\"Arg2\":\"\"}}"
                        });

                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist in keiner Fraktion. Es ist unmöglich ein Gangwar zu starten.", 5000, "red", "FEHLER", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("Gangwar")]
        public void startGangwar(Client p, string gebietname)
        {
            if (gebietname == null) { return; }

            Anticheat.Wait(p);

            try
            {
                Gebiet gebiet = Database.getGebietByName(gebietname);

                if (BlockedGebiete.Contains(gebiet))
                {
                    Notification.SendPlayerNotifcation(p, "Dieses Gangwar-Gebiet wurde bereits angegriffen.", 5000, "red", "", "white");
                    return;
                }

                if (currentRunningGangwarGebiet != null)
                {
                    Notification.SendPlayerNotifcation(p, "Derzeit läuft bereits ein Gangwar.", 5000, "red", "", "white");
                    return;
                }
                if (p.GetSharedData("FRAKTION") == "Zivilist")
                {
                    Notification.SendPlayerNotifcation(p, "Du kannst kein Gangwar starten da du in keiner Fraktion bist.", 5000, "red", "", "white");
                    return;
                }



                if (Database.getGebietFraktion(gebiet.name).fraktionName == "Keine" || Database.getGebietFraktion(gebiet.name).fraktionName == p.GetSharedData("FRAKTION"))
                {
                    Database.updateGebietFraktion(gebiet.name, p.GetSharedData("FRAKTION"));
                    Notification.SendPlayerNotifcation(p, "Deine Fraktion besitzt bereits das Gangwar-Gebiet.", 5000, "blue", "", "blue");
                }
                else
                {
                    pointsFraktionOne = 0;
                    pointsFraktionTwo = 0;
                    fraktionOne = Database.getFraktionByName(p.GetSharedData("FRAKTION"));
                    fraktionTwo = Database.getGebietFraktion(gebiet.name);
                    InGangwarMarker.Clear();
                    p.Dimension = (uint)Fraktionen.FraktionRegister.GangwarDimension;
                    currentGangwarMarker = NAPI.Marker.CreateMarker(1, gebiet.position.Subtract(new Vector3(0.0f, 0.0f, 12.0f)), new Vector3(), new Vector3(), gebiet.gebietRadius, new Color(255, 140, 0), false, (uint)Fraktionen.FraktionRegister.GangwarDimension);
                    ColShape val = NAPI.ColShape.CreateCylinderColShape(gebiet.position, gebiet.gebietRadius, gebiet.gebietRadius, (uint)Fraktionen.FraktionRegister.GangwarDimension);
                    val.SetData("COLSHAPE_IS_GANGWARZONE", true);
                    val.SetData("GEBIET", gebiet.name);
                    gangwarStartTime = DateTime.Now;
                    currentRunningGangwarGebiet = gebiet;
                    Color rgb = Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor;
                    string rgbString = "rgb(" + rgb.Red + ", " + rgb.Green + ", " + rgb.Blue + ")";
                    BlockedGebiete.Add(gebiet);

                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        if (p.GetSharedData("FRAKTION") == target.GetSharedData("FRAKTION"))
                        {
                            Notification.SendPlayerNotifcation(target, "Deine Fraktion greift nun das Gebiet " + gebiet.name + " an.", 5000, "red", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                        }
                    }

                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        if (Database.getGebietFraktion(gebiet.name).fraktionName == target.GetSharedData("FRAKTION"))
                        {
                            Notification.SendPlayerNotifcation(target, "Euer Gebiet " + gebiet.name + " wird von der Fraktion " + p.GetSharedData("FRAKTION") + " angegriffen.", 8000, "red", Database.getGebietFraktion(gebiet.name).fraktionName, "rgb(" + Database.getGebietFraktion(gebiet.name).rgbColor.Red + ", " + Database.getGebietFraktion(gebiet.name).rgbColor.Green + ", " + Database.getGebietFraktion(gebiet.name).rgbColor.Blue + ")");
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void onEnterColshape(ColShape colShape, Client player)
        {
            try
            {
                if (currentRunningGangwarGebiet == null) { return; }

                if (player.Dimension != Fraktionen.FraktionRegister.GangwarDimension) { return; }

                if (colShape.HasData("GEBIET") && colShape.GetData("GEBIET") == currentRunningGangwarGebiet.name)
                {

                    if (colShape.HasData("COLSHAPE_IS_GANGWARZONE"))
                    {
                        if (!InGangwarMarker.Contains(player))
                        {
                            InGangwarMarker.Add(player);
                        }
                        Anticheat.Wait(player);
                        player.RemoveAllWeapons();
                        player.GiveWeapon(WeaponHash.AdvancedRifle, 9999);
                        player.GiveWeapon(WeaponHash.Gusenberg, 9999);
                        player.GiveWeapon(WeaponHash.HeavyPistol, 9999);
                        NAPI.Player.SetPlayerCurrentWeapon(player, WeaponHash.Unarmed);
                        player.TriggerEvent("initializeGangwar", fraktionOne.shortName, fraktionTwo.shortName, Database.getFraktionSQLId(fraktionOne.fraktionName), Database.getFraktionSQLId(fraktionTwo.fraktionName), gangwarTime);

                        Notification.SendPlayerNotifcation(player, "Gangwar-Zone " + currentRunningGangwarGebiet.name + " beigetreten.", 5000, "green", "GANGWAR", "white");
                    }
                }

                if (colShape.HasData("IS_FLAG"))
                {
                    if (Gangwar.GebietRegister.currentRunningGangwarGebiet == null) { return; }

                    if (Start.deathTime.ContainsKey(player)) { return; }

                    if (!Gangwar.GebietRegister.InFlag.Contains(player))
                    {
                        Gangwar.GebietRegister.InFlag.Add(player);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        [Command("gangwardl", GreedyArg = true)]
        public void CMD_GangwarDL(Client p, string gebietname)
        {
            Gebiet gebiet = Database.getGebietByName(gebietname);
            {
                if (Database.getPlayerRights(p.Name) >= 99)
                    if (BlockedGebiete.Contains(gebiet))

                        BlockedGebiete.Remove(gebiet);
                Notification.SendPlayerNotifcation(p, "Du hast das Gebiet " + gebietname + " wieder angreifbar gemacht!", 5000, "red", "", "red");
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        public void onExitColshape(ColShape colShape, Client player)
        {
            try
            {
                if (player.Dimension != Fraktionen.FraktionRegister.GangwarDimension) { return; }

                if (colShape.HasData("GEBIET") && colShape.GetData("GEBIET") == currentRunningGangwarGebiet.name)
                {
                    if (colShape.HasData("COLSHAPE_IS_GANGWARZONE"))
                    {
                        if (InGangwarMarker.Contains(player))
                        {
                            InGangwarMarker.Remove(player);
                        }
                        Anticheat.Wait(player);
                        player.TriggerEvent("finishGangwar");
                        Start.deathTime.Remove(player);


                        NAPI.Task.Run(() =>
                        {
                            Anticheat.Wait(player);
                            player.RemoveAllWeapons();
                            Items.Weapons.WeaponSync(player);
                        }, 2000);

                    }
                }

                if (colShape.HasData("IS_FLAG"))
                {
                    if (Gangwar.GebietRegister.currentRunningGangwarGebiet == null) { return; }

                    if (Gangwar.GebietRegister.InFlag.Contains(player))
                    {
                        Gangwar.GebietRegister.InFlag.Remove(player);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void OnSecondSpent(object unused)
        {
            try
            {
                if (currentRunningGangwarGebiet != null)
                {
                    gangwarTime--;
                    if (gangwarTime < 1)
                    {
                        Fraktionen.Fraktion winner = null;
                        Fraktionen.Fraktion loser = null;

                        if (pointsFraktionOne > pointsFraktionTwo)
                        {
                            winner = fraktionOne;
                            loser = fraktionTwo;
                        }
                        else if (pointsFraktionOne < pointsFraktionTwo)
                        {
                            winner = fraktionTwo;
                            loser = fraktionOne;
                        }
                        else
                        {
                            winner = null;
                            loser = null;
                        }

                        if (winner == null)
                        {
                            gangwarStartTime = gangwarStartTime.AddMinutes(5);
                            return;
                        }
                        else
                        {
                            Color rgbOne = winner.rgbColor;
                            string rgbStringOne = "rgb(" + rgbOne.Red + ", " + rgbOne.Green + ", " + rgbOne.Blue + ")";

                            foreach (Client player in NAPI.Pools.GetAllPlayers())
                                Notification.SendGlobalVENUX(player, "Die Fraktion " + winner.fraktionName + " hat den Gangwar um das Gebiet " + currentRunningGangwarGebiet.name + " gegen die Fraktion " + loser.fraktionName + " gewonnen.", 8000, rgbStringOne, Notification.icon.dev);

                            NAPI.Task.Run(() =>
                            {
                                currentGangwarMarker.Delete();
                                currentGangwarMarker = null;

                                foreach (Marker marker in NAPI.Pools.GetAllMarkers())
                                {
                                    if (marker.Dimension == Fraktionen.FraktionRegister.GangwarDimension)
                                    {
                                        if (marker.MarkerType == 1 && marker.Color.Green == 140)
                                            marker.Delete();
                                    }
                                }
                            }, 500);

                            InGangwarMarker.Clear();
                            InFlag.Clear();
                            pointsFraktionOne = 0;
                            pointsFraktionTwo = 0;

                            foreach (Client fraktionPlayer in NAPI.Pools.GetAllPlayers())
                            {
                                if (fraktionPlayer.HasSharedData("FRAKTION") && fraktionPlayer.GetSharedData("FRAKTION") == fraktionOne.fraktionName)
                                {
                                    fraktionPlayer.Position = fraktionOne.spawnPoint.Add(new Vector3(0.0f, 0.0f, 1.5f));
                                    fraktionPlayer.Dimension = 0;
                                    fraktionPlayer.TriggerEvent("finishGangwar");
                                }
                                else if (fraktionPlayer.HasSharedData("FRAKTION") && fraktionPlayer.GetSharedData("FRAKTION") == fraktionTwo.fraktionName)
                                {
                                    fraktionPlayer.Position = fraktionTwo.spawnPoint.Add(new Vector3(0.0f, 0.0f, 1.5f));
                                    fraktionPlayer.Dimension = 0;
                                    fraktionPlayer.TriggerEvent("finishGangwar");
                                }
                            }

                            fraktionOne = null;
                            fraktionTwo = null;
                            NAPI.ClientEvent.TriggerClientEventInDimension((uint)Fraktionen.FraktionRegister.GangwarDimension, "finishGangwar");

                            foreach (Client player in NAPI.Pools.GetAllPlayers())
                            {
                                if (player.Dimension == Fraktionen.FraktionRegister.GangwarDimension)
                                {
                                    Items.Weapons.WeaponSync(player);



                                }
                                if (Start.deathTime.ContainsKey(player))
                                {
                                    Database.setDeathStatus(player.Name, false);
                                    Start.deathTime.Remove(player);
                                    NAPI.Player.SpawnPlayer(player, player.Position);
                                    player.TriggerEvent("stopScreenEffect", "DeathFailOut");
                                    player.TriggerEvent("setInvincible", false);
                                    Functions.disableAllPlayerControls(player, false);
                                    Anticheat.Wait(player); player.Health = 200;
                                    player.Armor = 0;
                                    Items.Weapons.WeaponSync(player);
                                    NAPI.Player.StopPlayerAnimation(player);
                                }
                            }
                            Database.updateGebietFraktion(currentRunningGangwarGebiet.name, winner.fraktionName);
                            blipList[currentRunningGangwarGebiet.name].Color = winner.blipColor;
                            blipList[currentRunningGangwarGebiet.name].Name = currentRunningGangwarGebiet.name + " - " + winner.fraktionName;
                            currentRunningGangwarGebiet = null;
                            gangwarTime = 1900;
                        }
                    }

                    foreach (Client p in InGangwarMarker.ToArray())
                    {
                        Color rgbOne = fraktionOne.rgbColor;
                        string rgbStringOne = "rgb(" + rgbOne.Red + ", " + rgbOne.Green + ", " + rgbOne.Blue + ")";

                        Color rgbTwo = fraktionTwo.rgbColor;
                        string rgbStringTwo = "rgb(" + rgbTwo.Red + ", " + rgbTwo.Green + ", " + rgbTwo.Blue + ")";

                        TimeSpan span = (DateTime.Now - gangwarStartTime);
                        p.TriggerEvent("updateGangwarScore", pointsFraktionOne, pointsFraktionTwo);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void OnTwentySecondsSpent()
        {
            try
            {
                if (currentRunningGangwarGebiet == null) { return; }

                foreach (Client p in InFlag)
                {
                    if (!p.HasSharedData("FRAKTION") || (Start.deathTime.ContainsKey(p))) { return; }

                    if (p.GetSharedData("FRAKTION") == fraktionOne.fraktionName)
                    {
                        Gangwar.GebietRegister.pointsFraktionOne = Gangwar.GebietRegister.pointsFraktionOne + 2;
                        foreach (Client target in NAPI.Pools.GetAllPlayers())
                        {
                            if (target.HasSharedData("FRAKTION"))
                            {
                                if (target.GetSharedData("FRAKTION") == fraktionOne.fraktionName)
                                {
                                    Notification.SendPlayerNotifcation(target, "+2 Punkte für das Besetzen einer Flagge", 5000, "green", fraktionOne.fraktionName, "");
                                }
                            }
                        }
                    }

                    if (p.HasSharedData("FRAKTION") && p.GetSharedData("FRAKTION") == fraktionTwo.fraktionName)
                    {
                        Gangwar.GebietRegister.pointsFraktionTwo = Gangwar.GebietRegister.pointsFraktionTwo + 2;
                        foreach (Client target in NAPI.Pools.GetAllPlayers())
                        {
                            if (target.HasSharedData("FRAKTION"))
                            {
                                if (target.GetSharedData("FRAKTION") == fraktionTwo.fraktionName)
                                {
                                    Notification.SendPlayerNotifcation(target, "+2 Punkte für das Besetzen einer Flagge", 5000, "green", fraktionTwo.fraktionName, "rgb(" + fraktionTwo.rgbColor.Red + ", " + fraktionTwo.rgbColor.Green + ", " + fraktionTwo.rgbColor.Blue + ")");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}

// Copyright© Test Server Script Dirion // --- Angemeldet 1-0 --- //
