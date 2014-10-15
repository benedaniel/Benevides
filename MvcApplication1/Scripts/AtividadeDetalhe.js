function BuscaAtividades() {
    $(".buttonPacoteRefazer").click(function () {

        if ($("#hdnDestinoAtividadeCodigo").val() == '') {
            return false;
        }
        else {

            $("#gridContainer").show();
            $("#gridContainer").load("Atividade/LoadGrid", {
                DataInicio: $('#dtEmbarque').val(), DataFinal: $('#hdnDtRetorno').val(),
                DestinoAtividade: $("#dtRetorno").val(),
                Crianca1: $('#ddlQ0Crianca0').val(),
                Crianca2: $('#ddlQ0Crianca1').val(),
                Crianca3: $('#ddlQ0Crianca2').val(),
                Crianca4: $('#ddlQ0Crianca3').val(),
                Adulto: $('#ddlQ0Adulto').val()
            });
        }
    });
}

function aumentarIdade(eu, qtd, quartoid) {
   
    for (var i = 0; i < 5 ; i++) {
        var elem = document.getElementById(eu + i);
        if (elem != null)
            elem.parentNode.removeChild(elem);
    }

    $('#lblIDades').show();

    if (qtd == 0)
        $('#lblIDades').hide();


    for (var i = 0; i < qtd ; i++) {
        $('<select/>', {
            id: eu + i,
            name: eu + i,
            style: "margin-top:8px;width:45px;height: 37px;background: url(../Images/images_pacote/dropdown_arrow.png) no-repeat right #fff;font-weight: bold;font-family: Verdana;color: #00387b;"
        }).appendTo('#agrpQuarto' + quartoid);

        $('<option/>', { value: "0", text: "0" }).appendTo('#' + eu + i);
        $('<option/>', { value: "1", text: "1" }).appendTo('#' + eu + i);
        $('<option/>', { value: "2", text: "2" }).appendTo('#' + eu + i);
        $('<option/>', { value: "3", text: "3" }).appendTo('#' + eu + i);
        $('<option/>', { value: "4", text: "4" }).appendTo('#' + eu + i);
        $('<option/>', { value: "5", text: "5" }).appendTo('#' + eu + i);
        $('<option/>', { value: "6", text: "6" }).appendTo('#' + eu + i);
        $('<option/>', { value: "7", text: "7" }).appendTo('#' + eu + i);
        $('<option/>', { value: "8", text: "8" }).appendTo('#' + eu + i);
        $('<option/>', { value: "9", text: "9" }).appendTo('#' + eu + i);
        $('<option/>', { value: "10", text: "10" }).appendTo('#' + eu + i);
        $('<option/>', { value: "11", text: "11" }).appendTo('#' + eu + i);
        $('<option/>', { value: "12", text: "12" }).appendTo('#' + eu + i);

    }


}

/* Adicionar no Carrinho*/
function AdicionarAtividadeTransfer(Atividade, comprar) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { Atividade: Atividade, Valor: $('#hdnValorTotal_' + Atividade).val(), Dia: $('#ddl' + Atividade).find('option:selected').text(), Comprar: comprar, entrada: $('#lbl_entrada_' + Atividade).text(), parcelas: $('#lbl_parcelas_' + Atividade).text() }
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeTransfer',
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
            alert("error");
        }
    });
}

function AdicionarAtividadeRestaurante(Atividade, comprar) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { Atividade: Atividade, Valor: $('#hdnValorTotal_' + Atividade).val(), Dia: $('#ddl' + Atividade).find('option:selected').text(), Comprar: comprar, entrada: $('#lbl_entrada_' + Atividade).text(), parcelas: $('#lbl_parcelas_' + Atividade).text() }
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeRestaurante',
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
            alert("error");
        }
    });
}

