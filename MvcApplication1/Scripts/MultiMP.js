gerarLog = true;

$(document).ready(function () {
    $("#OrigemCmpMP_1").addClass("inputdestino watermark");
    $("#OrigemCmpMP_1").attr("title", "Informe sua origem");
    $("#OrigemCmpMP_1").val("");
    $('#OrigemCmpMP_1').watermark();

    $("#DestinoCmpMP_1").addClass("inputdestino watermark");
    $("#DestinoCmpMP_1").attr("title", "Informe seu destino");
    $("#DestinoCmpMP_1").val("");
    $('#DestinoCmpMP_1').watermark();

    autoComplete(6, "OrigemCmpMP_1");
    autoComplete(6, "DestinoCmpMP_1");

    $('#MPIdaevolta').change(function () {
        var selectedVal = $("#MPIdaevolta:checked").val();
        if (selectedVal == "Single") {
            $('.MPSingle').show();
            $('.MPmultiDiv').hide();
        }
    });
    $('#MPMulti').change(function () {
        var selectedVal = $("#MPMulti:checked").val();
        if (selectedVal == "Multi") {
            $('.MPSingle').hide();
            $('.MPmultiDiv').show();
            adicionarTrechoMP();
            $("#DataEmbarqueMP-2").val(getTomorow());
        }
    });

    $("#plusRoomsMonteMulti").click(function () {
        var quant = $(".listaQuartosMPMulti li").length;
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
                $(".listaQuartosMPMulti").append(data);
            },
            error: function () {
                alert("Problema ao adicionar mais quartos.");
                window.location = "/Home";
            }
        });
        return false;
    });

    $("#minusRoomsMonteMulti").click(function () {
        if ($(".listaQuartosMPMulti li").length != 1) {
            $(".listaQuartosMPMulti li").last().remove();
            return false;
        } else { return false; }
    });




});


var Retornos = new Array();
function adicionarTrechoMP() {

    var quant = $("#trechosAdicionaisMP li").length + 1;
    var args = new Object();

    if (quant > 4)
        $('#divAddtrechoMP').fadeOut();

    args = {
        indice: quant + 1
    };

    $.ajax({
        async: false,
        url: '/MontePacote/TrechosMP',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#trechosAdicionaisMP").append(data);
        },
        error: function () {
            alert("Problema ao adicionar um novo trecho.");
        }
    });

    var retornosMP = $(".MPRetorno");
    if (retornosMP.length == 2) {
        Retornos.push(retornosMP[0]);
        retornosMP[0].remove();
    }

    CalculateDate();
    CalculateNights();
}

