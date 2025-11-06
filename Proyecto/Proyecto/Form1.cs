using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class Proyecto : Form
    {
        private string connectionString =
            @"Server=localhost;Database=Proyecto;TrustServerCertificate=True;Integrated Security=SSPI;";

        private List<string> expresion = new List<string>();
        private bool nuevoNumero = true;
        private string operacionPendiente = "";
        private string operacionEspecial = "";
        public double resultado;
        public double valor;

        public Proyecto()
        {
            InitializeComponent();
        }

        private void Proyecto_Load(object sender, EventArgs e) { }
        private void Pantalla_TextChanged(object sender, EventArgs e) { }

        private void AgregarNumero(string numero)
        {
            if (Pantalla.Text == "0" || nuevoNumero)
            {
                Pantalla.Text = numero;
                nuevoNumero = false;
            }
            else
            {
                Pantalla.Text += numero;
            }
        }
        private void btn0_Click(object sender, EventArgs e) => AgregarNumero("0");
        private void btn1_Click(object sender, EventArgs e) => AgregarNumero("1");
        private void btn2_Click(object sender, EventArgs e) => AgregarNumero("2");
        private void btn3_Click(object sender, EventArgs e) => AgregarNumero("3");
        private void btn4_Click(object sender, EventArgs e) => AgregarNumero("4");
        private void btn5_Click(object sender, EventArgs e) => AgregarNumero("5");
        private void btn6_Click(object sender, EventArgs e) => AgregarNumero("6");
        private void btn7_Click(object sender, EventArgs e) => AgregarNumero("7");
        private void btn8_Click(object sender, EventArgs e) => AgregarNumero("8");
        private void btn9_Click(object sender, EventArgs e) => AgregarNumero("9");

        private void btnPunto_Click(object sender, EventArgs e)
        {
            if (!Pantalla.Text.Contains(".")) Pantalla.Text += ".";
        }
        private void CE_Click(object sender, EventArgs e)
        {
            Pantalla.Text = "0";
            nuevoNumero = true;
        }
        private void C_Click(object sender, EventArgs e)
        {
            Pantalla.Text = "0";
            expresion.Clear();
            operacionPendiente = "";
            operacionEspecial = "";
            nuevoNumero = true;
        }
        private void PrepararOperacion(string op)
        {
            if (!string.IsNullOrEmpty(operacionEspecial))
            {
                btnIgual_Click(null, null);
            }
            if (!string.IsNullOrEmpty(operacionPendiente) && expresion.Count >= 2)
            {
                if (!nuevoNumero)
                {
                    expresion.Add(Pantalla.Text);
                }
                double resultadoParcial = EvaluarExpresion(expresion);
                Pantalla.Text = resultadoParcial.ToString();
                expresion.Clear();
                expresion.Add(Pantalla.Text);
            }
            else if (expresion.Count == 0)
            {
                expresion.Add(Pantalla.Text);
            }
            expresion.Add(op);
            operacionPendiente = op;
            nuevoNumero = true;
        }
        private void btnSuma_Click(object sender, EventArgs e) => PrepararOperacion("+");
        private void btnResta_Click(object sender, EventArgs e) => PrepararOperacion("-");
        private void btnMultiplica_Click(object sender, EventArgs e) => PrepararOperacion("*");
        private void btnDivide_Click(object sender, EventArgs e) => PrepararOperacion("/");
        private void btnCuadrado_Click(object sender, EventArgs e)
        {
            if (double.TryParse(Pantalla.Text, out double num))
            {
                operacionEspecial = "x²";
                expresion.Clear();
                operacionPendiente = "";
                nuevoNumero = true;
            }
        }
        private void btnRaiz_Click(object sender, EventArgs e)
        {
            if (double.TryParse(Pantalla.Text, out double num) && num >= 0)
            {
                operacionEspecial = "√";
                expresion.Clear();
                operacionPendiente = "";
                nuevoNumero = true;
            }
            else if (num < 0)
            {
                MessageBox.Show("No se puede calcular la raíz de un número negativo");
            }
        }
        private void btnNegativo_Click(object sender, EventArgs e)
        {
            if (Pantalla.Text.StartsWith("-"))
                Pantalla.Text = Pantalla.Text.Substring(1);
            else if (Pantalla.Text != "0")
                Pantalla.Text = "-" + Pantalla.Text;
        }
        private void btnIgual_Click(object sender, EventArgs e)
        {
            double resultadoFinal = 0;
            string operacionTexto = "";

            if (!string.IsNullOrEmpty(operacionEspecial))
            {
                valor = double.Parse(Pantalla.Text);
                resultadoFinal = valor;
                if (operacionEspecial == "√")
                {
                    if (valor >= 0)
                    {
                        resultadoFinal = Math.Sqrt(valor);
                        operacionTexto = $"√{valor}";
                    }
                    else
                    {
                        MessageBox.Show("No se puede calcular la raíz de un número negativo");
                        return;
                    }
                }
                else if (operacionEspecial == "x²")
                {
                    resultadoFinal = Math.Pow(valor, 2);
                    operacionTexto = $"({valor})²";
                }
                Pantalla.Text = resultadoFinal.ToString();
                GuardarOperacion(operacionTexto, resultadoFinal.ToString(), operacionEspecial);
                operacionEspecial = "";
                nuevoNumero = true;
            }
            else if (expresion.Count > 0)
            {
                if (!nuevoNumero)
                {
                    expresion.Add(Pantalla.Text);
                }

                if (expresion.Count >= 3 && expresion.Count % 2 == 1)
                {
                    resultadoFinal = EvaluarExpresion(expresion);
                    Pantalla.Text = resultadoFinal.ToString();
                    operacionTexto = string.Join(" ", expresion);
                    GuardarOperacion(operacionTexto, resultadoFinal.ToString(), "Expr");
                }
                else
                {
                    // Si no hay una expresión completa, guardar solo el número actual
                    resultadoFinal = double.Parse(Pantalla.Text);
                    operacionTexto = Pantalla.Text;
                    GuardarOperacion(operacionTexto, resultadoFinal.ToString(), "Num");
                }

                expresion.Clear();
                nuevoNumero = true;
            }
            else
            {
                // Si no hay expresión ni operación especial, guardar solo el número
                resultadoFinal = double.Parse(Pantalla.Text);
                operacionTexto = Pantalla.Text;
                GuardarOperacion(operacionTexto, resultadoFinal.ToString(), "Num");
            }

            operacionPendiente = "";
        }
        private double EvaluarExpresion(List<string> expr)
        {
            if (expr.Count == 0) return double.Parse(Pantalla.Text);

            if (expr.Count < 3 || expr.Count % 2 != 1)
            {
                throw new InvalidOperationException("Expresión incompleta");
            }

            // Primero procesar multiplicaciones y divisiones
            for (int i = 0; i < expr.Count; i++)
            {
                if (expr[i] == "*" || expr[i] == "/")
                {
                    double a = double.Parse(expr[i - 1]);
                    double b = double.Parse(expr[i + 1]);
                    double res = expr[i] == "*" ? a * b : a / b;
                    expr[i - 1] = res.ToString();
                    expr.RemoveAt(i);
                    expr.RemoveAt(i);
                    i--;
                }
            }

            // Luego procesar sumas y restas
            double resultado = double.Parse(expr[0]);
            for (int i = 1; i < expr.Count; i += 2)
            {
                string op = expr[i];
                double num = double.Parse(expr[i + 1]);
                if (op == "+") resultado += num;
                else if (op == "-") resultado -= num;
            }

            return resultado;
        }
        private void GuardarOperacion(string operacionTexto, string resultado, string tipo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO HISTORIAL (OPERACION, RESULTADO, TIPO) VALUES (@operacion, @resultado, @tipo)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@operacion", operacionTexto);
                    cmd.Parameters.AddWithValue("@resultado", resultado);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar en historial: {ex.Message}");
            }
        }
        private void btnHistorial_Click(object sender, EventArgs e)
        {
            string registros = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT OPERACION, RESULTADO, FECHA FROM HISTORIAL ORDER BY ID DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string operacion = reader["OPERACION"].ToString();
                        string resultado = reader["RESULTADO"].ToString();
                        string fecha = Convert.ToDateTime(reader["FECHA"]).ToString("g");

                        registros += $"{fecha} | {operacion} = {resultado}\n";
                    }
                }

                if (string.IsNullOrEmpty(registros))
                {
                    MessageBox.Show("No hay operaciones en el historial", "Historial de operaciones");
                }
                else
                {
                    MessageBox.Show(registros, "Historial de operaciones");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar historial: {ex.Message}");
            }
        }
    }
}