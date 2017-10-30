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
    $('select').material_select();
    $('.nueva-venta .articulosVenta .acciones .search').on('click', function () {
        $('.nueva-venta .articulosVenta .acciones input').toggle(500);
    });
    mostrarProveedorTmp();
    generales();
    mostrarProductosTmp(1);
});

function generales() {
    $.ajax({
        url: '/compra/obtener/generales',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".datosGenerales").empty();
            $(".datosGenerales").append(
                '<div class="col l6">' +
                '<p class=""><strong>Compra N°: </strong>' + data[3] + '</p>' +
                '<p class=""><strong>Gerente: </strong>' + data[1] + '</p>' +
                '</div>' +
                '<div class="col l6">' +
                '<p class=""><strong>Fecha: </strong>' + data[0] + '</p>' +
                '<p class=""><strong>Sucursal: </strong>' + data[2] + '</p>' +
                '</div>'
            );
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function proveedores() {
    $.ajax({
        url: '/compras/obtener/proveedores',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".lista-proveedores").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".lista-proveedores").append(
                        '<div class="col l4">' +
                        '<a onclick="agregarProveedorExistente(' + value.ID + ')">' +
                        '<div class="card white grey-text text-darken-3 hoverable">' +
                        '<div class="card-content">' +
                        '<span class="card-title truncate">' + value.Nombre + '</span>' +
                        '<p>Tel: ' + value.Telefono + '</p>' +
                        '<p>Ruc: ' + value.Ruc + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</a>' +
                        '</div>'
                    );
                });
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    $('#proveedores').modal('open');
}
function agregarProveedor() {
    $('#nuevoProveedor').modal('open');
}
function agregarProveedorTmp() {
    var datos = {
        Nombre: $(".nuevoCliente #nombre_cliente").val(),
        Apellido: $('.nuevoCliente #apellidos_cliente').val(),
        Direccion: $('.nuevoCliente #direccion').val(),
        Departamento: $('.nuevoCliente #departamento').val(),
        Telefono: $('.nuevoCliente #telefono').val(),
        Cedula: $('.nuevoCliente #cedula').val()
    };
    $.ajax({
        url: '/compras/agregar/proveedorTmp',
        type: 'POST',
        data: datos,
        'success': function (data) {
            mostrarClienteTmp();
            $('#nuevoProveedor').modal('close');
            Materialize.toast('Proveedor agregado exitosamente', 4000);
            $(".nuevoCliente #nombre_cliente").val('');
            $('.nuevoCliente #apellidos_cliente').val('');
            $('.nuevoCliente #direccion').val('');
            $('.nuevoCliente #departamento').val('');
            $('.nuevoCliente #telefono').val('');
            $('.nuevoCliente #cedula').val('');
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function agregarProveedorExistente(id) {
    $.ajax({
        url: '/compras/agregar/ProveedorExistente/' + id,
        type: 'GET',
        'success': function (data) {
            mostrarProveedorTmp();
            $('#proveedores').modal('close');
            Materialize.toast('Proveedor definido exitosamente', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function editarProveedor() {
    $('#editarProveedor').modal('open');
    $.ajax({
        url: '/compras/editar/proveedorTmp',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $.each(data, function () {
                $(".editarCliente #nombre_cliente").val(data.Nombre);
                $(".editarCliente #apellidos_cliente").val(data.Apellido)
                $(".editarCliente #cedula").val(data.Cedula)
                $(".editarCliente #telefono").val(data.Telefono)
                $(".editarCliente #departamento").val(data.Departamento)
                $(".editarCliente #direccion").val(data.Direccion)
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function editarGuardarProveedor() {
    var datos = {
        Nombre: $(".nuevoCliente #nombre_cliente").val(),
        Apellido: $('.nuevoCliente #apellidos_cliente').val(),
        Direccion: $('.nuevoCliente #direccion').val(),
        Departamento: $('.nuevoCliente #departamento').val(),
        Telefono: $('.nuevoCliente #telefono').val(),
        Cedula: $('.nuevoCliente #cedula').val()
    };
    $.ajax({
        url: '/compras/editar/proveedorGuardarTmp',
        type: 'POST',
        data: datos,
        'success': function (data) {
            mostrarClienteTmp();
            $('#editarProveedor').modal('close');
            Materialize.toast('Proveedor editado exitosamente', 4000);
            $(".nuevoCliente #nombre_cliente").val('');
            $('.nuevoCliente #apellidos_cliente').val('');
            $('.nuevoCliente #direccion').val('');
            $('.nuevoCliente #departamento').val('');
            $('.nuevoCliente #telefono').val('');
            $('.nuevoCliente #cedula').val('');
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function eliminarProveedor() {
    $.ajax({
        url: '/compras/eliminar/proveedorTmp',
        type: 'GET',
        'success': function (data) {
            mostrarProveedorTmp();
            Materialize.toast('Proveedor removido exitosamente.', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function mostrarProveedorTmp() {
    $.ajax({
        url: '/compras/obtener/proveedorTmp',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".proveedor .opcionesProveedor a").hide();
                $(".datosProveedor").empty();
                $(".datosProveedor").append(
                    '<div class="center-align vacio">' +
                    '<p>Aun no a definido cliente para esta venta</p>' +
                    '<button class="btn btn-flat green white-text" onclick="agregarProveedor()">Agregar</button>' + ' ' +
                    '<button class="btn btn-flat green white-text" onclick="proveedores()">Buscar</button>' +
                    '</div>'
                );
            }
            else {
                $(".proveedor .opcionesProveedor a").show();
                $(".datosProveedor").empty();
                $(".datosProveedor").append(
                    '<div class="col l12">' +
                    '<p><strong>Nombre: </strong>' + data.Nombre + '</p>' +
                    '</div>' +
                    '<div class="col l4">' +
                    '<p><strong>Telefono: </strong>' + data.Telefono + '</p>' +
                    '</div>' +
                    '<div class="col l4">' +
                    '<p><strong>N° Ruc: </strong>' + data.Ruc + '</p>' +
                    '</div>'
                );
            }
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function CancelProveedor() {
    $('#proveedores').modal('close');
    $(".lista-proveedores").empty();
}
function CancelNuevoProveedor() {
    $('#nuevoProveedor').modal('close');
}

function abrirArticulos() {
    $('#articulos').modal('open');
    articulos(1);
}
function articulos(vista) {
    $.ajax({
        url: '/compras/obtener/productos',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (vista === 0) {
                articulosCard(data)
            }
            if (vista === 1) {
                articulosTable(data);
            }
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
};
function CancelArticulos() {
    $('#articulos').modal('close');
    $(".lista-articulos").empty();
}
function agregarArticuloTmp(id) {
    var cant = $(".articulos #cant" + id).val();
    var precio = $(".articulos #precio" + id).val();
    var desc = $(".articulos #desc" + id).val();
    $.ajax({
        url: '/compras/agregar/producto/' + id + '/' + cant + '/' + precio + '/' + desc,
        type: 'GET',
        'success': function (data) {
            $('#articulos').modal('close');
            mostrarProductosTmp(1);
            Materialize.toast('Articulo agregado a la orden', 2000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function eliminarProductoTmp(id) {
    $.ajax({
        url: '/compras/eliminar/productoTmp/' + id,
        type: 'GET',
        'success': function (data) {
            mostrarProductosTmp(1);
            Materialize.toast('Articulo eliminado de la compra', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function eliminarProductosTodos() {
    $.ajax({
        url: '/compras/eliminar/eliminarProductosTodos',
        type: 'GET',
        'success': function (data) {
            mostrarProductosTmp(0);
            Materialize.toast('A eliminado todos los articulos de la compra', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function mostrarProductosTmp(view) {
    $.ajax({
        url: '/compras/obtener/productosTmp',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".articulos-orden").empty();
            $(".articulos-orden").append(
                '<div class="divider-mio"></div>' +
                '<div class="articulosLista col l12 mCustomScrollbar blue" data-mcs-theme="minimal-dark"></div>'
            );
            if (data === 0) {
                $(".opcionesArticulos .card-v").hide();
                $(".opcionesArticulos .table-v").hide();
                $(".opcionesArticulos .delete").hide();
                $(".opcionesArticulos .search").hide();
                $(".articulos-orden .articulosLista").empty();
                $(".articulos-orden .articulosLista").append(
                    '<div class="center" style="margin-top:15vh; margin-bottom:5vh;">' +
                    '<p class="white-text">No hay articulos agregados a la orden, por favor seleccione o agregue los </br> articulos para poder realizar la compra.</p>' +
                    '<a class="btn btn-large white grey-text text-darken-2" onclick="abrirArticulos()">Seleccionar</a>' +
                    '</div > '
                );
                detalles(0, 0);
            }
            else {
                if (view === 0) {
                    articulosOrdenCard(data);
                }
                if (view === 1) {
                    $(".articulos-orden .articulosLista").css("padding", "0px");
                    articulosOrdenTable(data);
                }
            }
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
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
                '<td>' + value.Imagen + '</td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td>' +
                '<input placeholder="Precio" id="precio' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Cantidad" id="cant' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Descuento" id="desc' + value.ID + '" type="text" class="browser-default" value="0">' +
                '</td > ' +
                '<td class="right-align">' +
                '<a class="btn btn-flat pink white-text" onclick="agregarArticuloTmp(' + value.ID + ')"><i class="material-icons">add_shopping_cart</i></a>' +
                '</td > ' +
                '</tr>'
            );
        });
    });
}
function articulosOrdenCard(data) {
    $(".articulosCompra .opcionesArticulos .table-v").show();
    $(".articulosCompra .opcionesArticulos .delete").show();
    $(".articulosCompra .opcionesArticulos .search").show();
    $(".articulosCompra .opcionesArticulos .card-v").hide();
    $(".articulos-orden .articulosLista").toggleClass("blue white");
    $(".articulosCompra > div > div").addClass("z-depth-1");
    $(".lista-articulos").empty();
    $.each(data, function () {
        $.each(this, function (name, value) {
            $(".articulos-orden .articulosLista").append(
                '<div class="col l3">' +
                '<div class="card hoverable">' +
                '<div class="card-image">' +
                '<div class="top">' +
                '<span class="left white-text">Cod: 001</span>' +
                '<a class="right" onclick="eliminarProductoTmp(' + value.ID + ')"><i class="material-icons">cancel</i></a>' +
                '</div>' +
                '<img src="https://lorempixel.com/100/190/nature/6">' +
                '<span class="card-title">' + value.Nombre + '</span>' +
                '</div>' +
                '<div class="card-content">' +
                '<div>' +
                '<label class="grey-text text-darken-2">Cantidad</label>' +
                '<input class="browser-default" type="text"></input>' +
                '</div>' +
                '<div>' +
                '<label class="grey-text text-darken-2">Sub-Total</label>' +
                '<input class="browser-default" type="text"></input>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>'
            );
        });
    });
}
function articulosOrdenTable(data) {
    $(".articulosCompra .opcionesArticulos .table-v").hide();
    $(".articulosCompra .opcionesArticulos .delete").show();
    $(".articulosCompra .opcionesArticulos .search").show();
    $(".articulosCompra .opcionesArticulos .card-v").show();
    $(".articulos-orden .articulosLista").toggleClass("blue white");
    $(".lista-articulos").empty();
    $(".articulos-orden .articulosLista").append(
        '<table class="bordered highlight responsive-table centered white">' +
        '<thead class="z-depth-1">' +
        '<tr>' +
        '<th>Cod</th>' +
        '<th>Img</th>' +
        '<th>Nombre</th>' +
        '<th>Precio</th>' +
        '<th>Cantidad</th>' +
        '<th>Descuento</th>' +
        '<th>Subtotal</th>' +
        '<th>Eliminar</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '</table>'
    );
    cantidadTotal = 0;
    subTotal = 0;
    $.each(data, function () {
        $.each(this, function (name, value) {
            $(".articulosLista table tbody").append(
                '<tr>' +
                '<td>' + value.ID + '</td>' +
                '<td>' + '<img src="https://lorempixel.com/100/190/nature/6">' + '</td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td>' +
                '<input class="browser-default precio" type="text" id="precio' + value.ID + '" value="' + value.Precio + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="cant' + value.ID + '" value="' + value.Cantidad + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="desc' + value.ID + '" value="' + value.Descuento + '"></input>' +
                '</td>' +
                '<td>C$' + ((value.Precio * value.Cantidad) + value.Descuento) + '</td>' +
                '<td>' + '<a class="center" onclick="eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' + '</td>' +
                '</tr>'
            );
            cantidadTotal = cantidadTotal + value.Cantidad;
            subTotal = subTotal + (value.Precio * value.Cantidad);
        });
    });
    detalles(cantidadTotal, subTotal);
}

function detalles(cantidadTotal, SubTotal) {
    iva = (subTotal * parseFloat($('#iva').val() / 100));
    Total = subTotal + iva;
    $(".detalle .detalles").empty();
    $(".detalle .detalles").append(
        '<p><strong>Detalle:</strong></p>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Articulos: </strong>' + cantidadTotal + '</span>' +
        '</div>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Iva: C$</strong>' + iva + '</span>' +
        '</div>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Subtotal: C$</strong>' + subTotal + '</span>' +
        '</div>'
    );
    $(".detalle .total").text("Total: C$" + Total);
}
function cancelarCompra() {
    $.ajax({
        url: '/compras/cancelar',
        type: 'POST',
        contentType: "application/json",
        dataType: "json",
        crossDomain: true,
        success: function (data) {
            window.location.href = data;
        }
    });
}
function facturar() {

    if ($("#N_factura").empty()){
        Materialize.toast('No a ingresado el numero de la factura', 2000);
    }

    var datos = {
        fact_compra: $("#N_factura").val(),
        tipo_comprobante_compra: $('#comprobante').val(),
        iva_compra: $('#iva').val()
    };

    $.ajax({
        url: '/compras/facturar',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        data: datos,
        'success': function (data) {
            if (data === true)
            {
                window.location.href = "/Compras/Index";
            }
            else {
                Materialize.toast('Error, no se pudo realizar la compra', 2000);
            }
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}