using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataGridView
{
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        
        public string[] ReturnData()
        {
            string[] mas = new string[] { textBox1.Text, textBox2.Text, textBox3.Text };
            return mas;
        }

    }
}
