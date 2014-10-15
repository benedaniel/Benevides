$(document).ready(function () {

    $("#txtNoitesMonte").val(1);
    $("#Saida").val(getToday());
    $("#Retorno").val(getTomorow());

    $("#NovotxtNoitesMonte").val(1);
    $("#NovaSaida").val(getToday());
    $("#NovoRetorno").val(getTomorow());

    $("#OrigemMP").addClass("inputdestino watermark");
    $("#OrigemMP").attr("title", "Informe sua origem");
    $("#OrigemMP").val("");
    $('#OrigemMP').watermark();

    $("#DestinoMP").addClass("inputdestino watermark");
    $("#DestinoMP").attr("title", "Informe seu destino");
    $("#DestinoMP").val("");
    $('#DestinoMP').watermark();

    autoComplete(6, "Origem");
    autoComplete(6, "Destino");

    $('#FiltroMP').show();
    $("#FiltroMP").html("");

    //$(".combo-ordenar").change(function () {
    //    $('.loadingDefault').show();

    //    var ordem = $(".combo-ordenar").val();
    //    var qtdItensPorPagina = $("#quantidadeResultados").val();

    //    var args = new Object();

    //    args = {
    //        ordem: ordem,
    //        qtdItensPorPagia: qtdItensPorPagina,
    //        isMontePacote: true
    //    };

    //    $.ajax({
    //        url: '/Hotel/ResultadoGridNovaOrdenacao',
    //        type: 'POST',
    //        data: args,
    //        dataType: 'html',
    //        success: function (result) {
    //            $("#HotelResultado").html(result);

    //            $('.loadingDefault').hide();
    //        }
    //    });
    //});

    //$("#quantidadeResultados").change(function () {
    //    $('.loadingDefault').show();

    //    var qtdItensPorPagina = $("#quantidadeResultados").val();

    //    var args = new Object();

    //    args = {
    //        qtdItensPorPagina: qtdItensPorPagina,
    //        isMontePacote: true
    //    };

    //    $.ajax({
    //        url: '/Hotel/AlterarQuantidadeItens',
    //        type: 'POST',
    //        data: args,
    //        dataType: 'html',
    //        success: function (result) {
    //            $("#HotelResultado").html(result);

    //            $('.loadingDefault').hide();

    //            $(holder).pagination('updateItemsOnPage', qtdItensPorPagina);
    //        }
    //    });
    //});



    $("#plusRoomsMonte").click(function () {
        var quant = $(".listaQuartosMP li").length;
        var args = new Object();
        args = {
            indice: quant + 1
        };

        $.ajax({
            url: '/MontePacote/QuartosMP',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'html',
            contentType: 'application/json',
            success: function (data) {
                $(".listaQuartosMP").append(data);
            },
            error: function () {
                alert("Problema ao adicionar mais quartos.");
                //window.location = "/Home";
            }
        });
        return false;
    });

    $("#minusRoomsMonte").click(function () {
        if ($(".listaQuartosMP li").length != 1) {
            $(".listaQuartosMP li").last().remove();
            return false;
        } else { return false; }
    });

    $("#plusNightsMonte").click(function () {
        var text = $(this).next(":text");
        text.val(parseInt(text.val(), 10) + 1);
        noites();
    });

    $("#minusNightsMonte").click(function () {
        var text = $(this).prev(":text");
        if (text.val() != 1) {
            text.val(parseInt(text.val(), 10) - 1);
            noites();
        }
    });

    $('#txtNoitesMonte').change(function () {
        noites();
    });

    $("#NovoplusNightsMonte").click(function () {
        var text = $(this).next(":text");
        text.val(parseInt(text.val(), 10) + 1);
        NovoNoites();
    });

    $("#NovominusNightsMonte").click(function () {
        var text = $(this).prev(":text");
        if (text.val() != 1) {
            text.val(parseInt(text.val(), 10) - 1);
            NovoNoites();
        }
    });

    $("#NovotxtNoitesMonte").change(function () {
        NovoNoites();
    });

    $("#Saida").blur(function () {
        if ($("#Saida").val() != "" && $("#Retorno").val() != "") {

            var checkinSplit = $("#Saida").val().split("/");
            var checkoutSplit = $("#Retorno").val().split("/");

            var dataCheckinFormatada = new Date(checkinSplit[2], checkinSplit[1], checkinSplit[0]);
            var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

            var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

            var dias = Math.round(Math.abs((dataCheckinFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

            $("#txtNoitesMonte").val(dias);
            $("#hdnNoitesMonte").val(dias);
        }
    });

    $("#NovaSaida").blur(function () {
        if ($("#NovaSaida").val() != "" && $("#NovoRetorno").val() != "") {

            var SaidaSplit = $("#NovaSaida").val().split("/");
            var checkoutSplit = $("#NovoRetorno").val().split("/");
            if (SaidaSplit[1] == "01") {
                SaidaSplit[1] = "0";
            }

            if (checkoutSplit[1] == "01") {
                checkoutSplit[1] = "0";
            }

            var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
            var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

            var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

            var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

            $("#NovotxtNoitesMonte").val(dias);
            $("#NovohdnNoitesMonte").val(dias);
        }
    });

    $("#Retorno").blur(function () {
        if ($("#Saida").val() != "" && $("#Retorno").val() != "") {

            var SaidaSplit = $("#Saida").val().split("/");
            var checkoutSplit = $("#Retorno").val().split("/");
            if (SaidaSplit[1] == "01") {
                SaidaSplit[1] = "0";
            }

            if (checkoutSplit[1] == "01") {
                checkoutSplit[1] = "0";
            }

            var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
            var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

            var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

            var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

            $("#txtNoitesMonte").val(dias);
            $("#hdnNoitesMonte").val(dias);
        }
    });

    $("#NovoRetorno").blur(function () {
        if ($("#NovaSaida").val() != "" && $("#NovoRetorno").val() != "") {

            var SaidaSplit = $("#NovaSaida").val().split("/");
            var checkoutSplit = $("#NovoRetorno").val().split("/");
            if (SaidaSplit[1] == "01") {
                SaidaSplit[1] = "0";
            }

            if (checkoutSplit[1] == "01") {
                checkoutSplit[1] = "0";
            }

            var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
            var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

            var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

            var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

            $("#NovotxtNoitesMonte").val(dias);
            $("#NovohdnNoitesMonte").val(dias);
        }
    });
});

function VerVooMP(divs, sender) {
    $("#" + divs).toggle("fast");
    if ($("#" + divs).css('display') == "block") {
        $('.img' + sender).attr('src', '../Images/avas.jpg');
    } else {
        $('.img' + sender).attr('src', '../Images/avadess.jpg');
    }

    //$("#" + divs).toggle("fast", function () {
    //    $('.img' + sender).attr('src', '../Images/avadess.jpg');
    //});

    //$('#btnFiltro').toggle(function () {
    //    $("#FiltroMP").animate({
    //        width: 309,
    //        height: 605,
    //    }, 800, function () {
    //        $('.content-left').show("fast");
    //    });
    //}, function () {
    //    $('.content-left').hide("fast");
    //    $("#FiltroMP").animate({
    //        width: 41,
    //        height: 135,
    //    }, 1000, function () {
    //    });
    //});
}

function SendToServerMP() {

    var retornoValidacao = Validar();


    if ($('#hdnOrigemCodigo').val() == "") {
        $('.loadingDefault').hide();
        alert('Você deve selecionar uma origem válida.');
        return false;
    }


    if (FormatadaDatas($("#Saida").val()) < FormatadaDatas(getToday())) {
        alert('Favor selecione uma data valida');
        $('.loadingDefault').hide();
        return false;
    }

    if ($('#hdnDestinoCodigo').val() == "") {
        $('.loadingDefault').hide();
        alert('Você deve selecionar um destino válido.');
        return false;
    }
    var item = parseInt($(".adultomp").val());

    if (item <= 0) {
        $('.loadingDefault').hide();
        alert('Você deve selecionar os passageiros da viagens.');
        return false;
    }

    if (!retornoValidacao) {
        $('.loadingDefault').hide();
        alert('Você deve selecionar os passageiros da viagens.');
        return false;
    }
    else {
        $('.loadingDefault').show();

        //$('.loadingDefault').show();
        $('.watermark').watermark('clearWatermarks');
        var criancas = Array();
        var agechild = Array();

        var Apartamentos = Array();
        var Quartos = $(".listaQuartosMP li");

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

        $("#origemMP").val(JSON.stringify(Apartamentos));

        $("#formMontePacote").submit();
    }
}

function Validar() {

    var validado = true;

    if ($("#Origem").val() == "" || $("#Origem").val() == "Informe sua origem") {
        validado = false;
        $("#lblErroOrigem").text("Por favor informar a origem");
        $("#lblErroOrigem").css("display", "block");
    }
    else {
        $("#lblErroOrigem").text("");
        $("#lblErroOrigem").css("display", "none");
    }

    if ($("#Destino").val() == "" || $("#Destino").val() == "Informe seu destino") {
        validado = false;
        $("#lblErroDestino").text("Por favor informar o destino");
        $("#lblErroDestino").css("display", "block");
    }
    else {
        $("#lblErroDestino").text("");
        $("#lblErroDestino").css("display", "none");
    }

    if ($("#Saida").val() == "") {
        validado = false;
        $("#lblErroDataSaida").text("Por favor informar a data de saída");
        $("#lblErroDataSaida").css("display", "block");
    }
    else {

        var dataSeparada = $("#Saida").val().split("/");

        var dataFormatada = new Date(dataSeparada[2], dataSeparada[1] - 1, dataSeparada[0]);

        if (!/Invalid|NaN/.test(dataFormatada)) {

            var dataAtual = new Date();

            var comparacao = dataFormatada > dataAtual;

            if (comparacao) {
                $("#lblErroDataSaida").text("");
                $("#lblErroDataSaida").css("display", "none");
            }
            else {
                validado = false;
                $("#lblErroDataSaida").text("Data de saída não pode ser menor ou igual a data de hoje");
                $("#lblErroDataSaida").css("display", "block");
            }
        }
        else {
            validado = false;
            $("#lblErroDataSaida").text("Data de saída inválida");
            $("#lblErroDataSaida").css("display", "block");
        }
    }

    if ($("#Retorno").val() == "") {
        validado = false;
        $("#lblErroDataRetorno").text("Por favor informar a data de retorno");
        $("#lblErroDataRetorno").css("display", "block");
    }
    else {
        var dataCheckIn = $("#Saida").val();
        var dataSeparadaCheckin = dataCheckIn.split("/");
        var dataCheckinFormatada = new Date(dataSeparadaCheckin[2], dataSeparadaCheckin[1] - 1, dataSeparadaCheckin[0]);
        var dataSeparadaCheckout = $("#Retorno").val().split("/");
        var dataCheckoutFormatada = new Date(dataSeparadaCheckout[2], dataSeparadaCheckout[1] - 1, dataSeparadaCheckout[0]);

        var comparacao = (new Date(dataCheckoutFormatada) > new Date(dataCheckinFormatada));

        if (comparacao) {
            $("#lblErroDataRetorno").text("");
            $("#lblErroDataRetorno").css("display", "none");
        }
        else {
            validado = false;
            $("#lblErroDataRetorno").text("Data de retorno tem que ser maior que a data de saída");
            $("#lblErroDataRetorno").css("display", "block");
        }
    }

    var Quartos = $(".listaQuartosMP li");

    for (var i = 0; i < Quartos.length ; i++) {
        var Apartamento = new Object();
        Quartos[i].id = "liId" + i;
        Apartamento.quantadulto = $("#liId" + i + " input.QuantAdultos").val();
        Apartamento.criancas = Array();
        agechild = $("#" + Quartos[i].id + " .agechild");
        for (var j = 0; j < agechild.length ; j++) {
            Apartamento.criancas.push(agechild[j].value);
        }

        if (Apartamento.quantadulto <= 0 && Apartamento.criancas.length <= 0) {
            validado = false;
            $("#lblErroApartamentos").text("Viagem deve conter passageiros.");
            $("#lblErroApartamentos").css("display", "block");
        }
        else {
            $("#lblErroApartamentos").text("");
            $("#lblErroApartamentos").css("display", "none");
        }
    }

    return validado;
}

function addMP(nomeElemento, tipo) {
    if (tipo == 'multi') {
        var text = $("#multi_" + nomeElemento).next(":text");
        if (text.val() < 9) {
            text.val(parseInt(text.val(), 10) + 1);
        }
    } else {
        var text = $("#" + nomeElemento).next(":text");
        if (text.val() < 9) {
            text.val(parseInt(text.val(), 10) + 1);
        }
    }
}

function removeQtdAdultosMP(nomeElemento, tipo) {
    if (tipo == 'multi') {
        var text = $("#multi_" + nomeElemento).prev(":text");
        if (text.val() > 1) {
            text.val(parseInt(text.val(), 10) - 1);
        }
    } else {
        var text = $("#" + nomeElemento).prev(":text");
        if (text.val() > 1) {
            text.val(parseInt(text.val(), 10) - 1);
        }
    }

}

function qtdDiasMonte() {
    if ($("#Saida").val() != "" && $("#Retorno").val() != "") {

        var SaidaSplit = $("#Saida").val().split("/");
        var checkoutSplit = $("#Retorno").val().split("/");
        SaidaSplit[1] = parseInt(SaidaSplit[1]) - 1;
        checkoutSplit[1] = parseInt(checkoutSplit[1]) - 1;
        var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
        var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);
        var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos
        var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));
        $("#txtNoitesMonte").val(dias);
        $("#hdnNoitesMonte").val(dias);
    }
}

