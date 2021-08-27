﻿using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.Clothing;
using Venux.Menus;

namespace Venux.ClothingShops
{
    class ClothingShopRegister : Script
    {
        public static List<ClothingShop> clothingshopList = new List<ClothingShop>();
        public static List<ClothingModel> clothingList = new List<ClothingModel>();

        [ServerEvent(Event.ResourceStart)]
        public void registerClothingShops()
        {
            clothingshopList.Add(new ClothingShop("Kleiderladen Davis", new Vector3(76.97766, -1391.165, 28.276144)));
            clothingshopList.Add(new ClothingShop("Kleiderladen PD", new Vector3(424.76266, -808.4952, 28.391127)));
            clothingshopList.Add(new ClothingShop("Kleiderladen Vinewood", new Vector3(123.13352, -220.96146, 53.45787)));
            clothingshopList.Add(new ClothingShop("Kleiderladen Vespucci", new Vector3(-820.81775, -1074.068, 10.228109)));
            clothingshopList.Add(new ClothingShop("Kleiderladen Flughafen", new Vector3(-1045.412, -2827.655, 26.59199)));
            clothingshopList.Add(new ClothingShop("Kleiderladen Casino", new Vector3(1101.569, 197.0486, -50.54008)));

            clothingList = Database.getClothingDataList();

            foreach (ClothingShop clothingShop in clothingshopList)
            {
                ColShape val = NAPI.ColShape.CreateCylinderColShape(clothingShop.position, 1.4f, 1.4f, 0);
                val.SetData("COLSHAPE_FUNCTION", new FunctionModel("openClothingShop", clothingShop.name));
                val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um den Kleidungsladen zu öffnen.", clothingShop.name, "blue", 5000));

                NAPI.Marker.CreateMarker(1, clothingShop.position, new Vector3(), new Vector3(), 1.0f, new Color((int)byte.MaxValue, 140, 0), false, 0);
                NAPI.Blip.CreateBlip(73, clothingShop.position, 1f, (byte)0, clothingShop.name, byte.MaxValue, 0.0f, true, (short)0, 0);
            }
        }

        [RemoteEvent("openClothingShop")]
        public static void openClothingShop(Client p, string name)
        {
            if (name == null)
                return;

            new NativeMenu("Kleiderladen", name.Replace("Kleiderladen ", ""), new List<NativeItem>()
            {
              new NativeItem("Maske", "Maske"),
              new NativeItem("Hüte", "Hüte"),
              new NativeItem("Oberteil", "Oberteil"),
              new NativeItem("Unterteil", "Unterteil"),
              new NativeItem("Koerper", "Koerper"),
              new NativeItem("Hose", "Hose"),
              new NativeItem("Schuhe", "Schuhe")
            }).showNativeMenu(p);
        }

        [RemoteEvent("nM-Kleiderladen")]
        public static void Kleiderladen(Client p, string selection)
        {
            if (selection == null)
                return;

            try
            {
                List<NativeItem> Items = new List<NativeItem>();

                if (selection == "Maske")
                {
                    List<NativeItem> nativeItemList = Items;
                    string name = "Keine Maske";
                    string[] strArray = new string[7];
                    strArray[0] = selection;
                    strArray[1] = "-";
                    int num = 1;
                    strArray[2] = num.ToString();
                    strArray[3] = "-";
                    num = 0;
                    strArray[4] = num.ToString();
                    strArray[5] = "-";
                    num = 0;
                    strArray[6] = num.ToString();
                    string selectionName = string.Concat(strArray);
                    NativeItem nativeItem = new NativeItem(name, selectionName);
                    nativeItemList.Add(nativeItem);
                }

                if (selection == "Hüte")
                {
                    List<NativeItem> nativeItemList = Items;
                    string name = "Kein Hut";
                    string[] strArray = new string[7];
                    strArray[0] = selection;
                    strArray[1] = "-";
                    strArray[2] = "1";
                    // Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
                    strArray[3] = "-";
                    strArray[4] = "500";
                    strArray[5] = "-";
                    strArray[6] = "0";
                    string selectionName = string.Concat(strArray);
                    NativeItem nativeItem = new NativeItem(name, selectionName);
                    nativeItemList.Add(nativeItem);
                }

                foreach (ClothingModel cloting in ClothingShopRegister.clothingList)
                {
                    if (cloting.category == selection)
                    {
                        List<NativeItem> nativeItemList = Items;
                        string name = cloting.name;
                        string[] strArray = new string[7];
                        strArray[0] = selection;
                        strArray[1] = "-";
                        int num = cloting.component;
                        strArray[2] = num.ToString();
                        strArray[3] = "-";
                        num = cloting.drawable;
                        strArray[4] = num.ToString();
                        strArray[5] = "-";
                        num = cloting.texture;
                        strArray[6] = num.ToString();
                        string selectionName = string.Concat(strArray);
                        NativeItem nativeItem = new NativeItem(name, selectionName);
                        nativeItemList.Add(nativeItem);
                    }
                }
                NativeMenu.closeNativeMenu(p);
                new NativeMenu("Kleidungsauswahl", selection, Items).showNativeMenu(p);
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("nM-Kleidungsauswahl")]
        public static void Kleidungsauswahl(Client p, string selection)
        {
            if (selection == null)
                return;

            try
            {
                string[] strArray = selection.Split("-");

                PlayerClothes playerClothes = Database.getDBClothing(p);

                if (strArray[0] == "Maske")
                {
                    playerClothes.Maske = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    Clothing.PlayerClothes.setClothes(p, 1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                }
                else if (strArray[0] == "Hüte")
                {
                    if (Convert.ToInt32(strArray[2]) == 500)
                    {
                        playerClothes.Hut = new clothingPart()
                        {
                            drawable = -1,
                            texture = 0
                        };
                        p.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                    }
                    else
                    {
                        playerClothes.Hut = new clothingPart()
                        {
                            drawable = Convert.ToInt32(strArray[2]),
                            texture = Convert.ToInt32(strArray[3])
                        };
                        p.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                    }
                }
                else if (strArray[0] == "Oberteil")
                {
                    playerClothes.Oberteil = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    Clothing.PlayerClothes.setClothes(p, 11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                }
                else if (strArray[0] == "Unterteil")
                {
                    playerClothes.Unterteil = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    Clothing.PlayerClothes.setClothes(p, 8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                }
                else if (strArray[0] == "Koerper")
                {
                    playerClothes.Koerper = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    Clothing.PlayerClothes.setClothes(p, 3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                }
                else if (strArray[0] == "Hose")
                {
                    playerClothes.Hose = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    Clothing.PlayerClothes.setClothes(p, 4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                }
                else if (strArray[0] == "Schuhe")
                {
                    playerClothes.Schuhe = new clothingPart()
                    {
                        drawable = Convert.ToInt32(strArray[2]),
                        texture = Convert.ToInt32(strArray[3])
                    };
                    Clothing.PlayerClothes.setClothes(p, 6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                }

                Database.setClothes(p, playerClothes);

            }
            catch (Exception ex) { Log.Write(ex.Message); }


        }

    }
}
// Copyright© Venux-Crimelife Script Mendosa // --- Angemeldet 1-0 --- //
