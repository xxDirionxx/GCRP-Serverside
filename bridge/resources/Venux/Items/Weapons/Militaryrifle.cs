using GTANetworkAPI;

namespace Venux.Items
{
    class Militaryrifle : Item
    {
        public Militaryrifle()
        {
            Id = 28;
            Name = "Militaryrifle";
            ImagePath = "Militaryrifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}