//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
/*

#### CreateApplicationBuilder 通用方法 (generic host approach) 如下:
//Sample ref:   
https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration#basic-example
https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#json-configuration-provider

Basic example with hosting and using the indexer API

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();

IHostEnvironment env = builder.Environment;

builder.Configuration
    .AddIniFile("appsettings.ini", optional: true, reloadOnChange: true)
    .AddIniFile($"appsettings.{env.EnvironmentName}.ini", true, true);

using IHost host = builder.Build();

// Application code should start here.
foreach ((string key, string? value) in
    builder.Configuration.AsEnumerable().Where(t => t.Value is not null))
{
    Console.WriteLine($"{key}={value}");
}
// Sample output:
//    TransientFaultHandlingOptions:Enabled=True
//    TransientFaultHandlingOptions:AutoRetryDelay=00:00:07
//    SecretKey=Secret key value
//    Logging:LogLevel:Microsoft=Warning
//    Logging:LogLevel:Default=Information

await host.RunAsync();


 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConsoleBase;
using System;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ConsoleBase // Note: actual namespace depends on the project name.
{
    public class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Main()");
            using (CProject project = new CProject())
            {
                project.Function1();
            }
        }
    }
}