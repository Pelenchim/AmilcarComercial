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

    var proveedor = false, articulo = false;
    var total, subTotal, cantidadTotal, iva;
    mostrarProveedorTmp();
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
        url: '/compra/obtener/generales',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".datosGenerales").empty();
            $(".datosGenerales").append(
                '<div class="col l6">' +
                '<p class=""><strong>Compra N°: </strong>' + data[3] + '</p>' +
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
function proveedores() {
    $(".lista-proveedores").empty();
    $("#pre-Proveedores").css("display", "inline");
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
            $("#pre-Proveedores").css("display", "none");
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
        nombre: $(".modal-compras .nuevoProveedor #nombre-p").val(),
        telefono: $('.modal-compras .nuevoProveedor #telefono-p').val(),
        ruc: $('.modal-compras .nuevoProveedor #ruc-p').val()
    };
    $.ajax({
        url: '/compras/agregar/proveedorTmp',
        type: 'GET',
        data: datos,
        'success': function (data) {
            mostrarProveedorTmp();
            $('#nuevoProveedor').modal('close');
            Materialize.toast('Proveedor agregado exitosamente', 4000);
            $(".modal-compras .nuevoProveedor #nombre-p").val('');
            $('.modal-compras .nuevoProveedor #telefono-p').val('');
            $('.modal-compras .nuevoProveedor #ruc-p').val('');
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
            $('.modal-compras .editarProveedor #nombre-p').val(data.Nombre).focus();
            $(".modal-compras .editarProveedor #telefono-p").val(data.Telefono).focus();
            $(".modal-compras .editarProveedor #ruc-p").val(data.Ruc).focus();
            $(".modal-compras .editarProveedor #id-p").val(data.ID);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function editarGuardarProveedor() {
    var datos = {
        nombre: $(".modal-compras .editarProveedor #nombre-p").val(),
        telefono: $('.modal-compras .editarProveedor #telefono-p').val(),
        ruc: $('.modal-compras .editarProveedor #ruc-p').val(),
        id_proveedorTmp: $('.modal-compras .editarProveedor #id-p').val()
    };
    $.ajax({
        url: '/compras/editar/proveedorGuardarTmp',
        type: 'POST',
        data: datos,
        'success': function (data) {
            mostrarProveedorTmp();
            $('#editarProveedor').modal('close');
            Materialize.toast('Proveedor editado exitosamente', 4000);
            $(".modal-compras .editarProveedor #nombre-p").val('');
            $('.modal-compras .editarProveedor #telefono-p').val('');
            $('.modal-compras .editarProveedor #ruc-p').val('');
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
    $(".datosProveedor").empty();
    $("#pre-Proveedor").css("display","inline");
    $.ajax({
        url: '/compras/obtener/proveedorTmp',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".proveedor .opcionesProveedor a").hide();
                $(".datosProveedor").append(
                    '<div class="center-align vacio">' +
                    '<p>Aun no a definido cliente para esta venta</p>' +
                    '<button class="btn btn-flat green white-text" onclick="agregarProveedor()">Agregar</button>' + ' ' +
                    '<button class="btn btn-flat green white-text" onclick="proveedores()">Buscar</button>' +
                    '</div>'
                );
                proveedor = false;
            }
            else {
                $(".proveedor .opcionesProveedor a").show();
                $(".datosProveedor").empty();
                $(".datosProveedor").append(
                    '<div class="col l7">' +
                    '<p><strong>Nombre: </strong>' + data.Nombre + '</p>' +
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
            $("#pre-Proveedor").css("display", "none");
            proveedorSelected = true;
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
    $(".modal-compras .nuevoProveedor #nombre-p").val('');
    $('.modal-compras .nuevoProveedor #telefono-p').val('');
    $('.modal-compras .nuevoProveedor #ruc-p').val('');
}
function CancelEditarProveedor() {
    $('#editarProveedor').modal('close');
    $(".modal-compras .editarProveedor #nombre-p").val('');
    $('.modal-compras .editarProveedor #telefono-p').val('');
    $('.modal-compras .editarProveedor #ruc-p').val('');
}

function abrirArticulos() {
    $('#articulos').modal('open');
    articulos(1);
}
function articulos(vista) {
    $(".lista-articulos").empty();
    $("#pre-Articulos").css("display","inline");
    $.ajax({
        url: '/compras/obtener/productos',
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
};
function CancelArticulos() {
    $('#articulos').modal('close');
    $(".lista-articulos").empty();
}
function agregarArticuloTmp(id) {
    var cant = $(".articulos #cant" + id).val();
    var precio = $(".articulos #precio" + id).val();
    var desc = $(".articulos #desc" + id).val();

    if (precio === '') {
        Materialize.toast("Debe agregar el costo", 2000);
        $(".articulos #precio" + id).focus();
        return;
    }
    if (precio === 0) {
        Materialize.toast("El costo no puede ser igual a 0", 2000);
        $(".articulos #precio" + id).focus();
        return;
    }
    if (precio < 0) {
        Materialize.toast("El costo no es valido", 2000);
        $(".articulos #precio" + id).focus();
        return;
    }
    if (!/^([0-9])*$/.test(precio)) {
        Materialize.toast("El valor no es valido", 2000);
        $(".articulos #precio" + id).focus();
        return;
    }
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
    if (!/^([0-9])*$/.test(cant)) {
        Materialize.toast("El valor no es valido", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (desc === '') {
        Materialize.toast("Debe agregar el descuento", 2000);
        $(".articulos #desc" + id).focus();
        return;
    }
    if (desc < 0) {
        Materialize.toast("El descuento no es valido", 2000);
        $(".articulos #desc" + id).focus();
        return;
    }
    if (!/^([0-9])*$/.test(desc)) {
        Materialize.toast("El valor no es valido", 2000);
        $(".articulos #desc" + id).focus();
        return;
    }
    if (desc > precio) {
        Materialize.toast("El descuento no puede ser mayor que el costo", 2000);
        $(".articulos #desc" + id).focus();
        return;
    }

    $.ajax({
        url: '/compras/agregar/producto/' + id + '/' + cant + '/' + precio + '/' + desc,
        type: 'GET',
        'success': function (data) {
            //$('#articulos').modal('close');
            articulos(1);
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
            Materialize.toast('Articulo eliminado de la compra', 2000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    detalles();
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
    cantidadTotal = 0;
    subTotal = 0;
    detalles();
}
function mostrarProductosTmp(view) {
    cantidadTotal = 0;
    $(".articulos-orden").empty();
    $("#pre-ArticulosOrden").css("display","inline");
    $.ajax({
        url: '/compras/obtener/productosTmp',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
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
                detalles();
                articulo = false;
            }
            else {
                if (view === 0) {
                    articulosOrdenCard(data);
                }
                if (view === 1) {
                    $(".articulos-orden .articulosLista").css("padding", "0px");
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
                '<img class="blue" src="/Content/images/articulos/' + value.Imagen + '">' +
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
        '<th>Costo</th>' +
        '<th>Cantidad</th>' +
        '<th>Descuento</th>' +
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
                '<td>' +
                '<input class="browser-default precio" type="text" id="precio-' + value.ID + '" value="' + value.Precio + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="cant-' + value.ID + '" value="' + value.Cantidad + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="desc-' + value.ID + '" value="' + value.Descuento + '"></input>' +
                '</td>' +
                '<td>C$' + ((value.Precio * value.Cantidad) - value.Descuento).toFixed(2) + '</td>' +
                '<td>' + '<a class="center" onclick="eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' +
                '</td>' +
                '</tr>'
            );
            cantidadTotal = cantidadTotal + value.Cantidad;
            subTotal = subTotal + (value.Precio * value.Cantidad);
        });
    });
    detalles();
}
function detectarCambios() {
    var ID_Obj;
    var AnteriorValor, NuevoValor; 
    var dividiendo;
    var id, campo;

    $(document).on({
        'focusin': function () {
            ID_Obj = $(this).attr("id");
            dividiendo = ID_Obj.split("-", 2);
            campo = dividiendo[0];
            id = dividiendo[1];
            AnteriorValor = $(this).val();
        },
        'focusout': function () {
            NuevoValor = $(this).val();

            if (NuevoValor === '') {
                Materialize.toast("Debe ingresar un valor", 2000);
                $(this).focus();
                return;
            }
            else if (NuevoValor === 0) {
                Materialize.toast("El valor no puede ser 0", 2000);
                $(this).val(AnteriorValor);
                return;
            }
            else if (NuevoValor < 0) {
                Materialize.toast("El valor no es valido", 2000);
                $(this).val(AnteriorValor);
                return;
            }
            else if (NuevoValor === AnteriorValor) {
                return;
            }
            else if (!/^([0-9])*$/.test(NuevoValor)) {
                Materialize.toast("El valor no es valido", 2000);
                $(this).val(AnteriorValor);
                return;
            }

            if (campo === "precio") {
                $.ajax({
                    url: '/compras/actualizar/costo/productoTmp/' + id + '/' + NuevoValor,
                    type: 'GET',
                    'success': function (data) {
                        mostrarProductosTmp(1);
                        Materialize.toast('Costo del articulo actualizado', 2000);
                    },
                    'error': function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    }
                }); 
            }

            if (campo === "cant") {
                $.ajax({
                    url: '/compras/actualizar/cantidad/productoTmp/' + id + '/' + NuevoValor,
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

            if (campo === "desc") {

                var costo = $("#precio-" + id).val();
                if (NuevoValor >= costo) {
                    Materialize.toast("El descuento no puede ser mayor que el costo", 2000);
                    $("#desc-" + id).val(AnteriorValor);
                    return;
                }

                $.ajax({
                    url: '/compras/actualizar/descuento/productoTmp/' + id + '/' + NuevoValor,
                    type: 'GET',
                    'success': function (data) {
                        mostrarProductosTmp(1);
                        Materialize.toast('Descuento del articulo actualizado', 2000);
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
            '<p class="center-align"><strong>No hay articulos en la orden, agrege para realizar la compra.</strong></span>' +
            '</div>'
        );
        $(".detalle .total").text("");
        $("#comprar").addClass('disabled');
        return;
    }

    iva = (subTotal * parseFloat($('#iva').val() / 100));
    Total = subTotal + iva;
    $(".detalle .detalles").empty();
    $(".detalle .detalles").append(
        '<p><strong>Detalle:</strong></p>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Articulos: </strong>' + cantidadTotal + '</span>' +
        '</div>' +        
        '<div class="col l12">' +
        '<span class="left"><strong>Subtotal: C$</strong>' + (subTotal).toFixed(2) + '</span>' +
        '</div>' +
        '<div class="col l12">' +
        '<span class="left"><strong>Iva: C$</strong>' + (iva).toFixed(2) + '</span>' +
        '</div>' 
    );
    $(".detalle .total").text("Total: C$" + (Total).toFixed(2));
    $("#comprar").removeClass('disabled');
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

    if (proveedor === false) {
        Materialize.toast("Debe definir un proveedor", 3000);
        return;
    }
    if (articulo === false) {
        Materialize.toast("Debe seleccionar productos a comprar", 3000);
        return;
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