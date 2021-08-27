using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Venux.ClothingShops;
using System.Timers;

namespace Venux.Jail
{
	public class Jail : Script
	{
		public static int jailtime = 0;
		public static Vector3 prisonIn = new Vector3(1691.46, 2565.555, 44.46485);
		public static Vector3 prisonOut = new Vector3(1845.698, 2585.883, 44.57205);


		public static List<ClothingModel> jailClothes = new List<ClothingModel>();


		[ServerEvent(Event.ResourceStart)]
		public void onResourceStart()
		{
			NAPI.Blip.CreateBlip(188, new Vector3(1846.464, 2603.645, 44.48911), 1.0f, 1, "Boilingbroke Jail", 255, 0, true, 0, 0);
			Log.Write("Jail geladen");
		}

		[Command("jail")]
		public void CMD_JAIL(Client p, string name, int jailtime)
		{
			try
			{
				foreach (KeyValuePair<Client, string> item in Start.loggedInPlayers)
				{
					bool found = false;

					if (item.Value == name)
					{

						if (Database.getPlayerFraktion(p.Name) == "LSPD")
						{
							Clothing.PlayerClothes.setClothes(item.Key, 11, 0, 4);
							Clothing.PlayerClothes.setClothes(item.Key, 4, 7, 15);
							Clothing.PlayerClothes.setClothes(item.Key, 6, 7, 1);
							if (item.Key.GetSharedData("FRAKTION") == "LSPD")
							{
								Notification.SendPlayerNotifcation(item.Key, "Der Spieler " + p.Name + " wurde für " + jailtime + " inhaftiert", 4500, "grey", "JAIL", "");
							}
							Database.updateJailtime(item.Key.Name, jailtime);
							NAPI.Task.Run(delegate
							{
								Database.updateJailtime(item.Key.Name, 0);
								Notification.SendPlayerNotifcation(item.Key, "Du bist aus dem Gefängnis befreit", 4500, "grey", "JAIL", "");
								Anticheat.Wait(item.Key); item.Key.Position = prisonOut.Add(new Vector3(0, 0, 1.5));

							}, jailtime * 60000);
							Notification.SendPlayerNotifcation(item.Key, "Du bist für " + jailtime + " im Gefängis", 4500, "grey", "Jail", "");
							Anticheat.Wait(item.Key); item.Key.Position = prisonIn.Add(new Vector3(0, 0, 1.5));

						}
						else
						{
							Notification.SendPlayerNotifcation(item.Key, "Du bist nicht berechtigt dazu", 4500, "red", "JAIL", "");
						}
					}

					if (!found)
						return;
				}
			}
			catch (Exception ex)
			{
				Log.Write("Error while jail player: " + ex.Message);
			}
		}

		[Command("unjail")]
		public void CMD_UNJAIL(Client p, string name)
		{
			try
			{
				bool found = false;

				foreach (KeyValuePair<Client, string> item in Start.loggedInPlayers)
				{

					if (Database.getPlayerFraktion(item.Key.Name) == "Los Santos Police Department")
					{

						Database.updateJailtime(item.Key.Name, 0);
						Anticheat.Wait(item.Key); item.Key.Position = prisonOut.Add(new Vector3(0, 0, 1.5));
						Notification.SendPlayerNotifcation(item.Key, "Du bist aus dem Gefängnis entlassen", 4500, "grey", "BUTZBACH", "");

					}
					else
					{
						Notification.SendPlayerNotifcation(p, "Du bist nicht berechtigt dazu", 4500, "red", "BUTZBACH", "");
					}
				}

				if (!found)
					Notification.SendPlayerNotifcation(p, "Dieser Spieler existiert nicht", 4500, "red", "JAIL", "");

			}
			catch (Exception ex)
			{
				Log.Write("Error while unjail Player: " + ex.Message);
			}
		}

		[Command("jailtime")]
		public void CMD_JAILTIME(Client p)
		{
			try
			{
				Notification.SendPlayerNotifcation(p, "Du sitzt noch " + Database.getUserJailtime(p.Name) + " im Gefängnis", 4500, "grey", "JAIL", "");
			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		public void getJailPlayer(Client p)
		{
			if (Database.getUserJailtime(p.Name) > 0)
			{
				CMD_JAIL(p, p.Name, (int)Database.getUserJailtime(p.Name));
			}
		}

	}
}
