using System;
using FacturacionWeb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace FacturacionWeb.Mantenimiento
{
    public partial class FrmClientes : System.Web.UI.Page
    {
        //Conexion BD
        SqlConnection SqlCon = new SqlConnection();

        private void ContarRegistro()
        {
            //Verificar si existe registro BD
            SqlCon = ConexionDB.getInstancia().CrearConexion();
            SqlCon.Open();
            SqlCommand cmd = new SqlCommand("sp_ContarRegistro_Clientes", SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            int maxId = Convert.ToInt32(cmd.ExecuteScalar());
            TxtCodigo.Text = maxId.ToString();
            SqlCon.Close();
        }

        private DataSet GetData(string SPName, SqlParameter SPParameter)
        {
            SqlCon = ConexionDB.getInstancia().CrearConexion();
            SqlCon.Open();
            SqlDataAdapter da = new SqlDataAdapter(SPName, SqlCon);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            if (SPParameter != null)
            {
                da.SelectCommand.Parameters.Add(SPParameter);
            }

            DataSet DS = new DataSet();
            da.Fill(DS);

            return DS;

        }

        private void CambioCliente()
        {
            if (DdlTipo.Text == "Personal")
            {

                PnlEmpresarial.Visible = false;
                PnlPersonal.Visible = true;

            }
            else
            {

                PnlPersonal.Visible = false;
                PnlEmpresarial.Visible = true;

            }
        }

        public void Desactivar_Botones()
        {

            ImgAceptar.Enabled = false;
            ImbCancelar.Enabled = false;
            ImbModificar.Enabled = false;
            ImbBuscar.Enabled = false;
            ImbImprimir.Enabled = false;

            DdlTipo.Enabled = false;
            TxtNombre.Enabled = false;
            TxtRNCCliente.Disabled = true;
            TxtContacto.Enabled = false;
            TxtCargo.Enabled = false;
            TxtPagina.Enabled = false;
            TxtApellido.Enabled = false;
            TxtCedulaCliente.Disabled = true;
            TxtDireccion.Enabled = false;
            DdlPais.Enabled = false;
            DdlProvincia.Enabled = false;
            TxtTelefono.Disabled = true;
            TxtCelular.Disabled = true;
            TxtCorreo.Enabled = false;

        }

        private void Reiniciar()
        {

            ContarRegistro();

            ImgAceptar.Enabled = true;
            ImbNuevo.Enabled = false;
            ImbCancelar.Enabled = true;
            ImbModificar.Enabled = false;
            ImbBuscar.Enabled = true;
            ImbImprimir.Enabled = false;

            DdlTipo.Enabled = true;
            TxtNombre.Enabled = true;
            TxtRNCCliente.Disabled = false;
            TxtContacto.Enabled = true;
            TxtCargo.Enabled = true;
            TxtPagina.Enabled = true;
            TxtApellido.Enabled = true;
            TxtCedulaCliente.Disabled = false;
            TxtDireccion.Enabled = true;
            DdlPais.Enabled = true;
            DdlProvincia.Enabled = true;
            TxtTelefono.Disabled = false;
            TxtCelular.Disabled = false;
            TxtCorreo.Enabled = true;

            llenadoTipoCliente();

            TxtNombre.Text = "";
            TxtRNCCliente.Value = "";
            TxtContacto.Text = "";
            TxtCargo.Text = "";
            TxtPagina.Text = "";
            TxtApellido.Text = "";
            TxtCedulaCliente.Value = "";
            TxtDireccion.Text = "";

            DdlPais.SelectedIndex = 0;
            DdlProvincia.SelectedIndex = 0;
            DdlProvincia.Enabled = false;
            DdlPais.SelectedItem.Text = "Selecciona Pais";
            DdlProvincia.SelectedItem.Text = "Selecciona Provincia";

            TxtTelefono.Value = "";
            TxtCelular.Value = "";
            TxtCorreo.Text = "";

            PnlPersonal.Visible = false;
            PnlEmpresarial.Visible = true;

            GridView1.DataBind();

            ImgBarras.ImageUrl = "~/Imagenes/BarraAzul.png";
            LblMensaje2.Text = "Mensaje.:";

        }

        private void llenadoTipoCliente()
        {
            //LLenado Tipo de suplidor
            SqlCon = ConexionDB.getInstancia().CrearConexion();
            SqlCon.Open();
            SqlCommand comando = new SqlCommand("sp_Consulta_DdlCliente", SqlCon);
            comando.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(comando);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DdlTipo.DataTextField = ds.Tables[0].Columns["Tip_Descripcion"].ToString();
            DdlTipo.DataSource = ds.Tables[0];
            DdlTipo.DataBind();

        }

        private void LlenadoPais()
        {

            DdlPais.DataSource = GetData("Sp_Pais", null);
            DdlPais.DataBind();

            ListItem liPais = new ListItem("Selecciona Pais", "-1");
            DdlPais.Items.Insert(0, liPais);

            ListItem liProvincia = new ListItem("Selecciona Provincia", "-1");
            DdlProvincia.Items.Insert(0, liProvincia);

            DdlProvincia.Enabled = false;

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {

                    ContarRegistro();

                    llenadoTipoCliente();

                    LlenadoPais();

                }

                CambioCliente();

            }
            catch
            {
            }

            //Login Seguridad
            LblCodigo2.Text = (string)Session["Log_Codigo"];
            LblUsuario.Text = (string)Session["Log_Usuario1"];
            Label1.Text = (string)Session["Log_Nombre"];
            Label2.Text = (string)Session["Log_Apellido"];
            LblDepartamento.Text = (string)Session["Log_Departamento"];
            Label3.Text = (string)Session["Log_Cargo"];

        }

        protected void DdlPais_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DdlPais.SelectedIndex == 0)
            {

                DdlProvincia.SelectedIndex = 0;
                DdlProvincia.Enabled = false;

            }
            else
            {

                DdlProvincia.Enabled = true;
                SqlParameter parameter = new SqlParameter("@Id_Provincia", DdlPais.SelectedValue);
                DataSet DS = GetData("Sp_Provincia", parameter);


                DdlProvincia.DataSource = DS;
                DdlProvincia.DataBind();


                ListItem liProvincia = new ListItem("Selecciona Provincia", "-1");
                DdlProvincia.Items.Insert(0, liProvincia);

                TextBox1.Visible = false;
                TextBox2.Visible = false;

            }

        }

        protected void ImgAceptar_Click(object sender, ImageClickEventArgs e)
        {

            if (DdlTipo.Text == "Empresarial")
            {

                if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtRNCCliente.Value) || string.IsNullOrWhiteSpace(TxtDireccion.Text) || string.IsNullOrWhiteSpace(DdlPais.Text) || string.IsNullOrWhiteSpace(DdlProvincia.Text) || string.IsNullOrWhiteSpace(TxtTelefono.Value))
                {

                    ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                    LblMensaje2.Text = "Los campos Nombre, RNC, Direccion, Pais, Provincia y Telefono no pueden quedar en blanco";
                    ImbNuevo.Enabled = false;
                    Timer1.Enabled = true;

                }
                else
                {

                    SqlCon = ConexionDB.getInstancia().CrearConexion();
                    SqlCon.Open();
                    SqlCommand cmd2 = new SqlCommand("sp_RegistroExiste_Cliente", SqlCon);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Cli_RNC", TxtRNCCliente.Value);
                    cmd2.Connection = SqlCon;
                    SqlDataReader rd = cmd2.ExecuteReader();
                    bool existe = rd.HasRows;
                    rd.Close();
                    SqlCon.Close();

                    if (existe)
                    {

                        ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                        LblMensaje2.Text = "Registro Existe";
                        Timer1.Enabled = true;

                    }
                    else
                    {

                        SqlCon = ConexionDB.getInstancia().CrearConexion();
                        SqlCon.Open();

                        try
                        {

                            SqlCommand cmd = new SqlCommand("sp_Insert_Cliente", SqlCon);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Cli_Codigo", TxtCodigo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Tipo", DdlTipo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Nombre", TxtNombre.Text);
                            cmd.Parameters.AddWithValue("@Cli_RNC", TxtRNCCliente.Value);
                            cmd.Parameters.AddWithValue("@Cli_Contacto", TxtContacto.Text);
                            cmd.Parameters.AddWithValue("@Cli_Cargo", TxtCargo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Pagina", TxtPagina.Text);
                            cmd.Parameters.AddWithValue("@Cli_Direccion", TxtDireccion.Text);
                            cmd.Parameters.AddWithValue("@Cli_Telefono", TxtTelefono.Value);
                            cmd.Parameters.AddWithValue("@Cli_Celular", TxtCelular.Value);
                            cmd.Parameters.AddWithValue("@Cli_Correo", TxtCorreo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Pais", DdlPais.Text);
                            cmd.Parameters.AddWithValue("@Cli_Provincia", DdlProvincia.Text);
                            cmd.ExecuteScalar();

                            ImgBarras.ImageUrl = "~/Imagenes/BarraVerde.png";
                            LblMensaje2.Text = "Registro guardado satisfactoriamente";

                            Desactivar_Botones();

                            ImbNuevo.Enabled = true;
                            SqlCon.Close();

                        }
                        catch
                        {

                            ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                            LblMensaje2.Text = "Registro Existe";
                            Timer1.Enabled = true;

                        }

                    }

                }

            }

            if (DdlTipo.Text == "Personal")
            {

                if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtApellido.Text) || string.IsNullOrWhiteSpace(TxtCedulaCliente.Value) || string.IsNullOrWhiteSpace(TxtDireccion.Text) || string.IsNullOrWhiteSpace(DdlPais.Text) || string.IsNullOrWhiteSpace(DdlProvincia.Text) || string.IsNullOrWhiteSpace(TxtCelular.Value))
                {

                    ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                    LblMensaje2.Text = "Los campos Nombre, Apellido, Cedula, Direccion, Pais, Provincia y Celular no pueden quedar en blanco";
                    ImbNuevo.Enabled = false;
                    Timer1.Enabled = true;

                }
                else
                {

                    SqlCon = ConexionDB.getInstancia().CrearConexion();
                    SqlCon.Open();
                    SqlCommand cmd2 = new SqlCommand("sp_RegistroExiste_ClientePersonal", SqlCon);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Cli_Cedula", TxtCedulaCliente.Value);
                    cmd2.Connection = SqlCon;
                    SqlDataReader rd = cmd2.ExecuteReader();
                    bool existe = rd.HasRows;
                    rd.Close();
                    SqlCon.Close();

                    if (existe)
                    {

                        ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                        LblMensaje2.Text = "Registro Existe";
                        Timer1.Enabled = true;

                    }
                    else
                    {

                        SqlCon = ConexionDB.getInstancia().CrearConexion();
                        SqlCon.Open();

                        try
                        {

                            SqlCommand cmd = new SqlCommand("sp_Insert_ClientePersonal", SqlCon);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Cli_Codigo", TxtCodigo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Tipo", DdlTipo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Nombre", TxtNombre.Text);
                            cmd.Parameters.AddWithValue("@Cli_Apellido", TxtApellido.Text);
                            cmd.Parameters.AddWithValue("@Cli_Cedula", TxtCedulaCliente.Value);
                            cmd.Parameters.AddWithValue("@Cli_Direccion", TxtDireccion.Text);
                            cmd.Parameters.AddWithValue("@Cli_Telefono", TxtTelefono.Value);
                            cmd.Parameters.AddWithValue("@Cli_Celular", TxtCelular.Value);
                            cmd.Parameters.AddWithValue("@Cli_Correo", TxtCorreo.Text);
                            cmd.Parameters.AddWithValue("@Cli_Pais", DdlPais.Text);
                            cmd.Parameters.AddWithValue("@Cli_Provincia", DdlProvincia.Text);
                            cmd.ExecuteScalar();

                            ImgBarras.ImageUrl = "~/Imagenes/BarraVerde.png";
                            LblMensaje2.Text = "Registro guardado satisfactoriamente";

                            Desactivar_Botones();

                            ImbNuevo.Enabled = true;
                            SqlCon.Close();

                        }
                        catch
                        {

                            ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                            LblMensaje2.Text = "Registro Existe";
                            Timer1.Enabled = true;

                        }

                    }

                }

            }
        }

        protected void ImbNuevo_Click(object sender, ImageClickEventArgs e)
        {
            Reiniciar();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            DdlProvincia.Enabled = false;

            TextBox1.Visible = true;
            TextBox2.Visible = true;

            TextBox1.Text = "";
            TextBox2.Text = "";

            TxtCodigo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[1].Text;
            DdlTipo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[2].Text;
            TxtNombre.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[3].Text;

            TxtRNCCliente.Value = GridView1.Rows[GridView1.SelectedIndex].Cells[4].Text;
            TxtContacto.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[5].Text;
            TxtCargo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[6].Text;

            TxtPagina.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[7].Text;
            TxtDireccion.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[8].Text;

            TxtTelefono.Value = GridView1.Rows[GridView1.SelectedIndex].Cells[11].Text;
            TxtCelular.Value = GridView1.Rows[GridView1.SelectedIndex].Cells[12].Text;
            TxtCorreo.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[13].Text;

            TxtApellido.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[14].Text;
            TxtCedulaCliente.Value = GridView1.Rows[GridView1.SelectedIndex].Cells[15].Text;

            CambioCliente();

            DdlPais.SelectedItem.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[9].Text;
            DdlProvincia.SelectedItem.Text = GridView1.Rows[GridView1.SelectedIndex].Cells[10].Text;


            if (ImbModificar.Enabled == true)
            {

                SqlCon = ConexionDB.getInstancia().CrearConexion();
                SqlCon.Open();
                SqlCommand cmd2 = new SqlCommand("Sp_ConsultaPais", SqlCon);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Id_Pais", DdlPais.SelectedItem.Text);
                cmd2.Connection = SqlCon;
                SqlDataReader rd = cmd2.ExecuteReader();

                LlenadoPais();

                if (rd.Read())
                {

                    TextBox1.Text = rd["Pai_Nombre"].ToString();


                }

                SqlCon = ConexionDB.getInstancia().CrearConexion();
                SqlCon.Open();
                SqlCommand cmd3 = new SqlCommand("Sp_ConsultaProvincia", SqlCon);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@Id_Provincia", DdlProvincia.SelectedItem.Text);
                cmd3.Connection = SqlCon;
                SqlDataReader rd2 = cmd3.ExecuteReader();

                if (rd2.Read())
                {

                    DdlProvincia.SelectedItem.Text = "Selecciona Provincia";

                    TextBox2.Text = rd2["Pro_Nombre"].ToString();

                }

            }
            else
            {

                SqlCon = ConexionDB.getInstancia().CrearConexion();
                SqlCon.Open();
                SqlCommand cmd2 = new SqlCommand("Sp_ConsultaPais", SqlCon);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Id_Pais", DdlPais.SelectedItem.Text);
                cmd2.Connection = SqlCon;
                SqlDataReader rd = cmd2.ExecuteReader();

                if (rd.Read())
                {

                    TextBox1.Text = rd["Pai_Nombre"].ToString();

                }

                SqlCon = ConexionDB.getInstancia().CrearConexion();
                SqlCon.Open();
                SqlCommand cmd3 = new SqlCommand("Sp_ConsultaProvincia", SqlCon);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@Id_Provincia", DdlProvincia.SelectedItem.Text);
                cmd3.Connection = SqlCon;
                SqlDataReader rd2 = cmd3.ExecuteReader();

                if (rd2.Read())
                {

                    TextBox2.Text = rd2["Pro_Nombre"].ToString();

                }

            }

            SqlCon.Close();

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {

            ImgBarras.ImageUrl = "~/Imagenes/BarraAzul.png";
            LblMensaje2.Text = "Mensajes";
            Timer1.Enabled = false;

        }

        protected void ImbCancelar_Click(object sender, ImageClickEventArgs e)
        {
            Reiniciar();
        }

        protected void ImbBuscar_Click(object sender, ImageClickEventArgs e)
        {

            SqlCon = ConexionDB.getInstancia().CrearConexion();
            DataTable dt = new DataTable();
            SqlCon.Open();
            SqlCommand comando = new SqlCommand("sp_Consulta_GridCliente", SqlCon);
            comando.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(comando);
            sda.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            SqlCon.Close();

            ImgAceptar.Enabled = false;
            ImbModificar.Enabled = true;

        }

        protected void ImbModificar_Click(object sender, ImageClickEventArgs e)
        {

            if (DdlTipo.Text == "Empresarial")
            {

                if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtRNCCliente.Value) || string.IsNullOrWhiteSpace(TxtDireccion.Text) || string.IsNullOrWhiteSpace(DdlPais.Text) || string.IsNullOrWhiteSpace(DdlProvincia.Text) || string.IsNullOrWhiteSpace(TxtTelefono.Value))
                {

                    ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                    LblMensaje2.Text = "Mensaje.: Existen campos en blanco, no se puede modificar este registro";
                    Timer1.Enabled = true;

                }
                else
                {

                    SqlCon = ConexionDB.getInstancia().CrearConexion();
                    SqlCon.Open();
                    SqlCommand cmd2 = new SqlCommand("sp_RegistroExiste_Cliente", SqlCon);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Cli_RNC", TxtRNCCliente.Value);
                    cmd2.Connection = SqlCon;
                    SqlDataReader rd = cmd2.ExecuteReader();

                    if (rd.HasRows)
                    {

                        rd.Close();

                        if (DdlPais.SelectedItem.Text == "Selecciona Pais")
                        {

                            SqlCommand comando = new SqlCommand("sp_Update_Cliente2", SqlCon);
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@Cli_Codigo", TxtCodigo.Text);
                            comando.Parameters.AddWithValue("@Cli_Tipo", DdlTipo.Text);
                            comando.Parameters.AddWithValue("@Cli_Nombre", TxtNombre.Text);
                            comando.Parameters.AddWithValue("@Cli_RNC", TxtRNCCliente.Value);
                            comando.Parameters.AddWithValue("@Cli_Contacto", TxtContacto.Text);
                            comando.Parameters.AddWithValue("@Cli_Cargo", TxtCargo.Text);
                            comando.Parameters.AddWithValue("@Cli_Pagina", TxtPagina.Text);
                            comando.Parameters.AddWithValue("@Cli_Direccion", TxtDireccion.Text);
                            comando.Parameters.AddWithValue("@Cli_Telefono", TxtTelefono.Value);
                            comando.Parameters.AddWithValue("@Cli_Celular", TxtCelular.Value);
                            comando.Parameters.AddWithValue("@Cli_Correo", TxtCorreo.Text);

                            comando.Parameters.AddWithValue("@Log_Codigo", LblCodigo2.Text);
                            comando.Parameters.AddWithValue("@Log_Usuario", LblUsuario.Text);
                            comando.Parameters.AddWithValue("@Log_Nombre", Label1.Text);
                            comando.Parameters.AddWithValue("@Log_Apellido", Label2.Text);
                            comando.Parameters.AddWithValue("@Log_Departamento", LblDepartamento.Text);
                            comando.Parameters.AddWithValue("@Log_Cargo", Label3.Text);
                            comando.Parameters.AddWithValue("@Cli_FechaModificacion", DateTime.Now);

                            comando.ExecuteNonQuery();

                        }
                        else
                        {

                            SqlCommand comando = new SqlCommand("sp_Update_Cliente", SqlCon);
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@Cli_Codigo", TxtCodigo.Text);
                            comando.Parameters.AddWithValue("@Cli_Tipo", DdlTipo.Text);
                            comando.Parameters.AddWithValue("@Cli_Nombre", TxtNombre.Text);
                            comando.Parameters.AddWithValue("@Cli_RNC", TxtRNCCliente.Value);
                            comando.Parameters.AddWithValue("@Cli_Contacto", TxtContacto.Text);
                            comando.Parameters.AddWithValue("@Cli_Cargo", TxtCargo.Text);
                            comando.Parameters.AddWithValue("@Cli_Pagina", TxtPagina.Text);
                            comando.Parameters.AddWithValue("@Cli_Direccion", TxtDireccion.Text);
                            comando.Parameters.AddWithValue("@Cli_Telefono", TxtTelefono.Value);
                            comando.Parameters.AddWithValue("@Cli_Celular", TxtCelular.Value);
                            comando.Parameters.AddWithValue("@Cli_Correo", TxtCorreo.Text);
                            comando.Parameters.AddWithValue("@Cli_Pais", DdlPais.Text);
                            comando.Parameters.AddWithValue("@Cli_Provincia", DdlProvincia.Text);

                            comando.Parameters.AddWithValue("@Log_Codigo", LblCodigo2.Text);
                            comando.Parameters.AddWithValue("@Log_Usuario", LblUsuario.Text);
                            comando.Parameters.AddWithValue("@Log_Nombre", Label1.Text);
                            comando.Parameters.AddWithValue("@Log_Apellido", Label2.Text);
                            comando.Parameters.AddWithValue("@Log_Departamento", LblDepartamento.Text);
                            comando.Parameters.AddWithValue("@Log_Cargo", Label3.Text);
                            comando.Parameters.AddWithValue("@Cli_FechaModificacion", DateTime.Now);

                            comando.ExecuteNonQuery();

                        }

                        SqlCon.Close();

                        ImgBarras.ImageUrl = "~/Imagenes/BarraVerde.png";
                        LblMensaje2.Text = "Registro modificado exitosamente";
                        Timer1.Enabled = true;

                        ImbModificar.Enabled = false;

                    }
                    else
                    {

                        ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                        LblMensaje2.Text = "Mensaje.: Registro No Existe";
                        Timer1.Enabled = true;

                    }

                }

            }

            if (DdlTipo.Text == "Personal")
            {

                if (string.IsNullOrWhiteSpace(TxtNombre.Text) || string.IsNullOrWhiteSpace(TxtApellido.Text) || string.IsNullOrWhiteSpace(TxtDireccion.Text) || string.IsNullOrWhiteSpace(DdlPais.Text) || string.IsNullOrWhiteSpace(DdlProvincia.Text) || string.IsNullOrWhiteSpace(TxtTelefono.Value))
                {

                    ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                    LblMensaje2.Text = "Mensaje.: Existen campos en blanco, no se puede modificar este registro";
                    Timer1.Enabled = true;

                }
                else
                {

                    SqlCon = ConexionDB.getInstancia().CrearConexion();
                    SqlCon.Open();
                    SqlCommand cmd2 = new SqlCommand("sp_RegistroExiste_Cliente2", SqlCon);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Cli_Cedula", TxtCedulaCliente.Value);
                    cmd2.Connection = SqlCon;
                    SqlDataReader rd = cmd2.ExecuteReader();

                    if (rd.HasRows)
                    {

                        rd.Close();

                        if (DdlPais.SelectedItem.Text == "Selecciona Pais")
                        {

                            SqlCommand comando = new SqlCommand("sp_Update_Cliente4", SqlCon);
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@Cli_Codigo", TxtCodigo.Text);
                            comando.Parameters.AddWithValue("@Cli_Tipo", DdlTipo.Text);
                            comando.Parameters.AddWithValue("@Cli_Nombre", TxtNombre.Text);
                            comando.Parameters.AddWithValue("@Cli_Apellido", TxtApellido.Text);
                            comando.Parameters.AddWithValue("@Cli_Cedula", TxtCedulaCliente.Value);
                            comando.Parameters.AddWithValue("@Cli_Direccion", TxtDireccion.Text);
                            comando.Parameters.AddWithValue("@Cli_Telefono", TxtTelefono.Value);
                            comando.Parameters.AddWithValue("@Cli_Celular", TxtCelular.Value);
                            comando.Parameters.AddWithValue("@Cli_Correo", TxtCorreo.Text);

                            comando.Parameters.AddWithValue("@Log_Codigo", LblCodigo2.Text);
                            comando.Parameters.AddWithValue("@Log_Usuario", LblUsuario.Text);
                            comando.Parameters.AddWithValue("@Log_Nombre", Label1.Text);
                            comando.Parameters.AddWithValue("@Log_Apellido", Label2.Text);
                            comando.Parameters.AddWithValue("@Log_Departamento", LblDepartamento.Text);
                            comando.Parameters.AddWithValue("@Log_Cargo", Label3.Text);
                            comando.Parameters.AddWithValue("@Cli_FechaModificacion", DateTime.Now);

                            comando.ExecuteNonQuery();

                        }
                        else
                        {

                            SqlCommand comando = new SqlCommand("sp_Update_Cliente3", SqlCon);
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.AddWithValue("@Cli_Codigo", TxtCodigo.Text);
                            comando.Parameters.AddWithValue("@Cli_Tipo", DdlTipo.Text);
                            comando.Parameters.AddWithValue("@Cli_Nombre", TxtNombre.Text);
                            comando.Parameters.AddWithValue("@Cli_Apellido", TxtApellido.Text);
                            comando.Parameters.AddWithValue("@Cli_Cedula", TxtCedulaCliente.Value);
                            comando.Parameters.AddWithValue("@Cli_Direccion", TxtDireccion.Text);
                            comando.Parameters.AddWithValue("@Cli_Telefono", TxtTelefono.Value);
                            comando.Parameters.AddWithValue("@Cli_Celular", TxtCelular.Value);
                            comando.Parameters.AddWithValue("@Cli_Correo", TxtCorreo.Text);
                            comando.Parameters.AddWithValue("@Cli_Pais", DdlPais.Text);
                            comando.Parameters.AddWithValue("@Cli_Provincia", DdlProvincia.Text);

                            comando.Parameters.AddWithValue("@Log_Codigo", LblCodigo2.Text);
                            comando.Parameters.AddWithValue("@Log_Usuario", LblUsuario.Text);
                            comando.Parameters.AddWithValue("@Log_Nombre", Label1.Text);
                            comando.Parameters.AddWithValue("@Log_Apellido", Label2.Text);
                            comando.Parameters.AddWithValue("@Log_Departamento", LblDepartamento.Text);
                            comando.Parameters.AddWithValue("@Log_Cargo", Label3.Text);
                            comando.Parameters.AddWithValue("@Cli_FechaModificacion", DateTime.Now);

                            comando.ExecuteNonQuery();

                        }

                        SqlCon.Close();

                        ImgBarras.ImageUrl = "~/Imagenes/BarraVerde.png";
                        LblMensaje2.Text = "Registro modificado exitosamente";
                        Timer1.Enabled = true;

                        ImbModificar.Enabled = false;

                    }
                    else
                    {

                        ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                        LblMensaje2.Text = "Mensaje.: Registro No Existe";
                        Timer1.Enabled = true;

                    }

                }

            }

        }

    }
}