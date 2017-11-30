﻿$(document).ready(function () {
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
    //detectarCambios();
    $('.nueva-compra .detalle').on('change', '#iva', function (e) {
        iva = this.value;
        detalles();
    });
    //detalles();
});
function generales() {
    $("#pre-General").css("display", "inline");
    $.ajax({
        url: '/devolucion/proveedor/generales',
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
function compras() {
    $(".lista-compras").empty();
    $("#pre-compras").css("display", "inline");
    $.ajax({
        url: '/obtener/compras',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".lista-compras").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".lista-compras").append(
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
            $("#pre-compras").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    $('#compras').modal('open');
}
function agregarCompraExistente(id) {
    $.ajax({
        url: '/agregar/devoluciontmpProveedor/' + id,
        type: 'GET',
        'success': function (data) {
            mostrarDevolucionTmp();
            $('#compras').modal('close');
            Materialize.toast('Compra definida exitosamente', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function eliminarCompra() {
    var tipo = "Proveedor";
    $.ajax({
        url: '/eliminar/devoluciontmp/' + tipo,
        type: 'GET',
        'success': function (data) {
            mostrarDevolucionTmp();
            Materialize.toast('Compra removida exitosamente.', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function mostrarDevolucionTmp() {
    $(".datosCompra").empty();
    $("#pre-Devolucion").css("display", "inline");
    var tipo = "Proveedor";
    $.ajax({
        url: '/obtener/devoluciontmpProveedor/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".compra .opcionesCompra a").hide();
                $(".datosCompra").empty();
                $(".datosCompra").append(
                    '<div class="center-align vacio">' +
                    '<p>Aun no a definido una compra para esta devolucion</p>' +
                    '<button class="btn btn-flat green white-text" onclick="compras()">Buscar</button>' +
                    '</div>'
                );
                proveedor = false;
            }
            else {
                $(".compra .opcionesCompra a").show();
                $(".datosCompra").empty();
                $(".datosCompra").append(
                    '<div class="col l6">' +
                    '<p><strong>Proveedor: </strong>' + data.Entidad + '</p>' +
                    '<p class="hide id">' + data.ID + '</p>' +
                    '</div>' +
                    '<div class="col l6">' +
                    '<p><strong>Fecha Compra: </strong>' + data.Fecha + '</p>' +
                    '</div>' +
                    '<div class="col l12">' +
                    '<p><strong>Comprador: </strong>' + data.UserN + ' ' + data.UserA + '</p>' +
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
function CancelCompra() {
    $('#compras').modal('close');
    $(".lista-compras").empty();
}

function abrirArticulos() {
    $('#articulos').modal('open');
    articulos(1);
}
function articulos(vista) {
    $(".lista-articulos").empty();
    $("#pre-Articulos").css("display", "inline");
    var id = $(".datosCompra .id").text();
    $.ajax({
        url: '/devoluciones/obtener/productosCompra/' + id,
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
        '<th>Cantidad Comprada</th>' +
        '<th>Cantidad Devolver</th>' +
        '<th>Descripcion</th>' +
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
                '<td>' + value.Costo + '</td>' +
                '<td><span id="cantidad' + value.ID + '">' + value.Cantidad + '</span> Unids</td>' +
                '<td>' +
                '<input placeholder="Cantidad" id="cant' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Descripcion" id="descrip' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td class="right-align">' +
                '<a class="btn btn-flat pink white-text" onclick="agregarArticuloTmp(' + value.ID + ')"><i class="material-icons">add_shopping_cart</i></a>' +
                '</td > ' +
                '</tr>'
            );
        });
    });
}

function agregarArticuloTmp(id) {
    var compradas = $(".articulos #cantidad" + id).text();
    var cant = $(".articulos #cant" + id).val();
    var descrip = $(".articulos #descrip" + id).val();
    if (cant === '') {
        Materialize.toast("Debe agregar la cantidad", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (cant === 0) {
        Materialize.toast("La cantidad no puede ser igual a 0", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (cant < 0) {
        Materialize.toast("La cantidad no es valida", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (cant > compradas) {
        Materialize.toast("La cantidad a devolver no puede ser mayor a la comprada", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (!/^([0-9])*$/.test(cant)) {
        Materialize.toast("El valor no es valido", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (descrip === '') {
        Materialize.toast("Debe agregar una descripcion", 2000);
        $(".articulos #descrip" + id).focus();
        return;
    }
    var tipo = "Proveedor";
    $.ajax({
        url: '/devoluciones/agregar/producto/' + id + '/' + cant + '/' + descrip + '/' + tipo,
        type: 'GET',
        'success': function (data) {
            CancelArticulos();
            mostrarProductosTmp(1);
            Materialize.toast('Articulo agregado a la orden', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function eliminarProductoTmp(id) {
    $.ajax({
        url: '/ventas/eliminar/productoTmp/' + id,
        type: 'GET',
        'success': function (data) {
            mostrarProductosTmp(1);
            Materialize.toast('Articulo eliminado de la orden', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function eliminarProductosTodos() {
    $.ajax({
        url: '/ventas/eliminar/eliminarProductosTodos',
        type: 'GET',
        'success': function (data) {
            mostrarProductosTmp(1);
            Materialize.toast('A eliminado todos los articulos de la orden', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    cantidadTotal = 0;
    subTotal = 0;
    detalles();
}
function mostrarProductosTmp(view) {
    cantidadTotal = 0;
    $(".articulos-orden").empty();
    $("#pre-ArticulosOrden").css("display", "inline");
    var tipo = "Proveedor";
    $.ajax({
        url: '/devoluciones/obtener/productosTmp/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".articulos-devolucion").append(
                '<div class="divider-mio"></div>' +
                '<div class="articulosLista col l12 mCustomScrollbar blue" data-mcs-theme="minimal-dark"></div>'
            );
            if (data === 0) {
                $(".opcionesArticulos .card-v").hide();
                $(".opcionesArticulos .table-v").hide();
                $(".opcionesArticulos .delete").hide();
                $(".opcionesArticulos .search").hide();
                $(".articulos-devolucion .articulosLista").empty();
                $(".articulos-devolucion .articulosLista").append(
                    '<div class="center" style="margin-top:15vh; margin-bottom:5vh;">' +
                    '<p class="white-text">No hay articulos agregados a la lista, por favor seleccione los </br> articulos para poder realizar la devolucion.</p>' +
                    '<a class="btn btn-large white grey-text text-darken-2" onclick="abrirArticulos()">Seleccionar</a>' +
                    '</div > '
                );
                detalles();
                articulo = false;
                $("#pre-ArticulosOrden").css("display", "none");
            }
            else {
                if (view === 0) {
                    articulosOrdenCard(data);
                }
                if (view === 1) {
                    articulosOrdenTable(data);
                }
                articulo = true;
            }
            $("#pre-ArticulosOrden").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function articulosOrdenCard(data) {
    $(".articulosVenta .opcionesArticulos .table-v").show();
    $(".articulosVenta .opcionesArticulos .card-v").hide();
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
    $(".articulosVenta .opcionesArticulos .table-v").hide();
    $(".articulosVenta .opcionesArticulos .card-v").show();
    $(".articulosVenta .opcionesArticulos .delete").show();
    $(".articulosVenta .opcionesArticulos .search").show();
    $(".articulos-devolucion .articulosLista").toggleClass("blue white");
    $(".lista-articulos").empty();
    $(".articulos-devolucion .articulosLista").append(
        '<table class="bordered highlight centered responsive-table white">' +
        '<thead class="z-depth-1">' +
        '<tr>' +
        '<th>Cod</th>' +
        '<th>Img</th>' +
        '<th>Nombre</th>' +
        '<th>Existencia</th>' +
        '<th>Precio</th>' +
        '<th>Cantidad</th>' +
        '<th>Subtotal</th>' +
        '<th>Opciones</th>' +
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
                '<td>' + '<img src="/Content/images/articulos/' + value.Imagen + '">' + '</td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td id="exist-' + value.ID + '">' + value.Existecia + '</td>' +
                '<td>C$ ' + value.Precio + '</td>' +
                '<td>' +
                '<input class="browser-default" id="cant-' + value.ID + '" type="text" value="' + value.Cantidad + '"></input>' +
                '</td>' +
                '<td>' + value.Cantidad * value.Precio + '</td>' +
                '<td>' + '<a class="center" onclick= "eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' + '</td>' +
                '</tr>'
            );
            cantidadTotal = cantidadTotal + value.Cantidad;
            subTotal = subTotal + (value.Precio * value.Cantidad);
        });
    });
    detalles();
}