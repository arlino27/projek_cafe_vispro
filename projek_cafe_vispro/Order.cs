using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace projek_cafe_vispro
{
    public partial class Order : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public Order()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void cashierOrderForm_menuTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cashierOrderForm_receiptBtn_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order beranda = new Order();
            beranda.Show();

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            beranda beranda = new beranda();
            beranda.Show();

            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Product beranda = new Product();
            beranda.Show();

            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Customer regForm = new Customer();
            regForm.Show();

            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SignIn regForm = new SignIn();
            regForm.Show();

            this.Hide();
        }

        private void cashierOrderForm_addBtn_Click(object sender, EventArgs e)
        {

        }

        private void cashierOrderForm_productID_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void SearchProductByID(string productID)
        {
            try
            {
                // Membuka koneksi ke database
                koneksi.Open();

                // Query untuk mencari data produk berdasarkan Product ID
                query = $"SELECT * FROM tb_barang WHERE kode_barang = '{productID}'";

                // Menyiapkan MySqlCommand dan DataAdapter
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                // Menyiapkan DataTable untuk menampung hasil query
                DataTable dt = new DataTable();

                // Mengisi DataTable dengan data hasil query
                adapter.Fill(dt);

                // Menutup koneksi
                koneksi.Close();

                // Menampilkan data di DataGridView TabelPrice dan TabelNamaProduct
                KolomPrice.DataSource = dt;
                KolomProductName.DataSource = dt;
                KolomStock.DataSource = dt;

                // Menyembunyikan kolom yang tidak perlu ditampilkan di TabelPrice
                if (KolomPrice.Columns.Count > 0)
                {
                    KolomPrice.Columns[0].Visible = false; // Menyembunyikan kolom pertama
                    KolomPrice.Columns[1].Visible = false; // Menyembunyikan kolom kedua
                    KolomPrice.Columns[2].Visible = false; // Menyembunyikan kolom ketiga
                    KolomPrice.Columns[3].Visible = false; // Menyembunyikan kolom keempat
                    KolomPrice.Columns[4].Width = 70;      // Mengatur lebar kolom Harga
                }

                // Menyembunyikan kolom yang tidak perlu ditampilkan di TabelNamaProduct
                if (KolomProductName.Columns.Count > 0)
                {
                    KolomProductName.Columns[0].Visible = false; // Menyembunyikan kolom pertama
                    KolomProductName.Columns[2].Visible = false; // Menyembunyikan kolom ketiga
                    KolomProductName.Columns[3].Visible = false; // Menyembunyikan kolom keempat
                    KolomProductName.Columns[4].Visible = false; // Menyembunyikan kolom kelima
                    KolomProductName.Columns[1].Width = 70;      // Mengatur lebar kolom Nama Produk
                }

                if (KolomStock.Columns.Count > 0)
                {
                    KolomStock.Columns[0].Visible = false; // Menyembunyikan kolom pertama
                    KolomStock.Columns[1].Visible = false; // Menyembunyikan kolom ketiga
                    KolomStock.Columns[2].Visible = false; // Menyembunyikan kolom keempat
                    KolomStock.Columns[4].Visible = false; // Menyembunyikan kolom kelima
                    KolomStock.Columns[3].Width = 70;      // Mengatur lebar kolom Nama Produk
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }
        private void TabelPrice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            if (txtProductID.Text.Length > 0)
            {
                // Cari produk berdasarkan Product ID yang dimasukkan
                SearchProductByID(txtProductID.Text);
            }
            else
            {
                // Jika TextBox kosong, kosongkan DataGridView TabelPrice dan TabelNamaProduct
                KolomPrice.DataSource = null;
                KolomProductName.DataSource = null;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Order_Load(object sender, EventArgs e)
        {
            try
            {
                // Membuka koneksi database
                koneksi.Open();
                query = "SELECT * FROM tb_barang";
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                // Mengambil data dari database
                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();

                // DataGridView pertama (TabelMenu) menampilkan semua kolom
                TabelMenu.DataSource = ds.Tables[0];

                // Menyembunyikan kolom tertentu jika tidak ingin menampilkannya
                TabelMenu.Columns[2].Visible = false;  // Menampilkan Type Produk
                TabelMenu.Columns[3].Visible = false;  // Menampilkan Stok
                TabelMenu.Columns[4].Visible = false;  // Menampilkan Harga

                TabelMenu.Columns[0].Width = 50;  // Menampilkan ID Produk
                TabelMenu.Columns[1].Width = 100; // Menampilkan Nama Produk

                // DataGridView kedua (TabelPrice) hanya menampilkan kolom tertentu
                KolomPrice.DataSource = ds.Tables[0];

                // Menyembunyikan kolom yang tidak perlu ditampilkan
                KolomPrice.Columns[1].Visible = false; // Menyembunyikan Nama Produk
                KolomPrice.Columns[2].Visible = false; // Menyembunyikan Type Produk
                KolomPrice.Columns[3].Visible = false; // Menyembunyikan Stok
                KolomPrice.Columns[0].Visible = false; // Menyembunyikan Stok

                // Menyesuaikan lebar
                KolomPrice.Columns[4].Width = 70;  // Hanya tampilkan Harga

                KolomProductName.DataSource = ds.Tables[0];

                // Menyembunyikan kolom yang tidak perlu ditampilkan
                KolomProductName.Columns[0].Visible = false; // Menyembunyikan Nama Produk
                KolomProductName.Columns[2].Visible = false; // Menyembunyikan Type Produk
                KolomProductName.Columns[3].Visible = false; // Menyembunyikan Stok
                KolomProductName.Columns[4].Visible = false; // Menyembunyikan Stok

                KolomProductName.Columns[1].Width = 70;

                KolomStock.DataSource = ds.Tables[0];

                // Menyembunyikan kolom yang tidak perlu ditampilkan
                KolomStock.Columns[0].Visible = false; // Menyembunyikan Nama Produk
                KolomStock.Columns[1].Visible = false; // Menyembunyikan Type Produk
                KolomStock.Columns[2].Visible = false; // Menyembunyikan Stok
                KolomStock.Columns[4].Visible = false; // Menyembunyikan Stok

                KolomStock.Columns[3].Width = 70;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }
    }
}
