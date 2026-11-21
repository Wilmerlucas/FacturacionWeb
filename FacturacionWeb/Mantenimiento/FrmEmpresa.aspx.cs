using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI;
// Si ConexionDB esta en otro namespace (por ejemplo la carpeta Clases), agrega aqui el mismo using
// que tiene arriba tu FrmCaja.aspx.cs. Ejemplo:
// using FacturacionWeb.Clases;

namespace FacturacionWeb.Mantenimiento
{
    public partial class FrmEmpresa : System.Web.UI.Page
    {
        // Conexion BD
        SqlConnection SqlCon = new SqlConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // Despues de 3 segundos la barra vuelve a azul y el mensaje a su texto normal
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ImgBarras.ImageUrl = "~/Imagenes/BarraAzul.png";
            LblMensaje2.Text = "Mensaje:";
            Timer1.Enabled = false;
        }

        // Cambia la barra (color + texto) y arranca el timer que la regresa a azul
        private void Mostrar(string barra, string texto)
        {
            ImgBarras.ImageUrl = "~/Imagenes/" + barra;
            LblMensaje2.Text = texto;
            Timer1.Enabled = true;
        }

        protected void ImgAceptar_Click(object sender, ImageClickEventArgs e)
        {
            // 1. Validar que todos los campos esten llenos
            if (string.IsNullOrWhiteSpace(TxtNombre.Text) ||
                string.IsNullOrWhiteSpace(TxtDireccion.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefono.Value) ||
                string.IsNullOrWhiteSpace(TxtRNC.Value))
            {
                Mostrar("BarraRoja.png", "Error: debe completar todos los campos");
                return;
            }

            try
            {
                SqlCon = ConexionDB.getInstancia().CrearConexion();
                SqlCon.Open();

                // 2. Verificar si ya existe una empresa en la BD (solo se permite una)
                SqlCommand cmd = new SqlCommand("sp_ContarRegistro_Emoresa", SqlCon);
                cmd.CommandType = CommandType.StoredProcedure;
                int output = Convert.ToInt32(cmd.ExecuteScalar());

                if (output >= 1)
                {
                    Mostrar("BarraMarron.png", "Advertencia: La empresa esta creada, no se puede crear dos empresas en el sistema");
                    return;
                }

                // 3. El logo es obligatorio
                if (!fuploadImagen.HasFile)
                {
                    Mostrar("BarraRoja.png", "Error debes cargar primero la imagen");
                    return;
                }

                // 4. Leer la imagen y comprobar que de verdad sea una imagen
                byte[] ImagenOriginal = fuploadImagen.FileBytes;
                using (MemoryStream ms = new MemoryStream(ImagenOriginal))
                using (Bitmap ImagenOriginalBinaria = new Bitmap(ms))
                {
                    // Si el archivo no es una imagen valida, Bitmap lanza ArgumentException
                }

                string ImagenDataURL64 = "data:" + fuploadImagen.PostedFile.ContentType + ";base64," + Convert.ToBase64String(ImagenOriginal);
                imgPreview.ImageUrl = ImagenDataURL64;

                // 5. Guardar en la BD
                SqlCommand cmd2 = new SqlCommand("sp_Insert_Empresa", SqlCon);
                cmd2.CommandType = CommandType.StoredProcedure;

                cmd2.Parameters.AddWithValue("@Emp_Nombre", TxtNombre.Text.Trim());
                cmd2.Parameters.AddWithValue("@Emp_Direccion", TxtDireccion.Text.Trim());
                cmd2.Parameters.AddWithValue("@Emp_Telefono", TxtTelefono.Value.Trim());
                cmd2.Parameters.AddWithValue("@Emp_RNC", TxtRNC.Value.Trim());
                cmd2.Parameters.Add("@Emp_Logo", SqlDbType.VarBinary, -1).Value = ImagenOriginal;

                cmd2.ExecuteNonQuery();

                Mostrar("BarraVerde.png", "Registro guardado satisfactoriamente");
            }
            catch (ArgumentException)
            {
                Mostrar("BarraRoja.png", "Error: el archivo seleccionado no es una imagen valida");
            }
            catch (Exception ex)
            {
                Mostrar("BarraRoja.png", "Error: " + ex.Message);
            }
            finally
            {
                if (SqlCon != null && SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }
        }

        protected void ImbCancelar_Click(object sender, ImageClickEventArgs e)
        {
            TxtNombre.Text = "";
            TxtDireccion.Text = "";
            TxtTelefono.Value = "";
            TxtRNC.Value = "";
            imgPreview.ImageUrl = "";
        }
    }
}