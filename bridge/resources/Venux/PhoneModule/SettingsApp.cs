using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Handy
{
    class SettingsApp : Script
    {
        public static bool flugmodus = false;
        public static bool lautlos1 = false;
        public static bool anrufAblehnen1 = false;
        public static List<Wallpaper> Wallpapers = new List<Wallpaper>
        {
            new Wallpaper(1, "Standard", "../img/park.3c2ba4261.jpg"),
            new Wallpaper(2, "MG13", "../img/Marabunta.687aa756.png"),
            new Wallpaper(3, "LostMC", "../img/LOST.ca303ab9.png"),
            new Wallpaper(4, "LCN", "../img/LCN.db28d56e.png"),
            new Wallpaper(5, "Grove", "../img/Grove.89a3f207.png"),
            new Wallpaper(6, "Midnight", "../img/Midnight.6174a48d.png"),
            new Wallpaper(7, "Triaden", "../img/Triaden.47b0e58c.png"),
            new Wallpaper(8, "Vagos", "../img/Vagos.6aac918d.png"),
            new Wallpaper(9, "YakuZa", "../img/YakuZa.86f2ebd3.png"),
            new Wallpaper(10, "GIF", "../img/girl2.ce253a58.gif"),
            new Wallpaper(11, "GIF-2", "../img/park.3c2ba426.gif"),
            new Wallpaper(12, "GIF-3", "../img/bubblebutt.684890cc.gif"),
            new Wallpaper(13, "GIF-4", "../img/venux.gif"),
        };


        [RemoteEvent("requestPhoneWallpaper")]
        public void requestPhoneWallpaper(Client p)
        {
            p.TriggerEvent("componentServerEvent", "HomeApp", "responsePhoneWallpaper", Database.getWallpaper(p.Name));
        }

        [RemoteEvent("requestWallpaperList")]
        public void SettingsEditWallpaperApp(Client p)
        {
            p.TriggerEvent("componentServerEvent", "SettingsEditWallpaperApp", "responseWallpaperList", NAPI.Util.ToJson(Wallpapers));
        }

        [RemoteEvent("saveWallpaper")]
        public void saveWallpaper(Client p, string file)
        {
            Database.updateWallpaper(p.Name, file);
            NAPI.Player.StopPlayerAnimation(p);
            p.TriggerEvent("removeSmartphone");
        }

        public class Wallpaper
        {
            public int id { get; set; }
            public string name { get; set; }
            public string file { get; set; }

            public Wallpaper(int i, string n, string f)
            {
                id = i;
                name = n;
                file = f;
            }
        }

        [RemoteEvent("savePhoneSettings")]
        public void savePhoneSettings(Client p, bool flugmodus, string lautlos, string anrufAblehnen)
        {
            try
            {
                Notification.SendPlayerNotifcation(p, "Einstellungen gespeichert!", 4500, "green", "", "");

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
