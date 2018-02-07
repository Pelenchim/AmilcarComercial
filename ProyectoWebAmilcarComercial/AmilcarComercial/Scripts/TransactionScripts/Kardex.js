$(document).ready(function () {
    listaKardex();
});
function listaKardex() {
    $.ajax({
        url: '/kardex/lista',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".listado-kardex table tbody").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".listado-kardex table tbody").append(
                        '<tr onclick="informacionVenta(' + value.ID + ')">' +
                        '<td>' + value.Cod + '</td>' +
                        '<td>' + '<img src="/Content/images/articulos/' + value.Imagen + '">' + '</td>' +
                        '<td>' + value.Articulo + '</td>' +
                        '<td>' + value.Categoria + '</td>' +
                        '<td>' + value.EntradaFecha + '</td>' +
                        '<td>' + value.SalidaFecha + '</td>' +
                        '<td><i class="material-icons" onclick="detalleKardex(' + value.ID + ')">info</i></td>' +
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
function detalleKardex(id) {
    $('#kardex').modal('open');
    $.ajax({
        url: '/kardex/detalle/' + id,
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".lista-kardex table tbody").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    var color;
                    if (value.Observacion.includes('Aprovada')) {
                        color = "green";
                    }
                    else if (value.Observacion.includes('Anulada')) {
                        color = "red";
                    }
                    $(".modal-kardex .kardex h4 span").text("Detalle de " + value.Articulo);
                    $(".lista-kardex table tbody").append(
                        '<tr>' +
                        '<td>' + value.NumFact + '</td>' +
                        '<td>' + value.Fecha + '</td>' +
                        '<td>' + value.Tipo + '</td>' +
                        '<td>' + value.Entrada + '</td>' +
                        '<td>' + value.Salida + '</td>' +
                        '<td><span class="obs white-text ' + color + '">' + value.Observacion + '</span></td>' +
                        '<td>' + value.Nombre + ' ' + value.Apellido + '</td>' +
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