using GTANetworkAPI;
using System;

namespace Venux.Handy
{
    class TelefonApp : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            //Log.Write("TelefonApp geladen");
        }

        [RemoteEvent("callUserPhone")]
        public void callUserPhone(Client p, int phonenumber)
        {
            try
            {
                p.TriggerEvent("componentServerEvent", new object[2] {
                    "CallManageApp",
                    "acceptCall"
                });
                Notification.SendPlayerNotifcation(p, "Derzeit ist das Telefonieren unmöglich!", 4500, "red", "", "");

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
    }
}
