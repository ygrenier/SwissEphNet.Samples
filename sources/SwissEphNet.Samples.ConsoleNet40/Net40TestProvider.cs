using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SwissEphNet.Samples.ConsoleNet40
{
    class Net40TestProvider : ITestProvider
    {
        class DebugTextWriter : TextWriter
        {
            public override void Write(char[] buffer, int index, int count)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Out.Write(buffer, index, count);
                Console.Out.Flush();
                Console.ResetColor();
            }
            public override Encoding Encoding => Console.Out.Encoding;
        }
        DebugTextWriter _debugOut = new DebugTextWriter();
        public TextWriter Output => Console.Out;
        public TextWriter Debug => _debugOut;
        public Stream LoadFile(string filename, out Encoding encoding)
        {
            encoding = null;
            filename = filename.Replace("[ephe]\\", "datas\\");
            if (File.Exists(filename))
            {
                return File.OpenRead(filename);
            }
            return null;
        }
    }
}
