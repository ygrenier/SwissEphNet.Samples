using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;

namespace SwissEphNet.Samples.Droid
{
    [Activity(Label = "Swiss Ephemeris Tests", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnRunTest, btnRunTestAsync, btnRunTestLoadAsync;
        ProgressBar pbProgressBar;
        TextView tbResult;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            pbProgressBar = FindViewById<ProgressBar>(Resource.Id.pbProgressBar);
            btnRunTest = FindViewById<Button>(Resource.Id.btnRunTest);
            btnRunTestLoadAsync = FindViewById<Button>(Resource.Id.btnRunTestLoadAsync);
            btnRunTestAsync = FindViewById<Button>(Resource.Id.btnRunTestAsync);
            tbResult = FindViewById<TextView>(Resource.Id.tbResult);

            pbProgressBar.Visibility = Android.Views.ViewStates.Invisible;
            btnRunTest.Click += btnRunTest_Click;
            btnRunTestLoadAsync.Click += btnRunTestLoadAsync_Click;
            btnRunTestAsync.Click += btnRunTestAsync_Click;
        }

        private void btnRunTest_Click(object sender, System.EventArgs e)
        {
            try
            {
                StartRun();
                RunTest();
            }
            catch (Java.Lang.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                EndRun();
            }
        }

        private void btnRunTestLoadAsync_Click(object sender, System.EventArgs e)
        {
            try
            {
                StartRun();
                RunTestLoadAsync();
            }
            catch (Java.Lang.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                EndRun();
            }
        }

        private async void btnRunTestAsync_Click(object sender, System.EventArgs e)
        {
            try
            {
                StartRun();
                await RunTestAsync();
            }
            catch (Java.Lang.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                EndRun();
            }
        }

        private void StartRun()
        {
            btnRunTest.Enabled = false;
            btnRunTestLoadAsync.Enabled = false;
            btnRunTestAsync.Enabled = false;
            pbProgressBar.Visibility = Android.Views.ViewStates.Visible;
            tbResult.Text = string.Empty;
        }

        private void EndRun()
        {
            btnRunTest.Enabled = true;
            btnRunTestLoadAsync.Enabled = true;
            btnRunTestAsync.Enabled = true;
            pbProgressBar.Visibility = Android.Views.ViewStates.Invisible;
        }

        void RunTest()
        {
            var provider = new DroidTestProvider(this);
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }
        void RunTestLoadAsync()
        {
            var provider = new DroidTestProviderAsync(this);
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }
        async Task RunTestAsync()
        {
            var provider = new DroidTestProviderAsync(this);
            using (var swetest = new SwephTest(provider))
            {
                await Task.Run(() => swetest.RunTest());
                tbResult.Text = provider.OutputContent.ToString();
            }
        }

    }
}

