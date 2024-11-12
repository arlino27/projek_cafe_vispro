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
    public partial class Product : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public Product()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void adminAddProducts_name_TextChanged(object sender, EventArgs e)
        {

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

        private void button5_Click(object sender, EventArgs e)
        {
            SignIn regForm = new SignIn();
            regForm.Show();

            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Customer regForm = new Customer();
            regForm.Show();

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order beranda = new Order();
            beranda.Show();

            this.Hide();
        }

        private void adminAddProducts_addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Pastikan semua field terisi
                if (txtProductID.Text != "" && txtProductName.Text != "" && cmbType.SelectedItem != null && txtStock.Text != "" && txtPrice.Text != "")
                {
                    // Cek apakah kode_barang sudah ada di database
                    string checkQuery = string.Format("SELECT COUNT(*) FROM tb_barang WHERE kode_barang = '{0}'", txtProductID.Text);

                    koneksi.Open();
                    perintah = new MySqlCommand(checkQuery, koneksi);
                    int count = Convert.ToInt32(perintah.ExecuteScalar()); // Mengecek jumlah data dengan kode_barang yang sama
                    koneksi.Close();

                    if (count > 0)
                    {
                        // Jika sudah ada, tampilkan pesan error
                        MessageBox.Show("Kode barang sudah ada! Silakan gunakan kode barang yang berbeda.");
                    }
                    else
                    {
                        // Jika kode_barang belum ada, lanjutkan untuk menambahkan data
                        query = string.Format("INSERT INTO tb_barang (kode_barang, nama_barang, type, stok, harga) " +
                                              "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');",
                                              txtProductID.Text,
                                              txtProductName.Text,
                                              cmbType.SelectedItem.ToString(),
                                              txtStock.Text,
                                              txtPrice.Text);

                        koneksi.Open();
                        perintah = new MySqlCommand(query, koneksi);
                        int res = perintah.ExecuteNonQuery();
                        koneksi.Close();

                        if (res == 1)
                        {
                            MessageBox.Show("Data telah ditambahkan Suksess ...");
                            Product_Load(null, null); // Refresh data grid
                            ResetForm();
                        }
                        else
                        {
                            MessageBox.Show("Gagal Menambahkan Data ...");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Data Tidak lengkap !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ResetForm()
        {
            txtProductID.Clear();
            txtProductName.Clear();
            cmbType.SelectedIndex = -1;
            txtStock.Clear();
            txtPrice.Clear();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void adminAddProducts_updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Pastikan ProductID diisi
                if (txtProductID.Text != "")
                {
                    // Buat bagian query untuk update dinamis
                    List<string> updateColumns = new List<string>();

                    // Update nama produk jika ada input nama produk baru
                    if (!string.IsNullOrEmpty(txtProductName.Text))
                    {
                        updateColumns.Add($"nama_barang = '{txtProductName.Text}'");
                    }

                    // Update tipe produk jika ada input tipe produk baru
                    if (cmbType.SelectedItem != null)
                    {
                        updateColumns.Add($"type = '{cmbType.SelectedItem.ToString()}'");
                    }

                    // Update harga jika ada input harga baru
                    if (!string.IsNullOrEmpty(txtPrice.Text))
                    {
                        updateColumns.Add($"harga = '{txtPrice.Text}'");
                    }

                    // Update stok jika ada input stok baru
                    if (!string.IsNullOrEmpty(txtStock.Text))
                    {
                        updateColumns.Add($"stok = '{txtStock.Text}'");
                    }

                    // Jika ada perubahan, lanjutkan dengan query UPDATE
                    if (updateColumns.Count > 0)
                    {
                        // Gabungkan semua kolom yang diupdate menjadi satu string
                        string setQuery = string.Join(", ", updateColumns);

                        // Query UPDATE
                        query = $"UPDATE tb_barang SET {setQuery} WHERE kode_barang = '{txtProductID.Text}'";

                        // Eksekusi query UPDATE
                        koneksi.Open();
                        perintah = new MySqlCommand(query, koneksi);
                        int res = perintah.ExecuteNonQuery();
                        koneksi.Close();

                        // Cek apakah berhasil
                        if (res == 1)
                        {
                            MessageBox.Show("Data berhasil diperbarui!");
                            Product_Load(null, null); // Refresh data grid
                            ResetForm(); // Reset form input
                        }
                        else
                        {
                            MessageBox.Show("Gagal memperbarui data.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tidak ada data yang diubah!");
                    }
                }
                else
                {
                    MessageBox.Show("Product ID tidak ditemukan!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void adminAddProducts_clearBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Mengosongkan semua field input
                txtProductID.Clear();
                txtProductName.Clear();
                cmbType.SelectedIndex = -1; // Set combo box ke keadaan default (tidak ada yang terpilih)
                txtStock.Clear();
                txtPrice.Clear();

                // Menonaktifkan tombol Update dan Delete setelah clear
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                // Menampilkan kembali data yang terbaru di DataGridView
                Product_Load(null, null); // Memanggil fungsi untuk memuat ulang data

                // Menonaktifkan tombol-tombol tertentu
                btnAdd.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void adminAddProducts_deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductID.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from tb_barang where kode_barang = '{0}'", txtProductID.Text);
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
                    Product_Load(null, null);
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

        private void Product_Load(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                query = string.Format("select * from tb_barang");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                perintah.ExecuteNonQuery();
                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                DataGridProduct.DataSource = ds.Tables[0];
                DataGridProduct.DataSource = ds.Tables[0];
                DataGridProduct.Columns[0].Width = 50;
                DataGridProduct.Columns[0].HeaderText = "Product ID";
                DataGridProduct.Columns[1].Width = 100;
                DataGridProduct.Columns[1].HeaderText = "Product Name";
                DataGridProduct.Columns[2].Width = 70;
                DataGridProduct.Columns[2].HeaderText = "Type";
                DataGridProduct.Columns[3].Width = 50;
                DataGridProduct.Columns[3].HeaderText = "Stock";
                DataGridProduct.Columns[4].Width = 70;
                DataGridProduct.Columns[4].HeaderText = "Harga";

                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnClear.Enabled = true;
                btnAdd.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
