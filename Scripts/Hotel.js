$(document).ready(function () {
    autoComplete(6, "DestinoHotel");
    if ($("#hdnNoitesRequest").val() == "") {
        var checkinSplit = $("#Checkin").val().split("/");
        var checkoutSplit = $("#Checkout").val().split("/");

        var dataCheckinFormatada = new Date(checkinSplit[2], checkinSplit[1], checkinSplit[0]);
        var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

        var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

        var dias = Math.round(Math.abs((dataCheckinFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));


        $("#txtNoites").val(dias);
    }
    else { $("#txtNoites").val($("#hdnNoitesRequest").val()); }

    $(".fancybox-thumb").fancybox({
        prevEffect: 'none',
        nextEffect: 'none',
        helpers: {
            title: {
                type: 'outside'
            },
            thumbs: {
                width: 50,
                height: 50
            }
        }
    });

    $("#gridCarrinho").show();
    $("#gridCarrinho").load("../../Carrinho/LoadCarrinho", { teste: "" });
    $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', width: '400px' }).css("font-size", "8pt");

    $(".combo-ordenar").change(function () {
        $('.loadingDefault').show();

        var ordem = $(".combo-ordenar").val();
        var qtdItensPorPagina = $("#quantidadeResultados").val();

        var args = new Object();

        args = {
            ordem: ordem,
            qtdItensPorPagia: qtdItensPorPagina
        };

        $.ajax({
            url: '/Hotel/ResultadoGridNovaOrdenacao',
            type: 'POST',
            data: args,
            dataType: 'html',
            success: function (result) {
                $("#HotelResultado").html(result);

                $('.loadingDefault').hide();
            }
        });
    });

    $("#quantidadeResultados").change(function () {
        $('.loadingDefault').show();

        var qtdItensPorPagina = $("#quantidadeResultados").val();

        var args = new Object();

        args = { qtdItensPorPagina: qtdItensPorPagina };

        $.ajax({
            url: '/Hotel/AlterarQuantidadeItens',
            type: 'POST',
            data: args,
            dataType: 'html',
            success: function (result) {
                $("#HotelResultado").html(result);

                $('.loadingDefault').hide();

                $(holder).pagination('updateItemsOnPage', qtdItensPorPagina);
            }
        });
    });

    $("#plusRooms").click(function () {
        var quant = $(".listaQuartos li").length;
        var args = new Object();
        args = {
            indice: quant + 1
        };

        $.ajax({
            url: '/Home/Quartos',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'html',
            contentType: 'application/json',
            success: function (data) {
                $(".listaQuartos").append(data);
            },
            error: function () {
                alert("Problema ao adicionar mais quartos.");
            }
        });
        return false;
    });

    $("#minusRooms").click(function () {
        if ($(".listaQuartos li").length != 1) {
            $(".listaQuartos li").last().remove();
            return false;
        } else { return false; }
    });

    $('.bot-hoteis-mapa').click(function (e) {
        HoteisnoMapa();
    });

    $("#plusNights").click(function () {
        var text = $(this).next(":text");
            text.val(parseInt(text.val(), 10) + 1);
            noitesHotel();
    });

    $("#minusNights").click(function () {
        var text = $(this).prev(":text");
        if (text.val() != 1) {
            text.val(parseInt(text.val(), 10) - 1);
            noitesHotel();
        }
    });

    $('#txtNoites').change(function () {
        noitesHotel();
    });

    $("#Checkin").blur(function () {
        if ($("#Checkin").val() != "" && $("#Checkout").val() != "") {

            var checkinSplit = $("#Checkin").val().split("/");
            var checkoutSplit = $("#Checkout").val().split("/");

            var dataCheckinFormatada = new Date(checkinSplit[2], checkinSplit[1], checkinSplit[0]);
            var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

            var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

            var dias = Math.round(Math.abs((dataCheckinFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

            $("#txtNoites").val(dias);
            $("#hdnNoites").val(dias);
        }
    });

    $("#Checkout").blur(function () {
        if ($("#Checkin").val() != "" && $("#Checkout").val() != "") {

            var checkinSplit = $("#Checkin").val().split("/");
            var checkoutSplit = $("#Checkout").val().split("/");

            var dataCheckinFormatada = new Date(checkinSplit[2], checkinSplit[1], checkinSplit[0]);
            var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

            var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

            var dias = Math.round(Math.abs((dataCheckinFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

            $("#txtNoites").val(dias);
            $("#hdnNoites").val(dias);
        }
    });

    $(".numeroestrelas").change(function () {
        FiltrarPorEstrelas();
    });

    $(".facilidadesFiltro").change(function () {
        FiltrarPorFacilidades();
    });
});

function ChangeSlct(item) {
    $('option:selected', $("#" + item)).attr('selected', true).siblings().removeAttr('selected');
}

function add(nomeElemento) {
    var text = $("#" + nomeElemento).next(":text");
    text.val(parseInt(text.val(), 10) + 1);
}

function removeQtdAdultos(nomeElemento) {
    var text = $("#" + nomeElemento).prev(":text");
    if (text.val() != 0) {
        text.val(parseInt(text.val(), 10) - 1);
    }
}

function qtdDiasHotel() {
    if ($("#Checkin").val() != "" && $("#Checkout").val() != "") {
        var checkinSplit = $("#Checkin").val().split("/");
        var checkoutSplit = $("#Checkout").val().split("/");
        var dataCheckinFormatada = checkinSplit[2] + "-" +  checkinSplit[1]  + "-" + checkinSplit[0];
        var dataCheckoutFormatada = checkoutSplit[2] + "-" + checkoutSplit[1] + "-" + checkoutSplit[0];
        var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos
        var dias = Math.abs(((new Date(dataCheckoutFormatada) - new Date(dataCheckinFormatada)) / 24 / 3600000) >> 0);
        $("#txtNoites").val(dias);
        $("#hdnNoites").val(dias);
    }
}

function qtCriancas(sender, indice) {
    var quant = sender.value;
    $("#criancas_" + indice).html("");
    if (quant > 0) {
        for (var i = 0; i < quant; i++) {
            $.ajax({
                url: '/Home/Criancas',
                type: 'POST',
                data: '',
                dataType: 'html',
                contentType: 'application/json',
                success: function (data) {
                    $("#roomChilAge_" + indice).show();
                    $("#criancas_" + indice).append(data);
                },
                error: function () {
                    alert("Erro ao carregar número de crianças.");
                }
            });
        }
    }
    else {
        $("#roomChilAge_" + indice).hide();
    }
}
function changeNightsKeyHotel(valor) {
    if (parseInt(valor) > 0) {
        noitesHotel();
    }
}
function noitesHotel() {
    Days = parseInt($('#txtNoites').val());
    Day = $('#Checkin').val();
    var strData = new String(Day);
    var dia = new String(strData.substr(0, 2));
    var mes = new String(strData.substr(3, 2));
    var ano = new String(strData.substr(6, 4));

    if (dia.substr(0, 1) == "0") { dia = dia.substr(1, 1); }
    if (mes.substr(0, 1) == "0") { mes = mes.substr(1, 1); }

    var d = new Date(parseInt(ano), (parseInt(mes) - 1), parseInt(dia), 1, 0, 0, 0);

    d.setTime(d.getTime() + (parseInt(Days) * 1000 * 60 * 60 * 24));
    dia = d.getDate();
    mes = d.getMonth() + 1;
    ano = d.getFullYear();

    strDia = new String();
    strDia = dia.toString();
    strMes = mes.toString();
    if (strDia.length == 1) { strDia = "0" + strDia; }
    if (strMes.length == 1) { strMes = "0" + strMes; }

    $('#Checkout').val(strDia + '/' + strMes + '/' + ano);
}

function onlydate(sender) {
    sender.val($('#Checkout').dateFormat('dd/mm/yy'));
    sender.parseDate('dd/mm/yy', $('#Checkin').val());
}

function hotelGalleryDetalhes(fotos) {
    $('.loadingDefault').show();
    var args = new Object();
    args = fotos;
    $.ajax({
        url: '/Hotel/HotelGallery',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $('.loadingDefault').hide();
            $.fancybox.open(data, {
                padding: 0,
                type: 'image'
            });
        },
        error: function () {
            alert("Problema ao carregar fotos do hotel. ");
            $('.loadingDefault').hide();
        }
    });
}

function resultadoHotel() {

    $(".bot-hoteis-lista").hide();
    $("#itemContainer").show();
    $(".holder").show();
    $('.HotelDetalhado').hide();
}

function detalheHotel(hotel) {

    $(".bot-hoteis-lista").show();
    $('.loadingDefault').show();
    $("#itemContainer").hide();
    $(".holder").hide();
    $('.HotelDetalhado').show();

    var args = new Object();
    args = hotel;
    $.ajax({
        url: '/Hotel/HotelDetalhes',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $(".HotelDetalhado").html(data);
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao exibir detalhes do hotel.");
        }
    });
}

function VerDetalheHotel(codigohotel, checkin, checkout, codigocidade, quantcrianca, quantadulto) {
    $.modal.close();
    $('.loadingDefault').show();
    $("#itemContainer").hide();
    $(".holder").hide();
    $('.HotelDetalhado').show();

    var args = new Object();
    args = {
        codigohotel: codigohotel,
        checkin: checkin,
        checkout: checkout,
        codigocidade: codigocidade,
        quantcrianca: quantcrianca,
        quantadulto: quantadulto
    };

    $.ajax({
        url: '/Hotel/BuscarHotelDetalhe',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $(".HotelDetalhado").html(data);
            $('.loadingDefault').hide();

            $('.bot-hoteis-lista').show();
            $('.bot-hoteis-mapa').hide();
        },
        error: function () {
            alert("Problema ao exibir detalhes do Hotel. ");
        }
    });
}

function SendToServerHotel() {
    if ($('#ddlQ1Adulto').val() == 0) {
        alert('Favor selecione ao menos um adulto!');
        return false;
    }
    
    if (FormatadaDatas($("#Checkin").val()) < FormatadaDatas(getToday())) {
        alert('Favor selecione uma data valida');
        return false;
    }


    $(".loadingHotel").show();

    $("#imgload").imagesLoaded($("#imgload"));
    $("#hotelform").validate();
    if (!$("#hotelform").valid()) {
        $('.loadingHotel').hide();
        return false;
    }

    $('.watermark').watermark('clearWatermarks');

    if($("#hdnDestinoHotelCodigo").val() == null || $("#hdnDestinoHotelCodigo").val() == "")
    {
        $('.loadingHotel').hide();
        alert("Você deve selecionar um destino válido.");
        return false;
    }

    var criancas = Array();
    var agechild = Array();

    var Apartamentos = Array();
    var Quartos = $(".listaQuartos li");

    for (var i = 0; i < Quartos.length ; i++) {
        var Apartamento = new Object();
        Quartos[i].id = "liId" + i;
        Apartamento.quantadulto = $("#liId" + i + " input.QuantAdultos").val();
        Apartamento.criancas = Array();
        agechild = $("#" + Quartos[i].id + " .agechild");
        for (var j = 0; j < agechild.length ; j++) {
            Apartamento.criancas.push(agechild[j].value);
        }
        Apartamentos.push(Apartamento);
    }
    $("#origem").val(JSON.stringify(Apartamentos));
}

function postarform() {
    $('.watermark').watermark('clearWatermarks');
    $(".loadingHotel").show();

    var index = 0;
    var estrela = "";

    for (var i = 0; i < $(".numeroestrelas").length; i++) {
        if ($(".numeroestrelas")[i].checked) {
            estrela += $(".numeroestrelas")[i].value;
            estrela += ",";
        }
    }

    var facilidade = "";

    for (var i = 0; i < $(".facilidadesFiltro").length; i++) {
        if ($(".facilidadesFiltro")[i].checked) {
            facilidade += $(".facilidadesFiltro")[i].value;
            facilidade += ",";
        }
    }

    var quantCriancas = $("#QuantCriancas").val();
    var quantAdultos = $("#QuantAdultos").val();
    var ordernacao = $("#selectOrder").val();
    var checkout = $("#Checkout").val();
    var codigocidade = $("#CodigoCidade").val();
    var rangeValor = $("#amount").val();
    var nomehotel = $("#txtNome").val();
    var checkin = $("#Checkin").val();
    var codigocidade = $("#CodigoCidade").val();
    rangeValor = rangeValor.replace("R$", "");
    rangeValor = rangeValor.replace("R$", "");
    rangeValor = rangeValor.replace(" ", "");
    rangeValor = rangeValor.split("-");

    var args = new Object();
    args = {
        nome: nomehotel,
        menorPreco: rangeValor[0],
        maiorPreco: rangeValor[1],
        estrela: estrela,
        lstFacilidades: facilidade,
        hotelBuscaCheckin: checkin,
        hotelBuscaCheckout: checkout,
        order: ordernacao,
        quantAdultos: quantAdultos,
        quantCriancas: quantCriancas,
        codigocidade: codigocidade
    };

    $.ajax({
        url: '/Hotel/FiltroHotel',
        type: 'POST',
        data: args,
        success: function (result) {
            $("#HotelResultado").html(result);
            $('.loadingHotel').hide();
        }
    });
}


function Registrar(Hotel, Acomodacao, Tarifa, comprar) {
    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = {
        Carrinhoitem: Hotel,
        NomeAcomodacao: Acomodacao,
        CodigoTarifa: Tarifa,
        Comprar: comprar
    };
    $.ajax({
        url: '/Carrinho/AdicionarHotelCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (comprar) {
                window.location = "/Reserva";
            }
            else {
                AtualizarCarrinho();
                $('.loadingDefault').fadeOut();
                alert('Adicionado ao Carrinho!');
            }
        },
        error: function () {
            $('.loadingDefault').fadeOut();
            alert("Problema ao Registrar hotel.");
        }
    });
}

function AdicionarOrcamento(Hotel, Acomodacao, Tarifa) {
    $('.loadingDefault').show();

    var args = new Object();
    args = {
        orcamentoItem: Hotel,
        NomeAcomodacao: Acomodacao,
        CodigoTarifa: Tarifa
    };
    $.ajax({
        url: '/Orcamento/AdicionarHotelOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function (data) {
            alert("Problema ao adicionar item no orçamento.");
            alert(data);
        }
    });
}

function AdicionarMontePacoteTipoPacote1e3(Hotel, Acomodacao, Tarifa) {
    var args = new Object();
    args = {
        Carrinhoitem: Hotel,
        NomeAcomodacao: Acomodacao,
        CodigoTarifa: Tarifa
    };
    $.ajax({
        url: '../../Carrinho/AdicionarHotelCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/MPVoo";
        }
    });
}

