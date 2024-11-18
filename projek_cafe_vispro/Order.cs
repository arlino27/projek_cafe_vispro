using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace projek_cafe_vispro
{
    public partial class Order : Form
    {
        private int hargaSatuan = 0; // Untuk menyimpan harga per satuan
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        private int totalHarga = 0;

        public Order()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order beranda = new Order();
            beranda.Show();

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

        private void cashierOrderForm_addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();

                string kodeBarang = txtProductID.Text;
                int jumlah = (int)NumQuantity.Value;
                string tanggal_pembelian = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int cash = int.TryParse(txtCash.Text, out cash) ? cash : 0;

                query = $"SELECT harga, stok FROM tb_barang WHERE kode_barang = '{kodeBarang}'";
                perintah = new MySqlCommand(query, koneksi);
                MySqlDataReader reader = perintah.ExecuteReader();

                if (!reader.Read())
                {
                    MessageBox.Show("Produk tidak ditemukan di database.");
                    reader.Close();
                    koneksi.Close();
                    return;
                }

                int harga = Convert.ToInt32(reader["harga"]);
                int stokSaatIni = Convert.ToInt32(reader["stok"]);
                reader.Close();

                if (stokSaatIni < jumlah)
                {
                    MessageBox.Show("Stok tidak mencukupi untuk pesanan ini.");
                    koneksi.Close();
                    return;
                }

                int totalHarga = jumlah * harga;
                int kembalian = cash - totalHarga;
                labelChange.Text = $"Rp {kembalian.ToString("N0")}";

                query = $@"
                    INSERT INTO riwayat_pembelian (kode_barang, jumlah, total, cash, kembalian, tanggal_pembelian) 
                    VALUES ('{kodeBarang}', {jumlah}, {totalHarga}, {cash}, {kembalian}, '{tanggal_pembelian}')";
                perintah = new MySqlCommand(query, koneksi);
                perintah.ExecuteNonQuery();

                query = $"UPDATE tb_barang SET stok = stok - {jumlah} WHERE kode_barang = '{kodeBarang}'";
                perintah = new MySqlCommand(query, koneksi);
                perintah.ExecuteNonQuery();

                MessageBox.Show("Pesanan berhasil ditambahkan!");

                koneksi.Close();
                //LoadRiwayatPembelian();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtProductID.Text))
            {
                try
                {
                    // Buka koneksi
                    koneksi.Open();

                    // Query untuk mendapatkan data barang sesuai kode barang
                    query = $"SELECT nama_barang, harga, stok FROM tb_barang WHERE kode_barang = '{txtProductID.Text}'";
                    perintah = new MySqlCommand(query, koneksi);

                    // Eksekusi query dan ambil hasil
                    MySqlDataReader reader = perintah.ExecuteReader();

                    if (reader.Read())
                    {
                        // Set data barang di label
                        labelProductName.Text = reader["nama_barang"].ToString();
                        hargaSatuan = Convert.ToInt32(reader["harga"]); // Simpan harga per satuan
                        labelStock.Text = $"{reader["stok"]}";

                        // Hitung harga total berdasarkan jumlah default
                        int jumlah = (int)NumQuantity.Value;
                        int totalHarga = jumlah * hargaSatuan;
                        labelPrice.Text = $"Rp {totalHarga.ToString("N0")}";
                    }
                    else
                    {
                        // Jika data tidak ditemukan, reset semua data
                        labelProductName.Text = "";
                        labelPrice.Text = "";
                        labelStock.Text = "";
                        hargaSatuan = 0;
                    }

                    // Tutup reader dan koneksi
                    reader.Close();
                    koneksi.Close();
                }
                catch (Exception ex)
                {
                    // Tampilkan pesan error jika terjadi kesalahan
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                }
                
            }
            else
            {
                // Jika TextBox kosong, kosongkan label
                labelProductName.Text = "";
                labelPrice.Text = "";
                labelStock.Text = "";
                hargaSatuan = 0;
            }
        }

        private void Order_Load(object sender, EventArgs e)
        {
            try
            {
                
                NumQuantity.Value = 1;

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void cashierOrderForm_removeBtn_Click(object sender, EventArgs e)
        {
            DeletePesanan pesanan = new DeletePesanan();
            pesanan.Show();

            this.Hide();

        }
        private void NumQuantity_ValueChanged(object sender, EventArgs e)
        {
            int jumlah = (int)NumQuantity.Value;
            int totalHarga = jumlah * hargaSatuan;

            // Tampilkan hasilnya di labelPrice
            labelPrice.Text = $"Rp {totalHarga.ToString("N0")}";
        }

        private void cashierOrderForm_menuTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void cashierOrderForm_receiptBtn_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TabelRiwayatPembelian_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void cashierOrderForm_clearBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Mengosongkan semua field input
                txtProductID.Clear();
                NumQuantity.Value = 1; // Set combo box ke keadaan default (tidak ada yang terpilih
                txtCash.Clear();

                // Menampilkan kembali data yang terbaru di DataGridView
                Order_Load(null, null); // Memanggil fungsi untuk memuat ulang data
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void cashierOrderForm_orderPrice_Click(object sender, EventArgs e)
        {

        }

        private void labelChange_Click(object sender, EventArgs e)
        {

        }

        private void labelProductName_Click(object sender, EventArgs e)
        {

        }

        private void TabelPrice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
