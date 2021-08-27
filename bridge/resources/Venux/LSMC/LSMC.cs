using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Venux.Menus;

namespace Venux.LSMC
{
	class LSMC : Script
	{
		[ServerEvent(Event.ResourceStart)]
		public void onResourceStart()
		{
			//ColShape val = NAPI.ColShape.CreateCylinderColShape(new Vector3(299.1804, -584.7746, 42.16085), 2f, 2f, 0);
			//val.SetData("COLSHAPE_FUNCTION", new FunctionModel("goInDuty"));
			//val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um in den Dienst zu gehen", "LSMC", "red", 4500));
			//val.SetData("COLSHAPE_FUNCTION", new FunctionModel("goOffDuty"));
			//val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um aus dem Dienst zu gehen", "LSMC", "red", 4500));
		}

		[RemoteEvent("Medicsystem")]
		public void Medicsystem(Client p)
		{
			if (p.GetSharedData("FRAKTION") == "LSMC")
			{
				NativeMenu nativeMenu = new NativeMenu("Medicsystem", "Aktionen", new List<NativeItem>
				{
					new NativeItem("Wiederbeleben", "reviveplayer"),
					new NativeItem("Wunden versorgen", "healwounds"),
					new NativeItem("Ins Auto packen", "packplayer"),
					new NativeItem("In die Notaufnahme verlegen", "getplayer")
				});
				nativeMenu.showNativeMenu(p);
			}
			else
			{
				Notification.SendPlayerNotifcation(p, "Du bist kein Medic", 4500, "red", "LSMC", "");
			}
		}

		[RemoteEvent("nM-Medicsystem")]
		public void nmMedicsystem(Client p, string selection, Client target = null)
		{
			try
			{
				if (selection == "reviveplayer")
				{
					if (p.Position.DistanceTo(target.Position) < 2.5f)
					{

						if (Database.isPlayerDeath(target.Name))
						{
							NAPI.Player.PlayPlayerAnimation(p, 33, "mini@repair", "fixing_a_player", 8);
							p.TriggerEvent("sendProgressbar", new object[1]
							{
							60000
							});
							Functions.disableAllPlayerControls(p, true);
							Notification.SendPlayerNotifcation(p, "Du behandelst gerade jemanden", 4500, "red", "LSMC", "");
							Notification.SendPlayerNotifcation(target, "DU wirst gerade Behandelt", 4500, "red", "", "");
							NAPI.Task.Run(delegate
							{
								Functions.disableAllPlayerControls(p, false);
								target.TriggerEvent("stopScreenEffect", "DeathFailOut");
								target.TriggerEvent("setInvincible", false);
								target.TriggerEvent("disableAllPlayerActions", false);
								target.Health = 200;
								target.Armor = 0;
								Items.Weapons.WeaponSync(target);
								Database.changeMoney(p.Name, 3500, false);
								Notification.SendPlayerNotifcation(p, "Du hast 1.000$ fürs Wiederbeleben eines Spieler bekommen", 4500, "red", "LSMC", "");
								Notification.SendPlayerNotifcation(target, "Du wurdest wiederbelebt", 4500, "red", "", "");


							}, 60000);
						}
					}
				}
				else if (selection == "healwounds")
				{
					if (target.Health < 100)
					{
						if (p.Position.DistanceTo(target.Position) < 1f)
						{
							NAPI.Player.PlayPlayerAnimation(p, 33, "mini@repair", "fixing_a_player", 8);
							Functions.disableAllPlayerControls(p, true);
							p.TriggerEvent("sendProgressbar", new object[1]
							{
							45000
							});
							NAPI.Task.Run(delegate
							{
								target.Health = 100;
								Functions.disableAllPlayerControls(p, false);
								NAPI.Player.StopPlayerAnimation(p);
								Notification.SendPlayerNotifcation(p, "Du hast " + target.Name + " behandelt", 4500, "red", "LSMC", "");
								Notification.SendPlayerNotifcation(target, "Du wurdest behandelt", 4500, "red", "LSMC", "");

							}, 45000);
						}
						else
						{
							Notification.SendPlayerNotifcation(p, "In deiner Nähe ist kein Spieler", 4500, "red", "LSMC", "");
						}
					}
				}
				else if (selection == "packplayer")
				{
					Notification.SendPlayerNotifcation(p, "Dieses Feature kommt bald hinzu", 4500, "red", "FEATURE", "");

				}
				else if (selection == "getplayer")
				{
					Notification.SendPlayerNotifcation(p, "Dieses Feature kommt bald hinzu", 4500, "red", "FEATURE", "");
				}

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		[RemoteEvent("goInHospital")]
		public void goInHospital(Client p)
		{
			try
			{
				p.Position = new Vector3(300.1892, -584.9624, 42.18405).Add(new Vector3(0, 0, 1.5));
				return;

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		[RemoteEvent("goOutHospital")]
		public void goOutHospital(Client p)
		{
			try
			{
				p.Position = new Vector3(299.1804, -584.7746, 42.16085).Add(new Vector3(0, 0, 1.5));
				return;

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		[RemoteEvent("goInDuty")]
		public void goInDuty(Client p)
		{
			try
			{
				if (p.HasData("IS_MDUTY") == true)
					return;

				if (p.GetSharedData("FRAKTION") == "Los Santos Medical Department")
				{
					p.TriggerEvent("setPlayerMDuty", true);
					p.SetData("IS_MDUTY", true);
					Notification.SendPlayerNotifcation(p, "Du bist nun im Dienst", 4500, "red", "LSMC", "");
				}

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		[RemoteEvent("goOffDuty")]
		public void goOffDuty(Client p)
		{
			try
			{
				if (p.HasData("IS_MOffDUTY") == true)
					return;

				if (p.GetSharedData("FRAKTION") == "Los Santos Medical Department")
				{
					p.TriggerEvent("setPlayerMDuty", true);
					p.SetData("IS_MOffDUTY", true);
					Notification.SendPlayerNotifcation(p, "Du bist nun außer Dienst", 4500, "red", "LSMC", "");
				}

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}
	}
}
