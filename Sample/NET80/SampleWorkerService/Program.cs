using System.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string serviceName = "MyWorkerService";
                //string serviceExePath = $"\"{System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName}\"";
                string serviceExePath = Path.Combine(AppContext.BaseDirectory, "MyWorkerService");

                if (args[0] == "/Install")
                {
                    InstallService(serviceName, serviceExePath);
                }
                else if (args[0] == "/Uninstall")
                {
                    UninstallService(serviceName);
                }
                else
                {
                    Console.WriteLine("�������ѼơA�Шϥ� /Install �� /Uninstall�C");
                }
            }
            else
            {
                Host.CreateDefaultBuilder()
                    .UseWindowsService() // This requires the Microsoft.Extensions.Hosting.WindowsServices package
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                    })
                    .Build()
                    .Run();
            }
        }

        static void InstallService(string serviceName, string serviceExePath)
        {
            Console.WriteLine($"���b�w�� Windows �A��: {serviceName}");
            ExecuteCommand($"sc create {serviceName} binPath= {serviceExePath} start= auto");
            Console.WriteLine("�w�˧����I");
        }

        static void UninstallService(string serviceName)
        {
            Console.WriteLine($"���b����ò��� Windows �A��: {serviceName}");
            ExecuteCommand($"sc stop {serviceName}");
            ExecuteCommand($"sc delete {serviceName}");
            Console.WriteLine("���������I");
        }

        static void ExecuteCommand(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C {command}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
        }
    }
}