using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.AdminSystem;

namespace Venux
{
    class PlayerEvents : Script
    {
        public static List<Animation> Animations = new List<Animation>
        {
            new Animation("Stoppen", "Abbrechen.png", 0),
            new Animation("Animation Bearbeiten", "Bearbeiten.png", 1),
            new Animation("Arme verschränken", "Animation.png", 2),
            new Animation("Kniehen", "Animation.png", 3),
            new Animation("Ergeben", "Animation.png", 4),
            new Animation("Phone", "Animation.png", 5),
            new Animation("Doggy", "Animation.png", 6),
            new Animation("Tanzen", "Animation.png", 7),
            new Animation("Angst", "Animation.png", 8),
            new Animation("Liegen", "Animation.png", 9),
            new Animation("Trinken", "Animation.png", 10),
            new Animation("Hämmern", "Animation.png", 11),
            new Animation("Liegestützen", "Animation.png", 12),
            new Animation("Salutieren", "Animation.png", 13),
            new Animation("Putzen", "Animation.png", 14),
        };

        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(Client p)
        {
            try
            {
                p.SetSharedData("istEingeloggt", 8);
                p.SetSharedData("PLAYER_RANK", "User");
                p.SetSharedData("voiceRange", 15);
                Anticheat.Wait(p); p.Dimension = (uint)new Random().Next(1000, 9999);
                Anticheat.Wait(p); p.Position = new Vector3(-428.7609, 1111.689, 327.5877);
                p.Rotation = new Vector3(0.0f, 0.0f, 343f);
                Clothing.PlayerClothes.setCaillou(p);
                if (Start.loggedInPlayers.ContainsKey(p))
                {
                    Start.loggedInPlayers.Remove(p);
                }

                Functions.disableAllPlayerControls(p, true);

                if (!Database.isAccountExists(p.SocialClubName))
                    Database.createAccount(p);

                p.SetData("IS_FARMING", false);

                Database.updateAddress(p);
                Anticheat.Wait(p);

                if (Database.isIdentifierBanned("social", p.SocialClubName) || Database.isIdentifierBanned("hwid", p.Serial) || Database.isIdentifierBanned("ip", p.Address))
                {
                    p.SendNotification("Du hast keine Rechte zu joinen. Grund: " + Database.getBanReason(p.SocialClubName) + " Melde dich im Support.");
                    p.Kick();
                }

                p.TriggerEvent("guiReady");
                p.TriggerEvent("OnPlayerReady");
                p.TriggerEvent("client:respawning");
                p.TriggerEvent("openWindow", "TextInputBox",
                "{\"textBoxObject\":{\"Title\":\"Anmeldeformular\",\"Message\":\"Gebe bitte deinen Benutzernamen ein. Falls du noch nicht registriert bist, wirst du automatisch registriert! Im nächsten Schritt kannst du dir dein Passwort auswählen.\",\"Callback\":\"registerUser\",\"CloseCallback\":\"cancelLoginKick\"}}"
                );
                p.TriggerEvent("componentReady", "TextInputBox");
            }
            catch (Exception ex) { Log.Write(ex.Message); }

        }


