$(document).ready(function () {
    $(".nightsPlus").click(function () {
        var text = $(this).next(":text");
        text.val(parseInt(text.val(), 10) + 1);
        CalculateDate();
    });

    $(".nightsMinus").click(function () {
        var text = $(this).prev(":text");
        if (text.val() != 1 && text.val() != 0) {
            text.val(parseInt(text.val(), 10) - 1);
            CalculateDate();
        }
    });
    $('.calendar').val(getToday());
    $('.noites').val(1);
    CalculateDate();
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
            CalculateNights();
            CalculateDate();
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});

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
    var date = PreviousItem.value.split("/");
    var nextDayDate = new Date(date[2], date[1] - 1, date[0]);
    var noitesMP = parseInt(NumberOfNights.value);
    nextDayDate.setDate(nextDayDate.getDate() + noitesMP);
    var dd = nextDayDate.getDate();
    var mm = nextDayDate.getMonth() + 1; //January is 0!
    var yyyy = nextDayDate.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
    $(NextItem).val(nextDayDate);
}

function changeNights(NextItem, PreviousItem, Nights) {
    var SaidaSplit = $(PreviousItem).val().split("/");
    var checkoutSplit = $(NextItem).val().split("/");
    SaidaSplit[1] = parseInt(SaidaSplit[1]) - 1;
    checkoutSplit[1] = parseInt(checkoutSplit[1]) - 1;
    var dataSaidaFormatada = new Date(SaidaSplit[2], SaidaSplit[1], SaidaSplit[0]);
    var dataCheckoutFormatada = new Date(checkoutSplit[2], checkoutSplit[1], checkoutSplit[0]);
    var umDia = 24 * 60 * 60 * 1000; //horas*minutos*segundos*milisegundos
    var dias = Math.round(Math.abs((dataSaidaFormatada.getTime() - dataCheckoutFormatada.getTime()) / (umDia)));
    $(Nights).val(dias);
}