using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

/*namespace Venux
{
    public class Inventory : Script
    {
        private static object player;

        public static object HouseModule { get; private set; }
        public static object PlayerHandler { get; private set; }
        public static object Logger { get; private set; }

        public static bool isNearby(Client client, Vehicle veh)
		{
			return ((Entity)veh).Position.DistanceTo(((Entity)client).Position) <= 10f;
		}

		public static Vehicle GetClosestVehicle(Client sender, float distance = 1000f)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			Vehicle result = null;
			foreach (Vehicle allVehicle in NAPI.Pools.GetAllVehicles())
			{
				Vector3 entityPosition = NAPI.Entity.GetEntityPosition(allVehicle);
				float num = ((Entity)sender).Position.DistanceTo(entityPosition);
				if (num < distance)
				{
					distance = num;
					result = allVehicle;
				}
			}
			return result;
		}

		public static Client GetClosestClient(Client sender, float distance = 1000f)
		{
			Client result = null;
			foreach (Database.dbPlayer in PlayerHandler.GetPlayers())
			{
				Vector3 position = player.Position;
				float num = ((Entity)sender).Position.DistanceTo(position);
				if (num < distance && (Entity)(object)player.Client != (Entity)(object)sender)
				{
					distance = num;
					result = player.Client;
				}
			}
			return result;
		}

		[RemoteEvent("requestInventory")]
		public static void requestItems(Client c)
		{
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Expected O, but got Unknown
			//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c9: Expected O, but got Unknown
			if ((Entity)(object)c == (Entity)null)
			{
				return;
			}
			Database player = c.GetPlayer();
			if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null)
			{
				return;
			}
			if (player.IsFarming)
			{
				player.SendNotification("Hör erstmal auf zu farmen.");
			}
			else
			{
				if (player.DeathData.IsDead)
				{
					return;
				}
				try
				{
					((Entity)c).SetData("USING_FRAKLAGER", (object)false);
					((Entity)c).SetData("USING_LAGER", (object)false);
					((Entity)c).SetData("USING_TRESOR", (object)false);
					Vehicle closestVehicle = GetClosestVehicle(c, 5f);
					bool flag = false;
						if (flag2 && house != null)
						{
							((Entity)c).SetData("USING_LAGER", (object)true);
							c.TriggerEvent("openWindow", new object[2]
							{
							"Inventory",
							"{\"inventory\":[{\"Id\":" + player.Id + ",\"Name\":\"Inventar\",\"Money\":" + player.Money + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)player.GetInventoryItems()) + "}, {\"Id\":" + house.Id + ",\"Name\":\"Hauslager\",\"Money\":0,\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":80000,\"MaxSlots\":30,\"Slots\":" + NAPI.Util.ToJson((object)house.GetHouseItems()) + "}]}"
							});
						}
						else if (((Entity)c).Position.DistanceTo(new Vector3(1712.6771, 4766.299, 13.11)) > 20f)
						{
							c.TriggerEvent("openWindow", new object[2]
							{
							"Inventory",
							"{\"inventory\":[{\"Id\":" + player.Id + ",\"Name\":\"Inventar\",\"Money\":" + player.Money + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)player.GetInventoryItems()) + "}]}"
							});
						}
						else if (((Entity)c).Position.DistanceTo(new Vector3(1712.6771, 4766.299, 13.11)) < 20f && player.Faction.Id != 0)
						{
							((Entity)c).SetData("USING_FRAKLAGER", (object)true);
							c.TriggerEvent("openWindow", new object[2]
							{
							"Inventory",
							"{\"inventory\":[{\"Id\":" + player.Id + ",\"Name\":\"Inventar\",\"Money\":" + player.Money + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)player.GetInventoryItems()) + "}, {\"Id\":" + player.Faction.Id + ",\"Name\":\"Fraktionslager\",\"Money\":0,\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)player.Faction.GetFactionStorageItems()) + "}]}"
							});
						}
					}
					else if (closestVehicle.NumberPlate != null && ((Entity)closestVehicle).HasData("vehicle"))
					{
						Database vehicle = closestVehicle.GetVehicle();
						if (vehicle != null && vehicle.IsValid() && !((Entity)(object)vehicle.Vehicle == (Entity)null))
						{
							((Entity)c).SetData("PLAYER_LAST_KOFFERRAUM", (object)vehicle);
							c.TriggerEvent("openWindow", new object[2]
							{
							"Inventory",
							"{\"inventory\":[{\"Id\":" + player.Id + ",\"Name\":\"Inventar\",\"Money\":" + player.Money + ",\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)player.GetInventoryItems()) + "}, {\"Id\":" + vehicle.Id + ",\"Name\":\"Kofferraum\",\"Money\":0,\"Blackmoney\":0,\"Weight\":0,\"MaxWeight\":40000,\"MaxSlots\":12,\"Slots\":" + NAPI.Util.ToJson((object)vehicle.GetVehicleItems()) + "}]}"
							});
						}
					}
				}
				catch (Exception ex)
				{
					//Logger.Print("[EXCEPTION requestItems] " + ex.Message);
					//Logger.Print("[EXCEPTION requestItems] " + ex.StackTrace);
				}
			}
		}

		[RemoteEvent("useInventoryItem")]
		public static void useInventoryItem(Client client, string name)
		{
			if ((Entity)(object)client == (Entity)null)
			{
				return;
			}
			Database player = client.GetPlayer();
			if (player == null || !player.IsValid(ignorelogin: true) || (Entity)(object)player.Client == (Entity)null)
			{
				return;
			}
			try
			{
				ItemModel itemModel = player.GetInventoryItems().Find((ItemModel model) => model.Name == name);
				if (itemModel != null && itemModel.Amount > 0 && ItemModule.getItemFunction(client, itemModel.Id))
				{
					itemModel.Amount = 1;
					player.UpdateInventoryItems(itemModel.Name, 1, remove: true);
					WebhookSender.SendMessage("TEst", "Der Spieler " + player.Name + " hat das item " + itemModel.Name + " genutzt.", Webhooks.revivelogs, "Revive");
				}
			}
			catch (Exception ex)
			{
				//Logger.Print("[EXCEPTION useInventoryItem] " + ex.Message);
				//Logger.Print("[EXCEPTION useInventoryItem] " + ex.StackTrace);
			}
		}

		[RemoteEvent("fillInventorySlots")]
		public static void fillInventorySlots(Client client, string oldInventar)
		{
			try
			{
				if (!((Entity)(object)client == (Entity)null))
				{
					requestItems(client);
				}
			}
			catch (Exception ex)
			{
				//Logger.Print("[EXCEPTION fillInventorySlots] " + ex.Message);
				//Logger.Print("[EXCEPTION fillInventorySlots] " + ex.StackTrace);
			}
		}

		[RemoteEvent("dropInventoryItem")]
		public static void dropInventoryItem(Client client, string name, int amount)
		{
			if ((Entity)(object)client == (Entity)null)
			{
				return;
			}
			Database dbPlayer = client.GetPlayer();
			if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null)
			{
				return;
			}
			//Logger.Print("dropInventoryItem " + dbPlayer.Name + " " + name + " " + amount);
			if (amount < 1)
			{
				dbPlayer.SendNotification("hoff nicht");
				return;
			}
			try
			{
				if (amount == 0)
				{
					return;
				}
				ItemModel itemModel = dbPlayer.GetInventoryItems().Find((ItemModel model) => model.Name == name);
				if (itemModel == null || itemModel.Amount < 1)
				{
					return;
				}
				dbPlayer.UpdateInventoryItems(itemModel.Name, amount, remove: true);
				if (client.IsInVehicle)
				{
					return;
				}
				NAPI.Player.PlayPlayerAnimation(client, 33, "anim@mp_snowball", "pickup_snowball", 8f);
				NAPI.Task.Run((Action)delegate
				{
					if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
					{
						dbPlayer.StopAnimation();
					}
				}, 2000L);
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION dropInventoryItem] " + ex.Message);
				Logger.Print("[EXCEPTION dropInventoryItem] " + ex.StackTrace);
			}
		}

		[RemoteEvent("giveInventoryItem")]
		public static void giveInventoryItem(Client client, string name, int amount)
		{
			if ((Entity)(object)client == (Entity)null)
			{
				return;
			}
			DbPlayer dbPlayer = client.GetPlayer();
			if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null)
			{
				return;
			}
			Logger.Print("giveInventoryItem " + dbPlayer.Name + " " + name + " " + amount);
			if (amount < 1)
			{
				dbPlayer.SendNotification("hoff nicht");
				return;
			}
			try
			{
				if ((Entity)(object)GetClosestClient(client, 5f) == (Entity)null)
				{
					dbPlayer.SendNotification("Es wurde kein Spieler gefunden.", 3000, "red");
				}
				else
				{
					if (amount <= 0)
					{
						return;
					}
					MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
					mySqlQuery.AddParameter("@userId", dbPlayer.Id);
					MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
					try
					{
						MySqlDataReader reader = query.Reader;
						try
						{
							if (!((DbDataReader)(object)reader).HasRows)
							{
								return;
							}
							((DbDataReader)(object)reader).Read();
							List<ItemModel> list = new List<ItemModel>();
							string @string = reader.GetString("Items");
							list = NAPI.Util.FromJson<List<ItemModel>>(@string);
							ItemModel itemToUse = list.Find((ItemModel x) => x.Name == name);
							if (itemToUse.Amount == 1)
							{
								list.Remove(itemToUse);
								goto IL_0248;
							}
							if (itemToUse.Amount < amount)
							{
								return;
							}
							itemToUse.Amount -= amount;
							if (itemToUse.Amount <= 0)
							{
								list.Remove(itemToUse);
							}
							goto IL_0248;
						IL_0248:
							Client player = GetClosestClient(client, 5f);
							if (((Entity)client).HasData("PLAYER_GIVEITEM"))
							{
								player = (Client)(dynamic)((Entity)client).GetData("PLAYER_GIVEITEM");
							}
							((Entity)client).ResetData("PLAYER_GIVEITEM");
							DbPlayer player2 = player.GetPlayer();
							if (player2 == null || !player2.IsValid(ignorelogin: true))
							{
								return;
							}
							WebhookSender.SendMessage("Spieler gibt Item", "Der Spieler " + dbPlayer.Name + " hat " + player2.Name + " das Item " + itemToUse.Name + " " + amount + "x gegeben.", Webhooks.itemlogs, "Item");
							Item item = ItemModule.itemRegisterList.Find((Item x) => x.Name == itemToUse.Name);
							player2.UpdateInventoryItems(item.Name, amount, remove: false);
							((DbDataReader)(object)reader).Close();
							if (((DbDataReader)(object)reader).IsClosed)
							{
								mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
								mySqlQuery.Parameters = new List<MySqlParameter>
							{
								new MySqlParameter("@invItems", NAPI.Util.ToJson((object)list)),
								new MySqlParameter("@pId", dbPlayer.Id)
							};
								MySqlHandler.ExecuteSync(mySqlQuery);
							}
							NAPI.Player.PlayPlayerAnimation(client, 33, "anim@mp_snowball", "pickup_snowball", 8f);
							NAPI.Task.Run((Action)delegate
							{
								if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
								{
									dbPlayer.StopAnimation();
								}
							}, 2000L);
							dbPlayer.SendNotification("Du hast dem Spieler " + player2.Name + " " + amount + "x " + name + " übergeben.", 3000, "green");
						}
						finally
						{
							reader.Dispose();
						}
					}
					finally
					{
						query.Connection.Dispose();
					}
					return;
				}
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION giveInventoryItem] " + ex.Message);
				Logger.Print("[EXCEPTION giveInventoryItem] " + ex.StackTrace);
			}
		}

		[RemoteEvent("moveItemToInventory")]
		public static void moveItemToInventory(Client client, string name, int oldInventory, int amount)
		{
			if ((Entity)(object)client == (Entity)null)
			{
				return;
			}
			DbPlayer dbPlayer = client.GetPlayer();
			if (dbPlayer == null || !dbPlayer.IsValid(ignorelogin: true) || (Entity)(object)dbPlayer.Client == (Entity)null)
			{
				return;
			}
			Logger.Print("moveItemToInventory " + dbPlayer.Name + " " + name + " " + oldInventory + " " + amount);
			if (amount < 1)
			{
				dbPlayer.SendNotification("hoff nicht");
				return;
			}
			try
			{
				if (amount <= 0)
				{
					return;
				}
				bool flag = oldInventory == dbPlayer.Id;
				List<ItemModel> list = new List<ItemModel>();
				List<ItemModel> inventoryItems = dbPlayer.GetInventoryItems();
				MySqlQuery mySqlQuery = new MySqlQuery("");
				string text = "Items";
				string text2 = "inventorys";
				int num = 0;
				/*if ((dynamic)((Entity)client).GetData("USING_FRAKLAGER") == true)
				{
					if (dbPlayer.Faction.Id != 0)
					{
						mySqlQuery = new MySqlQuery("SELECT * FROM fraktionen WHERE Id = @id");
						mySqlQuery.AddParameter("@id", dbPlayer.Faction.Id);
						text = "StorageData";
						text2 = "fraktionen";
						num = dbPlayer.Faction.Id;
						goto IL_0703;
					}
				}
				else if ((dynamic)((Entity)client).GetData("IN_LABOR") == true)
				{
					if (dbPlayer.Faction.Id != 0 && ((Entity)client).HasData("USING_STORAGE"))
					{
						bool flag2 = (dynamic)((Entity)client).GetData("USING_STORAGE");
						Laboratory laboratory3 = LaboratoryModule.laboratories.Find((Laboratory laboratory2) => laboratory2.FactionId == dbPlayer.Faction.Id);
						if (laboratory3 != null)
						{
							mySqlQuery = new MySqlQuery("SELECT * FROM laboratorys WHERE Id = @id");
							mySqlQuery.AddParameter("@id", laboratory3.Id);
							text = ((!flag2) ? "ProductInventorys" : "StorageInventorys");
							text2 = "laboratorys";
							num = laboratory3.Id;
							goto IL_0703;
						}
					}
				}*/
				/*else if (((Entity)client).HasData("PLAYER_LAST_KOFFERRAUM"))
				{
					Database dbVehicle = (dynamic)((Entity)client).GetData("PLAYER_LAST_KOFFERRAUM");
					if (dbVehicle != null)
					{
						mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id");
						mySqlQuery.AddParameter("@id", dbVehicle.Id);
						text = "Items";
						text2 = "vehicles";
						num = dbVehicle.Id;
					}
				}
            object MySqlHandler = null;
            MySqlResult query = MySqlHandler.GetQuery(mySqlQuery);
				MySqlDataReader reader = query.Reader;
				if (((DbDataReader)(object)reader).HasRows)
				{
					while (((DbDataReader)(object)reader).Read())
					{
						if (text.Contains("Inventorys"))
						{
							Dictionary<int, List<ItemModel>> dictionary = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(reader.GetString(text));
							if (!dictionary.ContainsKey(dbPlayer.Id))
							{
								//dictionary.Add(Databse vehicle List<ItemModel>());
							}
							list = dictionary[dbPlayer.Id];
						}
						else
						{
							list = NAPI.Util.FromJson<List<ItemModel>>(reader.GetString(text));
						}
					}
				}
				if (flag)
				{
					ItemModel itemModel6 = inventoryItems.Find((ItemModel itemModel2) => itemModel2.Name == name);
					if (itemModel6 == null)
					{
						return;
					}
					if (amount > itemModel6.Amount)
					{
						dbPlayer.SendNotification("hoff nicht");
						return;
					}
					if (itemModel6.Amount >= amount)
					{
						inventoryItems.Remove(itemModel6);
						itemModel6.Amount -= amount;
						if (itemModel6.Amount > 0)
						{
							inventoryItems.Add(itemModel6);
						}
					}
					mySqlQuery.Parameters.Clear();
					mySqlQuery = new MySqlQuery("UPDATE inventorys SET Items = @items WHERE Id = @id");
					mySqlQuery.AddParameter("@id", dbPlayer.Id);
					mySqlQuery.AddParameter("@items", NAPI.Util.ToJson((object)inventoryItems));
					MySqlHandler.ExecuteSync(mySqlQuery);
					ItemModel itemModel7 = list.Find((ItemModel itemModel4) => itemModel4.Name == itemModel6.Name);
					if (itemModel7 != null && itemModel7.Amount > 0)
					{
						itemModel7.Amount += amount;
					}
					else
					{
						itemModel7 = new ItemModel
						{
							Id = itemModel6.Id,
							Amount = amount,
							ImagePath = itemModel6.ImagePath,
							Slot = list.Count + 1,
							Weight = 0,
							Name = itemModel6.Name
						};
						list.Add(itemModel7);
					}
					if (text2 == "laboratorys")
					{
						Dictionary<int, List<ItemModel>> dictionary2 = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(reader.GetString(text));
						if (!dictionary2.ContainsKey(dbPlayer.Id))
						{
							dictionary2.Add(dbPlayer.Id, list);
						}
						dictionary2[dbPlayer.Id] = list;
						mySqlQuery.Parameters.Clear();
						mySqlQuery = new MySqlQuery("UPDATE " + text2 + " SET " + text + " = @items WHERE Id = @id");
						mySqlQuery.AddParameter("@id", num);
						mySqlQuery.AddParameter("@items", NAPI.Util.ToJson((object)dictionary2));
						MySqlHandler.ExecuteSync(mySqlQuery);
						goto IL_0b6e;
					}
					mySqlQuery.Parameters.Clear();
					mySqlQuery = new MySqlQuery("UPDATE " + text2 + " SET " + text + " = @items WHERE Id = @id");
					mySqlQuery.AddParameter("@id", num);
					mySqlQuery.AddParameter("@items", NAPI.Util.ToJson((object)list));
					MySqlHandler.ExecuteSync(mySqlQuery);
					if (!(text2 == "houses"))
					{
						goto IL_0b6e;
					}
					House house4 = HouseModule.houses.Find((House house2) => house2.OwnerId == dbPlayer.Id || house2.TenantsIds.Contains(dbPlayer.Id));
					if (house4 == null)
					{
						return;
					}
					HouseModule.houses.Remove(house4);
					house4.Inventory = list;
					HouseModule.houses.Add(house4);
					goto IL_0b6e;
				}
				if (((dynamic)((Entity)client).GetData("USING_FRAKLAGER") == true) && dbPlayer.Factionrank < 10)
				{
					dbPlayer.SendNotification("Du brauchst einen höheren Rang um etwas aus dem Fraktionslager rauszunehmen!", 3000, "white", "FRAKLAGER");
					return;
				}
				ItemModel itemModel5 = list.Find((ItemModel itemModel2) => itemModel2.Name == name);
				if (amount > itemModel5.Amount)
				{
					dbPlayer.SendNotification("hoff nicht");
					return;
				}
				if (itemModel5.Amount >= amount)
				{
					list.Remove(itemModel5);
					itemModel5.Amount -= amount;
					if (itemModel5.Amount > 0)
					{
						list.Add(itemModel5);
					}
				}
				if (text2 == "laboratorys")
				{
					Dictionary<int, List<ItemModel>> dictionary3 = NAPI.Util.FromJson<Dictionary<int, List<ItemModel>>>(reader.GetString(text));
					if (dictionary3 == null)
					{
						return;
					}
					if (!dictionary3.ContainsKey(dbPlayer.Id))
					{
						dictionary3.Add(dbPlayer.Id, list);
					}
					dictionary3[dbPlayer.Id] = list;
					mySqlQuery.Parameters.Clear();
					mySqlQuery = new MySqlQuery("UPDATE " + text2 + " SET " + text + " = @items WHERE Id = @id");
					mySqlQuery.AddParameter("@id", num);
					mySqlQuery.AddParameter("@items", NAPI.Util.ToJson((object)dictionary3));
					MySqlHandler.ExecuteSync(mySqlQuery);
					goto IL_0ff9;
				}
				mySqlQuery.Parameters.Clear();
				mySqlQuery = new MySqlQuery("UPDATE " + text2 + " SET " + text + " = @items WHERE Id = @id");
				mySqlQuery.AddParameter("@id", num);
				mySqlQuery.AddParameter("@items", NAPI.Util.ToJson((object)list));
				MySqlHandler.ExecuteSync(mySqlQuery);
				goto IL_0ff9;
			IL_0b6e:
				//WebhookSender.SendMessage("Item wird verschoben", dbPlayer.Name + " hat das Item " + name + " " + amount + "x in sein eigenes Inventar verschoben. Von: Table " + text2 + " | ItemList " + text + " | Id " + num, Webhooks.inventarlogs, "Inventar-Log");
				goto IL_11de;
			IL_0ff9:
				//WebhookSender.SendMessage("Item wird verschoben", dbPlayer.Name + " hat das Item " + name + " " + amount + "x verschoben. In: Table " + text2 + " | ItemList " + text + " | Id " + num, Webhooks.inventarlogs, "Inventar-Log");
				ItemModel itemModel8 = inventoryItems.Find((ItemModel itemModel4) => itemModel4.Name == itemModel5.Name);
				if (itemModel8 != null && itemModel8.Amount > 0)
				{
					itemModel8.Amount += amount;
				}
				else
				{
					itemModel8 = new ItemModel
					{
						Id = itemModel5.Id,
						Amount = amount,
						ImagePath = itemModel5.ImagePath,
						Slot = inventoryItems.Count + 1,
						Weight = 0,
						Name = itemModel5.Name
					};
					inventoryItems.Add(itemModel8);
				}
				mySqlQuery.Parameters.Clear();
				mySqlQuery = new MySqlQuery("UPDATE inventorys SET Items = @items WHERE Id = @id");
				mySqlQuery.AddParameter("@id", dbPlayer.Id);
				mySqlQuery.AddParameter("@items", NAPI.Util.ToJson((object)inventoryItems));
				MySqlHandler.ExecuteSync(mySqlQuery);
				if (!(text2 == "houses"))
				{
					goto IL_11de;
				}
				House house5 = HouseModule.houses.Find((House house2) => house2.OwnerId == dbPlayer.Id || house2.TenantsIds.Contains(dbPlayer.Id));
				if (house5 == null)
				{
					return;
				}
				HouseModule.houses.Remove(house5);
				house5.Inventory = list;
				HouseModule.houses.Add(house5);
				goto IL_11de;
			IL_11de:
				reader.Dispose();
				query.Connection.Dispose();
				if (!client.IsInVehicle)
				{
					NAPI.Player.PlayPlayerAnimation(client, 33, "anim@mp_snowball", "pickup_snowball", 8f);
					NAPI.Task.Run((Action)delegate
					{
						if (NAPI.Pools.GetAllPlayers().Contains(dbPlayer.Client))
						{
							dbPlayer.StopAnimation();
						}
					}, 2000L);
				}
				client.TriggerEvent("closeWindow", new object[1] { "Inventory" });
				requestItems(client);
			end_IL_00e8:;
			}
			catch (Exception ex)
			{
				Logger.Print("[EXCEPTION moveItemToInventory] " + ex.Message);
				Logger.Print("[EXCEPTION moveItemToInventory] " + ex.StackTrace);
			}
		}
	}
}*/
