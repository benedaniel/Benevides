﻿$(document).ready(function () {
    //$.mask.definitions['~']='[+-]';
    $(".date-mask").mask("99/99/9999");
    $(".txtStartDate").mask("99/99/9999");
    $(".txtEndDate").mask("99/99/9999");

    $(".hasDatepicker").mask("99/99/9999");


    $(".phone-number-mask").mask("(99) 9999-9999");
    $(".cellphone-number-mask").mask("(99) 9 9999-9999");
});
function Registrar(Grupo, tarifa, comprar, tarifaOpe) {

    var myVoos = new Array();

    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;
    if (tarifa.Tipo == "IdaeVolta") {
        if (selected.length != 2) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "SomenteIda") {
        if (selected.length != 1) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "MultiTrecho") {
        if (selected.length < 1) {
            var valido = true;
        }
    }
    if (valido) {
        alert('Selecione o Horário desejado!');
    }
    else {
        $('.loadingDefault').fadeIn();
        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.Tarifas = null;
        tarifaSel.TrechosSel = myVoos;
        tarifaSel.Comprar = comprar;
        if (tarifaOpe == null || tarifaOpe == "") {
            tarifaSel.TarifaOperadora = false;
        } else {
            tarifaSel.TarifaOperadora = true;
        }

        var args = new Object();
        args = tarifaSel;
        $.ajax({
            url: '../../Carrinho/AdicionarAereoCarrinho',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                if (data != "") {
                    alert(data);
                    $('.loadingDefault').fadeOut();
                    $('#loadAereoForm').submit();
                }
                else {
                    if (comprar) {
                        window.location = "/Reserva";
                    }
                    else {
                        AtualizarCarrinho();
                        $('.loadingDefault').fadeOut();
                    }
                }
            },
            error: function () {
                alert("Erro ao Adicionar passagem ao carrinho.");
                $('.loadingDefault').fadeOut();
            }
        });
    }
}
function RegistrarMontePacoteTipoPacote1(tarifa) {
    var myVoos = new Array();

    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;
    if (tarifa.Tipo == "IdaeVolta") {
        if (selected.length != 2) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "SomenteIda") {
        if (selected.length != 1) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "MultiTrecho") {
        if (selected.length < 1) {
            var valido = true;
        }
    }
    if (valido) {
        alert('Selecione o Horário desejado!');
    }
    else {
        $('.loadingDefault').fadeIn();
        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.Tarifas = null;
        tarifaSel.TrechosSel = myVoos;

        var args = new Object();
        args = tarifaSel;
        $.ajax({
            url: '../../Carrinho/AdicionarAereoCarrinho',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                window.location = "/MontePacote/MPOpcionais";
            },
            error: function () {
                alert("Erro ao adicionar passagem ao carrinho");
                $('.loadingDefault').fadeOut();
            }
        });
    }
}
function RegistrarMontePacoteTipoPacote34(tarifa) {
    var myVoos = new Array();

    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;
    if (tarifa.Tipo == "IdaeVolta") {
        if (selected.length != 2) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "SomenteIda") {
        if (selected.length != 1) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "MultiTrecho") {
        if (selected.length < 1) {
            var valido = true;
        }
    }
    if (valido) {
        alert('Selecione o Horário desejado!');
    }
    else {
        $('.loadingDefault').fadeIn();
        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.Tarifas = null;
        tarifaSel.TrechosSel = myVoos;

        var args = new Object();
        args = tarifaSel;
        $.ajax({
            url: '../../Carrinho/AdicionarAereoCarrinho',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                window.location = "/MontePacote/MPCarro";
            },
            error: function () {
                alert("Erro ao adicionar passagem ao carrinho.");
                $('.loadingDefault').fadeOut();
            }
        });
    }
}
function AdicionarOrcamento(Grupo, tarifa) {
    var myVoos = new Array();


    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;
    if (tarifa.Tipo == "IdaeVolta") {
        if (selected.length != 2) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "SomenteIda") {
        if (selected.length != 1) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "MultiTrecho") {
        if (selected.length < 1) {
            var valido = true;
        }
    }
    if (valido) {
        alert('Selecione o Horário desejado!');
    }
    else {
        $('.loadingDefault').fadeIn();

        var myVoos = new Array();

        var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
        if (selected.length > 0) {
            for (var i = 0; i < selected.length; i++) {
                myVoos[i] = selected[i].value;
            }
        }

        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.tarifaSel = null;
        tarifaSel.TrechosSel = myVoos;
        
        var args = new Object();
        args = tarifaSel;
        $.ajax({
            url: '../../Orcamento/AdicionarAereoOrcamento',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                $('html, body').animate({ scrollTop: 0 }, 'fast');
                ShowOrcamento(data);
            },
            error: function () {
                alert("Erro ao adicioanar passagem ao orçamento.");
                $('.loadingDefault').fadeOut();
            }
        });
    }
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
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Problema ao exibir o orçamento.");
            $('.loadingDefault').fadeOut();
        }
    });
}
function showGrid() {
    $('#slidesCompanias').slidesjs({
        width: 500,
        height: 310,
        play: {
            active: false,
            effect: "slide",
            interval: 20000,
            auto: false,
            swap: true,
            pauseOnHover: true
        },
        pagination: {
            active: false,
            effect: "slide"
        },
        navigation: {
            active: false,
            effect: "slide"
        }
    });
};
function VerVooAereo(divs) {
    $("#" + divs).modal();
    $("#" + divs).css('display', 'block');
}
function SendToServer() {
    var quant = $("#QuantCriancas").val();
    var criancas = "";



    for (var i = 0; i < quant; i++) {
        criancas[i] = $(".criancas").children().eq(i).eval();
        if (i != quant - 1)
            criancas[i] += ",";
    }

    $("#origem").val(criancas);
}
function AereoLoadingHide() {
    $('.loadingDefault').fadeOut();
}

