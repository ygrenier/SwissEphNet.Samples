using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Threading.Tasks;

namespace SwissEphNet.Samples.Droid
{
    class DroidTestProviderAsync : ITestProvider
    {
        public DroidTestProviderAsync(Activity activity)
        {
            Activity = activity;
            OutputContent = new StringBuilder();
            Output = new StringWriter(OutputContent);
            Debug = Output;
        }

        public StringBuilder OutputContent { get; private set; }

        public TextWriter Debug { get; private set; }

        public TextWriter Output { get; private set; }

        public Activity Activity { get; private set; }

        async Task<Tuple<Stream, Encoding>> SimulateLoadFileAsync(string filename)
        {
            try
            {
                filename = filename.Replace("[ephe]\\", "datas/");
                //return Activity.Assets.Open(filename);
                using (var asset = Activity.Assets.Open(filename))
                {
                    var result = new MemoryStream();
                    await asset.CopyToAsync(result);
                    result.Seek(0, SeekOrigin.Begin);
                    return Tuple.Create<Stream, Encoding>(result, null);
                }
            }
            catch (Java.IO.FileNotFoundException)
            {
            }
            catch (FileNotFoundException)
            {
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