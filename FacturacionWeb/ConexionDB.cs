using System;
using System.Configuration;
using System.Data.SqlClient;

namespace FacturacionWeb
{
    public class ConexionDB
    {
        private static ConexionDB Con = null;

        private ConexionDB() { }

        public SqlConnection CrearConexion()
        {
            SqlConnection cadena = new SqlConnection();
            cadena.ConnectionString = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;
            return cadena;
        }

        public static ConexionDB getInstancia()
        {
            if (Con == null)
            {
                Con = new ConexionDB();
            }
            return Con;
        }
    }
}