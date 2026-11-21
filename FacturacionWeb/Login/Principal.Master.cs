using FacturacionWeb;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sistema_Facturacion.Login
{
    public partial class Principal : System.Web.UI.MasterPage
    {

        //Conexion BD
        SqlConnection SqlCon = new SqlConnection();

        protected void ConsultarImagenes()
        {

            //Llamar imagen de la base de datos
            SqlCon = ConexionDB.getInstancia().CrearConexion();
            SqlCon.Open();
            SqlCommand cmd = new SqlCommand("sp_ConsultarImagen_Emoresa", SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable ImagenesDB = new DataTable();
            ImagenesDB.Load(cmd.ExecuteReader());
            Repeater1.DataSource = ImagenesDB;
            Repeater1.DataBind();
            SqlCon.Close();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            LblIdentificacion.Text = (string)Session["Log_Nombre"] + (" ") + (string)Session["Log_Apellido"];

            ConsultarImagenes();

        }

        protected void LnkSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login/FrmLogin.aspx");
        }
    }
}