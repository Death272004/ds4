using System;
using System.Web.UI;

namespace Laboratorio20
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Solo mostrar mensaje inicial si no se ha generado ninguna tabla
            if (lblResultado.Text == "")
            {
                lblResultado.Text = "<p>Ingrese un número y haga click en 'Generar Tabla'</p>";
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            string input = txtNumero.Text;

            if (input == "")
            {
                lblResultado.Text = "<p style='color:red;'>Por favor, ingrese un número.</p>";
                return;
            }

            int numero;
            bool esNumero = int.TryParse(input, out numero);

            if (!esNumero)
            {
                lblResultado.Text = "<p style='color:red;'>Por favor, ingrese un número válido.</p>";
                return;
            }

            string tabla = "<h3>Tabla de multiplicar del " + numero + "</h3>";
            tabla += "<table>";
            tabla += "<tr><th>Operación</th><th>Resultado</th></tr>";

            for (int i = 1; i <= 25; i++)
            {
                int resultado = numero * i;
                tabla += "<tr><td>" + numero + " × " + i + "</td><td>" + resultado + "</td></tr>";
            }

            tabla += "</table>";
            lblResultado.Text = tabla;
        }
    }
}