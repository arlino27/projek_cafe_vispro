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
using MySql.Data;
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

        private void button5_Click(object sender, EventArgs e)
        {
            SignIn regForm = new SignIn();
            regForm.Show();

            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            History regForm = new History();
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
                if (!string.IsNullOrEmpty(txtProductName.Text) &&
            cmbType.SelectedItem != null &&
            !string.IsNullOrEmpty(txtStock.Text) &&
            !string.IsNullOrEmpty(txtPrice.Text))
                {
                    query = string.Format("INSERT INTO tb_barang (nama_barang, type, stok, harga) " +
                                  "VALUES ('{0}', '{1}', '{2}', '{3}');",
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
                        MessageBox.Show("Data telah ditambahkan sukses!");
                        Product_Load(null, null); // Refresh data grid
                        ResetForm(); // Reset input form
                    }
                    else
                    {
                        MessageBox.Show("Gagal menambahkan data.");
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak lengkap!");
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
                // Mengosongkan semua field 
                txtProductName.Clear();
                cmbType.SelectedIndex = -1; // Set combo box ke keadaan default (tidak ada yang terpilih)
                txtStock.Clear();
                txtPrice.Clear();

                // Menonaktifkan tombol Update dan Delete setelah clear
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
            Delete_Barang delete_Barang = new Delete_Barang();
            delete_Barang.Show();

            this.Hide();
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

                btnDelete.Enabled = true;
                btnClear.Enabled = true;
                btnAdd.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ResetForm()
        {
            txtProductName.Clear();
            cmbType.SelectedIndex = -1;
            txtStock.Clear();
            txtPrice.Clear();
        }
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