function NovoqtdDiasMonte() {
    if ($("#NovaSaida").val() != "" && $("#NovoRetorno").val() != "") {

        var SaidaSplit = $("#NovaSaida").val().split("/");
        var checkoutSplit = $("#NovoRetorno").val().split("/");
        SaidaSplit[1] = parseInt(SaidaSplit[1]) - 1;
        checkoutSplit[1] = parseInt(checkoutSplit[1]) - 1;
        var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
        var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);
        var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos
        var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));
        $("#NovotxtNoitesMonte").val(dias);
        $("#NovohdnNoitesMonte").val(dias);
    }
}

//function qtCriancas(sender, indice) {
//    var quant = sender.value;

//    $("#criancas_" + indice).html("");

//    if (quant > 0) {
//        for (var i = 0; i < quant; i++) {
//            $.ajax({
//                url: '/Home/Criancas',
//                type: 'POST',
//                data: '',
//                dataType: 'html',
//                contentType: 'application/json',
//                success: function (data) {
//                    $("#roomChilAge_" + indice).show();
//                    $("#criancas_" + indice).append(data);
//                },
//                error: function () {
//                    alert("Problema ao carregar número de crianças.");
//                    window.location = "/Home";
//                }
//            });
//        }
//    }
//    else {
//        $("#roomChilAge_" + indice).hide();
//    }
//}