function removerTrechoMP() {
    var quant = $("#trechosAdicionaisMP li").length + 1;
    if (quant - 1 > 1) {
        $('#liMP-' + quant).remove();
        var num = 0;
        $.ajax({
            async: false,
            url: '/MontePacote/TrechosRemoverMP',
            type: 'POST',
            data: null,
            dataType: 'html',
            contentType: 'application/json',
            success: function (data) {
                $('#divAddtrechoMP').fadeIn();
            },
            error: function () {
                alert("Problema ao remover trecho.");
            }
        });

        var ind = $("#trechosAdicionaisMP li").length + 1;
        $(".testeMPRe_" + ind).append(Retornos[Retornos.length - 1]);
        Retornos.splice(Retornos.length - 1, 1);
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////

$(document).ready(function () {
    $('.calendar').val(getToday());
    $(".plusMPMulti").click(function () {
        var text = $(this).next(":text");
        text.val(parseInt(text.val(), 10) + 1);
        CalculateDate();
        CalculateNights();
    });

    $(".minusMPMulti").click(function () {
        var text = $(this).prev(":text");
        if (text.val() != 1 && text.val() != 0) {
            text.val(parseInt(text.val(), 10) - 1);
            CalculateDate();
            CalculateNights();
        }
    });

    //$('.noites').val(1);
    //CalculateDate();
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
    $('.calendar').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        minDate: 0,
        onSelect: function (dateText, inst) {
            CalculateDate();
            CalculateNights();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});


function Logar(msg){
    if(gerarLog){
        console.log(msg);
    }
}

function CalculateDate() {
    var calendars = $('.calendar');
    var noites = $('.noites');
    for (var i = 0; i < calendars.length - 1; i++) {
        changeDates(noites[i], calendars[i + 1], calendars[i]);
    }
}

function CalculateNights() {
    var calendars = $('.calendar');
    var noites = $('.noites');
    for (var i = 0; i < calendars.length - 1; i++) {
        changeNights(calendars[i + 1], calendars[i], noites[i]);
    }
}

function changeDates(NumberOfNights, NextItem, PreviousItem) {
    Logar("changeDates INIT");
    var date = PreviousItem.value.split("/");
    var nextDayDate = new Date(date[2], date[1] - 1, date[0]);
    var noitesMP = parseInt(NumberOfNights.value);
    //nextDayDate.setDate(nextDayDate.getDate() + noitesMP);
    var dd = nextDayDate.getDate() + noitesMP;
    var mm = nextDayDate.getMonth() + 1; //January is 0!
    var yyyy = nextDayDate.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
    $(NextItem).val(nextDayDate);
    Logar("changeDates:" + nextDayDate);
}

function changeNights(NextItem, PreviousItem, Nights) {
    Logar("changeNights INIT");
    var hoje = new Date();
    var SaidaSplit = $(PreviousItem).val().split("/");
    var checkoutSplit = $(NextItem).val().split("/");
    SaidaSplit[1] = parseInt(SaidaSplit[1]) - 1;
    checkoutSplit[1] = parseInt(checkoutSplit[1]) - 1;
    var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
    var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);

    if (hoje.setDate(1) == dataCheckoutFormatada) {
        alert('oi');
    }

    var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos
    var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));
    $(Nights).val(dias);

    Logar("changeNights:" + dias);
}

function autoCompleteMultiMP(tipolista, nome, indx) {
    $("#" + nome).append(
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
            $("#hdn" + nome + "Codigo").val("");
            $("#hdn" + nome + "Nome").val("");
            $("#hdn" + nome + "Text").val("");
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
                $("#OrigemCmpMP_" + (indx + 1)).val(select);
                $('#hdnOrigemCmpMP_' + (indx + 1) + 'Codigo').val(codigo);
                $('#hdnOrigemCmpMP_' + (indx + 1) + 'Nome').val(select);
                $('#hdnOrigemCmpMP_' + (indx + 1) + 'Text').val(select + ' - ' + codigo);
            }
            else {
                event.preventDefault();
                $(this).val('');
            }
        }
    });
}

function SendToServerMPMulti() {
    var retornoValidacao = ValidarMulti();

    if (!ValidaOrigensMulti()) {
        $('.loadingDefault').hide();
        alert('Você deve selecionar uma origem válida.');
        return false;
    }

    if (!ValidaDestinosMulti()) {
        $('.loadingDefault').hide();
        alert('Você deve selecionar um destino válido.');
        return false;
    }

    if (!ValidarMulti()) {
        $('.loadingDefault').hide();
        alert('Você deve selecionar os passageiros da viagens.');
        return false;
    }

    $('.loadingDefault').show();

    $('.watermark').watermark('clearWatermarks');
    var criancas = Array();
    var agechild = Array();

    var Apartamentos = Array();
    var Quartos = $(".listaQuartosMPMulti li");

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

    $("#ApartamentoMP").val(JSON.stringify(Apartamentos));

    FillOrigens();
    FillDestinos();
    FillSaidas();

    $("#formMontePacoteMulti").submit();
}

function ValidaOrigensMulti() {
    var IsValid = true;
    var origens = $('.OrigenMulti');
    for (var i = 0; i < origens.length; i++) {
        var ind = i + 1;
        if ($('#hdnOrigemCmpMP_' + ind + 'Codigo').val() == "") {
            IsValid = false;
        }
    }
    return IsValid;
}

