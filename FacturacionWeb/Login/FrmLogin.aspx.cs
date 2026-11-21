using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace FacturacionWeb.Login
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Iniciar el foco en el primer textbox
            TxtUsuario.Focus();
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            //Conexion a la DB
            SqlConnection SqlCon = new SqlConnection();

            //Consulta de los datos a la DB
            try
            {
                SqlCon = ConexionDB.getInstancia().CrearConexion();
                SqlParameter param = new SqlParameter("@usuario", TxtUsuario.Text.Trim());
                SqlParameter param1 = new SqlParameter("@clave", TxtClave.Text.Trim());

                // CORREGIDO: era "sp_CheclLogin" (con una l de mas)
                SqlCommand cmd = new SqlCommand("sp_ChecLogin", SqlCon);
                cmd.Parameters.Add(param);
                cmd.Parameters.Add(param1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    //Variables de seccion
                    Session["Log_Status"] = dr["Log_Status"].ToString();
                    Session["Log_Codigo"] = dr["Log_Codigo"].ToString();
                    Session["Log_Usuario1"] = TxtUsuario.Text;
                    Session["Log_Nombre"] = dr["Log_Nombre"].ToString();
                    Session["Log_Apellido"] = dr["Log_Apellido"].ToString();
                    Session["Log_Departamento"] = dr["Log_Departamento"].ToString();
                    Session["Log_Cargo"] = dr["Log_Cargo"].ToString();

                    if (dr["Log_Status"].ToString() == "Activo")
                    {
                        LblMensaje.Text = "Mensaje.: Login correcto";

                        TxtUsuario.Enabled = false;
                        TxtClave.Enabled = false;
                        BtnAceptar.Enabled = false;

                        ImgCerradura.ImageUrl = "~/Imagenes/CerraduraVerde.png";
                        Timer1.Enabled = true;
                    }
                    else
                    {
                        LblMensaje.Text = "Mensaje.: Usuario Inabilitado";

                        Timer1.Enabled = true;
                    }
                }
                else
                {
                    LblMensaje.Text = "Mensaje.: Login incorrecto";

                    Timer1.Enabled = true;
                }
            }
            // CORREGIDO: ahora muestra el error real (puedes volver al mensaje original cuando funcione)
            catch (Exception ex)
            {
                LblMensaje.Text = "Mensaje.: Error: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
        }

        protected void Timer1_Tick1(object sender, EventArgs e)
        {
            //Acceder al menu de la aplicacion y quitar mensajes
            if (LblMensaje.Text == "Mensaje.: Login correcto")
            {
                Session["Log_Usuario"] = LblMensaje.Text;
                Response.Redirect("~/Mantenimiento/DefaultMantenimiento.aspx");
            }
            else
            {
                LblMensaje.Text = "Mensaje.:";
            }

            Timer1.Enabled = false;
        }
    }
}