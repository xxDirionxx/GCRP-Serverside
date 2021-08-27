using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.AdminSystem;

namespace Venux.Other
{
    class Inventory : Script
    {

        public static bool isNearby(Client client, Vehicle veh)
        {
            return veh.Position.DistanceTo(client.Position) <= 10f;
        }

        public static Vehicle GetClosestVehicle(Client sender, float distance = 1000f)
        {
            Vehicle result = null;
            foreach (Vehicle allVehicle in NAPI.Pools.GetAllVehicles())
            {
                Vector3 entityPosition = NAPI.Entity.GetEntityPosition(allVehicle);
                float num = sender.Position.DistanceTo(entityPosition);
                if (num < distance)
                {
                    distance = num;
                    result = allVehicle;
                }
            }
            return result;
        }

        [RemoteEvent("requestPlayerKeys")]
        public void requestPlayerKeys(Client p)
        {
            try
            {
                p.TriggerEvent("openWindow", "Keys", "{\"Keys\":[{\"Name\":\"Schlüssel\",\"keys\":" + NAPI.Util.ToJson(Database.getUserVehicles(p.Name).ToArray()) + "}]}");

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [RemoteEvent("requestInventory")]
        public static void requestItems(Client p)
        {
            try
            {

                if (Database.isPlayerDeath(p.Name))
                {
                    return;
                }

                Vehicle closestVehicle = GetClosestVehicle(p, 5f);
                bool flag = false;
                if (closestVehicle != null)
                {
                    if (closestVehicle.NumberPlate != null && closestVehicle.HasSharedData("kofferraumStatus"))
                    {
                        if (closestVehicle.Locked == false && closestVehicle.GetSharedData("kofferraumStatus") == true ? true : false)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
                if (!flag)
                {
                    if (p.Position.DistanceTo(new Vector3(1712.6771, 4766.299, 13.110)) > 20.0f)
                    {
                        p.TriggerEvent("openWindow", "Inventory", "{\"inventory\":[{\"Id\":" + Database.getSQLId(p.Name) + ",\"Name\":\"Rucksack\",\"Money\":" + Database.getMoney(p.Name) + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":10000,\"MaxSlots\":15,\"Slots\":" + NAPI.Util.ToJson(Database.getUserItems(p.Name)) + "}]}");
                        return;
                    }
                    else if (p.Position.DistanceTo(new Vector3(1712.6771, 4766.299, 13.110)) < 20.0f)
                    {
                        if (p.GetSharedData("FRAKTION") == "Zivilist")
                            return;

                        p.TriggerEvent("openWindow", "Inventory", "{\"inventory\":[{\"Id\":" + Database.getSQLId(p.Name) + ",\"Name\":\"Rucksack\",\"Money\":" + Database.getMoney(p.Name) + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":10000,\"MaxSlots\":15,\"Slots\":" + NAPI.Util.ToJson(Database.getUserItems(p.Name)) + "}]}");
                    }

                }
                else
                {
                    if (closestVehicle.NumberPlate == null) { return; }
                    if (p.IsInVehicle) { return; }

                    p.SetData("LAST_INVENTORY", "Kofferraum | " + closestVehicle.NumberPlate);
                    p.TriggerEvent("openWindow", "Inventory", "{\"inventory\":[{\"Id\":" + Database.getSQLId(p.Name) + ",\"Name\":\"Rucksack\",\"Money\":" + Database.getMoney(p.Name) + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":15,\"Slots\":" + NAPI.Util.ToJson(Database.getUserItems(p.Name)) + "}, {\"Id\":" + Database.getVehicleSQLId(closestVehicle.NumberPlate) + ",\"Name\":\"Kofferraum\",\"Money\":0,\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":80000,\"MaxSlots\":18,\"Slots\":" + NAPI.Util.ToJson(Database.getKofferraumItems(closestVehicle.NumberPlate)) + "}]}");
                }
            }



            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("moveItemToInventory")]
        public void moveItemToInventory(Client p, int oldSlot, int newSlot, string ItemName, int Amount)
        {
            try
            {

                if (p.HasData("LAST_INVENTORY"))
                {
                    if (p.GetData("LAST_INVENTORY").Contains("Fraktionslager"))
                    {

                        int InventoryId = int.Parse(ItemName);

                        if (InventoryId == Database.getSQLId(p.Name))
                        {

                            if (!p.HasSharedData("FRAKTION")) { return; }

                            Items.ItemModel item = Database.getUserItems(p.Name).Find((Items.ItemModel itemModel) => itemModel.Slot == oldSlot);

                            if (item == null) { return; }

                            Database.changeInventoryItem(p.Name, item.Name, Amount, true);
                            Database.changeFraklagerItem(p.GetSharedData("FRAKTION"), item.Name, Amount, false);

                            p.TriggerEvent("closeWindow", "Inventory");
                        }
                        else
                        {
                            if (!p.HasSharedData("FRAKTION")) { return; }

                            Items.ItemModel item2 = Database.getFraklagerItems((string)p.GetSharedData("FRAKTION")).Find((Items.ItemModel itemModel) => itemModel.Slot == oldSlot);

                            if (item2 == null) { return; }

                            Database.changeInventoryItem(p.Name, item2.Name, Amount, false);
                            Database.changeFraklagerItem(p.GetSharedData("FRAKTION"), item2.Name, Amount, true);

                            p.TriggerEvent("closeWindow", "Inventory");
                        }
                    }
                    else if (p.GetData("LAST_INVENTORY").Contains("Kofferraum"))
                    {

                        int InventoryId = int.Parse(ItemName);
                        string numberPlate = (string)p.GetData("LAST_INVENTORY").Replace("Kofferraum | ", "");

                        if (InventoryId == Database.getSQLId(p.Name))
                        {

                            if (!p.HasSharedData("FRAKTION") || numberPlate == null) { return; }

                            Items.ItemModel item = Database.getUserItems(p.Name).Find((Items.ItemModel itemModel) => itemModel.Slot == oldSlot);

                            if (item == null) { return; }

                            Database.changeInventoryItem(p.Name, item.Name, Amount, true);
                            Database.changeKofferraumItem(numberPlate, item.Name, Amount, false);
                            NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                            //Discord.DiscordWebhooks.SendMessage("Spieler Kofferaum", "Spieler "+ p.Name +" hat  " + item.Name + " " + Amount + " in seinen Kofferaum gelegt.", Discord.DiscordWebhooks.adminWebhook, "Admin-Log");

                            p.TriggerEvent("closeWindow", "Inventory");
                            NAPI.Task.Run(delegate
                            {
                                NAPI.Player.StopPlayerAnimation(p);
                            }, 1800L);
                        }
                        else
                        {

                            if (!p.HasSharedData("FRAKTION") || numberPlate == null) { return; }

                            Items.ItemModel item2 = Database.getKofferraumItems(numberPlate).Find((Items.ItemModel itemModel) => itemModel.Slot == oldSlot);

                            if (item2 == null) { return; }

                            Database.changeInventoryItem(p.Name, item2.Name, Amount, false);
                            Database.changeKofferraumItem(numberPlate, item2.Name, Amount, true);
                            NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                            DiscordWebhook.SendMessage("Spieler Kofferaum", "Spieler " + p.Name + " hat  " + item2.Name + " " + Amount + " in seinen Kofferaum rausgenommen.", DiscordWebhook.adminWebhook, "Admin-Log");

                            p.TriggerEvent("closeWindow", "Inventory");
                            NAPI.Task.Run(delegate
                            {
                                NAPI.Player.StopPlayerAnimation(p);
                            }, 1800L);
                        }
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("useInventoryItem")]
        public void useInventoryItem(Client p, int Slot)
        {
            List<Items.ItemModel> itemModels = Database.getUserItems(p.Name);

            Items.ItemModel itemToUse = itemModels.Find((Items.ItemModel x) => x.Slot == Slot);

            if (itemToUse != null && itemToUse.Amount >= 1)
            {
                Items.ItemManager.useItem(p, itemToUse.Name);
            }
        }

        [RemoteEvent("dropInventoryItem")]
        public void dropInventoryItem(Client p, int Slot, int amount)
        {
            List<Items.ItemModel> itemModels = Database.getUserItems(p.Name);

            Items.ItemModel itemToUse = itemModels.Find((Items.ItemModel x) => x.Slot == Slot);

            if (itemToUse != null && itemToUse.Amount >= amount)
            {
                Database.changeInventoryItem(p.Name, itemToUse.Name, amount, true);
                Notification.SendPlayerNotifcation(p, "Du hast " + amount + (amount < 2 ? " Item" : " Items") + " weggeworfen.", 3500, "orange", "", "");
                NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                NAPI.Task.Run(delegate
                {
                    NAPI.Player.StopPlayerAnimation(p);
                }, 1800L);
            }
        }

        [RemoteEvent("fillInventorySlots")]
        public void fillInventorySlots(Client p)
        {
            p.TriggerEvent("closeWindow", "Inventory");
        }


        [RemoteEvent("packblackmoney")]
        public void CMD_Packgun(Client p)
        {
            try
            {
                if (Database.isPlayerDeath(p.Name) || (Gangwar.GebietRegister.currentRunningGangwarGebiet != null && p.Dimension == Fraktionen.FraktionRegister.GangwarDimension) || (p.HasData("IS_PACK") == true) || (Other.Paintball.StabCityPlayers.Contains(p))) { return; }

                WeaponHash weapon = p.CurrentWeapon;
                if (Items.Weapons.weapons.ContainsKey(weapon) && Database.getLoadout(p.Name).Contains(weapon))
                {
                    p.SetData("IS_PACK", true);
                    NAPI.Player.SetPlayerCurrentWeapon(p, WeaponHash.Unarmed);
                    p.RemoveWeapon(weapon);
                    p.TriggerEvent("sendProgressbar", new object[1]
                    {
                        5000
                    });
                    Functions.disableAllPlayerControls(p, true);
                    NAPI.Player.PlayPlayerAnimation(p, 33, "amb@prop_human_parking_meter@female@base", "base_female", 8f);
                    NAPI.Task.Run((() =>
                    {
                        p.TriggerEvent("componentServerEvent", new object[2]
                        {
                            "Progressbar",
                            "StopProgressbar"
                        });
                        Database.removeLoadout(p.Name, weapon);
                        Database.changeInventoryItem(p.Name, Items.Weapons.weapons[weapon], 1, false);
                        Notification.SendPlayerNotifcation(p, "Du hast die Waffe [ " + weapon + " ] gepackt.", 5000, "orange", "", "orange");
                        p.StopAnimation();
                        Functions.disableAllPlayerControls(p, false);
                    }), 5000);
                    p.ResetData("IS_PACK");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }



        [RemoteEvent("giveInventoryItem")]
        public void giveItems(Client p, int Slot, int amount)
        {

            try
            {

                List<Items.ItemModel> itemModels = Database.getUserItems(p.Name);

                Items.ItemModel itemToUse = itemModels.Find((Items.ItemModel x) => x.Slot == Slot);


                if (itemToUse != null && itemToUse.Amount >= amount)
                {


                    Client target = null;
                    float distance = 9999999.0f;

                    foreach (Client p2 in NAPI.Pools.GetAllPlayers())
                    {
                        float distance2 = p.Position.DistanceTo(p2.Position);
                        if (distance2 < distance && p2 != p && distance2 < 3)
                        {
                            target = p2;
                            distance = distance2;
                        }
                    }

                    if (target != null)
                    {
                        //Discord.DiscordWebhooks.SendMessage("Spieler gibt Item.", "Der Spieler " + p.Name + " gibt Spieler " + target.Name + " " + count + "x " + name + ".", Discord.DiscordWebhooks.itemWebhook, "Item-Log");
                        Database.changeInventoryItem(p.Name, itemToUse.Name, amount, true);
                        Database.changeInventoryItem(Start.loggedInPlayers[target], itemToUse.Name, amount, false);
                        Notification.SendPlayerNotifcation(p, "Du hast " + amount + "x " + itemToUse.Name + " an " + Start.loggedInPlayers[target] + " gegeben.", 3500, "green", "", "greem");
                        Notification.SendPlayerNotifcation(target, "Du hast " + amount + "x " + itemToUse.Name + " von " + p.Name + " bekommen.", 3500, "green", "", "green");
                        NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_snowball", "pickup_snowball");
                        NAPI.Task.Run(delegate
                        {
                            NAPI.Player.StopPlayerAnimation(p);
                        }, 1800L);
                    }
                    else
                    {
                        Notification.SendPlayerNotifcation(p, "Es wurde kein Spieler gefunden.", 3500, "red", "", "white");
                    }
                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast nicht genug Items.", 3500, "red", "", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
