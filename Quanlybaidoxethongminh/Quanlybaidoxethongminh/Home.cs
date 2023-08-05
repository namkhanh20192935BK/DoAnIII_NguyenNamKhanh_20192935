using System;
using System.IO;
using System.Net;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using static IronPython.SQLite.PythonSQLite;
using System.Reflection.Emit;
using System.Collections;
using System.Net.NetworkInformation;
using static IronPython.Modules.PythonDateTime;
using static IronPython.Modules.PythonIterTools;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Quanlybaidoxethongminh
{

    public partial class Home : Form
    {
        private FilterInfoCollection cameras;
        private VideoCaptureDevice cam;
        public Home()
        {
            InitializeComponent();

            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in cameras)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;

            // Thêm các mục vào ComboBox
            comboBox2.Items.Add("Xe may");
            comboBox2.Items.Add("O to");
            comboBox2.Items.Add("Xe dap");

            // Thiết lập mục mặc định được chọn
            comboBox2.SelectedIndex = 0;

            // Khởi tạo Timer và cài đặt Interval (5 giây)
            timer2 = new Timer();
            timer2.Interval = 5000; // 5 giây
            timer2.Enabled = true;

            // Gắn sự kiện Tick cho Timer
            timer2.Tick += timer2_Tick;
        }
        //private void Home_SizeChanged(object sender, EventArgs e)
        //{
        //    // Thay đổi kích thước của picture box theo kích thước mới của form
        //    pictureBox_Camera.Width = this.ClientSize.Width - 500; // Điều chỉnh kích thước tùy ý
        //    pictureBox_Camera.Height = this.ClientSize.Height - 500; // Điều chỉnh kích thước tùy ý
        //}
        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pictureBox_Camera.Image = bitmap;
        }
        private const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\Database_QLBDX1.mdf;Integrated Security=True";

        private void Home_Load(object sender, EventArgs e)
        {
            // Thay đổi kích thước của picture box theo kích thước mới của form
            timer1.Start();
            tRANGCHUToolStripMenuItem.Enabled = false;
            if (Const.PhanQuyen == "Nhân viên")
            {
                QUANLYNHANVIENToolStripMenuItem.Enabled = false;
                tHONGKEDOANHTHUToolStripMenuItem.Enabled = false;
            }

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
        // Ham goi HTTP Get len server
        public string sendGet(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        // Ham chuyen Image thanh Base 64
        public static string ConvertImageToBase64String(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        // Ham convert B64 de gui len server
        private String EscapeData(String B64)
        {
            int B64_length = B64.Length;
            if (B64_length <= 32000)
            {
                return Uri.EscapeDataString(B64);
            }


            int idx = 0;
            StringBuilder builder = new StringBuilder();
            String substr = B64.Substring(idx, 32000);
            while (idx < B64_length)
            {
                builder.Append(Uri.EscapeDataString(substr));
                idx += 32000;

                if (idx < B64_length)
                {

                    substr = B64.Substring(idx, Math.Min(32000, B64_length - idx));
                }

            }
            return builder.ToString();

        }

        // Ham goi HTTP POST len server de detect
        private String sendPOST(String url, String B64)
        {
            try
            {

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;
                var postData = "image=" + EscapeData(B64);

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            }
            catch (Exception ex)
            {
                return "Exception" + ex.ToString();
            }
        }

        private void THỐNGKÊToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongKe thongKe = new ThongKe();
            thongKe.ShowDialog();

            Hide();
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        Modify modify = new Modify();
        public int countClick = 0;
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            ++countClick;
            if (countClick == 1)
            {
                //GIAO TIẾP VỚI SERVER//
                /////////////////////////////////////////////////////
                // Ghi lại thời gian vào
                labelTime_in.Text = DateTime.Now.ToString("HH:mm:ss");

                // Ghi lại ngày vào
                labelDate_in.Text = DateTime.Now.ToString("dd/MM/yyyy");

                //// Chụp ảnh và lưu vào biến image
                //var image = CaptureImage();

                //// Hiển thị ảnh lên pictureBox
                //pictureBox_Lane_in.Image = image;

                // Doc du lieu cac ten class tu file yolov3.txt 
                string[] lines = File.ReadAllLines("C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\yolov3-tiny.txt");

                // Convert image to B64
                String B64 = ConvertImageToBase64String(pictureBox_Lane_in.Image);
                // Goi len server va tra ve ket qua
                String server_ip = "127.0.0.1";
                String server_path = "http://" + server_ip + ":8000/detect";
                String retStr = sendPOST(server_path, B64);


                // Ve cac khung chu nhat va ten class len anh 
                Graphics newGraphics = Graphics.FromImage(pictureBox_Lane_in.Image);

                String[] items = retStr.Split('|');
                for (int idx = 0; idx < items.Length - 1; idx++)
                {
                    String[] val = items[idx].Split(',');
                    // Draw it
                    Pen blackPen = new Pen(System.Drawing.Color.Blue, 2);

                    // Create rectangle.
                    Rectangle rect = new Rectangle(int.Parse(val[1]), int.Parse(val[2]), int.Parse(val[3]), int.Parse(val[4]));

                    // Draw rectangle to screen.
                    newGraphics.DrawRectangle(blackPen, rect);
                    PointF location0 = new PointF(rect.X, rect.Y - 12);
                    newGraphics.DrawString(lines[int.Parse(val[0])], new System.Drawing.Font("Tahoma", 8), Brushes.Red, location0);

                    // Input string to screen
                    string name_liscense_plate = val[5];
                    System.Drawing.Font font = new System.Drawing.Font("Tahoma", 8);
                    Brush brush = Brushes.Red;
                    PointF location = new PointF(rect.X, rect.Y - 2 * font.Height);
                    newGraphics.DrawString(name_liscense_plate, font, brush, location);

                    label_LicensePlate.Text = name_liscense_plate;

                    labelTicket_code.Text = Regex.Replace(name_liscense_plate, @"[-.]", "");

                    // Tạo một đối tượng Bitmap từ hình ảnh gốc
                    Bitmap originalImage = new Bitmap(original: pictureBox_Lane_in.Image);

                    // Tọa độ cắt
                    int x = int.Parse(val[1]);
                    int y = int.Parse(val[2]);
                    int width = int.Parse(val[3]);
                    int height = int.Parse(val[4]);

                    // Tạo một hình ảnh mới từ phần được cắt
                    Bitmap croppedImage = originalImage.Clone(new Rectangle(x, y, width, height), originalImage.PixelFormat);

                    // Hiển thị hình ảnh đã cắt trong một PictureBox
                    pictureBox_Crop.Image = croppedImage;
                    //pictureBox_Crop.SizeMode = PictureBoxSizeMode.AutoSize;
                }
                pictureBox_Lane_in.Refresh();
                ///////////////////////////////////////////////////////
                //try
                //{
                string datein = labelDate_in.Text;
                    string timein = labelTime_in.Text;
                    string liscensein = label_LicensePlate.Text;
                    string vehicleType = comboBox2.SelectedItem.ToString();
                    int countInOut;

                    //XỬ LÝ TRƯỜNG HỢP XE RA VÀO NHIỀU LẦN

                    //TH chưa có dữ liệu trong bảng
                    string query2 = "SELECT Count(Bien_so) from QuanLyVao where Bien_so = '" + liscensein + "'";
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand(query2, sqlConnection);
                        countInOut = (int)command.ExecuteScalar();
                        if (countInOut == 0)
                        {
                            ++countInOut;
                            string ticketcode = labelTicket_code.Text + '0' + countInOut.ToString();
                            labelTicket_code.Text = ticketcode;
                            // Lưu ảnh Biển số đã cắt vào folder
                            string outputPath_Liscense = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//DataImage_LicensePlate";
                            string fileName_LiscensePlate = "BiensoIn_" + labelTicket_code.Text + ".jpg";

                            string outputFilePath_liscense_plate = Path.Combine(outputPath_Liscense, fileName_LiscensePlate);
                              pictureBox_Crop.Image.Save(outputFilePath_liscense_plate, System.Drawing.Imaging.ImageFormat.Jpeg);

                            // Lưu ảnh cam toàn xe vào folder
                            string outputPath_FullCam = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//DataImage_Fullcam";
                            string fileName_FullCam = "FullCamIn_" + labelTicket_code.Text + ".jpg";

                            string outputFilePath_FullCam = Path.Combine(outputPath_FullCam, fileName_FullCam);
                            pictureBox_Lane_in.Image.Save(outputFilePath_FullCam, System.Drawing.Imaging.ImageFormat.Jpeg);
                            string query = "INSERT INTO QuanLyVao (Ma_ve, Gio_vao, Ngay_vao, Bien_so, Loai_xe, Luot_vao, Hinh_anh_Bien_So_vao, Hinh_anh_toan_xe_vao) VALUES ('" + ticketcode + "','" + timein + "', '" + datein + "','" + liscensein + "','" + vehicleType + "','" + countInOut + "','" + outputFilePath_liscense_plate + "','" + outputFilePath_FullCam + "')";
                            using (SqlCommand command1 = new SqlCommand(query, sqlConnection))
                            {
                                modify.Command(query);
                            }

                            MessageBox.Show("Lưu dữ liệu xe vào thành công!", "Thông báo");
                            
                        }
                        else
                        {
                            string query4 = "SELECT Count(Bien_so) from QuanLyRa where Bien_so = '" + liscensein + "'";
                            using (SqlConnection sqlConnection1 = new SqlConnection(connectionString))
                            {
                                sqlConnection1.Open();
                                SqlCommand command1 = new SqlCommand(query4, sqlConnection1);
                                int countOut = (int)command1.ExecuteScalar();
                                labelTicket_code.Text += '0' + (countOut + 1).ToString();
                            if (countOut != countInOut)
                            {
                                string query3 = "SELECT * FROM QuanLyVao WHERE Ma_ve = '" + labelTicket_code.Text + "' ";
                                using (SqlConnection sqlConnection2 = new SqlConnection(connectionString))
                                {
                                    sqlConnection2.Open();
                                    using (SqlCommand command2 = new SqlCommand(query3, sqlConnection2))
                                    {
                                        using (SqlDataReader reader = command2.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                // Đọc dữ liệu từ các cột trong mỗi dòng
                                                string Ma_ve = reader.GetString(0);
                                                string Time_in = reader.GetString(1);
                                                string Date_in = reader.GetString(2);
                                                string LiscensePlate_in = reader.GetString(3);
                                                string vehicle_Type = reader.GetString(4);
                                                int Count_in = reader.GetInt32(5);
                                                string Image_liscense_in = reader.GetString(6);
                                                string Image_vehical_in = reader.GetString(7);
                                                // Xử lý dữ liệu

                                                data_ticket_code.Text = Ma_ve;
                                                data_time_in.Text = Time_in;
                                                data_date_in.Text = Date_in;
                                                data_liscenseplate.Text = LiscensePlate_in;
                                                data_vehicle_type.Text = vehicle_Type;

                                                int costTicket = 0;

                                                if (vehicle_Type == "Xe may")
                                                {
                                                    costTicket = 5000;
                                                }
                                                else if (vehicle_Type == "O to")
                                                {
                                                    costTicket = 10000;
                                                }
                                                else
                                                {
                                                    costTicket = 3000;
                                                }
                                                cost_Ticket.Text = costTicket.ToString();
                                                ////Hiển thị ảnh
                                                Image image_LS_in = Image.FromFile(Image_liscense_in);

                                                //// Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                                                pictureBox_Cropdata.Image = image_LS_in;
                                                pictureBox_Cropdata.SizeMode = PictureBoxSizeMode.StretchImage;

                                                ////Hiển thị ảnh
                                                Image image_vh_in = Image.FromFile(Image_vehical_in);

                                                //// Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                                                pictureBox_Lane_out.Image = image_vh_in;
                                                pictureBox_Lane_out.SizeMode = PictureBoxSizeMode.StretchImage;

                                                ++countOut;
                                                string datein1 = labelDate_in.Text;
                                                string timein1 = labelTime_in.Text;
                                                string ticketcode1 = labelTicket_code.Text;
                                                string liscensein1 = label_LicensePlate.Text;
                                                string vehicleType1 = comboBox2.SelectedItem.ToString();

                                                // Lưu ảnh Biển số đã cắt vào folder
                                                string outputPath_LiscenseOut = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//DataImage_LicensePlate";
                                                string fileName_LiscensePlateOut = "BienSoOut_" + labelTicket_code.Text + ".jpg";

                                                string outputFilePath_liscense_plateOut = Path.Combine(outputPath_LiscenseOut, fileName_LiscensePlateOut);
                                                pictureBox_Crop.Image.Save(outputFilePath_liscense_plateOut, System.Drawing.Imaging.ImageFormat.Jpeg);

                                                // Lưu ảnh cam toàn xe vào folder
                                                string outputPath_FullCamOut = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//DataImage_Fullcam";
                                                string fileName_FullCamOut = "FullCamOut_" + labelTicket_code.Text + ".jpg";

                                                string outputFilePath_FullCamOut = Path.Combine(outputPath_FullCamOut, fileName_FullCamOut);
                                                pictureBox_Lane_in.Image.Save(outputFilePath_FullCamOut, System.Drawing.Imaging.ImageFormat.Jpeg);

                                                string query1 = "INSERT INTO QuanLyRa (Ma_ve, Gio_ra, Ngay_ra, Bien_so, Loai_xe, Luot_ra, Gia_ve, Hinh_anh_Bien_so_ra, Hinh_anh_toan_xe_ra) VALUES ('" + ticketcode1 + "','" + timein1 + "', '" + datein1 + "','" + liscensein1 + "','" + vehicleType1 + "','" + countOut + "','" + costTicket + "','" + outputFilePath_liscense_plateOut + "','" + outputFilePath_FullCamOut + "')";
                                                using (SqlCommand command3 = new SqlCommand(query1, sqlConnection2))
                                                {
                                                    modify.Command(query1);
                                                }

                                                int account_money = 0;

                                                string query10 = "SELECT * from QuanLyVeThang where Bien_so = '" + label_LicensePlate.Text + "'";
                                                using (SqlConnection sqlConnection3 = new SqlConnection(connectionString))
                                                {
                                                    sqlConnection3.Open();
                                                    SqlCommand command3 = new SqlCommand(query10, sqlConnection3);
                                                    SqlDataReader reader2 = command3.ExecuteReader();
                                                    while (reader2.Read())
                                                    {
                                                        string customer_name = reader2.GetString(0);
                                                        label_tickerCustomer.Text = customer_name;
                                                        account_money = reader2.GetInt32(3);
                                                        label_ticketAlert.Text = account_money.ToString();
                                                    }
                                                    sqlConnection3.Close();
                                                }

                                                int account_money_enable = account_money - int.Parse(cost_Ticket.Text);
                                                if (account_money_enable < 0)
                                                {
                                                    label_ticketAlert.Text = "Tài khoản không đủ";
                                                }
                                                else
                                                {
                                                    string query11 = "UPDATE QuanLyVeThang SET Tien_tai_khoan = '" +account_money_enable+ "' where Bien_so = '" + label_LicensePlate.Text + "'";
                                                    using (SqlConnection sqlConnection4 = new SqlConnection(connectionString))
                                                    {
                                                        sqlConnection4.Open();
                                                        SqlCommand command4 = new SqlCommand(query11, sqlConnection4);
                                                        command4.ExecuteNonQuery();
                                                        sqlConnection4.Close();
                                                    }
                                                }
                                                MessageBox.Show("Lưu dữ liệu xe ra thành công!", "Thông báo");

                                            }
                                        }
                                        sqlConnection2.Close();
                                    }
                                }
                            }
                            else
                            {
                                ++countInOut;
                                string ticketcode = labelTicket_code.Text;
                                // Lưu ảnh Biển số đã cắt vào folder
                                string outputPath_Liscense = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//DataImage_LicensePlate";
                                string fileName_LiscensePlate = "BiensoIn_" + labelTicket_code.Text + ".jpg";

                                string outputFilePath_liscense_plate = Path.Combine(outputPath_Liscense, fileName_LiscensePlate);
                                pictureBox_Crop.Image.Save(outputFilePath_liscense_plate, System.Drawing.Imaging.ImageFormat.Jpeg);

                                // Lưu ảnh cam toàn xe vào folder
                                string outputPath_FullCam = "C://Users//Admin//Downloads//DoAnIII_NguyenNamKhanh_20192935//DataImage_Fullcam";
                                string fileName_FullCam = "FullCamIn_" + labelTicket_code.Text + ".jpg";

                                string outputFilePath_FullCam = Path.Combine(outputPath_FullCam, fileName_FullCam);
                                pictureBox_Lane_in.Image.Save(outputFilePath_FullCam, System.Drawing.Imaging.ImageFormat.Jpeg);
                                string query = "INSERT INTO QuanLyVao (Ma_ve, Gio_vao, Ngay_vao, Bien_so, Loai_xe, Luot_vao, Hinh_anh_Bien_So_vao, Hinh_anh_toan_xe_vao) VALUES ('" + ticketcode + "','" + timein + "', '" + datein + "','" + liscensein + "','" + vehicleType + "','" + countInOut + "','" + outputFilePath_liscense_plate + "','" + outputFilePath_FullCam + "')";
                                using (SqlCommand command2 = new SqlCommand(query, sqlConnection1))
                                {
                                    modify.Command(query);
                                }

                                MessageBox.Show("Nhập dữ liệu xe vào thành công!", "Thông báo");
                            }
                                sqlConnection1.Close();
                            } 
                                
                        }
                    }
                //}
                //catch
                //{
                //    MessageBox.Show("Nhập dữ liệu không thành công, vui lòng kiểm tra lại!", "Thông báo");
                //}
            }else if(countClick > 1) {
                countClick = 0;
                label_tickerCustomer.Text = null;
                label_ticketAlert.Text = null;
                //Làn xe
                labelTime_in.Text = null;
                labelDate_in.Text = null;
                label_LicensePlate.Text=null;
                labelTicket_code.Text = null;
                pictureBox_Crop.Image = null;
                pictureBox_Lane_in.Image = null;

                //Thông tin xe vào
                cost_Ticket.Text = null;
                data_date_in.Text = null;
                data_time_in.Text = null;
                data_ticket_code.Text = null;
                data_vehicle_type.Text = null; 
                data_liscenseplate.Text = null;
                pictureBox_Cropdata.Image = null;
                pictureBox_Lane_out.Image = null;
            }     
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now.Add(new TimeSpan());
            label_time.Text = String.Format("{0:hh:mm:ss tt}", dt);
            label_date.Text = String.Format("{0:dd/MM/yyyy}", dt);
            
        }

        // Phương thức chụp ảnh
        private Bitmap CaptureImage()
        {
            // Lấy ảnh từ pictureBox_Camera
            Bitmap bitmap = new Bitmap(pictureBox_Camera.Width, pictureBox_Camera.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(pictureBox_Camera.PointToScreen(Point.Empty), Point.Empty, pictureBox_Camera.Size);
            }
            return bitmap;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cam != null && cam.IsRunning)
            {
                cam.Stop();
            }
            cam = new VideoCaptureDevice(cameras[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += Cam_NewFrame;
            cam.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileImage = new OpenFileDialog();
            if (fileImage.ShowDialog() == DialogResult.OK)
            {
                string imagePath = fileImage.FileName;

                // Đọc ảnh gốc
                Image originalImage = Image.FromFile(imagePath);

                // Điều chỉnh kích thước ảnh theo tỷ lệ
                //Image resizedImage = ScaleImage(originalImage, pictureBox_Lane_in.Width, pictureBox_Lane_in.Height);

                // Hiển thị ảnh đã điều chỉnh kích thước trong PictureBox
                pictureBox_Lane_in.Image = originalImage;
                pictureBox_Lane_in.SizeMode = PictureBoxSizeMode.StretchImage;

                textBox_LinkImage.Text = imagePath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Chụp ảnh và lưu vào biến image
            var image = CaptureImage();

            // Hiển thị ảnh lên pictureBox
            pictureBox_Lane_in.Image = image;
        }

        private void QUANLYNHANVIENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLNV qLNV = new QLNV(); 
            qLNV.ShowDialog();

            Hide();
            this.Close();
            return;

        }

        private void label_PhanQuyen_Click(object sender, EventArgs e)
        {

        }

        private void qUANLYKHACHHANGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLKH qLKH = new QLKH();
            qLKH.ShowDialog();

            Hide();
            this.Close();
            return;
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
