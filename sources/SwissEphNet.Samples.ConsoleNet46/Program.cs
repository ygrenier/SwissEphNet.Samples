using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissEphNet.Samples.ConsoleNet46
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var swetest = new SwephTest(new Net46TestProvider()))
                swetest.RunTest();
            Console.WriteLine("Press enter to quit...");
            Console.ReadLine();
        }
    }
}
