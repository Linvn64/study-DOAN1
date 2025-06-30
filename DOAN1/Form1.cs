﻿using MySql.Data.MySqlClient;
using System.Data;

namespace DOAN1
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;user id=root;password=;database=qlbanhang;charset=utf8";
        MySqlConnection conn;
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // first commit 
            conn = new MySqlConnection(connectionString);
            LoadHoaDon();


        }
        private void LoadHoaDon()
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM hoadon"; // hoặc truy vấn cụ thể hơn

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);
                dgvHoaDon.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadChiTietHoaDon(string maHD)
        {
            try
            {
                conn.Open();
                string query = "SELECT maHoaDon,maSanPham, soLuong, donGiaBan, thanhTien FROM  tt_chitiet_hoadon WHERE maHoaDon = @maHoaDon";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@maHoaDon", maHD);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvChiTietHoaDon.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chi tiết: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnChiTietHoaDon_Click(object sender, EventArgs e)
        {
           
        }
        enum TrangDangXem { HoaDon, ChiTiet }
        TrangDangXem trangHienTai = TrangDangXem.HoaDon;

        private void tsThem_Click(object sender, EventArgs e)
        {
            if (trangHienTai == TrangDangXem.HoaDon)
            {
                // Thêm hóa đơn mới
                FormThemHoaDon f = new FormThemHoaDon();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    LoadHoaDon(); // Load lại
                }
            }
            else if (trangHienTai == TrangDangXem.ChiTiet)
            {
                if (dgvHoaDon.CurrentRow != null)
                {
                    string maHD = dgvHoaDon.CurrentRow.Cells["maHoaDon"].Value.ToString();
                    FormThemChiTiet f = new FormThemChiTiet(maHD); // Truyền mã hóa đơn để thêm đúng
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        LoadChiTietHoaDon(maHD);
                    }
                }
            }
        }

        private void tsChitiet_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow != null)
            {
                string maHD = dgvHoaDon.CurrentRow.Cells["maHoaDon"].Value.ToString();
                LoadChiTietHoaDon(maHD);
                panelChiTiet.Visible = true;
                trangHienTai = TrangDangXem.ChiTiet; // Chuyển trạng thái
            }
        }
    }
}
