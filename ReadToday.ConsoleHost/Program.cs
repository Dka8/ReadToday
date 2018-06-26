using System;
using ReadToday.Services;
using System.ServiceModel;

namespace ReadToday.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost hostLookupManager = new ServiceHost(typeof(LookupManager));

            hostLookupManager.Open();

            Console.WriteLine("Services started. Press [Enter] to exit.");
            Console.ReadLine();

            hostLookupManager.Close();
        }
    }
}
