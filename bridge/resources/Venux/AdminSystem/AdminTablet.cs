using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Other
{
    class AdminTablet : Script
    {
        public static List<Tickets.openTicket> openTickets = new List<Tickets.openTicket>();
        public static List<Tickets.Ticket> tickets = new List<Tickets.Ticket>();

        string SupportApp = "{\"appName\":\"SupportOverviewApp\", \"id\":\"SupportOverviewApp\",\"name\":\"Support\",\"icon\": \"support_tickets.4cac8738.svg\"},";

        string VehicleApp = "{\"appName\":\"SupportVehicleApp\", \"id\":\"SupportVehicleApp\",\"name\":\"Fahrzeuguebersicht\",\"icon\": \"vehicle_support.4a257081.svg\"}";


        [RemoteEvent("VENUXADMIN")]
        public void openVENUX(Client venux)
        {
            try
            {
                if (Database.isPlayerTeam(venux.Name))
                {
                    venux.TriggerEvent("openComputer");
                    venux.TriggerEvent("componentServerEvent", new object[3]
                    {
                        "DesktopApp",
                        "responseComputerApps",
                        "[" + SupportApp + " " + VehicleApp + "]"
                    });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("requestOpenSupportTickets")]
        public void onrequestOpenServices(Client client)
        {
            client.TriggerEvent("componentServerEvent", "SupportOpenTickets", "responseOpenTicketList", NAPI.Util.ToJson(openTickets));
        }

        [RemoteEvent("acceptOpenSupportTicket")]
        public void onAcceptOpenSupportTicket(Client client, string author)
        {
            if (openTickets.Find(x => x.author == author) != null)
            {
                tickets.Add(new Tickets.Ticket(openTickets.Find(x => x.author == author).author, openTickets.Find(x => x.author == author).description, openTickets.Find(x => x.author == author).id, client.Name));
                Notification.SendPlayerNotifcation(NAPI.Pools.GetAllPlayers().Find(x => x.Name == openTickets.Find(x => x.author == author).author), "Dein Ticket wurde angenommen", 5000, "yellow", "TICKETSYSTEM", "yellow");
                openTickets.Remove(openTickets.Find(x => x.author == author));
            }
        }

        [RemoteEvent("requestAcceptedTickets")]
        public void onRequestAcceptedTickets(Client client)
        {
            client.TriggerEvent("componentServerEvent", "SupportAcceptedTickets", "responseAcceptedTicketList", NAPI.Util.ToJson(tickets.FindAll(x => x.bearbeiter == client.Name)));
        }

        [RemoteEvent("closeTicket")]
        public void onCloseTicket(Client client, string author)
        {

            if (tickets.Find(x => x.author == author) != null)
            {
                Notification.SendPlayerNotifcation(NAPI.Pools.GetAllPlayers().Find(x => x.Name == tickets.Find(x => x.author == author).author), "Dein Ticket wurde geschlossen", 5000, "red", "TICKETSYSTEM", "yellow");
                tickets.Remove(tickets.Find(x => x.author == author));
                foreach (Client target in NAPI.Pools.GetAllPlayers())
                    target.ResetData("IS_SUPPORT");
            }
        }

        [RemoteEvent("supportTeleportToPlayer")]
        public void onSupportTeleportToPlayer(Client client, string author)
        {
            if (tickets.Find(x => x.author == author) != null)
            {
                client.Position = NAPI.Pools.GetAllPlayers().Find(x => x.Name == tickets.Find(x => x.author == author).author).Position;
                Notification.SendPlayerNotifcation(client, "Du hast dich zu " + author + " teleportiert", 5000, "red", "TICKETSYSTEM", "yellow");
            }
        }

        [RemoteEvent("supportRevivePlayer")]
        public void onSupportRevivePlayer(Client client, string author)
        {
            if (tickets.Find(x => x.author == author) != null)
            {
                Notification.SendPlayerNotifcation(client, "Du kannst nur über Command reviven.", 5000, "red", "FEHLER!", "red");
            }
        }

        [RemoteEvent("supportBringPlayer")]
        public void onSupportBringPlayer(Client client, string author)
        {
            if (tickets.Find(x => x.author == author) != null)
            {
                AdminSystem.AdminCommand.CMD_BRING(client, author);
            }
        }

        [RemoteEvent("VENUXCLOSE")]
        public void CloseTablet(Client p)
        {
            try
            {
                if (Database.isPlayerTeam(p.Name))
                {
                    p.TriggerEvent("closeComputer");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static void addTicket(Client p, string des)
        {
            Random r = new Random();
            openTickets.Add(new Tickets.openTicket(p.Name, des, r.Next(1000, 9999)));
        }

        [RemoteEvent("requestVehicleData")]
        public void onRequestVehicleData(Client client, int id)
        {
            client.TriggerEvent("componentServerEvent", "SupportVehicleProfile", "responseVehicleData", NAPI.Util.ToJson(new List<Tickets.SupVehicleData> { Database.getVehicleData(id) }));
        }

        [RemoteEvent("requestSupportVehicleList")]
        public void onRequestVehicleData(Client client, string id)
        {
            client.TriggerEvent("componentServerEvent", "SupportVehicleList", "responseVehicleList", NAPI.Util.ToJson(Database.getVehicleData2(id)));
        }

        [RemoteEvent("SupportSetGarage")]
        public void onSupportSetGarage(Client client, int id)
        {
            if (NAPI.Pools.GetAllVehicles().Find(x => x.HasData("VEHICLE_SQL_ID") && x.GetData("VEHICLE_SQL_ID") == id) != null)
            {
                Database.changeVehicleState(NAPI.Pools.GetAllVehicles().Find(x => x.HasData("VEHICLE_SQL_ID") && x.GetData("VEHICLE_SQL_ID") == id).NumberPlate, 1);
                NAPI.Pools.GetAllVehicles().Find(x => x.HasData("VEHICLE_SQL_ID") && x.GetData("VEHICLE_SQL_ID") == id).Delete();
                Notification.SendPlayerNotifcation(client, "Du hast das Fahrzeug mit der ID " + id + " eingeparkt!", 5000, "red", "TICKETSYSTEM", "yellow");
            }
            else
            {
                Notification.SendPlayerNotifcation(client, "Das Fahrzeug ist bereits eingeparkt!", 5000, "red", "TICKETSYSTEM", "yellow");
            }
        }

        [RemoteEvent("SupportGoToVehicle")]
        public void onSupportGoToVehicle(Client client, int id)
        {
            if (NAPI.Pools.GetAllVehicles().Find(x => x.HasData("VEHICLE_SQL_ID") && x.GetData("VEHICLE_SQL_ID") == id) != null)
            {
                client.Position = NAPI.Pools.GetAllVehicles().Find(x => x.HasData("VEHICLE_SQL_ID") && x.GetData("VEHICLE_SQL_ID") == id).Position;
                client.SetIntoVehicle(client, -1);
                Notification.SendPlayerNotifcation(client, "Du hast dich zum Fahrzeug mit ID " + id + " Teleportiert!", 5000, "red", "TICKETSYSTEM", "yellow");
            }
            else
            {
                Notification.SendPlayerNotifcation(client, "Das Fahrzeug ist nicht ausgeparkt!", 5000, "red", "TICKETSYSTEM", "yellow");
            }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
