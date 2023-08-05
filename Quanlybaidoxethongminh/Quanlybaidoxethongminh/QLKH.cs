using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using static IronPython.Modules.PythonDateTime;
using ClosedXML;

namespace Quanlybaidoxethongminh
{
    public partial class QLKH : Form
    {
        public QLKH()
        {
            InitializeComponent();
        }

        private void label_InsertLinkDataExcel_Click(object sender, EventArgs e)
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

            if(strFileName != "")
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
                        dataGridView_Excel.Rows.Add(i,xlRange.Cells[xlRow, 1].Text, xlRange.Cells[xlRow, 2].Text, xlRange.Cells[xlRow, 3].Text, xlRange.Cells[xlRow, 4].Text);
                    }
  
                }
                xlWorkbook.Close();
                xlApp.Quit();
            }




            //using(OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Wordbook|*.xlsx" , Multiselect = false })
            //{
            //    if(ofd.ShowDialog() == DialogResult.OK)
            //    {
            //        DataTable dt = new DataTable();
            //        using (XLWorkbook workbook = new XLWorkbook(ofd.FileName))
            //        {
            //            bool isFirsrRow = true;
            //            var rows = workbook.Worksheet(1).RowsUsed();
            //            foreach ( var row in rows )
            //            {
            //                if(isFirsrRow)
            //                {
            //                    foreach (IXLCell cell in row.Cells())
            //                        dt.Columns.Add(cell.Value.ToString());
            //                    isFirsrRow = false;
            //                }
            //                else
            //                {
            //                    dt.Rows.Add();
            //                    int i = 0;
            //                    foreach(IXLCell cell in row.Cells())
            //                        dt.Rows[dt.Rows.Count - 1][i++] = cell.Value.ToString();
            //                }
            //            }
            //            dataGridView1.DataSource = dt.DefaultView;
            //            Cursor.Current = Cursors.Default;
            //        }
            //    }
            //}
        }

        private void button_OpenLink_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dv = dataGridView_Excel.DataSource as DataView;
                if (dv != null)
                {
                    dv.RowFilter = textSearch.Text;
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_SaveData_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\Database_QLBDX1.mdf;Integrated Security=True";

            for (int i = 0;i< dataGridView_Excel.Rows.Count;i++)
            {
                string query1 = "select * from QuanLyVeThang Where SDT = '" + dataGridView_Excel.Rows[i].Cells[2].Value.ToString() + "'";
                using (SqlConnection sqlConnection1 = new SqlConnection(connectionString))
                {
                    sqlConnection1.Open();
                    SqlCommand command1 = new SqlCommand(query1, sqlConnection1);

                    SqlDataReader reader = command1.ExecuteReader();

                    if (reader.Read())
                    {
                        int moneyBonus = int.Parse(dataGridView_Excel.Rows[i].Cells[4].Value.ToString());

                        string sdt = dataGridView_Excel.Rows[i].Cells[2].Value.ToString();

                        string query2 = "UPDATE QuanLyVeThang SET Tien_tai_khoan = Tien_tai_khoan + '" + moneyBonus + "' WHERE SDT = '" + sdt + "'";

                        using (SqlConnection sqlConnection2 = new SqlConnection(connectionString))
                        {
                            sqlConnection2.Open();

                            SqlCommand command2 = new SqlCommand(query2, sqlConnection2);
                            command2.ExecuteNonQuery();
                            sqlConnection2.Close();
                        }
                    }
                    else
                    {
                        string query = "INSERT INTO QuanLyVeThang (Ho_va_ten, SDT, Bien_so, Tien_tai_khoan) VALUES (N'" + dataGridView_Excel.Rows[i].Cells[1].Value.ToString() + "',N'" + dataGridView_Excel.Rows[i].Cells[2].Value.ToString() + "', N'" + dataGridView_Excel.Rows[i].Cells[3].Value.ToString() + "','" + dataGridView_Excel.Rows[i].Cells[4].Value.ToString() + "')";
                        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                        {
                            sqlConnection.Open();
                            SqlCommand command = new SqlCommand(query, sqlConnection);
                            command.ExecuteNonQuery();
                            sqlConnection.Close();
                        }
                    }
                    sqlConnection1.Close();
                }
            }
            MessageBox.Show("Nhập dữ liệu thành công!", "Thông báo");
        }

        private void InsertData_Load(object sender, EventArgs e)
        {
            timer1.Start();
            dataGridView_data.ReadOnly = true;
            dataGridView_Excel.ReadOnly = true;

            qUANLYKHACHHANGToolStripMenuItem.Enabled = false;

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
        private const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\source\\repos\\Quanlybaidoxethongminh\\Quanlybaidoxethongminh\\Database_QLBDX1.mdf;Integrated Security=True";
        private void button2_Click(object sender, EventArgs e)
        {
            // Tạo DataTable hoặc BindingSource để lưu dữ liệu
            System.Data.DataTable dataTable = new System.Data.DataTable();

            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection1.Open();

                // Truy vấn dữ liệu từ bảng
                string query1 = "SELECT * FROM QuanLyVeThang"; // Thay TableName bằng tên bảng thực tế của bạn
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
            dataGridView_data.DataSource = dataTable;
        }

        private void qUANLYKHACHHANGToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now.Add(new TimeSpan());
            label_time.Text = String.Format("{0:hh:mm:ss tt}", dt);
            label_date.Text = String.Format("{0:dd/MM/yyyy}", dt);
        }

        private void dataGridView_data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
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

        private void QUANLYNHANVIENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QLNV qLNV = new QLNV();
            qLNV.ShowDialog();

            Hide();
        }
    }
}
