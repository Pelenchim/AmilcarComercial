$(document).ready(function () {
    $('.modal').modal({
        opacity: 0.5,
        inDuration: 350,
        outDuration: 250,
        ready: undefined,
        complete: undefined,
        dismissible: false,
        startingTop: '4%',
        endingTop: '10%'
    });
    $('.tooltipped').tooltip({ delay: 50 });
    $('.nueva-venta .articulosVenta .acciones .search').on('click', function () {
        $('.nueva-venta .articulosVenta .acciones input').toggle(500);
    });
    var compra = false, articulo = false;
    var total, subTotal, cantidadTotal, iva;
    mostrarDevolucionTmp();
    generales();
    mostrarProductosTmp(1);
    detectarCambios();
    $('.nueva-compra .detalle').on('change', '#iva', function (e) {
        iva = this.value;
        detalles();
    });
    detalles();
});
function generales() {
    $("#pre-General").css("display", "inline");
    $.ajax({
        url: '/devolucion/cliente/generales',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".datosGenerales").empty();
            $(".datosGenerales").append(
                '<div class="col l6">' +
                '<p class=""><strong>Devolucion: </strong>' + data[4] + '</p>' +
                '<p class="truncate"><strong>Gerente: </strong>' + data[1] + '</p>' +
                '</div>' +
                '<div class="col l6">' +
                '<p class=""><strong>Fecha: </strong>' + data[0] + '</p>' +
                '<p class=""><strong>Sucursal: </strong>' + data[2] + '</p>' +
                '</div>'
            );
            $("#N_factura").val(data[4]);
            $("#pre-General").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function ventas() {
    $(".lista-ventas").empty();
    $("#pre-ventas").css("display", "inline");
    $.ajax({
        url: '/obtener/ventas',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".lista-ventas").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".lista-ventas").append(
                        '<div class="col l4">' +
                        '<a onclick="agregarCompraExistente(' + value.ID + ')">' +
                        '<div class="card white grey-text text-darken-3 hoverable">' +
                        '<div class="card-content">' +
                        '<span class="card-title truncate">' + value.Proveedor + '</span>' +
                        '<p>Tel: ' + value.Factura + '</p>' +
                        '<p>Ruc: ' + value.ID + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</a>' +
                        '</div>'
                    );
                });
            });
            $("#pre-ventas").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    $('#ventas').modal('open');
}
function eliminarVenta() {
    var tipo = "Cliente";
    $.ajax({
        url: '/eliminar/devoluciontmp/' + tipo,
        type: 'GET',
        'success': function (data) {
            mostrarDevolucionTmp();
            Materialize.toast('Venta removida exitosamente.', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function mostrarDevolucionTmp() {
    $(".datosVenta").empty();
    $("#pre-Devolucion").css("display", "inline");
    var tipo = "Cliente";
    $.ajax({
        url: '/obtener/devoluciontmp/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".venta .opcionesVenta a").hide();
                $(".datosVenta").append(
                    '<div class="center-align vacio">' +
                    '<p>Aun no a definido una venta para esta devolucion</p>' +
                    '<button class="btn btn-flat green white-text" onclick="ventas()">Buscar</button>' +
                    '</div>'
                );
                proveedor = false;
            }
            else {
                $(".venta .opcionesVenta a").show();
                $(".datosVenta").empty();
                $(".datosVenta").append(
                    '<div class="col l7">' +
                    '<p><strong>Nombre: </strong>' + data.Comprador + '</p>' +
                    '</div>' +
                    '<div class="col l5">' +
                    '<p><strong>N° Ruc: </strong>' + data.Ruc + '</p>' +
                    '</div>' +
                    '<div class="col l12">' +
                    '<p><strong>Telefono: </strong>' + data.Telefono + '</p>' +
                    '</div>'
                );
                proveedor = true;
            }
            $("#pre-Devolucion").css("display", "none");
            proveedorSelected = true;
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
