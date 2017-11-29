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
                '<p class=""><strong>Factura: </strong>' + data[4] + '</p>' +
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
                        '<a onclick="agregarVentaExistente(' + value.ID + ')">' +
                        '<div class="card white grey-text text-darken-3 hoverable">' +
                        '<div class="card-content">' +
                        '<span class="card-title truncate">' + value.ClienteNombre + ' ' + value.ClienteApellido + '</span>' +
                        '<p>Tel: ' + value.Factura + '</p>' +
                        '<p>Ruc: ' + value.Cantidad + '</p>' +
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
function agregarVentaExistente(id) {
    $.ajax({
        url: '/agregar/devoluciontmpCliente/' + id,
        type: 'GET',
        'success': function (data) {
            mostrarDevolucionTmp();
            $('#ventas').modal('close');
            Materialize.toast('Venta definida exitosamente', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
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
    $(".datosCompra").empty();
    $("#pre-Devolucion").css("display", "inline");
    var tipo = "Cliente";
    $.ajax({
        url: '/obtener/devoluciontmpCliente/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".venta .opcionesVenta a").hide();
                $(".datosVenta").empty();
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
                    '<p class="truncate"><strong>Cliente: </strong>' + data.EntidadN + ' ' + data.EntidadA + '</p>' +
                    '<p class="hide id">' + data.ID + '</p>' +
                    '</div>' +
                    '<div class="col l5">' +
                    '<p><strong>Fecha Venta: </strong>' + data.Fecha + '</p>' +
                    '</div>' +
                    '<div class="col l12">' +
                    '<p><strong>Vendedor: </strong>' + data.UserN + ' ' + data.UserA + '</p>' +
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
function CancelVenta() {
    $('#ventas').modal('close');
    $(".lista-ventas").empty();
}

function abrirArticulos() {
    $('#articulos').modal('open');
    articulos(1);
}
function articulos(vista) {
    $(".lista-articulos").empty();
    $("#pre-Articulos").css("display", "inline");
    var id = $(".datosVenta .id").text();
    $.ajax({
        url: '/devoluciones/obtener/productosVenta/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            //if (data.length === 0) {
            //    $(".modal-compras .articulos .acciones .table-v").hide();
            //    $(".modal-compras .articulos .acciones .card-v").hide();
            //    $(".lista-articulos").empty();
            //    $(".lista-articulos").append(
            //        '<p class="center-align">No hay articulos para agregar a la compra </br> ya has agregado todos los articulos.</p>'
            //    );
            //    return;
            //}
            if (vista === 0) {
                articulosCard(data)
            }
            if (vista === 1) {
                articulosTable(data);
            }
            $("#pre-Articulos").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function CancelArticulos() {
    $('#articulos').modal('close');
    $(".lista-articulos").empty();
}
function articulosCard(data) {
    $.each(data, function () {
        $(".modal-compras .articulos .acciones .card-v").hide();
        $(".modal-compras .articulos .acciones .table-v").show();
        $(".lista-articulos").empty();
        $.each(this, function (name, value) {
            $(".lista-articulos").append(
                '<div class="col l4">' +
                '<div class="card">' +
                '<div class="row grey-text text-darken-3">' +
                '<div class="col l4">' +
                '</div>' +
                '<div class="col l8">' +
                '<span class="card-title">' + value.Nombre + '</span>' +
                '<p>Cod: ' + value.Codigo + '</p>' +
                '</div>' +
                '</div>' +
                '<div class="row blue-grey darken-2">' +
                '<input class="left browser-default" placeholder="Costo" id="precio' + value.ID + '" type="text"></input>' +
                '<input class="left browser-default" placeholder="Cant" id="cant' + value.ID + '" type="text"></input>' +
                '<input class="left browser-default" placeholder="Desc" id="desc' + value.ID + '" type="text" value="0"></input>' +
                '<a class="btn btn-flat green white-text right" onclick="agregarArticuloTmp(' + value.ID + ')"><i class="material-icons">add_shopping_cart</i></a>' +
                '</div>' +
                '</div>' +
                '</div>'
            );
        });
    });
}
function articulosTable(data) {
    $(".modal-compras .articulos .acciones .table-v").hide();
    $(".modal-compras .articulos .acciones .card-v").show();
    $(".lista-articulos").empty();
    $(".lista-articulos").append(
        '<table class="responsive-table bordered highlight centered white z-depth-1">' +
        '<thead>' +
        '<tr>' +
        '<th>Cod</th>' +
        '<th>Img</th>' +
        '<th>Nombre</th>' +
        '<th>Costo C$</th>' +
        '<th>Cantidad</th>' +
        '<th>Descuento</th>' +
        '<th class="right-align">Agregar</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '</table>'
    );
    $.each(data, function () {
        $.each(this, function (name, value) {
            $(".lista-articulos table tbody").append(
                '<tr>' +
                '<td>' + value.Codigo + '</td>' +
                '<td><img src="/Content/images/articulos/' + value.Imagen + '"></td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td>' +
                '<input placeholder="Precio" id="precio' + value.ID + '" type="text" class="browser-default" value="' + value.Costo + '" disabled>' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Cantidad" id="cant' + value.ID + '" type="text" class="browser-default" value="' + value.Cantidad + '" disabled>' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Descuento" id="desc' + value.ID + '" type="text" class="browser-default" value="' + value.Descuento + '" disabled>' +
                '</td > ' +
                '<td class="right-align">' +
                '<a class="btn btn-flat pink white-text" onclick="agregarArticuloTmp(' + value.ID + ')"><i class="material-icons">add_shopping_cart</i></a>' +
                '</td > ' +
                '</tr>'
            );
        });
    });
}
