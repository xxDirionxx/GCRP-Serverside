using GTANetworkAPI;
using System;

namespace Venux.Other
{
    class Blips : Script
    {
        public bool BlipSetted = false;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            if (BlipSetted == false)
            {
                BlipSetted = true;
                NAPI.Blip.CreateBlip(617, new Vector3(924.71, 47.01, 81.11), 1.0f, 0, "The Diamond Casino & Resort", 255, 0, true, 0, 0);
                NAPI.Blip.CreateBlip(490, new Vector3(-1051.723, -236.8069, 42.92113), 1.0f, 46, "LifeInvader", 255, 46, true, 0, 0);
                NAPI.Blip.CreateBlip(307, new Vector3(-1065.907, -2798.816, 26.60876), 1.0f, 4, "Flughafen", 255, 46, true, 0, 0);
                NAPI.Blip.CreateBlip(501, new Vector3(813.9174, -491.5391, 29.52957), 1.0f, 4, "Dealer", 255, 75, true, 0, 0);

                ColShape val = NAPI.ColShape.CreateCylinderColShape(new Vector3(356.1, -596.33, 27.77), 1.4f, 1.4f, 0);
                val.SetData("COLSHAPE_FUNCTION", new FunctionModel("openSchönheitsklinik"));
                val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um die Schönheitsklinik zu öffnen.", "SCHÖNHEITSKLINIK", "white", 5000));

            }
        }

        [RemoteEvent("openSchönheitsklinik")]
        public void openSchönheitsklinik(Client p)
        {
            try
            {
                p.TriggerEvent("openWindow", new object[] { "CharacterCreator", "{\"customization\":{\"Gender\":0,\"Parents\":{\"FatherShape\":0,\"MotherShape\":0,\"FatherSkin\":0,\"MotherSkin\":0,\"Similarity\":1,\"SkinSimilarity\":1},\"Features\":[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],\"Hair\":{\"Hair\":0,\"Color\":0,\"HighlightColor\":0},\"Appearance\":[{\"Value\":255,\"Opacity\":1},{\"Value\":255,\"Opacity\":1},{\"Value\":1,\"Opacity\":1},{\"Value\":5,\"Opacity\":0.4},{\"Value\":0,\"Opacity\":0},{\"Value\":0,\"Opacity\":0},{\"Value\":255,\"Opacity\":1},{\"Value\":255,\"Opacity\":1},{\"Value\":0,\"Opacity\":0},{\"Value\":255,\"Opacity\":1},{\"Value\":255,\"Opacity\":1}],\"EyebrowColor\":0,\"BeardColor\":0,\"EyeColor\":0,\"BlushColor\":0,\"LipstickColor\":0,\"ChestHairColor\":0},\"level\":0}" });
                p.Position = new Vector3(402.8664, -996.4108, -99.00027);
                p.TriggerEvent("client:respawning");
                p.Eval("mp.players.local.setHeading(-185);");
                p.Dimension = (uint)new Random().Next(10000, 99999);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
