using System;
using System.Web.UI;

namespace Laboratorio202
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblMatriz.Text == "")
            {
                lblMatriz.Text = "<p>Ingrese una dimensión N y haga click en 'Generar Matriz'</p>";
            }
        }

        protected void btnGenerarMatriz_Click(object sender, EventArgs e)
        {
            string dimension = txtDimension.Text;

            if (dimension == "")
            {
                lblMatriz.Text = "<p style='color:red;'>Por favor, ingrese una dimensión.</p>";
                return;
            }

            int n;
            bool esNumero = int.TryParse(dimension, out n);

            if (!esNumero || n <= 0)
            {
                lblMatriz.Text = "<p style='color:red;'>Por favor, ingrese una dimensión válida mayor a 0.</p>";
                return;
            }
            GenerarMatriz(n);
        }

        private void GenerarMatriz(int n)
        {
            string matrizHTML = "<h3>Matriz " + n + " × " + n + " - Diagonal Inversa</h3>";
            matrizHTML += "<p>La diagonal inversa tiene valores 1 (amarillo), el resto 0 (azul)</p>";

            matrizHTML += "<div style='max-height: 600px; overflow: auto;'>";
            matrizHTML += "<table class='matrix-table'>";

            for (int i = 0; i < n; i++)
            {
                matrizHTML += "<tr>";
                for (int j = 0; j < n; j++)
                {
                    if (i + j == n - 1)
                    {
                        matrizHTML += "<td class='diagonal-cell'>1</td>";
                    }
                    else
                    {
                        matrizHTML += "<td class='regular-cell'>0</td>";
                    }
                }
                matrizHTML += "</tr>";
            }

            matrizHTML += "</table>";
            matrizHTML += "</div>";
            matrizHTML += "<p><strong>Tipo de matriz:</strong> " +
                         (n % 2 == 0 ? "N par (" + n + ")" : "N impar (" + n + ")") + "</p>";

            lblMatriz.Text = matrizHTML;
        }
    }
}