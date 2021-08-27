using GTANetworkAPI;
using System.Collections.Generic;
using Venux.Vehicles;

namespace Venux.XMenu
{
    class VehicleMenu : Script
    {
        public static List<Eject> EjectPlayer = new List<Eject>
        {
            new Eject("4"),
        };

        [RemoteEvent("REQUEST_VEHICLE_REPAIR")]
        public static void REQUEST_VEHICLE_REPAIR(Client c, Vehicle veh = null)
        {
            if (Database.isPlayerDeath(c.Name)) { return; }
            if (c.HasData("IS_REPAIR") == true) { return; }

            if (Database.getItemCount(c.Name, "Repairkit") >= 1)
            {
                Database.changeInventoryItem(c.Name, "Repairkit", 1, true);
                NAPI.Player.PlayPlayerAnimation(c, 33, "mini@repair", "fixing_a_player", 8f);
                Functions.disableAllPlayerControls(c, true);
                c.SetData("IS_REPAIR", true);
                c.TriggerEvent("sendProgressbar", new object[1]
    {
                        6000
    });
                NAPI.Task.Run((() =>
                {
                    c.TriggerEvent("componentServerEvent", new object[2]
                    {
                    "Progressbar",
                    "StopProgressbar"
                    });
                    veh.Repair();
                    c.StopAnimation();
                    Functions.disableAllPlayerControls(c, false);
                    c.ResetData("IS_REPAIR");
                }), 6000);
            }
        }

        [RemoteEvent("REQUEST_VEHICLE_FILL_FUEL")]
        public static void REQUEST_VEHICLE_FILL_FUEL(Client p)
        {
            Notification.SendPlayerNotifcation(p, "Sie muessen an einer Tankstelle sein!", 5000, "gray", "", "");
        }

        [RemoteEvent("REQUEST_VEHICLE_INFORMATION")]
        public static void REQUEST_VEHICLE_INFORMATION(Client c, Vehicle veh = null)
        {
            Notification.SendPlayerNotifcation(c, "Fahrzeug ID: " + veh.GetData("VEHICLE_SQL_ID"), 3500, "blue", "KFZ", "blue");
        }

        [RemoteEvent("REQUEST_VEHICLE_TOGGLE_LOCK_OUTSIDE")]
        public static void REQUEST_VEHICLE_TOGGLE_LOCK_OUTSIDE(Client c, Vehicle veh = null)
        {
            if (!(veh == null))
            {
                if (veh.NumberPlate != null && Database.isVehicleOwnedByPlayer(c.Name, veh.NumberPlate))
                {
                    veh.Locked = !veh.Locked;
                    veh.SetSharedData(Vehicles.VehicleData.VEHICLE_LOCKED_STATUS, veh.Locked);
                    string text = veh.Locked ? "zugeschlossen" : "aufgeschlossen";
                    Notification.SendPlayerNotifcation(c, "Fahrzeug " + text, 3500, (text == "zugeschlossen") ? "red" : "green", "", "");
                }
            }
        }

        [RemoteEvent("REQUEST_VEHICLE_TOGGLE_LOCK")]
        public static void REQUEST_VEHICLE_TOGGLE_LOCK(Client c)
        {
            Vehicle vehicle = c.Vehicle;
            if (!(vehicle == null))
            {
                if (vehicle.NumberPlate != null && Database.isVehicleOwnedByPlayer(c.Name, vehicle.NumberPlate))
                {
                    vehicle.Locked = !vehicle.Locked;
                    vehicle.SetSharedData(Vehicles.VehicleData.VEHICLE_LOCKED_STATUS, vehicle.Locked);
                    string text = vehicle.Locked ? "zugeschlossen" : "aufgeschlossen";
                    Notification.SendPlayerNotifcation(c, "Fahrzeug " + text, 3500, (text == "zugeschlossen") ? "red" : "green", "", "");
                }
            }
        }

        [RemoteEvent("REQUEST_VEHICLE_TOGGLE_DOOR")]
        public static void REQUEST_VEHICLE_TOGGLE_DOOR(Client c)
        {


            Vehicle vehicle = c.Vehicle;

            if (vehicle.GetSharedData("kofferraumStatus") == null)
                return;

            if (!(vehicle == null) && !vehicle.Locked)
            {
                vehicle.SetSharedData("kofferraumStatus", !vehicle.GetSharedData("kofferraumStatus"));
                string text = (vehicle.GetSharedData("kofferraumStatus") ? "aufgeschlossen" : "zugeschlossen");
                Notification.SendPlayerNotifcation(c, "Kofferraum " + text, 3500, (text == "zugeschlossen") ? "red" : "green", "", "");
            }
        }

        [RemoteEvent("REQUEST_VEHICLE_EJECT")]
        public static void REQUEST_VEHICLE_EJECT(Client venux)
        {

            Vehicle vehicle = venux.Vehicle;

            if (!(vehicle == null) && !vehicle.Locked) { venux.TriggerEvent("openWindow", new object[] { "EjectWindow", "{\"ejectAction\": \"cancel\"}" }); }
            //Notification.SendPlayerNotifcation(c, "test", 3500, "blue", "KFZ", "blue");
        }

        [RemoteEvent("REQUEST_VEHICLE_TOGGLE_DOOR_OUTSIDE")]
        public static void REQUEST_VEHICLE_TOGGLE_DOOR_OUTSIDE(Client c, Vehicle veh = null)
        {
            if (!(veh == null) && !veh.Locked)
            {
                if (veh.GetSharedData("kofferraumStatus") == null)
                    return;

                veh.SetSharedData("kofferraumStatus", !veh.GetSharedData("kofferraumStatus"));
                string text = veh.GetSharedData("kofferraumStatus") ? "aufgeschlossen" : "zugeschlossen";
                Notification.SendPlayerNotifcation(c, "Kofferraum " + text, 3500, (text == "zugeschlossen") ? "red" : "green", "", "");
            }
        }

        [RemoteEvent("REQUEST_VEHICLE_TOGGLE_ENGINE")]
        public static void REQUEST_VEHICLE_TOGGLE_ENGINE(Client c)
        {
            Vehicle vehicle = c.Vehicle;
            if (!(vehicle == null))
            {
                if (vehicle.NumberPlate != null)
                {
                    vehicle.EngineStatus = !vehicle.EngineStatus;
                    vehicle.SetSharedData(Vehicles.VehicleData.VEHICLE_ENGINE_STATUS, vehicle.EngineStatus);
                    string text = vehicle.EngineStatus ? "eingeschaltet." : "ausgeschaltet.";
                    Notification.SendPlayerNotifcation(c, "Motor " + text, 3500, (text == "ausgeschaltet.") ? "red" : "green", "", "");
                }
            }
        }

        public class Eject
        {
            public string Seats { get; set; }
            public Eject(string eject)
            {
                Seats = eject;
            }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
