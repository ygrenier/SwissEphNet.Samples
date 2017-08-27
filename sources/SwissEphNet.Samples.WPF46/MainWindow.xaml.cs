using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SwissEphNet.Samples.WPF46
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pbProgress.Visibility = Visibility.Collapsed;
        }

        void RunTest()
        {
            var provider = new WpfTestProvider();
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }
        void RunTestLoadAsync()
        {
            var provider = new WpfTestProviderAsync();
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }
        async Task RunTestAsync()
        {
            var provider = new WpfTestProviderAsync();
            using (var swetest = new SwephTest(provider))
            {
                await Task.Run(() => swetest.RunTest());
                tbResult.Text = provider.OutputContent.ToString();
            }
        }

        private void StartRun()
        {
            btnRunTest.IsEnabled = false;
            btnRunTestLoadAsync.IsEnabled = false;
            btnRunTestAsync.IsEnabled = false;
            pbProgress.Visibility = Visibility.Visible;
            tbResult.Clear();
        }

        private void EndRun()
        {
            btnRunTest.IsEnabled = true;
            btnRunTestLoadAsync.IsEnabled = true;
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

        private void btnRunTestLoadAsync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartRun();
                RunTestLoadAsync();
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
