$(document).ready(function () {
    $(".devoluciones-content .listado-devoluciones").on('click', '.info-sidenav', function () {
        if ($(".devoluciones .devoluciones-details").hasClass('show')) {
            $(".devoluciones .devoluciones-details").css("right", "0px");
            $(".devoluciones .devoluciones-content").css("padding", "0 280px 0 0");
            $(".devoluciones .devoluciones-details").toggleClass('show hiden');
        }
    });
    $(".devoluciones-details").on('click', '.info-sidenav', function () {
        $(".devoluciones .devoluciones-details").css("right", "-300px");
        $(".devoluciones .devoluciones-content").css("padding", "0");
        $(".devoluciones .devoluciones-details").toggleClass('show hiden');
        $(".devoluciones .listado-devoluciones table tbody .item").removeClass("active");
        vacio();
    });
    $('ul.tabs').tabs();
    $('select').material_select();
    listaDevoluciones();
    vacio();

    $(".devoluciones .listado-devoluciones table tbody").on('click', '.item', function () {
        $(".devoluciones .listado-devoluciones table tbody .item").removeClass("active");
        $(this).addClass("active");
    });
    $('.devoluciones .devoluciones-content, .devoluciones .devoluciones-content .devoluciones-opciones, .devoluciones .devoluciones-content .paginacion').on('click', function (e) {
        if (e.target !== this)
            return;
        $(".devoluciones .listado-devoluciones table tbody .item").removeClass("active");
        vacio();
    });
});
function listaDevoluciones() {
    $.ajax({
        url: '/devolucion/proveedor/lista',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".listado-devoluciones table tbody").empty();
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
                    $(".listado-devoluciones table tbody").append(
                        '<tr class="item info-sidenav" onclick="informacionDevolucion(' + value.ID + ')">' +
                        '<td>' + value.ID + '</td>' +
                        '<td>' + value.Factura + '</td>' +
                        '<td>' + value.Fecha + '</td>' +
                        '<td>' + value.Articulos + '</td>' +
                        '<td>' + value.Cantidad + ' Unidades</td>' +
                        '<td>' + value.Proveedor + '</td>' +
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
    $(".devoluciones-details .general").empty();
    $(".devoluciones-details .detalle").empty();
    $(".devoluciones-details .general").append(
        '<div class="no-selected">' +
        '<div class="center">' +
        '<i class="grey-text text-darken-1 material-icons large">touch_app</i>' +
        '</div>' +
        '<p class="no center-align grey-text text-darken-1">No a seleccionado ni una devolucion para ver la informacion</p>' +
        '</div>'
    );
    $(".devoluciones-details .detalle").append(
        '<div class="no-selected">' +
        '<div class="center">' +
        '<i class="grey-text text-darken-1 material-icons large">touch_app</i>' +
        '</div>' +
        '<p class="no center-align grey-text text-darken-1">No a seleccionado ni una devolucion para ver la informacion</p>' +
        '</div>'
    );
}
function informacionDevolucion(id) {
    var id = id;
    detalleDevolucionGeneral(id);
    detalleDevolucionEspecifico(id);
}
function detalleDevolucionGeneral(id) {
    $.ajax({
        url: '/devolucion/proveedor/detalle/general/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".devoluciones-details .general").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".devoluciones-details .general").append(
                        '<div>' +
                        '<p><span class="left">N° Devolucion</span>' + value.Devolucion + '</p>' +
                        '<p><span class="left">N° Factura</span>C$ ' + value.Factura + '</p>' +
                        '<p><span class="left">Usuario</span>' + value.Usuario + '</p>' +
                        '<p><span class="left">Proveedor</span> ' + value.Proveedor + ' </p>' +
                        '<p><span class="left">Fecha</span>' + value.Fecha + '</td>' +
                        '<p><span class="left">Tipos Articulos</span>' + value.Articulos + '</p>' +
                        '<p><span class="left">Cantidad Articulos</span>' + value.CantidadTotal + ' Unds</p>' +
                        '</div>'
                    );
                });
            });
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function detalleDevolucionEspecifico(id) {
    $.ajax({
        url: '/devolucion/proveedor/detalle/especifico/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".devoluciones-details .detalle").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".devoluciones-details .detalle").append(
                        '<div class="individual col l12">' +
                        '<div class="col l2">' +
                        '<img src="/Content/images/articulos/' + value.Img + '">' +
                        '</div>' +
                        '<div class="col l10">' +
                        '<p class="name"><strong>' + value.Articulo + '</strong></p>' +
                        '<p class="cate">' + value.Categoria + '</p>' +
                        '</div>' +
                        '<div class="col l12">' +
                        '<p><span>Cant: ' + value.Cantidad + ' </span></p>' +
                        '<p><span>Descripcion: ' + value.Descripcion + ' </span></p>' +
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