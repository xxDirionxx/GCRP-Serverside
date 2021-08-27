using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux.Other
{
    class Equippoint : Script
    {
        public static List<string> alreadyEquipped = new List<string>();
        public static Dictionary<string, Vector3> equippoints = new Dictionary<string, Vector3>();

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            if (equippoints.Count < 1)
            {
                equippoints.Add("Equippunkt", new Vector3(186.21584, -914.0723, 23.040178));

                foreach (KeyValuePair<string, Vector3> equippoint in equippoints)
                {
                    ColShape val = NAPI.ColShape.CreateCylinderColShape(equippoint.Value, 7.0f, 1.4f, 0);
                    val.SetData("COLSHAPE_FUNCTION", new FunctionModel("useEquippoint"));
                    val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um dein Equip abzuholen.", "", "orange", 5000));

                }
            }
        }


        [RemoteEvent("useEquippoint")]
        public static void equipPlayer(Client p)
        {
            try
            {
                if (alreadyEquipped.Contains(p.Name))
                {
                    Notification.SendPlayerNotifcation(p, "Du kannst dein Equip bereits abgehohlt!", 5000, "red", "FEHLER", "red");
                    return;
                }

                alreadyEquipped.Add(p.Name);
                p.TriggerEvent("sendProgressbar", new object[1]
                {
                    5000
                });
                Functions.disableAllPlayerControls(p, true);
                NAPI.Player.PlayPlayerAnimation(p, 33, "amb@medic@standing@tendtodead@idle_a", "idle_a", 8f);
                NAPI.Task.Run((() =>
                {
                    switch (new Random().Next(0, 5))
                    {
                        case 0:
                            Database.changeInventoryItem(p.Name, "Assaultrifle", 1, false);
                            Database.changeInventoryItem(p.Name, "Schutzweste", 10, false);
                            break;
                        case 1:
                            Database.changeInventoryItem(p.Name, "Compactrifle", 1, false);
                            Database.changeInventoryItem(p.Name, "Heavypistol", 1, false);
                            break;
                        case 2:
                            Database.changeInventoryItem(p.Name, "Heavypistol", 1, false);
                            Database.changeInventoryItem(p.Name, "Verbandskasten", 6, false);
                            break;
                        case 3:
                            Database.changeInventoryItem(p.Name, "Heavypistol", 1, false);
                            Database.changeInventoryItem(p.Name, "Schutzweste", 5, false);
                            break;
                        case 4:
                            Database.changeInventoryItem(p.Name, "Heavypistol", 1, false);
                            Database.changeInventoryItem(p.Name, "Advancedrifle", 1, false);
                            break;
                        case 5:
                            Database.changeInventoryItem(p.Name, "Heavypistol", 1, false);
                            Database.changeInventoryItem(p.Name, "Gusenberg", 1, false);
                            break;
                    }
                    p.TriggerEvent("componentServerEvent", new object[2]
                    {
                        "Progressbar",
                        "StopProgressbar"
                    });
                    Notification.SendPlayerNotifcation(p, "Du hast dein Equip für die jetzige Wende abgeholt.", 5000, "green", "", "white");
                    p.StopAnimation();
                    Functions.disableAllPlayerControls(p, false);
                }), 5000);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
