using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.Other;

namespace Venux.Items
{
    class ItemManager : Script
    {

        public static List<Item> itemRegisterList = new List<Item>();
        public static Dictionary<string, string> weaponImages = new Dictionary<string, string>();

        public ItemManager()
        {
            itemRegisterList.Add(new Schutzweste());
            itemRegisterList.Add(new Verbandskasten());
            itemRegisterList.Add(new Waffenkiste());
            itemRegisterList.Add(new Advancedrifle());
            itemRegisterList.Add(new Assaultrifle());
            itemRegisterList.Add(new Heavypistol());
            itemRegisterList.Add(new Compactrifle());
            itemRegisterList.Add(new Methkisten());
            itemRegisterList.Add(new Revolver());
            itemRegisterList.Add(new Meth());
            itemRegisterList.Add(new Marksmankiste());
            itemRegisterList.Add(new Waffenteil());
            itemRegisterList.Add(new Seil());
            itemRegisterList.Add(new Repairkit());
            itemRegisterList.Add(new Schweißgeraet());
            itemRegisterList.Add(new Koks());
            itemRegisterList.Add(new Koksblätter());

            weaponImages.Add("Advancedrifle", "AdvanvcedRifle.png");
            weaponImages.Add("Assaultrifle", "Assaultrifle.png");
            weaponImages.Add("Gusenberg", "Gusenberg.png");
            weaponImages.Add("Revolver", "Revolver.png");
            weaponImages.Add("Heavypistol", "HeavyPistol.png");
            weaponImages.Add("Bullpuprifle", "BullpupRifle.png");
            weaponImages.Add("Marksmanrifle", "MarksmanRifle.png");
            weaponImages.Add("Militaryrifle", "Militaryrifle.png");
        }

        public static void useItem(Client p, string item)
        {
            if (item == null)
                return;

            try
            {
                if (!Start.loggedInPlayers.ContainsKey(p) || Start.deathTime.ContainsKey(p) || (Gangwar.GebietRegister.currentRunningGangwarGebiet != null && p.Dimension == Fraktionen.FraktionRegister.GangwarDimension) || (p.IsInVehicle) || (Other.Paintball.StabCityPlayers.Contains(p)))
                    return;

                if (Items.Weapons.weapons.ContainsValue(item))
                {
                    p.TriggerEvent("sendProgressbar", new object[1]
                     {
                        5000
                     });
                    Database.changeInventoryItem(p.Name, item, 1, true);
                    Functions.disableAllPlayerControls(p, true);
                    NAPI.Player.PlayPlayerAnimation(p, 33, "amb@prop_human_parking_meter@female@base", "base_female", 8f);
                    NAPI.Task.Run((() =>
                    {
                        p.TriggerEvent("componentServerEvent", new object[2]
                        {
                            "Progressbar",
                            "StopProgressbar"
                        });
                        p.GiveWeapon(Utils.KeyByValue(Items.Weapons.weapons, item), 9999);
                        Database.addLoadout(p.Name, Utils.KeyByValue(Items.Weapons.weapons, item));
                        p.StopAnimation();
                        Functions.disableAllPlayerControls(p, false);
                    }), 5000);
                }
                else
                {
                    Item item2 = itemRegisterList.Find((Item x) => x.Name == item);
                    if (Database.getItemCount(p.Name, item) >= 1 && item2.getItemFunction(p))
                    {
                        Database.changeInventoryItem(p.Name, item, 1, true);
                    }
                }

            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
