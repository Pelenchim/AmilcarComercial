$(document).ready(function () {
    detalle();
    $(".anular").on("click", function () {
        var id = $(this).attr("id");
        anular(id);
    });
});
function detalle() {
    $.ajax({
        url: '/compras/detallefactura',
        type: 'GET',
        contentType: "application/json",
        'success': function (data) {
            $(".detalleCompra .detalle").append(
                '<p><span class="left">Cantidad de Articulos: </span><span class="right">' + data[0] + ' Unds</span></p>' +
                '<div class="divider-mio"></div>' +
                '<p><span class="left">SubTotal: </span><span class="right"> C$ ' + data[1].toFixed(2) + '</span></p>' +
                '<div class="divider-mio"></div>' +
                '<p><span class="left">Iva Total: </span><span class="right"> C$ ' + data[2].toFixed(2) + '</span></p>' +
                '<div class="divider-mio"></div>' +
                '<p><span class="left">Descuento Total: </span><span class="right"> C$ ' + data[3].toFixed(2) + '</span></p>' +
                '<div class="divider-mio"></div>' +
                '<p><span class="left">Total a Pagar: </span><span class="right"> C$ ' + data[4].toFixed(2) + '</span></p>'
            );
            $(".detalleCompra .anular").attr("id", data[5]);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function anular(id) {
    $.ajax({
        url: '/compras/anular/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            window.location.href = '/compras/index';
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}