function ValidarDatasMultiTrecho() {

    // 19051983
    if ($("#radio3").attr("checked") == 'checked') {
        var quant = $("#trechosAdicionais li").length + 1;
        if (quant >= 2) {

            var dtEmbarque = $("#DataEmbarque").val();
            if (dtEmbarque != null && dtEmbarque != '')
                dtEmbarque = FormatadaDatas(dtEmbarque);

            for (i = 0; i < quant; i++) {
                var dtEmbarqueProx = $("#DataEmbarque-" + (i + 2)).val();
                if (dtEmbarqueProx != null && dtEmbarqueProx != '') {

                    dtEmbarqueProx = FormatadaDatas(dtEmbarqueProx);

                    if (dtEmbarqueProx < dtEmbarque) {
                        alert('A data de embarque do Trecho ' + (i + 2) + ' deve ser maior ou igual a data de embarque do Trecho ' + (i + 1));
                        return false;
                    }
                }
                dtEmbarque = dtEmbarqueProx;
            }
        }
    }
    return true;
}

function AereoLoadingShow() {

    if ($('#hdnOrigemAereoCodigo').val() == "") {
        $('.loadingDefault').hide();
        alert('Você deve selecionar uma origem válida.');
        return false;
    }

    if ($('#hdnDestinoAereoCodigo').val() == "") {
        $('.loadingDefault').hide();
        alert('Você deve selecionar um destino válido.');
        return false;
    }


    if (FormatadaDatas($("#DataEmbarque").val()) < FormatadaDatas(getToday())) {
        alert('Favor selecione uma data valida');
        $('.loadingDefault').hide();
        return false;
    }

    if (ValidarDatasMultiTrecho() == false) {
        $('.loadingDefault').hide();
        return false;
    }

    if (($('#qtAdultos').val() == 0) && ($('#qtCriancas').val() == 0) && ($('#qtBebes').val() == 0)) {
        alert('Favor selecione ao menos um adulto!');
        return false;
    }

    $("#aereoForm").validate();

    if ($("#aereoForm").valid()) {


        $('.loadingDefault').fadeIn();

        return true;
    }
    else {
        return false;
    }
}
function AereoLoadingShowRefazer() {
    $('.loadingDefault').fadeIn();
    if ($('#hdnOrigemCodigoOld').val() == "") {
        $('.loadingDefault').hide();
        alert('Você deve selecionar uma origem válida.');
        $('.loadingDefault').fadeOut();
        return false;
    }

    if ($('#hdnDestinoCodigoOld').val() == "") {
        $('.loadingDefault').hide();
        alert('Você deve selecionar um destino válido.');
        $('.loadingDefault').fadeOut();
        return false;
    }

    if (FormatadaDatas($("#DataEmbarque").val()) < FormatadaDatas(getToday())) {
        alert('Favor selecione uma data valida');
        $('.loadingDefault').hide();
        return false;
    }

    if (ValidarDatasMultiTrecho() == false) {
        $('.loadingDefault').hide();
        return false;
    }

    if (($('#qtAdultos').val() == 0) && ($('#qtCriancas').val() == 0) && ($('#qtBebes').val() == 0)) {
        alert('Favor selecione ao menos um adulto!');
        $('.loadingDefault').fadeOut();
        return false;
    }

    $("#aereoForm").validate();

    if ($("#aereoForm").valid()) {


        $('.loadingDefault').fadeIn();

        return true;
    }
    else {
        return false;
        $('.loadingDefault').fadeOut();
    }
}
function AereoPartialLoadingShow() {

    if ($('#qtAdultos').val() == 0) {
        alert('Favor selecione ao menos um adulto!');
        return false;
    }

    $("#loadAereoForm").validate();

    if ($("#loadAereoForm").valid()) {

        $('.loadingDefault').fadeIn();

        return true;
    }
    else {
        return false;
    }
}
function CarregarCarrinho() {
    $("#gridCarrinho").show();
    $("#gridCarrinho").load("../Carrinho/LoadCarrinho", {
    });
    $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', width: '400px' }).css("font-size", "8pt");
}
function conteudoShow(quant) {
    $('.loadingDefault').fadeOut();
    $('#conteudo').fadeIn(200);
    if (quant > 3) {
        showGrid();
    }
}
function adicionarTrecho() {

    var quant = $("#trechosAdicionais li").length + 1;
    if (quant <= 5) {
        var args = new Object();

        if (quant > 40)
            $('#divAddtrecho').fadeOut();

        args = {
            indice: quant + 1
        };

        $.ajax({
            async: false,
            url: '/Home/Trechos',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'html',
            contentType: 'application/json',
            success: function (data) {
                $("#trechosAdicionais").append(data);

            },
            error: function () {
                alert("Problema ao adicionar um novo trecho.");
            }
        });
        $('#Divtitulo').attr('style', 'display:block');

        try {
            AjusteHeightFrameParentParent("iframeBuscadorAereo", "iframes", 10);
        } catch (e) {
        }
    }
}
function adicionarTrechoFiltroLateral() {

    var quant = $("#trechosAdicionais li").length + 1;

    if (quant <= 6) {
        var args = new Object();

        if (quant > 40)
            $('#divAddtrecho').fadeOut();

        args = {
            indice: quant + 1
        };

        $.ajax({
            async: false,
            url: '/Home/TrechosFiltroLateral',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'html',
            contentType: 'application/json',
            success: function (data) {
                $("#trechosAdicionais").append(data);

            },
            error: function () {
                alert("Problema ao adicionar um novo trecho.");
            }
        });
        $('#Divtitulo').attr('style', 'display:block');

        try {
            AjusteHeightFrameParentParent("iframeBuscadorAereo", "iframes", 10);
        } catch (e) {
        }
    }
}
function removerTrecho() {
    var quant = $("#trechosAdicionais li").length + 1;
    $('#li-' + quant).remove();
    var num = 0;
    $.ajax({
        async: false,
        url: '/Home/TrechosRemover',
        type: 'POST',
        data: null,
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $('#divAddtrecho').fadeIn();
        },
        error: function () {
            alert("Problema ao remover trecho.");
        }
    });
}
function Filtrar(componente) {
    $('.loadingDefault').fadeIn();
    $('#conteudo').fadeOut(200);
    jQuery('#' + componente.id).closest('form').submit();

}
function FiltrarLoad(componente) {
    $('.loadingDefault').fadeIn();
    $('#conteudo').fadeOut(200);
    $('#hdnComboPrice').val(jQuery('#' + componente.id).val());
    $('#filterAereoForm').submit();

}
function FiltrarLoadMontePacote(componente) {

    $('.loadingH').css("display", "block");
    $('#hdnCombo').val(jQuery('#' + componente.id).val());
    $('#hdnIsMontePacote').val('1');
    $('#filterAereoForm').submit();
}
function FiltrarLoadValor(valor) {
    $('.loadingDefault').fadeIn();
    $('#conteudo').fadeOut(200);
    $("#hdnValorSelecionado").val(valor);
    $('#filterAereoForm').submit();
}
$(document).ready(function () {
    var selectedVal = $("#radio1:checked").val();
    if (selectedVal == "SomenteIda") {
        $('#destinoDiv').attr('style', 'visibility: hidden');
        $('#divAddtrecho').fadeOut();
        $('#trechosAdicionais').fadeOut();

    }
    var selectedVal = $("#radio3:checked").val();
    if (selectedVal == "MultiTrecho") {
        $('#destinoDiv').attr('style', 'visibility: hidden');
        $('#divAddtrecho').fadeIn();
        $('#trechosAdicionais').fadeIn();
        adicionarTrecho();
    }
    var selectedVal = $("#radio2:checked").val();
    if (selectedVal == "IdaeVolta") {
        $('#destinoDiv').attr('style', 'display: block');
        $('#divAddtrecho').fadeOut();
        $('#trechosAdicionais').fadeOut();
        $('#divRemovetrecho').fadeOut();
    }
    $('#radio1').change(function () {
        var selectedVal = $("#radio1:checked").val();
        if (selectedVal == "SomenteIda") {
            $('#destinoDiv').attr('style', 'visibility: hidden');
            $('#divAddtrecho').fadeOut();
            $('#trechosAdicionais').fadeOut();
        }
        try {
            AjusteHeightFrameParentParent("iframeBuscadorAereo", "iframes", 10);
        } catch (e) {
        }
    });
    $('#radio2').change(function () {
        var selectedVal = $("#radio2:checked").val();
        if (selectedVal == "IdaeVolta") {
            $('#destinoDiv').attr('style', 'display:block');
            $('#divAddtrecho').fadeOut();
            $('#trechosAdicionais').fadeOut();
        }
        try {
            AjusteHeightFrameParentParent("iframeBuscadorAereo", "iframes", 10);
        } catch (e) {
        }
    });
    $('#radio3').change(function () {
        var selectedVal = $("#radio3:checked").val();
        if (selectedVal == "MultiTrecho") {
            $('#destinoDiv').attr('style', 'visibility: hidden');
            $('#divAddtrecho').fadeIn();
            $('#trechosAdicionais').fadeIn();
            var quant = $("#trechosAdicionais li").length + 1;
            var args = new Object();

            if (quant < 2)
                adicionarTrecho();
        }
        try {
            AjusteHeightFrameParentParent("iframeBuscadorAereo", "iframes", 10);
        } catch (e) {
        }
    });
    autoComplete(10, "OrigemAereo");
    autoCompleteAereoDestino(5, "DestinoAereo", 1);

    $(".plus").click(function () {
        if ($(this).attr('disabled') != 'disabled') {
            var text = $(this).next(":text");
            if (text.val() < 9) {
                text.val(parseInt(text.val(), 10) + 1);
            }
        }
    });

    $(".minus").click(function () {
        if ($(this).attr('disabled') != 'disabled') {
            var text = $(this).prev(":text");
            if (text.val() != 1) {
                text.val(parseInt(text.val(), 10) - 1);
            }
        }
    });


    $(".minusCC").click(function () {
        if ($(this).attr('disabled') != 'disabled') {
            var text = $(this).prev(":text");
            if (text.val() != 0) {
                text.val(parseInt(text.val(), 10) - 1);
            }
        }
    });

    $("#DataRetorno").change(function () {
        $("#aereoForm").validate();
    });
    documentWatermark();
    $('.watermarkAereo').watermarkAereo();

    if ($("#DataEmbarque").val() == '')
        $("#DataEmbarque").val(AereogetToday());
    if ($("#DataRetorno").val() == '')
        $("#DataRetorno").val(AereogetTomorow());

    $('.bot-hoteis-mapa').click(function (e) {
        HoteisnoMapa();
    });

});
function AereogetToday() {
    var today = new Date();
    var Days = parseInt(4);
    var dia = (today.getDate()).toString();;
    var mes = (today.getMonth() + 1).toString();;
    var ano = (today.getFullYear()).toString();;

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

    var tomorow = strDia + '/' + strMes + '/' + ano;

    return tomorow;
}
function AereogetTomorow() {
    var today = new Date();
    var Days = parseInt(1);
    Day = AereogetToday();
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

    var tomorow = strDia + '/' + strMes + '/' + ano;

    return tomorow;
}
$(function () {
    $.datepicker.setDefaults($.datepicker.regional["br"]);
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
    $('#DataRetorno').datepicker({
        numberOfMonths: 2,
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
    var today = new Date();
    var date3 = today;
    //var nextDayDate = new Date();
    var nextDayDate1 = new Date(date3);
    nextDayDate1.setDate(nextDayDate1.getDate() + 3);
    var dd1 = nextDayDate1.getDate();
    var mm1 = nextDayDate1.getMonth() + 1; //January is 0!
    var yyyy1 = nextDayDate1.getFullYear();
    if (dd1 < 10) {
        dd1 = '0' + dd1;
    } if (mm1 < 10) {
        mm1 = '0' + mm1;
    } nextDayDate1 = dd1 + '/' + mm1 + '/' + yyyy1;

    $('#DataEmbarque').datepicker({
        numberOfMonths: 2,
        minDate: 4,
        onSelect: function (dateText, inst) {
            $('#DataRetorno').datepicker("option", "minDate", dateText);
            //$('#DataEmbarque-2').datepicker("option", "minDate", dateText);


            var date2 = $('#DataEmbarque').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            nextDayDate.setDate(nextDayDate.getDate() + 1);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            } if (mm < 10) {
                mm = '0' + mm;
            } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#DataRetorno').val(nextDayDate);
            $('#DataEmbarque-2').val(nextDayDate);
            validarDataRetorno('DataEmbarque', 'DataRetorno');

        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});
$(function () {
    $("#searchAdv").click(function () {
        if ($('#moreoptions').css('display') == "block") {
            $('#imgavaereo').attr('src', '../Images/ava.gif');
        } else {
            $('#imgavaereo').attr('src', '../Images/avades.gif');
        }
        $("#moreoptions").toggle("slow");
    });
});
$(function () {
    $("#searchAdvMenu").click(function () {
        if ($('#moreoptions').css('display') == "block") {
            $('#imgavaereo').attr('src', '../Images/ava.gif');
        } else {
            $('#imgavaereo').attr('src', '../Images/avades.gif');
        }
        $("#moreoptions").toggle();
        AjusteHeightFrameParentParent("iframeBuscadorAereo", "iframes", 10);
    });
});
function documentWatermark() {


    var watermarkArray = [];
    var methods = {
        init: function (options) {
            return this.each(function (i) {
                var $this = $(this);
                var $mark = $this.attr('title');
                watermarkArray.push($mark);
                if ($this.is(':password')) {
                    var stringNewTB = '<input type="text" class="watermark marked password" value="' + $mark + '" />';
                    $this.wrap('<span class=\"pw\" />').after(stringNewTB).hide().removeClass('watermark');
                    $this.blur(function () {
                        if ($this.val().length == 0)
                            $this.hide().next().show();
                    }).next().focus(function () {
                        $this.next().hide().prev().show().focus();
                    });
                }
                else if ($this.is(':text') || $this.is('textarea')) {
                    $this.blur(function () {
                        if ($this.val().length == 0)
                            $this.val(watermarkArray[i]).addClass('marked');
                    }).focus(function () {
                        if ($this.val() == watermarkArray[i] && $this.hasClass('marked'))
                            $this.val('').removeClass('marked');
                    });
                    if ($this.val().length < 1)
                        $this.val($mark).addClass('marked');
                }
            });
        },
        clearWatermarks: function () {
            return this.each(function (i) {
                if ($(this).hasClass('marked') && $(this).val() == watermarkArray[i])
                    $(this).val('');
            });
        }
    };

    $.fn.watermarkAereo = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.watermark');
        }
    };
}
function autoCompleteAereoDestino(tipolista, nome, indx) {
    $("#" + nome).append(
        //"<input type='text' id='txt" + nome + "' name='txt" + nome + "' style='width:100%;' autocomplete='off' data-val-length-max='200' data-val-autocompletevalidation='Campo obrigatório' data-val='true' data-val-autocompletevalidation-nomediv='HotelDestino' data-val-autocompletevalidation-textopradao='Informe seu destino' /> " +
        //"<span class='field-validation-error' data-valmsg-for='txt" + nome + "' data-valmsg-replace='true'>" +
        //"<span for='txt" + nome + "' generated='true' class='field-validation-error'></span> " + 
        //"</span>" +
        " <input type='hidden' id='hdn" + nome + "Codigo' name='hdn" + nome + "Codigo' /><input type='hidden' id='hdn" + nome + "Text' />" +
        "<input type='hidden' id='hdn" + nome + "Nome' name='hdn" + nome + "Nome' />"
      );

    if ($('#hdn' + nome + 'Nome').val() != '') {
        $('#' + nome).val($('#hdn' + nome + 'Nome').val());
    }
    $('#' + nome).keyup(function () {
        if ($(this).val() == '') {
            $('#hdn' + nome + 'Codigo').val('');
            $('#hdn' + nome + 'Nome').val('');
            $('#hdn' + nome + 'Text').val('');
            $(this).removeClass('working');
        }
        else if ($(this).val().length <= 2) {
            $('#hdn' + nome + 'Codigo').val('');
            $('#hdn' + nome + 'Nome').val('');
            $('#hdn' + nome + 'Text').val('');
        }
    });

    $('#' + nome).autocomplete({
        source: "/AutoComplete/Repository.ashx?lista=" + tipolista,
        minLength: 2,
        search: function () { $(this).addClass('working'); },
        open: function () { $(this).removeClass('working'); },
        select: function (event, ui) {
            var codigo = ui.item.codigo;
            var select = ui.item.label;
            if (select.length > 1) {
                $('#hdn' + nome + 'Codigo').val(codigo);
                $('#hdn' + nome + 'Nome').val(select);
                $('#hdn' + nome + 'Text').val(select + ' - ' + codigo);
                $("#OrigemCmp_" + (indx + 1)).val(select);
                $('#hdnOrigemCmp_' + (indx + 1) + 'Codigo').val(codigo);
                $('#hdnOrigemCmp_' + (indx + 1) + 'Nome').val(select);
                $('#hdnOrigemCmp_' + (indx + 1) + 'Text').val(select + ' - ' + codigo);
            }
            else {
                event.preventDefault();
                $(this).val('');
            }
        }
    });
}
function VerPrecoParcelas(Grupo, tarifa, listaTarifas) {
    var myVoos = new Array();

    var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
    if (selected.length > 0)
        for (var i = 0; i < selected.length; i++) {
            myVoos[i] = selected[i].value;
        }

    var valido = false;
    if (tarifa.Tipo == "IdaeVolta") {
        if (selected.length != 2) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "SomenteIda") {
        if (selected.length != 1) {
            var valido = true;
        }
    }
    else if (tarifa.Tipo == "MultiTrecho") {
        if (selected.length < 1) {
            var valido = true;
        }
    }
    if (valido) {
        alert('Selecione o Horário desejado!');
    }
    else {
        var myVoos = new Array();

        var selected = $("#DivRadio-" + Grupo + " input[type='radio']:checked");
        if (selected.length > 0) {
            for (var i = 0; i < selected.length; i++) {
                myVoos[i] = selected[i].value;
            }
        }

        var tarifaSel = tarifa;
        tarifaSel.GrupoTarifas = null;
        tarifaSel.Trechos = null;
        tarifaSel.tarifaSel = null;
        tarifaSel.TrechosSel = myVoos;

        $("." + Grupo + "-Parcelas").show();

        var args = new Object();
        args = { AereoModel: tarifaSel, MontePacote: false, listaTarifas: listaTarifas };
        $.ajax({
            url: '/Aereo/FormasdePagamento',
            type: 'POST',
            data: JSON.stringify(args),
            dataType: 'html',
            contentType: 'application/json',
            success: function (data) {
                $("." + Grupo + "-Parcelas div").html(data);
            },
            error: function () {
                $("." + Grupo + "-Parcelas div").html("Tente novamente mais tarde.");
                $("." + Grupo + "-Parcelas").hide();
            }
        });
    }
}
function FecharPrecoParcelas(div, preco) {
    $("." + div + "-Parcelas").hide();
}
function exibirPacotes(Grupo, tarifas) {

    var args = { trechos: tarifas };
    $.ajax({
        url: '/Aereo/ConsultarAereoPacoteMatriz',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $('#divpacotes-' + Grupo).html(data);
            $('.divpacotes').css('display', 'none');
            $('#divpacotes-' + Grupo).fadeIn('slow');
        },
        error: function (data) {
            alert('Problemas ao consultar os pacotes!');
        }
    });
}
function esconderPacotes() {
    $('.divpacotes').fadeOut('slow');

}
function exibirPacotesVenda(Grupo, tarifas, codigoMatriz) {

    var args = { trechos: tarifas, codigoMatriz: codigoMatriz };
    $.ajax({
        url: '/Aereo/ConsultarAereoPacoteVenda',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $('#divpacotesvenda-' + Grupo).html(data);
            $('.divpacotesvenda').css('display', 'none');
            $('#divpacotesvenda-' + Grupo).fadeIn('slow');
        },
        error: function (data) {
            alert('Problemas ao consultar os pacotes!');
        }
    });
}
function esconderPacotesVenda() {
    $('.divpacotesvenda').fadeOut('slow');

}
function AdicionarCarrinhoPA(Atividade, comprar) {

    $.ajax({
        url: '/Carrinho/AdicionarPacoteCarrinho',
        type: 'POST',
        data: JSON.stringify({ CodigoPacote: Atividade, Comprar: comprar }),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (comprar) {
                window.location = "/Reserva";
            }
            else {
                AtualizarCarrinho();
                $('.loadingDefault').fadeOut();
            }

        },
        error: function () {
            alert("Problema ao adicionar no carrinho.");
        }
    });
}
function VerPrecoAP(div) {
    $("." + div + "-Preco").show();
}
function FecharPrecoAP(div, preco) {
    $("." + div + "-Preco").hide();
}
function VerPrecoParcelasAP(div) {
    $("." + div + "-Parcelas").show();
    args = {
        CodigoPacote: div
    };
    $.ajax({
        url: '/Pacote/FormasdePagamento',
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
function FecharPrecoParcelasAP(div, preco) {
    $("." + div + "-Parcelas").hide();
}