function AdicionarAtividadeIngresso(Atividade, comprar) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { Atividade: Atividade, Valor: $('#hdnValorTotal_' + Atividade).val(), Dia: $('#ddl' + Atividade).find('option:selected').text(), Comprar: comprar, entrada: $('#lbl_entrada_' + Atividade).text(), parcelas: $('#lbl_parcelas_' + Atividade).text() }
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeIngresso',
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
            alert("Problema ao adicionar Transfer da Atividade.");
        }
    });
}

function AdicionarAtividadeTransferMontePacote(Atividade, comprar) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeTransfer',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/Reserva";
        },
        error: function () {
            alert("Problema ao adicionar Transfer da Atividade.");
        }
    });
}

function AdicionarAtividadePasseio(Atividade, comprar) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { Atividade: Atividade, Valor: $('#hdnValorTotal_' + Atividade).val(), Dia: $('#ddl' + Atividade).find('option:selected').text(), Comprar: comprar, entrada: $('#lbl_entrada_' + Atividade).text(), parcelas: $('#lbl_parcelas_' + Atividade).text() }
    $.ajax({
        url: '/Carrinho/AdicionarAtividadePasseio',
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
            alert("error");
        }
    });
}

/*Fim Adicionar Carrinho*/

/* Métodos relacionados a orçamento */

function AdicionarAtividadeIngressoOrcamento(orcamentoItem) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { orcamentoItem: orcamentoItem, Valor: $('#hdnValorTotal_' + orcamentoItem).val(), Dia: $('#ddl' + orcamentoItem).find('option:selected').text(), entrada: $('#lbl_entrada_' + orcamentoItem).text(), parcelas: $('#lbl_parcelas_' + orcamentoItem).text() };
    $.ajax({
        url: '/Orcamento/AdicionarAtividadeIngressoOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar Ingresso da Atividade.");
        }

    });
}

function AdicionarAtividadePasseioOrcamento(orcamentoItem) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { orcamentoItem: orcamentoItem, Valor: $('#hdnValorTotal_' + orcamentoItem).val(), Dia: $('#ddl' + orcamentoItem).find('option:selected').text(), entrada: $('#lbl_entrada_' + orcamentoItem).text(), parcelas: $('#lbl_parcelas_' + orcamentoItem).text() };
    $.ajax({
        url: '/Orcamento/AdicionarAtividadePasseioOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar Passeio da Atividade.");
        }

    });
}

function AdicionarAtividadeRestauranteOrcamento(orcamentoItem) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { orcamentoItem: orcamentoItem, Valor: $('#hdnValorTotal_' + orcamentoItem).val(), Dia: $('#ddl' + orcamentoItem).find('option:selected').text(), entrada: $('#lbl_entrada_' + orcamentoItem).text(), parcelas: $('#lbl_parcelas_' + orcamentoItem).text() };
    $.ajax({
        url: '/Orcamento/AdicionarAtividadeRestauranteOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar Restaurante da Atividade.");
        }

    });
}

function AdicionarAtividadeAssitenciaOrcamento(orcamentoItem, dataInicial) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { orcamentoItem: orcamentoItem, Valor: $('#hdnValorTotal_' + orcamentoItem).val(), Dia: dataInicial, entrada: $('#lbl_entrada_' + orcamentoItem).text(), parcelas: $('#lbl_parcelas_' + orcamentoItem).text() };
    $.ajax({
        url: '/Orcamento/AdicionarAtividadeAssistenciaOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar Transfer da Atividade.");
        }

    });
}

function AdicionarAtividadeTransferOrcamento(orcamentoItem) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { orcamentoItem: orcamentoItem, Valor: $('#hdnValorTotal_' + orcamentoItem).val(), Dia: $('#ddl' + orcamentoItem).find('option:selected').text(), entrada: $('#lbl_entrada_' + orcamentoItem).text(), parcelas: $('#lbl_parcelas_' + orcamentoItem).text() };
    $.ajax({
        url: '/Orcamento/AdicionarAtividadeTransferOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar Transfer da Atividade.");
        }

    });
}

