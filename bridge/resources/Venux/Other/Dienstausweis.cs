using GTANetworkAPI;

namespace Venux
{
    class Dienstausweis : Script
    {
        public static void showDienstausweis(Client p, string behoerde, int dienstnummer, int casino, string fistname, string lastname, string cduty, int govLevel)
        {
            p.TriggerEvent("showDienstausweis", behoerde, dienstnummer, casino, fistname, lastname, cduty, govLevel);
        }
    }
}
