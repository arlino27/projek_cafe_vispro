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
using MySql.Data;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

namespace projek_cafe_vispro
{
    public partial class History : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public History()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            History regForm = new History();
            regForm.Show();

            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SignIn regForm = new SignIn();
            regForm.Show();

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();

            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Product beranda = new Product();
            beranda.Show();

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order beranda = new Order();
            beranda.Show();

            this.Hide();
        }

        private void History_Load(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                query = "SELECT * FROM riwayat_pembelian";
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                // Mengambil data dari database
                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();


                // DataGridView pertama (TabelMenu) menampilkan semua kolom
                TabelRiwayat.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void datagridview1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cashierOrderForm_clearBtn_Click(object sender, EventArgs e)
        {
;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
