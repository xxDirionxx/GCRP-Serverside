using GTANetworkAPI;

namespace Venux.Items
{
    class Eisen : Item
    {

        public Eisen()
        {
            Id = 12;
            Name = "Eisen";
            ImagePath = "Eisen.png";
            WeightInG = 10;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }
    }
}
