<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/MPMantenimiento.master" AutoEventWireup="true" CodeBehind="FrmCaja.aspx.cs" Inherits="FacturacionWeb.Mantenimiento.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contenidoPrincipal" runat="server">

    <%-- Estilos propios del formulario. Todo cuelga de #cjWrap (contenedor de referencia),
         asi la botonera, el panel de datos y la barra de mensaje quedan posicionados
         dentro de un mismo marco y no dependen de margenes negativos encadenados. --%>
    <style type="text/css">
        /* ===== Contenedor de referencia ===== */
        #cjWrap { position: relative; width: 855px; height: 421px; }
        #columnCentro h2 { margin: 10px 0 6px 0; }
        #columnCentro { min-height: 465px; }
        #columnIzq, #ColumnDer { min-height: 490px; }

        /* ===== Botonera (panel negro a la derecha) ===== */
        #cjWrap .PnlBotonera { margin: 0; top: -42px; left: 900px; }
        #cjWrap .PnlBotonera input { top: 0; left: 0; width: 116px; height: 30px; }
        #cjWrap .ImgAceptar   { margin-top: 14px; }
        #cjWrap .ImbCancelar  { margin-top: 114px; }
        #cjWrap .ImbBuscar    { margin-top: 164px; }
        #cjWrap .ImbModificar { margin-top: 214px; }
        #cjWrap .ImbPrinter   { margin-top: 264px; }
        /* Nuevo: en Theme1.css la clase se llama .ImgNuevo y aqui es .ImbNuevo, por eso se montaba sobre Aceptar */
        #cjWrap .ImbNuevo { position: absolute; margin-top: 64px; margin-left: 14px; }

        /* ===== Panel de datos (recuadro beige) ===== */
        #cjWrap .PnlDatos { margin: 0; top: 8px; left: 54px; }

        .cj-form { position: relative; width: 790px; height: 363px; }

        .cj-lbl  { position: absolute !important; left: 225px !important; font-weight: bold; font-size: 14px; color: #000; }
        .cj-txt  { position: absolute !important; left: 318px !important; width: 220px; height: 22px; box-sizing: border-box;
                   padding: 2px 6px; border: 1px solid #7A7A7A; background: #FFF; color: #0066FF;
                   font-weight: bold; font-size: medium; }

        .cj-lbl-codigo      { top: 47px !important; }
        .cj-lbl-referencia  { top: 83px !important; }
        .cj-lbl-descripcion { top: 119px !important; }

        .cj-txt-codigo      { top: 43px !important;  width: 80px; text-align: right; background: #F4F4F4; }
        .cj-txt-referencia  { top: 79px !important; }
        .cj-txt-descripcion { top: 115px !important; }

        /* Selector de archivo y cuadro de imagen (igual que Datos Empresa) */
        .cj-file { position: absolute !important; left: 4px !important; top: 6px !important; font-weight: bold; color: #000; font-size: 13px; }
        .cj-file::file-selector-button { font-weight: normal; }
        .cj-img  { position: absolute !important; left: 24px !important; top: 40px !important; width: 180px; height: 100px;
                   border: 3px solid #000; box-sizing: border-box; object-fit: contain; }

        /* Grilla: el fondo gris solo aparece cuando hay resultados (tabla dentro) */
        .cj-grid { position: absolute !important; left: 4px !important; top: 203px !important;
                   width: 780px; height: 158px; overflow: auto; background: transparent; box-sizing: border-box; }
        .cj-grid:has(table) { background: #999999; }
        .cj-grid table { border-collapse: collapse; }
        .cj-grid th, .cj-grid td { padding: 4px 8px; text-align: left; font-size: 13px; }

        /* ===== Barra de mensaje (debajo del recuadro beige, como en Datos Empresa) ===== */
#cjWrap .cj-mensaje { position: absolute; left: 0; top: 385px; width: 855px; height: 36px; }        #cjWrap .cj-barra   { display: block; }
        #cjWrap .cj-lblmsg  { position: absolute; left: 74px; top: 9px; font-weight: bold; color: #000; }
    </style>

    <script type="text/javascript">
        // Vista previa inmediata de la imagen elegida dentro del cuadro negro
        function cjPreview(input) {
            var img = document.getElementById('<%= ImgCaja.ClientID %>');
            if (input.files && input.files[0]) { img.src = URL.createObjectURL(input.files[0]); }
        }
    </script>

    <h2>&nbsp;Crear Cajas</h2>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" Interval="3000" Enabled="false" OnTick="Timer1_Tick"></asp:Timer>

    <div id="cjWrap">

        <%-- ===== BOTONERA (orden: Aceptar, Nuevo, Cancelar, Buscar, Modificar, Imprimir) ===== --%>
        <asp:Panel ID="PnlBotonera" runat="server" BorderStyle="Solid" Width="138px" Height="460px"
            CssClass="PnlBotonera" BackColor="Black" BackImageUrl="~/Imagenes/Negra.png">

            <asp:ImageButton ID="ImgAceptar" runat="server" ImageUrl="~/Imagenes/Aceptar.png" AlternateText="Aceptar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImgAceptar" Enabled="True" OnClick="ImgAceptar_Click" />

            <asp:ImageButton ID="ImbNuevo" runat="server" ImageUrl="~/Imagenes/Nuevo.png" AlternateText="Nuevo"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbNuevo" Enabled="False" OnClick="ImbNuevo_Click" />

            <asp:ImageButton ID="ImbCancelar" runat="server" ImageUrl="~/Imagenes/Cancelar.png" AlternateText="Cancelar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbCancelar" OnClick="ImbCancelar_Click" />

            <asp:ImageButton ID="ImbBuscar" runat="server" ImageUrl="~/Imagenes/Buscar.png" AlternateText="Buscar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbBuscar" Enabled="True" OnClick="ImbBuscar_Click" />

            <asp:ImageButton ID="ImbModificar" runat="server" ImageUrl="~/Imagenes/Modificar.png" AlternateText="Modificar"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbModificar" Enabled="False" OnClick="ImbModificar_Click" />

            <asp:ImageButton ID="ImbImprimir" runat="server" ImageUrl="~/Imagenes/Printer.png" AlternateText="Imprimir"
                BorderColor="#999999" BorderStyle="Solid" CssClass="ImbPrinter" Enabled="False" />

        </asp:Panel>

        <%-- ===== DATOS ===== --%>
        <asp:Panel ID="PnlDatos" runat="server" BorderStyle="Solid" BorderColor="Black"
            Width="790px" Height="363px" CssClass="PnlDatos"
            BackColor="#F1E3C0" BackImageUrl="~/Imagenes/Crema.jpg">

            <div class="cj-form">

                <asp:FileUpload ID="FuImagen" runat="server" CssClass="cj-file" accept="image/*"
                    onchange="cjPreview(this);" Visible="False" />
                <asp:Image ID="ImgCaja" runat="server" CssClass="cj-img" Visible="False" />

                <asp:Label ID="LblCodigo" runat="server" Text="Codigo.:" CssClass="cj-lbl cj-lbl-codigo"></asp:Label>
                <asp:TextBox ID="TxtCodigo" runat="server" ReadOnly="True" AutoCompleteType="Disabled"
                    CssClass="cj-txt cj-txt-codigo"></asp:TextBox>

                <asp:Label ID="LblReferencia" runat="server" Text="Referencia.:" CssClass="cj-lbl cj-lbl-referencia"></asp:Label>
                <asp:TextBox ID="TxtReferencia" runat="server" AutoCompleteType="Disabled" MaxLength="50"
                    CssClass="cj-txt cj-txt-referencia"></asp:TextBox>

                <asp:Label ID="LblDescripcion" runat="server" Text="Descripcion.:" CssClass="cj-lbl cj-lbl-descripcion"></asp:Label>
                <asp:TextBox ID="TxtDescripcion" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                    CssClass="cj-txt cj-txt-descripcion"></asp:TextBox>

<asp:Panel ID="PnlGrid" runat="server" CssClass="cj-grid" BackImageUrl="~/Imagenes/Gris.png">                    <asp:GridView ID="GvCajas" runat="server" AutoGenerateColumns="False"
                        DataKeyNames="Caj_Codigo" OnSelectedIndexChanged="GvCajas_SelectedIndexChanged"
                        GridLines="None" Width="100%" ForeColor="#222222">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" />
                            <asp:BoundField DataField="Caj_Codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="Caj_Referencia" HeaderText="Referencia" />
                            <asp:BoundField DataField="Caj_Descripcion" HeaderText="Descripcion" />
                        </Columns>
                        <HeaderStyle BackColor="#FF7700" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#FFF7E7" />
                        <AlternatingRowStyle BackColor="#FFFFFF" />
                        <SelectedRowStyle BackColor="#FFCC99" Font-Bold="True" />
                    </asp:GridView>
                </asp:Panel>

            </div>

        </asp:Panel>

        <%-- ===== BARRA DE MENSAJE ===== --%>
        <asp:Panel ID="PnlMensaje" runat="server" CssClass="cj-mensaje">

    <asp:Image ID="ImgBarras" runat="server" ImageUrl="~/Imagenes/BarraAzul.png"
        CssClass="cj-barra" Width="900px" Height="36px" />

    <asp:Label ID="LblMensaje2" runat="server" Text="Mensaje:" Font-Bold="True"
        ForeColor="Black" CssClass="cj-lblmsg"></asp:Label>
</asp:Panel>

    </div>

</asp:Content>
