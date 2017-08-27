using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SwissEphNet.Samples.Universal
{
    class UniversalTestProvider : ITestProvider
    {
        public UniversalTestProvider()
        {
            OutputContent = new StringBuilder();
            Output = new StringWriter(OutputContent);
            Debug = Output;
        }

        public StringBuilder OutputContent { get; private set; }

        public TextWriter Debug { get; private set; }

        public TextWriter Output { get; private set; }

        IList<string> assets = null;
        private async Task<Tuple<Stream, Encoding>> FileLoaderAsync(string filename)
        {
            try
            {
                filename = filename.Replace("[ephe]\\", "");
                var assetsDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\datas");
                if (assets == null)
                {
                    assets = (await assetsDirectory.GetItemsAsync()).OfType<StorageFile>().Select(f => f.Name).ToList();
                }
                if (!assets.Any(f => f == filename)) return null;
                return Tuple.Create<Stream, Encoding>(await assetsDirectory.OpenStreamForReadAsync(filename), null);
            }
            catch (FileNotFoundException)
            {
                return Tuple.Create<Stream, Encoding>(null, null);
            }
        }

        public Stream LoadFile(string filename, out Encoding encoding)
        {
            var res = Task.Run(async () => await FileLoaderAsync(filename)).Result;
            encoding = res?.Item2;
            return res?.Item1;
        }
    }

}
