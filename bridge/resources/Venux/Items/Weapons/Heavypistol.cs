using GTANetworkAPI;

namespace Venux.Items
{
    class Heavypistol : Item
    {
        public Heavypistol()
        {
            Id = 6;
            Name = "Heavypistol";
            ImagePath = "HeavyPistol.png";
            WeightInG = 0;
            MaxStackSize = 25;
        }

        public override bool getItemFunction(Client p)
        {
            return true;
        }

    }
}
