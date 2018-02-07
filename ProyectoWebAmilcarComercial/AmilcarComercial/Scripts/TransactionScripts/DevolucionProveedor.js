$(document).ready(function () {
    $('.tooltipped').tooltip({ delay: 50 });    
    var compra = false, articulo = false;
    var cantidad, cantidadTotal;
    var idcompra;
    mostrarDevolucionTmp();
    generales();
    mostrarProductosTmp(1);
    detectarCambios();
    detalles();
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
                '<p class="">.</p>' +
                '</div>'
            );
            //$("#N_factura").val(data[4]);
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
                        '<div class="card blue white-text hoverable">' +
                        '<div class="card-content">' +
                        '<span class="card-title truncate">Fact: ' + value.Factura + '</span>' +
                        '<p class="truncate">Prov: ' + value.Proveedor + '</p>' +
                        '<p>Fecha: ' + value.Fecha + '</p>' +
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
            if (articulo === true) {
                eliminarProductosTodos();
            }
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
            if (articulo === true) {
                eliminarProductosTodos();
            }
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
                    '<button class="btn btn-flat blue-grey darken-4 white-text" onclick="compras()">Buscar</button>' +
                    '</div>'
                );
                compra = false;
                idcompra = null;
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
                compra = true;
                idcompra = data.ID;
            }
            $("#pre-Devolucion").css("display", "none");
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
    if (compra === false) {
        Materialize.toast("Debe definir una compra", 2000);
        return;
    }
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
        '<table class="responsive-table bordered highlight centered white">' +
        '<thead>' +
        '<tr>' +
        '<th>Cod</th>' +
        '<th>Img</th>' +
        '<th>Nombre</th>' +
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
        url: '/devoluciones/agregar/producto/' + id + '/' + cant + '/' + descrip + '/' + tipo + '/' + compradas,
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
    var tipo = "Proveedor";
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
    var tipo = "Proveedor";
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
    $(".articulos-devolucion").empty();
    $("#pre-ArticulosDevolucion").css("display", "inline");
    var tipo = "Proveedor";
    $.ajax({
        url: '/devoluciones/obtener/productosTmp/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".articulos-devolucion").append(
                '<div class="divider-mio"></div>' +
                '<div class="articulosLista col l12 blue" style="padding:0"></div>'
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
        '<th>Nombre</th>' +
        '<th>Img</th>' +
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
                '<td>' + value.Nombre + '</td>' +
                '<td>' + '<img src="/Content/images/articulos/' + value.Imagen + '">' + '</td>' +
                '<td id="exist-' + value.ID + '">' + value.CantidadCV + '</td>' +
                '<td>' +
                '<input class="browser-default" id="cant-' + value.ID + '" type="text" value="' + value.Cantidad + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" id="descrip-' + value.ID + '" type="text" value="' + value.Descripcion + '"></input>' +
                '</td>' +
                '<td>' + '<a class="center" onclick= "eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' + '</td>' +
                '</tr>'
            );
            cantidad++;
            cantidadTotal = cantidadTotal + value.Cantidad;
        });
    });
    detalles();
}
function detectarCambios() {
    var ID_Obj;
    var AnteriorValor, NuevoValor;
    var dividiendo;
    var id, campo;
    var tipo = "Proveedor";
    var exist;

    $(document).on({
        'focusin': function () {
            ID_Obj = $(this).attr("id");
            dividiendo = ID_Obj.split("-", 2);
            campo = dividiendo[0];
            id = dividiendo[1];
            AnteriorValor = $(this).val();
            exist = $("#exist-" + id).text();
        },
        'focusout': function () {
            NuevoValor = $(this).val();

            if (NuevoValor === '') {
                Materialize.toast("Debe ingresar un valor", 2000);
                $(this).focus();
                return;
            }          
            else if (NuevoValor === AnteriorValor) {
                return;
            }

            if (campo === "cant") {
                if (!/^([0-9])*$/.test(NuevoValor)) {
                    Materialize.toast("El valor no es valido", 2000);
                    $(this).val(AnteriorValor);
                    return;
                }
                else if (NuevoValor === 0) {
                    Materialize.toast("El valor no puede ser 0", 2000);
                    $(this).val(AnteriorValor);
                    return;
                }
                if (NuevoValor < 0) {
                    Materialize.toast("El valor no es valido", 2000);
                    $(this).val(AnteriorValor);
                    return;
                }
                if (NuevoValor > exist) {
                    Materialize.toast("La cantidad no puede ser mayor que la comprada", 2000);
                    $(this).val(AnteriorValor);
                    return;
                }

                $.ajax({
                    url: '/devoluciones/actualizar/cantidad/productoTmp/' + id + '/' + NuevoValor + '/' + tipo,
                    type: 'GET',
                    'success': function (data) {
                        mostrarProductosTmp(1);
                        Materialize.toast('Cantidad del articulo actualizada', 2000);
                    },
                    'error': function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    }
                });
            }

            if (campo === "descrip") {
                $.ajax({
                    url: '/devoluciones/actualizar/descripcion/productoTmp/' + id + '/' + NuevoValor + '/' + tipo,
                    type: 'GET',
                    'success': function (data) {
                        mostrarProductosTmp(1);
                        Materialize.toast('Descripcion actualizada', 2000);
                    },
                    'error': function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    }
                });
            }
        }
    }, '.articulosLista table tbody input');
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
        $("#devolucion").addClass('disabled');
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
    $("#devolucion").removeClass('disabled');
}
function cancelarDevolucion() {
    $.ajax({
        url: '/devoluciones/proveedor/cancelar',
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
    if (compra === false) {
        Materialize.toast("Debe definir una venta", 3000);
        return;
    }
    if (articulo === false) {
        Materialize.toast("Debe seleccionar productos a devolver", 3000);
        return;
    }
    var fact = $("#N_factura").val();
    if (fact === "") {
        Materialize.toast("Debe definir una factura", 3000);
        return;
    }

    var datos = {
        fact: fact,
        id_compra: idcompra
    };

    $.ajax({
        url: '/devoluciones/proveedor/guardar',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        data: datos,
        'success': function (data) {
            if (data === true) {
                window.location.href = "/proveedor/devolucion/facturado";
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