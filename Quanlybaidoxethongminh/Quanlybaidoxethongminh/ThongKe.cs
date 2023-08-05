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
using static IronPython.Modules.PythonCsvModule;

namespace Quanlybaidoxethongminh
{
    public partial class ThongKe : Form
    {
        public ThongKe()
        {
            InitializeComponent();
        }

        private const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\Database_QLBDX1.mdf;Integrated Security=True";

        private void button1_Click(object sender, EventArgs e)
        {
            // Tạo DataTable hoặc BindingSource để lưu dữ liệu
            System.Data.DataTable dataTable = new System.Data.DataTable();

            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection1.Open();

                // Truy vấn dữ liệu từ bảng
                string query1 = "SELECT * FROM QuanLyVao"; // Thay TableName bằng tên bảng thực tế của bạn
                using (SqlCommand command1 = new SqlCommand(query1, connection1))
                {
                    // Đọc dữ liệu từ SqlDataReader
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        // Đổ dữ liệu từ SqlDataReader vào DataTable
                        dataTable.Load(reader);
                    }
                }
            }

            // Thiết lập nguồn dữ liệu cho DataGridView
            dataGridView_LaneIn.DataSource = dataTable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Tạo DataTable hoặc BindingSource để lưu dữ liệu
            System.Data.DataTable dataTable = new System.Data.DataTable();

            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection1.Open();

                // Truy vấn dữ liệu từ bảng
                string query1 = "SELECT * FROM QuanLyRa"; // Thay TableName bằng tên bảng thực tế của bạn
                using (SqlCommand command1 = new SqlCommand(query1, connection1))
                {
                    // Đọc dữ liệu từ SqlDataReader
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {
                        // Đổ dữ liệu từ SqlDataReader vào DataTable
                        dataTable.Load(reader);
                    }
                }
            }

            // Thiết lập nguồn dữ liệu cho DataGridView
            dataGridView_LaneOut.DataSource = dataTable;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now.Add(new TimeSpan());
            label_time.Text = String.Format("{0:hh:mm:ss tt}", dt);
            label_date.Text = String.Format("{0:dd/MM/yyyy}", dt);
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {
            timer1.Start();

            dataGridView_LaneIn.ReadOnly = true;
            dataGridView_LaneOut.ReadOnly = true;  
            THONGKE_toolStripMenuItem.Enabled = false;

            comboBox_Search.Items.Add("Tất cả");
            comboBox_Search.Items.Add("Xe chưa ra");
            comboBox_Search.Items.Add("Xe vào ra trong ngày");

            comboBox_Search.SelectedIndex = 0;

            string query_count_in = "SELECT Count(Bien_so) from QuanLyVao";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query_count_in, sqlConnection);
                int count_in = (int)sqlCommand.ExecuteScalar();
                LabelCount_In.Text = count_in.ToString();
                sqlConnection.Close();
            }
            string query_count_out = "SELECT Count(Bien_so) from QuanLyRa";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query_count_out, sqlConnection);
                int count_out = (int)sqlCommand.ExecuteScalar();
                LabelCount_out.Text = count_out.ToString();
                sqlConnection.Close();
            }

            labelCount_present.Text = (int.Parse(LabelCount_In.Text) - int.Parse(LabelCount_out.Text)).ToString();
        }
        private void DeleteLaneOut()
        {
            textBox_ticketlaneout.Text = null;
            textBox_timeout.Text = null;
            textBox_dateout.Text = null;
            textBox_BSout.Text = null;
            textBox_LXout.Text = null;
            textBox_lrout.Text = null;
            textBox_ticketCost.Text = null;
            pictureBox_BSlaneout.Image = null;
            pictureBox_FClaneout.Image = null;
        }
        private void dataGridView_LaneIn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DeleteLaneOut();
            // Kiểm tra xem người dùng đã click vào một dòng hợp lệ trong DataGridView
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView_LaneIn.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView_LaneIn.Rows[e.RowIndex];

                // Lấy dữ liệu từ các cột trong dòng được chọn
                string value_ticket = selectedRow.Cells["Ma_ve"].Value.ToString();
                string value_giovao = selectedRow.Cells["Gio_vao"].Value.ToString();
                string value_ngayvao = selectedRow.Cells["Ngay_vao"].Value.ToString();
                string value_bienso = selectedRow.Cells["Bien_so"].Value.ToString();
                string value_loaixe = selectedRow.Cells["Loai_xe"].Value.ToString();
                string value_luotvao = selectedRow.Cells["Luot_vao"].Value.ToString();
                string value_habxv = selectedRow.Cells["Hinh_anh_Bien_so_vao"].Value.ToString();
                string value_hatxv = selectedRow.Cells["Hinh_anh_toan_xe_vao"].Value.ToString();

                // Hiển thị dữ liệu ở mục thông tin nhân viên
                textBox_ticketlanein.Text = value_ticket;
                textBox_timelanin.Text = value_giovao;
                textBox_datelanin.Text = value_ngayvao;
                textBox_BSlanein.Text = value_bienso;
                textBox_Lxlanein.Text = value_loaixe;
                textBox_lvlanein.Text = value_luotvao;

                ////Hiển thị ảnh biển xe vào
                Image image_habxv = Image.FromFile(value_habxv);

                //// Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                pictureBox_BSlanein.Image = image_habxv;

                ////Hiển thị ảnh biển xe ra
                Image image_hatxv = Image.FromFile(value_hatxv);

                //// Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                pictureBox_FClanein.Image = image_hatxv;
                string query2 = "SELECT * from QuanLyRa where Bien_so = '" + value_bienso + "' and Luot_ra ='" + value_luotvao + "'";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query2, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        textBox_ticketlaneout.Text=reader.GetString(0);
                        textBox_timeout.Text=reader.GetString(1);
                        textBox_dateout.Text=reader.GetString(2);
                        textBox_BSout.Text=reader.GetString(3);
                        textBox_LXout.Text=reader.GetString(4);
                        int lrout = reader.GetInt32(5);
                        textBox_lrout.Text = lrout.ToString();
                        int ticketCost=reader.GetInt32(6);
                        textBox_ticketCost.Text = ticketCost.ToString();


                        Image image_habsxr = Image.FromFile(reader.GetString(7));
                        pictureBox_BSlaneout.Image = image_habsxr;

                        Image image_hatxr =Image.FromFile(reader.GetString(8));
                        pictureBox_FClaneout.Image = image_hatxr;
                    }
                    sqlConnection.Close();
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void tRANGCHUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.ShowDialog();

            Hide();
            this.Close();
        }

        private void QUANLYNHANVIENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLNV qLNV = new QLNV();
            qLNV.ShowDialog();

            Hide();
            this.Close();
        }

        private void qUANLYKHACHHANGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLKH qLKH = new QLKH();
            qLKH.ShowDialog();

            Hide();
            this.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string query_count_in = "SELECT Count(Bien_so) from QuanLyVao";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query_count_in, sqlConnection);
                int count_in = (int)sqlCommand.ExecuteScalar();
                LabelCount_In.Text = count_in.ToString();
                sqlConnection.Close();
            }
            string query_count_out = "SELECT Count(Bien_so) from QuanLyRa";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query_count_out, sqlConnection);
                int count_out = (int)sqlCommand.ExecuteScalar();
                LabelCount_out.Text = count_out.ToString();
                sqlConnection.Close();
            }

            labelCount_present.Text = (int.Parse(LabelCount_In.Text) - int.Parse(LabelCount_out.Text)).ToString();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }
    }
}