function noites() {
    Days = parseInt($('#txtNoitesMonte').val());
    Day = $('#Saida').val();
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

    $('#Retorno').val(strDia + '/' + strMes + '/' + ano);
    $("#hdnNoitesMonte").val($('#txtNoitesMonte').val());
}

function NovoNoites() {
    Days = parseInt($('#NovotxtNoitesMonte').val());
    Day = $('#NovaSaida').val();
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

    $('#NovoRetorno').val(strDia + '/' + strMes + '/' + ano);
    $("#NovohdnNoites").val($("#NovotxtNoitesMonte").val());
}

function onlydate(sender) {
    sender.val($('#Retorno').dateFormat('dd/mm/yy'));
    sender.parseDate('dd/mm/yy', $('#Saida').val());
}

function Novoonlydate(sender) {
    sender.val($("#NovoRetorno").dateFormat('dd/MM/yy'));
    sender.parseDate('dd/mm/yy', $('#NovaSaida').val());
}

$(function () {
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    //$.datepicker.parseDate("dd/mm/yy", "26-01-2007");

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
    $('#Retorno').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        onSelect: function (dateText, inst) {
            qtdDiasMonte();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
    $('#Saida').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        minDate: 2,
        onSelect: function (dateText, inst) {
            $('#Retorno').datepicker("option", "minDate", dateText);
            var date2 = $('#Saida').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            var noitesMP = parseInt($("#txtNoitesMonte").val());
            nextDayDate.setDate(nextDayDate.getDate() + noitesMP);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#Retorno').val(nextDayDate);
            qtdDiasMonte();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });

    $('#NovoRetorno').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        onSelect: function (dateText, inst) {
            NovoqtdDiasMonte();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });

    $('#NovaSaida').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        minDate: 0,
        onSelect: function (dateText, inst) {
            $('#NovoRetorno').datepicker("option", "minDate", dateText);
            var date2 = $('#NovaSaida').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            var noitesMP = parseInt($("#NovotxtNoitesMonte").val());
            nextDayDate.setDate(nextDayDate.getDate() + noitesMP);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#NovoRetorno').val(nextDayDate);
            NovoqtdDiasMonte();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});

function AdicionarCarroMontePacote(codigoTarifa) {
    $('.loadingDefault').show();
    $.modal.close();
    var args = new Object();
    args = { codigoTarifa: codigoTarifa };
    $.ajax({
        url: '/MontePacote/AdicionarCarroMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").modal({
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                //$.modal.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
            //$(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
        },
        error: function () {
            alert("Problema ao adicionar.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
            $.modal.close();
        }
    });
}

function RemoverCarroMontePacote(indice) {
    $('.loadingDefault').show();
    var args = new Object();
    args = { indice: indice };
    $.ajax({
        url: '/MontePacote/RemoverCarroMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
            $.modal.close();
        }
    });
}