function AdicionarMontePacoteTipoPacote2(Hotel, Acomodacao, Tarifa) {
    var args = new Object();
    args = {
        Carrinhoitem: Hotel,
        NomeAcomodacao: Acomodacao,
        CodigoTarifa: Tarifa
    };
    $.ajax({
        url: '../../Carrinho/AdicionarHotelCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/MPCarro";
        }
    });
}

function ShowOrcamento(SessionKey) {
    $.ajax({
        url: '/Orcamento/ShowOrcamento',
        type: 'POST',
        data: { SessionKey: SessionKey },
        dataType: 'html',
        success: function (data) {
            $("#orcamentocontent").html("");
            $("#orcamentocontent").append(data);
            $('.loadingDefault').hide();
        },
        error: function (data) {
            alert("Problema ao exibir o orçamento.");
            alert(data);
        }
    });
}

$(function () {
    $("#clickme").click(function () {
        if ($('#moreoptionshotel').css('display') == "block") {
            $('#imgav').attr('src', '../Images/ava.gif');
        } else {
            $('#imgav').attr('src', '../Images/avades.gif');
        }
        $("#moreoptionshotel").toggle("slow");
    });
});

$(function () {
    $("#clickmeMenu").click(function () {
        if ($('#moreoptionshotel').css('display') == "block") {
            $('#imgav').attr('src', '../Images/ava.gif');
        } else {
            $('#imgav').attr('src', '../Images/avades.gif');
        }
        $("#moreoptionshotel").toggle();

        AjusteHeightFrameParent("iframeBuscadorHotel", 10);
    });
});

