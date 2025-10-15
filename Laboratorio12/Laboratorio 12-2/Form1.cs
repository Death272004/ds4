using System;
using System.Windows.Forms;

namespace Laboratorio_12_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double num1 = double.Parse(textBox1.Text);
                double num2 = double.Parse(textBox2.Text);
                double num3 = double.Parse(textBox3.Text);

                double promedio = calc.Calcular_promedio(num1, num2, num3);

                textBox4.Text = promedio.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese solo números válidos en los tres primeros campos.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
