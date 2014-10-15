

$(document).ready(function () {
    //$.mask.definitions['~']='[+-]';
    $("#DataCheckIn").mask("99/99/9999");
    $("#DataCheckOut").mask("99/99/9999");
    
});

function verDetalhesCarro(carro) {
    var args = new Object();
    args = carro;
    $.ajax({
        url: '/Carro/ReservaCarro',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divReservaCarro").html(data);

            $("#divReservaCarro").modal({
                minHeight: 700,
                minWidth: 705,
                onClose: function (dialog) {
                    dialog.data.fadeOut('slow', function () {
                        dialog.container.slideUp('slow', function () {
                            dialog.overlay.fadeOut('slow', function () {
                                $.modal.close(); // must call this!
                                $("#divReservaCarro").html("");
                            });
                        });
                    });
                }
            });
        },
        error: function () {
            alert("Problema ao exibir detalhes.");
        }
    });
}

function PostarFormCarro() {

    $('.loadingCarro').show();

    if ($('#hdnLocalRetiradaCodigo').val() == "") {
        $('.loadingCarro').hide();
        alert('Você deve selecionar um local de retirada válido.');
        return false;
    }

    if (FormatadaDatas($("#DataCheckIn").val()) < FormatadaDatas(getToday())) {
        alert('Favor selecione uma data valida');
        $('.loadingCarro').hide();
        return false;
    }

    if ($("#IsLocalDiferenteEntrega").is(":checked")) {
        if ($("#hdnLocalDevolucaoCodigo").val() == "") {
            $('.loadingCarro').hide();
            alert("Você deve selecionar um local de devolução válido.");
            return false;
        }
    }

    $("#carroform").validate();
    if (!$("#carroform").valid()) {
        $('.loadingCarro').hide();
        return false;
    }
}

function AdicionarCarrinho(Carro, comprar) {
    $('.loadingCarro').fadeIn();
    var args = new Object();
    args = { reservaCarroModel: Carro, Comprar: comprar };
    $.ajax({
        url: '/Carrinho/AdicionarCarroCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (comprar) {
                $.modal.close();
                window.location = "/Reserva";
            }
            else {
                $.modal.close();
                AtualizarCarrinho();
                $('.loadingCarro').fadeOut();
                alert('Adicionado ao Carrinho!');
            }

        },
        error: function () {
            alert("Problema ao adicionar item no carrinho.");
        }
    });
}

function AdicionarCarrinhoMontePacote(Carro) {
    var args = new Object();
    args = Carro;
    $.ajax({
        url: '/Carrinho/AdicionarCarroCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $.modal.close();
            window.location = "/MontePacote/MPOpcionais";
        },
        error: function () {
            alert("Problema ao adicionar item no carrinho.");
        }
    });

}

function AdicionarOrcamento(Carro) {
    var args = new Object();
    args = Carro;
    $.ajax({
        url: '/Orcamento/AdicionarCarroOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $('html, body').animate({ scrollTop: 0 }, 'fast');
            ShowOrcamento(data);
        },
        error: function () {
            alert("Problema ao adicionar item ao orçamento.");
        }

    });
}

function ShowOrcamento(SessionKey) {
    $.ajax({
        url: '../../Orcamento/ShowOrcamento',
        type: 'POST',
        data: { SessionKey: SessionKey },
        dataType: 'html',
        success: function (data) {
            $("#orcamentocontent").html("");
            $("#orcamentocontent").append(data);
            $('.loadingCarro').hide();
            $.modal.close();
        },
        error: function () {
            alert("Problema ao exibir o orçamento.");
        }
    });
}

function FinalizarCompra(Carro) {
    var args = new Object();
    args = Carro;
    $.ajax({
        url: '/Carro/EfetuarReserva',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $("#divReservaCarro").hide();

            alert("Reserva efetuada com sucesso.");
        },
        error: function () {
            alert("Problema ao finalizar compra.");
        }
    });
}

function GetBaseUrl() {
    try {
        var url = location.href;

        var start = url.indexOf('//');
        if (start < 0)
            start = 0
        else
            start = start + 2;

        var end = url.indexOf('/', start);
        if (end < 0) end = url.length - start;

        var baseURL = url.substring(start, end);
        return baseURL;
    }
    catch (arg) {
        return null;
    }
}

