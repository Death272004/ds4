using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Laboratorio203
{
    public partial class Default : Page
    {
        string connectionString = @"Server=localhost;Database=productos;Trusted_Connection=True;";
        bool nuevo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigurarEstadoInicial();
                CargarLaptops();
            }
        }

        private void ConfigurarEstadoInicial()
        {
            btnNuevo.Enabled = true;
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnEliminar.Enabled = false;
            txtId.Enabled = false;
            btnBuscar.Enabled = true;
            txtNombre.Enabled = false;
            txtPrecio.Enabled = false;
            txtStock.Enabled = false;
            LimpiarCampos();
            lblMensaje.Text = "";
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            btnNuevo.Enabled = false;
            btnGuardar.Enabled = true;
            btnCancelar.Enabled = true;
            btnEliminar.Enabled = false;
            txtId.Enabled = false;
            btnBuscar.Enabled = false;
            txtNombre.Enabled = true;
            txtPrecio.Enabled = true;
            txtStock.Enabled = true;
            txtNombre.Focus();
            nuevo = true;
            LimpiarCampos();
            lblMensaje.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (nuevo)
            {
                // INSERT
                string sql = "INSERT INTO LAPTOPS (NOMBRE, PRECIO, STOCK) VALUES (@Nombre, @Precio, @Stock)";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));

                    con.Open();
                    try
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            MostrarMensaje("Registro ingresado correctamente!", true);
                    }
                    catch (Exception ex)
                    {
                        MostrarMensaje("Error: " + ex.Message, false);
                    }
                }
            }
            else
            {
                // UPDATE
                string sql = "UPDATE LAPTOPS SET NOMBRE=@Nombre, PRECIO=@Precio, STOCK=@Stock WHERE ID=@Id";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Precio", decimal.Parse(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@Stock", int.Parse(txtStock.Text));
                    cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));

                    con.Open();
                    try
                    {
                        int i = cmd.ExecuteNonQuery();
                        if (i > 0)
                            MostrarMensaje("Registro actualizado correctamente!", true);
                    }
                    catch (Exception ex)
                    {
                        MostrarMensaje("Error: " + ex.Message, false);
                    }
                }
            }

            ConfigurarEstadoInicial();
            CargarLaptops();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ConfigurarEstadoInicial();
            CargarLaptops();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM LAPTOPS WHERE ID=@Id";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));

                con.Open();
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                        MostrarMensaje("Registro eliminado correctamente!", true);
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error: " + ex.Message, false);
                }
            }

            ConfigurarEstadoInicial();
            CargarLaptops();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM LAPTOPS WHERE ID=@Id";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", int.Parse(txtBuscarId.Text));

                con.Open();
                try
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        btnNuevo.Enabled = false;
                        btnGuardar.Enabled = true;
                        btnCancelar.Enabled = true;
                        btnEliminar.Enabled = true;
                        txtId.Enabled = false;
                        btnBuscar.Enabled = false;
                        txtNombre.Enabled = true;
                        txtPrecio.Enabled = true;
                        txtStock.Enabled = true;
                        txtNombre.Focus();

                        txtId.Text = reader["ID"].ToString();
                        txtNombre.Text = reader["NOMBRE"].ToString();
                        txtPrecio.Text = reader["PRECIO"].ToString();
                        txtStock.Text = reader["STOCK"].ToString();
                        nuevo = false;
                        lblMensaje.Text = "";
                    }
                    else
                    {
                        MostrarMensaje("Ningún registro encontrado con el Id ingresado!", false);
                    }
                }
                catch (Exception ex)
                {
                    MostrarMensaje("Error: " + ex.Message, false);
                }
            }
            txtBuscarId.Text = "";
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("about:blank");
        }

        protected void gvLaptops_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = gvLaptops.SelectedRow.Cells[0].Text;
            txtBuscarId.Text = id;
            btnBuscar_Click(sender, e);
        }

        private void CargarLaptops()
        {
            string sql = "SELECT * FROM LAPTOPS";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvLaptops.DataSource = dt;
                gvLaptops.DataBind();
            }
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            txtBuscarId.Text = "";
        }

        private void MostrarMensaje(string mensaje, bool esExitoso)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.CssClass = esExitoso ? "message success" : "message error";
        }
    }
}