$(function () {

    $("#clickmeResult").click(function () {
        if ($('#moreoptionshotel').css('display') == "block") {
            $("#clickmeResult").show();
            $('#imgavResult').attr('src', '../Images/avsa.jpg');
        } else {
            $("#clickmeResult").show();
            $('#imgavResult').attr('src', '../Images/avadess.jpg');
        }
        $("#moreoptionshotel").toggle("slow");
        $("#clickmeResult").show();
    });
});

$(function () {
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);

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
    $('#Checkout').datepicker({
        showOn: "both",
        buttonImageOnly: false,
        numberOfMonths: 2,
        minDate: 2,
        onSelect: function (dateText, inst) {
            var date2 = $('#Checkin').val();
            if (dateText == date2) {
                alert('Não é possivel selecionar a mesma data para checkout.');
                return false;
            }



            qtdDiasHotel();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
    $('#Checkin').datepicker({
        showOn: "both",
        buttonImageOnly: false,
        numberOfMonths: 2,
        minDate: 2,
        onSelect: function (dateText, inst) {
            $('#Checkout').datepicker("option", "minDate", dateText);
            var date2 = $('#Checkin').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            var noitesHT = parseInt($("#txtNoites").val());
            nextDayDate.setDate(nextDayDate.getDate() + noitesHT);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#Checkout').val(nextDayDate);
            validarDataRetorno('Checkin', 'Checkout');
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});

function VerPrecoParcelas(div, acomodacao) {
    $("." + div + "-Parcelas").show();
    args = {
        CodigoTarifa: div,
        NomeAcomodacao: acomodacao
    };
    $.ajax({
        url: '/Hotel/FormasdePagamento',
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