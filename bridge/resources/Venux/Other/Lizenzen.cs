using GTANetworkAPI;

namespace Venux.Other
{
    public class Lizenzen : Script
    {
        public static void showLicense(Client p, string name, string firstaid, string gunlicense, string driverlicense, string trucklicense, string motorcyclelicense, string boatlicense, string flyinglicensea, string flyinglicenseb, string taxilicense, string passengertransportlicense, string lawyerlicense, string registryofficelicense)
        {
            p.TriggerEvent("showLicense", name, firstaid, gunlicense, driverlicense, trucklicense, motorcyclelicense, boatlicense, flyinglicensea, flyinglicenseb, taxilicense, passengertransportlicense, lawyerlicense, registryofficelicense);
        }


    }
}
