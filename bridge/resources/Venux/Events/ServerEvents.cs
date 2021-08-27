using GTANetworkAPI;
using System;
using System.Collections.Generic;

namespace Venux
{
    class ServerEvents : Script
    {
        [RemoteEvent("Pressed_E")]
        public void PressedE(Client p)
        {
            try
            {

                ColShape val = NAPI.Pools.GetAllColShapes().Find((ColShape col) => col.IsPointWithin(p.Position));
                if (!(val != null) || (val.Dimension != uint.MaxValue) && (p.Dimension != val.Dimension)) { return; }

                if (val.HasData("IS_FLAG"))
                    return;

                if (val.HasData("COLSHAPE_IS_GANGWARZONE"))
                    return;

                FunctionModel functionModel = val.GetData("COLSHAPE_FUNCTION");
                if (functionModel != null)
                {
                    if (functionModel.arg1 != null && functionModel.arg2 != null)
                    {
                        p.Eval("mp.events.callRemote('" + functionModel.functionName + "', '" + functionModel.arg1 + "', '" + functionModel.arg2 + "');");
                    }
                    else if (functionModel.arg2 == null && functionModel.arg1 != null)
                    {
                        p.Eval("mp.events.callRemote('" + functionModel.functionName + "', '" + functionModel.arg1 + "');");
                    }
                    else
                    {
                        p.Eval("mp.events.callRemote('" + functionModel.functionName + "');");
                    }

                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [ServerEvent(Event.PlayerWeaponSwitch)]
        public static void weaponSwitch(Client player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            NAPI.Player.SetPlayerCurrentWeapon(player, newWeapon);
        }

        [ServerEvent(Event.PlayerEnterColshape)]
        public void onEnterColshape(ColShape colShape, Client player)
        {
            try
            {
                if (colShape.HasData("IS_FLAG")) { return; }

                if (colShape.HasData("COLSHAPE_MESSAGE"))
                {
                    Notification.Message message = colShape.GetData("COLSHAPE_MESSAGE");
                    Notification.SendPlayerNotifcation(player, message.message, message.duration, message.color, message.title, "");
                }
                if (colShape.HasData("IS_FLAG"))
                {
                    if (Gangwar.GebietRegister.currentRunningGangwarGebiet == null) { return; }

                    if (!Gangwar.GebietRegister.InFlag.Contains(player))
                    {
                        Gangwar.GebietRegister.InFlag.Add(player);
                    }
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("XCM")]
        public void XCM(Client p, string cheatcode)
        {
            if (cheatcode == null) { return; }

            try
            {
                Log.Write(p.Name + " - " + cheatcode);
                Functions.XCM(p);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        // ----> VOICE <---- // 

        public static List<int> voiceRanges = new List<int>()
        {
           5,
           15,
           40
        };

        [RemoteEvent("Server:Voice:SwitchRange")]
        public void changeVoiceRange(Client p)
        {
            try
            {
                int nextRange = 0;
                int index = voiceRanges.IndexOf(p.GetSharedData("voiceRange"));
                if (index == -1 || index == voiceRanges.Count - 1)
                {
                    nextRange = voiceRanges[0];
                }
                else
                {
                    nextRange = voiceRanges[index + 1];
                }
                p.SetSharedData("voiceRange", nextRange);
                p.TriggerEvent("setVoiceType", (index + 1).ToString());
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
