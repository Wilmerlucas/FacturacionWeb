using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace FacturacionWeb.Mantenimiento
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        // Same connection string name you use in FrmEmpresa (check Web.config)
        SqlConnection SqlCon = new SqlConnection(
            ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCodigo();
                CargarGrid();
            }
        }

        // ---------- Helpers ----------
        private void CargarCodigo()
        {
            try
            {
                SqlCon.Open();
                SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(Caj_Codigo),0) + 1 FROM Caja", SqlCon);
                TxtCodigo.Text = cmd.ExecuteScalar().ToString();
            }
            finally
            {
                SqlCon.Close();
            }
        }

        private void CargarGrid()
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT Caj_Codigo, Caj_Referencia, Caj_Descripcion FROM Caja ORDER BY Caj_Codigo", SqlCon);
            DataTable dt = new DataTable();
            da.Fill(dt);
            GvCajas.DataSource = dt;
            GvCajas.DataBind();
        }

        private void LimpiarCampos()
        {
            TxtReferencia.Text = "";
            TxtDescripcion.Text = "";
        }

        // ---------- ACEPTAR (guardar) ----------
        protected void ImgAceptar_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtReferencia.Text) || string.IsNullOrWhiteSpace(TxtDescripcion.Text))
            {
                ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                LblMensaje2.Text = "Error: debe completar todos los campos";
                Timer1.Enabled = true;
            }
            else
            {
                try
                {
                    SqlCon.Open();

                    // Verificar si existe registro en la BD
                    SqlCommand cmd = new SqlCommand("sp_ContarRegistro_Caja", SqlCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Caj_Referencia", TxtReferencia.Text.Trim());
                    int output = (int)cmd.ExecuteScalar();

                    if (output >= 1)
                    {
                        ImgBarras.ImageUrl = "~/Imagenes/BarraMarron.png";
                        LblMensaje2.Text = "Advertencia: La caja ya esta creada, no se puede crear dos cajas con la misma referencia";
                        Timer1.Enabled = true;
                    }
                    else
                    {
                        SqlCommand cmd2 = new SqlCommand("sp_Insert_Caja", SqlCon);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@Caj_Referencia", TxtReferencia.Text.Trim());
                        cmd2.Parameters.AddWithValue("@Caj_Descripcion", TxtDescripcion.Text.Trim());

                        cmd2.ExecuteScalar();

                        ImgBarras.ImageUrl = "~/Imagenes/BarraVerde.png";
                        LblMensaje2.Text = "Registro guardado satisfactoriamente";
                        Timer1.Enabled = true;

                        SqlCon.Close();

                        LimpiarCampos();
                        CargarCodigo();
                        CargarGrid();
                    }
                }
                catch (Exception ex)
                {
                    ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                    LblMensaje2.Text = "Error: " + ex.Message;
                    Timer1.Enabled = true;
                }
                finally
                {
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
        }

        // ---------- NUEVO ----------
        protected void ImbNuevo_Click(object sender, ImageClickEventArgs e)
        {
            LimpiarCampos();
            CargarCodigo();

            ImgAceptar.Enabled = true;
            ImbNuevo.Enabled = false;
            ImbModificar.Enabled = false;
            ImbBuscar.Enabled = true;
        }

        // ---------- CANCELAR ----------
        protected void ImbCancelar_Click(object sender, ImageClickEventArgs e)
        {
            LimpiarCampos();
            CargarCodigo();

            ImgAceptar.Enabled = true;
            ImbNuevo.Enabled = false;
            ImbModificar.Enabled = false;
            ImbBuscar.Enabled = true;
            GvCajas.SelectedIndex = -1;
        }

        // ---------- BUSCAR ----------
        protected void ImbBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string sql = "SELECT Caj_Codigo, Caj_Referencia, Caj_Descripcion FROM Caja " +
                             "WHERE Caj_Referencia LIKE @txt OR Caj_Descripcion LIKE @txt " +
                             "ORDER BY Caj_Codigo";

                SqlDataAdapter da = new SqlDataAdapter(sql, SqlCon);

                da.SelectCommand.Parameters.AddWithValue("@txt",
                    "%" + TxtReferencia.Text.Trim() + TxtDescripcion.Text.Trim() + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                GvCajas.DataSource = dt;
                GvCajas.DataBind();

                ImgBarras.ImageUrl = "~/Imagenes/BarraAzul.png";
                LblMensaje2.Text = dt.Rows.Count + " registro(s) encontrado(s)";
                Timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                LblMensaje2.Text = "Error: " + ex.Message;
                Timer1.Enabled = true;
            }
        }

        // ---------- SELECCIONAR FILA DEL GRID ----------
        protected void GvCajas_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow_Load();
        }

        private void GridViewRow_Load()
        {
            var row = GvCajas.SelectedRow;
            TxtCodigo.Text = row.Cells[1].Text;
            TxtReferencia.Text = Server.HtmlDecode(row.Cells[2].Text);
            TxtDescripcion.Text = Server.HtmlDecode(row.Cells[3].Text);

            ImgAceptar.Enabled = false;
            ImbNuevo.Enabled = true;
            ImbModificar.Enabled = true;
        }

        // ---------- MODIFICAR ----------
        protected void ImbModificar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SqlCon.Open();

                SqlCommand cmd = new SqlCommand("sp_Update_Caja", SqlCon);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Caj_Codigo", Convert.ToInt32(TxtCodigo.Text));
                cmd.Parameters.AddWithValue("@Caj_Referencia", TxtReferencia.Text);
                cmd.Parameters.AddWithValue("@Caj_Descripcion", TxtDescripcion.Text);

                cmd.ExecuteNonQuery();
                SqlCon.Close();

                ImgBarras.ImageUrl = "~/Imagenes/BarraVerde.png";
                LblMensaje2.Text = "Registro modificado satisfactoriamente";
                Timer1.Enabled = true;

                LimpiarCampos();
                CargarCodigo();
                CargarGrid();

                ImgAceptar.Enabled = true;
                ImbNuevo.Enabled = false;
                ImbModificar.Enabled = false;
            }
            catch (Exception ex)
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                ImgBarras.ImageUrl = "~/Imagenes/BarraRoja.png";
                LblMensaje2.Text = "Error: " + ex.Message;
                Timer1.Enabled = true;
            }
        }

        // ---------- TIMER (regresa la barra a azul) ----------
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ImgBarras.ImageUrl = "~/Imagenes/BarraAzul.png";
            LblMensaje2.Text = "Mensaje:";
            Timer1.Enabled = false;
        }
    }
}