function AdicionarAtividadePacoteAtividadeOrcamento(orcamentoItem) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { orcamentoItem: orcamentoItem, Valor: $('#hdnValorTotal_' + orcamentoItem).val(), Dia: $('#ddl' + orcamentoItem).find('option:selected').text(), entrada: $('#lbl_entrada_' + orcamentoItem).text(), parcelas: $('#lbl_parcelas_' + orcamentoItem).text() };
    $.ajax({
        url: '/Orcamento/AdicionarAtividadePacoteAtividadeOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar Pacote atividade da Atividade.");
        }

    });
}

/*Fim dos objetos enviado a Orçamentos*/

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
            $.modal.close();
        },
        error: function () {
            alert("Problema ao mostrar orçamento.");
        }
    });
}

function AdicionarAtividadeAssistencia(Atividade, comprar, dataInicial, dataFinal) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = { Atividade: Atividade, Valor: $('#hdnValorTotal_' + Atividade).val(), Dia: dataInicial, DiaRetorno: dataFinal, Comprar: comprar }
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeAssistencia',
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
            alert("Problema ao adicionar Assistencia da Atividade.");
        }
    });
}

function AdicionarAtividadeAssistenciaMontePacote(Atividade) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Carrinho/AdicionarAtividadeAssistencia',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            window.location = "/Reserva";
        },
        error: function () {
            alert("Problema ao adicionar Assistencia da Atividade.");
        }
    });
}

function AdicionarAtividadeAssistenciaOrcamento(Atividade) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Orcamento/AdicionarAtividadeAssistenciaOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            alert("Orçamento incluído");
        },
        error: function () {
            alert("Problema ao adicionar Assistencia da Atividade.");
        }
    });
}

function AdicionarCarrinho(Atividade) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Carrinho/AdicionarAtividade',
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
            alert("Problema ao adicionar item no carrinho.");
        }
    });
}

function AdicionarCarrinhoMontePacote(Atividade) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Carrinho/AdicionarAtividade',
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
            alert("Problema ao adicionar item no carrinho.");
        }
    });
}

function AdicionarOrcamento(Atividade) {

    $('.loadingDefault').fadeIn();
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Orcamento/AdicionarAtividadeOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $('html, body').animate({ scrollTop: 0 }, 'fast');
            alert("Orçamento incluído");
        },
        error: function () {
            alert("Problema ao adicionar item no orçamento.");
        }
    });
}

function DataTarifaChange(valor) {
    var campo1 = $('#ddl' + valor).val().split('|')[0].toString().slice(0, -2);
    var campo2 = $('#ddl' + valor).val().split('|')[0].toString().substring($('#ddl' + valor).val().split('|')[0].length - 2, $('#ddl' + valor).val().split('|')[0].length);

    $('#lbl_' + valor).text(campo1);
    $('#lbl_centavos_' + valor).text(campo2);
}

function Registrar() {
    var args = new Object();
    args = $('#FomMatriz').serialize();
    $.ajax({
        url: '/Pacote/LoadDetalhePacoteMatriz',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            //do Nothing
        },
        error: function () {
            alert("Erro (data tarifa)");
        }
    });
}

