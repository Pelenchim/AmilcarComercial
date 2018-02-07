function buscar() {
    var fact = $(".consulta .form #search").val();

    if (fact == "") {
        Materialize.toast("Debe ingresar el numero de factura", 2000);
        return;
    }
    $(".detalle-consulta .vacio").hide();
    $(".detalle-consulta").empty();
    $("#pre-Consulta").css("display", "inline");
    $.ajax({
        url: '/compra/buscar/' + fact,
        type: 'GET',
        contentType: "application/json",
        'success': function (data) {
            if (data == false) {
                Materialize.toast("No existe el registro", 3000);
                $("#pre-Consulta").css("display", "none");
                $(".detalle-consulta .vacio").show();
                return;
            }
            $(".detalle-consulta").empty();
            var estado, color;
            if (data.Estado === true) {
                estado = "Aprovada";
                color = "green";
            }
            else {
                estado = "Anulada";
                color = "red";
            }
            $(".detalle-consulta").append(
                '<div class="maestro col l10 offset-l1">' +
                '<div class="col l4" style="padding-left:0">' +
                '<span>N° Factura: ' + data.Maestro.Factura + '</span>' +
                '</div>' +
                '<div class="col l4">' +
                '<span>Fecha: ' + data.Maestro.Fecha + '</span>' +
                '</div>' +
                '<div class="col l4">' +
                '<span>Estado: <span class="' + color + ' estado white-text">' + estado + '</span></span>' +
                '</div>' +
                '<div class="col l4" style="padding-left:0">' +
                '<span>Proveedor: ' + data.Maestro.Proveedor + '</span>' +
                '</div>' +
                '<div class="col l8">' +
                '<span>Comprador: ' + data.Maestro.CompradorN + ' ' + data.Maestro.CompradorA + '</span>' +
                '</div>' +
                '<div class="col l3" style="padding-left:0">' +
                '<span>Articulos: ' + data.Maestro.CantidadTotal + '</span>' +
                '</div>' +
                '<div class="col l3">' +
                '<span>SubTotal: C$' + (data.Maestro.Subtotal).toFixed(2) + '</span>' +
                '</div>' +
                '<div class="col l3">' +
                '<span>Iva: C$' + (data.Maestro.Iva).toFixed(2) + '</span>' +
                '</div>' +
                '<div class="col l3">' +
                '<span>Total: C$' + (data.Maestro.Total).toFixed(2) + '</span>' +
                '</div>' +
                '</div>' +
                '<div class="divider-mio col l10 offset-l1"></div>'
            );
            $(".detalle-consulta").append(
                '<div class="col l11 offset-l1"><h5>Detalle de la Compra</h5></div>' +
                '<table class="responsive-table centered white z-depth-1 bordered highlight col l10 offset-l1">' +
                '<thead>' +
                '<tr>' +
                '<th>Nombre</th>' +
                '<th>Imagen</th>' +
                '<th>Cantidad</th>' +
                '<th>Costo</th>' +
                '<th>Descuento</th>' +
                '<th>SubTotal</th>' +
                '</tr>' +
                '</thead>' +
                '<tbody>' +
                '</tbody>' +
                '</table>'
            );
            $.each(data.Detalle, function (name, value) {
                $(".detalle-consulta table tbody").append(
                    '<tr>' +
                    '<td>' + value.Articulo + '</td>' +
                    '<td><img src="/Content/images/articulos/' + value.Img + '"></td>' +
                    '<td>' + value.Cantidad + '</td>' +
                    '<td>C$ ' + value.Costo + '</td>' +
                    '<td>C$ ' + value.Descuento + '</td>' +
                    '<td>C$ ' + value.Subtotal + '</td>' +
                    '</tr>'
                );
            });
            $("#pre-Consulta").css("display", "none");
        },
        'error': function (request, error) {
            $(".detalle-consulta .vacio").show();
            $("#pre-Consulta").css("display", "none");
            Materialize.toast("No existe el registro", 3000);
        }
    });
}
