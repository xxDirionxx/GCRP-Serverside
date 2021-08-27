/*using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Venux.Dokumentfälscherei
{
	public class Dokumentfälscherei : Script
	{
		public static Vector3 dokumentPos = new Vector3(1138.132, -3199.072, -40.76569);

		[ServerEvent(Event.ResourceStart)]
		public void onResourceStart()
		{
			ColShape val = NAPI.ColShape.CreateCylinderColShape(new Vector3(2889.577, 4391.291, 49.35161), 2f, 2f, 0);
			val.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterFaelscherei"));
			val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um die Fälscherei zu betreten", "FÄLSCHEREI", "grey", 4500));

			ColShape val2 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1138.132, -3199.108, -40.76571), 2f, 2f, 0);
			val2.SetData("COLSHAPE_FUNCTION", new FunctionModel("exitFaelscherei"));
			val2.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um die Fälscherei zu verlassen", "FÄLSCHEREI", "grey", 4500));

			ColShape val3 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1117.015, -3196.73, -41.49728), 2f, 2f, uint.MaxValue);
			val3.SetData("COLSHAPE_FUNCTION", new FunctionModel("faelschPerso"));
			val3.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um einen Personalausweis für 300.000$ zu Fälschen", "FÄLSCHEREI", "grey", 4500));

			ColShape val4 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1135.308, -3194.232, -41.49701), 2f, 2f, uint.MaxValue);
			val4.SetData("COLSHAPE_FUNCTION", new FunctionModel("faelschDienstausweis"));
			val4.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um einen Dienstausweis für 450.000$ zu Fälschen", "FÄLSCHEREI", "grey", 4500));

			ColShape val5 = NAPI.ColShape.CreateCylinderColShape(new Vector3(), 2f, 2f, uint.MaxValue);
			val5.SetData("COLSHAPE_FUNCTION", new FunctionModel("faelschDokumenteEinreisen"));
			val5.SetData("COLSHAPE_MESSAGE", new Notification.Message("Drücke E um Einreise Dokumente für 65.000$ zu Faelschen", "FÄLSCHEREI", "grey", 4500));

			Log.Write("Dokumentfälscherei geladen.");
		}

		[RemoteEvent("enterFaelscherei")]
		public void enterFalsch(Client p)
		{
			try
			{
				p.Position = dokumentPos.Add(new Vector3(0, 0, 1.5));
				p.Dimension = 2222;
				p.TriggerEvent("loadblackmoneyInterior");
				Notification.SendPlayerNotifcation(p, "Du hast die Dokumentfälscherei betreten", 4500, "white", "FÄLSCHEREI", "");
				return;
			} catch(Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		[RemoteEvent("exitFaelscherei")]
		public void exitFalsch(Client p)
		{
			try
			{
				p.Position = new Vector3(2889.577, 4391.291, 49.35161).Add(new Vector3(0, 0, 1.5));
				p.Dimension = 0;
				Notification.SendPlayerNotifcation(p, "Du hast die Dokumentfälscherei verlassen", 4500, "white", "FÄLSCHEREI", "");
				return;

			} catch(Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

		[RemoteEvent("faelschPerso")]
		public void faelschPerso(Client p)
		{
			try
			{
				if (p.HasData("IS_PROCESSINGFAKEID") == true)
				{
					Notification.SendPlayerNotifcation(p, "Warte bis du dir eine 2te FakeID erstellst", 4500, "red", "FÄLSCHEREI", "");

				}
				else
				{

					p.TriggerEvent("disableAllPlayerActions", new object[1]
					{
					true
					});
					p.TriggerEvent("sendProgressbar", new object[1]
					{
					500000
					});
					p.SetSharedData("IS_PROCESSINGFAKEID", true);
					NAPI.Player.PlayPlayerAnimation(p, 33, "mini@repair", "fixing_a_player", 8f);
					NAPI.Task.Run(delegate
					{
						Database.changeInventoryItem(p.Name, "FakeID", 1, false);
						Database.changeMoney(p.Name, 300000, false);
						NAPI.Player.StopPlayerAnimation(p);
						p.TriggerEvent("disableAllPlayerActions", new object[1]
						{
						false
						});
						Notification.SendPlayerNotifcation(p, "Du hast für 300.000$ eine FakeID erstellt", 4500, "green", "FÄLSCHEREI", "");

					}, 500000);
				}

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}
	}
}
*/