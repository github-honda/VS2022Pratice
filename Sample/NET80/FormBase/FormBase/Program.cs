using System.Diagnostics;

namespace FormBase
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Debug.WriteLine("Main()");
            using (CProject._Me = new CProject(args))
            {
                ApplicationConfiguration.Initialize();
                //Application.Run(new Form1());
                CProject._MainForm = new Form1();
                Application.Run(CProject._MainForm);

                //// �� Form1_Shown �A����
                //Task.Run(CProject._Me.Run);
            }
        }
    }
}