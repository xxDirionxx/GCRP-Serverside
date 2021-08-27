using GTANetworkAPI;
using System;

namespace Venux.Events
{
    class AnimationHandler : Script
    {

        [RemoteEvent("REQUEST_ANIMATION_USE")]
        public void REQUEST_ANIMATION_USE(Client p, int slot)
        {
            if (Database.isPlayerDeath(p.Name)) { return; }

            try
            {
                {
                    if (slot == 0)
                        p.StopAnimation();
                    if (slot == 1)
                        Notification.SendPlayerNotifcation(p, "Diese Funktion ist noch in Bearbeitung!", 5000, "red", "", "");
                    if (slot == 2)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "anim@amb@nightclub@peds@", "rcmme_amanda1_stand_loop_cop");
                    if (slot == 3)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "rcmextreme3", "idle");
                    if (slot == 4)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "random@arrests@busted", "idle_a");
                    if (slot == 5)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@world_human_stand_mobile@male@text@base", "base");
                    if (slot == 6)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "rcmpaparazzo_2", "shag_loop_a");
                    if (slot == 7)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@code_human_in_car_mp_actions@dance@std@ds@base", "idle_a");
                    if (slot == 8)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@code_human_cower@female@base", "base");
                    if (slot == 9)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@world_human_bum_slumped@male@laying_on_left_side@idle_a", "idle_b");
                    if (slot == 10)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@world_human_drinking@beer@male@idle_a", "idle_a");
                    if (slot == 11)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@world_human_hammering@male@base", "base");
                    if (slot == 12)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@world_human_push_ups@male@base", "base");
                    if (slot == 13)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "anim@mp_player_intincarsalutestd@ds@", "idle_a");
                    if (slot == 14)
                        NAPI.Player.PlayPlayerAnimation(p, 33, "amb@world_human_maid_clean@idle_a", "idle_a");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
