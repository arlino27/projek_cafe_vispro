using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace projek_cafe_vispro
{
    public partial class SignUp : Form
    {
        private string alamat;
        public SignUp()
        {
            alamat = "server=localhost; database=db_penjualan; username=root; password=;";
            InitializeComponent();
        }

        private void register_username_TextChanged(object sender, EventArgs e)
        {

        }

        private void register_password_TextChanged(object sender, EventArgs e)
        {

        }

        private void register_btn_Click(object sender, EventArgs e)

        {
            try
            {
                if (txt_Usn.Text != "" && txt_Psw.Text != "")
                {
                    string query = "INSERT INTO tb_user (username, password) VALUES (@username, @password);";

                    using (MySqlConnection koneksi = new MySqlConnection(alamat))
                    {
                        koneksi.Open();
                        using (MySqlCommand perintah = new MySqlCommand(query, koneksi))
                        {
                            perintah.Parameters.AddWithValue("@username", txt_Usn.Text);
                            perintah.Parameters.AddWithValue("@password", txt_Psw.Text);

                            int res = perintah.ExecuteNonQuery();
                            if (res == 1)
                            {
                                MessageBox.Show("Data berhasil disimpan.");
                                ResetForm();
                            }
                            else
                            {
                                MessageBox.Show("Gagal menyimpan data.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Data tidak lengkap.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan. Silakan coba lagi.");
            }
        }
        private void register_showPass_CheckedChanged(object sender, EventArgs e)
        {
            txt_Psw.PasswordChar = register_showPass.Checked ? '\0' : '*';
        }

        private void ResetForm()
        {
            txt_Usn.Clear();
            txt_Psw.Clear();
            register_showPass.Checked = false;
        }

        private void register_loginBtn_Click(object sender, EventArgs e)
        {
            SignIn regForm = new SignIn();
            regForm.Show();

            this.Hide();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }
    }
}
