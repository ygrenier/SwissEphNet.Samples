using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwissEphNet.Samples.WinForms46
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pbProgress.Visible = false;
        }

        void RunTest()
        {
            var provider = new WinFormsTestProvider();
            using (var swetest = new SwephTest(provider))
            {
                swetest.RunTest();
                tbResult.Text = provider.OutputContent.ToString();
            }
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            try
            {
                btnRunTest.Enabled = false;
                pbProgress.Visible = true;
                tbResult.Clear();
                RunTest();
            }
            finally
            {
                btnRunTest.Enabled = true;
                pbProgress.Visible = false;
            }
        }
    }
}
