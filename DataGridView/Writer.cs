using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataGridView
{
    public partial class Writer : Form
    {
        Image img = null;
        string path;
        public Writer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        public string[] ReturnData()
        {
            string[] mas = new string[] { textBox1.Text, textBox2.Text, textBox3.Text, path };
            return mas;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open Image";
            openFileDialog1.Filter = "Image Files(*.bmp, *.jpg, *.jpeg, *.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                img = Image.FromFile(path);
                pictureBox1.Image = img;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
