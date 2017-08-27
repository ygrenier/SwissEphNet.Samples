using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwissEphNet.Samples.WPF46
{
    class WpfTestProviderAsync : ITestProvider
    {
        public WpfTestProviderAsync()
        {
            OutputContent = new StringBuilder();
            Output = new StringWriter(OutputContent);
            Debug = Output;
        }

        public StringBuilder OutputContent { get; private set; }

        public TextWriter Debug { get; private set; }

        public TextWriter Output { get; private set; }

        async Task<Tuple<Stream, Encoding>> SimulateLoadFileAsync(string filename)
        {
            filename = filename.Replace("[ephe]\\", "datas\\");
            await Task.Delay(500);
            if (File.Exists(filename))
            {
                using (var file = File.OpenRead(filename))
                {
                    var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    return Tuple.Create<Stream, Encoding>(stream, null);
                }
            }
            return Tuple.Create<Stream, Encoding>(null, null);
        }

        public Stream LoadFile(string filename, out Encoding encoding)
        {
            var res = Task.Run(async () => await SimulateLoadFileAsync(filename)).Result;
            encoding = res.Item2;
            return res.Item1;
        }

    }
}
