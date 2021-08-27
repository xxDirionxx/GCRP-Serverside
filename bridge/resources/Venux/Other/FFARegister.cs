using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Other
{
    class Paintball : Script
    {
        public static List<Vector3> StabCityPoints = new List<Vector3> {
            new Vector3(113.4925, 3731.04, 38.64072),
            new Vector3(50.17555, 3634.005, 38.58967),
            new Vector3(4.314305, 3698.297, 38.44342),
            new Vector3(81.36716, 3719.341, 38.6500911)
        };

        public static List<Client> StabCityPlayers = new List<Client>();
        public static Dictionary<string, Vector3> paintballs = new Dictionary<string, Vector3>();

        public static string PAINTBALL_KILLS = "PAINTBALL_KILLS";
        public static string PAINTBALL_DEATHS = "PAINTBALL_DEATHS";

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            if (paintballs.Count < 1)
            {
                paintballs.Add("StabCity", new Vector3(570.46, 2796.11168, 41.01111));

                foreach (KeyValuePair<string, Vector3> paintball in paintballs)
                {
                    NAPI.Blip.CreateBlip(432, paintball.Value, 1.0f, 0, "Paintball", 255, 0, true, 0, 0);
                    NAPI.Marker.CreateMarker(1, paintball.Value, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 1.0f, new Color(255, 140, 0), false, 0);

                    ColShape val = NAPI.ColShape.CreateCylinderColShape(paintball.Value, 1.4f, 1.4f, 0);
                    val.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterPaintball", paintball.Key));
                    val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um den FFA zu betreten.", "PAINTBALL", "orange", 5000));

                }
            }
        }

        [RemoteEvent("enterPaintball")]
        public static void enterPaintball(Client p, string name)
        {
            if (name == null)
                return;

            try
            {
                p.TriggerEvent("openWindow", new object[2]
                    {
                                            "Confirmation",
                                            "{\"confirmationObject\":{\"Title\":\"FFA StabCity\",\"Message\":\"Möchtest du dem FFA beitreten? ( " + Other.Paintball.StabCityPlayers.Count + " / 50 )\",\"Callback\":\"Paintball\",\"Arg1\":\"\",\"Arg2\":\"\"}}"
                    });
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        public static Vector3 getRandomSpawnpoint(List<Vector3> list)
        {
            var random = new Random();
            int index = random.Next(list.Count);
            return list[index];
        }

        [RemoteEvent("Paintball")]
        public void joinPaintball(Client p, string value)
        {

            try
            {
                {
                    p.TriggerEvent("initializePaintball");
                    StabCityPlayers.Add(p);
                    Notification.SendPlayerNotifcation(p, "Du bist dem FFA - Stab City beigetreten.", 5000, "white", "INFORMATION", "white");
                    Anticheat.Wait(p); p.Position = new Vector3(104.44258, 3711.567, 39.03214);
                    p.Dimension = 3;
                    Anticheat.Wait(p); p.Armor = 100;

                    if (p.HasData(PAINTBALL_DEATHS))
                        p.ResetData(PAINTBALL_DEATHS);

                    if (p.HasData(PAINTBALL_KILLS))
                        p.ResetData(PAINTBALL_KILLS);

                    p.TriggerEvent("updatePaintballScore", 0, 0, 0);

                    p.SetData(PAINTBALL_DEATHS, 0);
                    p.SetData(PAINTBALL_KILLS, 0);
                    p.RemoveAllWeapons();



                    NAPI.Task.Run(() =>
                    {
                        p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_ADVANCEDRIFLE"), 9999);
                        p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_GUSENBERG"), 9999);
                        p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_HEAVYPISTOL"), 9999);
                        p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_ASSAULTRIFLE"), 9999);
                        p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_BULLPUPRIFLE"), 9999);
                    }, delayTime: 100);

                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
