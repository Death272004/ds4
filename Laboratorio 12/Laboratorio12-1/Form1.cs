using System;
using System.Windows.Forms;

namespace Laboratorio12_1
{
    public partial class Front : Form
    {
        Calculos calc = new Calculos();

        public Front()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void Front_Load(object sender, EventArgs e)
        {
            Resultado.Text = "Resultado:";
            textBox3.Text = "";
        }

        private void Calcular_Click(object sender, EventArgs e)
        {
            try
            {
                double velocidad = double.Parse(txtVelocidad.Text);
                double tiempo = double.Parse(txtTiempo.Text);

                double distancia = calc.CalcularDistancia(velocidad, tiempo);

                textBox3.Text = distancia.ToString("F2") + " m";
            }
            catch
            {
                MessageBox.Show("Ingrese solo números válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            txtVelocidad.Clear();
            txtTiempo.Clear();
            textBox3.Clear();
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void label4_Click(object sender, EventArgs e)
        {
        }
    }
}
