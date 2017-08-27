using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SwissEphNet.Samples.UWP
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            pbProgress.Visibility = Visibility.Collapsed;
        }

        void RunTest()
        {
            var provider = new UwpTestProvider();
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }
        async Task RunTestAsync()
        {
            var provider = new UwpTestProvider();
            using (var swetest = new SwephTest(provider))
            {
                await Task.Run(() => swetest.RunTest());
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
