using GTANetworkAPI;
using System.Collections.Generic;

namespace Venux.Players
{
    class AdminRanks : Script
    {

        public static List<AdminRank> adminRankList = new List<AdminRank>();

        public static void RegisterAdmins()
        {
            if (adminRankList.Count < 2)
            {
                adminRankList.Add(new AdminRank(100, "Inhaber", 2, new Color(255, 0, 0)));
                adminRankList.Add(new AdminRank(99, "Morgen", 2, new Color(255, 0, 0)));
                adminRankList.Add(new AdminRank(98, "Manager", 11, new Color(227, 121, 0)));
                adminRankList.Add(new AdminRank(97, "Entwickler", 13, new Color(189, 74, 255)));
                adminRankList.Add(new AdminRank(97, "Superadministrator", 12, new Color(91, 32, 118)));
                adminRankList.Add(new AdminRank(96, "Administration", 3, new Color(228, 180, 0)));
                adminRankList.Add(new AdminRank(94, "Moderator", 4, new Color(0, 45, 207)));
                adminRankList.Add(new AdminRank(93, "Supporter", 5, new Color(0, 154, 51)));
                adminRankList.Add(new AdminRank(92, "Test-Supporter", 5, new Color(193, 162, 208)));
                adminRankList.Add(new AdminRank(1, "Frakmedic", 0, new Color(0, 0, 0)));
            }
            Log.Write("Ränge geladen.");
        }

        public static AdminRank getRankFromName(string name)
        {
            foreach (AdminRank adminRank in adminRankList)
            {
                if (adminRank.rankName == name)
                    return adminRank;
            }
            return new AdminRank(0, "User", 0, new Color(0, 0, 0));
        }


    }
}
