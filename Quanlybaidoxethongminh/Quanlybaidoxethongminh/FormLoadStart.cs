using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlybaidoxethongminh
{
    public partial class FormLoadStart : Form
    {
        private Timer timer;
        public FormLoadStart()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 5000; // 5000ms = 5 giây
            timer.Tick += timer1_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer.Stop(); // Dừng đếm thời gian
            this.Close(); // Đóng Form 1
            this.Hide();
            Home form2 = new Home();
            form2.ShowDialog();

            return;
        }

        private void FormLoadStart_Load(object sender, EventArgs e)
        {
            timer.Start();
        }
    }
}
