using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Venux.Game.Handy.Models;

namespace Venux.Fraktionen
{
    public class FraktionTablet : Script
    {
        public static List<Service.openService> openService = new List<Service.openService>();
        public static List<Service.Service> Service = new List<Service.Service>();

        string FraktionApp = "{\"appName\":\"VehicleClawUebersichtApp\", \"id\":\"VehicleClawUebersichtApp\",\"name\":\"\",\"icon\": \"VehicleTracker.svg\"},";

        string ServiceApp = "{\"appName\":\"ServiceOverviewApp\", \"id\":\"ServiceOverviewApp\",\"name\":\"\",\"icon\": \"MessengerApp.svg\"}";

        //string KennzeichenApp = "{\"appName\":\"KennzeichenUebersichtApp\", \"id\":\"KennzeichenUebersichtApp\",\"name\":\"Kennzeichen\",\"icon\": \"VehicleTrackerApp.svg\"}";

        [RemoteEvent("computerCheck")]
        public void OpenComputer(Client venux)
        {
            if (Database.isPlayerDeath(venux.Name))
            {
                return;
            }
            try
            {
                {
                    venux.TriggerEvent("openComputer");
                    venux.TriggerEvent("componentServerEvent", new object[3]
                    {
                        "DesktopApp",
                        "responseComputerApps",
                        "[" + ServiceApp + "]"
                    });
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        // -- ServiceApp -- //

        [RemoteEvent("requestOpenServices")]
        public void onrequestOpenServices(Client venux)
        {
            venux.TriggerEvent("componentServerEvent", "ServiceListApp", "responseOpenServiceList", NAPI.Util.ToJson(openService));
        }

        [RemoteEvent("RequestTeamServiceList")]
        public void onRequestAcceptedTickets(Client client)
        {
            client.TriggerEvent("componentServerEvent", "ServiceAcceptedApp", "responseTeamServiceList", NAPI.Util.ToJson(Service.FindAll(x => x.name == client.Name)));
        }

        [RemoteEvent("setGpsCoordinatesAccepted")]
        public void setGpsCoordinatesAccepted(Client venux)
        {
            NAPI.Blip.CreateBlip(433, venux.Position, 1f, 1, "Service", 255, 0, true, 0, uint.MaxValue);
        }

        [RemoteEvent("acceptOpenService")]
        public void onAcceptOpenService(Client client, int id)
        {
            if (openService.Find(x => x.id == id) != null)
            {
                Service.Add(new Service.Service(openService.Find(x => x.id == id).name, openService.Find(x => x.id == id).message, openService.Find(x => x.id == id).id, client.Name));
                Notification.SendPlayerNotifcation(NAPI.Pools.GetAllPlayers().Find(x => x.Name == openService.Find(x => x.id == id).name), "Dein Service wurde angenommen", 5000, "yellow", "SERVICE", "yellow");
                openService.Remove(openService.Find(x => x.id == id));
            }
        }

        public static void addService(Client p, string des)
        {
            Random r = new Random();
            openService.Add(new Service.openService(p.Name, des, r.Next(1000, 9999)));
        }

        [RemoteEvent("finishOwnAcceptedService")]
        public void onfinishOwnAcceptedService(Client client, int id)
        {
            if (openService.Find(x => x.id == id) != null)
            {
                //Service.Add(new Service.Service(openService.Find(x => x.id == id).name, openService.Find(x => x.id == id).message, openService.Find(x => x.id == id).id, client.Name));
                openService.Remove(openService.Find(x => x.id == id));
            }
        }

        // -- FraktionListAPP -- //

        [RemoteEvent("requestFraktionMembers")]
        public static void requestFraktionMembersT(Client venux)
        {
            List<FraktionMemberListModel> FraktionMemberList = new List<FraktionMemberListModel>();

            int ManagePermission2 = 0;

            if (venux.GetSharedData("FRAKTION_RANK") > 9 && venux.GetSharedData("FRAKTION_RANK") != 12)
            {
                ManagePermission2 = 2;
            }
            else if (venux.GetSharedData("FRAKTION_RANK") == 12)
            {
                ManagePermission2 = 2;
            }

            foreach (Fraktionen.FraktionPlayer fraktionPlayer in Database.getFraktionOnlinePlayers(venux.GetSharedData("FRAKTION")))
            {

                FraktionMemberList.Add(
                    new FraktionMemberListModel()
                    {
                        member = fraktionPlayer.playerName,
                        rang = fraktionPlayer.fraktionRank,
                        payday = fraktionPlayer.fraktionRank,

                    });
            }

            object JSONobject = new
            {
                rang = FraktionMemberList.OrderByDescending(o => o.rang),
                description = FraktionMemberList.OrderByDescending(o => o.rang),
                name = FraktionMemberList.OrderByDescending(o => o.rang),
                payday = FraktionMemberList.OrderByDescending(o => o.rang),
                ManagePermission = ManagePermission2
            };

            venux.TriggerEvent("componentServerEvent", new object[3]
                    {
                        "FraktionListApp",
                        "responseMembers",
                        NAPI.Util.ToJson(JSONobject)
                    });
        }

        [RemoteEvent("closeComputer")]
        public void CloseComputer(Client venux)
        {
            try
            {
                {
                    venux.TriggerEvent("closeComputer");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}