using GTANetworkAPI;

namespace Venux.Items
{
    class Marksmanrifle : Item
    {
        public Marksmanrifle()
        {
            Id = 18;
            Name = "Marksmanrifle";
            ImagePath = "MarksmanRifle.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
