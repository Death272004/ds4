using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratorio_12_3
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

                // Usando la clase Perimetro
                double suma = Perimetro.Calcular_Perimetro(num1, num2, num3);

                textBox4.Text = suma.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese solo números válidos.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double a = double.Parse(textBox1.Text);
                double b = double.Parse(textBox2.Text);
                double c = double.Parse(textBox3.Text);

                // Usando la clase Area
                double area = Area.Calcular_Area(a, b, c);

                textBox5.Text = area.ToString("F2");
            }
            catch (FormatException)
            {
                MessageBox.Show("Ingrese solo números válidos en los tres lados.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular el área: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}