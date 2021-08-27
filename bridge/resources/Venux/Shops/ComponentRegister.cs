using GTANetworkAPI;
using System;

namespace Venux.Shops
{
    class ComponentRegister : Script
    {

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            NAPI.Marker.CreateMarker(1, new Vector3(-1065.907, -2798.816, 26.60876), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
            ColShape val2 = NAPI.ColShape.CreateCylinderColShape(new Vector3(-1065.907, -2798.816, 26.60876), 1.4f, 1.4f, uint.MaxValue);
            val2.SetData("COLSHAPE_FUNCTION", new FunctionModel("enterFlughafen"));
            val2.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um Station zu verlassen", "FLUGHAFEN", "white", 5000));
        }

        [RemoteEvent("enterFlughafen")]
        public void enterFlughafen(Client p)
        {
            try
            {

                {
                    Anticheat.Wait(p); p.Position = new Vector3(-1042.694, -2745.876, 21.25928);
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}