/*
CUserSecrets.cs

#### 建議
1. UserSecrets 只適用於 開發環境, 不要在正式環境上使用!
   不同的開發環境可擁有各自不同的 UserSecrets.

2. UserSecrets 不會加密，但將來可能會加密

3. 建議不要用!
   搞個半天, 結果不加密, 還只適用在開發環境!

依據 Microsoft.Extensions.Configuration.UserSecrets.
https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows


#### secrets.json 存放位置
File system path:
%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json

<user_secrets_id> 為專案檔中設定的 UserSecretsId 值, 例如:
專案.csproj:
<PropertyGroup>
    <UserSecretsId>bdf8ce6b-1a89-4a10-ba4f-d27fac662f0f</UserSecretsId>
</PropertyGroup>


#### 啟用 
1. dotnet user-secrets init
以上指令會在 專案.csproj 中設定 UserSecretsId 值 GUID
<PropertyGroup>
  <TargetFramework>netcoreapp3.1</TargetFramework>
  <UserSecretsId>79a3edd0-2092-40a2-a04d-dcb46d5ca9ed</UserSecretsId>
</PropertyGroup>

2. 或是在 Visual Studio 中啟動 Manage User Secrets

#### 設定一個秘密
dotnet user-secrets set "Movies:ServiceApiKey" "12345"
dotnet user-secrets set "Movies:ServiceApiKey" "12345" --project "C:\apps\WebApp1\src\WebApp1"
dotnet user-secrets set "DbPassword" "pass123"

#### 多人使用相同的秘密:
https://stackoverflow.com/questions/67790738/using-net-core-user-secrets-for-a-team
If usersecrets are used by multiple developers:
1. Dev1 creates usersecrets:
dotnet user-secrets init
dotnet user-secrets set "Movies:ServiceApiKey" "12345"
secrets.json will be created in C:\Users\Dev1\AppData\Roaming\Microsoft\UserSecrets\32fb5ba1-4330-43a8-a03b-4868ba51ca11

2. Dev2 checkout/clones the project, and creates his secrets:
dotnet user-secrets init 
Gets the message: "The MSBuild project 'C:\Temp\ConsoleApp1\ConsoleApp1 \ConsoleApp1.csproj' has already been initialized with a UserSecretsId."

dotnet user-secrets set "Movies:ServiceApiKey" "12345"
secrets.json will be created in C:\Users\Dev2\AppData\Roaming\Microsoft\UserSecrets\32fb5ba1-4330-43a8-a03b-4868ba51ca11
或是 以(設定多個秘密)方式匯入多個秘密資料.


3. So both users will have their secrets accessible thru the same GUID (all user secrets are stored in one file), so there wont be a problem with the project file.

Entry in project file for booth Devs:
 <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <UserSecretsId>32fb5ba1-4330-43a8-a03b-4868ba51ca11</UserSecretsId>
  </PropertyGroup>


#### 扁平化 .json 資料結構
JSON structure flattening in Visual Studio

結構化:
secrets.json
{
  "Movies": {
    "ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=Movie-1;Trusted_Connection=True;MultipleActiveResultSets=true",
    "ServiceApiKey": "12345"
  }
}

扁平化:
{
  "Movies:ConnectionString": "Server=(localdb)\\mssqllocaldb;Database=Movie-1;Trusted_Connection=True;MultipleActiveResultSets=true",
  "Movies:ServiceApiKey": "12345"
}

#### 設定多個秘密, 匯入多個秘密資料
type .\input.json | dotnet user-secrets set
the input.json file's contents are piped to the set command
以上將 input.json 匯入目前的 user-secret 資料檔中.

#### 程式讀取:
// Microsoft.Extensions.Configuration 設計精神是唯讀, 不應該由程式存放設定值. 這也是最大的需求問題.
// 若需要存放設定值, 則要另尋其他方案.

Access a secret
  1. Register the user secrets configuration source
  2. Read the secret via the Configuration API

a. Web
using System.Data.SqlClient;
var builder = WebApplication.CreateBuilder(args);
var conStrBuilder = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("Movies"));
conStrBuilder.Password = builder.Configuration["DbPassword"];
var connection = conStrBuilder.ConnectionString;
var app = builder.Build();
app.MapGet("/", () => connection);
app.Run();

public class IndexModel : PageModel
{
    private readonly IConfiguration _config;
    public IndexModel(IConfiguration config)
    {
        _config = config;
    }
    public void OnGet()
    {
        var moviesApiKey = _config["Movies:ServiceApiKey"];
        // call Movies service with the API key
    }
}

2. Console:
using Microsoft.Extensions.Configuration;
namespace ConsoleApp;
class Program
{
    static void Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        Console.WriteLine(config["AppSecret"]);
    }
}

#### 列出清單:
List the secrets
dotnet user-secrets list

#### 刪除一個秘密:
dotnet user-secrets remove "Movies:ConnectionString"

#### 刪除所有秘密:
dotnet user-secrets clear

#### 非 Web 專案必須自行安裝 Microsoft.Extensions.Configuration.UserSecrets
Projects that target Microsoft.NET.Sdk.Web automatically include support for user secrets. For projects that target Microsoft.NET.Sdk, such as console applications, install the configuration extension and user secrets NuGet packages explicitly.
PowerShell:
Install-Package Microsoft.Extensions.Configuration
Install-Package Microsoft.Extensions.Configuration.UserSecrets

.NET CLI:
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.UserSecrets

Console 範例:
using Microsoft.Extensions.Configuration;
namespace ConsoleApp;
class Program
{
    static void Main(string[] args)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        Console.WriteLine(config["AppSecret"]);
    }
}


 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBase
{
    public class CUserSecrets
    {
        // 欄位可以少於 .json 內容.
        public string? UserSecret1 { get; set; }
        public string? DbPassword1 { get; set; }
        public string? ServiceApiKey1 { get; set; }
    }
}