function ValidaDestinosMulti() {
    var IsValid = true;
    var destinos = $('.DestinoMulti');

    for (var i = 0; i < destinos.length; i++) {
        var ind = i + 1;
        if ($('#hdnDestinoCmpMP_' + ind + 'Codigo').val() == "") {
            IsValid = false;
        }
    }
    return IsValid;
}

function FillDestinos() {
    var destinos = $('.DestinoMulti');
    var listaDestinos = "";
    var listaDestinosCodigo = "";
    for (var i = 0; i < destinos.length; i++) {
        var ind = i + 1;

        if ($('#hdnDestinoCmpMP_' + ind + 'Nome').val().indexOf(",") > 0) {
            listaDestinos += $('#hdnDestinoCmpMP_' + ind + 'Nome').val().split(',')[0];
        } else {
            listaDestinos += $('#hdnDestinoCmpMP_' + ind + 'Nome').val();
        }

        listaDestinosCodigo += $('#hdnDestinoCmpMP_' + ind + 'Codigo').val();
        if (i != destinos.length - 1) {
            listaDestinos += ",";
            listaDestinosCodigo += ",";
        }
    }

    $("#ListaDestino").val(listaDestinos);
    $("#ListaDestinoCodigo").val(listaDestinosCodigo);
}

function FillOrigens() {
    var destinos = $('.OrigenMulti');
    var listaDestinos = "";
    var listaDestinosCodigo = "";
    for (var i = 0; i < destinos.length; i++) {
        var ind = i + 1;
        if ($('#hdnOrigemCmpMP_' + ind + 'Nome').val().indexOf(",") > 0) {
            listaDestinos += $('#hdnOrigemCmpMP_' + ind + 'Nome').val().split(',')[0];
        } else {
            listaDestinos += $('#hdnOrigemCmpMP_' + ind + 'Nome').val();
        }
        listaDestinosCodigo += $('#hdnOrigemCmpMP_' + ind + 'Codigo').val();
        if (i != destinos.length - 1) {
            listaDestinos += ",";
            listaDestinosCodigo += ",";
        }
    }

    $("#ListaOrigem").val(listaDestinos);
    $("#ListaOrigemCodigo").val(listaDestinosCodigo);
}

function FillSaidas() {
    var Saidas = $('.SaidaMulti');
    var listaSaidas = "";
    for (var i = 0; i < Saidas.length; i++) {
        var ind = i + 1;
        listaSaidas += Saidas[i].value;
        if (i != Saidas.length - 1) {
            listaSaidas += ",";
        }
    }
    $("#ListaSaida").val(listaSaidas);
    $("#RetornoMultiMP").val($(".RetornoMP").val());
}

function ValidarMulti() {

    var item = parseInt($(".QuartosMulti").val());

    if (item <= 0) {
        return false;
    } else {
        return true;
    }

    //var validado = true;

    //var Quartos = $(".listaQuartosMPMulti li");

    //for (var i = 0; i < Quartos.length ; i++) {
    //    var Apartamento = new Object();
    //    Quartos[i].id = "liId" + i;
    //    Apartamento.quantadulto = $("#liId" + i + " input.QuantAdultos").val();
    //    Apartamento.criancas = Array();
    //    agechild = $("#" + Quartos[i].id + " .agechild");
    //    for (var j = 0; j < agechild.length ; j++) {
    //        Apartamento.criancas.push(agechild[j].value);
    //    }

    //    if (Apartamento.quantadulto <= 0 && Apartamento.criancas.length <= 0) {
    //        validado = false;
    //        $("#lblErroApartamentos").text("Viagem deve conter passageiros.");
    //        $("#lblErroApartamentos").css("display", "block");
    //    }
    //    else {
    //        $("#lblErroApartamentos").text("");
    //        $("#lblErroApartamentos").css("display", "none");
    //    }
    //}

    //return validado;
}

function documentWatermarkMP() {
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
                    })
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
    }

}

function changeNightsKey(valor) {
    if (parseInt(valor) > 0)
    {
        CalculateDate();
        CalculateNights();
    }
}