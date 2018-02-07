﻿$(document).ready(function () {
    $(".compras-content .listado-compras").on('click', '.info-sidenav', function () {
        if ($(".compras .compras-details").hasClass('show')) {
            $(".compras .compras-details").css("right", "0px");
            $(".compras .compras-content").css("padding", "0 280px 0 0");
            $(".compras .compras-details").toggleClass('show hiden');
        }
    });
    $(".compras-details").on('click', '.info-sidenav', function () {
        $(".compras .compras-details").css("right", "-300px");
        $(".compras .compras-content").css("padding", "0");
        $(".compras .compras-details").toggleClass('show hiden');
        $(".compras .listado-compras table tbody .item").removeClass("active");
        vacio();
    });
    $('ul.tabs').tabs();
    $('select').material_select();
    listaCompras();
    vacio();

    $(".compras .listado-compras table tbody").on('click', '.item', function () {
        $(".compras .listado-compras table tbody .item").removeClass("active");
        $(this).addClass("active");
    });
    $('.compras .compras-content, .compras .compras-content .compras-opciones, .compras .compras-content .paginacion').on('click', function (e) {
        if (e.target !== this)
            return;
        $(".compras .listado-compras table tbody .item").removeClass("active");
        vacio();
    });
    $(".compras .compras-details").on('click', '.btn-anular', function () {
        var id = $(".compras .compras-details .id-compra").attr('id');
        $('#anularCompra').modal('open');
    });
    $(".modales .anularCompra").on('click', '.anular', function () {
        var id = $(".compras .compras-details .id-compra").attr('id');
        anular(id);
        $('#anularCompra').modal('close');
    });
});
function listaCompras() {
    $.ajax({
        url: '/compras/listacompras',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".listado-compras table tbody").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    var estado, color;
                    if (value.Estado === true) {
                        estado = "Aprovada";
                        color = "green"
                    }
                    else {
                        estado = "Anulada";
                        color = "red";
                    }
                    $(".listado-compras table tbody").append(
                        '<tr class="item info-sidenav" onclick="informacionCompra(' + value.ID + ')">' +
                        '<td>' + value.Compra + '</td>' +
                        '<td>' + value.Factura + '</td>' +
                        '<td> <span class="' + color + ' badge estado">' + estado + '</span></td>' +
                        '<td>' + value.Proveedor + '</td>' +
                        '<td>' + value.Fecha + '</td>' +
                        '<td>' + value.Articulos + '</td>' +
                        '<td>' + value.CantidadTotal + ' Unidades</td>' +
                        '<td>C$ ' + value.PagoTotal + '</td>' +
                        '</tr>'
                    );
                });
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function vacio() {
    $(".compras-details .general").empty();
    $(".compras-details .detalle").empty();
    $(".compras-details .general").append(
        '<div class="no-selected">' +
        '<div class="center">' +
        '<i class="grey-text text-darken-1 material-icons large">touch_app</i>' +
        '</div>' +
        '<p class="no center-align grey-text text-darken-1">No a seleccionado ni un compra para ver la informacion</p>' +
        '</div>'
    );
    $(".compras-details .detalle").append(
        '<div class="no-selected">' +
        '<div class="center">' +
        '<i class="grey-text text-darken-1 material-icons large">touch_app</i>' +
        '</div>' +
        '<p class="no center-align grey-text text-darken-1">No a seleccionado ni un compra para ver la informacion</p>' +
        '</div>'
    );
}
function informacionCompra(id) {
    var id = id;
    detalleCompraGeneral(id);
    detalleCompraEspecifico(id);
}
function detalleCompraGeneral(id) {
    $.ajax({
        url: '/compras/detallecompra/general/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".compras-details .general").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    var color, estado;
                    var fecha = new Date();
                    var fechahoy = fecha.getDate();

                    if (value.Estado === true) {
                        estado = "Aprovada";
                        color = "green";
                    }
                    else {
                        estado = "Anulada";
                        color = "red";
                    }
                    $(".compras-details .general").append(
                        '<div>' +
                        '<div>' +
                        '<div class="col l12 center border-radius-2 ' + color + '" style="margin-bottom:8px;">' +
                        '<span class="white-text">Compra ' + estado + '</span>' +
                        '</div>' +
                        '<p class="id-compra" id="' + value.Compra + '"><span class="left">N° Compra</span>' + value.Compra + '</p>' +
                        '<p><span class="left">N° Factura</span>' + value.Factura + '</p>' +
                        '<p><span class="left">Comprador</span>' + value.Usuario + '</p>' +
                        '<p><span class="left">Proveedor</span>' + value.Proveedor + '</p>' +
                        '<p><span class="left">Sucursal</span>' + value.Sucursal + '</p>' +
                        '<p><span class="left">Fecha</span>' + value.Fecha + '</td>' +
                        '<p><span class="left">Tipos Articulos</span>' + value.Articulos + '</p>' +
                        '<p><span class="left">Cantidad Articulos</span>' + value.CantidadTotal + ' Unds</p>' +
                        '<p><span class="left">SubTotal</span>C$ ' + value.SubTotal + '</p>' +
                        '<p><span class="left">Iva %</span>C$ ' + value.Iva + '</p>' +
                        '<p><span class="left">Iva Total</span>C$ ' + value.Iva + '</p>' +
                        '<p><span class="left">Descuento Total</span>C$ ' + value.DescuentoTotal + '</p>' +
                        '<p><span class="left">Pago Total</span>C$ ' + value.PagoTotal + '</p>' +
                        '</div>'
                    );
                    if (value.Estado === true && value.Fecha.endsWith(fechahoy)) {
                        $(".compras-details .general").append(
                            '<div class="center">' +
                            '<a class="btn btn-flat btn-anular white grey-text text-darken-2">Anular Compra</a>' +
                            '</div > '
                        );
                    }
                });
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function detalleCompraEspecifico(id) {
    $.ajax({
        url: '/compras/detallecompra/especifico/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".compras-details .detalle").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".compras-details .detalle").append(
                        '<div class="individual col l12">' +
                        '<div class="col l2">' +
                        '<img src="/Content/images/articulos/' + value.Img + '">' +
                        '</div>' +
                        '<div class="col l10">' +
                        '<p class="name"><strong>' + value.Articulo + '</strong></p>' +
                        '<p class="cate">' + value.Categoria + '</p>' +
                        '</div>' +
                        '<div class="col l12">' +
                        '<p><span>Cant: ' + value.Cantidad + ' </span>' + '<span class="right">Costo: ' + value.Costo + '</span>' + '</p>' +
                        '<p><span>SubTotal: ' + value.SubTotal + ' </span>' + '<span class="right">Total: ' + value.Total + '</span>' + '</p>' +
                        '</div>' +
                        '</div>' +
                        '<div class="col l12 divider-mio"></div>'
                    );
                });
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function anular(id) {
    $.ajax({
        url: '/compra/anular/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            listaCompras();
            detalleCompraGeneral(id);
            alert(Eliminada);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}