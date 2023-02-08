From: 011netservice@gmail.com
Date: 2023-01-10
Subject: SignalR Introduction and Tutorials, ASP.NET 4.x
File: CodeHelper\cs\vs2022\VS2022Pratice\Asp\SignalR40\Readme-SignalR40.txt

歡迎來信交流, 訂購軟體需求.

結論:
1. SignalR 可用.

2. SignalR + backplane 不建議使用. 
   由於 backplane 會轉送每個訊息到每個端點, 很容易造成網路傳輸量爆量遽增, 造成瓶頸! 
   僅適用於 Server broadcast (e.g., stock ticker) with controls the rate at which messages are sent.


#### SignalR Introduction and Tutorials, ASP.NET 4.x:
Introduction to SignalR
  https://learn.microsoft.com/en-us/aspnet/signalr/overview/getting-started/ 
□ Tutorial: Getting Started with SignalR 2
  Tutorial-Real-time chat with SignalR 2.pdf
  Folder: SignalR2
	
□ Tutorial: Getting Started with SignalR 2 and MVC 5
  Tutorial-Real-time chat with SignalR 2 and MVC 5.pdf
  Folder: SignalR2MVC
	○ 注意
	  ◇ Project name 應為 SignalRChat
	  ◇ VS2022 在 Create a new ASP.NET Web Application 畫面時, 選擇 (Template=MVC, Authentication=None, Add folders & core references=MVC, Advanced=Configure for HTTPS.
	○ Code Description:
         https://www.c-sharpcorner.com/article/signalr-message-conversation-using-asp-net-mvc-in-real-time-scenario/
		 
□ Tutorial: High-Frequency Realtime with SignalR 2
  Tutorial-Create high-frequency real-time app with SignalR 2.pdf
  Folder: SignalR2Realtime
    MoveShapeDemo1: 
	  1. 客戶端網頁上滑鼠每一點移動 就會拋送位置訊息 到伺服器端.
	  2. 伺服器端收到多少訊息, 就會拋送一樣多的訊息 到客戶端.
	MoveShapeDemo2:
      MoveShapeDemo2 與 MoveShapeDemo1 的差異 
	  1. Client 與 Server 各加入 timer 當作節流閥(throttle), 只定時定時拋送訊息, 以減少網路流量.
	     類似 "game loop".
	  2. 改用 jQuery UI 的 animate 功能, 加速前端繪圖, 視覺效果較平順.

□ Tutorial: Server Broadcast with SignalR 2
  Tutorial-ServerBroadcast.pdf
     server broadcast. Periodically, the server randomly updates stock prices and broadcast the updates to all connected clients.
	 每個連線上來不同的客戶端, 可共享一個伺服器端的最新共用資料區. 
  Folder: SignalR2Broadcast
     內含兩個範例, 兩者皆可獨立執行: 
	 若 Set As Start Page 為
     1. SignalR.StockTicker\StockTicker.js	 
	   則為 Tutorial 的範例.
	   
     2. SignalR.StockTicker\SignalR.Sample\StockTicker.html	 
       則為安裝 (Microsoft.AspNet.SignalR.Sample) 的範例, 提供更多示範功能.	 

□ Hands On Lab: Real-Time Web Applications with SignalR
	Folder: SignalR2Web
		這是 WebCampTrainingKit v2015.10.13b 的示範教學程式.
		Release summary:
			Updated for Visual Studio 2015 and ASP.NET 4.6 release
			Updated Modern Web Front Ends session for Edge, other web technologies
			Added ASP.NET 5 preview session
			Thank you for your feedback! Issues and Pull Requests are very welcome!
		原版: (Begin, End)
			https://github.com/Microsoft-Web/WebCampTrainingKit/releases/tag/v2015.10.13b
			或 CodeHelper\cs\SignalR\WebCampTrainingKit-2015.10.13b.zip 
		差異: (Diff)
			CodeHelper\cs\vs2022\VS2022Pratice\Asp\SignalR40\SignalR2Web\RealTimeSignalR\Source\Ex1-WorkingWithRealTimeData\Diff\End.sln
			CodeHelper\cs\vs2022\VS2022Pratice\Asp\SignalR40\SignalR2Web\RealTimeSignalR\Source\Ex2-ScalingOutWithSQLServer\Diff\End.sln
		測試: (EndPratice)
		    □ 注意都必須自行建立空的資料庫後才能執行!
			    執行時, 經過 Register new user 後, 就會自動建立 Tables.
			    建立資料庫可以使用 SQL Server Object Explorer 使用 SQL Server Express 或 LocalDB, 將資料庫檔案, 建立在各專案下的 App_Data 目錄中.
			□ 測試帳號
			    都是 usertest1, test1111
			□ Folders
			○ Ex1-WorkingWithRealTimeData\Begin\Begin.sln
			  一般的網站未使用 SignalR 架構, 就必須靠重新瀏覽網頁後, 才會更新網頁上的統計數字.
			  Web.config 的 connectionString 需改成執行環境中的資料庫:
                  <add name="DefaultConnection" connectionString="Data Source=(localdb)\ProjectModels;Initial Catalog=GeekQuiz1Begin;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" providerName="System.Data.SqlClient" />

			○ Ex1-WorkingWithRealTimeData\End\
			  原版將 Begin.sln 改成使用 SignalR 後, 可以 Server push 或 broadcasting 的方式, 自動更新網頁上的統計數字..

			○ Ex1-WorkingWithRealTimeData\Diff\
			  原版 End 跟 Begin 差異.

			○ Ex1-WorkingWithRealTimeData\EndPratice\EndPratice.sln
			  以 Diff 版本測試及加入註解.
			  步驟詳如 CodeHelper\cs\SignalR\WebCampTrainingKit-2015.10.13b\HOL\RealTimeSignalR\README.md, 截錄重點如下:
			  ◇ Web.config 的 connectionString 需改成執行環境中的資料庫:
				例如: <add name="DefaultConnection" connectionString="Data Source=(localdb)\ProjectModels;Initial Catalog=GeekQuiz1End;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False" providerName="System.Data.SqlClient" />

			  ◇ 安裝 Package Microsoft.AspNet.SignalR
			    Install-Package Microsoft.AspNet.SignalR
				  若還沒有安裝 OWIN, 則需先安裝"Microsoft.Owin*"系列套件.
				    get-package | where-object { $_.Id -like "Microsoft.Owin*"} | Update-Package
				    未來版本的 SignalR 會自動安裝 OWIN, 應可省略本步驟
					
			  ◇ Add SignalR Hub Class(V2) = Statictics.cs
			  ◇ 修改 OWIN 的啟動程式 Startup.cs
			  ◇ 修改 Services\StatisticsServices.cs
			    △ NotifyUpdates() 取得 Context 後, 轉送到客戶端或廣播.
					public async Task NotifyUpdates()
					{

						var hubContext = GlobalHost.ConnectionManager.GetHubContext<StatisticsHub>();
						if (hubContext != null)
						{
							var stats = await this.GenerateStatistics();

							// updateStatistics 可以為任何名稱, 但是必須在執行期間, 可在客戶端找到對應的函式名稱, 否則不會有任何反應.
							hubContext.Clients.All.updateStatistics(stats); 
						}
					}
			    △ TriviaController.cs.Post() 增加更新統計數字的邏輯..
			  ◇ 修改 Statistics.cshtml
			    △ 確定使用的 javascript 版本對應到新安裝的 /Scripts/ 目錄下的版本
				Make sure that the script reference in your code matches the version of the script library installed in your project.
				例如:
				@section Scripts {
				@Scripts.Render("~/Scripts/jquery.signalR-2.2.0.min.js")
				@Scripts.Render("~/signalr/hubs")
				@Scripts.Render("~/Scripts/flotr2.js")
				@Scripts.Render("~/Scripts/statistics.js")
				...
				
			    △ Creating a Hub Proxy and registering an event handler to listen for messages sent by the server.
				增加 updateStatistics 方法, 自動接收伺服器端最新的統計資料, 因此可不需要重新瀏覽本畫面, 即可更新畫面上的內容.
				updateStatistics 可以為任何名稱. 但是在執行期間, 必須在客戶端可找到對應的函式名稱, 否則不會有任何反應.

			○ Ex2-ScalingOutWithSQLServer\Begin\Begin.sln
			    Scaling up: 更強的伺服器.
				Scaling out: 更多的伺服器, 但是必須要將不同伺服器收集到的資料集中處理.
				When scaling a web application, you can generally choose between scaling up and scaling out options. 
				Scale up means using a larger server, with more resources (CPU, RAM, etc.) while scale out means adding more servers to handle the load. 
				The problem with the latter is that the clients can get routed to different servers. 
				A client that is connected to one server will not receive messages sent from another server.

				backplane component: 可轉送伺服器資料到其他的伺服器.
				You can solve these issues by using a component called backplane, to forward messages between servers. 
				With a backplane enabled, each application instance sends messages to the backplane, and the backplane forwards them to the other application instances.

				有三種型態的 backplane:
				There are currently three types of backplanes for SignalR:
				  1. Azure Service Bus. 
				     Service Bus is a messaging infrastructure that allows components to send loosely coupled messages.
				  2. SQL Server. 
				     The SQL Server backplane writes messages to SQL tables. 
					 The backplane uses Service Broker for efficient messaging. However, it also works if Service Broker is not enabled.
				  3. Redis. 
				     Redis is an in-memory key-value store. Redis supports a publish/subscribe (�pub/sub�) pattern for sending messages.
				
				注意 SignalR 的上限, 參考這篇:
				https://stackoverflow.com/questions/24891744/why-is-message-delivery-time-not-scaling-well-using-signalr
Q: I'm still testing SignalR but one of the things that's really important to me is that the messages reach the client as quickly as possible (I'm dealing with real time stock rates).
The things is - under almost every scenario I've tried - from totally local to running 100's of instances on Azure (with back-plane and everything...) , the time it takes the message to get from the server to the client increases exponentially as the number of connected clients increase.
I've tried this with Hubs, Persistent Connection, .net clients, JS clients running in phantomJS, zombieJS & node.js ... I've pretty much tried dozens of configurations, but the behavior is always the same, which leads me to the conclusion that this is something inherent in SignalR.
I know SignalR can handle thousands of concurrent clients on very few servers, but if it takes a couple of seconds for the message to go across (In the same Azure region), it's of no use to me.
Any idea what might be slowing the messages down ?

A: It is explained in their scale out guide:
  https://learn.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-in-signalr

Limitations
Using a backplane, the maximum message throughput is lower than it is when clients talk directly to a single server node. That's because the backplane forwards every message to every node, so the backplane can become a bottleneck. Whether this limitation is a problem depends on the application. For example, here are some typical SignalR scenarios:

1. Server broadcast (e.g., stock ticker): Backplanes work well for this scenario, because the server controls the rate at which messages are sent.
2. Client-to-client (e.g., chat): In this scenario, the backplane might be a bottleneck if the number of messages scales with the number of clients; that is, if the rate of messages grows proportionally as more clients join.
3. High-frequency realtime (e.g., real-time games): A backplane is not recommended for this scenario.

So using a backplane will create delays, and depending on the type of application you are doing... it may not be the right choice.
I gave up SignalR long ago and focus on websockets with a proper queuing system behind. 
You may use a websocket library together with MassTransit for example. SignalR is for little projects, or not very segmented or real time scenarios.
				結論: 
				  由於 backplane 會轉送每個訊息到每個端點, 很容易造成網路傳輸量爆量遽增, 造成瓶頸! 
				  例如以上問題案例, 隨著連接的客戶端數量的增加呈指數增長, 大約100個Client同時連線就掛了!
				  因此
				    backplane 較適用於 Server broadcast (e.g., stock ticker), 因為可以由 Server 決定 broadcast 的速度.
				    backplane 不適用於 (2. Client-to-client 及 3. High-frequency realtime)
				後面的 backblane 我就懶得再測試了!
				預計改研究 websocket library together with MassTransit
				2023-01-16
				

ref:
SignalR Introduction and Tutorials, ASP.NET 4.x, 2020-02-20:
https://learn.microsoft.com/en-us/aspnet/signalr/overview/getting-started/ 

SignalR, ASP.NET 4.x:
CodeHelper\cs\SignalR\SignalRFork\SignalR
https://github.com/github-honda/SignalR

SignalR, ASP.NET Core
CodeHelper\cs\AspNetCoreFork
CodeHelper\cs\AspNetCoreFork\src\SignalR
https://github.com/github-honda/aspnetcore