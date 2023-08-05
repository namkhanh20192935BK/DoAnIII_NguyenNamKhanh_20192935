using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Quanlybaidoxethongminh
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        Modify modify = new Modify();

        private string phanQuyen;

        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = textBox_TenTaiKkhoan.Text;
            string matkhau = textBox_MatKhau.Text;
            if (tentk.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!"); 
                return;
            }
            else if (matkhau.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                return;
            }
            else
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\Database_QLBDX1.mdf;Integrated Security=True";
                string query = "Select * from QuanLyTaiKhoan where Username = '" + tentk + "'  and Password = '" + matkhau + "'";
                if (modify.TaiKhoans(query).Count != 0)
                { 
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            phanQuyen = reader.GetString(2);
                        }
                        Const.PhanQuyen = phanQuyen;
                        MessageBox.Show("Phân quyền : '" + phanQuyen + "' ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Hide();
                        FormLoadStart fl = new FormLoadStart();
                        fl.ShowDialog();
                        reader.Close();
                        connection.Close();
                    }
                    // Them Close vao de sau khi tat cua so thi Form sẽ dung hoat dong
                    Close();
                    return; 
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();  
            ForgetPassword forgetPassword = new ForgetPassword();
            forgetPassword.ShowDialog();
            Close();
        }
    }
}
