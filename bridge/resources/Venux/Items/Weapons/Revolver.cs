using GTANetworkAPI;

namespace Venux.Items
{
    class Revolver : Item
    {
        public Revolver()
        {
            Id = 38;
            Name = "Revolver";
            ImagePath = "Revolver.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
