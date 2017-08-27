using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissEphNet.Samples.WinForms46
{
    class WinFormsTestProvider : ITestProvider
    {
        public WinFormsTestProvider()
        {
            OutputContent = new StringBuilder();
            Output = new StringWriter(OutputContent);
            Debug = Output;
        }

        public StringBuilder OutputContent { get; private set; }

        public TextWriter Debug { get; private set; }

        public TextWriter Output { get; private set; }

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
