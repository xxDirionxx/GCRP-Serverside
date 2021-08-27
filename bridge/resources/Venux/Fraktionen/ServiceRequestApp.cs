using GTANetworkAPI;
using System;

namespace Venux.Handy
{
    class ServiceRequestApp : Script
    {
        public static bool abgebrochen = false;

        [RemoteEvent("addServiceRequest")]
        public void addServiceRequest(Client p, string category, string content, bool phonenumber)
        {
            try
            {
                if (category == "mg13")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Marabunta Grande 13"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Marabunta Grande 13"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }

                }
                else if (category == "vagos")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Los Santos Vagos"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Los Santos Vagos"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "grove")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Grove Street"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Grove Street"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "lostmc")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "LostMC"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "LostMC"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "triaden")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Triaden"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Triaden"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "yakuza")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "YakuZa"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "YakuZa"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "orga")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Organisazija"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Organisazija"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "lcn")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "La Cosa Nostra"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "La Cosa Nostra"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }
                else if (category == "ballas")
                {
                    if (phonenumber == true)
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Ballas"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                    else
                    {
                        foreach (Client client in NAPI.Pools.GetAllPlayers())
                        {
                            if (p.HasData("IS_ANFRAGE") == true)
                            {
                                Notification.SendPlayerNotifcation(client, "Du hast bereits eine Anfrage gesendet!", 4500, "red", "SERVICE", "");
                                return;
                            }

                            if (Database.isPlayerInFrak(client, "Ballas"))
                            {
                                Random r = new Random();
                                Fraktionen.FraktionTablet.openService.Add(new Service.openService(p.Name, content, r.Next(1000, 9999)));
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(client, "Deine Fraktion hat eine neue Anfrage erhalten!", 4500, "red", "SERVICE", "");
                            }
                            else
                            {
                                p.SetData("IS_ANFRAGE", true);
                                Notification.SendPlayerNotifcation(p, "Du hast deine Anfrage erfolgreich verschickt", 4500, "red", "", "");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }

        [RemoteEvent("cancelServiceRequest")]
        public void cancelRequest(Client p)
        {
            try
            {
                if (p.HasData("IS_ANFRAGE"))
                    Notification.SendPlayerNotifcation(p, "Du hast die Anfrage abgebrochen!", 4500, "red", "", "");
                p.ResetData("IS_ANFRAGE");
                abgebrochen = true;
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}

// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
