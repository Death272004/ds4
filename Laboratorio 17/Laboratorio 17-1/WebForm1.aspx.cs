using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Laboratorio171
{
    public partial class WebForm1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosSeafood();
            }
        }

        private void CargarDatosSeafood()
        {
            try
            {
                ConnectionStringSettings conString = ConfigurationManager.ConnectionStrings["db.Name"];

                if (conString == null)
                {
                    Response.Write("Error: No se encontró la cadena de conexión 'db.Name' en web.config");
                    return;
                }

                using (SqlConnection conexion = new SqlConnection(conString.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SalesByCategory", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 15).Value = "Seafood";

                        conexion.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                GridV.DataSource = reader;
                                GridV.DataBind();
                            }
                            else
                            {
                                Response.Write("No se encontraron datos para la categoría Seafood");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
        }
    }
}