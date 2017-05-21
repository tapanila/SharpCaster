using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharpCaster.Console.YouTube
{
    class Program
    {
        private static string screenName = "Golang Test TV";
        private static string screenApp = "golang-test-838";
        private static string screenUid = "2a026ce9-4429-4c5e-8ef5-0101eddf5671";
        static void Main(string[] args)
        {
            //Do something about youtube
            DoSomething();
            System.Console.WriteLine("Execution completed");
            var wait = System.Console.ReadLine();
        }

        private static async void DoSomething()
        {
            var screenId = await HttpGet("https://www.youtube.com/api/lounge/pairing/generate_screen_id");
            var loungeToken = await HttpGet("https://www.youtube.com/api/lounge/pairing/get_lounge_token?screen_id=" + screenId);


            var bindCollection = new NameValueCollection
            {
                {"device", "LOUNGE_SCREEN"},
                {"id", screenUid},
                {"name", screenName},
                {"app", screenApp},
                {"theme", "cl"},
                {"mdx-version", "2"},
                {"loungeIdToken", loungeToken},
                {"VER", "8"},
                {"CVER", "1" },
                {"RID", "1337"},
                {"AID", "42"},
                {"zx", "xxxxxxxxxxxx"},
                {"t", "1"}
            };
            var bindUrl = "https://www.youtube.com/api/lounge/bc/bind?" + ToQueryString(bindCollection);
            var next = await HttpPost(bindUrl, "");

            var pairingCollection = new NameValueCollection
            {
                {"ctx", "pair" },
                {"access_type", "permanent" },
                {"app", screenApp },
                {"lounge_token", loungeToken },
                {"screen_id", screenId },
                {"screen_name", screenName }   
            };
            var pairUrl = "https://www.youtube.com/api/lounge/pairing/get_pairing_code?" +
                          ToQueryString(pairingCollection);
            next = await HttpGet(pairUrl);
            var i = 0;

            //https://www.youtube.com/api/lounge/pairing/generate_screen_id
            //https://www.youtube.com/api/lounge/pairing/get_lounge_token?screen_id=rqlc3ge6idciuac6lmpeii17s6
            //https://www.youtube.com/api/lounge/bc/bind?device=LOUNGE_SCREEN&id={Random_ID}&name=%s&loungeIdToken=%s&VER=8&RID=%d&zx=%s
        }

        private static async Task<string> HttpGet(string url)
        {
            var client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        private static async Task<string> HttpPost(string url, string postData)
        {
            var client = new HttpClient();
            return (await client.PostAsync(url, new StringContent(postData, Encoding.UTF8, "application/json"))).ToString();
        }
        private static string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}")
                .ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