function AdicionarAtividade(indice) {
    $('.loadingDefault').show();
    var args = new Object();

    args = { indice: indice };

    $.ajax({
        url: '/MontePacote/MPAdicionarAtividade',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").html("");
            $("#divModal").html(data);
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 744,
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                $.modal.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });
            $('.loadingDefault').hide();
            //$(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
        },
        error: function () {
            alert("Problema ao adicionar atividade.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAtividadeTransferMontePacote(codigoAtividade) {
    $('.loadingDefault').show();
    $.modal.close();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, Valor: $('#lbl_' + codigoAtividade).text() + $('#lbl_centavos_' + codigoAtividade).text(), Dia: $('#ddl' + codigoAtividade).find('option:selected').text(), DiaRetorno: $('#ddlRetorno' + codigoAtividade).find('option:selected').text(), entrada: $('#lbl_entrada_' + codigoAtividade).text(), parcelas: $('#lbl_parcelas_' + codigoAtividade).text() };
    $.ajax({
        url: '/MontePacote/AdicionarAtividadeTransferMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("error");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAtividadeAssistenciaMontePacote(codigoAtividade, dataInicial, dataFinal) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, Valor: $('#lbl_' + codigoAtividade).text() + $('#lbl_centavos_' + codigoAtividade).text(), Dia: dataInicial, DiaRetorno: dataFinal, entrada: $('#lbl_entrada_' + codigoAtividade).text(), parcelas: $('#lbl_parcelas_' + codigoAtividade).text() };
    $.ajax({
        url: '/MontePacote/AdicionarAtividadeAssistenciaMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar Assistencia. ");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAtividadeIngressoMontePacote(codigoAtividade) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, Valor: $('#lbl_' + codigoAtividade).text() + $('#lbl_centavos_' + codigoAtividade).text(), Dia: $('#ddl' + codigoAtividade).find('option:selected').text(), DiaRetorno: $('#ddlRetorno' + codigoAtividade).find('option:selected').text(), entrada: $('#lbl_entrada_' + codigoAtividade).text(), parcelas: $('#lbl_parcelas_' + codigoAtividade).text() };
    $.ajax({
        url: '/MontePacote/AdicionarAtividadeIngressoMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar Assistencia. ");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAtividadePasseioMontePacote(codigoAtividade) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, Valor: $('#lbl_' + codigoAtividade).text() + $('#lbl_centavos_' + codigoAtividade).text(), Dia: $('#ddl' + codigoAtividade).find('option:selected').text(), DiaRetorno: $('#ddlRetorno' + codigoAtividade).find('option:selected').text(), entrada: $('#lbl_entrada_' + codigoAtividade).text(), parcelas: $('#lbl_parcelas_' + codigoAtividade).text() };
    $.ajax({
        url: '/MontePacote/AdicionarAtividadePasseioMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar Assistencia. ");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAtividadeRestauranteMontePacote(codigoAtividade) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, Valor: $('#lbl_' + codigoAtividade).text() + $('#lbl_centavos_' + codigoAtividade).text(), Dia: $('#ddl' + codigoAtividade).find('option:selected').text(), DiaRetorno: $('#ddlRetorno' + codigoAtividade).find('option:selected').text(), entrada: $('#lbl_entrada_' + codigoAtividade).text(), parcelas: $('#lbl_parcelas_' + codigoAtividade).text() };
    $.ajax({
        url: '/MontePacote/AdicionarAtividadeRestauranteMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar Assistencia. ");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAtividadePacoteAtividadeMontePacote(codigoAtividade) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, Valor: $('#lbl_' + codigoAtividade).text() + $('#lbl_centavos_' + codigoAtividade).text(), Dia: $('#ddl' + codigoAtividade).find('option:selected').text(), DiaRetorno: $('#ddlRetorno' + codigoAtividade).find('option:selected').text(), entrada: $('#lbl_entrada_' + codigoAtividade).text(), parcelas: $('#lbl_parcelas_' + codigoAtividade).text() };
    $.ajax({
        url: '/MontePacote/AdicionarAtividadePacoteAtividadeMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar Assistencia. ");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarCarrinhoMontePacote(Atividade) {
    $('.loadingDefault').show();
    $.modal.close();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/MontePacote/AdicionarAtividadeMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar no carrinho.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function RemoverTraslado(codigoAtividade, indice, indiceAtividades) {
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, indice: indice, indiceAtividades: indiceAtividades };
    $.ajax({
        url: '/MontePacote/RemoverTraslado',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover traslado.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function RemoverAssistenciaViagem(codigoAtividade, indice, indiceAtividades) {
    $('.loadingDefault').show();
    var args = new Object();
    args = { codigoAtividade: codigoAtividade, indice: indice, indiceAtividades: indiceAtividades };
    $.ajax({
        url: '/MontePacote/RemoverAssistenciaViagem',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover assistencia.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function RemoverPasseio(codigoAtividade, indice, indiceAtividades) {
    $('.loagindDefault').show();

    var args = new Object();
    args = { codigoAtividade: codigoAtividade, indice: indice, indiceAtividades: indiceAtividades };

    $.ajax({
        url: '/MontePacote/RemoverPasseio',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover passeio.");
            $('.loadingDefault').hide();
        }
    });
}

function RemoverIngresso(codigoAtividade, indice, indiceAtividades) {
    $('.loadingDefault').show();

    var args = new Object();
    args = { codigoAtividade: codigoAtividade, indice: indice, indiceAtividades: indiceAtividades };

    $.ajax({
        url: '/MontePacote/RemoverIngresso',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover ingresso.");
            $('.loadingDefault').hide();
        }
    });
}

function RemoverPacoteAtividade(codigoAtividade, indice, indiceAtividades) {
    $('.loadingDefault').show();

    var args = new Object();
    args = { codigoAtividade: codigoAtividade, indice: indice, indiceAtividades: indiceAtividades };

    $.ajax({
        url: '/MontePacote/RemoverIngresso',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover pacote atividade.");
            $('.loadingDefault').hide();
        }
    });
}

function RemoverRestaurante(codigoAtividade, indice, indiceAtividades) {
    $('.loadingDefault').show();

    var args = new Object();
    args = { codigoAtividade: codigoAtividade, indice: indice, indiceAtividades: indiceAtividades };

    $.ajax({
        url: '/MontePacote/RemoverRestaurante',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover restaurante.");
            $('.loadingDefault').hide();
        }
    });
}

function VerificarHorarioHotel(indice, saida, retorno) {
    $('.loadingDefault').show();
    var args = new Object();
    args = { indice: indice };
    var verificacaoCheckIn = false;
    $.ajax({
        url: '/MontePacote/VerificarHorarioHotel',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data) {
                $("#hdnIndiceSelecionado").val(indice);
                $("#NovaSaida").val(saida);
                $("#NovoRetorno").val(retorno);
                NovoqtdDiasMonte();
                $('.loadingDefault').hide();
                $("#divModalAdicionarNoiteHotel").modal({
                    minHeight: 230,
                    minWidth: 744,
                    onClose: function (dialog) {
                        dialog.data.fadeOut('fast', function () {
                            dialog.container.slideUp('fast', function () {
                                dialog.overlay.fadeOut('fast', function () {
                                    $.modal.close(); // must call this!
                                });
                            });
                        });
                    }
                });
            }
            else {
                $.modal.close();
                AdicionarHotel(indice);
            }
        }
    });
}

function VerificarHorariosHotel(indice, saida, retorno) {
    var args = new Object();
    args = { indice: indice };
    $("#hdnIndiceSelecionado").val(indice);

    //Verifica se vai sugerir uma nova data de CheckIn
    $.ajax({
        url: '/MontePacote/VerificarHorarioCheckinHotel',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data) {
                $('.loadingDefault').hide();
                //$.ajax({
                //    url: '/MontePacote/Filtrohotel',
                //    type: 'POST',
                //    contentType: 'application/json',
                //    success: function (data) {
                //        $("#FiltroMP").html("");
                //        $("#FiltroMP").html(data);
                //        $('#FiltroMP').show();
                //    },
                //    error: function (data) {
                //        alert("Problema ao adicionar Hotel.");
                //    }
                //});
                $("#hdnNovaDataCheckIn").val("true");

                $("#divModalAdicionarNoiteHotel").modal({
                    minHeight: 230,
                    minWidth: 744,
                    onClose: function (dialog) {
                        dialog.data.fadeOut('fast', function () {
                            dialog.container.slideUp('fast', function () {
                                dialog.overlay.fadeOut('fast', function () {
                                    $.modal.close();
                                });
                            });
                        });
                    }
                });

                $("#divPerguntaCheckIn").css('display', '');
                $("#divPerguntaCheckOut").css('display', 'none');
            }
            else {
                $.ajax({
                    url: '/MontePacote/VerificarHorarioCheckoutHotel',
                    type: 'POST',
                    data: JSON.stringify(args),
                    dataType: 'json',
                    contentType: 'application/json',
                    success: function (data) {
                        if (data) {
                            $("#hdnNovaDataCheckOut").val("true");

                            $("#divModalAdicionarNoiteHotel").modal({
                                minHeight: 230,
                                minWidth: 744,
                                onClose: function (dialog) {
                                    dialog.data.fadeOut('fast', function () {
                                        dialog.container.slideUp('fast', function () {
                                            dialog.overlay.fadeOut('fast', function () {
                                                $.modal.close();
                                            });
                                        });
                                    });
                                }
                            });


                            $("#divPerguntaCheckOut").css('display', '');
                            $("#divPerguntaCheckIn").css('display', 'none');
                        }
                        else {
                            var ars = new Object();
                            $.modal.close();
                            $(".loading").show();

                            var criancas = Array();
                            var ageChild = Array();

                            var Apartamentos = Array();
                            var Quartos = $(".listaQuartosMP li");

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

                            args = { indice: indice, origemMP: JSON.stringify(Apartamentos) };

                            $('.loadingDefault').show();

                            $.ajax({
                                url: '/MontePacote/AdicionarHotelDatasAlteradas',
                                type: 'POST',
                                data: JSON.stringify(args),
                                dataType: 'html',
                                contentType: 'application/json',
                                success: function (data) {
                                    $("#divModal").append("<div class='content-hotel'>" + data + "</div>");
                                    $("#divModal").modal({
                                        minHeight: 707,
                                        minWidth: 960,
                                        onClose: function (dialog) {
                                            dialog.data.fadeOut('fast', function () {
                                                dialog.container.slideUp('fast', function () {
                                                    dialog.overlay.fadeOut('fast', function () {
                                                        $.modal.close(); // must call this!
                                                        $("#divModal").html("");
                                                        $("#divModal").append("<div id='FiltroMP'>  </div>");
                                                        $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                                                    });
                                                });
                                            });
                                        }
                                    });

                                    $(window).trigger('resize.simplemodal');
                                    $('.loadingDefault').hide();
                                    $.ajax({
                                        url: '/MontePacote/Filtrohotel',
                                        type: 'POST',
                                        contentType: 'application/json',
                                        success: function (data) {
                                            $("#FiltroMP").html("");
                                            $("#FiltroMP").html(data);
                                            $('#FiltroMP').show();
                                        },
                                        error: function (data) {
                                            alert("Problema ao adicionar Hotel.");
                                        }
                                    });
                                    //$(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
                                },
                                error: function (data) {
                                    alert("Problema ao adicionar Hotel.");
                                    //window.location = "/Home";
                                    $('.loadingDefault').hide();
                                }
                            });
                        }
                    }
                    ,

                    error: function (data) {
                        alert("Problema ao adicionar Hotel.");
                        //window.location = "/Home";
                        $('.loadingDefault').hide();
                    }
                });
            }
        },

        error: function (data) {
            alert("Problema ao adicionar Hotel.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }


    });
}

function AdicionarDiaCheckin() {
    var indice = $("#hdnIndiceSelecionado").val();
    $('.loadingDefault').show();
    $('.btnSim').prop('disabled', 'true');
    $('.btnNao').prop('disabled', 'true');
    var args = new Object();
    args = { indice: indice };

    $.ajax({
        url: '/MontePacote/AdicionarDiaCheckin',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {

        }
    });

    $.ajax({
        url: '/MontePacote/VerificarHorarioCheckoutHotel',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (data) {
                $("#hdnNovaDataCheckOut").val("true");

                $("#divPerguntaCheckIn").css('display', 'none');
                $("#divPerguntaCheckOut").css('display', 'block');

                $("#simplemodal-container").css('height', '285');
                $("#simplemodal-container").css('width', '744');
                $("#simplemodal-container").css('left', '250');
                $('.btnSim').prop('disabled', '');
                $('.btnNao').prop('disabled', '');
            }
            else {
                var ars = new Object();
                $.modal.close();

                var criancas = Array();
                var ageChild = Array();

                var Apartamentos = Array();
                var Quartos = $(".listaQuartosMP li");

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

                args = { indice: indice, origemMP: JSON.stringify(Apartamentos) };
                $('.loadingDefault').show();

                $.ajax({
                    url: '/MontePacote/AdicionarHotelDatasAlteradas',
                    type: 'POST',
                    data: JSON.stringify(args),
                    dataType: 'html',
                    contentType: 'application/json',
                    success: function (data) {
                        $("#divModal").append("<div class='content-hotel'>" + data + "</div>");
                        $("#divModal").modal({
                            minHeight: 707,
                            minWidth: 960,
                            onClose: function (dialog) {
                                dialog.data.fadeOut('fast', function () {
                                    dialog.container.slideUp('fast', function () {
                                        dialog.overlay.fadeOut('fast', function () {
                                            $.modal.close(); // must call this!
                                            $("#divModal").html("");
                                            $("#divModal").append("<div id='FiltroMP'>  </div>");
                                            $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                                        });
                                    });
                                });
                            }
                        });


                        $(window).trigger('resize.simplemodal');
                        $('.loadingDefault').hide();
                        $.ajax({
                            url: '/MontePacote/Filtrohotel',
                            type: 'POST',
                            contentType: 'application/json',
                            success: function (data) {
                                $("#FiltroMP").html("");
                                $("#FiltroMP").html(data);
                                $('#FiltroMP').show();
                            },
                            error: function (data) {
                                alert("Problema ao adicionar Hotel.");
                            }
                        });
                        //$(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
                    },
                    error: function (data) {
                        alert("Problema ao adicionar Hotel.");
                        //window.location = "/Home";
                        $('.loadingDefault').hide();
                    }
                });
            }
        }
    });
}

function AdicionarDiaCheckout() {
    var indice = $("#hdnIndiceSelecionado").val();
    $('.btnSim').prop('disabled', 'true');
    $('.btnNao').prop('disabled', 'true');
    var args = new Object();
    args = { indice: indice };
    $.modal.close();
    $.ajax({
        url: '/MontePacote/AdicionarDiaCheckout',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {

            $(".loading").show();

            var criancas = Array();
            var ageChild = Array();

            var Apartamentos = Array();
            var Quartos = $(".listaQuartosMP li");

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

            args = { indice: indice, origemMP: JSON.stringify(Apartamentos) };
        }
    });

    $('.loadingDefault').show();

    $.ajax({
        url: '/MontePacote/AdicionarHotelDatasAlteradas',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {

            $("#divModal").append("<div class='content-hotel'>" + data + "</div>");
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 960,
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                $.modal.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });

            $(window).trigger('resize.simplemodal');
            $('.loadingDefault').hide();
            $.ajax({
                url: '/MontePacote/Filtrohotel',
                type: 'POST',
                contentType: 'application/json',
                success: function (data) {
                    $("#FiltroMP").html("");
                    $("#FiltroMP").html(data);
                    $('#FiltroMP').show();
                },
                error: function (data) {
                    alert("Problema ao adicionar Hotel.");
                }
            });
            //$(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
        },
        error: function (data) {
            alert("Problema ao adicionar Hotel.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function ShowNewDates() {
    $("#botoesSimENao").css("display", "none");
    $("#divNovasDatasCheckinCheckOutHotel").css("display", "block");
}

function NaoAlterarDatas() {
    $.modal.close();
    AdicionarHotel($("#hdnIndiceSelecionado").val());
}

function AdicionarHotel(indice, saida, retorno) {
    $('.loadingDefault').hide();
    $('.loadingDefault').show();

    $('#FiltroMP').show();

    $("#FiltroMP").html("");
    var criancas = Array();
    var agechild = Array();

    var Apartamentos = Array();
    var Quartos = $(".listaQuartosMP li");

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

    var args = new Object();
    args = { indice: indice, origemMP: JSON.stringify(Apartamentos), dataSaida: saida, dataChegada: retorno };
    $.ajax({
        url: '/MontePacote/AdicionarHotel',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").append("<div class='content-hotel'>" + data + "</div>");
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 960,
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                $.dialog.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });

            //$("#simplemodal-container").css('height', '707');
            //$("#simplemodal-container").css('width', '960');
            $(window).trigger('resize.simplemodal');
            $('.loadingDefault').hide();
            // $(".simplemodal-container").height(707); $(".simplemodal-container").width(960);

            $.ajax({
                url: '/MontePacote/Filtrohotel',
                type: 'POST',
                contentType: 'application/json',
                success: function (data) {
                    $("#FiltroMP").html("");
                    $("#FiltroMP").html(data);
                    $('#FiltroMP').show();
                },
                error: function (data) {
                    alert("Problema ao adicionar Hotel.");
                }
            });


        },
        error: function (data) {
            alert("Problema ao adicionar Hotel.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarHotelMontePacote(Hotel, Acomodacao, Tarifa) {
    $('.loadingH').show();
    $.modal.close();
    var args = new Object();
    args = {
        montePacoteItem: Hotel,
        NomeAcomodacao: Acomodacao,
        CodigoTarifa: Tarifa
    };
    $.ajax({
        url: '../../MontePacote/AdicionarHotelMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingH').hide();
        },
        error: function (data) {
            alert("Problema ao adicionar Hotel.");
            //window.location = "/Home";
            $('.loadingH').hide();
        }
    });
}

function RemoverHotelMontePacote(Tarifa, Indice) {
    $('.loadingDefault').show();
    var args = new Object();
    args = {
        CodigoTarifa: Tarifa,
        Indice: Indice
    };
    $.ajax({
        url: '../../MontePacote/RemoverHotelMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/MontePacote/ReloadIndex";
            $('.loadingDefault').hide();
        },
        error: function (data) {
            alert("Problema ao remover Hotel.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAereo(Indice) {
    $('.loadingDefault').show();
    var args = new Object();
    var todosCheckBoxIdaVolta = $(".radioIdaVolta");
    var isIdaVolta = todosCheckBoxIdaVolta[Indice].checked;

    args = { indiceDestino: Indice, isIdaVolta: isIdaVolta };
    $('#FiltroMP').show();
    $("#FiltroMP").html("");
    $.ajax({
        url: '/MontePacote/AdicionarAereo',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").append(data);
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 960,
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                $.modal.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });

            // $("#simplemodal-container").css('height', '707');
            // $("#simplemodal-container").css('width', '960');
            $(window).trigger('resize.simplemodal');
            //  $(".simplemodal-container").height(707); $(".simplemodal-container").width(960);
            $('.loadingDefault').hide();

            $.ajax({
                url: '/MontePacote/FiltroAereo',
                type: 'POST',
                contentType: 'application/json',
                success: function (data) {
                    $("#FiltroMP").html("");
                    $('#FiltroMP').show();
                    $("#FiltroMP").html(data);
                },
                error: function (data) {
                    alert("Problema ao adicionar Aereo.");
                }
            });

        },
        error: function () {
            alert("Problema ao adicionar passagem.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function AdicionarAereoMultiTrecho() {
    $('.loadingDefault').show();
    var checkboxTrechos = $(".radioTrechoSelecionado");
    var checkboxIdaVolta = $(".radioIdaVolta");
    var args = new Object();

    var isChecked = new Array();

    for (var i = 0; i < checkboxTrechos.length; i++) {
        isChecked.push(checkboxTrechos[i].checked + "|" + checkboxIdaVolta[i].checked);
    }

    args = { trechos: JSON.stringify(isChecked) }
    $('#FiltroMP').show();
    $("#FiltroMP").html("");

    $.ajax({
        url: '/MontePacote/AdicionarAereoMultiTrecho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").html(data);
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 1005,
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                $.modal.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });
            $('#loadAereoForm').css('display', 'none');
            $(window).trigger('resize.simplemodal');
            $('.loadingDefault').hide();
            $.ajax({
                url: '/MontePacote/FiltroAereo',
                type: 'POST',
                contentType: 'application/json',
                success: function (data) {
                    $("#FiltroMP").html("");
                    $('#FiltroMP').show();
                    $("#FiltroMP").html(data);
                    $('#DivPai #buscas').css('display', 'block');
                },
                error: function (data) {
                    alert("Problema ao adicionar Aereo.");
                }
            });
        },
        error: function () {
            alert("Problema ao adicionar passagem.");
            //window.location = "/Home";
            // $(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
            $('.loadingDefault').hide();
        }
    });

}

function AdicionarAereoMontePacote(Grupo, tarifa, tipoTarifa) {
    var myVoos = new Array();

    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;
    if (tipoTarifa == "IdaeVolta") {
        if (selected.length != 2) {
            var valido = true;
        }
    }
    else if (tipoTarifa == "SomenteIda") {
        if (selected.length != 1) {
            var valido = true;
        }
    }
    else if (tipoTarifa == "MultiTrecho") {
        if (selected.length < 1) {
            var valido = true;
        }
    }
    if (valido) {
        alert('Selecione o vôo desejado.');
    }
    else {
        $.modal.close();
        $('.loadingDefault').show();
        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.Tarifas = null;
        tarifaSel.TrechosSel = myVoos;

        var args = new Object();
        args = { AereoModel: tarifaSel };
        $.ajax({
            url: '../../MontePacote/AdicionarAereoMontePacote',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                window.location = '/MontePacote/ReloadIndex';
                $('.loadingDefault').hide();
            },
            error: function () {
                alert("Problema ao adicionar passagem.");
                //window.location = "/Home";
                $('.loadingDefault').hide();
            }
        });
    }
}

function AdicionarAereoMontePacoteMulti(Grupo, tarifa, tipoTarifa, destinos) {
    var myVoos = new Array();

    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;

    if (selected.length < destinos) {
        var valido = true;
    }

    if (valido) {
        alert('Selecione o vôo desejado.');
    }
    else {
        $.modal.close();
        $('.loadingDefault').show();
        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.Tarifas = null;
        tarifaSel.TrechosSel = myVoos;

        var args = new Object();
        args = { AereoModel: tarifaSel };
        $.ajax({
            url: '../../MontePacote/AdicionarAereoMontePacote',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                window.location = '/MontePacote/ReloadIndex';
                $('.loadingDefault').hide();
            },
            error: function () {
                alert("Problema ao adicionar passagem.");
                //window.location = "/Home";
                $('.loadingDefault').hide();
            }
        });
    }
}

function RemoverAereoMontePacote() {
    $('.loadingDefault').show();
    $.ajax({
        url: '../../MontePacote/RemoverAereoMontePacote',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = '/MontePacote/ReloadIndex';
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao remover passagem.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function RemoverDestino(indice, counter) {
    var args = new Object();
    args = { indice: indice };
    $.ajax({
        url: '../../MontePacote/RemoverDestino',
        type: 'POST',
        beforeSend: function () { showdivLoad(); },
        complete: function () { hidedivLoad(); },
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $(".item_" + counter).fadeOut({
                duration: 1000,
                always: function () { window.location = '/MontePacote/ReloadIndex'; }
            });
        },
        error: function () {
            alert("Problema ao remover Destino.");
            //window.location = "/Home";

        }
    });
}

function VerificarIdaVolta(indice) {

    var todosCheckBoxIdaVolta = $(".radioIdaVolta");
    var todosCheckBoxIda = $(".radioIda");
    var todosCheckBoxTrecho = $(".radioTrechoSelecionado");

    for (var i = 0; i < todosCheckBoxIdaVolta.length; i++) {
        if (i != indice) {
            if (todosCheckBoxIdaVolta[indice].checked == true) {

                todosCheckBoxIdaVolta[i].checked = false;
                $("#" + todosCheckBoxIdaVolta[i].id).prop("disabled", true);

                for (var j = 0; j < todosCheckBoxIda.length; j++) {
                    todosCheckBoxIda[j].checked = false;
                    //$("#" + todosCheckBoxIda[j].id).prop("disabled", true);
                }

                todosCheckBoxTrecho[i].checked = false;
                //$("#" + todosCheckBoxTrecho[i].id).prop("disabled", true);
            }
            else {
                todosCheckBoxIda[i].checked = true;
                $("#" + todosCheckBoxIda[i].id).removeAttr("disabled");
                $("#" + todosCheckBoxIdaVolta[i].id).removeAttr("disabled");
                //$("#" + todosCheckBoxTrecho[i].id).removeAttr("disabled");
                //todosCheckBoxTrecho[i].checked = true;
            }
        }
        else {
            if (todosCheckBoxIdaVolta[i].checked == false) {
                todosCheckBoxIda[i].checked = true;
                $("#" + todosCheckBoxIda[i].id).removeAttr("disabled");
                //$("#" + todosCheckBoxTrecho[i].id).removeAttr("disabled");
            }
            else {
                //$("#" + todosCheckBoxTrecho[i].id).prop("disabled", true);
                todosCheckBoxIda[i].checked = false;
            }
        }
    }
}

function finalizarMontePacote(totalPacote) {

    var total = totalPacote;
    if (total <= 0) {
        alert('selecionar um produto');
        return false;
    }
    else {
        $('.loadingDefault').show();
        $.ajax({
            url: '../../Carrinho/AdicionarMontePacoteCarrinho',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                window.location = '/Reserva';
                $('.loadingDefault').hide();
            },
            error: function () {
                alert("Problema ao finalizar o monte seu pacote");
                //window.location = "/Home";
                $('.loadingDefault').hide();
            }
        });
    }
}

function OrcamentoMontePacote(totalPacote) {
    var total = totalPacote;
    if (total <= 0) {
        alert('selecionar um produto');
        return false;
    }
    else {
        var Nome = $("#txtorcamento").val();

        if (Nome == null || Nome == "" || Nome == " ") {
            alert("Informe o nome do orçamento.");
        }
        else {
            $('.loadingDefault').show();
            $.ajax({
                url: '../Orcamento/AdicionarMontePacoteOrcamento',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    salvaOrcamentoMP();
                },
                error: function () {
                    alert("Problema ao finalizar o monte seu pacote");
                    $('.loadingDefault').hide();
                }
            });
        }
    }
}

function salvaOrcamentoMP() {
    $.ajax({
        url: '/Orcamento/SalvarOrcamento',
        type: 'POST',
        data: { NomeOrcamento: $("#txtorcamento").val() },
        dataType: 'html',
        success: function (data) {
            if (data) {
                $.modal.close();
                alert("Orçamento salvo com sucesso.");
                window.location = "/Orcamento/EditarOrcamento?code=" + data;
                $('.loadingDefault').hide();
            } else { alert("Erro ao salvar o orçamento."); $('.loadingDefault').hide(); }
        },
        error: function () {
            alert("Erro ao salvar o orçamento");
            $('.loadingDefault').hide();
        }
    });
}

function TratarSomenteIda(indice) {
    var todosCheckBoxIdaVolta = $(".radioIdaVolta");
    var todosCheckBoxIda = $(".radioIda");
    var todosCheckBoxTrecho = $(".radioTrechoSelecionado");

    if (todosCheckBoxIda[indice].checked == true) {
        todosCheckBoxIdaVolta[indice].checked = false;
        todosCheckBoxTrecho[indice].checked = true;
    }
    else {
        todosCheckBoxIdaVolta[indice].checked = true;
        todosCheckBoxTrecho[indice].checked = true;
    }
}

$(document).ready(function () {
    autoComplete(6, "DestinoHotel");

    if ($("#hdnNoitesRequest").val() == "")
    { $("#txtNoites").val(1); }
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
                //window.location = "/Home";
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

    $('#txtNoites').change(function () {
        noitesHotel();
    });
});

function add(nomeElemento) {
    var text = $("#" + nomeElemento).next(":text");
    text.val(parseInt(text.val(), 10) + 1);
}

function removeQtdAdultos(nomeElemento) {
    var text = $("#" + nomeElemento).prev(":text");
    if (text.val() != 1) {
        text.val(parseInt(text.val(), 10) - 1);
    }
}

function qtdDias() {
    if ($("#Checkin").val() != "" && $("#Checkout").val() != "") {

        var checkinSplit = $("#Checkin").val().split("/");
        var checkoutSplit = $("#Checkout").val().split("/");

        if (SaidaSplit[1] == "01") {
            SaidaSplit[1] == "0";
        }
        if (checkoutSplit[1] == "01") {
            checkoutSplit[1] == "00";
        }



        var dataCheckinFormatada = new Date(checkinSplit[2], checkinSplit[1], checkinSplit[0]);
        var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

        var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos

        var dias = Math.round(Math.abs((dataCheckinFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));

        $("#txtNoites").val(dias);
        $("#hdnNoites").val(dias);
    }
}

function qtCriancas(sender, indice, tipo) {
    var quant = sender.value;

    if (tipo == 'multi') {
        $("#multi_criancas_" + indice).html("");
    } else {
        $("#criancas_" + indice).html("");
    }


    if (quant > 0) {
        for (var i = 0; i < quant; i++) {
            $.ajax({
                url: '/Home/Criancas',
                type: 'POST',
                data: '',
                dataType: 'html',
                contentType: 'application/json',
                success: function (data) {
                    if (tipo == 'multi') {
                        $("#multi_roomChilAge_" + indice).show();
                        $("#multi_criancas_" + indice).append(data);
                    } else {
                        $("#roomChilAge_" + indice).show();
                        $("#criancas_" + indice).append(data);
                    }

                },
                error: function () {
                    alert("Erro ao carregar número de crianças.");
                    //window.location = "/Home";
                }
            });
        }
    }
    else {
        if (tipo == 'multi') {
            $("#multi_roomChilAge_" + indice).hide();
        } else {
            $("#roomChilAge_" + indice).hide();
        }

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
    var args = new Object();
    args = fotos;
    $.ajax({
        url: '/Hotel/HotelGallery',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $.fancybox.open(data, {
                padding: 0
            });
        },
        error: function () {
            alert("Problema ao carregar fotos do hotel. ");
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

    try { $(".hoteisMapa").CloseModalPopup(); } catch (ex) { }

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
        },
        error: function () {
            alert("Problema ao exibir detalhes do Hotel. ");
        }
    });
}

function Registrar(Hotel, Acomodacao, Tarifa) {
    $('.loadingDefault').fadeIn();
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
            window.location = "/Reserva";
        },
        error: function () {
            alert("Problema ao Registrar hotel.");
            //window.location = "/Home";
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
        url: '../../Orcamento/AdicionarHotelOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $('html, body').animate({ scrollTop: 0 }, 'fast');
            ShowOrcamento(data);
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
        url: '../../Orcamento/ShowOrcamento',
        type: 'POST',
        data: { SessionKey: SessionKey },
        dataType: 'html',
        success: function (data) {
            $("#orcamentocontent").append(data);
            $('.loadingDefault').hide();
        },
        error: function (data) {
            alert("Problema ao exibir o orçamento.");
            alert(data);
        }
    });
}

function AdicionarHotelNovaData() {
    var indice = $("#hdnIndiceSelecionado").val();
    var dataSaida = $("#NovaSaida").val();
    var dataChegada = $("#NovoRetorno").val();

    var ars = new Object();
    $.modal.close();
    $(".loading").show();

    var criancas = Array();
    var ageChild = Array();

    var Apartamentos = Array();
    var Quartos = $(".listaQuartosMP li");

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

    args = { indice: indice, origemMP: JSON.stringify(Apartamentos), dataSaida: dataSaida, dataChegada: dataChegada };
    $.ajax({
        url: '/MontePacote/AdicionarHotel',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").html(data);
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 744,
                onClose: function (dialog) {
                    dialog.data.fadeOut('fast', function () {
                        dialog.container.slideUp('fast', function () {
                            dialog.overlay.fadeOut('fast', function () {
                                $.modal.close(); // must call this!
                                $("#divModal").html("");
                                $("#divModal").append("<div id='FiltroMP'>  </div>");
                                $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                            });
                        });
                    });
                }
            });

            // $("#simplemodal-container").css('height', '707');
            //$("#simplemodal-container").css('width', '744');
            $(window).trigger('resize.simplemodal');
            //$('.loadingDefault').hide();
            //$(".simplemodal-container").height(707); $(".simplemodal-container").width(744);
        },
        error: function (data) {
            alert("Problema ao adicionar Hotel.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
        }
    });
}

function postarformMP() {
    $('.watermark').watermark('clearWatermarks');
    $('.loadingMP').show();

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
        url: '/MontePacote/FiltrarHotel',
        type: 'POST',
        data: args,
        success: function (result) {
            $(".content-hotel").html("");
            $(".content-hotel").append(result);
            $('.loadingMP').hide();

        },
        error: function (result) {
            $('.loadingMP').hide();
        }
    });
}

function AdicionarCarro(indiceDestino) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();

    var horarioSaidaSelecionado = $("#ddlHoraRetirada_" + indiceDestino).val();
    var horarioRetornoSelecionado = $("#ddlHoraDevolucao_" + indiceDestino).val();

    var localDevolucao = $("#combod_" + indiceDestino + " option:selected").text();
    var codigoLocalDevolucao = $("#combod_" + indiceDestino + " option:selected").val();

    var localRetirada = $("#combo_" + indiceDestino + " option:selected").text();
    var codigoLocalRetirada = $("#combo_" + indiceDestino + " option:selected").val();

    var dataSaida = $("#DataEmbarqueCodigoCmpMP_" + indiceDestino).val();
    var dataRetorno = $("#DataRetornoCmpMP_" + indiceDestino).val();
    args = {
        indiceDestino: indiceDestino,
        horarioSaidaSelecionado: horarioSaidaSelecionado,
        horarioRetornoSelecionado: horarioRetornoSelecionado,
        localDevolucao: localDevolucao,
        localRetirada: localRetirada,
        codigoLocalRetirada: codigoLocalRetirada,
        codigoLocalDevolucao: codigoLocalDevolucao,
        dataSaida: dataSaida,
        dataRetorno: dataRetorno
    };

    $.ajax({
        url: '/MontePacote/MPAdicionarCarro',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {

            $("#divModal").html(data);
            $("#divModal").modal({
                minHeight: 707,
                minWidth: 710,
                onClose: function (dialog) {
                    $.modal.close();
                    $("#divModal").html("");
                    $("#divModal").append("<div id='FiltroMP'>  </div>");
                    $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                }
            });
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao adicionar no carrinho.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
            $.modal.close();
        }
    });
}

function verDetalhesCarro(carro, isMontePacote) {
    $.modal.close();
    $('.loadingDefault').show();
    var args = new Object();
    args = { carro: carro, isMontePacote: isMontePacote };
    $.ajax({
        url: '/Carro/ReservaCarroMontePacote',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#divModal").html(data);
            $("#divModal").modal({
                minHeight: 758,
                minWidth: 724,
                onClose: function (dialog) {
                    $.modal.close();
                    $("#divModal").html("");
                    $("#divModal").append("<div id='FiltroMP'>  </div>");
                    $("#divModal").append("<div class='loadingMP' style='display: none;'><img src='../Images/Loading_2.gif' /></div>");
                }
            });
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao exibir detalhes.");
            //window.location = "/Home";
            $('.loadingDefault').hide();
            $.modal.close();
        }
    });
}

function callCar(item) {
    AdicionarCarro(item);
}

function changeNightsKeyMP(valor) {
    if (parseInt(valor) > 0) {
        noites();
    }
}