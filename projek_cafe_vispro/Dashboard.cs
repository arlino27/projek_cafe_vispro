using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace projek_cafe_vispro
{
    public partial class Dashboard : Form
    {
        private MySqlConnection koneksi;
        private MySqlCommand perintah;
        private string alamat, query;
        public Dashboard()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent(); ;
        }
        private void LoadStatistics()
        {
            try
            {
                koneksi.Open();

                // Query untuk menghitung total produk
                query = "SELECT IFNULL(COUNT(*), 0) AS Total FROM tb_user;";
                perintah = new MySqlCommand(query, koneksi);
                int totalCashier = Convert.ToInt32(perintah.ExecuteScalar() ?? 0);
                labelTotalCashier.Text = $"{totalCashier}";

                // Query untuk menghitung total transaksi/order
                query = "SELECT IFNULL(COUNT(*), 0) AS Total FROM riwayat_pembelian";
                perintah = new MySqlCommand(query, koneksi);
                int totalOrders = Convert.ToInt32(perintah.ExecuteScalar() ?? 0);
                labelTotalCustomer.Text = $"{totalOrders}";

                // Query untuk menghitung total kasir
                query = "SELECT IFNULL(SUM(total), 0) AS Total FROM riwayat_pembelian WHERE DATE(tanggal_pembelian) = CURDATE()";
                perintah = new MySqlCommand(query, koneksi);
                int totalToday = Convert.ToInt32(perintah.ExecuteScalar() ?? 0);
                labelTodayIncome.Text = $"{totalToday}";

                // Query untuk menghitung total customer
                query = "SELECT IFNULL(SUM(total), 0) AS Total FROM riwayat_pembelian;";
                perintah = new MySqlCommand(query, koneksi);
                int totalIncome = Convert.ToInt32(perintah.ExecuteScalar()?? 0);
                labelTotalIncome.Text = $"{totalIncome}";

                koneksi.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Product productForm = new Product();
            productForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order orderForm = new Order();
            orderForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            History historyForm = new History();
            historyForm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SignIn signInForm = new SignIn();
                signInForm.Show();
                this.Close();
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            LoadStatistics();
        }
    }
}
