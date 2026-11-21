<%@ Page Title="" Language="C#" MasterPageFile="~/Mantenimiento/MPMantenimiento.master" AutoEventWireup="true" CodeBehind="FrmClientes.aspx.cs" Inherits="FacturacionWeb.Mantenimiento.FrmClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contenidoPrincipal" runat="server">

    <style type="text/css">
        /* ===== Contenedor de referencia (igual que FrmCaja) ===== */
        #clWrap { position: relative; width: 855px; height: 600px; }
        #columnCentro h2 { margin: 10px 0 6px 0; }
       #columnCentro { min-height: 650px; }
#columnIzq, #ColumnDer { min-height: 675px; }

        /* ===== Botonera ===== */
        #clWrap .PnlBotonera { margin: 0; top: -42px; left: 900px; }
        #clWrap .PnlBotonera input { top: 0; left: 0; width: 116px; height: 30px; }
        #clWrap .ImgAceptar   { margin-top: 14px; }
        #clWrap .ImbCancelar  { margin-top: 114px; }
        #clWrap .ImbBuscar    { margin-top: 164px; }
        #clWrap .ImbModificar { margin-top: 214px; }
        #clWrap .ImbPrinter   { margin-top: 264px; }
        #clWrap .ImbNuevo { position: absolute; margin-top: 64px; margin-left: 14px; }

        /* ===== Panel de datos (recuadro beige ampliado) ===== */
        #clWrap .cl-datos { position: absolute; margin: 0; top: 8px; left: 54px; }
        .cl-form { position: relative; width: 775px; height: 525px; }

        .cl-lbl { position: absolute; font-weight: bold; font-size: 14px; color: #000; }
        .cl-txt { position: absolute; height: 22px; box-sizing: border-box; padding: 2px 6px;
                  border: 1px solid #7A7A7A; background: #FFF; color: #0066FF;
                  font-weight: bold; font-size: medium; }
        .cl-ddl { position: absolute; height: 24px; box-sizing: border-box; border: 1px solid #7A7A7A;
                  background: #FFF; color: #0066FF; font-weight: bold; font-size: medium; }

        /* Encabezado: Codigo / Tipo / Nombre */
        .cl-lbl-codigo { top: 16px; left: 15px; }
        .cl-txt-codigo { top: 12px; left: 80px; width: 65px; text-align: right; background: #F4F4F4; }
        .cl-lbl-tipo   { top: 16px; left: 208px; }
        .cl-ddl-tipo   { top: 12px; left: 308px; width: 154px; }
        .cl-lbl-nombre { top: 16px; left: 524px; }
        .cl-txt-nombre { top: 12px; left: 596px; width: 154px; }

        .cl-linea { position: absolute; top: 44px; left: 0; }

        /* Sub-paneles */
        .cl-pnl-izq { position: absolute; top: 45px; left: 0; }
        .cl-pnl-der { position: absolute; top: 45px; left: 392px; }

        /* Filas dentro de los sub-paneles */
        .cl-r1 { top: 11px; } .cl-r2 { top: 51px; } .cl-r3 { top: 91px; }
        .cl-r4 { top: 131px; } .cl-r5 { top: 171px; } .cl-r6 { top: 211px; }
        .cl-l  { left: 15px; }
        .cl-c  { left: 93px; width: 195px; }
        .cl-cd { left: 93px; width: 203px; }
        .cl-lbl.cl-r1 { top: 14px; } .cl-lbl.cl-r2 { top: 54px; } .cl-lbl.cl-r3 { top: 94px; }
        .cl-lbl.cl-r4 { top: 134px; } .cl-lbl.cl-r5 { top: 174px; } .cl-lbl.cl-r6 { top: 214px; }

        /* Grilla DENTRO del recuadro beige: gris solo cuando hay resultados */
        .cl-grid { position: absolute; left: 0; top: 291px; width: 775px; height: 234px;
           box-sizing: border-box; overflow: auto; border: 0;
           background-size: 100% 100%; background-repeat: no-repeat; }
        .cl-grid table { border-collapse: collapse; }
        .cl-grid th, .cl-grid td { padding: 4px 8px; text-align: left; font-size: 13px; white-space: nowrap; }

        /* Barra de mensaje */
        #clWrap .cl-mensaje { position: absolute; left: 0; top: 550px; width: 855px; height: 36px; }
        #clWrap .cl-barra   { display: block; }
        #clWrap .cl-lblmsg  { position: absolute; left: 74px; top: 9px; font-weight: bold; color: #000; }
    </style>

    <script type="text/javascript">
        function acceptNum(evt) {
            var key = evt.which || evt.keyCode;
            return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
        }
        var PAT_RNC = [/^\d{3}$/, /^\d{3}-\d{5}$/];
        var PAT_CED = [/^\d{3}$/, /^\d{3}-\d{7}$/];
        var PAT_TEL = [/^\d{3}$/, /^\d{3}-\d{3}$/];
        function autoGuion(input, pats) {
            var v = input.value;
            for (var i = 0; i < pats.length; i++) {
                if (pats[i].test(v)) { input.value = v + '-'; return; }
            }
        }
    </script>

    <h2>&nbsp;Crear Clientes</h2>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Timer ID="Timer1" runat="server" Interval="4000" Enabled="False" OnTick="Timer1_Tick"></asp:Timer>

    <div id="clWrap">

        <%-- ===== BOTONERA ===== --%>
        <asp:Panel ID="PnlBotonera" runat="server" BorderStyle="Solid" Width="138px" Height="640px"
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
            Width="775px" Height="525px" CssClass="cl-datos"
            BackColor="#F1E3C0" BackImageUrl="~/Imagenes/Crema.jpg">

            <div class="cl-form">

                <asp:Label ID="LblCodigo" runat="server" Text="Codigo.:" CssClass="cl-lbl cl-lbl-codigo"></asp:Label>
                <asp:TextBox ID="TxtCodigo" runat="server" ReadOnly="True" MaxLength="4" AutoCompleteType="Disabled" CssClass="cl-txt cl-txt-codigo"></asp:TextBox>

                <asp:Label ID="LblCliente" runat="server" Text="Tipo Cliente.:" CssClass="cl-lbl cl-lbl-tipo"></asp:Label>
                <asp:DropDownList ID="DdlTipo" runat="server" AutoPostBack="True" CssClass="cl-ddl cl-ddl-tipo"></asp:DropDownList>

                <asp:Label ID="LblNombre" runat="server" Text="Nombre.:" CssClass="cl-lbl cl-lbl-nombre"></asp:Label>
                <asp:TextBox ID="TxtNombre" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-txt-nombre"></asp:TextBox>

                <asp:Panel ID="PnlLinea1" runat="server" Width="769px" Height="0px" CssClass="cl-linea" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"></asp:Panel>

                <%-- ===== Empresarial ===== --%>
                <asp:Panel ID="PnlEmpresarial" runat="server" Width="388px" Height="242px" CssClass="cl-pnl-izq" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Visible="true">

                    <asp:Label ID="LblRNC" runat="server" Text="RNC.:" CssClass="cl-lbl cl-l cl-r1"></asp:Label>
                    <asp:Label ID="LblContacto" runat="server" Text="Contacto.:" CssClass="cl-lbl cl-l cl-r2"></asp:Label>
                    <asp:Label ID="LblCargo" runat="server" Text="Cargo.:" CssClass="cl-lbl cl-l cl-r3"></asp:Label>
                    <asp:Label ID="LblPagina" runat="server" Text="Pagina.:" CssClass="cl-lbl cl-l cl-r4"></asp:Label>

                    <input id="TxtRNCCliente" runat="server" type="text" class="cl-txt cl-c cl-r1" maxlength="11" autocomplete="off"
                        title="Introduzca el RNC de la compañia"
                        onfocus="this.style.borderColor='black'" onblur="this.style.borderColor=''"
                        onkeypress="return acceptNum(event)" onkeyup="autoGuion(this, PAT_RNC)" />

                    <asp:TextBox ID="TxtContacto" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-c cl-r2"></asp:TextBox>
                    <asp:TextBox ID="TxtCargo" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-c cl-r3"></asp:TextBox>
                    <asp:TextBox ID="TxtPagina" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-c cl-r4"></asp:TextBox>

                </asp:Panel>

                <%-- ===== Personal ===== --%>
                <asp:Panel ID="PnlPersonal" runat="server" Width="388px" Height="242px" CssClass="cl-pnl-izq" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Visible="false">

                    <asp:Label ID="LblApellido" runat="server" Text="Apellido.:" CssClass="cl-lbl cl-l cl-r1"></asp:Label>
                    <asp:Label ID="LblCedula" runat="server" Text="Cedula.:" CssClass="cl-lbl cl-l cl-r2"></asp:Label>

                    <asp:TextBox ID="TxtApellido" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-c cl-r1"></asp:TextBox>

                    <input id="TxtCedulaCliente" runat="server" type="text" class="cl-txt cl-c cl-r2" maxlength="13" autocomplete="off"
                        title="Introduzca la Cedula de la persona"
                        onfocus="this.style.borderColor='black'" onblur="this.style.borderColor=''"
                        onkeypress="return acceptNum(event)" onkeyup="autoGuion(this, PAT_CED)" />

                </asp:Panel>

                <%-- ===== Datos generales ===== --%>
                <asp:Panel ID="PnlDatosGeneral" runat="server" Width="379px" Height="242px" CssClass="cl-pnl-der" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px">

                    <asp:Label ID="LblDireccion" runat="server" Text="Direccion.:" CssClass="cl-lbl cl-l cl-r1"></asp:Label>
                    <asp:Label ID="LblPais" runat="server" Text="Pais.:" CssClass="cl-lbl cl-l cl-r2"></asp:Label>
                    <asp:Label ID="LblProvincia" runat="server" Text="Provincia.:" CssClass="cl-lbl cl-l cl-r3"></asp:Label>
                    <asp:Label ID="LblTelefono" runat="server" Text="Telefono.:" CssClass="cl-lbl cl-l cl-r4"></asp:Label>
                    <asp:Label ID="LblCelular" runat="server" Text="Celular.:" CssClass="cl-lbl cl-l cl-r5"></asp:Label>
                    <asp:Label ID="LblCorreo" runat="server" Text="Correo.:" CssClass="cl-lbl cl-l cl-r6"></asp:Label>

                    <asp:TextBox ID="TxtDireccion" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-c cl-r1"></asp:TextBox>

                    <asp:DropDownList ID="DdlPais" runat="server" ToolTip="Seleccione el pais deseado" AutoPostBack="true" TabIndex="10"
                        CssClass="cl-ddl cl-cd cl-r2" DataTextField="Pai_Nombre" DataValueField="Id_Pais"
                        OnSelectedIndexChanged="DdlPais_SelectedIndexChanged"></asp:DropDownList>

                    <asp:DropDownList ID="DdlProvincia" runat="server" ToolTip="Seleccione la provincia del pais" AutoPostBack="False" TabIndex="10"
                        CssClass="cl-ddl cl-cd cl-r3" DataTextField="Pro_Nombre" DataValueField="Id_Pais"></asp:DropDownList>

                    <asp:TextBox ID="TextBox1" runat="server" CssClass="cl-txt cl-cd cl-r2" Visible="False" ReadOnly="True"></asp:TextBox>
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="cl-txt cl-cd cl-r3" Visible="False" ReadOnly="True"></asp:TextBox>

                    <input id="TxtTelefono" runat="server" type="text" class="cl-txt cl-c cl-r4" maxlength="12" autocomplete="off"
                        title="Introduzca el telefono de la compañia"
                        onfocus="this.style.borderColor='black'" onblur="this.style.borderColor=''"
                        onkeypress="return acceptNum(event)" onkeyup="autoGuion(this, PAT_TEL)" />

                    <input id="TxtCelular" runat="server" type="text" class="cl-txt cl-c cl-r5" maxlength="12" autocomplete="off"
                        title="Introduzca el Celular de contacto"
                        onfocus="this.style.borderColor='black'" onblur="this.style.borderColor=''"
                        onkeypress="return acceptNum(event)" onkeyup="autoGuion(this, PAT_TEL)" />

                    <asp:TextBox ID="TxtCorreo" runat="server" MaxLength="50" AutoCompleteType="Disabled" CssClass="cl-txt cl-c cl-r6"></asp:TextBox>

                </asp:Panel>

                <%-- ===== GRILLA (dentro del recuadro beige) ===== --%>
<asp:Panel ID="PnlGridview" runat="server" CssClass="cl-grid" ToolTip="Buscar Registro" BackImageUrl="~/Imagenes/Gris.png">                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CellPadding="3" RowStyle-Wrap="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                        <HeaderStyle BackColor="#A55129" Font-Bold="True" Wrap="False" ForeColor="White" />
                        <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                        <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                        <SelectedRowStyle BackColor="#738A9C" ForeColor="White" Font-Bold="True" />
                        <SortedAscendingCellStyle BackColor="#FFF1D4" />
                        <SortedAscendingHeaderStyle BackColor="#B95C30" />
                        <SortedDescendingCellStyle BackColor="#F1E5CE" />
                        <SortedDescendingHeaderStyle BackColor="#93451F" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" ShowSelectButton="True" SelectImageUrl="../Imagenes/Seleccion.png" />
                            <asp:BoundField DataField="Cli_Codigo" HeaderText="Codigo" />
                            <asp:BoundField DataField="Cli_Tipo" HeaderText="Tipo Cliente" />
                            <asp:BoundField DataField="Cli_Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Cli_RNC" HeaderText="RNC" />
                            <asp:BoundField DataField="Cli_Contacto" HeaderText="Contacto" />
                            <asp:BoundField DataField="Cli_Cargo" HeaderText="Cargo" />
                            <asp:BoundField DataField="Cli_Pagina" HeaderText="Pagina" />
                            <asp:BoundField DataField="Cli_Direccion" HeaderText="Direccion" />
                            <asp:BoundField DataField="Cli_Pais" HeaderText="Pais" />
                            <asp:BoundField DataField="Cli_Provincia" HeaderText="Provincia" />
                            <asp:BoundField DataField="Cli_Telefono" HeaderText="Telefono" />
                            <asp:BoundField DataField="Cli_Celular" HeaderText="Celular" />
                            <asp:BoundField DataField="Cli_Correo" HeaderText="Correo" />
                            <asp:BoundField DataField="Cli_Apellido" HeaderText="Apellido" />
                            <asp:BoundField DataField="Cli_Cedula" HeaderText="Cedula" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

            </div>

        </asp:Panel>

        <%-- Panel que exige el designer; no se muestra --%>
        <asp:Panel ID="PnlCuadroGridview" runat="server" Visible="False"></asp:Panel>

        <%-- ===== BARRA DE MENSAJE ===== --%>
        <asp:Panel ID="PnlMensaje" runat="server" CssClass="cl-mensaje">
            <asp:Image ID="ImgBarras" runat="server" ImageUrl="~/Imagenes/BarraAzul.png" CssClass="cl-barra" Width="900px" Height="36px" />
            <asp:Label ID="LblMensaje2" runat="server" Text="Mensaje:" Font-Bold="True" ForeColor="Black" CssClass="cl-lblmsg"></asp:Label>
        </asp:Panel>

        <%-- Variables de sesion --%>
        <asp:Label ID="LblCodigo2" runat="server" Text="" Visible="False"></asp:Label>
        <asp:Label ID="LblUsuario" runat="server" Text="" Visible="False"></asp:Label>
        <asp:Label ID="Label1" runat="server" Text="" Visible="False"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="" Visible="False"></asp:Label>
        <asp:Label ID="LblDepartamento" runat="server" Text="" Visible="False"></asp:Label>
        <asp:Label ID="Label3" runat="server" Text="" Visible="False"></asp:Label>

    </div>

</asp:Content>