<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/MPMantenimiento.master" AutoEventWireup="true" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="FacturacionWeb" %>

<script runat="server">
    // Version con el codigo dentro de la pagina: no depende de compilar el .aspx.cs ni del .designer.cs
    SqlConnection SqlCon = new SqlConnection();

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        ImgBarras.ImageUrl = "~/Imagenes/BarraAzul.png";
        LblMensaje2.Text = "Mensaje:";
        Timer1.Enabled = false;
    }

    private void Mostrar(string barra, string texto)
    {
        ImgBarras.ImageUrl = "~/Imagenes/" + barra;
        LblMensaje2.Text = texto;
        Timer1.Enabled = true;
    }

    protected void ImgAceptar_Click(object sender, ImageClickEventArgs e)
    {
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

            SqlCommand cmd = new SqlCommand("sp_ContarRegistro_Emoresa", SqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            int output = Convert.ToInt32(cmd.ExecuteScalar());

            if (output >= 1)
            {
                Mostrar("BarraMarron.png", "Advertencia: La empresa esta creada, no se puede crear dos empresas en el sistema");
                return;
            }

            if (!fuploadImagen.HasFile)
            {
                Mostrar("BarraRoja.png", "Error debes cargar primero la imagen");
                return;
            }

            byte[] ImagenOriginal = fuploadImagen.FileBytes;
            using (MemoryStream ms = new MemoryStream(ImagenOriginal))
            using (Bitmap ImagenOriginalBinaria = new Bitmap(ms))
            {
                // Si el archivo no es una imagen valida, Bitmap lanza ArgumentException
            }

            imgPreview.ImageUrl = "data:" + fuploadImagen.PostedFile.ContentType + ";base64," + Convert.ToBase64String(ImagenOriginal);

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
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="contenidoPrincipal" runat="server">

    <%-- Mismo marco que FrmCaja: todo cuelga de #emWrap (contenedor de referencia),
         asi la botonera, el panel de datos y la barra de mensaje quedan posicionados
         dentro de un mismo marco y no dependen de margenes negativos encadenados. --%>
    <style type="text/css">
        /* ===== Contenedor de referencia ===== */
        #emWrap { position: relative; width: 855px; height: 399px; }
        #columnCentro h2 { margin: 10px 0 6px 0; }
        #columnCentro { min-height: 441px; }
        #columnIzq, #ColumnDer { min-height: 466px; }

        /* ===== Botonera (panel negro a la derecha) ===== */
        #emWrap .PnlBotonera { margin: 0; top: -43px; left: 860px; }
        #emWrap .PnlBotonera input { top: 0; left: 0; width: 112px; height: 28px; }
        #emWrap .ImgAceptar   { margin-top: 14px; }
        #emWrap .ImbNuevo     { position: absolute; margin-top: 62px; margin-left: 14px; }
        #emWrap .ImbCancelar  { margin-top: 110px; }
        #emWrap .ImbBuscar    { margin-top: 158px; }
        #emWrap .ImbModificar { margin-top: 206px; }
        #emWrap .ImbPrinter   { margin-top: 254px; }

        /* ===== Panel de datos (recuadro beige) ===== */
        #emWrap .PnlDatos { margin: 0; top: 5px; left: 37px; }

        .em-form { position: relative; width: 780px; height: 349px; }

        /* Selector de archivo y cuadro del logo (arriba a la izquierda, como la diapositiva) */
        .em-file { position: absolute !important; left: 4px !important; top: 1px !important; font-weight: bold; color: #000; font-size: 13px; }
        .em-file::file-selector-button { font-weight: normal; }
        .em-img  { position: absolute !important; left: 60px !important; top: 30px !important; width: 154px; height: 70px;
                   border: 2px solid #000; box-sizing: border-box; object-fit: contain; }

        /* Etiquetas y cajas de texto */
        .em-lbl  { position: absolute !important; left: 313px !important; font-weight: bold; font-size: 14px; color: #000; }
        .em-txt  { position: absolute !important; left: 393px !important; width: 220px; height: 19px; box-sizing: border-box;
                   padding: 1px 4px; border: 1px solid #7A7A7A; background: #FFF; color: #0066FF;
                   font-weight: bold; font-size: 13px; }

        .em-lbl-nombre    { top: 156px !important; }
        .em-lbl-direccion { top: 190px !important; }
        .em-lbl-telefono  { top: 224px !important; }
        .em-lbl-rnc       { top: 258px !important; }

        .em-txt-nombre    { top: 155px !important; }
        .em-txt-direccion { top: 189px !important; }
        .em-txt-telefono  { top: 223px !important; }
        .em-txt-rnc       { top: 257px !important; }

        /* ===== Barra de mensaje (debajo del recuadro beige) ===== */
        #emWrap .em-mensaje { position: absolute; left: 0; top: 364px; width: 855px; height: 35px; }
        #emWrap .em-barra   { display: block; }
        #emWrap .em-lblmsg  { position: absolute; left: 74px; top: 8px; font-weight: bold; color: #000; }
    </style>

    <script type="text/javascript">
        // Vista previa inmediata del logo elegido dentro del cuadro negro
        function emPreview(input) {
            var img = document.getElementById('<%= imgPreview.ClientID %>');
            if (input.files && input.files[0]) { img.src = URL.createObjectURL(input.files[0]); }
        }
    </script>

    <h2>&nbsp;Datos Empresa</h2>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" Interval="3000" Enabled="false" OnTick="Timer1_Tick"></asp:Timer>

    <div id="emWrap">

        <%-- ===== BOTONERA (orden: Aceptar, Nuevo, Cancelar, Buscar, Modificar, Imprimir) ===== --%>
        <asp:Panel ID="PnlBotonera" runat="server" BorderStyle="Solid" Width="138px" Height="440px"
            CssClass="PnlBotonera" BackColor="Black" BackImageUrl="~/Imagenes/Negra.png">

            <asp:ImageButton ID="ImgAceptar" runat="server" ImageUrl="~/Imagenes/Aceptar.png" AlternateText="Aceptar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImgAceptar" Enabled="True" OnClick="ImgAceptar_Click" />

            <asp:ImageButton ID="ImbNuevo" runat="server" ImageUrl="~/Imagenes/Nuevo.png" AlternateText="Nuevo"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbNuevo" Enabled="False" />

            <asp:ImageButton ID="ImbCancelar" runat="server" ImageUrl="~/Imagenes/Cancelar.png" AlternateText="Cancelar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbCancelar" OnClick="ImbCancelar_Click" />

            <asp:ImageButton ID="ImbBuscar" runat="server" ImageUrl="~/Imagenes/Buscar.png" AlternateText="Buscar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbBuscar" Enabled="False" />

            <asp:ImageButton ID="ImbModificar" runat="server" ImageUrl="~/Imagenes/Modificar.png" AlternateText="Modificar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbModificar" Enabled="False" />

            <asp:ImageButton ID="ImbImprimir" runat="server" ImageUrl="~/Imagenes/Printer.png" AlternateText="Imprimir"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbPrinter" Enabled="False" />

        </asp:Panel>

        <%-- ===== DATOS ===== --%>
        <asp:Panel ID="PnlDatos" runat="server" BorderStyle="Solid" BorderColor="Black"
            Width="780px" Height="349px" CssClass="PnlDatos"
            BackColor="#F1E3C0" BackImageUrl="~/Imagenes/Crema.jpg">

            <div class="em-form">

                <asp:FileUpload ID="fuploadImagen" runat="server" CssClass="em-file" accept="image/*"
                    onchange="emPreview(this);" />
                <asp:Image ID="imgPreview" runat="server" CssClass="em-img" />

                <asp:Label ID="LblNombre" runat="server" Text="Nombre.:" CssClass="em-lbl em-lbl-nombre"></asp:Label>
                <asp:TextBox ID="TxtNombre" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                    CssClass="em-txt em-txt-nombre"></asp:TextBox>

                <asp:Label ID="LblDireccion" runat="server" Text="Direccion.:" CssClass="em-lbl em-lbl-direccion"></asp:Label>
                <asp:TextBox ID="TxtDireccion" runat="server" AutoCompleteType="Disabled" MaxLength="200"
                    CssClass="em-txt em-txt-direccion"></asp:TextBox>

                <%-- Telefono y RNC son controles HTML (runat="server"): en el codigo se leen con .Value --%>
                <asp:Label ID="LblTelefono" runat="server" Text="Telefono.:" CssClass="em-lbl em-lbl-telefono"></asp:Label>
                <input type="text" id="TxtTelefono" runat="server" maxlength="14" autocomplete="off"
                    class="em-txt em-txt-telefono" oninput="this.value = this.value.replace(/[^0-9-]/g, '');" />

                <asp:Label ID="LblRNC" runat="server" Text="RNC.:" CssClass="em-lbl em-lbl-rnc"></asp:Label>
                <input type="text" id="TxtRNC" runat="server" maxlength="11" autocomplete="off"
                    class="em-txt em-txt-rnc" oninput="this.value = this.value.replace(/[^0-9]/g, '');" />

            </div>

        </asp:Panel>

        <%-- ===== BARRA DE MENSAJE ===== --%>
        <asp:Panel ID="PnlMensaje" runat="server" CssClass="em-mensaje">

            <asp:Image ID="ImgBarras" runat="server" ImageUrl="~/Imagenes/BarraAzul.png"
                CssClass="em-barra" Width="855px" Height="35px" />

            <asp:Label ID="LblMensaje2" runat="server" Text="Mensaje:" Font-Bold="True"
                ForeColor="Black" CssClass="em-lblmsg"></asp:Label>
        </asp:Panel>

    </div>

</asp:Content>