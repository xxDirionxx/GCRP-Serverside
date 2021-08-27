using GTANetworkAPI;
using System;
using Venux.AdminSystem;
using Venux.Other;

namespace Venux.XMenu
{
    class PlayerMenu : Script
    {

        [RemoteEvent("REQUEST_PEDS_PLAYER_GIVEITEM")]
        public static void REQUEST_PEDS_PLAYER_GIVEITEM(Client c, Client giveItem = null)
        {
            if (Database.isPlayerDeath(c.Name))
            {
                return;
            }

            if (giveItem != null)
            {
                c.SetData("PLAYER_GIVEITEM", giveItem);
                Inventory.requestItems(c);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_GIVEMONEY_DIALOG")]
        public static void REQUEST_PEDS_PLAYER_GIVEMONEY_DIALOG(Client p, Client target)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }

            if (target != null)
            {
                p.TriggerEvent("openWindow", new object[] { "GiveMoney", "{\"targetName\": \"target.Name\"}" });
            }
        }

        [RemoteEvent("GivePlayerMoney")]
        public void GivePlayerMoney(Client client, string target, int amount)
        {
            if (amount == null) return;
            {
                try
                {
                    Client mptarget = NAPI.Player.GetPlayerFromName(target);

                    if (Functions.handcuffed.Contains(client.Name) || (Database.isPlayerDeath(client.Name)))
                        return;

                    if (amount < 1)
                        return;

                    if (Database.getMoney(client.Name) >= amount)
                    {

                        float distance = 9999999.0f;

                        foreach (Client p2 in NAPI.Pools.GetAllPlayers())
                        {
                            float distance2 = client.Position.DistanceTo(p2.Position);
                            if (distance2 < distance && p2 != client && distance2 < 3)
                            {
                                mptarget = p2;
                                distance = distance2;
                            }
                        }

                        if (mptarget != null)
                        {
                            DiscordWebhook.SendMessage("Spieler gibt Geld.", "Der Spieler " + client.Name + " gibt " + mptarget.Name + " " + amount + "$", DiscordWebhook.geldWebhook, "Geld-Log");
                            Database.changeMoney(client.Name, amount, true);
                            Database.changeMoney(mptarget.Name, amount, false);
                            Notification.SendPlayerNotifcation(client, "Du hast einer Person " + amount + "$ gegeben.", 5000, "green", "", "");
                            Notification.SendPlayerNotifcation(mptarget, "Du hast " + amount + "$ von " + client.Name + " erhalten.", 5000, "green", "", "");
                        }
                        else
                        {
                            Notification.SendPlayerNotifcation(client, "Es wurde kein Spieler gefunden.", 5000, "red", "FEHLER", "red");
                        }
                    }
                    else
                    {
                        Notification.SendPlayerNotifcation(client, "Du hast nicht genug Geld.", 5000, "red", "FEHLER", "red");
                    }
                }
                catch (Exception ex) { Log.Write(ex.Message); }
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_TIE")]
        public static void REQUEST_PEDS_PLAYER_TIE(Client p, Client target = null)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }
            if (Database.getItemCount(p.Name, "Seil") >= 1)

                if (target != null)
                {
                    if (!Functions.handcuffed.Contains(target.Name))
                    {
                        Functions.setHandcuff(target, true);
                        Notification.SendPlayerNotifcation(p, "Du hast " + target.Name + " erfolgreich gefesselt.", 5000, "green", "", "");
                        Notification.SendPlayerNotifcation(target, "Du wurdest von " + p.Name + " gefesselt.", 5000, "red", "", "");
                    }
                    else
                    {
                        Functions.setHandcuff(target, false);
                        Notification.SendPlayerNotifcation(p, "Du hast " + target.Name + " entfesselt.", 5000, "yellow", "SPIELERINTERAKTION", "");
                        Notification.SendPlayerNotifcation(target, "Du wurdest von " + p.Name + " entfesselt.", 5000, "yellow", "SPIELERINTERAKTION", "");
                    }
                }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_FRISK")]
        public static void REQUEST_PEDS_PLAYER_FRISK(Client p, Client target = null)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }
            if (target != null)
            {
                if (!Functions.handcuffed.Contains(target.Name))
                {
                    Notification.SendPlayerNotifcation(p, "Der Spieler ist nicht gefesselt.", 5000, "red", "", "");
                    return;
                }
                else if (Database.isPlayerDeath(target.Name))

                    Functions.disableAllPlayerControls(p, true);
                p.TriggerEvent("showProgress", 5000);
                NAPI.Player.PlayPlayerAnimation(p, 33, "mini@repair", "fixing_a_player", 8f);
                NAPI.Task.Run((() =>
                {
                    p.TriggerEvent("openWindow", "Inventory", "{\"inventory\":[{\"Id\":" + Database.getSQLId(p.Name) + ",\"Name\":\"Inventar von " + target + "\",\"Money\":" + Database.getMoney(target.Name) + ",\"Blackmoney\":" + Database.getBlackMoney(target.Name) + ",\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":15,\"Slots\":" + NAPI.Util.ToJson(Database.getUserItems(target.Name)) + "}]}");
                }), 5000);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_STABALIZE")]
        public static void REQUEST_PEDS_PLAYER_STABALIZE(Client p, Client target = null)
        {
            try
            {
                if (Database.isPlayerDeath(p.Name) || (Gangwar.GebietRegister.currentRunningGangwarGebiet != null && p.Dimension == Fraktionen.FraktionRegister.GangwarDimension) || (Database.getPlayerRights(p.Name) < 1))
                {
                    return;
                }
                if (p.HasData("IS_DOINSTABI") == true)
                {
                    Notification.SendPlayerNotifcation(p, "Du bist gerade am Stabilisieren", 3500, "grey", "", "");
                    return;
                }
                else
                if (Database.isPlayerDeath(target.Name))
                {
                    p.TriggerEvent("sendProgressbar", new object[1]
                    {
                        15000
                    });
                    p.SetData("IS_DOINSTABI", true);
                    NAPI.Player.PlayPlayerAnimation(p, 33, "amb@medic@standing@tendtodead@idle_a", "idle_a", 8f);
                    p.TriggerEvent("disableAllPlayerActions", new object[1]
                    {
                        true
                    });
                    NAPI.Task.Run(delegate
                    {
                        Database.setDeathStatus(target.Name, false);
                        Start.deathTime.Remove(target);
                        target.TriggerEvent("stopScreenEffect", "DeathFailOut");
                        target.TriggerEvent("setInvincible", false);
                        Functions.disableAllPlayerControls(target, false);
                        Anticheat.Wait(p); target.Health = 200;
                        target.Armor = 0;
                        Items.Weapons.WeaponSync(target);
                        NAPI.Player.SpawnPlayer(target, target.Position);
                        Notification.SendPlayerNotifcation(target, "Du wurdest stabilisiert", 4500, "lightblue", "", "");
                        Notification.SendPlayerNotifcation(p, "Du hast eine Person Stabilisiert!", 4500, "lightblue", "", "");
                        NAPI.Player.StopPlayerAnimation(p);
                        p.TriggerEvent("disableAllPlayerActions", new object[1]
                        {
                            false
                        });
                        p.ResetData("IS_DOINSTABI");
                        NAPI.Player.StopPlayerAnimation(target);
                    }, 15000);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_SHOW_PERSO")]
        public static void REQUEST_PEDS_PLAYER_SHOW_PERSO(Client p, Client target = null)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }
            Personalausweis.showPerso(target, p.Name.Split("_")[0], p.Name.Split("_")[1], Database.getPlayerFraktion(p.Name).ToString(), 0, 0, 0, 0);
            Notification.SendPlayerNotifcation(p, "Personalausweis gezeigt.", 4500, "gray", "", "");
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_SHOW_LIC")]
        public static void REQUEST_PEDS_PLAYER_SHOW_LIC(Client p, Client target = null)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }
            Lizenzen.showLicense(target, p.Name, "True", "", "", "", "", "", "", "", "", "", "", "");
            Notification.SendPlayerNotifcation(p, "Lizenzen gezeigt.", 4500, "gray", "", "");
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_GETPERSO")]
        public static void REQUEST_PEDS_PLAYER_GETPERSO(Client p, Client target = null)
        {
            if (Database.isPlayerDeath(p.Name))
            {
                return;
            }
            if (Database.isPlayerDeath(target.Name))
                Personalausweis.showPerso(p, target.Name.Split("_")[0], target.Name.Split("_")[1], Database.getPlayerFraktion(target.Name).ToString(), 0, 0, 0, 0);
        }

        [RemoteEvent("REQUEST_PEDS_PLAYER_GIVEMONEY")]
        public static void REQUEST_PEDS_PLAYER_GIVEMONEY(Client p, string value)
        {
            if (value != null)
            {
                try
                {
                    if (Functions.handcuffed.Contains(p.Name) || (Database.isPlayerDeath(p.Name)))
                        return;

                    int count = 0;
                    bool canConvert = int.TryParse(value, out count);

                    if (canConvert == false)
                        return;

                    if (count < 1)
                        return;

                    if (Database.getMoney(p.Name) >= count)
                    {

                        Client target = null;
                        float distance = 9999999.0f;

                        foreach (Client p2 in NAPI.Pools.GetAllPlayers())
                        {
                            float distance2 = p.Position.DistanceTo(p2.Position);
                            if (distance2 < distance && p2 != p && distance2 < 3)
                            {
                                target = p2;
                                distance = distance2;
                            }
                        }

                        if (target != null)
                        {
                            DiscordWebhook.SendMessage("Spieler gibt Geld.", "Der Spieler " + p.Name + " gibt " + target.Name + " " + count + "$", DiscordWebhook.geldWebhook, "Geld-Log");
                            Database.changeMoney(p.Name, count, true);
                            Database.changeMoney(target.Name, count, false);
                            Notification.SendPlayerNotifcation(p, "Du hast einer Person " + count + "$ gegeben.", 5000, "green", "", "");
                            Notification.SendPlayerNotifcation(target, "Du hast " + count + "$ von " + p.Name + " erhalten.", 5000, "green", "", "");
                        }
                        else
                        {
                            Notification.SendPlayerNotifcation(p, "Es wurde kein Spieler gefunden.", 5000, "red", "FEHLER", "red");
                        }
                    }
                    else
                    {
                        Notification.SendPlayerNotifcation(p, "Du hast nicht genug Geld.", 5000, "red", "FEHLER", "red");
                    }
                }
                catch (Exception ex) { Log.Write(ex.Message); }
            }
        }

        /*[RemoteEvent("REQUEST_PEDS_PLAYER_ROPES_FRONT")]
        public void REQUEST_PEDS_PLAYER_ROPES_FRONT(Client p, Client target = null)
        {
            try
            {
                if (target != null)
                {
                    if (!Functions.handcuffed.Contains(target.Name))
                    {
                        target.TriggerEvent("disableAllPlayerActions", false);
                        NAPI.Player.PlayPlayerAnimation(target, 33, "anim@move_m@prisoner__cuffed_rc", "aim_low_loop", 8);
                        Notification.SendPlayerNotifcation(target, "Dir wurden die Fesseln nach vorne gelegt", 4500, "grey", "", "");

                    }
                    else
                    {
                        Functions.setHandcuff(target, true);
                        Notification.SendPlayerNotifcation(target, "Dir wurden die Fesseln nach hinten gelegt", 4500, "grey", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }*/
    }
}

