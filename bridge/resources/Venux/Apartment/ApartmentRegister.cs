using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Venux.Menus;

namespace Venux.Apartment
{
	public class ApartmentRegister : Script
	{

		public static Vector3 apartmentHigh = new Vector3(-785.2194, 323.6381, 210.8972);

		public static Vector3 apartmentMiddle = new Vector3(-787.029, 315.7113, 217.6385);

		public static Vector3 apartmentLow = new Vector3(-781.8146, 326.4167, 175.7036);

		public static Dictionary<string, Vector3> points = new Dictionary<string, Vector3>();

		public static Dictionary<string, Vector3> ausgang = new Dictionary<string, Vector3>();


		[ServerEvent(Event.ResourceStart)]
		public void onResourceStart()
		{
			if (points.Count < 1)
			{
				points.Add("pennerApartment1", new Vector3(291.4987, -1078.593, 28.30464));
				points.Add("pennerApartment2", new Vector3(278.7133, -1118.127, 28.31966));
				points.Add("pennerApartment3", new Vector3());
				points.Add("middleApartment1", new Vector3());
				points.Add("middleApartment2", new Vector3());
				points.Add("middleApartment3", new Vector3());
				points.Add("highApartment1", new Vector3());
				points.Add("highApartment2", new Vector3());
				points.Add("highApartment3", new Vector3());

				foreach (KeyValuePair<string, Vector3> point in points)
				{
					NAPI.Blip.CreateBlip(350, point.Value, 1f, 4, "Apartment", 255, 0, true, 0, uint.MaxValue);
					ColShape val = NAPI.ColShape.CreateCylinderColShape(point.Value, 1.5f, 2f, 0);
					val.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterApartment"));
					val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um dein Apartment zu betreten", "APARTMENT", "white", 3500));
				}
			}
			if (ausgang.Count < 1)
			{


				foreach (KeyValuePair<string, Vector3> exit in ausgang)
				{
					NAPI.Blip.CreateBlip(350, exit.Value, 1f, 4, "Apartment", 255, 0, true, 0, uint.MaxValue);
					ColShape val = NAPI.ColShape.CreateCylinderColShape(exit.Value, 1.5f, 2f, 0);
					val.SetData("COLSHAPE_FUNCTION", new FunctionModel("exitApartment"));
					val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um dein Apartment zu Verlassen", "APARTMENT", "white", 3500));
				}
			}
		}

		[RemoteEvent("enterApartment")]
		public void enterApartment(Client p)
		{
			if (Database.getItemCount(p.Name, "SchlüsselApartment") == 1)
			{
				Anticheat.Wait(p); p.Position = apartmentLow.Add(new Vector3(0, 0, 1.5));
				p.Dimension = (uint)new Random().Next(10, 99999);
				Notification.SendPlayerNotifcation(p, "Du bist in dein Apartment eingetreten", 4500, "white", "APARTMENT", "");

			}
			else
			{
				Notification.SendPlayerNotifcation(p, "Du hast dafür keinen Schlüssel", 4500, "red", "APARTMENT", "");
			}

		}

		[RemoteEvent("exitApartment")]
		public void exitApartment(Client p, string arg1)
		{
			if (arg1 == null)
				return;

			try
			{
				if (arg1 == "pennerApartment1")
				{
					p.Position = new Vector3(291.4987, -1078.593, 28.30464).Add(new Vector3(0, 0, 1.5));
					p.Dimension = 0;
					return;
				}

			}
			catch (Exception ex)
			{
				Log.Write(ex.Message);
			}
		}
	}
}
