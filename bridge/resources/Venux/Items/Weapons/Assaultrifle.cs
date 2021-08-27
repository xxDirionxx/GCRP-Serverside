using GTANetworkAPI;

namespace Venux.Items
{
    class Assaultrifle : Item
    {
        public Assaultrifle()
        {
            Id = 5;
            Name = "Assaultrifle";
            ImagePath = "AssaultRifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