$(function () {
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    $.datepicker.parseDate("dd-mm-yy", "26-01-2007");

    $.datepicker.regional['pt-BR'] = {
        closeText: 'Fechar',
        prevText: '&#x3c;Anterior',
        nextText: 'Pr&oacute;ximo&#x3e;',
        currentText: 'Hoje',
        monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
        'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: '',
        showOn: "both",
        changeFirstDay: false
    };
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    $('#DataCheckOut').datepicker({
        numberOfMonths: 2,
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
    $('#DataCheckIn').datepicker({
        numberOfMonths: 2,
        minDate: 2,
        onSelect: function (dateText, inst) {
            $('#DataCheckOut').datepicker("option", "minDate", dateText);

            var date2 = $('#DataCheckIn').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            nextDayDate.setDate(nextDayDate.getDate() + 1);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#DataCheckOut').val(nextDayDate);
            validarDataRetorno('DataCheckIn', 'DataCheckOut');
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});

$(function () {
    $("#IsLocalDiferenteEntrega").click(function () {
        if ($("#IsLocalDiferenteEntrega").is(':checked')) {
            $("#divLocalDevolucao").show();
        }
        else {
            $("#divLocalDevolucao").hide();
        }
    });
});

$(function () {
    $("#IsLocalDiferenteEntrega").click(function () {
        if ($("#IsLocalDiferenteEntrega").is(":checked")) {
            $("#hdnIsLocalDiferenteEntrega").val("true");
        }
        else {
            $("#hdnIsLocalDiferenteEntrega").val("false");
        }
    });
});

$(function () {

    $(".buttonRefazerPesquisa-filtros-left").click(function () {

        if ($("#hdnLocalRetiradaCodigo").val() == null || $("#hdnLocalRetiradaCodigo").val() == "") {
            alert("Por favor informar um local de retirada válido.");
            return false;
        }
        
        if ($("#IsLocalDiferenteEntrega").is(":checked")) {
            if ($("#hdnLocalDevolucaoCodigo").val() == null || $("#hdnLocalDevolucaoCodigo").val() == "") {
                alert("Por favor informar um local de devolução válido.");
                return false;
            }
        }

        $('.loadingCarro').show();

        $("#FiltroLateralCarro").validate();

        if (!$("#FiltroLateralCarro").valid()) {
            $('.loadingCarro').hide();
            $('span:contains("Campo obrigatório")').css('display', 'block');
            return false;
        }
        var localDevolucao;
        var HdnLocalDevolucaoCodigo;
        var HdnLocalRetiradaCodigo = $("#hdnLocalRetiradaCodigo").val();
        
        var localretirada = $("#LocalRetirada").val();
        var dataCheckIn = $("#DataCheckIn").val();
        var dataCheckOut = $("#DataCheckOut").val();
        var horaCheckInSelecionada = $("#HoraCheckInSelecionada").val();
        var horaCheckOutSelecionada = $("#HoraCheckOutSelecionada").val();

        if ($("#IsLocalDiferenteEntrega").is(":checked")) {
            localDevolucao = $("#LocalDevolucao").val();
            HdnLocalDevolucaoCodigo = $("#hdnLocalDevolucaoCodigo").val();
        } else {
            localDevolucao = localretirada;
            HdnLocalDevolucaoCodigo = HdnLocalRetiradaCodigo;
        }
        
        var IsLocalDiferenteEntrega = $("#IsLocalDiferenteEntrega").is(":checked");

        $("#lblLocalRetirada").text(localretirada);
        $("#lblDataCheckin").text(dataCheckIn);
        $("#lblDataCheckOut").text(dataCheckOut);
        //$("#lblHoraCheckIn").text(horaCheckInSelecionada);
        //$("#lblHoraCheckOut").text(horaCheckOutSelecionada);
        //$("#lblLocalDevolucao").text(localDevolucao);
        var args = new Object();
        args = {

            LocalRetirada: localretirada,
            DataCheckIn: dataCheckIn,
            DataCheckOut: dataCheckOut,
            HoraCheckInSelecionada: horaCheckInSelecionada,
            HoraCheckOutSelecionada: horaCheckOutSelecionada,
            LocalDevolucao: localDevolucao
        };

        //(string hdnLocalRetiradaCodigo, string hdnLocalDevolucaoCodigo, bool isLocalDiferenteEntrega, string dataRetirada, string horaRetirada, 
        //string dataDevolucao, string horaDevolucao, string nomeCidadeRetirada, string nomeCidadeDevolucao)

        $.ajax({
            url: '/Carro/ResultadoFiltroLateral',
            type: 'POST',
            data: {
                hdnLocalRetiradaCodigo: HdnLocalRetiradaCodigo, hdnLocalDevolucaoCodigo: HdnLocalDevolucaoCodigo, isLocalDiferenteEntrega: IsLocalDiferenteEntrega,
                dataRetirada: dataCheckIn, horaRetirada: horaCheckInSelecionada, dataDevolucao: dataCheckOut, horaDevolucao: horaCheckOutSelecionada,
                nomeCidadeRetirada: localretirada, nomeCidadeDevolucao: localDevolucao
            },
            beforeSend: function () { showdivLoad(); },
            complete: function () { hidedivLoad(); },
            dataType: 'html',
            success: function (result) {
                $("#divresultado").html(result);
                $('.loadingCarro').hide();
            },
            error: function () {
                $('.loadingCarro').hide();
                alert("Problema ao carregar filtro do carro. ");
            }
        });

    });
});

$(document).ready(function () {
    autoComplete(9, "LocalRetirada");
    autoComplete(9, "LocalDevolucao");

    if ($("#DataCheckIn").val() == "") {
        $("#DataCheckIn").val(getToday());
    }

    if ($("#DataCheckOut").val() == "") {
        $("#DataCheckOut").val(getTomorow());
    }

    if ($("#hdnIsLocalDiferenteEntrega").val() == "true") {
        $('input[name=hdnIsLocalDiferenteEntrega]').attr('checked', true);
    }

    $('span:contains("The field {0} is invalid.")').css('display', 'none');
});



function alterarLocalRetirada(valor, LocadoraLoja, carro) {

    var nomeLocadoraLoja = LocadoraLoja.options[LocadoraLoja.selectedIndex].text;
    var args = new Object();

    args = { valor: valor, nomeLocadoraLoja: nomeLocadoraLoja, carroModel: carro };

    $.ajax({
        url: '/Carro/AlterarLocalRetirada',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divReservaCarro").html(data);
        },
        error: function () {
            alert("Problema ao alterar local de retirada.");
        }
    });
}

function alterarLocalDevolucao(valor, LocadoraLoja, carro) {

    var nomeLocadoraLoja = LocadoraLoja.options[LocadoraLoja.selectedIndex].text;
    var args = new Object();

    args = { valor: valor, nomeLocadoraLoja: nomeLocadoraLoja, carroModel: carro };

    $.ajax({
        url: '/Carro/AlterarLocalDevolucao',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divReservaCarro").html(data);
        },
        error: function () {
            alert("Problema ao alterar local de devolução.");
        }
    });
}

////function responsavel por fazer a div flutuar na pagina
//function divFlutua() {
//    // visto que a div esta com float: left no css para que ela flutue na página
//    // crie usei as informações da pagina e da div para centralizar a mesma usando
//    // o atributo left para definir a distancia o objeto da margen direita da pagina
//    var CenterFlutua = $(document).width()-(($(document).width() - $(".divDetalhesVeiculo").width()) / 2);
//    $(".regiao-detalhe-comprar").css("left", CenterFlutua);

//    // aplica animação na div toda fez que o scroll da janela é usado
//    $(window).scroll(function () {
//        $(".regiao-detalhe-comprar").stop().animate({
//            // basicamente é utilizado a posição do scroll vertical
//            // para definir a posição da div atravéz da propriedade marginTop
//            marginTop: $(window).scrollTop()
//        });
//    });
//}


function VerPrecoParcelas(div) {
    $("." + div + "-Parcelas").show();
    args = {
        CodigoTarifa: div
    };
    $.ajax({
        url: '/Carro/FormasdePagamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("." + div + "-Parcelas div").html(data);
        },
        error: function () {
            $("." + div + "-Parcelas div").html("Tente novamente mais tarde.");
            $("." + div + "-Parcelas").hide();
        }
    });
}

function FecharPrecoParcelas(div, preco) {
    $("." + div + "-Parcelas").hide();
}
