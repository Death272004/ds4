using CalculadoraAPI.Models;
using CalculadoraAPI.Models.WS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace CalculadoraAPI.Controllers
{
    public class CalculadoraController : ApiController
    {
        private string connectionString =
            @"Server=localhost;Database=Proyecto;TrustServerCertificate=True;Integrated Security=SSPI;";

        [HttpGet]
        [ActionName("TestConnection")]
        public Reply TestConnection()
        {
            Reply oR = new Reply();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Verificar que la tabla existe y tiene datos
                    string query = "SELECT COUNT(*) as Total FROM HISTORIAL";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int totalRegistros = (int)cmd.ExecuteScalar();

                    oR.result = 1;
                    oR.data = new
                    {
                        Estado = "✅ CONEXIÓN EXITOSA",
                        BaseDeDatos = "Proyecto",
                        Tabla = "HISTORIAL",
                        TotalRegistros = totalRegistros,
                        Servidor = "localhost"
                    };
                    oR.message = $"Conexión exitosa a la base de datos. Se encontraron {totalRegistros} registros.";
                }
            }
            catch (Exception ex)
            {
                oR.result = 0;
                oR.message = "❌ ERROR de conexión: " + ex.Message;
                oR.data = new
                {
                    Error = ex.Message,
                    ConnectionString = connectionString
                };
            }
            return oR;
        }

        [HttpGet]
        [ActionName("TodosLosCalculos")]
        public Reply ObtenerTodosLosCalculos()
        {
            Reply oR = new Reply();
            try
            {
                List<Calculo> calculos = new List<Calculo>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ID, OPERACION, RESULTADO, TIPOS, FECHA FROM HISTORIAL ORDER BY FECHA DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        calculos.Add(new Calculo
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            OPERACION = reader["OPERACION"].ToString(),
                            RESULTADO = reader["RESULTADO"].ToString(),
                            TIPOS = reader["TIPOS"].ToString(),
                            FECHA = Convert.ToDateTime(reader["FECHA"])
                        });
                    }
                }

                oR.result = 1;
                oR.data = calculos;
                oR.message = "Todos los cálculos obtenidos correctamente";
            }
            catch (Exception ex)
            {
                oR.result = 0;
                oR.message = "Error: " + ex.Message;
            }
            return oR;
        }

        [HttpGet]
        [ActionName("Sumas")]
        public Reply ObtenerSumas()
        {
            return ObtenerCalculosPorTipo("Suma");
        }

        [HttpGet]
        [ActionName("Restas")]
        public Reply ObtenerRestas()
        {
            return ObtenerCalculosPorTipo("Resta");
        }

        [HttpGet]
        [ActionName("Multiplicaciones")]
        public Reply ObtenerMultiplicaciones()
        {
            return ObtenerCalculosPorTipo("Multiplicacion");
        }

        [HttpGet]
        [ActionName("Divisiones")]
        public Reply ObtenerDivisiones()
        {
            return ObtenerCalculosPorTipo("Division");
        }

        [HttpGet]
        [ActionName("CalculosRecientes")]
        public Reply ObtenerCalculosRecientes()
        {
            Reply oR = new Reply();
            try
            {
                List<Calculo> calculos = new List<Calculo>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT TOP 5 ID, OPERACION, RESULTADO, TIPOS, FECHA FROM HISTORIAL ORDER BY FECHA DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        calculos.Add(new Calculo
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            OPERACION = reader["OPERACION"].ToString(),
                            RESULTADO = reader["RESULTADO"].ToString(),
                            TIPOS = reader["TIPOS"].ToString(),
                            FECHA = Convert.ToDateTime(reader["FECHA"])
                        });
                    }
                }

                oR.result = 1;
                oR.data = calculos;
                oR.message = "Últimos 5 cálculos obtenidos correctamente";
            }
            catch (Exception ex)
            {
                oR.result = 0;
                oR.message = "Error: " + ex.Message;
            }
            return oR;
        }

        // Método auxiliar para obtener cálculos por tipo
        private Reply ObtenerCalculosPorTipo(string tipo)
        {
            Reply oR = new Reply();
            try
            {
                List<Calculo> calculos = new List<Calculo>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ID, OPERACION, RESULTADO, TIPOS, FECHA FROM HISTORIAL WHERE TIPOS = @Tipo ORDER BY FECHA DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Tipo", tipo);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        calculos.Add(new Calculo
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            OPERACION = reader["OPERACION"].ToString(),
                            RESULTADO = reader["RESULTADO"].ToString(),
                            TIPOS = reader["TIPOS"].ToString(),
                            FECHA = Convert.ToDateTime(reader["FECHA"])
                        });
                    }
                }

                oR.result = 1;
                oR.data = calculos;
                oR.message = $"{tipo}s obtenidas correctamente";
            }
            catch (Exception ex)
            {
                oR.result = 0;
                oR.message = "Error: " + ex.Message;
            }
            return oR;
        }

        // OPCIONAL: Método POST para guardar nuevos cálculos
        [HttpPost]
        [ActionName("GuardarCalculo")]
        public Reply GuardarCalculo([FromBody] Calculo calculo)
        {
            Reply oR = new Reply();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO HISTORIAL (OPERACION, RESULTADO, TIPOS) VALUES (@Operacion, @Resultado, @Tipo)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Operacion", calculo.OPERACION);
                    cmd.Parameters.AddWithValue("@Resultado", calculo.RESULTADO);
                    cmd.Parameters.AddWithValue("@Tipo", calculo.TIPOS);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                oR.result = 1;
                oR.message = "Cálculo guardado correctamente";
            }
            catch (Exception ex)
            {
                oR.result = 0;
                oR.message = "Error al guardar: " + ex.Message;
            }
            return oR;
        }
    }
}