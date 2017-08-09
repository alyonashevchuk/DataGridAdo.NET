using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace DataGridView
{
    public partial class Form1 : Form
    {
        SqlConnection connection = null;
        SqlDataAdapter adapter = null;
        DataSet ds = null;
        public Form1()
        {
            try
            {
                InitializeComponent();
                var con = new ConnectionStringSettings
                {
                    Name = "DataGridViewConnection",
                    ConnectionString = "Server=localhost; Integrated Security=SSPI; Database=Book"
                };
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.ConnectionStrings.ConnectionStrings.Add(con);
                config.Save();
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataGridViewConnection"].ConnectionString);
                connection.Open();
                string query = "Select name from sys.tables where type_desc='USER_TABLE'";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["name"].ToString() != "systemdiagrams")
                        comboBox1.Items.Add(reader["name"]);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Writer")
            {
                var command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "BookWriterInfo";
                adapter = new SqlDataAdapter(command);
            }
            else
            {
                string str = "Select * from " + comboBox1.SelectedItem.ToString();
                adapter = new SqlDataAdapter(str, connection);
            }
            ds = new DataSet();
            adapter.Fill(ds);
            for(int i = 1; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ToString() != "Photo")
                    comboBox2.Items.Add(ds.Tables[0].Columns[i].ToString());
            }
            dataGridView1.DataSource = ds.Tables[0];
            bindingNavigator1.BindingSource = bindingSource1;
            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.Columns[0].Visible = false;
        }
        
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Validate();
            dataGridView1.EndEdit();
            SqlCommandBuilder b = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bindingSource1.Filter = comboBox2.SelectedItem.ToString() + " Like '%" + textBox1.Text + "%'";
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows != null)
            {
                foreach (DataGridViewRow i in dataGridView1.SelectedRows)
                    ds.Tables[0].Rows[i.Index].Delete();
                SqlCommandBuilder b = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow();
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Book":
                    Book f = new Book();
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        row["Title"] = f.ReturnData()[0];
                        row["PublishDate"] = f.ReturnData()[1];
                        row["Pages"] = f.ReturnData()[2];
                    }
                    break;
                case "Author":
                    Writer wr = new Writer();
                    wr.ShowDialog();
                    if (wr.DialogResult == DialogResult.OK)
                    {
                        row["FirstName"] = wr.ReturnData()[0];
                        row["SecondName"] = wr.ReturnData()[1];
                        row["BirthDate"] = wr.ReturnData()[2];
                        row["Photo"] = Image.FromFile(wr.ReturnData()[3]);
                    }
                    break;
            }
            ds.Tables[0].Rows.Add(row);
            SqlCommandBuilder b = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
        }
        
    }
}
