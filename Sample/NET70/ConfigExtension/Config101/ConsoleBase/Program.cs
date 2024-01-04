/*
 
The Host.CreateApplicationBuilder(String[]) method provides default configuration for the app in the following order, 
from highest to lowest priority:
1. Command-line arguments using the Command-line configuration provider.
2. Environment variables using the Environment Variables configuration provider.
3. App secrets when the app runs in the Development environment.
4. appsettings.Environment.json using the JSON configuration provider. For example, appsettings.Production.json and appsettings.Development.json.
5. appsettings.json using the JSON configuration provider.
6. ChainedConfigurationProvider : Adds an existing IConfiguration as a source.

Adding a configuration provider overrides previous configuration values. 
For example, the Command-line configuration provider overrides all values from other providers because it's added last. 
If SomeKey is set in both appsettings.json and the environment, the environment value is used because it was added after appsettings.json.
 
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

// Basic usage:
var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()
{
    ["SomeKey"] = "SomeValue"
}).Build();
Console.WriteLine($"SomeKey={configuration["SomeKey"]}");
// Outputs:
//   SomKey=SomeValue

// with Hosting:
using IHost host1 = Host.CreateDefaultBuilder(args).Build();
// Application code should start here.

// Runs an application and returns a Task that only completes when the token is triggered or shutdown is triggered and all IHostedService instances are stopped.
await host1.RunAsync();

