using System;

namespace WPF.RealTime.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new WcfServer())
            {

                var start = DateTime.UtcNow;
                Console.WriteLine(String.Format("Server has been created at {0} and listening for connections ...",start));

                while (true)
                {
                    //if ((DateTime.UtcNow.Second % 15 == 0))
                    //    Console.WriteLine(String.Format("Still up and running at {0}", DateTime.UtcNow));
                }
            }   
        }
    }
}
