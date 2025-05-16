using System.Diagnostics;

namespace ConsoleBase 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Main()");
            using (CProject._Me = new CProject(args))
                CProject._Me.Run();
        }
    }
}