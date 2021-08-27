using GTANetworkAPI;
using System;

namespace Venux
{
    class Anticheat : Script
    {

        [RemoteEvent("server:CheatDetection")]
        public void cheatDetection(Client p, string flag)
        {
            try
            {
                if (Start.deathTime.ContainsKey(p) || (Database.getPlayerRights(p.Name) > 3)) { return; }

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Database.getPlayerRights(target.Name) > 3)
                    {
                        Notification.SendPlayerNotifcation(target, "Der Spieler " + p.Name + " ist verdächtig. Flag: " + flag, 8000, "red", "ANTICHEAT", "red");
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("server:detectVehiclespeed")]
        public void vehicleSpeed(Client p)
        {
            try
            {
                if (Start.deathTime.ContainsKey(p) || (Database.getPlayerRights(p.Name) > 3)) { return; }

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Database.getPlayerRights(target.Name) > 3)
                    {
                        Notification.SendPlayerNotifcation(target, "Der Spieler " + p.Name + " ist verdächtig. Flag: VehicleSpeed", 8000, "red", "ANTICHEAT", "red");
                        p.Kick();
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("server:anticheat:detectGodmode")]
        public void detectGodmode(Client p)
        {
            try
            {
                if (Start.deathTime.ContainsKey(p) || (Database.getPlayerRights(p.Name) > 3)) { return; }

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Database.getPlayerRights(p.Name) > 3)
                    {
                        Notification.SendPlayerNotifcation(p, "Der Spieler " + p.Name + " ist verdächtig. Flag: GODMODE", 8000, "red", "ANTICHEAT", "red");
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        /* [RemoteEvent("server:anticheat:detectWeapon")]
         public void detectWeapon(Client p)
         {
             try
             {

                 foreach (Client target in NAPI.Pools.GetAllPlayers())
                 {
                     if (Database.getPlayerRights(target.Name) > 3)
                     {
                         Database.banPlayer(target.SocialClubName, "Hacker | Weapon Hack");
                         p.Kick();
                     }
                 }
             }
             catch (Exception ex) { Log.Write(ex.Message); }
         }*/

        // Vorrübergehend deaktiviert

        [RemoteEvent("Pressed_EINFG")]
        public static void einfügen(Client p)
        {
            try
            {
                if (Start.deathTime.ContainsKey(p)) { return; }

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Database.getPlayerRights(target.Name) > 3)
                    {
                        Notification.SendPlayerNotifcation(target, "Der Spieler " + p.Name + " drückt eine Taste. Key: EINFG", 8000, "red", "", "red");
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("Pressed_END")]
        public static void end(Client p)
        {
            try
            {
                if (Start.deathTime.ContainsKey(p)) { return; }

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (Database.getPlayerRights(target.Name) > 3)
                    {
                        Notification.SendPlayerNotifcation(target, "Der Spieler " + p.Name + " ist verdächtig. Flag: END", 8000, "red", "CHEAT ACTIVE", "red");
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void Wait(Client p)
        {
            try
            {
                /*if(p != null && p.Exists)
                    p.TriggerEvent("client:respawning");*/
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

    }
}

// Copyright© Test Server Script Dirion // --- Angemeldet 1-0 --- //
