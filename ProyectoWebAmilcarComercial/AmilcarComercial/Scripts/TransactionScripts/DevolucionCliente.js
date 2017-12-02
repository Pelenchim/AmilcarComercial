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
    var venta = false, articulo = false;
    var cantidadTotal, cantidad;
    var idventa;
    mostrarDevolucionTmp();
    generales();
    mostrarProductosTmp(1);
    //detectarCambios();
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
            if (articulo === true) {
                eliminarProductosTodos();
            }
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
            if (articulo === true) {
                eliminarProductosTodos();
            }
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
                venta = false;
                idventa = null;
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
                venta = true;
                idventa = data.ID;
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
    if (venta === false) {
        Materialize.toast("Debe definir una venta", 2000);
        return;
    }
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
        '<th>Cant-Comprada</th>' +
        '<th>Cantidad</th>' +
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
                '<td id="cantidad' + value.ID + '">' + value.Cantidad + '</td>' +
                '<td>' +
                '<input placeholder="Cantidad" id="cant' + value.ID + '" type="text" class="browser-default" value="' + value.Cantidad + '">' +
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
    var vendidas = $(".articulos #cantidad" + id).text();
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
    if (cant > vendidas) {
        Materialize.toast("La cantidad a devolver no puede ser mayor a la vendida", 2000);
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
    var tipo = "Cliente";
    $.ajax({
        url: '/devoluciones/agregar/producto/' + id + '/' + cant + '/' + descrip + '/' + tipo + '/' + 1,
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
    var tipo = "Cliente";
    $.ajax({
        url: '/devoluciones/eliminar/productoTmp/' + id + '/' + tipo,
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
    var tipo = "Cliente";
    $.ajax({
        url: '/devoluciones/eliminar/eliminarProductosTodos/' + tipo,
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
    cantidad = 0;
    $(".articulos-devolucion").empty();
    $("#pre-ArticulosDevolucion").css("display", "inline");
    var tipo = "Cliente";
    $.ajax({
        url: '/devoluciones/obtener/productosTmp/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".articulos-devolucion").append(
                '<div class="divider-mio"></div>' +
                '<div class="articulosLista col l12 blue" style="padding:0;"></div>'
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
                $("#pre-ArticulosDevolucion").css("display", "none");
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
            $("#pre-ArticulosDevolucion").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function articulosOrdenTable(data) {
    $(".articulosDevolucion .opcionesArticulos .table-v").hide();
    $(".articulosDevolucion .opcionesArticulos .card-v").show();
    $(".articulosDevolucion .opcionesArticulos .delete").show();
    $(".articulosDevolucion .opcionesArticulos .search").show();
    $(".articulos-devolucion .articulosLista").toggleClass("blue white");
    $(".lista-articulos").empty();
    $(".articulos-devolucion .articulosLista").append(
        '<table class="bordered highlight centered responsive-table white">' +
        '<thead class="z-depth-1">' +
        '<tr>' +
        '<th>Cod</th>' +
        '<th>Img</th>' +
        '<th>Nombre</th>' +
        '<th>Cant-Comprada</th>' +
        '<th>Cantidad</th>' +
        '<th>Descripcion</th>' +
        '<th>Opciones</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '</table>'
    );
    cantidadTotal = 0;
    cantidad = 0;
    $.each(data, function () {
        $.each(this, function (name, value) {
            $(".articulosLista table tbody").append(
                '<tr>' +
                '<td>' + value.ID + '</td>' +
                '<td>' + '<img src="/Content/images/articulos/' + value.Imagen + '">' + '</td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td id="exist-' + value.ID + '">' + value.Existecia + '</td>' +
                '<td>' +
                '<input class="browser-default" id="cant-' + value.ID + '" type="text" value="' + value.Cantidad + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" id="descrip-' + value.ID + '" type="text" value="' + value.Descripcion + '"></input>' +
                '</td>' +               
                '<td>' + '<a class="center" onclick= "eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' + '</td>' +
                '</tr>'
            );
            cantidadTotal = cantidadTotal + value.Cantidad;
            cantidad++;
        });
    });
    detalles();
}

function detalles() {
    if (cantidadTotal === 0) {
        $(".detalle .detalles").empty();
        $(".detalle .detalles").append(
            '<p><strong>Detalle:</strong></p>' +
            '<div class="col l12">' +
            '<p class="center-align"><strong>No hay articulos en la lista, agrege para realizar la devolucion.</strong></span>' +
            '</div>'
        );
        $(".detalle .total").text("");
        $("#facturar").addClass('disabled');
        return;
    }

    $(".detalle .detalles").empty();
    $(".detalle .detalles").append(
        '<p><strong>Detalle:</strong></p>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Articulos: </strong>' + cantidad + ' tipos</span>' +
        '</div>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Total Articulos:</strong>' + cantidadTotal + ' Unidades</span>' +
        '</div>'
    );
    $("#comprar").removeClass('disabled');
}
function cancelarDevolucion() {
    $.ajax({
        url: '/devoluciones/cliente/cancelar',
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
    if (venta === false) {
        Materialize.toast("Debe definir una venta", 3000);
        return;
    }
    if (articulo === false) {
        Materialize.toast("Debe seleccionar productos a devolver", 3000);
        return;
    }

    var datos = {
        fact: $("#N_factura").val(),
        id_venta: idventa
    };

    $.ajax({
        url: '/devoluciones/cliente/guardar',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        data: datos,
        'success': function (data) {
            if (data === true) {
                window.location.href = "//Facturado";
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