/*using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Venux.Handy
{
	class MessengerApp : Script
	{

		[ServerEvent(Event.ResourceStart)]
		public void onResourceStart()
		{
			Log.Write("MessengerApp geladen");
		}

		[RemoteEvent("sendPhoneMessage")]
		public void sendPhoneMessage(Client p, string reciver, string message)
		{
			try
			{
				Notification.SendPlayerNotifcation(p, message, 6000, "white", "", "white");

			} catch(Exception ex)
			{
				Log.Write(ex.Message);
			}
		}

	}
}*/
