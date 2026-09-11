using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxZone.Services;

namespace TaxZone
{
    public partial class F_inspector : Form
    {
        public F_inspector()
        {
            InitializeComponent();
            TaxAutomationInspector.Start();
            //this.Close();
            Application.Exit();
        }
    }
}
