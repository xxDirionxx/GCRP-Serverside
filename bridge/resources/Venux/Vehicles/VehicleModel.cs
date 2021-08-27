namespace Venux.Vehicles
{
    public class VehicleModel
    {

        public string owner { get; set; }

        public string name { get; set; }

        public string modelname { get; set; }

        public string plate { get; set; } = "";

        public VehicleModel(string owner, string name, string plate)
        {
            this.owner = owner;
            this.name = name;
            this.plate = plate;
        }
    }
}
