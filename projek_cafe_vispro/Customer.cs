using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projek_cafe_vispro
{
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }

        private void datagridview1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void button2_Click(object sender, EventArgs e)
        {
            Order beranda = new Order();
            beranda.Show();

            this.Hide();
        }
    }
}
