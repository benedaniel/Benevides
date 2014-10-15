$(document).ready(function () {


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
    $('#DataFileFinal').datepicker({
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
    nextDayDate1.setDate(nextDayDate1.getDate() );
    var dd1 = nextDayDate1.getDate();
    var mm1 = nextDayDate1.getMonth() + 1; //January is 0!
    var yyyy1 = nextDayDate1.getFullYear();
    if (dd1 < 10) { dd1 = '0' + dd1 } if (mm1 < 10) { mm1 = '0' + mm1 } nextDayDate1 = dd1 + '/' + mm1 + '/' + yyyy1;

    $('#DataFileInicial').datepicker({
        numberOfMonths: 2,
        maxDate: nextDayDate1,
        onSelect: function (dateText, inst) {
            var nextDayDate1 = new Date(date3);
            nextDayDate1.setDate(nextDayDate1.getDate());
            $('#DataFileFinal').datepicker("option", "minDate", dateText);
            $('#DataFileFinal').datepicker("option", "maxDate", nextDayDate1);
            //$('#DataEmbarque-2').datepicker("option", "minDate", dateText);


            var date2 = $('#DataFileInicial').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            nextDayDate.setDate(nextDayDate.getDate() + 1);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#DataFileFinal').val(nextDayDate);

            validarDataRetorno('DataFileInicial', 'DataFileFinal');

        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });


    datepickerCampo("DataSaidaInicial");
    datepickerCampo("DataSaidaFinal");
    datepickerCampo("DataVoucherInicial");
    datepickerCampo("DataVoucherFinal");
});

$(function () {
    
    $("#btnPesqusa").click(function () {
        if (validaForm())
        {
            var args = new Object();
            args = {
                DataFileInicial: $("#DataFileInicial").val(),
                DataFileFinal: $("#DataFileFinal").val()
            };

            $.ajax({
                url: '/Venda/Consulta',
                type: 'POST',
                beforeSend: function () { showdivLoad(); },
                complete: function () { hidedivLoad(); },
                data: JSON.stringify(args),
                datatype: 'json',
                contentType: 'application/json',
                success: function (data) {
                    $("#divResultadoConsultaVenda").html(data);
                },
                error: function (data) {
                    alert("Problema ao consultar.");
                }
            });

        }
    });
});

function validaForm() {
    $("#formConsultaVenda").validate();

    if ($("#formConsultaVenda").valid()) {
        return true;
    }
    else {
        return false;
    }
}

function detalheVenda(venda) {

    $.ajax({
        url: '/Venda/Detalhe',
        type: 'POST',
        beforeSend: function () { showdivLoad(); },
        complete: function () { hidedivLoad(); },
        data: { id_file: $('#ID_FILE').html() },
        datatype: 'json',

        success: function (data) {
            $("#divDetalheConsultaVenda").html(data);
            $("#divDetalheConsultaVenda").modal({
                minHeight: 620,
                minWidth: 900
            });
            $('#simplemodal-container a').after('<div class="title-detalhe">Detalhe da Venda</div>');
            $('.simplemodal-wrap').css('height', '95%');
        },
        error: function (data) {
            alert("Problema ao exibir detalhe da venda.");
        }
    });

}

function datepickerCampo(idcampo) {
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
    $('#' + idcampo).datepicker({
        numberOfMonths: 2,
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });

};
function showhideFiltre() {
    $('#divFiltro').toggle('slow');
}