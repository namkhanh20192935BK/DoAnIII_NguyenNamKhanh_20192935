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
    public partial class Security_Data : Form
    {
        public Security_Data()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        string password = "88888888";
        private void Security_Data_Load(object sender, EventArgs e)
        {

        }
        
        private void button_checkCode_Click(object sender, EventArgs e)
        {
            if (textBox_inputPassword.Text == password)
            {
                SecurityData.checkCorrect = true;
                MessageBox.Show("Xác thực thành công!", "Thông báo", MessageBoxButtons.OK);
                this.Close();
                return;
            }
            else
            {
                SecurityData.checkCorrect = false;
                MessageBox.Show("Xác thực không thành công! Vui lòng thử lại!", "Thông báo",MessageBoxButtons.OK);
            }
        }
    }
}
