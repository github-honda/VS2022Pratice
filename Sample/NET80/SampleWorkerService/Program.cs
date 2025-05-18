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
                    Console.WriteLine("未知的參數，請使用 /Install 或 /Uninstall。");
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
            Console.WriteLine($"正在安裝 Windows 服務: {serviceName}");
            ExecuteCommand($"sc create {serviceName} binPath= {serviceExePath} start= auto");
            Console.WriteLine("安裝完成！");
        }

        static void UninstallService(string serviceName)
        {
            Console.WriteLine($"正在停止並移除 Windows 服務: {serviceName}");
            ExecuteCommand($"sc stop {serviceName}");
            ExecuteCommand($"sc delete {serviceName}");
            Console.WriteLine("移除完成！");
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