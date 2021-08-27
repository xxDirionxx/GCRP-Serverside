using GTANetworkAPI;
using System;

namespace Venux.Handy
{
    class FraktionAPP : Script
    {

        [RemoteEvent("leaveFrak")]
        public void LeaveFrak(Client p)
        {
            try
            {
                Database.setUserFraktion(p.Name, "Zivilist");
                Database.setUserFraktionRank(p.Name, 0);
                Notification.SendPlayerNotifcation(p, "Du hast die Fraktion verlassen.", 5000, "red", "", "white");
                p.TriggerEvent("removeSmartphone");
                NAPI.Player.StopPlayerAnimation(p);

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    if (target.HasSharedData("FRAKTION"))
                    {
                        if (target.GetSharedData("FRAKTION") == p.GetSharedData("FRAKTION"))
                        {
                            Notification.SendPlayerNotifcation(target, "Der Spieler " + p.Name + " hat die Fraktion verlassen.", 5000, "red", p.GetSharedData("FRAKTION"), "red");
                        }
                    }
                }

                p.SetSharedData("FRAKTION", "Zivilist");
                p.SetSharedData("FRAKTION_RANK", 0);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

    }
}