        [RemoteEvent("registerUser")]
        public static void registerUser(Client c, string username)
        {
            int count = 0;
            foreach (char s in username)
                if (s == '_') count++;

            if (count != 1)
            {
                c.SendNotification("Der Name darf nur aus einem Vor- und Nachnamen bestehen. Beispiel: Vorname_Nachname!");
                c.TriggerEvent("openWindow", new object[2]
                {
                    "TextInputBox",
                    "{\"textBoxObject\":{\"Title\":\"Anmeldeformular\",\"Message\":\"Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert! Im nächsten Schritt kannst du dir dein Passwort auswählen.\",\"Callback\":\"registerUser\",\"CloseCallback\":\"cancelLoginKick\"}}"
                });
                return;
            }

            string vorname = username.Split("_")[0];
            string nachname = username.Split("_")[1];

            if (vorname.Length < 3 || nachname.Length < 3)
            {
                c.SendNotification("Dein Vor-/Nachname muss mindestens 3 Buchstaben enthalten");
                c.TriggerEvent("openWindow", new object[2]
                {
                    "TextInputBox",
                    "{\"textBoxObject\":{\"Title\":\"Anmeldeformular\",\"Message\":\"Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert! Im nächsten Schritt kannst du dir dein Passwort auswählen.\",\"Callback\":\"registerUser\",\"CloseCallback\":\"cancelLoginKick\"}}"
                });
                return;
            }

            string specialChar = @"^\|!#$%&/()=?»«@£§€{}.;:[]'<>_,1234567890 ";
            foreach (var item in specialChar)
            {
                if (vorname.Contains(item) || nachname.Contains(item))
                {
                    c.SendNotification("Dein Charakter darf keine Sonderzeichen enthalten!");
                    c.TriggerEvent("openWindow", new object[2]
                    {
                    "TextInputBox",
                    "{\"textBoxObject\":{\"Title\":\"Anmeldeformular\",\"Message\":\"Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert! Im nächsten Schritt kannst du dir dein Passwort auswählen.\",\"Callback\":\"registerUser\",\"CloseCallback\":\"cancelLoginKick\"}}"
                    });
                    return;
                }
            }

            if (vorname == nachname)
            {
                c.SendNotification("Dein Vorname darf nicht wie dein Nachname klingen!");
                c.TriggerEvent("openWindow", new object[2]
                {
                    "TextInputBox",
                    "{\"textBoxObject\":{\"Title\":\"Anmeldeformular\",\"Message\":\"Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert! Im nächsten Schritt kannst du dir dein Passwort auswählen.\",\"Callback\":\"registerUser\",\"CloseCallback\":\"cancelLoginKick\"}}"
                });
                return;
            }
            else
            {
                c.TriggerEvent("openWindow", new object[2]
                {
                    "Login",
                    "{\"name\":\"" + username + "\"}"
                });

                username = vorname + "_" + nachname;

                c.Name = username;
            }
        }

        [RemoteEvent("cancelLoginKick")]
        public void cancelLoginKick(Client p)
        {
            p.SendNotification("Bro, Warum willst du versuchen dir Admin zu geben?");
            p.SendNotification("Kleiner Hund.");
            p.Kick();
        }

        [RemoteEvent("showLoginInput")]
        public void showLoginInput(Client p)
        {
            p.TriggerEvent("openWindow", new object[2]
                {
                    "TextInputBox",
                    "{\"textBoxObject\":{\"Title\":\"Anmeldeformular\",\"Message\":\"Gebe bitte deinen Benutzernamen ein (Beispiel: Vorname_Nachname). Falls du noch nicht registriert bist, wirst du automatisch registriert! Im nächsten Schritt kannst du dir dein Passwort auswählen.\",\"Callback\":\"registerUser\",\"CloseCallback\":\"cancelLoginKick\"}}"
                });
            p.TriggerEvent("componentReady", new object[1]
            {
                    "TextInputBox"
            });
        }

