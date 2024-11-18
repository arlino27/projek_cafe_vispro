using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace projek_cafe_vispro
{
    public partial class DeletePesanan : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public DeletePesanan()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtKodeBeli.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from riwayat_pembelian where kode_pembelian = '{0}'", txtKodeBeli.Text);
                        ds.Clear();
                        koneksi.Open();
                        perintah = new MySqlCommand(query, koneksi);
                        adapter = new MySqlDataAdapter(perintah);
                        int res = perintah.ExecuteNonQuery();
                        koneksi.Close();
                        if (res == 1)
                        {
                            MessageBox.Show("Delete Data Suksess ...");
                        }
                        else
                        {
                            MessageBox.Show("Gagal Delete data");
                        }
                    }
                    EditPesanan_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        private void EditPesanan_Load(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                query = string.Format("select * from riwayat_pembelian");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                perintah.ExecuteNonQuery();
                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                TabelRiwayat.DataSource = ds.Tables[0];
                TabelRiwayat.DataSource = ds.Tables[0];
                TabelRiwayat.Columns[0].Width = 50;
                TabelRiwayat.Columns[0].HeaderText = "Kode Pembelian";
                TabelRiwayat.Columns[1].Width = 100;
                TabelRiwayat.Columns[2].Visible = false;
                TabelRiwayat.Columns[2].HeaderText = "Jumlah Pesanan";
                TabelRiwayat.Columns[3].Width = 70;
                TabelRiwayat.Columns[3].HeaderText = "Total Pesanan";
                TabelRiwayat.Columns[4].Width = 70;
                TabelRiwayat.Columns[4].HeaderText = "Waktu Pesan";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            if (txtKodeBeli.Text.Length > 0)
            {
                // Cari produk berdasarkan Product ID yang dimasukkan
                SearchProductByID(txtKodeBeli.Text);
            }
            else
            {
                // Jika TextBox kosong, kosongkan DataGridView TabelPrice dan TabelNamaProduct
                TabelRiwayat.DataSource = null;

            }
        }

        private void SearchProductByID(string kode_beli)
        {
            try
            {
                // Membuka koneksi ke database
                koneksi.Open();

                // Query untuk mencari data produk berdasarkan Product ID
                query = $"SELECT * FROM riwayat_pembelian WHERE kode_pembelian = '{kode_beli}'";

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
                TabelRiwayat.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            order.Show();

            this.Hide();

        }

        private void TabelRiwayat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ResetForm()
        {
            txtKodeBeli.Clear();
        }
    }
}
