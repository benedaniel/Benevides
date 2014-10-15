$(document).ready(function () {
    $("#DataInicial").val(getYesterday());
    $("#DataFinal").val(getTodaySame());

    $('#NumOrcamento').keydown(function (event) {
        if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9
            || event.keyCode == 27 || event.keyCode == 13
            || (event.keyCode == 65 && event.ctrlKey === true)
            || (event.keyCode >= 35 && event.keyCode <= 39)) {
            return;
        } else {
            if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                event.preventDefault();
            }
        }
    });

    $(".btnPesquisar").click(function () {
        getOrcamentoAtivo();
    });

    $(".button-salvar-notsend").click(function () {
        $('.loadingDefault').show();
        $.ajax({
            url: '/Orcamento/atualizaDadosOrcamento',
            type: 'POST',
            data: { CodigoOrcamento: $("#hdncodigoORcamento").val(), Nome: $("#inputname").val(), EmailEnvio: $("#inputEmail").val(), Assunto: $("#inputAssunto").val(), Mensagem: $("#inputMensagem").val() },
            dataType: 'html',
            success: function (data) {
                $('.loadingDefault').hide();
                if (data) {
                    $.modal.close();
                    alert("Orçamento salvo com sucesso.");
                    window.location = "/Orcamento/EditarOrcamento?code=" + data;
                } else { alert("Erro ao salvar o orçamento."); }
            },
            error: function () {
                $('.loadingDefault').hide();
                alert("Erro ao salvar o orçamento");
            }
        });
    });

    $(".button-closed-notsave").click(function () {
        window.location = "/" ;
    });

});



function getOrcamentoAtivo() {
    var dataini = $("#DataInicial").val();
    var datafin = $("#DataFinal").val();
    var cod = $("#NumOrcamento").val();

    $(".orcamento-resultado img").show();
    $(".orcamento-resultado div").remove();

    var args = new Object();
    args = {
        datainicial: dataini,
        datafinal: datafin,
        codigo: cod,
        status: 1
    };

    $.ajax({
        url: '/Orcamento/GetOrcamentos',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $(".orcamento-resultado img").hide();
            $(".orcamento-resultado").append(data);
        },
        error: function () {
            alert("Erro ao carregar orçamentos");
        }
    });
}
function showMensage() {
    $("#divsalvar").modal({
        Height: 350
    });
}

function salvaOrcamento() {
    var Nome = $("#NameCotizacao").val();

    if (Nome == null || Nome == "" || Nome == " ") {
        alert("Informe o nome do orçamento.");
    }
    else {
        $.ajax({
            url: '/Orcamento/SalvarOrcamento',
            type: 'POST',
            data: { NomeOrcamento: Nome },
            dataType: 'html',
            success: function (data) {
                if (data) {
                    $.modal.close();
                    alert("Orçamento salvo com sucesso.");
                    window.location = "/Orcamento/EditarOrcamento?code=" + data;
                } else { alert("Erro ao salvar o orçamento."); }
            },
            error: function () {
                alert("Erro ao salvar o orçamento");
            }
        });
    }
}

function atualizarOrcamento() {
    var CodigoOrcamento = $("#SelectCotizacao").val();

    if (CodigoOrcamento == 0)
    { alert("Escolha um orçamento."); }
    else {
        $.ajax({
            url: '/Orcamento/atualizarOrcamento',
            type: 'POST',
            data: { CodigoOrcamento: CodigoOrcamento },
            dataType: 'html',
            success: function (data) {
                if (data) {
                    $.modal.close();
                    alert("Orçamento salvo com sucesso.");
                    window.location = "/Orcamento/EditarOrcamento?code=" + data;
                } else { alert("Erro ao salvar o orçamento."); }
            },
            error: function () {
                alert("Erro ao salvar o orçamento");
            }
        });
    }
}

function GetOrcamentosInativos() {
    var dataini = $("#DataInicial").val();
    var datafin = $("#DataFinal").val();
    var cod = $("#NumOrcamento").val();

    $(".orcamento-resultado img").show();
    $(".orcamento-resultado div").remove();

    var args = new Object();
    args = {
        datainicial: dataini,
        datafinal: datafin,
        codigo: cod,
        status: 1
    };

    $.ajax({
        url: '/Orcamento/GetOrcamentosInativos',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $(".orcamento-resultado img").hide();
            $(".orcamento-resultado").append(data);
        },
        error: function () {
            alert("Erro ao carregar orçamentos");
        }
    });
}

function arquivarOrca(CodigoOrcamento) {
    $('.loadingDefault').show();
    $.ajax({
        url: '/Orcamento/arquivarOrcamento',
        type: 'POST',
        data: { CodigoOrcamento: CodigoOrcamento },
        dataType: 'html',
        success: function (data) {
            getOrcamentoAtivo();
            $('.loadingDefault').hide();
        },
        error: function () {
            alert("Erro ao arquivar orçamentos");
        }
    });

}


function getOrcamentoArquivado() {
    $("#DataInicial").val(getToday());
    $("#DataFinal").val(getTomorow());
}

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
        firstDay: new Date(2009, 10 - 1, 25),
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: '',
        showOn: "both",
        changeFirstDay: false
    };
    $.datepicker.setDefaults($.datepicker.regional['pt-BR']);
    $('#DataFinal').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        onSelect: function (dateText, inst) {
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
    $('#DataInicial').datepicker({
        showOn: "both",
        numberOfMonths: 2,
        minDate: new Date(2009, 10 - 1, 25),
        onSelect: function (dateText, inst) {
            $('#DataFinal').datepicker("option", "minDate", dateText);
            //$('#DataEmbarque-2').datepicker("option", "minDate", dateText);
            var date2 = $('#DataInicial').datepicker('getDate');
            //var nextDayDate = new Date();
            var nextDayDate = new Date(date2);
            nextDayDate.setDate(nextDayDate.getDate() + 1);
            var dd = nextDayDate.getDate();
            var mm = nextDayDate.getMonth() + 1; //January is 0!
            var yyyy = nextDayDate.getFullYear();
            if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } nextDayDate = dd + '/' + mm + '/' + yyyy;
            $('#DataFinal').val(nextDayDate);
            validarDataRetorno('DataInicial', 'DataFinal');
        },
        beforeShow: function () {
            setTimeout(function () {
                $('.ui-datepicker').css('z-index', 99999999999999);
            }, 0);
        }
    });
});

function removerHotelOrcamento(codeHotel, codeOrca)
{
    var args = new Object();
    args = {
        codigoHotel: codeHotel,
        codigoOrcamento: codeOrca
    };

    $.ajax({
        url: '/Orcamento/RemoverHotel',
        type: 'POST',
        beforeSend: function () { showdivLoad(); },
        data: JSON.stringify(args),
        contentType: 'application/json',
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            alert("Erro ao remover item do orçamentos");
        }
    });
}

function removerItemOrcamento(codeHotel, codeOrca, TipoItem) {
    var args = new Object();
    args = {
        codigoItem: codeHotel,
        codigoOrcamento: codeOrca,
        itemOrcamento: TipoItem
    };

    $.ajax({
        url: '/Orcamento/RemoverItemOrcamento',
        type: 'POST',
        beforeSend: function () { showdivLoad(); },
        data: JSON.stringify(args),
        contentType: 'application/json',
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            alert("Erro ao remover item do orçamentos");
        }
    });
}