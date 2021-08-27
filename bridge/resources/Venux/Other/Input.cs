using GTANetworkAPI;

namespace Venux
{
    public class Input : Script
    {
        public static void sendPlayerInput(Client p, string eventname, bool remote, string argument = "")
        {
            p.TriggerEvent("sendPlayerInput", eventname, remote, argument);
        }
    }
}
