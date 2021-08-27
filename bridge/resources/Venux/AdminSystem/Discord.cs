using System;
using System.Net.Http;
using System.Text;

namespace Venux.AdminSystem
{
    class DiscordWebhook
    {

        public static string killWebhook = "https://discord.com/api/webhooks/876862674946908160/mk6Hrgj8RieUs3eScp9MXLhnyKPEGe823bUEUtQrSLKLtM6HEgEgIi56SeV5CFu_SYl_";
        public static string itemWebhook = "https://discord.com/api/webhooks/876862787563970630/fvvPR-XimpBrTbYJz_IVl6EJq3NXzZnjOOQSOMFJWv2rfO1L8GUa9IcHFyeRi8pS-iA2";
        public static string joinWebhook = "https://discord.com/api/webhooks/876862889426829312/5wKH33JEo56Sbyidrl5joek9JB6halhu4oAcge0roPLzzBvMqv7bDxX49tJLqQQIYyUA";
        public static string kofferraumWebhook = "https://discord.com/api/webhooks/876863012290564147/KUn4A6fZFzzlrSD6fUDFrOFijdCaVrnTl54ezKr28iXOZOCl3aipzKC6Vlgrg4LjJm7t";
        public static string fraklagerWebhook = "https://discord.com/api/webhooks/876863152267100190/0XPSl1rxq04Ks8gs4pNpSQIpvl-43G_06pVVYJC0fYsVLIk6-gklOF7d9BZMy3zfBykv";
        public static string geldWebhook = "https://discord.com/api/webhooks/876863257934188584/IpxuiMWHk2Ta8OsMPd5djhHzvYWCIlotNV8pDsL9u73XYZ2OEEY7ZSydUFG5BgticUO1";
        public static string adminWebhook = "https://discord.com/api/webhooks/876863424498397285/sQeuuRIAsjJMgBAC0nIVkkJYqA_FqxUUd0nFqZdqjkGbMD9KZZQzcbQw7ajtlDp7RD1s";
        public static string kickWebhook = "https://discord.com/api/webhooks/877585923523616848/xGKt-XqWZmx-BEDjYnVzQ_l4qOfcgoS53OpUg6gKQP7aZJjMW2pnvyvIJIbOsy51n5Ws";
        public static string banWebhook = "https://discord.com/api/webhooks/876863915181637662/h2m5wS79eqo7rVIYgg1yRNvldOhwR4ZquE0gC11h-KP9wIRs6Z8YXRwf3TA95GB73m6J";
        public static string reviveWebhook = "https://discord.com/api/webhooks/876864035658797077/RRdoYSSLpf8udiIrFCiDyAfdoQ7-cAfit2-FNdXTN9KP-nJKZu6f4GMXDa16hk99I0G-";
        public static string warnWebhook = "https://discord.com/api/webhooks/876864140130549761/jPp8-5UOMiI4Ldjjm9aCDdInqgTBdLVtg_8ikAo_kBnYiQ38uoJarkw1wMVyCthLLxlC";
        //public static string supportWebhook = "https://discord.com/api/webhooks/876896896814297108/MmsjbILxbv4fm6MljjjMI3s0TEpvSrbeeyBzM4Rh1NkJeJXf1TxQ27Yp9XJe5_F5R4QW";


        public static async void SendMessage(string title, string msg, string webhook, string type)
        {
            try
            {
                //msg = msg + Environment.NewLine + information;
                DateTime now = DateTime.Now;
                string[] strArray = new string[20];
                strArray[0] = "{\"username\":\"Classic-Crimelife\",\"avatar_url\":\"https://media.discordapp.net/attachments/770324511483494443/821076959966134342/VCL.png?width=545&height=545\",\"content\":\"\",\"embeds\":[{\"author\":{\"name\":\"Test Server\",\"url\":\"https://discord.gg/c4SatEHhSy\",\"icon_url\":\"https://media.discordapp.net/attachments/770324511483494443/821076959966134342/VCL.png?width=545&height=545\"},\"title\":\"" + type + "\",\"thumbnail\":{\"url\":\"https://media.discordapp.net/attachments/770324511483494443/821076959966134342/VCL.png?width=545&height=545\"},\"url\":\"https://discord.gg/c4SatEHhSy\",\"description\":\"Es wurde am **";
                int num = now.Day;
                strArray[1] = num.ToString();
                strArray[2] = ".";
                num = now.Month;
                strArray[3] = num.ToString();
                strArray[4] = ".";
                num = now.Year;
                strArray[5] = num.ToString();
                strArray[6] = " | ";
                num = now.Hour;
                strArray[7] = num.ToString();
                strArray[8] = ":";
                num = now.Minute;
                strArray[9] = num.ToString();
                strArray[10] = "** ein " + type + " ausgelöst.\",\"color\":5433007,\"fields\":[{\"name\":\"";
                strArray[11] = title;
                strArray[12] = "\",\"value\":\"";
                strArray[13] = msg;
                strArray[14] = "\",\"inline\":true}],\"footer\":{\"text\":\"German Classic Server-Logs | " + type + " Dirion (c) 2021\"}}]}";
                string stringPayload = string.Concat(strArray);
                StringContent httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(webhook, (HttpContent)httpContent);
                }
                stringPayload = (string)null;
                httpContent = (StringContent)null;
            }
            catch (Exception ex) { Log.Write(ex.Message); }
        }

        internal static void SendMessage(string v1, object p, string supportWebhook, string v2) => throw new NotImplementedException();

        class DiscordColor
        {
            public enum Colors
            {
                BLACK = 1,
                GREEN = 4289797, // 0x00417505
                RCL = 5433007,
                BLUE = 4886754, // 0x004A90E2
                CYAN = 5301186, // 0x0050E3C2
                BROWN = 9131818, // 0x008B572A
                GREY = 10197915, // 0x009B9B9B
                RED = 13632027, // 0x00D0021B
                ORANGE = 16098851, // 0x00F5A623
                YELLOW = 16312092, // 0x00F8E71C
                WHITE = 16777215, // 0x00FFFFFF
            }
        }
    }
}

// Copyright© Test Server Script Dirion // --- Angemeldet 1-0 --- //

