using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Venux.AdminSystem
{
    class AdminCommand : Script
    {
        public static List<Client> adminDuty = new List<Client>();
        public static List<Client> admin = new List<Client>();

        [Command("tune")]
        public void CMD_Tune(Client p, int id1, int id2)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (Database.getPlayerRights(p.Name) >= 97)
                {
                    if (p.IsInVehicle)
                    {
                        p.Vehicle.SetMod(id1, id2);
                        Database.changeVehicleTuning(p.Vehicle.NumberPlate, id1, id2);
                        Notification.SendPlayerNotifcation(p, "Du hast dein Fahrzeug erfolgreich getuned.", 5000, "blue", "", "blue");
                    }
                    else
                    {
                        Notification.SendPlayerNotifcation(p, "Du sitzt in keinem Fahrzeug.", 5000, "blue", "", "blue");
                        return;
                    }

                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast keine Berechtigung für diesen Befehl.", 5000, "white", "", "blue");
                    return;
                }

            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        /*   [Command("color")]
           public void CMD_Color(Client p, int id1, int id2, int id3)
           {

               if (p.GetSharedData("istEingeloggt") < 9)
               {
                   p.SendNotification("~r~Sie sind nicht eingeloggt!");
                   p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                   return;
               }

               {
                   if (Database.getPlayerRights(p.Name) >= 97)
                   {
                       if (p.IsInVehicle)
                       {
                           p.Vehicle.CustomPrimaryColor = new Color(id1, id2, id3);
                           p.Vehicle.CustomSecondaryColor = new Color(id1, id2, id3);
                           Notification.SendPlayerNotifcation(p, "Du hast die Farbe geändert.", 5000, "red", "", "blue");
                       }
                       else
                       {
                           Notification.SendPlayerNotifcation(p, "Du sitzt in keinem Fahrzeug.", 5000, "red", "", "blue");
                           return;
                       }

                   }
                   else
                   {
                       Notification.SendPlayerNotifcation(p, "Du hast keine Berechtigung für diesen Befehl.", 5000, "red", "FEHLER", "red");
                       return;
                   }
               }
               catch (Exception ex) { Log.Write(ex.Message); }

           }*/

        [RemoteEvent("aduty:noclip")]
        public void ADuty_NoClip(Client p)
        {
            try
            {
                if (isPlayerInAduty(p))
                    p.TriggerEvent("aduty:togglenoclip");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("giveitem")]
        public void CMD_GiveWeapon(Client p, string item, int count)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (Database.getPlayerRights(p.Name) >= 99)
                {
                    Database.changeInventoryItem(p.Name, item, count, false);
                    Notification.SendPlayerNotifcation(p, "Du hast dir das Item " + item + " gegeben.", 5000, "red", "", "red");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("addmoney")]
        public void CMD_AddMoney(Client p, decimal value)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (Database.getPlayerRights(p.Name) >= 99)
                {
                    Database.changeMoney(p.Name, value, false);
                    Notification.SendPlayerNotifcation(p, "Du hast dir das Geld gegeben.", 5000, "red", "", "white");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        // -------> ADMIN-DUTY <-------- //

        [Command("aduty")]
        public void CMD_ADuty(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("Sie sind nicht eingeloggt!");
                p.SendNotification("Loggen sie sich ein um Befehle ausführen.");
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }
                if (isPlayerInAduty(p))
                {
                    Database.restoreCustomization(p);
                    adminDuty.Remove(p);
                    p.TriggerEvent("aduty:toggleaduty", false);
                    p.TriggerEvent("setPlayerAdutyVCL", !p.GetSharedData("aduty"));
                    p.TriggerEvent("updateAdutyCLV", !p.GetSharedData("aduty"));
                    p.TriggerEvent("setInvincible", false);
                    NAPI.Player.SpawnPlayer(p, new Vector3(p.Position.X, p.Position.Y, p.Position.Z + 01), 0);
                    p.SetSharedData("aduty", false);
                    Items.Weapons.WeaponSync(p);
                }
                else
                //Admin Dienst Betreten
                {
                    NAPI.Task.Run(() =>
                    Clothing.PlayerClothes.setAdmin(p, Database.getPlayerRank(p.Name).clothId));
                    adminDuty.Add(p);
                    p.TriggerEvent("aduty:toggleaduty", true);
                    p.TriggerEvent("setInvincible", true);
                    p.TriggerEvent("setPlayerAdutyVCL", !p.GetSharedData("aduty"));
                    p.TriggerEvent("updateAdutyCLV", !p.GetSharedData("aduty"));
                    NAPI.Player.SpawnPlayer(p, new Vector3(p.Position.X, p.Position.Y, p.Position.Z + 01), 0);
                    DiscordWebhook.SendMessage("Admin-Duty", "Teammitglied " + p.Name + " ist nun Aduty.", DiscordWebhook.adminWebhook, "Aduty-Log");
                    p.SetSharedData("aduty", true);
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
        // ----------> ADUTY-END <---------- //

        [Command("tpm", Alias = "tptowaypoint")]
        public void CMD_TPM(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "ADMINISTATION", "red"); return; }

                if (isPlayerInAduty(p))
                {
                    Notification.SendPlayerNotifcation(p, "Du hast dich zu deinem Wegpunkt teleportiert.", 5000, "red", "ADMINISTATION", "white");
                    DiscordWebhook.SendMessage("Admin-TP", "Das Teammitglied " + p.Name + " hat sich zum Wegpunkt Teleportiert.", DiscordWebhook.adminWebhook, "Admin-Wegpunkt-TP");
                    p.TriggerEvent("aduty:tptowayVENUX");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht im Admindienst.", 5000, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("setped")]
        public void CMD_PED(Client p, int hash)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (hash == 0)
                {
                    p.SetSkin((PedHash)1885233650);
                }
                else
                {
                    p.SetSkin((PedHash)hash);
                    Notification.SendPlayerNotifcation(p, "Spieler-Skin auf " + hash + " gesetzt.", 5000, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("info")]
        public void CMD_Info(Client p, string name)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                int warns = 0;

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Start.loggedInPlayers.ContainsKey(target))
                    {
                        if (Start.loggedInPlayers[target] == name)
                        {
                            warns = (int)Database.getWarns(target.SocialClubName);
                        }
                    }
                }
                Notification.SendPlayerNotifcation(p, "Warns von " + name + ": " + warns, 5000, "red", "ADMINISTATION", "white");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("warn")]
        public void CMD_Warn(Client p, string name)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                int warns = 0;

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Start.loggedInPlayers.ContainsKey(target))
                    {
                        if (Start.loggedInPlayers[target] == name)
                        {
                            Database.changeWarns(target.SocialClubName, 1, false);
                            warns = (int)Database.getWarns(target.SocialClubName);
                            Notification.SendPlayerNotifcation(target, "Du hast einen Warn bekommen!", 5000, "red", "ADMINISTATION", "white");
                            target.Kick("Du wurdest gekickt: Warn");
                        }
                    }
                }
                Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + name + " gewarnt. Warns von " + name + ": " + warns, 3500, "red", "ADMINISTATION", "white");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("xcm", GreedyArg = true)]
        public void CMD_XCM(Client p, string arg)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }

            if (arg == "Paco_White")
            {
                return;
            }

            if (arg == "Teka_Abi")
            {
                return;
            }

            try
            {
                string[] args = arg.Split(" ");

                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                string targetname = args[0];
                string reason = arg.Replace(targetname + " ", "").Replace(targetname, "");

                if (Database.getPlayerRights(p.Name) > 94)
                {
                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        if (Start.loggedInPlayers.ContainsKey(target))
                        {
                            if (Start.loggedInPlayers[target] == targetname)
                            {
                                DiscordWebhook.SendMessage("Spieler gebannt", "Der Spieler " + p.Name + " hat den Spieler " + target.Name + " gebannt. Grund: " + reason, DiscordWebhook.banWebhook, "Ban-Log");
                                Database.banPlayer(target.SocialClubName, reason);
                                NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, "Der Spieler " + target.Name + " wurde aus der Community ausgeschlossen.", 8000, "#f30202", Notification.icon.dev));
                                target.Kick();
                            }
                        }
                    }
                    Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + targetname + " gebannt. Dein Rang: " + Database.getPlayerRank(p.Name).rankName + "", 3500, "red", "", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("unban")]
        public void CMD_Unban(Client p, string arg)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                string targetname = arg;

                if (Database.getPlayerRights(p.Name) > 95)
                {
                    Database.unbanPlayer(targetname);
                    Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + targetname + " entbannt.", 3500, "red", "", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }



        [Command("kick", GreedyArg = true)]
        public void CMD_KICK(Client p, string arg)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                string[] args = arg.Split(" ");

                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                string targetname = args[0];
                string reason = arg.Replace(targetname + " ", "").Replace(targetname, "");

                if (Database.getPlayerRights(p.Name) > 90)
                {
                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        if (Start.loggedInPlayers.ContainsKey(target))
                        {
                            if (Start.loggedInPlayers[target] == targetname)
                            {
                                DiscordWebhook.SendMessage("Spieler gekickt", "Der Spieler " + p.Name + " hat den Spieler " + target.Name + " gekickt. Grund: " + reason, DiscordWebhook.kickWebhook, "Kick-Log");
                                NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, "Der Spieler " + target.Name + " wurde von " + Database.getPlayerRank(p.Name).rankName + " " + p.Name + " gekickt.", 8000, "#f30202", Notification.icon.dev));
                                Notification.SendPlayerNotifcation(target, "Du wurdest vom Server gekickt! Grund: " + reason + "", 3500, "red", "ADMINISTATION", "white");
                                target.TriggerEvent("openWindow", new object[] { "Kick", "{\"targetName\": \"target.Name\"}" });
                                target.Kick("Du wurdest gekickt. Grund: " + reason);
                            }
                        }
                    }
                    Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + targetname + " gekickt.", 3500, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("bwp")]
        public void CMD_Bwp(Client p, string fraktion)
        {

            if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }

            if (fraktion is "LSV")
            {
                Notification.SendGlobalVENUX("Vagos hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "MG13")
            {
                Notification.SendGlobalVENUX("MG13 hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "Orga")
            {
                Notification.SendGlobalVENUX("Organisazija hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "LCN")
            {
                Notification.SendGlobalVENUX("La Cosa Nostra hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "Triaden")
            {
                Notification.SendGlobalVENUX("Triaden hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "Grove")
            {
                Notification.SendGlobalVENUX("Grove Street hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "Lost")
            {
                Notification.SendGlobalVENUX("Lost MC hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "Ballas")
            {
                Notification.SendGlobalVENUX("Front Yard Ballas hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "Midnight")
            {
                Notification.SendGlobalVENUX("Midnight Club hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }

            if (fraktion is "YakuZa")
            {
                Notification.SendGlobalVENUX("YakuZa hat eine offene Bewerbungsphase! 20 Minuten Safezone.", 15000, "red", Notification.icon.casino);
            }
        }

        [Command("av")]
        public void CMD_AdminCar(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (isPlayerInAduty(p))
                {
                    Vehicle veh = NAPI.Vehicle.CreateVehicle(0xe7d2a16e, p.Position, 123, 5, 5);
                    veh.CustomPrimaryColor = Database.getPlayerRank(p.Name).rgbColor;
                    veh.CustomSecondaryColor = Database.getPlayerRank(p.Name).rgbColor;
                    veh.Dimension = p.Dimension;
                    p.SetIntoVehicle(veh, -1);
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht im Admindienst.", 5000, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("hash")]
        public void HandleHash(Client p, string vehName)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (isPlayerInAduty(p))
                {
                    uint hash = NAPI.Util.GetHashKey(vehName);
                    Notification.SendPlayerNotifcation(p, "" + vehName + " Hash: " + hash + "", 5000, "red", "ADMINISTATION", "white");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht im Admindienst.", 5000, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("goto")]
        public void CMD_GOTO(Client p, string name)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                Vector3 targetPos = new Vector3(0, 0, 0);

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Start.loggedInPlayers.ContainsKey(target))
                    {
                        if (Start.loggedInPlayers[target] == name)
                        {
                            targetPos = target.Position;
                        }
                    }
                }

                if (targetPos.X == 0 && targetPos.Y == 0 && targetPos.Z == 0)
                {
                    Notification.SendPlayerNotifcation(p, "Der Spieler wurde nicht gefunden.", 3500, "red", "ADMINISTATION", "red");
                }
                else
                {
                    Anticheat.Wait(p); p.Position = targetPos;
                    Notification.SendPlayerNotifcation(p, "Du hast dich zu Spieler " + name + " teleportiert.", 3500, "red", "ADMINISTATION", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("tc")]
        public static void CMD_TeamChat(Client p, string message)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }

            try
            {
                if (Database.getPlayerRights(p.Name) > 3)
                {
                    Notification.SendPlayerNotifcation(p, "Nachricht:" + message, 10000, "yellow", "TEAM-CHAT - " + p.Name, "yellow");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("bring")]
        public static void CMD_BRING(Client p, string name)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Start.loggedInPlayers.ContainsKey(target))
                    {
                        if (Start.loggedInPlayers[target] == name)
                        {
                            target.Position = p.Position;
                        }
                    }
                }


                Notification.SendPlayerNotifcation(p, "Du hast Spieler " + name + " zu dir teleportiert.", 3500, "red", "ADMINISTATION", "red");
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("tppos", Alias = "tp")]
        public void CMD_TP(Client p, float x, float y, float z)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (isPlayerInAduty(p))
                {
                    Notification.SendPlayerNotifcation(p, "Du hast dich zu den Koordinaten teleportiert.", 5000, "red", "ADMINISTATION", "red");
                    Anticheat.Wait(p); p.Position = new Vector3(x, y, z);
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht im Admindienst.", 5000, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("pos", Alias = "position")]
        public void CMD_Pos(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                Log.Write("POS: " + p.Position.X.ToString().Replace(",", ".") + ", " + p.Position.Y.ToString().Replace(",", ".") + ", " + (p.Position.Z - 1.1f).ToString().Replace(",", ".") + " ROT: " + p.Rotation.ToString().Replace(",", "."));
                Notification.SendPlayerNotifcation(p, "Deine Position wurde nun erfolgreich eingetragen.", 8000, "red", "ADMINISTATION", "white");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("dimension")]
        public void CMD_Dimension(Client p, int dimension)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (Database.getPlayerRights(p.Name) >= 98)
                {
                    p.Dimension = (uint)dimension;
                    Notification.SendPlayerNotifcation(p, "Deine Dimension wurde auf " + dimension + " gesetzt.", 5000, "red", "ADMINISTATION", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("announce", GreedyArg = true)]
        public void CMD_Announce(Client p, int toggle, string message)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                string[] arg = message.Split(" ");

                if (Database.getPlayerRights(p.Name) >= 95)
                {
                    if (toggle == 2)
                    {
                        NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, "Die Fraktion " + message + " sucht neue Member! (30 Minuten Schutz)", 8000, "orange", Notification.icon.gov));
                    }
                    else
                    {
                        NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, message, 8000, "red", Notification.icon.gov));
                    }
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast keine Berechtigung.", 5000, "red", "", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("restart")]
        public void CMD_Restart(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            if (Database.getPlayerRights(p.Name) >= 99)
                NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, "[AUTO-RESTART] In 15 Sekunden findet ein Server-Restart statt!", 8000, "#f37f02", Notification.icon.glob));

            Task.Run(() =>
            {
                Task.Delay(1000 * 15 * 1).Wait();

                Environment.Exit(0);
            });
        }

        [Command("clearcar")]
        public void CMD_Clearcar(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            if (Database.getPlayerRights(p.Name) >= 99)
                NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, "[AUTO-CLEAR] Alle Fahrzeuge werden in 15 Sekunden ADMINISTRATIV eingeparkt!", 8000, "#02f3d7", Notification.icon.dev));
            Task.Run(() =>
            {
                Task.Delay(1000 * 15 * 1).Wait();

                Database.parkAllVehicles();
            });
        }

        [Command("krieg", GreedyArg = true)]
        public void CMD_Krieg(Client p, string ID1, string ID2)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            if (Database.getPlayerRights(p.Name) >= 99)
                try
                {
                    NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, "Die Fraktion " + ID1 + " hat den Kriegsvertrag gegen die Fraktion " + ID2 + " unterschrieben!", 35000, "#f37f02", Notification.icon.dev));
                    NAPI.World.SetWeather(Weather.THUNDER);
                    NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", true));
                    NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("sound:playPurge"));
                    NAPI.Task.Run(delegate
                    {
                        NAPI.World.SetWeather(Weather.EXTRASUNNY);
                        NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", false));
                    }, 31800L);
                }
                catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("v")]
        public void CMD_Vanish(Client p, int name)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }

            try
            {

                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (isPlayerInAduty(p))
                {
                    if (name == 1)
                    {
                        p.Transparency = 0;
                        Notification.SendPlayerNotifcation(p, "Du bist nun unsichtbar.", 3500, "red", "ADMINISTRATION", "red");
                    }
                    else if (name == 0)
                    {
                        p.Transparency = 255;
                        Notification.SendPlayerNotifcation(p, "Du bist nun sichtbar.", 3500, "red", "ADMINISTRATION", "red");
                    }
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht im Admindienst.", 3500, "red", "ADMINISTRATION", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("timeban")]
        public void CMD_timeban(Client p, string arg, string time)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                string[] args = arg.Split(" ");

                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }
                DateTime now = DateTime.Now;
                DateTime hours = now.AddHours(Convert.ToDouble(time));
                string targetname = args[0];
                string reason = arg.Replace(targetname + " ", "").Replace(targetname, "");

                if (Database.getPlayerRights(p.Name) > 95)
                {
                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        if (Start.loggedInPlayers.ContainsKey(target))
                        {
                            if (Start.loggedInPlayers[target] == targetname)
                            {
                                DiscordWebhook.SendMessage("Spieler gebannt", "Teammitglied " + p.Name + " hat den Spieler " + target.Name + " gebannt. Grund: " + reason, DiscordWebhook.banWebhook, "Ban-Log");
                                Database.banPlayer(target.SocialClubName, reason);
                                NAPI.Pools.GetAllPlayers().ForEach(player => Notification.SendGlobalVENUX(player, Database.getPlayerRank(p.Name).rankName + " " + p.Name + " hat " + target.Name + " aus der Community ausgeschlossen!", 8000, "#f30202", Notification.icon.gov));
                                target.Kick();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("setclothes")]
        public void Setclothes(Client p, int slot, int drawable, int texture)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {

                if (Database.getPlayerRights(p.Name) > 97)
                {
                    var clothDictionary = new Dictionary<int, ComponentVariation>();
                    clothDictionary.Add(slot, new ComponentVariation { Drawable = drawable, Texture = texture });
                    NAPI.Player.SetPlayerClothes(p, clothDictionary);
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 5000, "red", "", "green");
                }
            }
            catch (FormatException ex)
            {
                Log.Write(ex.Message);
            }

        }

        [Command("veh", Alias = "car")]
        public void CMD_Veh(Client p, string vehname)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                VehicleHash vehHash = (VehicleHash)NAPI.Util.GetHashKey(vehname);
                if (vehHash == 0)
                {
                    Notification.SendPlayerNotifcation(p, "Ungültiges Fahrzeug.", 5000, "red", "INFORMATION", "red");
                    return;
                }
                Vehicle veh = NAPI.Vehicle.CreateVehicle(vehHash, p.Position, p.Rotation, 5, 5);
                veh.NumberPlate = "Admin";
                veh.Dimension = p.Dimension;
                p.SetIntoVehicle(veh, -1);
                Notification.SendPlayerNotifcation(p, "Das Fahrzeug wurde erfolgreich gespawnt.", 5000, "white", "INFORMATION", "red");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("setfraktion", GreedyArg = true)]
        public void CMD_Setfrak(Client p, string target, string fraktion)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (Database.getPlayerRights(p.Name) >= 97)
                    Database.setUserFraktion(target, fraktion);
                Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + target + " in die Fraktion " + fraktion + " gesetzt!", 5000, "red", "", "red");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("vrank", GreedyArg = true)]
        public void CMD_SetRank(Client p, string target, string rank)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (Database.getPlayerRights(p.Name) >= 100)
                    Database.setRights(target, rank);
                Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + target + " Rang  " + rank + "  gesetzt!", 5000, "red", "", "red");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("kill", GreedyArg = true)]
        public void CMD_Kill(Client p)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (Database.getPlayerRights(p.Name) >= 90)
                    p.Health = 0;
                Notification.SendPlayerNotifcation(p, "Du hast dich selber getötet!", 5000, "red", "", "red");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("dv", Alias = "delcar")]
        public void CMD_DV(Client p)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }
                if (p.IsInVehicle)
                {
                    p.Vehicle.Delete();
                    Notification.SendPlayerNotifcation(p, "Dein Fahrzeug wurde entfernt.", 5000, "red", "", "red");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du sitzt in keinem Fahrzeug.", 5000, "red", "", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("addschwarzgeld")]
        public void CMD_AddblackMoney(Client p, string name, int amount)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (Database.getPlayerRights(p.Name) >= 99)
                {
                    Database.changeBlackMoney(name, amount, true);
                    p.TriggerEvent("updateBlackMoney", amount);
                    Notification.SendPlayerNotifcation(p, "Du hast dir " + amount + " Schwarzgeld gegeben", 4500, "red", "Administration", "");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [Command("revive")]
        public static void CMD_Revive(Client p, string name)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

                if (Database.getPlayerRights(p.Name) < 92)
                {
                    Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red");
                }
                else
                {
                    if (name == "me")
                    {
                        NAPI.Task.Run(() =>
                        DiscordWebhook.SendMessage("Spieler revived", "Teammitglied " + p.Name + " hat sich selber revived.", DiscordWebhook.reviveWebhook, "Revive-Log"));
                        Database.setDeathStatus(p.Name, false);
                        Notification.SendPlayerNotifcation(p, "Du hast dich wiederbelebt.", 5000, "orange", "", "orange");
                        Start.deathTime.Remove(p);
                        NAPI.Player.SpawnPlayer(p, p.Position);
                        p.TriggerEvent("stopScreenEffect", "DeathFailOut");
                        p.TriggerEvent("setInvincible", false);
                        p.TriggerEvent("disableAllPlayerActions", false);
                        p.TriggerEvent("client:respawning");
                        p.Health = 200;
                        Items.Weapons.WeaponSync(p);
                        p.StopAnimation();
                    }
                    else
                    {
                        bool found = false;
                        foreach (KeyValuePair<Client, string> item in Start.loggedInPlayers)
                        {
                            if (item.Value == name)
                            {
                                NAPI.Task.Run(() =>
                                found = true);
                                DiscordWebhook.SendMessage("Spieler revived", "Teammitglied " + p.Name + " hat den Spieler " + item.Key.Name + " revived.", DiscordWebhook.reviveWebhook, "Kick-Log");
                                Notification.SendPlayerNotifcation(p, "Du hast den Spieler " + name + " wiederbelebt.", 5000, "red", "INFORMATION", "red");
                                Database.setDeathStatus(item.Key.Name, false);
                                Notification.SendPlayerNotifcation(item.Key, "Du wurdest von Teammitglied " + p.Name + " wiederbelebt.", 5000, "red", "", "red");
                                Start.deathTime.Remove(item.Key);
                                NAPI.Player.SpawnPlayer(item.Key, item.Key.Position);

                                item.Key.TriggerEvent("stopScreenEffect", "DeathFailOut");
                                item.Key.TriggerEvent("setInvincible", false);
                                item.Key.TriggerEvent("disableAllPlayerActions", false);
                                item.Key.TriggerEvent("client:respawning");
                                item.Key.Health = 200;
                                Items.Weapons.WeaponSync(item.Key);
                                item.Key.StopAnimation();
                            }
                        }

                        if (!found)
                            Notification.SendPlayerNotifcation(p, "Der Spieler wurde nicht gefunden.", 5000, "white", "INFORMATION", "white");

                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("dvall")]
        public void CMD_DVALL(Client p)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (Database.getPlayerRights(p.Name) > 98)
                {
                    foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                    {
                        vehicle.Delete();
                    }
                    Notification.SendPlayerNotifcation(p, "Alle Fahrzeuge gelöscht.", 5000, "red", "", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("dvradius")]
        public void CMD_DVALL(Client p, int radius)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (Database.getPlayerRights(p.Name) > 98)
                {
                    foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                    {
                        if (p.Position.DistanceTo(vehicle.Position) < radius)
                            vehicle.Delete();
                    }
                    Notification.SendPlayerNotifcation(p, "Alle in Fahrzeuge im Radius gelöscht.", 5000, "red", "", "red");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static bool isPlayerInAduty(Client p)
        {
            return adminDuty.Contains(p);
        }
    }
}

// Copyright© Test Server Script Dirion // --- Angemeldet 1-0 --- //
