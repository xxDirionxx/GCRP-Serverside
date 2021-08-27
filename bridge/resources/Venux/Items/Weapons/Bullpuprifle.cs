using GTANetworkAPI;

namespace Venux.Items
{
    class Bullpuprifle : Item
    {
        public Bullpuprifle()
        {
            Id = 10;
            Name = "Bullpuprifle";
            ImagePath = "BullpupRifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
