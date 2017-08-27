using System;
using System.Collections.Generic;
using System.Text;

namespace SwissEphNet.Samples
{
    public class SwephTest : IDisposable
    {
        public SwephTest(ITestProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            Provider = provider;
            SwissEph = new SwissEph();
            SwissEph.OnLoadFile += SwissEph_OnLoadFile;
        }

        public void Dispose()
        {
            SwissEph.Dispose();
        }

        private void SwissEph_OnLoadFile(object sender, LoadFileEventArgs e)
        {
            //e.File = FileLoader?.Invoke(e.FileName);
            //Debug.WriteLine($"Required file:{e.FileName} => {(e.File != null ? "OK" : "Not found")}");
        }

        public TestResult RunTest(TestDefinition test = null)
        {
            if (test == null) test = new TestDefinition();
            var result = new TestResult();
            return result;
        }

        public SwissEph SwissEph { get; private set; }

        public ITestProvider Provider { get; set; }

    }
}
