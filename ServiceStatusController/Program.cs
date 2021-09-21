using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;

namespace ServiceStatusController
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] services = ConfigurationManager.AppSettings.Get("Services").Split(',').Select(s => s.Trim()).ToArray(); ;
            while (1 == 1)
            {
                foreach (var service in services)
                    KeepItAlive(new ServiceController(service));
            }
        }

        public static void KeepItAlive(ServiceController service)
        {

            if (service.Status == ServiceControllerStatus.Stopped)
            {

                Console.WriteLine("{0} - {1} service stoped...", DateTime.Now, service.ServiceName);
                Console.WriteLine("{0} - Starting the Alerter service...", DateTime.Now);
                try
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);

                    Console.WriteLine("{0} - The Alerter service status is now set to {1}.", DateTime.Now,
                                       service.Status.ToString());
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("{0} - Could not start the Alerter service. \n {1}", DateTime.Now, e.ToString());
                }
            }
        }
    }
}
