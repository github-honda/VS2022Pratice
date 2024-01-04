/*

Configuration Bind sample.

{
  "Parent": {
    "FavoriteNumber": 7,
    "Child": {
      "Name": "Example",
      "GrandChild": {
        "Age": 3
      }
    }
  }
}
The following table represents example keys and their corresponding values for the preceding example JSON:
Key	Value
"Parent:FavoriteNumber"	7
"Parent:Child:Name"	"Example"
"Parent:Child:GrandChild:Age"	3

 */

using ConsoleBase;
using Microsoft.Extensions.Configuration;

// Build a config object, using env vars and JSON providers.
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Get values from the config given their key and their target type.
Settings? settings = config.GetRequiredSection("Settings").Get<Settings>();

// Write the values to the console.
Console.WriteLine($"KeyOne = {settings?.KeyOne}");
Console.WriteLine($"KeyTwo = {settings?.KeyTwo}");
Console.WriteLine($"KeyThree:Message = {settings?.KeyThree?.Message}");

Console.WriteLine("Press any key to continue...");
Console.ReadKey();

// Application code which might rely on the config could start here.

// This will output the following:
//   KeyOne = 1
//   KeyTwo = True
//   KeyThree:Message = Oh, that's nice...
