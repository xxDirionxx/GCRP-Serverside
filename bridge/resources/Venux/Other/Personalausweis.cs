using GTANetworkAPI;

namespace Venux
{
    class Personalausweis : Script
    {
        public static void showPerso(Client p, string firstname, string lastname, string address, int level, int id, int casino, int govLevel)
        {
            p.TriggerEvent("showPerso", firstname, lastname, address, level, id, casino, govLevel);
        }
    }
}
