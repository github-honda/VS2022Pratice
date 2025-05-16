using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DSample
{
    internal class TaskDoSomething
    {
        string v1 = string.Empty;
        public void DoSomething() => v1 = "Something";
        public Task DoSomethingTask() => Task.Run(DoSomething);
        public async Task DoSomethingTaskAsync() => await Task.Run(DoSomething);


        public void Call_Task() => DoSomethingTask();
        public Task Call_AsyncTask1() => DoSomethingTaskAsync();
        public async void Call_AsyncTask2() => await DoSomethingTaskAsync();
        public void Call_ByCodeBlock()
        {
            string sReturn = string.Empty;
            try
            {
                Task.Run(async () => await Task.Run(DoSomething));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void Call_ByLambda1()
            => Task.Run(() => Task.Run(DoSomething));

        public void Call_ByLambda2()
            => Task.Run(async () => await Task.Run(DoSomething));
    }
}
