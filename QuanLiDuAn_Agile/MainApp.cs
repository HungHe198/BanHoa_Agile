﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuanLiDuAn_Agile.Models;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;



namespace QuanLiDuAn_Agile
{
    public partial class MainApp : Form
    {
        string _prevId;
        public MainApp()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=LEVANHUNG;Initial Catalog=QLBH;Integrated Security=True";
        private SqlConnection conn;

        SqlCommand cmd;
        SqlDataReader reader;
        QLBHContext _dbContext = new QLBHContext();

        private void MainApp_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectionString);
            LoadData();

         

        }
        public void LoadData()
        {
            conn.Open();

            string Query = "select * from SANPHAM ";
            cmd = new SqlCommand(Query, conn);
            DataTable data = new DataTable();
            data.Load(cmd.ExecuteReader());
            dgv_SanPham.DataSource = data;
            dgv_SanPham.Columns[0].HeaderText = "Id";
            dgv_SanPham.Columns[0].Width = 45;
            dgv_SanPham.Columns[1].HeaderText = "Tên sản phẩm ";
            dgv_SanPham.Columns[1].Width = 310;
            dgv_SanPham.Columns[2].HeaderText = "Giá nhập";
            dgv_SanPham.Columns[2].Width = 233;
            dgv_SanPham.Columns[3].HeaderText = "Số lượng nhập";
            dgv_SanPham.Columns[3].Width = 233;
            conn.Close();


        }
        //public bool checkExit(string id)
        //{
        //}
        
        private void btn_Find_Click(object sender, EventArgs e)
        {
            
            string content = txt_Find.Text.ToString();
            string key = cbo_Find.Text;
            var query = from sp in _dbContext.Sanphams
                        where sp.IdsanPham.ToString().Contains(content)
                        select
                        new
                        {
                            sp.IdsanPham,
                            sp.Ten,
                            sp.Gianhap,
                            sp.Slnhap
                        };
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            if (key == "ID")
            {
                 query = from sp in _dbContext.Sanphams
                            where sp.IdsanPham.Contains(content)
                            select
                            new
                            {
                                sp.IdsanPham,
                                sp.Ten,
                                sp.Gianhap,
                                sp.Slnhap
                            };

            }
            else if (key == "Tên sản phẩm")
            {
                 query = from sp in _dbContext.Sanphams
                            where sp.Ten.Contains(content)
                            select
                            new
                            {
                                sp.IdsanPham,
                                sp.Ten,
                                sp.Gianhap,
                                sp.Slnhap
                            };

            }
            else if (key == "Giá nhập")
            {
                query = from sp in _dbContext.Sanphams
                            where sp.Gianhap.ToString().Contains(content)
                            select
                            new
                            {
                                sp.IdsanPham,
                                sp.Ten,
                                sp.Gianhap,
                                sp.Slnhap
                            };

            }
            else
            {
                query = from sp in _dbContext.Sanphams
                            where sp.Slnhap.ToString().Contains(content)
                            select
                            new
                            {
                                sp.IdsanPham,
                                sp.Ten,
                                sp.Gianhap,
                                sp.Slnhap
                            };

            }
            List<Sanpham> lstSanPham = new List<Sanpham>();
            lstSanPham = query.Select(x => new Sanpham
            {
                IdsanPham = x.IdsanPham,
                Ten = x.Ten,
                Gianhap = x.Gianhap,
                Slnhap = x.Slnhap
            }).ToList();

            DataTable data = new DataTable();
            data.Columns.Add("IdsanPham", typeof(string));
            data.Columns.Add("Ten", typeof(string));
            data.Columns.Add("Gianhap", typeof(decimal));
            data.Columns.Add("Slnhap", typeof(int));
            foreach (var item in lstSanPham)
            {
                DataRow row = data.NewRow();
                row["IdsanPham"] = item.IdsanPham;
                row["Ten"] = item.Ten;
                row["Gianhap"] = item.Gianhap;
                row["Slnhap"] = item.Slnhap;
                data.Rows.Add(row);
            }

            dgv_SanPham.DataSource = data;
            dgv_SanPham.Columns[0].HeaderText = "Id";
            dgv_SanPham.Columns[0].Width = 45;
            dgv_SanPham.Columns[1].HeaderText = "Tên sản phẩm ";
            dgv_SanPham.Columns[1].Width = 310;
            dgv_SanPham.Columns[2].HeaderText = "Giá nhập";
            dgv_SanPham.Columns[2].Width = 233;
            dgv_SanPham.Columns[3].HeaderText = "Số lượng nhập";
            dgv_SanPham.Columns[3].Width = 233;

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            string id = this.txt_Id.Text;
            string tenSanPham = this.txt_Name.Text;
            decimal giaNhap = Convert.ToDecimal(this.txt_GiaNhap.Text);
            int soLuongNhap = Convert.ToInt32(this.txt_SoLuongNhap.Text);

            //if (checkExit(id))
            //{
            //    return;
            //}

            if (id == "" || tenSanPham == "" || giaNhap == 0 || soLuongNhap == 0)
            {
                MessageBox.Show("Bạn cần điền đầy đủ thông tin!");
                return;
            }
            Sanpham sanpham = new Sanpham()
            {
                IdsanPham = id,
                Ten = tenSanPham,
                Gianhap = giaNhap,
                Slnhap = soLuongNhap
            };

            _dbContext.Sanphams.Add(sanpham);
            _dbContext.SaveChanges();

            LoadData();

        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            string id = this.txt_Id.Text;
            string tenSanPham = this.txt_Name.Text;
            decimal giaNhap = Convert.ToDecimal(this.txt_GiaNhap.Text);
            int soLuongNhap = Convert.ToInt32(this.txt_SoLuongNhap.Text);

            //if (checkExit(id))
            //{
            //    return;
            //}

            if (id == "" || tenSanPham == "" || giaNhap == 0 || soLuongNhap == 0)
            {
                MessageBox.Show("Bạn cần điền đầy đủ thông tin!");
                return;
            }
            var sanpham = _dbContext.Sanphams.Find(id);
            sanpham.Ten = tenSanPham;
            sanpham.Gianhap = giaNhap;
            sanpham.Slnhap = soLuongNhap;
            _dbContext.Sanphams.Update(sanpham);
            _dbContext.SaveChanges();
            LoadData();
        }

        private void btn_Del_Click(object sender, EventArgs e)
        {
            string id = this.txt_Id.Text;
            string tenSanPham = this.txt_Name.Text;
            decimal giaNhap = Convert.ToDecimal(this.txt_GiaNhap.Text);
            int soLuongNhap = Convert.ToInt32(this.txt_SoLuongNhap.Text);

            Sanpham sanpham = new Sanpham()
            {
                IdsanPham = id,
                Ten = tenSanPham,
                Gianhap = giaNhap,
                Slnhap = soLuongNhap
            };
            _dbContext.Sanphams.Remove(sanpham);
            txt_Id.Text = "";
            txt_Name.Text = "";
            txt_GiaNhap.Text = "";
            txt_SoLuongNhap.Text = "";
            _dbContext.SaveChanges();
            LoadData();

        }

        private void btn_Show_Click(object sender, EventArgs e)
        {
            string id = this.txt_Id.Text;
            string tenSanPham = this.txt_Name.Text;
            decimal giaNhap = Convert.ToDecimal(this.txt_GiaNhap.Text);
            int soLuongNhap = Convert.ToInt32(this.txt_SoLuongNhap.Text);

            MessageBox.Show($"Id: {id} - Tên sản phẩm: {tenSanPham} - Giá nhập: {giaNhap} - Số lượng nhập: {soLuongNhap}", "Thông tin chi tiết ");
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txt_Find.Text = "";
            txt_Id.Text = "";
            txt_Name.Text = "";
            txt_GiaNhap.Text = "";
            txt_SoLuongNhap.Text = "";
            LoadData();
        }

        private void dgv_SanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            int row = e.RowIndex;
            txt_Id.Text = dgv_SanPham.Rows[row].Cells[0].Value.ToString();
            txt_Name.Text = dgv_SanPham.Rows[row].Cells[1].Value.ToString();
            txt_GiaNhap.Text = dgv_SanPham.Rows[row].Cells[2].Value.ToString();
            txt_SoLuongNhap.Text = dgv_SanPham.Rows[row].Cells[3].Value.ToString();


        }
    }
}