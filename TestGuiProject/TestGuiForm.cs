using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NAS.Tests
{
    public partial class TestGuiForm : Form
    {
        public TestGuiForm()
        {
            InitializeComponent();
        }

        private void lb_TestLabel_DoubleClick(object sender, EventArgs e)
        {
            lb_TestLabel.Text = TestLibProject.GetTestString("Double Clicked Label !!");
        }
    }
}
