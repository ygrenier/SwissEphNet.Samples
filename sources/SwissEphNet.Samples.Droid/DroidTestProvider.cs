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

namespace SwissEphNet.Samples.Droid
{
    class DroidTestProvider : ITestProvider
    {
        public DroidTestProvider(Activity activity)
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

        public Stream LoadFile(string filename, out Encoding encoding)
        {
            encoding = null;
            try
            {
                filename = filename.Replace("[ephe]\\", "datas/");
                //return Activity.Assets.Open(filename);
                using (var asset = Activity.Assets.Open(filename))
                {
                    var result = new MemoryStream();
                    asset.CopyTo(result);
                    result.Seek(0, SeekOrigin.Begin);
                    return result;
                }
            }
            catch (Java.IO.FileNotFoundException)
            {
                return null;
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

    }
}