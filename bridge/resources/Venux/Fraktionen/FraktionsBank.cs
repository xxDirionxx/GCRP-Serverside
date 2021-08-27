using GTANetworkAPI;
using System;

namespace Venux.Fraktionen
{
    class FraktionsBank : Script
    {

        public static void openFrakBank(Client p)
        {
            try
            {
                if (p.GetSharedData("FRAKTION") != "Zivilist")
                {
                    p.TriggerEvent("openWindow", new object[] { "Bank", "{\"playername\": \"" + p.GetSharedData("FRAKTION") + "\", \"money\": " + Database.getMoney(p.Name) + ",\"balance\": " + Database.getFrakBank(p.GetSharedData("FRAKTION")) + "}" });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("bankDeposit")]
        public void FrakbankEinzahlen(Client p, int value)
        {
            if (value == null) { return; }

            try
            {
                if (Database.getMoney(p.Name) >= value)
                {
                    Database.changeMoney(p.Name, value, true);
                    Database.changeFraktionMoney(p.GetSharedData("FRAKTION"), value, false);
                    Notification.SendPlayerNotifcation(p, "Du hast " + value + "$ auf die Fraktionsbank eingezahlt.", 5000, "white", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast nicht genügend Geld.", 5000, "white", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("bankPayout")]
        public void FrakbankAbheben(Client p, int value)
        {
            if (value != null)
                try
                {
                    if (Database.getFrakBank(p.GetSharedData("FRAKTION")) >= value)
                    {
                        if (p.GetSharedData("FRAKTION_RANK") < 12)
                        {
                            Notification.SendPlayerNotifcation(p, "Du bist dazu nicht berechtigt.", 5000, "white", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                            return;
                        }

                        Database.changeMoney(p.Name, value, false);
                        Database.changeFraktionMoney(p.GetSharedData("FRAKTION"), value, true);
                        Notification.SendPlayerNotifcation(p, "Du hast " + value + "$ von der Fraktionsbank abgehoben.", 5000, "white", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                    }
                    else
                    {
                        Notification.SendPlayerNotifcation(p, "Auf der Fraktionsbank ist nicht genügend Geld.", 5000, "white", p.GetSharedData("FRAKTION"), "rgb(" + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Red + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Green + ", " + Database.getFraktionByName(p.GetSharedData("FRAKTION")).rgbColor.Blue + ")");
                    }
                }
                catch (Exception ex) { Log.Write(ex.Message); }
        }

    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
