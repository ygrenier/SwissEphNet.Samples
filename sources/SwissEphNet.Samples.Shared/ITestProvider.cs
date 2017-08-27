using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SwissEphNet.Samples
{
    public interface ITestProvider
    {
        TextWriter Output { get; }
        TextWriter Debug { get; }
        Stream LoadFile(string filename, out Encoding encoding);
    }
}
