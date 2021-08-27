using GTANetworkAPI;

namespace Venux.Items
{
    class Advancedrifle : Item
    {
        public Advancedrifle()
        {
            Id = 9;
            Name = "Assaultrifle";
            ImagePath = "AdvanvcedRifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
