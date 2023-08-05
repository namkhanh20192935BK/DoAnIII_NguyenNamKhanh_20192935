using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using System.Globalization;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Quanlybaidoxethongminh
{
    public partial class QLNV : Form
    {
        public QLNV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void QLNV_Load(object sender, EventArgs e)
        {
            timer1.Start();
            //Thiet lap ComboBox Gioi tinh
            comboBox_GT.Items.Add("Nam");
            comboBox_GT.Items.Add("Nữ");
            comboBox_GT.Items.Add("Giới tính khác");
            // Thiết lập mục mặc định được chọn
            comboBox_GT.SelectedIndex = 0;

            //Thiet lap comboBox Phan quyen

            comboBox_PhanQuyen.Items.Add("Nhân viên");
            comboBox_PhanQuyen.Items.Add("Quản lý");

            comboBox_PhanQuyen.SelectedIndex = 0;
            button_SaveFix.Enabled = false;
            dataGridView1.ReadOnly = true;

            QUANLYNHANVIENToolStripMenuItem.Enabled = false;

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

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        private const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\Database_QLBDX1.mdf;Integrated Security=True";
        // Chuỗi kết nối cơ sở dữ liệu
        private void button5_Click(object sender, EventArgs e)
        {
            // Tạo một OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

            // Mở hộp thoại chọn tệp tin
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn tệp tin đã chọn
                string filePath = openFileDialog.FileName;

                // Tải ảnh từ tệp tin
                Image image = Image.FromFile(filePath);

                // Gán ảnh cho PictureBox
                pictureBox_AnhTheNV.Image = image;
            }
        }
        public void deleteInfoNV()
        {
            textBox_email.Text = null;
            textBox_Hovaten.Text = null;
            textBox_DiaChi.Text = null;
            textBox_Hovaten.Text = null;
            textBox_Password.Text = null;
            textBox_Username.Text = null;
            textBox_SDT.Text = null;
            pictureBox_AnhTheNV.Image = null;

        }
        //Button Xóa dữ liệu người dùng database
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result_DeleteData = MessageBox.Show("Bạn có muốn xóa vĩnh viễn dữ liệu không?", "Xác nhận xóa", MessageBoxButtons.OKCancel);

            if (result_DeleteData == DialogResult.OK)
            {
                string query5 = "DELETE FROM QuanLyNhanVien WHERE Email = '" + textBox_email.Text + "'";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command5 = new SqlCommand(query5, connection);
                    command5.ExecuteNonQuery();
                    connection.Close();
                }

                string query6 = "DELETE FROM QuanLyTaiKhoan WHERE Username = '" + textBox_Username.Text + "'";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command6 = new SqlCommand(query6, connection);
                    command6.ExecuteNonQuery();
                    connection.Close();
                }
                deleteInfoNV();
            }
        }
        public void activeInfoNV()
        {
            //textBox_Hovaten.ReadOnly = false;
            textBox_email.ReadOnly = false;
            //textBox_SDT.ReadOnly = false;
            textBox_Username.ReadOnly = false;
            textBox_DiaChi.ReadOnly = false;
            //dateTimePicker_NgaySinh.Enabled = true;
            //comboBox_GT.Enabled = true;
            textBox_Password.ReadOnly = false;
            button_upload.Enabled = true;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            // Hiển thị MessageBox thông báo yêu cầu xác nhận xóa dữ liệu
            DialogResult result = MessageBox.Show("Bạn có chắc chắn nhập dữ liệu mới?", "Xác nhận", MessageBoxButtons.OKCancel);

            // Nếu người dùng nhấn OK
            if (result == DialogResult.OK)
            {
                activeInfoNV();
                textBox_Hovaten.ReadOnly = false;
                textBox_SDT.ReadOnly = false;
                dateTimePicker_NgaySinh.Enabled = true;
                comboBox_GT.Enabled = true;
                comboBox_PhanQuyen.Enabled = true;
                textBox_Password.ReadOnly = false;
                button_SaveFix.Enabled = false;
                button_SaveInfoNew.Enabled = true;
                // Thực hiện xóa dữ liệu ở đây
                textBox_Hovaten.Text = null;
                textBox_email.Text = null;
                textBox_DiaChi.Text = null;
                textBox_Password.Text = null;
                textBox_SDT.Text = null;
                textBox_Username.Text = null;
                pictureBox_AnhTheNV.Image = null;
            }
        }
        // Kiem tra emiail
        static bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            // Kiểm tra địa chỉ email với biểu thức chính quy
            bool isValid = Regex.IsMatch(email, pattern);

            return isValid;
        }

        //Kiem tra so dien thoai
        static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Kiểm tra độ dài của số điện thoại
            if (phoneNumber.Length != 10)
            {
                return false;
            }

            // Kiểm tra số đầu tiên bắt đầu bằng 0
            if (phoneNumber[0] != '0')
            {
                return false;
            }

            // Kiểm tra tất cả các ký tự còn lại là chữ số
            for (int i = 1; i < phoneNumber.Length; i++)
            {
                if (!char.IsDigit(phoneNumber[i]))
                {
                    return false;
                }
            }

            return true;


        }
        // Tach duoi email lay thanh Username
        static string GetUsernameFromEmail(string email)
        {
            int index = email.IndexOf('@');
            string username = email.Substring(0, index);
            return username;
        }
        // Random Password
        static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";

            StringBuilder passwordBuilder = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(0, validChars.Length);
                char randomChar = validChars[index];
                passwordBuilder.Append(randomChar);
            }

            return passwordBuilder.ToString();
        }

        //Button Luu thong tin
        string thongbao = "";
        private void button3_Click(object sender, EventArgs e)
        {
            if (CheckEmailData(textBox_email.Text))
            {
                thongbao += "Địa chỉ email đã được sử dụng.\n";
            }
            if (!IsValidEmail(textBox_email.Text) || textBox_email.Text == "")
            {
                thongbao += "Địa chỉ email không hợp lệ.\n";
            }
            if (CheckSDT(textBox_SDT.Text))
            {
                thongbao += "Số điện thoại đã được sử dụng.\n";
            }
            if (!IsValidPhoneNumber(textBox_SDT.Text) || textBox_SDT.Text == "")
            {
                thongbao += "Số điện thoại không hợp lệ.\n";
            }
            if (CheckUserName(textBox_Username.Text))
            {
                thongbao += "Username đã được sử dụng.\n";
            }
            if (textBox_DiaChi.Text == "")
            {
                thongbao += "Địa chỉ không hợp lệ.\n";
            }
            if (textBox_Hovaten.Text == "")
            {
                thongbao += "Họ và tên không hợp lệ.\n";
            }
            DateTime dateOfBirth = dateTimePicker_NgaySinh.Value;
            DateTime currentDate = DateTime.Now;

            // Tính khoảng cách thời gian giữa ngày sinh và ngày hiện tại
            TimeSpan ageDifference = currentDate - dateOfBirth;

            // Kiểm tra xem đã đủ 18 tuổi hay chưa
            if (!(ageDifference.TotalDays >= 365 * 18))
            {
                thongbao += "Ngày sinh không hợp lệ, người lao động cần từ đủ 18 tuổi.\n";
            }
            if (pictureBox_AnhTheNV.Image == null)
            {
                thongbao += "Vui lòng upload hình ảnh nhân viên.\n";
            }
            if (thongbao != "")
            {
                MessageBox.Show(thongbao + "\nVui lòng kiểm tra lại các thông tin!", "Thong bao");
                thongbao = "";
            }
            else
            {
                // Luu anh 
                string outputPath_ImageNV = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//Image_Personal";
                string fileName_LiscensePlate = "ImageNV_" + textBox_Username.Text + ".jpg";

                string outputFilePath_ImageNV = Path.Combine(outputPath_ImageNV, fileName_LiscensePlate);
                pictureBox_AnhTheNV.Image.Save(outputFilePath_ImageNV, System.Drawing.Imaging.ImageFormat.Jpeg);

                string query = "INSERT INTO QuanLyNhanVien (Email, TenNhanVien, SDT, Username, Dia_chi, Ngay_sinh, Gioi_tinh, Anh_NV) VALUES ('" + textBox_email.Text + "',N'" + textBox_Hovaten.Text + "', '" + textBox_SDT.Text + "','" + textBox_Username.Text + "',N'" + textBox_DiaChi.Text + "',N'" + dateTimePicker_NgaySinh.Value + "',N'" + comboBox_GT.Text + "','" + outputFilePath_ImageNV + "')";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.ExecuteScalar();
                    sqlConnection.Close();
                }

                string query2 = "INSERT INTO QuanLyTaiKhoan (Username, Password, Phan_quyen) VALUES ('" + textBox_Username.Text + "','" + textBox_Password.Text + "',N'" + comboBox_PhanQuyen.Text + "')";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command2 = new SqlCommand(query2, sqlConnection);
                    command2.ExecuteScalar();
                    sqlConnection.Close();
                }
                MessageBox.Show("Lưu thông tin thành công!", "Thông báo");

            }

        }

        // Buton tai du lieu tu QLNV len DatagridView
        private void button7_Click(object sender, EventArgs e)
        {
            // Tạo DataTable hoặc BindingSource để lưu dữ liệu
            System.Data.DataTable dataTable = new System.Data.DataTable();

            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection1.Open();

                // Truy vấn dữ liệu từ bảng
                string query1 = "SELECT * FROM QuanLyNhanVien"; // Thay TableName bằng tên bảng thực tế của bạn
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
            dataGridView1.DataSource = dataTable;
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button_SaveFix.Enabled = false;
            button_SaveInfoNew.Enabled = false;
            // Kiểm tra xem người dùng đã click vào một dòng hợp lệ trong DataGridView
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Lấy dữ liệu từ các cột trong dòng được chọn
                string value_email = selectedRow.Cells["Email"].Value.ToString();
                string value_tennv = selectedRow.Cells["TenNhanVien"].Value.ToString();
                string value_sdt = selectedRow.Cells["SDT"].Value.ToString();
                string value_username = selectedRow.Cells["Username"].Value.ToString();
                string value_diachi = selectedRow.Cells["Dia_chi"].Value.ToString();
                string value_ngaysinh = selectedRow.Cells["Ngay_sinh"].Value.ToString();
                string value_gioitinh = selectedRow.Cells["Gioi_tinh"].Value.ToString();
                string value_anhNV = selectedRow.Cells["Anh_NV"].Value.ToString();

                tg_anhNV = value_anhNV;
                // Hiển thị dữ liệu ở mục thông tin nhân viên
                textBox_Hovaten.Text = value_tennv;
                textBox_email.Text = value_email;
                textBox_SDT.Text = value_sdt;
                textBox_Username.Text = value_username;
                textBox_DiaChi.Text = value_diachi;
                dateTimePicker_NgaySinh.Text = value_ngaysinh;
                comboBox_GT.Text = value_gioitinh;

                //Khong cho phep chinh sua thong tin
                textBox_email.ReadOnly = true;
                textBox_SDT.ReadOnly = true;
                textBox_Username.ReadOnly = true;
                textBox_DiaChi.ReadOnly = true;
                textBox_Hovaten.ReadOnly = true;
                dateTimePicker_NgaySinh.Enabled = false;
                comboBox_GT.Enabled = false;
                button_upload.Enabled = false;
                textBox_Password.ReadOnly = true;
                comboBox_PhanQuyen.Enabled = false;

                ////Hiển thị ảnh
                Image image_NV = Image.FromFile(value_anhNV);

                //// Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                pictureBox_AnhTheNV.Image = image_NV;

                string query2 = "SELECT * from QuanLyTaiKhoan where Username = '" + value_username + "'";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command2 = new SqlCommand(query2, sqlConnection);
                    SqlDataReader reader = command2.ExecuteReader();
                    while (reader.Read())
                    {
                        textBox_Password.Text = reader["Password"].ToString();
                        comboBox_PhanQuyen.Text = reader["Phan_quyen"].ToString();
                    }
                    sqlConnection.Close();
                }
            }
        }

        private void textBox_Username_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Password_TextChanged(object sender, EventArgs e)
        {

        }
        string tg_email;
        string tg_sdt;
        string tg_username;
        string tg_password;
        string tg_date;
        string tg_diachi;
        string tg_gioitinh;
        string tg_phanquyen;
        string tg_anhNV;
        private void button_FixInfo_Click(object sender, EventArgs e)
        {
            DialogResult result_FixData = MessageBox.Show("Bạn có muốn update dữ liệu không?", "Xác nhận", MessageBoxButtons.OKCancel);
            if (result_FixData == DialogResult.OK)
            {
                SecurityData.checkCorrect = false;
                Security_Data security_Data = new Security_Data();
                security_Data.ShowDialog();
                if (SecurityData.checkCorrect == true)
                {
                    activeInfoNV();
                    button_SaveInfoNew.Enabled = false;
                    button_SaveFix.Enabled = true;
                    comboBox_PhanQuyen.Enabled = true;
                    tg_email = textBox_email.Text;
                    tg_sdt = textBox_SDT.Text;
                    tg_username = textBox_Username.Text;
                    tg_password = textBox_Password.Text;
                    tg_date = dateTimePicker_NgaySinh.Text;
                    tg_diachi = textBox_DiaChi.Text;
                    tg_gioitinh = comboBox_GT.Text;
                    tg_phanquyen = comboBox_PhanQuyen.Text;

                }
            }

        }

        private void button_CreateTK_Click(object sender, EventArgs e)
        {
            if (textBox_email.Text == "")
            {
                MessageBox.Show("Vui lòng hoàn thiện các thông tin phía trên!", "Thông báo");
            }
            else
            {
                textBox_Password.Text = GenerateRandomPassword(10);
                textBox_Username.Text = GetUsernameFromEmail(textBox_email.Text);
            }
        }

        private void textBox_email_TextChanged(object sender, EventArgs e)
        {

        }
        private bool CheckEmailData(string email)
        {
            string query = "SELECT * from QuanLyNhanVien where Email = '" + email + "'";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader_email = command.ExecuteReader();
                if (reader_email.Read())
                {
                    sqlConnection.Close();
                    return true;
                }
                sqlConnection.Close();
                return false;

            }
        }

        private bool CheckUserName(string userName)
        {
            string query = "SELECT * from QuanLyNhanVien where Username = '" + userName + "'";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader_username = command.ExecuteReader();
                if (reader_username.Read())
                {
                    sqlConnection.Close();
                    return true;
                }
                sqlConnection.Close();
                return false;

            }
        }
        private bool CheckSDT(string sdt)
        {
            string query = "SELECT * from QuanLyNhanVien where SDT = '" + sdt + "'";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader_username = command.ExecuteReader();
                if (reader_username.Read())
                {
                    sqlConnection.Close();
                    return true;
                }
                sqlConnection.Close();
                return false;

            }
        }
        private void NonActiveInfo()
        {
            textBox_email.ReadOnly = true;
            textBox_SDT.ReadOnly = true;
            textBox_Username.ReadOnly = true;
            textBox_DiaChi.ReadOnly = true;
            textBox_Hovaten.ReadOnly = true;
            dateTimePicker_NgaySinh.Enabled = false;
            comboBox_GT.Enabled = false;
            button_upload.Enabled = false;
            textBox_Password.ReadOnly = true;
            comboBox_PhanQuyen.Enabled = false;
        }
        int count;
        private void button_SaveFix_Click(object sender, EventArgs e)
        {
            if (CheckEmailData(textBox_email.Text) && textBox_email.Text != tg_email)
            {
                thongbao += "Địa chỉ email đã được sử dụng, vui lòng sử dụng email khác!\n";
            }
            if (CheckUserName(textBox_Username.Text) && textBox_Username.Text != tg_username)
            {
                thongbao += "Địa chỉ username đã được sử dụng, vui lòng sử dụng username khác!\n";
            }
            if (CheckUserName(textBox_SDT.Text) && textBox_SDT.Text != tg_sdt)
            {
                thongbao += "Số điện thoại đã được sử dụng, vui lòng sử dụng username khác!\n";
            }
            if (thongbao != "")
            {
                MessageBox.Show(thongbao + "\nVui lòng kiểm tra lại thông tin!", "Thông báo");
                thongbao = "";
            }
            else if (textBox_email.Text == tg_email && textBox_Username.Text == tg_username && textBox_SDT.Text == tg_sdt)
            {
                ++count;
                string outputPath_ImageNV = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//Image_Personal";
                string fileName_LiscensePlate = "ImageNV" + count + "_" + textBox_Username.Text + ".jpg";

                string outputFilePath_ImageNV = Path.Combine(outputPath_ImageNV, fileName_LiscensePlate);
                pictureBox_AnhTheNV.Image.Save(outputFilePath_ImageNV, System.Drawing.Imaging.ImageFormat.Jpeg);

                string query7 = "UPDATE QuanLyNhanVien SET Dia_chi = N'" + textBox_DiaChi.Text + "', Anh_NV = '" + outputFilePath_ImageNV + "' WHERE TenNhanVien = N'" + textBox_Hovaten.Text + "' and SDT = '" + textBox_SDT.Text + "'";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command7 = new SqlCommand(query7, sqlConnection);
                    command7.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                string query8 = "UPDATE QuanLyTaiKhoan SET Password = '" + textBox_Password.Text + "', Phan_quyen = N'" + comboBox_PhanQuyen.Text + "' WHERE Username = '" + tg_username + "'";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command8 = new SqlCommand(query8, sqlConnection);
                    command8.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                MessageBox.Show("Dữ liệu đã được cập nhật thành công", "Thông báo");
                button_SaveFix.Enabled = false;
                NonActiveInfo();
            }
            else
            {
                // Luu anh 
                ++count;
                string outputPath_ImageNV = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//Image_Personal";
                string fileName_LiscensePlate = "ImageNV" + count + "_" + textBox_Username.Text + ".jpg";

                string outputFilePath_ImageNV = Path.Combine(outputPath_ImageNV, fileName_LiscensePlate);
                pictureBox_AnhTheNV.Image.Save(outputFilePath_ImageNV, System.Drawing.Imaging.ImageFormat.Jpeg);
                string query7 = "UPDATE QuanLyNhanVien SET Email = '" + textBox_email.Text + "', Username = '" + textBox_Username.Text + "', Dia_chi = N'" + textBox_DiaChi.Text + "', Anh_NV = '" + outputFilePath_ImageNV + "'WHERE TenNhanVien = N'" + textBox_Hovaten.Text + "' and SDT = '" + textBox_SDT.Text + "'";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command7 = new SqlCommand(query7, sqlConnection);
                    command7.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                string query8 = "UPDATE QuanLyTaiKhoan SET Username = '" + textBox_Username.Text + "', Password = '" + textBox_Password.Text + "', Phan_quyen = N'" + comboBox_PhanQuyen.Text + "' WHERE Username = '" + tg_username + "'";
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand command8 = new SqlCommand(query8, sqlConnection);
                    command8.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                MessageBox.Show("Dữ liệu đã được cập nhật thành công", "Thông báo");
                button_SaveFix.Enabled = false;
                NonActiveInfo();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now.Add(new TimeSpan());
            label_time.Text = String.Format("{0:hh:mm:ss tt}", dt);
            label_date.Text = String.Format("{0:dd/MM/yyyy}", dt);
        }

        private void qUANLYKHACHHANGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLKH qLKH = new QLKH();
            qLKH.ShowDialog();

            Hide();
        }

        private void tRANGCHUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.ShowDialog();

            Hide();
        }

        private void THONGKE_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongKe thongKe = new ThongKe();
            thongKe.ShowDialog();

            Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dataGridView_Excel.Rows.Clear();
            OpenFileDialog openFD = new OpenFileDialog();
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorksheet;
            Microsoft.Office.Interop.Excel.Range xlRange;

            int xlRow;
            string strFileName;

            openFD.Filter = "Excel Office|*.xls; *xlsx";
            openFD.ShowDialog();

            strFileName = openFD.FileName;

            if (strFileName != "")
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(strFileName);
                xlWorksheet = xlWorkbook.Worksheets["Sheet1"];
                xlRange = xlWorksheet.UsedRange;

                int i = 0;

                for (xlRow = 2; xlRow <= xlRange.Rows.Count; xlRow++)
                {
                    if (xlRange.Cells[xlRow, 1].Text != "")
                    {
                        i++;
                        dataGridView_Excel.Rows.Add(i, xlRange.Cells[xlRow, 1].Text, xlRange.Cells[xlRow, 2].Text, xlRange.Cells[xlRow, 3].Text, xlRange.Cells[xlRow, 4].Text, xlRange.Cells[xlRow, 5].Text, xlRange.Cells[xlRow, 6].Text, xlRange.Cells[xlRow, 7].Text, xlRange.Cells[xlRow, 8].Text);
                    }

                }
                xlWorkbook.Close();
                xlApp.Quit();
            }
        }

        private void dataGridView_Excel_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            activeInfoNV();
            dateTimePicker_NgaySinh.Enabled = true;
            comboBox_GT.Enabled = true;
            comboBox_PhanQuyen.Enabled = true;
            button_SaveFix.Enabled = false;
            textBox_Hovaten.ReadOnly = false;
            textBox_SDT.ReadOnly = false;
            button_SaveInfoNew.Enabled = true;
            // Kiểm tra xem người dùng đã click vào một dòng hợp lệ trong DataGridView_Excel 
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView_Excel.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView_Excel.Rows[e.RowIndex];

                // Lấy dữ liệu từ các cột trong dòng được chọn
                string email = selectedRow.Cells[1].Value.ToString();
                string tennv = selectedRow.Cells[2].Value.ToString();
                string sdt = selectedRow.Cells[3].Value.ToString();
                string diachi = selectedRow.Cells[4].Value.ToString();
                string ngaysinh = selectedRow.Cells[5].Value.ToString();
                string gioitinh = selectedRow.Cells[6].Value.ToString();
                string anhNV = selectedRow.Cells[7].Value.ToString();

                // Hiển thị dữ liệu ở mục thông tin nhân viên
                textBox_Hovaten.Text = tennv;
                textBox_email.Text = email;
                textBox_SDT.Text = sdt;
                textBox_DiaChi.Text = diachi;
                DateTime date = DateTime.ParseExact(ngaysinh, "MM/dd/yyyy", null);
                dateTimePicker_NgaySinh.Value = date;
                comboBox_GT.Text = gioitinh;

                //Khong cho phep chinh sua thong tin
                //textBox_email.ReadOnly = true;
                //textBox_SDT.ReadOnly = true;
                //textBox_Username.ReadOnly = true;
                //textBox_DiaChi.ReadOnly = true;
                //textBox_Hovaten.ReadOnly = true;
                //dateTimePicker_NgaySinh.Enabled = false;
                //comboBox_GT.Enabled = false;
                //button_upload.Enabled = false;
                //textBox_Password.ReadOnly = true;
                //comboBox_PhanQuyen.Enabled = false;

                ////Hiển thị ảnh
                Image image_NV = Image.FromFile(anhNV);

                //// Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                pictureBox_AnhTheNV.Image = image_NV;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
