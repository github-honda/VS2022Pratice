namespace GeekQuiz.Services
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using GeekQuiz.Hubs;
    using GeekQuiz.Models;
    using Microsoft.AspNet.SignalR;
    
    public class StatisticsService
    {
        private TriviaContext db;

        public StatisticsService(TriviaContext db)
        {
            this.db = db;
        }

        public async Task<StatisticsViewModel> GenerateStatistics()
        {
            var correctAnswers = await this.db.TriviaAnswers.CountAsync(a => a.TriviaOption.IsCorrect);
            var totalAnswers = await this.db.TriviaAnswers.CountAsync();
            var totalUsers = (float)await this.db.TriviaAnswers.GroupBy(a => a.UserId).CountAsync();

            var incorrectAnswers = totalAnswers - correctAnswers;

            return new StatisticsViewModel
            {
                CorrectAnswers = correctAnswers,
                IncorrectAnswers = incorrectAnswers,
                TotalAnswers = totalAnswers,
                CorrectAnswersAverage = (totalUsers > 0) ? correctAnswers / totalUsers : 0,
                IncorrectAnswersAverage = (totalUsers > 0) ? incorrectAnswers / totalUsers : 0,
                TotalAnswersAverage = (totalUsers > 0) ? totalAnswers / totalUsers : 0,
            };
        }

        public async Task NotifyUpdates()
        {

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<StatisticsHub>();
            if (hubContext != null)
            {
                var stats = await this.GenerateStatistics();

                // updateStatistics 可以為任何名稱. 但是在執行期間, 必須在客戶端可找到對應的函式名稱, 否則不會有任何反應.
                hubContext.Clients.All.updateStatistics(stats); 
            }
            /*
            Note: 
            In the code above, you are using an arbitrary method name to call a function on the client (i.e.: updateStatistics). 
            The method name that you specify is interpreted as a dynamic object, which means there is no IntelliSense or compile-time validation for it. 
            The expression is evaluated at run time. 
            When the method call executes, SignalR sends the method name and the parameter values to the client. 
            If the client has a method that matches the name, that method is called and the parameter values are passed to it. 
            If no matching method is found on the client, no error is raised. 
            For more information, refer to http://www.asp.net/signalr/overview/signalr-20/hubs-api/hubs-api-guide-server.             
             */
        }
    }
}