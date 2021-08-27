using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/*namespace Venux
{
    class ItemModule : Venux.Module.Module<ItemModule>
    {
        public static List<Item> itemRegisterList = new List<Item>();

        protected override bool OnLoad()
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM items";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Item item = new Item
                            {
                                Id = reader.GetInt32("Id"),
                                ImagePath = reader.GetString("Image"),
                                MaxStackSize = reader.GetInt32("Stack"),
                                Name = reader.GetString("Name"),
                                WeightInG = reader.GetInt32("Weight"),
                                Whash = (WeaponHash)NAPI.Util.GetHashKey(reader.GetString("Whash"))
                            };

                            itemRegisterList.Add(item);
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadItems] " + ex.Message);
                Logger.Print("[EXCEPTION loadItems] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }

            return true;
        }

        [RemoteEvent("Pressed_KOMMA")]
        public static void komma(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                if (dbPlayer.IsFarming)
                {
                    return;
                }

                MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                try
                {
                    MySqlDataReader reader = mySqlReaderCon.Reader;
                    try
                    {
                        if (!reader.HasRows)
                        {
                            return;
                        }

                        reader.Read();
                        List<ItemModel> list = new List<ItemModel>();
                        string @string = reader.GetString("Items");
                        list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                        ItemModel itemToUse = list.Find((ItemModel x) => x.Name == "Verbandskasten");
                        if (itemToUse == null)
                        {
                            return;
                        }

                        int index = list.IndexOf(itemToUse);
                        if (itemToUse.Amount == 1)
                        {
                            list.Remove(itemToUse);
                        }
                        else
                        {
                            itemToUse.Amount--;
                            list[index] = itemToUse;
                        }

                        Item item = itemRegisterList.Find((Item x) => x.Name == itemToUse.Name);
                        reader.Close();
                        if (reader.IsClosed)
                        {
                            mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                            mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                            MySqlHandler.ExecuteSync(mySqlQuery);
                            dbPlayer.disableAllPlayerActions(true);
                            dbPlayer.SendProgressbar(4000);
                            dbPlayer.IsFarming = true;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.PlayAnimation(33, "amb@medic@standing@tendtodead@idle_a", "idle_a", 8f);
                            NAPI.Task.Run(delegate
                            {
                                dbPlayer.SetHealth(100);
                                dbPlayer.TriggerEvent("client:respawning");
                                dbPlayer.StopProgressbar();
                                dbPlayer.IsFarming = false;
                                dbPlayer.RefreshData(dbPlayer);
                                dbPlayer.StopAnimation();
                                dbPlayer.disableAllPlayerActions(false);
                                dbPlayer.SendNotification("Du hast einen Verbandskasten benutzt.", 3000, "green");
                            }, 4000);
                        }
                    }
                    finally
                    {
                        reader.Dispose();
                    }
                }
                finally
                {
                    mySqlReaderCon.Connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.StackTrace);
            }
        }

        [RemoteEvent("Pressed_PUNKT")]
        public static void punkt(Client client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                if (dbPlayer.IsFarming)
                {
                    return;
                }


                if (dbPlayer.Faction.Name == "FIB")
                {
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                    mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (!reader.HasRows)
                            {
                                return;
                            }

                            reader.Read();
                            List<ItemModel> list = new List<ItemModel>();
                            string @string = reader.GetString("Items");
                            list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                            ItemModel itemToUse = list.Find((ItemModel x) => x.Name == "BeamtenSchutzweste");
                            if (itemToUse == null)
                            {
                                return;
                            }

                            int index = list.IndexOf(itemToUse);
                            if (itemToUse.Amount == 1)
                            {
                                list.Remove(itemToUse);
                            }
                            else
                            {
                                itemToUse.Amount--;
                                list[index] = itemToUse;
                            }

                            Item item = itemRegisterList.Find((Item x) => x.Name == itemToUse.Name);
                            reader.Close();
                            if (reader.IsClosed)
                            {
                                mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                                mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                                MySqlHandler.ExecuteSync(mySqlQuery);
                                dbPlayer.disableAllPlayerActions(true);
                                dbPlayer.SendProgressbar(4000);
                                dbPlayer.IsFarming = true;
                                dbPlayer.RefreshData(dbPlayer);
                                dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle",
                                    "gang_chatting_idle01", 8f);
                                NAPI.Task.Run(delegate
                                {
                                    dbPlayer.SetArmor(100);
                                    dbPlayer.TriggerEvent("client:respawning");
                                    dbPlayer.StopProgressbar();
                                    dbPlayer.IsFarming = false;
                                    dbPlayer.RefreshData(dbPlayer);
                                    dbPlayer.disableAllPlayerActions(false);
                                    dbPlayer.SendNotification("Du hast eine BeamtenSchutzweste benutzt.", 3000, "green");
                                    dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 3 : 3, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 0);
                                    dbPlayer.StopAnimation();
                                }, 4000);
                            }
                        }
                                       
                    finally
                        {
                            reader.Dispose();
                        }
                    }
                    finally
                    {
                        mySqlReaderCon.Connection.Dispose();
                    }

                }
                else
            {
                    if (client == null) return;
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        return;

                    if (dbPlayer.DeathData.IsDead) return;

                    if (dbPlayer.IsFarming)
                    {
                        return;
                    }
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);

                try
                {
                    MySqlDataReader reader = mySqlReaderCon.Reader;
                    try
                    {
                        if (!reader.HasRows)
                        {
                            return;
                        }

                        reader.Read();
                        List<ItemModel> list = new List<ItemModel>();
                        string @string = reader.GetString("Items");
                        list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                        ItemModel itemToUse = list.Find((ItemModel x) => x.Name == "Schutzweste");
                        if (itemToUse == null)
                        {
                            return;
                        }

                        int index = list.IndexOf(itemToUse);
                        if (itemToUse.Amount == 1)
                        {
                            list.Remove(itemToUse);
                        }
                        else
                        {
                            itemToUse.Amount--;
                            list[index] = itemToUse;
                        }

                        Item item = itemRegisterList.Find((Item x) => x.Name == itemToUse.Name);
                        reader.Close();
                        if (reader.IsClosed)
                        {
                            mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                            mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                            MySqlHandler.ExecuteSync(mySqlQuery);
                            dbPlayer.disableAllPlayerActions(true);
                            dbPlayer.SendProgressbar(4000);
                            dbPlayer.IsFarming = true;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle",
                                "gang_chatting_idle01", 8f);
                            NAPI.Task.Run(delegate
                            {
                                dbPlayer.SetArmor(100);
                                dbPlayer.TriggerEvent("client:respawning");
                                dbPlayer.StopProgressbar();
                                dbPlayer.IsFarming = false;
                                dbPlayer.RefreshData(dbPlayer);
                                dbPlayer.disableAllPlayerActions(false);
                                dbPlayer.SendNotification("Du hast eine Schutzweste benutzt.", 3000, "green");
                                dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 16 : 15, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 2 : 2);
                                dbPlayer.StopAnimation();
                            }, 4000);
                        }
                    }
                finally
                    {
                        reader.Dispose();
                    }
                }
                finally
                {
                    mySqlReaderCon.Connection.Dispose();
                }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_PUNKT] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_PUNKT] " + ex.StackTrace);
            }
        }


        public static bool getItemFunction(Client c, int id)
        {
            if (c == null) return false;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
            return false;

            Item item = itemRegisterList.Find((Item item2) => item2.Id == id);
            if (item == null) return false;

          
             if (item.Whash != WeaponHash.Unarmed)
            {
                WeaponManager.addWeapon(c, item.Whash);
                dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                return true;
            }
            else if(item.Name == "Schutzweste")
            {
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetArmor(100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.StopProgressbar();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.SendNotification("Du hast eine Schutzweste benutzt.", 3000, "green");
                        dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 16 : 15, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 2 : 2);
                        dbPlayer.StopAnimation();
                    }, 4000);
                    return true;
                }
            }
            else if (item.Name == "BeamtenSchutzweste")
            {
                if (dbPlayer.Faction.Name != "FIB")
                {
                    dbPlayer.SendNotification("Du kannst die Schutzweste nicht benutzen, da du kein Mitglied einer Staatsfraktion bist!", 3000, "green");
                    return true;
                }
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetArmor(100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.StopProgressbar();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.SendNotification("Du hast eine BeamtenSchutzweste benutzt.", 3000, "green");
                        dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 3 : 3, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 0);
                        dbPlayer.StopAnimation();
                    }, 4000);
                    return true;
                }
            }
            
            else if (item.Name == "BeamtenSpezialKarabiner")
            {
                if (dbPlayer.Faction.Name != "FIB")
                {
                    dbPlayer.SendNotification("Du kannst das nicht benutzen, da du kein Mitglied einer Staatsfraktion bist!", 3000, "green");
                    return true;
                }
                else
                {
                    dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                    WeaponManager.addWeapon(c, WeaponHash.SpecialCarbine);
                    return true;
                }
            }
            
            else if (item.Name == "BeamtenHeavyshotgun")
            {
                if (dbPlayer.Faction.Name != "FIB")
                {
                    dbPlayer.SendNotification("Du kannst das nicht benutzen, da du kein Mitglied einer Staatsfraktion bist!", 3000, "green");
                    return true;
                }
                else
                {
                    dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                    WeaponManager.addWeapon(c, WeaponHash.HeavyShotgun);
                    return true;
                }
            }
            else if (item.Name == "BeamtenAdvancedrifle")
            {
                if (dbPlayer.Faction.Name != "FIB")
                {
                    dbPlayer.SendNotification("Du kannst das nicht benutzen, da du kein Mitglied einer Staatsfraktion bist!", 3000, "green");
                    return true;
                }
                else
                {
                    dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                    WeaponManager.addWeapon(c, WeaponHash.AdvancedRifle);
                    return true;
                }
            }
            else if (item.Name == "BeamtenBullpupRifleMK2")
            {
                if (dbPlayer.Faction.Name != "FIB")
                {
                    dbPlayer.SendNotification("Du kannst das nicht benutzen, da du kein Mitglied einer Staatsfraktion bist!", 3000, "green");
                    return true;
                }
                else
                {
                    dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                    WeaponManager.addWeapon(c, WeaponHash.BullpupRifle);
                    return true;
                }
            }
            else if (item.Name == "BeamtenMG")
            {
                if (dbPlayer.Faction.Name != "FIB")
                {
                    dbPlayer.SendNotification("Du kannst das nicht benutzen, da du kein Mitglied einer Staatsfraktion bist!", 3000, "green");
                    return true;
                }
                else
                {
                    dbPlayer.SendNotification("Du hast die Waffe ausgerüstet.", 3000, "green", "Inventar");
                    WeaponManager.addWeapon(c, WeaponHash.MG);
                    return true;
                }
            }
            else if (item.Name == "Underarmor")
            {
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetArmor(dbPlayer.GetArmor() + 100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.StopProgressbar();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.SendNotification("Du hast eine Underarmor benutzt.", 3000, "green");
                        dbPlayer.SetClothes(9, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 16 : 15, (dbPlayer.Faction.Id != 0 && !dbPlayer.Faction.BadFraktion) ? 0 : 2);
                        dbPlayer.StopAnimation();
                    }, 4000);
                    return true;
                }
            }
            else if(item.Name == "Verbandskasten")
            {
                if (!dbPlayer.IsFarming)
                {
                    dbPlayer.SendProgressbar(4000);
                    dbPlayer.disableAllPlayerActions(true);
                    dbPlayer.IsFarming = true;
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.PlayAnimation(33, "anim@heists@narcotics@funding@gang_idle", "gang_chatting_idle01", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.SetHealth(100);
                        dbPlayer.TriggerEvent("client:respawning");
                        dbPlayer.StopAnimation();
                        dbPlayer.disableAllPlayerActions(false);
                        dbPlayer.StopProgressbar();
                        dbPlayer.IsFarming = false;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.SendNotification("Du hast einen Verbandskasten benutzt.", 3000, "green");
                    }, 4000);
                    return true;
                }
            }
            else if(item.Name == "Mietvertrag")
            {
                House house = HouseModule.houses.Find((House house2) => house2.OwnerId == dbPlayer.Id);
                if (house == null)
                {
                    dbPlayer.SendNotification("Du besitzt kein Haus!", 3000, "red");
                    return false;
                }
                
                dbPlayer.OpenTextInputBox(new TextInputBoxObject
                {
                    Title = "Mietvertrag",
                    Message = "Gebe bitte den Namen des Spielers ein, dem du gerne einen Mietvertrag machen möchtest.",
                    Callback = "sendMietvertrag"
                });
                return true;
            }

            else if (item.Name == "Magazin")
            {
                Client client = dbPlayer.Client;
                if (NAPI.Player.GetPlayerCurrentWeapon(client) != WeaponHash.Unarmed)
            {
                    WeaponHash weapon = client.CurrentWeapon;
                    dbPlayer.TriggerEvent("sendProgressbar", new object[1]
                    {
                    5000
                    });
                    dbPlayer.TriggerEvent("disableAllPlayerActions", new object[1]
                    {
                    true
                    });
                    //NAPI.Player.PlayPlayerAnimation(p, 33, "ac_ig_3_p3_b-0", "w_ar_assaultrifle_mag1-0", 8);
                    dbPlayer.PlayAnimation(33, "weapons@submg@micro_smg_str", "reload_aim", 8f);
                    NAPI.Task.Run(delegate
                    {
                        dbPlayer.GiveWeapon(weapon, 9999);
                        dbPlayer.SendNotification("Du hast 1 Magazin benutzt, deine Waffe ist nun wieder voll!", 3000, "green");
                        dbPlayer.TriggerEvent("disableAllPlayerActions", new object[1]
                        {
                            false
                        });
                        dbPlayer.StopAnimation();
                        dbPlayer.ResetData("IS_FARMING");

                    }, 5000);
                    }
                else
                { 
                    dbPlayer.SendNotification("Du musst eine Waffe in der Hand halten!", 3000, "red");
                }
        }

        
            else if (item.Name == "AspectsDildo")
            {
                Client client = dbPlayer.Client;
                {
                    //NAPI.Player.PlayPlayerAnimation(p, 33, "ac_ig_3_p3_b-0", "w_ar_assaultrifle_mag1-0", 8);
                    dbPlayer.PlayAnimation(33, "rcmpaparazzo_2", "shag_loop_poppy", 8f); 
                }
            }

            else if (item.Name == "Schweissgeraet")
            {
                return Schweissgeraet.useSchweissgeraet(dbPlayer);
            }

            return false;
        }
    }
}*/
