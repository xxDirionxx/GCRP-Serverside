using GTANetworkAPI;

namespace Venux.Items
{
    class Compactrifle : Item
    {
        public Compactrifle()
        {
            Id = 8;
            Name = "Compactrifle";
            ImagePath = "CompactRifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
