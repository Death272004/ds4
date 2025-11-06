using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Parcial_2
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Server=localhost;Database=Conversor;TrustServerCertificate=True;Integrated Security=SSPI;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GuardarHistorial(string operacion, string resultado)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO HISTORIAL (OPERACION, RESULTADO) VALUES (@op, @res)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@op", operacion);
                        cmd.Parameters.AddWithValue("@res", resultado);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void Fahrenheit_A_Click(object sender, EventArgs e)
        {
            if (double.TryParse(texboxF1.Text, out double fah))
            {
                double cel = (fah - 32) * 5 / 9;
                double kel = (fah - 32) * 5 / 9 + 273.15;

                texboxF1_2.Text = cel.ToString("F2");
                texboxF1_3.Text = kel.ToString("F2");
                texboxF1_1.Text = fah.ToString("F2");

                GuardarHistorial(fah + " °F", cel + " °C, " + kel + " K");
            }
            else
            {
                MessageBox.Show("Ingrese un número válido");
            }
        }

        private void Celsius_A_Click(object sender, EventArgs e)
        {
            if (double.TryParse(TextBoxC2.Text, out double cel))
            {
                double fah = (cel * 9 / 5) + 32;
                double kel = cel + 273.15;

                TextBoxC2_1.Text = fah.ToString("F2");
                TextBoxC2_3.Text = kel.ToString("F2");
                TextBoxC2_2.Text = cel.ToString("F2");

                GuardarHistorial(cel + " °C", fah + " °F, " + kel + " K");
            }
            else
            {
                MessageBox.Show("Ingrese un número válido");
            }
        }

        private void Kelvin_A_Click(object sender, EventArgs e)
        {
            if (double.TryParse(TextBoxK3.Text, out double kel))
            {
                double fah = (kel - 273.15) * 9 / 5 + 32;
                double cel = kel - 273.15;

                TextBoxK3_1.Text = fah.ToString("F2");
                TextBoxK3_2.Text = cel.ToString("F2");
                TextBoxK3_3.Text = kel.ToString("F2");

                GuardarHistorial(kel + " K", fah + " °F, " + cel + " °C");
            }
            else
            {
                MessageBox.Show("Ingrese un número válido");
            }
        }

        private void Historial_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT OPERACION, RESULTADO FROM HISTORIAL ORDER BY ID DESC";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            string historial = "HISTORIAL:\n\n";
                            while (reader.Read())
                            {
                                historial += $"De: {reader["OPERACION"]}\n";
                                historial += $"A: {reader["RESULTADO"]}\n";
                                historial += "----------------\n";
                            }
                            MessageBox.Show(historial);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar historial: " + ex.Message);
            }
        }
    }
}