        [RemoteEvent("stopAnimation")]
        public void stopAnimation(Client p)
        {
            try { NAPI.Player.StopPlayerAnimation(p); }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("playFunkAnimation")]
        public void playFunkAnimation(Client p)
        {
            if (Database.isPlayerDeath(p.Name)) { return; }

            try
            {
                NAPI.Player.PlayPlayerAnimation(p, 49, "random@arrests", "generic_radio_chatter", 8f);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("PlayerLogin")]
        public void Login(Client p, string password)
        {
            if (password == null) { return; }

            try
            {
                Anticheat.Wait(p);

                string result = Database.loginUser(p.Name, password, p);

                string name = p.Name;

                if (result.Contains("eingeloggt"))
                {
                    p.SetSharedData("PLAYER_RANK", Database.getPlayerRank2(name));
                    p.TriggerEvent("LoadIpls");

                    if (!Start.loggedInPlayers.ContainsKey(p))
                        Start.loggedInPlayers.Add(p, name);

                    Functions.disableAllPlayerControls(p, false);

                    p.Dimension = 0;
                    p.SetSharedData("aduty", false);

                    p.Name = name;

                    p.SetSharedData("istEingeloggt", 10);

                    p.TriggerEvent("ConnectTeamspeak", false);
                    NAPI.Task.Run(() =>
                    {
                        p.TriggerEvent("ConnectTeamspeak", true);
                    }, 5000);
                    p.TriggerEvent("setVoiceData", 1, "Ingame", "VenuxCL");



                    if (Database.isMaintenance())
                    {
                        if (!Database.isPlayerTeam(p.Name))
                        {
                            p.Kick("Crimelife befindet sich gerade in Wartungsarbeiten, versuche es später erneut!");
                        }
                    }

                    Anticheat.Wait(p); p.Position = Database.getPlayerPosition(name);

                    NAPI.Task.Run(() =>
                    {
                        Anticheat.Wait(p);

                        Database.changeMoney(p.Name, 0, false);
                        Database.changeBlackMoney(p.Name, 0, false);
                        p.TriggerEvent("setNMenuItems", NAPI.Util.ToJson(Animations));
                        Database.restoreCustomization(p);
                    }, delayTime: 1000);


                    NAPI.Task.Run(() =>
                    {
                        Anticheat.Wait(p);

                        if (Database.isPlayerDeath(name))
                            p.Health = 0;

                        Anticheat.Wait(p);
                    }, delayTime: 4000);

                    p.SetSharedData("PLAYER_NAME", p.Name);
                    p.SetSharedData("FRAKTION", Database.getPlayerFraktion(name));
                    p.SetSharedData("FRAKTION_SHORT", Functions.getFraktionFromName(p.GetSharedData("FRAKTION")).shortName);
                    p.SetSharedData("FRAKTION_RANK", Database.getUserFraktionRank(name));
                    NAPI.Task.Run(() =>
                    {
                        Anticheat.Wait(p);

                        Items.Weapons.WeaponSync(p);
                    }, 3000);

                    int death = 0;

                    if (Database.isPlayerDeath(p.Name))
                    {
                        death = 1;
                    }

                    p.TriggerEvent("onPlayerLoaded", new object[30]
                                {
                                p.Name.Split("_")[0],
                                p.Name.Split("_")[1],
                                Database.getSQLId(p.Name),
                                0,
                                //Database.getPlayerBusiness(p.Name).ToString(),
                                0,
                                Database.getMoney(p.Name),
                                Database.getBlackMoney(p.Name),
                                Database.getWarns(p.SocialClubName),
                                0,
                                Database.getPlayerFraktion(p.Name),
                                0,
                                Database.getPlaytime(p.SocialClubName),
                                death,
                                false,
                                false,
                                false,
                                false,
                                false,
                                1,
                                1,
                                0,
                                "{ }",
                                Database.getPlayerRank(p.Name).rankPermission,
                                1,
                                1,
                                0,
                                0,
                                0,
                                "{ }",
                                Database.getFraktionByName(p.GetSharedData("FRAKTION")).fraktionsDimension
                            });

                    p.TriggerEvent("componentServerEvent", "Login", "status", "successfully");
                }
                else
                {
                    p.TriggerEvent("componentServerEvent", "Login", "status", result);
                }

            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        private static HeadOverlay CreateHeadOverlay(Byte index, Byte color, Byte secondaryColor, float opacity)
        {
            return new HeadOverlay
            {
                Index = index,
                Color = color,
                SecondaryColor = secondaryColor,
                Opacity = opacity
            };
        }

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(Client p, Client killer, uint reason)
        {
            try
            {

                if (killer.Exists && killer != null && killer != p && Other.Paintball.StabCityPlayers.Contains(killer))
                {
                    killer.Health = 200;
                    killer.Armor = 100;
                    killer.SetData(Other.Paintball.PAINTBALL_KILLS, killer.GetData(Other.Paintball.PAINTBALL_KILLS) + 1);
                    if (killer.GetData(Other.Paintball.PAINTBALL_DEATHS) == 0 || killer.GetData(Other.Paintball.PAINTBALL_KILLS) == 0)
                    {
                        killer.TriggerEvent("updatePaintballScore", killer.GetData(Other.Paintball.PAINTBALL_KILLS), killer.GetData(Other.Paintball.PAINTBALL_DEATHS), 0);
                    }
                    else
                    {
                        killer.TriggerEvent("updatePaintballScore", killer.GetData(Other.Paintball.PAINTBALL_KILLS), killer.GetData(Other.Paintball.PAINTBALL_DEATHS), (int)(killer.GetData(Other.Paintball.PAINTBALL_KILLS) / (int)(killer.GetData(Other.Paintball.PAINTBALL_DEATHS))));
                    }

                    Notification.SendPlayerNotifcation(p, "Du wurdest von " + killer.Name + " getötet.", 7000, "red", "", "white");
                    Notification.SendPlayerNotifcation(killer, "Du hast " + p.Name + " getötet. + 750$", 7000, "orange", "", "white");
                    Database.changeMoney(Start.loggedInPlayers[killer], 750, false);
                    DiscordWebhook.SendMessage("Spieler hat anderen Spieler getötet.", "Der Spieler " + killer.Name + " hat den Spieler " + p.Name + " getötet. Grund-ID: " + reason, DiscordWebhook.killWebhook, "Kill-Log");
                }

                if (Other.Paintball.StabCityPlayers.Contains(p))
                {
                    p.SetData(Other.Paintball.PAINTBALL_DEATHS, p.GetData(Other.Paintball.PAINTBALL_DEATHS) + 1);
                    if (p.GetData(Other.Paintball.PAINTBALL_DEATHS) == 0 || p.GetData(Other.Paintball.PAINTBALL_KILLS) == 0)
                    {
                        p.TriggerEvent("updatePaintballScore", p.GetData(Other.Paintball.PAINTBALL_KILLS), p.GetData(Other.Paintball.PAINTBALL_DEATHS), 0);
                    }
                    else
                    {
                        p.TriggerEvent("updatePaintballScore", p.GetData(Other.Paintball.PAINTBALL_KILLS), p.GetData(Other.Paintball.PAINTBALL_DEATHS), (int)(p.GetData(Other.Paintball.PAINTBALL_KILLS) / (int)(p.GetData(Other.Paintball.PAINTBALL_DEATHS))));
                    }

                    NAPI.Task.Run(() =>
                    {
                        p.TriggerEvent("stopScreenEffect", "DeathFailOut");
                        if (p.GetData(Other.Paintball.PAINTBALL_DEATHS) >= 10)
                        {
                            Anticheat.Wait(p);
                            NAPI.Player.SpawnPlayer(p, p.Position);
                            p.TriggerEvent("finishPaintball");
                            Other.Paintball.StabCityPlayers.Remove(p);
                            p.ResetData(Other.Paintball.PAINTBALL_DEATHS);
                            p.ResetData(Other.Paintball.PAINTBALL_KILLS);
                            Anticheat.Wait(p); p.Position = Other.Paintball.paintballs["StabCity"];
                            Notification.SendPlayerNotifcation(p, "Du wurdest aus dem FFA aufgrund von zuvielen Toden gekickt!", 5000, "red", "", "white");
                            p.Dimension = 0;
                            p.RemoveAllWeapons();
                            Items.Weapons.WeaponSync(p);
                        }
                        else
                        {
                            NAPI.Player.SpawnPlayer(p, Other.Paintball.getRandomSpawnpoint(Other.Paintball.StabCityPoints));
                            p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_ADVANCEDRIFLE"), 9999);
                            p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_GUSENBERG"), 9999);
                            p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_HEAVYPISTOL"), 9999);
                            p.GiveWeapon((WeaponHash)NAPI.Util.GetHashKey("WEAPON_BULLPUPRIFLE"), 9999);
                            Anticheat.Wait(p); p.Armor = 100;
                        }

                    }, delayTime: 2000);

                }
                else
                {

                    if (!Start.deathTime.ContainsKey(p))
                    {

                        if (killer.Exists && killer != null && killer != p)
                        {
                            if (p.GetSharedData("FRAKTION") != killer.GetSharedData("FRAKTION"))
                            {
                                if (Gangwar.GebietRegister.currentRunningGangwarGebiet != null && p.Dimension == Fraktionen.FraktionRegister.GangwarDimension)
                                {
                                    if (Gangwar.GebietRegister.fraktionOne == Database.getFraktionByName(killer.GetSharedData("FRAKTION")))
                                    {
                                        Gangwar.GebietRegister.pointsFraktionOne = Gangwar.GebietRegister.pointsFraktionOne + 3;
                                        foreach (Client target in NAPI.Pools.GetAllPlayers())
                                        {
                                            if (target.HasSharedData("FRAKTION"))
                                            {
                                                if (target.GetSharedData("FRAKTION") == killer.GetSharedData("FRAKTION"))
                                                {
                                                    Notification.SendPlayerNotifcation(target, "+3 Punkte für das töten eines Gegners (" + killer.Name + " hat " + p.Name + " getötet).", 5000, "lightblue", killer.GetSharedData("FRAKTION"), "orange");
                                                }
                                            }
                                        }
                                    }
                                    else if (Gangwar.GebietRegister.fraktionTwo == Database.getFraktionByName(killer.GetSharedData("FRAKTION")))
                                    {
                                        Gangwar.GebietRegister.pointsFraktionTwo = Gangwar.GebietRegister.pointsFraktionTwo + 3;
                                        foreach (Client target in NAPI.Pools.GetAllPlayers())
                                        {
                                            if (target.HasSharedData("FRAKTION"))
                                            {
                                                if (target.GetSharedData("FRAKTION") == killer.GetSharedData("FRAKTION"))
                                                {
                                                    Notification.SendPlayerNotifcation(target, "+3 Punkte für das töten eines Gegners (" + killer.Name + " hat " + p.Name + " getötet).", 5000, "lightblue", killer.GetSharedData("FRAKTION"), "");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            Notification.SendPlayerNotifcation(p, "Du wurdest von " + killer.Name + " getötet.", 5000, "red", "", "red");
                            Notification.SendPlayerNotifcation(killer, "Du hast " + p.Name + " getötet. +750$", 5000, "orange", "", "white");
                            Database.changeMoney(Start.loggedInPlayers[killer], 750, false);
                            DiscordWebhook.SendMessage("Spieler hat anderen Spieler getötet.", "Der Spieler " + killer.Name + " hat den Spieler " + p.Name + " getötet. Grund-ID: " + reason, DiscordWebhook.killWebhook, "Kill-Log");
                        }
                        else
                        {
                            Notification.SendPlayerNotifcation(p, "Du bist gestorben.", 5000, "red", "", "white");
                            //Discord.DiscordWebhooks.SendMessage("Spieler ist gestorben.", "Der Spieler " + p.Name + " ist gestorben. Grund-ID: " + reason, Discord.DiscordWebhooks.killWebhook, "Kill-Log");
                        }
                    }

                    try
                    {
                        if (Gangwar.GebietRegister.fraktionOne == Database.getFraktionByName(p.GetSharedData("FRAKTION")) || Gangwar.GebietRegister.fraktionTwo == Database.getFraktionByName(p.GetSharedData("FRAKTION")))
                        {
                            Start.deathTime.Add(p, (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds + 60);
                        }
                        else
                        {
                            Start.deathTime.Add(p, (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds + 120);
                        }
                    }
                    catch (Exception ex) { Log.Write(ex.Message); }
                    NAPI.Player.SpawnPlayer(p, p.Position);
                    p.RemoveAllWeapons();
                    p.StopAnimation();
                    p.Eval("mp.events.callRemote('server:leaveradio');");
                    Functions.setPlayerOnGround(p);
                    NAPI.Task.Run(() =>
                    {
                        p.TriggerEvent("startScreenEffect", new object[3]
                        {
                        "DeathFailOut",
                        480000,
                        false
                         });
                        Functions.disableAllPlayerControls(p, true);
                        NAPI.Player.PlayPlayerAnimation(p, 33, "combat@damage@rb_writhe", "rb_writhe_loop", 8f);
                        Database.setDeathStatus(p.Name, true);
                    }, delayTime: 300);


                }


            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            switch (type)
            {
                case DisconnectionType.Left:
                    NAPI.Chat.SendChatMessageToAll("");
                    break;

                case DisconnectionType.Timeout:
                    NAPI.Chat.SendChatMessageToAll("");
                    break;

                case DisconnectionType.Kicked:
                    NAPI.Chat.SendChatMessageToAll("");
                    break;
            }
        }

        [ServerEvent(Event.PlayerEnterVehicle)]
        public void PlayerEnterVehicle(Client p, Vehicle veh, sbyte seatId)
        {
            if (veh.Locked) { p.StopAnimation(); }
        }
    }
    public class Animation
    {
        public string name { get; set; }
        public string icon { get; set; }
        public int slot { get; set; }

        public Animation(string i, string n, int f)
        {
            name = i;
            icon = n;
            slot = f;
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
