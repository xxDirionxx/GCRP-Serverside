using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Venux.Items;

namespace Venux.Shops
{
    class ShopRegister : Script
    {
        public static List<Shop> shopList = new List<Shop>();

        [ServerEvent(Event.ResourceStart)]
        public void registerShops()
        {
            shopList.Add(new ShopAmmunation());
            shopList.Add(new ShopDavis());
            shopList.Add(new ShopVespucci());
            shopList.Add(new ShopVinewood());
            shopList.Add(new Shop247());

            foreach (Shop shop in shopList)
            {
                NAPI.Marker.CreateMarker(1, shop.position, new Vector3(), new Vector3(), 1.0f, new Color((int)byte.MaxValue, 165, 0), false, 0);

                ColShape val = NAPI.ColShape.CreateCylinderColShape(shop.position, 1.4f, 1.4f, 0);
                val.SetData("COLSHAPE_FUNCTION", new FunctionModel("openShop", NAPI.Util.ToJson(shop)));
                val.SetData("COLSHAPE_MESSAGE", new Notification.Message("Benutze E um den Shop zu öffnen.", shop.title, "white", 3500));

                if (shop.customBlip > 0U)
                    NAPI.Blip.CreateBlip(shop.customBlip, shop.position, 1f, shop.customBlipColor, shop.title, byte.MaxValue, 0.0f, true, (short)0, 0);
                else
                    NAPI.Blip.CreateBlip(52, shop.position, 1f, (byte)2, shop.title, byte.MaxValue, 0.0f, true, (short)0, 0);
            }

            Log.Write("Shops geladen.");
        }

        [RemoteEvent("openShop")]
        public void openShop(Client p, string shopModel)
        {
            if (shopModel == null)
                return;

            try
            {
                p.TriggerEvent("openWindow", new object[2]
                {
                    "Shop",
                    shopModel
                });
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        [RemoteEvent("shopBuy")]
        public void shopBuy(Client p, string json)
        {
            try
            {
                List<basketItems> basket = NAPI.Util.FromJson<basketShop>(json).basket;
                List<ItemModel> list = new List<ItemModel>();



                int num = 0;

                foreach (basketItems basketItems in basket)
                {
                    if (basketItems.price > 0)
                    {
                        num += basketItems.price;
                        list.Add(Database.getItemModelByName(basketItems.itemId, basketItems.count));
                    }
                }

                if (Database.getMoney(p.Name) >= num)
                {
                    NAPI.Task.Run(() =>
                    Database.changeMoney(p.Name, num, true));
                    Notification.SendPlayerNotifcation(p, "Du hast erfolgreich dein Equip eingekauft!", 3500, "green", "SHOP", "white");
                    foreach (ItemModel itemModel in list)
                        foreach (basketItems basketItems in basket)
                        {
                            Database.changeInventoryItem(p.Name, itemModel.Name, basketItems.count, false);
                        }

                }
                else
                {
                    Notification.SendPlayerNotifcation(p, "Du hast zu wenig Geld, um diese Items zu kaufen.", 5000, "green", "SHOP", "white");
                }
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }
    }
}
