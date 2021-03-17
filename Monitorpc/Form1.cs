using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Monitorpc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            float ram = pcRAM.NextValue();
            pbRAM.Value = (int)ram;
            lblRAM.Text = string.Format($"% {ram}");

            float cpu = pcCPU.NextValue();
            pbCPU.Value = (int)cpu;
            lblCPU.Text = string.Format($"% {cpu}");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
