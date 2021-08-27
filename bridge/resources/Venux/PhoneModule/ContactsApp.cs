using GTANetworkAPI;
using System;

namespace Venux.Handy
{
    class ContactsApp : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            //Log.Write("Kontakte geladen");
        }

        [RemoteEvent("addPhoneContact")]
        public void addPhoneContact(Client p, string name, int number, string display)
        {
            try
            {
                Database.createContact(name, number, display);
                Notification.SendPlayerNotifcation(p, "Du hast einen Kontakt eingespeichert", 4500, "grey", "HANDY", "");

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
