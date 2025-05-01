/*

ref:
https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder
 
 */


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GenericHost
{
    internal class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Hello, World!");
        //}
        static async Task Main(string[] args)
        {
            // HostBuilder
            HostApplicationBuilder builder1 = Host.CreateApplicationBuilder(args);

            // IHostedService
            builder1.Services.AddHostedService<CHostedService>();

            // Host configuration from:
            builder1.Environment.ContentRootPath = Directory.GetCurrentDirectory();
            builder1.Configuration.AddJsonFile("hostsettings.json", optional: true, reloadOnChange: false);
            builder1.Configuration.AddEnvironmentVariables(prefix: "PREFIX_");
            builder1.Configuration.AddCommandLine(args);

            using IHost host1 = builder1.Build();

            // Application code should start here.

            // Host shutdown/Hosting shtdown process ref to:
            // readme-GenericHost.pdf
            // or https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder
            await host1.RunAsync();
            //await Task.Delay(1000);
            //await host.StopAsync();

        }

    }
}

/*

Output:
info: GenericHost.CHostedService[0]
      1. StartAsync has been called.
info: GenericHost1.CHostedService[0]
      2. OnStarted has been called.
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: ...

Press Ctrl-C here

info: GenericHost.CHostedService[0]
      3. OnStopping has been called.
info: Microsoft.Hosting.Lifetime[0]
      Application is shutting down...
info: GenericHost.CHostedService[0]
      4. StopAsync has been called.
info: GenericHost.CHostedService[0]
      5. OnStopped has been called.

...\GenericHost.exe (process 28308) exited with code 0.
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .
 
 */


