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
    mostrarClienteTmp();
    mostrarProductosTmp(1);
    generales();
    detectarCambios();
});
function generales() {
    $("#pre-General").css("display", "inline");
    $.ajax({
        url: '/apartado/obtener/generales',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".datosGenerales").empty();
            $(".datosGenerales").append(
                '<div class="col l12">' +
                '<p class="left"><strong>Venta N°: </strong>' + data[2] + '</p>' +
                '<p class="right"><strong>Fecha: </strong>' + data[0] + '</p>' +
                '</div>' +
                '<div class="col l12">' +
                '<p><strong class="left">Vendedor: </strong> <span class="right">' + data[1] + '</span></p>' +
                '</div>'
            );
            $("#pre-General").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function clientes() {
    $(".lista-clientes").empty();
    $("#pre-Clientes").css("display", "inline");
    $.ajax({
        url: '/apartado/obtener/clientes',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            $(".lista-clientes").empty();
            $.each(data, function () {
                $.each(this, function (name, value) {
                    $(".lista-clientes").append(
                        '<a onclick="agregarClienteExistente(' + value.ID + ')">' +
                        '<div class="col l4">' +
                        '<div class="card white grey-text text-darken-3">' +
                        '<div class="card-content">' +
                        '<span class="card-title truncate">' + value.Nombre + ' ' + value.Apellido + '</span>' +
                        '<p>Dep: ' + value.Departamento + '</p>' +
                        '<p>Tel: ' + value.Telefono + '</p>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</a>'
                    );
                });
            });
            $("#pre-Clientes").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
    $('#clientes').modal('open');
};
function CancelCliente() {
    $('#clientes').modal('close');
    $(".lista-clientes").empty();
}
function CancelNuevoClientes() {
    $('#nuevoCliente').modal('close');
}
function agregarClienteTmp() {
    var datos = {
        Nombre: $(".nuevoCliente #nombre_cliente").val(),
        Apellido: $('.nuevoCliente #apellidos_cliente').val(),
        Direccion: $('.nuevoCliente #direccion').val(),
        Departamento: $('.nuevoCliente #departamento').val(),
        Telefono: $('.nuevoCliente #telefono').val(),
        Cedula: $('.nuevoCliente #cedula').val()
    };
    $.ajax({
        url: '/apartado/agregar/clienteTmp',
        type: 'POST',
        data: datos,
        'success': function (data) {
            mostrarClienteTmp();
            $('#nuevoCliente').modal('close');
            Materialize.toast('Cliente agregado exitosamente', 4000);
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
function agregarClienteExistente(id) {
    $.ajax({
        url: '/apartado/agregar/clienteExistente/' + id,
        type: 'GET',
        'success': function (data) {
            mostrarClienteTmp();
            $('#clientes').modal('close');
            Materialize.toast('Cliente definido exitosamente', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function agregarCliente() {
    $('#nuevoCliente').modal('open');
}
function editarCliente() {
    $('#editarCliente').modal('open');
    $.ajax({
        url: '/apartado/editar/clienteTmp',
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
function editarGuardarCliente() {
    var datos = {
        Nombre: $(".nuevoCliente #nombre_cliente").val(),
        Apellido: $('.nuevoCliente #apellidos_cliente').val(),
        Direccion: $('.nuevoCliente #direccion').val(),
        Departamento: $('.nuevoCliente #departamento').val(),
        Telefono: $('.nuevoCliente #telefono').val(),
        Cedula: $('.nuevoCliente #cedula').val()
    };
    $.ajax({
        url: '/apartado/editar/clienteGuardarTmp',
        type: 'POST',
        data: datos,
        'success': function (data) {
            mostrarClienteTmp();
            $('#editarCliente').modal('close');
            Materialize.toast('Cliente editado exitosamente', 4000);
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
function eliminarCliente() {
    $.ajax({
        url: '/apartado/eliminar/clienteTmp',
        type: 'GET',
        'success': function (data) {
            mostrarClienteTmp();
            Materialize.toast("Cliente removido exitosamente", 2000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function mostrarClienteTmp() {
    $(".datosCliente").empty();
    $("#pre-Cliente").css("display", "inline");
    $.ajax({
        url: '/apartado/obtener/clienteTmp',
        type: 'GET',
        contentType: "application/json",
        dataType: "json",
        'success': function (data) {
            if (data === 0) {
                $(".cliente .opcionesCliente a").hide();
                $(".datosCliente").empty();
                $(".datosCliente").append(
                    '<div class="center-align vacio">' +
                    '<p style="padding-bottom: 10px">Aun no a definido cliente para esta venta</p>' +
                    '<button class="btn btn-flat green white-text" onclick="agregarCliente()">Agregar</button>' + ' ' +
                    '<button class="btn btn-flat green white-text" onclick="clientes()">Buscar</button>' +
                    '</div>'
                );
            }
            else {
                $(".cliente .opcionesCliente a").show();
                $(".datosCliente").empty();
                $(".datosCliente").append(
                    '<div class="col l5">' +
                    '<p class="truncate"><strong>Nombre: </strong>' + data.Nombre + ' ' + data.Apellido + '</p>' +
                    '</div>' +
                    '<div class="col l4">' +
                    '<p><strong>Cedula: </strong>' + data.Cedula + '</p>' +
                    '</div>' +
                    '<div class="col l3">' +
                    '<p><strong>Telefono: </strong>' + data.Telefono + '</p>' +
                    '</div>' +
                    '<div class="col l3">' +
                    '<p><strong>Departamento: </strong>' + data.Departamento + '</p>' +
                    '</div>' +
                    '<div class="col l9">' +
                    '<p><strong>Direccion: </strong>' + data.Direccion + '</p>' +
                    '</div>'
                );
            }
            $("#pre-Cliente").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}

function articulos(vista) {
    $(".lista-articulos").empty();
    $("#pre-Articulos").css("display", "inline");
    $.ajax({
        url: '/apartado/obtener/articulos',
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
    $('#articulos').modal('open');
};
function CancelArticulos() {
    $('#articulos').modal('close');
    $(".lista-articulos").empty();
}
function agregarArticuloTmp(id) {
    var cant = $(".articulos #cant" + id).val();
    var stock = $(".articulos #stock" + id).text();
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
    if (cant > stock) {
        Materialize.toast("No hay suficientes productos en stock", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    if (!/^([0-9])*$/.test(cant)) {
        Materialize.toast("El valor no es valido", 2000);
        $(".articulos #cant" + id).focus();
        return;
    }
    $.ajax({
        url: '/apartado/agregar/producto/' + id + '/' + cant,
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
        url: '/apartado/eliminar/productoTmp/' + id,
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
        url: '/apartado/eliminar/eliminarProductosTodos',
        type: 'GET',
        'success': function (data) {
            mostrarProductosTmp(1);
            Materialize.toast('A eliminado todos los articulos de la orden', 4000);
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function mostrarProductosTmp(view) {
    $(".articulos-orden").empty();
    $("#pre-ArticulosOrden").css("display", "inline");
    $.ajax({
        url: '/apartado/obtener/productosTmp',
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
                    '<p class="white-text">No hay articulos agregados a la orden, por favor seleccione o agregue los </br> articulos para poder realizar la venta.</p>' +
                    '<a class="btn btn-large white grey-text text-darken-2" onclick="abrirArticulos()">Seleccionar</a>' +
                    '</div > '
                );
                //detalles(0, 0);
                $("#pre-ArticulosOrden").css("display", "none");
            }
            else {
                if (view === 0) {
                    articulosOrdenCard(data);
                }
                if (view === 1) {
                    articulosOrdenTable(data);
                }
            }
            $("#pre-ArticulosOrden").css("display", "none");
        },
        'error': function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
}
function abrirArticulos() {
    $('#articulos').modal('open');
    articulos(1);
}
function articulosCard(data) {
    $.each(data, function () {
        $(".modal-ventas .articulos .card-v").hide();
        $(".modal-ventas .articulos .table-v").show();
        $(".lista-articulos").empty();
        $.each(this, function (name, value) {
            $(".lista-articulos").append(
                '<div class="col l4">' +
                '<div class="card horizontal">' +
                '<div class="card-image">' +
                '<img src="https://lorempixel.com/100/190/nature/6">' +
                '</div>' +
                '<div class="card-stacked">' +
                '<div class="card-content grey-text text-darken-3">' +
                '<p class="nombre truncate"><strong>' + value.Nombre + '</strong></p>' +
                '<p><strong class="left">Stock: 102<strong/><strong class="rigth">$ 2,500<strong/></p>' +

                '</div>' +
                '<div class="card-action">' +
                '<input class="left browser-default" placeholder="Cantidad" id="cant' + value.ID + '" type="text"></input>' +
                '<a class="btn btn-flat green white-text right" onclick="agregarArticuloTmp(' + value.ID + ')">Agregar</a>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</div>'
            );
        });
    });
}
function articulosTable(data) {
    $(".modal-ventas .articulos .table-v").hide();
    $(".modal-ventas .articulos .delete").show();
    $(".lista-articulos").empty();
    $(".lista-articulos").append(
        '<table class="responsive-table centered white z-depth-1 bordered highlight">' +
        '<thead>' +
        '<tr>' +
        '<th>Cod</th>' +
        '<th>Img</th>' +
        '<th>Nombre</th>' +
        '<th>Stock</th>' +
        '<th>Precio</th>' +
        '<th>Cantidad</th>' +
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
                '<td>' + value.ID + '</td>' +
                '<td><img src="/Content/images/articulos/' + value.Imagen + '"></td>' +
                '<td>' + value.Nombre + '</td>' +
                '<td id="stock' + value.ID + '">' + value.Stock + '</td>' +
                '<td>$7.00</td>' +
                '<td>' +
                '<input placeholder="Cantidad" id="cant' + value.ID + '" type="text" class="browser-default">' +
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
    $(".articulos-orden .articulosLista").toggleClass("blue white");
    $(".lista-articulos").empty();
    $(".articulos-orden .articulosLista").append(
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
                '<td>C$ 98829</td>' +
                '<td>' + '<a class="center" onclick= "eliminarProductoTmp(' + value.ID + ')">' + '<i class="material-icons">delete</i>' + '</a >' + '</td>' +
                '</tr>'
            );
        });
    });
}
function detectarCambios() {
    var ID_Obj;
    var AnteriorValor, NuevoValor;
    var dividiendo;
    var id;

    $(document).on({
        'focusin': function () {
            ID_Obj = $(this).attr("id");
            dividiendo = ID_Obj.split("-", 2);
            id = dividiendo[1];
            AnteriorValor = $(this).val();
        },
        'focusout': function () {
            NuevoValor = $(this).val();

            if (NuevoValor === '') {
                Materialize.toast("Debe ingresar la cantidad", 2000);
                $(this).focus();
                return;
            }
            else if (NuevoValor === 0) {
                Materialize.toast("La cantidad no puede ser 0", 2000);
                $(this).val(AnteriorValor);
                return;
            }
            else if (NuevoValor < 0) {
                Materialize.toast("La cantidad no es valida", 2000);
                $(this).val(AnteriorValor);
                return;
            }
            else if (NuevoValor === AnteriorValor) {
                return;
            }
            else if (!/^([0-9])*$/.test(NuevoValor)) {
                Materialize.toast("La cantidad no es valida", 2000);
                $(this).val(AnteriorValor);
                return;
            }

            var exist = parseInt($("#exist-" + id).text());
            if (NuevoValor > exist) {
                Materialize.toast("No hay sufucientes productos para realizar el pedido", 2000);
                $("#cant-" + id).val(AnteriorValor);
                return;
            }

            $.ajax({
                url: '/apartado/actualizar/cantidad/productoTmp/' + id + '/' + NuevoValor,
                type: 'GET',
                'success': function (data) {
                    mostrarProductosTmp(1);
                    Materialize.toast('Cantidad del articulo actualizado', 2000);
                },
                'error': function (request, error) {
                    alert("Request: " + JSON.stringify(request));
                }
            });
        }
    }, '.articulosLista table tbody input');
}