$(document).ready(function () {

    $('.loadingDefault').fadeIn();
    autoComplete(6, "DestinoAtividade");
    $("#hdnDestinoAtividadeCodigo").val($("#hdnDestinosCodigo").val());
    $('#btnSubmit').click(function () {

        if ($("#hdnDestinoAtividadeCodigo").val() == "") {
            alert("Por favor informar um destino válido.");
            return;
        } else {
            //ValidacaodeHDN

            $('.loadingDefault').fadeIn();
            $("#lblPacotesPara").text($("#hdnDestinoAtividadeNome").val());

            $.ajax({
                url: '/Atividade/LoadGrid',
                type: 'POST',
                data: {
                    DataInicio: $('#dtEmbarque').val(), DataFinal: $('#dtRetorno').val(),
                    DestinoAtividade: $("#hdnDestinoAtividadeCodigo").val(),
                    Crianca1: $('#ddlQ0Crianca0').val(),
                    Crianca2: $('#ddlQ0Crianca1').val(),
                    Crianca3: $('#ddlQ0Crianca2').val(),
                    Crianca4: $('#ddlQ0Crianca3').val(),
                    Adulto: $('#ddlQ0Adulto').val()
                },
                success: function (result) {
                    $("#LoadGrid").html(result);
                    $('.loadingDefault').fadeOut();
                }
            });
        }
    });

    $('#DestinoAtividade').val($('#hdnDestino').val());
    $('#ddlQ0Adulto').val($('#hdnqtAdultos').val());
    aumentarIdade("ddlQ0Crianca", $('#hdnddlQ0crianca').val(), 0);
    $('#ddlQ0crianca').val($('#hdnddlQ0crianca').val());
    $('#ddlQ0Crianca0').val($('#hdnddlQ0crianca0').val());
    $('#ddlQ0Crianca1').val($('#hdnddlQ0crianca1').val());
    $('#ddlQ0Crianca2').val($('#hdnddlQ0crianca2').val());
    $('#ddlQ0Crianca3').val($('#hdnddlQ0crianca3').val());

    $("a#single_image").fancybox();
    $("a#inline").fancybox({ 'hideOnContentClick': true });
    $("a.group").fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        'overlayShow': false
    });

    $('.loadingDefault').fadeIn();
    $.ajax({
        url: '/Atividade/LoadGrid',
        type: 'POST',
        data: {
            DataInicio: $('#hdnDtEmbarque').val(), DataFinal: $('#hdnDtRetorno').val(),
            DestinoAtividade: $("#hdnDestinosCodigo").val(),
            Crianca1: $('#hdnddlQ0crianca0').val(),
            Crianca2: $('#hdnddlQ0crianca1').val(),
            Crianca3: $('#hdnddlQ0crianca2').val(),
            Crianca4: $('#hdnddlQ0crianca3').val(),
            Adulto: $('#hdnqtAdultos').val()
        },
        success: function (result) {
            $("#LoadGrid").html(result);
            $('.loadingDefault').fadeOut();
        }
    });

    BuscaAtividades();

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
            minDate: 0,
            onSelect: function (dateText, inst) {
                $('#DataRetorno').datepicker("option", "minDate", dateText);
            },
            beforeShow: function () {
                setTimeout(function () {
                    $('.ui-datepicker').css('z-index', 99999999999999);
                }, 0);
            }
        });

    });

});

function VerPrecoParcelas(codigoAtividade, tipoatividade, retorno) {
    $("." + codigoAtividade + "-Parcelas-" + tipoatividade).show();
    args = {
        CodigoAtividade: codigoAtividade,
        TipoAtividade: tipoatividade,

        Valor: $('#hdnValorTotal_' + codigoAtividade).val(),
        Dia: $('#ddl' + codigoAtividade).find('option:selected').text(),
        entrada: $('#lbl_entrada_' + codigoAtividade).text(),
        parcelas: $('#lbl_parcelas_' + codigoAtividade).text(),
        DiaRetorno: retorno,
        Custo: $("#hdnCustoTotal_" + codigoAtividade).val()
    };

    $.ajax({
        url: '/Atividade/FormasdePagamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("." + codigoAtividade + "-Parcelas-" + tipoatividade + " div").html(data);
        },
        error: function () {
            $("." + codigoAtividade + "-Parcelas-" + tipoatividade + " div").html("Tente novamente mais tarde.");
            $("." + codigoAtividade + "-Parcelas-" + tipoatividade).hide();
        }
    });
}

function FecharPrecoParcelas(codigoAtividade, tipoatividade) {
    $("." + codigoAtividade + "-Parcelas-" + tipoatividade).hide();
}
