using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Laboratorio_13_2
{
    public partial class Form1 : Form
    {
        string connectionString =
@"Server=localhost;Database=Northwind;TrustServerCertificate=True;Integrated Security=SSPI;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    MessageBox.Show("Se abrió la conexión con el servidor SQL Server y se seleccionó la base de datos");

                    SqlCommand cmd = new SqlCommand("SELECT ProductName FROM [dbo].[Products]", conexion);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        listBox1.Items.Clear();
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["ProductName"].ToString());
                        }
                    }

                    MessageBox.Show("Se cerró la conexión.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                MessageBox.Show("Producto seleccionado: " + listBox1.SelectedItem.ToString());
            }
        }
    }
}
