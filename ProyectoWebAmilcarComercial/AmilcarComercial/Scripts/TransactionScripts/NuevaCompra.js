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
  
    var proveedor = false, articulo = false;
    var total, subTotal, cantidadTotal, iva, subtotalpro;
    ultimo();
    mostrarProveedorTmp();
    generales();
    mostrarProductosTmp(1);
    detectarCambios();
    $('.nueva-compra .detalle').on('change', '#iva', function (e) {
        iva = this.value;
        detalles();
    });
    detalles();
    $('#id_categoria').on('change', '', function (e) {
        var idCategoria = this.value;

        if (idCategoria == 1) {
            $(".especificaciones").empty();
            return;
        }

        $.ajax({
            url: '/obtener/especificaciones/' + idCategoria,
            type: 'GET',
            contentType: "application/json",
            dataType: "json",
            'success': function (data) {
                $(".especificaciones").empty();
                $(".especificaciones").append(
                    '<div class="col l12">' +
                    '<br />' +
                    '<span>Especificaciones:</span>' +
                    '<div class="divider-mio white"></div>' +
                    '<br />' +
                    '</div>'
                );
                $.each(data, function () {
                    $.each(this, function (name, value) {
                        $(".especificaciones").append(
                            '<div class="input-field col l4">' +
                            '<input id="' + value.ID + '" type="text" class="validate">' +
                            '<label for="' + value.ID + '">' + value.Nombre + '</label>' +
                            '</div>'
                        );
                    });
                });
            },
            'error': function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }); 
})
Dropzone.autoDiscover = true;
Dropzone.options.myDropzone = {
    paramName: "file",
    uploadMultiple: false,
    maxFiles: 1,
    addRemoveLinks: true,
    init: function () {
        this.on("complete", function (data) {
            //var res = eval('(' + data.xhr.responseText + ')');
            var res = JSON.parse(data.xhr.responseText);
        });
    }
};  
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
                '<p>.</p>'+'</div>'
            );
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
        url: '/obtener/proveedores',
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
                        '<div class="card blue white-text text-darken-3 hoverable">' +
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
    var tipo = "Compra";
    $.ajax({
        url: '/agregar/proveedorTmp/' + tipo,
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
    var tipo = "Compra";
    $.ajax({
        url: '/agregar/ProveedorExistente/' + id + '/' + tipo,
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
    var tipo = "Compra";
    $.ajax({
        url: '/editar/proveedorTmp/' + tipo,
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
    var tipo = "Compra";
    $.ajax({
        url: '/editar/proveedorGuardarTmp/' + tipo,
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
    var tipo = "Compra";
    $.ajax({
        url: '/eliminar/proveedorTmp/' + tipo,
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
    $("#pre-Proveedor").css("display", "inline");
    var tipo = "Compra";
    $.ajax({
        url: '/obtener/proveedorTmp/' + tipo,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".proveedor .opcionesProveedor a").hide();
                $(".datosProveedor").append(
                    '<div class="center-align vacio">' +
                    '<p>Aun no a definido cliente para esta venta</p>' +
                    '<button class="btn btn-flat blue-grey darken-4 white-text" onclick="agregarProveedor()">Agregar</button>' + ' ' + 
                    '<button class="btn btn-flat blue-grey darken-4 white-text" onclick="proveedores()">Buscar</button>' +
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
}
function CancelArticulos() {
    $('#articulos').modal('close');
    $(".lista-articulos").empty();
}
function agregarArticuloTmp(id) {
    var cant = $(".articulos #cant" + id).val();
    var precio = $(".articulos #precio" + id).val();
    var desc = $(".articulos #desc" + id).val();
    var vent = $(".articulos #vent" + id).val();
    var motiv = $(".articulos #motivo" + id).val();

    if (motiv == 0) {
        Materialize.toast("Debe seleccionar el motivo", 2000);
        return;
    }
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
    if (vent === '') {
        Materialize.toast("Debe agregar el precio de venta", 2000);
        $(".articulos #vent" + id).focus();
        return;
    }
    if (vent === 0) {
        Materialize.toast("El precio de venta no puede ser igual a 0", 2000);
        $(".articulos #vent" + id).focus();
        return;
    }
    if (vent < 0) {
        Materialize.toast("El precio de venta no es valido", 2000);
        $(".articulos #vent" + id).focus();
        return;
    }
    if (!/^([0-9])*$/.test(vent)) {
        Materialize.toast("El valor no es valido", 2000);
        $(".articulos #vent" + id).focus();
        return;
    }

    $.ajax({
        url: '/compras/agregar/producto/' + id + '/' + cant + '/' + precio + '/' + desc + '/' + vent + '/' + motiv,
        type: 'GET',
        'success': function (data) {
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
        '<table class="responsive-table bordered highlight centered white">' +
        '<thead>' +
        '<tr>' +
        '<th>Nombre</th>' +
        '<th>Img</th>' +
        '<th>Motivo</th>' +
        '<th>Costo C$</th>' +
        '<th>Cantidad</th>' +
        '<th>Descuento</th>' +
        '<th>PrecioVenta</th>' +
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
                '<td>' + value.Nombre + '</td>' +
                '<td><img src="/Content/images/articulos/' + value.Imagen + '"></td>' +
                '<td>' +
                '<select id="motivo' + value.ID + '" class="browser-default">' +
                '<option value="0" selected>Seleccione</option>' +
                '<option value="1">Compra</option>' +
                '<option value="2">Bonificacion</option>' +
                '<option value="3">Reposicion</option>' +
                '</select>' +
                '</td>' +
                '<td>' +
                '<input placeholder="Costo" id="precio' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Cantidad" id="cant' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Descuento" id="desc' + value.ID + '" type="text" class="browser-default" value="0">' +
                '</td > ' +
                '<td>' +
                '<input placeholder="Precio" id="vent' + value.ID + '" type="text" class="browser-default">' +
                '</td > ' +
                '<td class="right-align">' +
                '<a class="btn btn-flat pink white-text" onclick="agregarArticuloTmp(' + value.ID + ')"><i class="material-icons">add_shopping_cart</i></a>' +
                '</td > ' +
                '</tr>'
            );
            if (value.Motivo == 1) {
                $("#motivo" + value.ID).children('option[value=1]').hide();
            }
            if (value.Motivo == 2) {
                $("#motivo" + value.ID).children('option[value=2]').hide();
            }
            if (value.Motivo == 3) {
                $("#motivo" + value.ID).children('option[value=3]').hide();
            }
            $('.lista-articulos table tbody').on("change", "#motivo" + value.ID, function (e) {
                var mot = this.value;
                if (mot == 2 || mot == 3) {
                    $(".lista-articulos table tbody #precio" + value.ID).val(0);
                }
            });
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
        '<th>Motivo</th>' +     
        '<th>Nombre</th>' +
        '<th>Img</th>' +
        '<th>Costo</th>' +
        '<th>Cantidad</th>' +
        '<th>DescXunidad</th>' +
        '<th>Subtotal</th>' +
        '<th>PrecioVenta</th>' +
        '<th>Opciones</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>' +
        '</tbody>' +
        '</table>'
    );
    cantidadTotal = 0;
    subTotal = 0;
    subtotalpro = 0;
    $.each(data, function () {
        $.each(this, function (name, value) {
            subtotalpro = ((value.Precio * value.Cantidad) - (value.Descuento * value.Cantidad));
            $(".articulosLista table tbody").append(
                '<tr>' +
                '<td>' +
                '<select id="motivo-' + value.ID + '" class="browser-default">' +
                '<option value="1" selected>Compra</option>' +
                '<option value="2">Bonificacion</option>' +
                '<option value="3">Reposicion</option>' +
                '</select>' +
                '</td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td>' + '<img src="/Content/images/articulos/' + value.Imagen + '">' + '</td>' +
                '<td>' +
                '<input class="browser-default precio" type="text" id="precio-' + value.ID + '" value="' + value.Precio + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="cant-' + value.ID + '" value="' + value.Cantidad + '"></input>' +
                '</td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="desc-' + value.ID + '" value="' + value.Descuento + '"></input>' +
                '</td>' +
                '<td><span id="subtotal-' + value.ID + '">C$ <span>' + subtotalpro.toFixed(2) + '</span></span></td>' +
                '<td>' +
                '<input class="browser-default" type="text" id="vent-' + value.ID + '" value="' + value.PrecioVenta + '"></input>' +
                '</td>' +
                '<td>' + '<a class="center" onclick="eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' +
                '</td>' +
                '</tr>'
            );
            $('.articulosLista table #motivo-' + value.ID).val(value.Motivo);
            if (value.Motivo === 2 || value.Motivo === 3) {
                $(".articulosLista table #precio-" + value.ID).prop('disabled', true).addClass("desabilitado");
                $(".articulosLista table #desc-" + value.ID).prop('disabled', true).addClass("desabilitado");
                $(".articulosLista table #subtotal-" + value.ID).addClass("desabilitado");
                $(".articulosLista table #subtotal-" + value.ID + " span").text("-- -- --");
                subtotalpro = 0;
            }
            $('.articulosLista table').on("change", "#motivo-" + value.ID, function (e) {
                var motivo = this.value;
                var id = value.ID;              
                detectarTipoVenta(motivo, id);
            });
            cantidadTotal = cantidadTotal + value.Cantidad;
            subTotal = subTotal + subtotalpro;
        });
    });
    detalles();
}
function detectarCambios() {
    var ID_Obj;
    var AnteriorValor, NuevoValor; 
    var dividiendo;
    var id, campo, costo;

    $(document).on({
        'focusin': function () {
            ID_Obj = $(this).attr("id");
            dividiendo = ID_Obj.split("-", 2);
            campo = dividiendo[0];
            id = dividiendo[1];
            AnteriorValor = $(this).val();
            costo = $("#precio-" + id).val();
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

            if (campo === "vent") {
                $.ajax({
                    url: '/compras/actualizar/precioventa/productoTmp/' + id + '/' + NuevoValor,
                    type: 'GET',
                    'success': function (data) {
                        mostrarProductosTmp(1);
                        Materialize.toast('El precio de venta del articulo actualizado', 2000);
                    },
                    'error': function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    }
                });
            }

            if (campo === "desc") {
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
function detectarTipoVenta(motivo, id) {
    $.ajax({
        url: '/compras/actualizar/motivo/productoTmp/' + id + '/' + motivo,
        type: 'GET',
        'success': function (data) {
            mostrarProductosTmp(1);
            Materialize.toast('Motivo del articulo actualizado', 2000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    }); 
}

function nuevopro() {
    $.ajax({
        url: '/compras/nuevopro',
        type: 'GET',
        contentType: "application/json",
        'success': function (data) {
                Materialize.toast("Nuevo articulo creado", 2000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function ultimo() {
    $.ajax({
        url: '/obtener/ultimo',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $.each(data, function (index, element) {
                $('#nuevoProducto .articulo-nuevo #codigo_articulo').val(element.ID);
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    $('#nuevo').modal('open');
};
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

    iva = (subTotal * parseFloat(15 / 100));
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
        Materialize.toast("Debe definir un proveedor", 2000);
        return;
    }
    if (articulo === false) {
        Materialize.toast("Debe seleccionar productos a comprar", 2000);
        return;
    }
    if ($("#N_factura").val() === ""){
        Materialize.toast("Ingrese el numero de factura de la compra", 2000);
        this.focus();
        return;
    }

    var datos = {
        fact_compra: $("#N_factura").val(),
        tipo_comprobante_compra: $('#comprobante').val(),
        iva_compra: 15
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
                window.location.href = "/Compras/Facturado";
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