using GTANetworkAPI;
using System.Collections.Generic;

namespace Venux.Handy
{
    class GpsApp : Script
    {
        [RemoteEvent("requestGpsLocations")]
        public void requestGpsLocations(Client client)
        {
            List<GPSCategorie> cat = new List<GPSCategorie> { };

            List<GPSPosition> positionen = new List<GPSPosition> {
                new GPSPosition("Meth Sammler", new Vector3(-500.5, 2977.22, 25.48)),
                new GPSPosition("Meth Verarbeiter", new Vector3(163.16, 2285.88, 92.95)),
                new GPSPosition("Dealer", new Vector3(-676.69, -884.92, 23.35)),
            };

            List<GPSPosition> garagen = new List<GPSPosition> {
                new GPSPosition("Vespucci Garage", new Vector3(-1184.2845, -1509.452, 3.5480242)),
                new GPSPosition("Pillbox Garage", new Vector3(-313.81174, -1034.3071, 29.430506)),
                new GPSPosition("Meeting Point Garage", new Vector3(100.46749, -1073.2855, 28.274118)),
                new GPSPosition("Vinewood Garage", new Vector3(638.3967, 206.5143, 96.5042)),
                new GPSPosition("Universität Garage", new Vector3(638.3967, 206.5143, 96.5042)),
                new GPSPosition("Hafen Garage", new Vector3(-331.3111, -2779.0078, 4.0451927)),
                new GPSPosition("Flughafen Garage", new Vector3(-984.3403, -2640.988, 12.852915)),
                new GPSPosition("Rockford Garage", new Vector3(-728.04517, -63.06201, 40.653107)),
                new GPSPosition("Mirrorpark Garage", new Vector3(1036.261, -763.1047, 56.892986)),
            };

            List<GPSPosition> shop = new List<GPSPosition> {
                new GPSPosition("Vespucci Shop", new Vector3(-707.8701, -913.9265, 18.115591)),
                new GPSPosition("Ammunation Shop", new Vector3(21.150259, -1107.3888, 28.697025)),
                new GPSPosition("Davis Shop", new Vector3(-48.4442, -1756.734, 28.42101)),
                new GPSPosition("24/7 Shop", new Vector3(25.7567, -1346.8448, 28.397045)),
                new GPSPosition("Vinewood Shop", new Vector3(374.30573, 326.5396, 102.46638)),
            };

            List<GPSPosition> labor = new List<GPSPosition> {
                new GPSPosition("MG13 Labor", new Vector3(1269.53, -1906.467, 38.32902)),
                new GPSPosition("Vagos Labor", new Vector3(-1201.977, -1800.218, 2.808579)),
                new GPSPosition("YakuZa Labor", new Vector3(-1507.397, 1505.021, 114.1888)),
                new GPSPosition("Grove Labor", new Vector3(1134.966, -789.9483, 56.49967)),
                new GPSPosition("Triaden Labor", new Vector3(845.4746, 2123.173, 51.54103)),
                new GPSPosition("LostMC Labor", new Vector3(2200.995, 3318.217, 45.6707)),
                new GPSPosition("LCN Labor", new Vector3(-1724.69, 234.7309, 57.37173)),
            };

            cat.Add(new GPSCategorie("Farming-Routen", positionen));
            cat.Add(new GPSCategorie("Garagen", garagen));
            cat.Add(new GPSCategorie("Shops", shop));
            cat.Add(new GPSCategorie("Labor", labor));

            client.TriggerEvent("componentServerEvent", "GpsApp", "gpsLocationsResponse", NAPI.Util.ToJson(cat));
        }

        [RemoteEvent("requestVehicleGps")]
        public void requestVehicleGps(Client client)
        {
            List<GPSCategorie> cat = new List<GPSCategorie> { };

            List<GPSPosition> vehicles = new List<GPSPosition> { };

            foreach (Vehicles.VehicleModel vm in Database.getUserVehicles(client.Name))
            {
                if (NAPI.Pools.GetAllVehicles().Find(x => x.NumberPlate == vm.plate) != null)
                {
                    vehicles.Add(new GPSPosition(vm.modelname + " [" + vm.plate + "] " + vm.modelname + "", NAPI.Pools.GetAllVehicles().Find(x => x.NumberPlate == vm.plate).Position));
                }
            }

            cat.Add(new GPSCategorie("Fahrzeuge", vehicles));

            client.TriggerEvent("componentServerEvent", "GpsApp", "gpsLocationsResponse", NAPI.Util.ToJson(cat));
        }

        public class GPSCategorie
        {
            public string name { get; set; }
            public List<GPSPosition> locations { get; set; }
            public GPSCategorie(string n, List<GPSPosition> pos)
            {
                name = n;
                locations = pos;
            }
        }

        public class GPSPosition
        {
            public string name { get; set; }
            public float x { get; set; }
            public float y { get; set; }

            public GPSPosition(string n, Vector3 pos)
            {
                name = n;
                x = pos.X;
                y = pos.Y;
            }
        }
    }
}
