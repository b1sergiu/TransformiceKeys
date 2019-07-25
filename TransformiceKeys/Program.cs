using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TransformiceKeys
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient client = new WebClient();
            //client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            string token = File.ReadAllText("api.json");
            JObject fileJson = JObject.Parse(token);

            string apiData = client.DownloadString($"http://api.tocu.tk/get_transformice_keys.php?tfmid={fileJson.GetValue("tfmid")}&token={fileJson.GetValue("token")}");

            if (apiData == "\"T\"")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error: Invalid ID or token.");
                Console.ReadKey();
                Environment.Exit(5);
            }

            JObject json = JObject.Parse(apiData);

            var success = json.GetValue("success");
            if (Convert.ToBoolean(success) == false)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Error: {json.GetValue("error")}");
                Console.ReadKey();
                Environment.Exit(5);
            }

            Console.WriteLine($"Version: 1.{json.GetValue("version")}");
            Console.WriteLine($"Connection key: {json.GetValue("connection_key")}");
            Console.WriteLine($"Authenthication key: {json.GetValue("auth_key")}");
            Console.WriteLine($"Packet keys: {json.GetValue("packet_keys")}");
            Console.WriteLine($"Identification keys: {json.GetValue("identification_keys")}");
            Console.WriteLine($"Message keys: {json.GetValue("msg_keys")}\n");
            Console.WriteLine("Save to file? Y/N");
            string response = Console.ReadLine();
            if (response.ToLower() == "y")
            {
                File.WriteAllText("keys.json", Convert.ToString(json));
                File.WriteAllText("keys.txt", $"Version: 1.{json.GetValue("version")}\nConnection key: {json.GetValue("connection_key")}\nAuthenthication key: {json.GetValue("auth_key")}\nPacket keys: {json.GetValue("packet_keys")}\nIdentification keys: {json.GetValue("identification_keys")}\nMessage keys: {json.GetValue("msg_keys")}\n");
            }
        }
    }
}
