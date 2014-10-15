function limparCompra() {

    $.ajax({
        url: '/Carrinho/LimparCompra',
        type: 'POST',
        data: null,
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#CarrinhoTotal').html(data);
            alert('Limpo com sucesso!');
        },
        error: function () {
            
        }
    });
}


$(document).ready(function () {
    document.getElementById("hideAll").style.display = "none";
    $(".regionalizacao").change(function () {
        var codigoCidades = $(".regionalizacao").val();
        $.ajax({
            url: '/Shared/RegionalizarSite',
            type: 'POST',
            data: { codigoCidade: codigoCidades },
            dataType: 'json',
            success: function (result) {
                location.reload();
            },
            error: function () {
                alert("Problema ao adicionar mais quartos.");
            }
        });
    });
    document.getElementById("hideAll").style.display = "none";
    $("#Checkin").val(getToday());
    $("#Checkout").val(getTomorow());
    //limparCompra();
});

$(function () {
    $("#slider-range").slider({
        range: true,
        min: 0,
        max: 500,
        values: [75, 300],
        slide: function (event, ui) {
            $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    $("#amount").val("$" + $("#slider-range").slider("values", 0) +
    " - $" + $("#slider-range").slider("values", 1));
});


$(function () {
    $("#tabs").tabs();
});

$(function () {
    $('#slidesDestaque-').slidesjs({
        width: 940,
        height: 528,
        play: {
            active: false,
            effect: "slide",
            interval: 20000,
            auto: true,
            swap: true,
            pauseOnHover: true
        }
    });
});

$(function () {
    $('#slidesPacotesNacionais').slidesjs({
        width: 500,
        height: 420,
        play: {
            active: false,
            effect: "slide",
            interval: 20000,
            auto: true,
            swap: true,
            pauseOnHover: true
        }
    });
});


$(function () {
    $('#slidesPacotesInternacionais').slidesjs({
        width: 500,
        height: 420,
        play: {
            active: false,
            effect: "slide",
            interval: 7000,
            auto: true,
            swap: true,
            pauseOnHover: true
        }
    });
});

$(function () {
    $('.destaquesNacionais').slidesjs({
        width: 940,
        height: 528,
        play: {
            active: false,
            effect: "slide",
            interval: 7000,
            auto: true,
            swap: true,
            pauseOnHover: true,
            restartDelay: 2000
        }
    });
});

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
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    $("#datepicker, #datepicker2").datepicker();
});

function customRange(input) {
    var min = new Date(2008, 11 - 1, 1), //Set this to your absolute minimum date
        dateMin = min,
        dateMax = null,
        dayRange = 6; // Set this to the range of days you want to restrict to

    if (input.id === "#datepicker") {
        if ($("#datepicker2").datepicker("getDate") != null) {
            dateMax = $("#datepicker2").datepicker("getDate");
            dateMin = $("#datepicker2").datepicker("getDate");
            dateMin.setDate(dateMin.getDate() - dayRange);
            if (dateMin < min) {
                dateMin = min;
            }
        }
        else {
            dateMax = new Date; //Set this to your absolute maximum date
        }
    }
    else if (input.id === "#datepicker2") {
        dateMax = new Date; //Set this to your absolute maximum date
        if ($("#datepicker").datepicker("getDate") != null) {
            dateMin = $("#datepicker").datepicker("getDate");
            var rangeMax = new Date(dateMin.getFullYear(), dateMin.getMonth(), dateMin.getDate() + dayRange);

            if (rangeMax < dateMax) {
                dateMax = rangeMax;
            }
        }
    }
    return {
        minDate: dateMin,
        maxDate: dateMax
    };
}