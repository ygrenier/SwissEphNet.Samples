using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SwissEphNet.Samples.Universal
{
    partial class MainPage
    {

        void RunTest()
        {
            var provider = new UniversalTestProvider();
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }
        async Task RunTestAsync()
        {
            var provider = new UniversalTestProvider();
            using (var swetest = new SwephTest(provider))
            {
                await Task.Run(() => swetest.RunTest());
                await Task.Delay(1000);
                tbResult.Text = provider.OutputContent.ToString();
            }
        }

        private void StartRun()
        {
            btnRunTest.IsEnabled = false;
            btnRunTestAsync.IsEnabled = false;
            pbProgress.Visibility = Visibility.Visible;
            tbResult.Text = string.Empty;
        }

        private void EndRun()
        {
            btnRunTest.IsEnabled = true;
            btnRunTestAsync.IsEnabled = true;
            pbProgress.Visibility = Visibility.Collapsed;
        }

        private void btnRunTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartRun();
                RunTest();
            }
            finally
            {
                EndRun();
            }
        }

        private async void btnRunTestAsync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartRun();
                await RunTestAsync();
            }
            finally
            {
                EndRun();
            }
        }
    }
}
