using System;
using System.Net;

namespace PumoxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string uri = "http://localhost.fiddler:4000/companies/get";

                using (WebClient client = new WebClient())
                {
                    Console.WriteLine(client.DownloadString(uri));
                }

                if (Console.ReadKey().Key == ConsoleKey.E)
                    break;
            }
        }
    }
}
