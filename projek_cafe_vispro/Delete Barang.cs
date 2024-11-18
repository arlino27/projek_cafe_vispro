using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace projek_cafe_vispro
{
    public partial class Delete_Barang : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public Delete_Barang()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }
        private void adminAddProducts_updateBtn_Click(object sender, EventArgs e)
        {
            
        }
        private void btnUpdate_Click(object sender, EventArgs e)
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
                            Delete_Barang_Load(null, null); // Refresh data grid
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

        private void btnEdit_Click(object sender, EventArgs e)
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
                    Delete_Barang_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Product product = new Product();
            product.Show();

            this.Hide();
        }

        private void Delete_Barang_Load(object sender, EventArgs e)
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
                TabelRiwayat.DataSource = ds.Tables[0];
                TabelRiwayat.DataSource = ds.Tables[0];
                TabelRiwayat.Columns[0].Width = 50;
                TabelRiwayat.Columns[0].HeaderText = "Kode Barang";
                TabelRiwayat.Columns[1].Width = 100;
                TabelRiwayat.Columns[1].HeaderText = "Nama Barang";
                TabelRiwayat.Columns[2].Width = 100;
                TabelRiwayat.Columns[2].HeaderText = "Tipe";
                TabelRiwayat.Columns[3].Width = 50;
                TabelRiwayat.Columns[3].HeaderText = "Stok";
                TabelRiwayat.Columns[4].Width = 100;
                TabelRiwayat.Columns[4].HeaderText = "Harga";
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
    }
}
