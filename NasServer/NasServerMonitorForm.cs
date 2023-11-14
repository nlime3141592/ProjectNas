using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAS;

namespace NAS.Server
{
    public partial class NasServerMonitorForm : Form
    {
        public NasServerMonitorForm()
        {
            InitializeComponent();
        }

        private void menuItem_FileBrowser_Click(object sender, EventArgs e)
        {
            FileBrowserForm.GetInstance().Show();
        }
    }
}
