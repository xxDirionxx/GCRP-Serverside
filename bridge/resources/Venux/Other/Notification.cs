using GTANetworkAPI;
using System;
using System.Diagnostics;

namespace Venux
{
    public class Notification
    {
        public class Message
        {
            public string message
            {
                get;
                set;
            }

            public string title
            {
                get;
                set;
            }

            public string color
            {
                get;
                set;
            }

            public int duration
            {
                get;
                set;
            }

            public Message(string message, string title, string color, int duration)
            {
                this.message = message;
                this.title = title;
                this.color = color;
                this.duration = duration;
            }
        }

        public enum icon
        {
            glob,
            gov,
            dev,
            wed,
            casino
        }

        public static void SendGlobalVENUX(Client player, string message, int durationInMS, string color, icon ico)
        {
            string text = "";
            if (ico == icon.glob)
            {
                text = "glob";
            }
            if (ico == icon.gov)
            {
                text = "gov";
            }
            if (ico == icon.dev)
            {
                text = "dev";
            }
            if (ico == icon.wed)
            {
                text = "wed";
            }
            if (ico == icon.casino)
            {
                text = "casino";
            }
            player.TriggerEvent("sendGlobalVENUX", new object[] { message, durationInMS, color, text });
        }

        public static void SendGlobalVENUX(Client player, string message, int durationInMS, string color, string ico)
        {
            player.TriggerEvent("sendGlobalVENUX", new object[4]
            {
                message,
                durationInMS,
                color,
                ico
            });
        }

        public static void SendGlobalVENUX(string message, int durationInMS, string color, icon ico)
        {
            foreach (Client target in NAPI.Pools.GetAllPlayers())
                target.TriggerEvent("sendGlobalVENUX", new object[4]
                {
                    message,
                    durationInMS,
                    color,
                    ico
                });
        }

        public static void SendPlayerNotifcation(Client player, string message, int durationInMS, string color, string title, string bgcolor)
        {
            try
            {
                player.TriggerEvent("sendPlayerNotification", new object[5]
                {
                message,
                durationInMS,
                color,
                color,
                title
                });
            }
            catch (Exception ex) { Log.Write("[EXCEPTION " + new StackTrace().GetFrame(0).GetMethod() + "] " + ex.Message); Log.Write("[EXCEPTION " + new StackTrace().GetFrame(0).GetMethod() + "] " + ex.StackTrace); }
        }
    }
}
