20221009

本專案將 Visual Studio 2022 產生的 ASP.NET MVC 網站專案預設樣板, 
  取消 EntityFramework Code first, 
  並將資料庫改為 MySql.

快速說明修改的步驟如下.
□ 將 Visual Studio 2022 產生的 ASP.NET MVC 網站專案 MVC48ID 改為 MVC48IDMySql.

□ 專案 MVC48IDMySql 刪除參考:
1. MVC48ID
1. Remove reference Microsoft.AspNet.Identity.EntityFramework
2. Remove reference EntityFramework.SqlServer
3. Remove reference EntityFramework

□ 加入專案
1. ZLib
2. ZLibMySql, 參考 ZLib.
3. LibIdentityMySql, 參考

□ 專案 MVC48IDMySql 參考以上三個專案(ZLib, ZLibMySql, LibIdentityMySql).

□ 專案 MVC48IDMySql 修改檔案:
1. App_Start\IdentityConfig.cs
2. App_Start\Startup.Auth.cs
3. Models\IdentityModels.cs
4. Web.config 的 ConnectionString.

□ 編譯後以瀏覽器測試 Register, Login 是否資料庫正確.
□ 可進入資料庫中, 刪除 AspNetUsers.id 資料, 就可以重新 Register 帳號.

取消 EntityFramework Code first 的好處可參考(這篇說明, https://svc.011.idv.tw/CodeHelper/cs/ASPNET/identity/WebAuthSQL/doc/Readme-WebAuthSQL.html)
另外可參考之前 VS2015, VS2019 改過的紀錄: https://svc.011.idv.tw/CodeHelper/cs/vs2019/vs2019Practice/WebIdentity/doc/Readme-WebIdentitySqlServer.html

另外可參考之前 VS2015, VS2019 改過的紀錄: https://svc.011.idv.tw/CodeHelper/cs/vs2019/vs2019Practice/WebIdentity/doc/Readme-WebIdentitySqlServer.html

□ 可進入資料庫中, 刪除 AspNetUsers.id 資料, 就可以重新 Register 帳號.
DBAspNetIdentity.sql: SQLite
IdentityTables.sql: SQLServer
MySQLIdentity.sql: MySql

□,○,△,◇,
