
function BuscaAtividades() {
    $("#btnBuscarAtividades").click(function () {
        $("#gridContainer").show();
        $("#gridContainer").load("Atividade/LoadGrid", {
            DestinoAtividade: $("#DestinoAtividade").val(),
            DataInicio: $("#dtEmbarque").val(),
            DataFinal: $("#dtRetorno").val(),
            qtdAdultos: $("#ddlAdultos").val(),
            qtdCriancas: $("#ddlCriancas").val()
        });
    });
}

function LoadCarrinho() {
    $("#gridCarrinho").show();
    $("#gridCarrinho").load("Carrinho/LoadCarrinho");
    $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', width: '400px' }).css("font-size", "8pt");
    }

function Reservar(codigo, atividade) {

}

function Registrar(Atividade) {
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $("#gridCarrinho").show();
            $("#gridCarrinho").load("Carrinho/LoadCarrinho", { teste: "" });
            $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', width: '400px' }).css("font-size", "8pt");
        },
        error: function () {
            alert("Problema ao registrar.");
        }
    });
}

function validar() {
    if ($('#hdnDestinoAtividadeCodigo').val() == "") {
        alert('Você deve selecionar um destino válido.');
        return false;
    }
    
    if (FormatadaDatas($("#dtEmbarque").val()) < FormatadaDatas(getToday())) {
        alert('Favor selecione uma data valida');
        return false;
    }

    if (!$('#atividadeForm').valid())
        return false;
}

$(document).ready(function () {


    autoComplete(6, "DestinoAtividade");
    $("#dtEmbarque").val(getToday());
    $("#dtRetorno").val(getTomorow());

    BuscaAtividades();
    LoadCarrinho();
    $(function () {
        $("#clickme").click(function () {
            $("#moreoptions").toggle("slow");
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
            yearSuffix: '',
            showOn: "both",
            changeFirstDay: false
        };
        $.datepicker.setDefaults($.datepicker.regional['pt-BR']);

        $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
        $('#dtRetorno').datepicker({
            numberOfMonths: 2,
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 99999999999999);
                }, 0);
            }
        });
        $('#dtEmbarque').datepicker({
            numberOfMonths: 2,
            minDate: 2,
            onSelect: function (dateText, inst) {
                $('#dtRetorno').datepicker("option", "minDate", dateText);
                var date2 = $('#dtEmbarque').datepicker('getDate');
                //var nextDayDate = new Date();
                var nextDayDate = new Date(date2);
                nextDayDate.setDate(nextDayDate.getDate() + 1);
                var dd = nextDayDate.getDate();
                var mm = nextDayDate.getMonth() + 1; //January is 0!
                var yyyy = nextDayDate.getFullYear();
                if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
                $('#dtRetorno').val(nextDayDate);
                validarDataRetorno('dtEmbarque', 'dtRetorno');
            },
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 99999999999999);
                }, 0);
            }
        });

    });
  
});
