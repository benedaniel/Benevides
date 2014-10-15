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
});

$(function () {
    $("#txtCodigoCep").keyup(function () {
        validaCep();
    });
});

function buyTO() {
    alert("Existe uma tarifa operadora em seu carrinho, esta tarifa só pode ser vendida junto com hotel.");
    return false;
}

function validaCep() {
    var cep = $("#txtCodigoCep").val().replace('-', '').length;

    if (cep > 8) {
        $("#txtCodigoCep").val($("#txtCodigoCep").val().substring(0, 9));
        return;
    }
    if (cep == 8) {
        consultaCep();
    }
}

function consultaCep() {
    var args = new Object();
    args = { cep: $("#txtCodigoCep").val() };

    $.ajax({
        url: '/PainelControle/ConsultaCep',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.Status == "S") {
                $("#txtLogradouro").val(data.Descricao);
                $("#txtPais").val("Brasil");
                $("#txtCidade").val(data.Cidade);
                $("#txtBairro").val(data.Bairro);

                $("#txtNumero").focus();
            }
            else {
                alert('Cep não emcontrado!');
            }

        },
        error: function () {
            //alert(data);
        }
    });
}

function LoadDistribuidor() {
    $.ajax({
        url: '/Reserva/LoadDistribuidor',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var list = "";
            // alert(data);
            console.log(data);
            var atendente;
            for (var i = 0; i < data.length; i++) {
                //list += "<option dt='teste' value='" + data[i].v_id_pessoa + "'>" + data[i].v_nome_pessoa + "</option>";
                list += data[i].v_nome_pessoa;
                atendente = data[i].v_id_pessoa;
            }
            $("#DLStates").append(list);
            $('#DLAtendente').empty();
            $.ajax({
                url: '/Reserva/LoadAtendente',
                type: 'POST',
                data: JSON.stringify({ idpessoa: atendente }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var list = " <option>Selecione</option>";
                    for (var i = 0; i < data.Atendentes.length; i++) {
                        list += "<option dt='teste' value='" + data.Atendentes[i].v_id_pessoa + "'>" + data.Atendentes[i].v_nome_pessoa + "</option>";
                    }
                    $("#DLAtendente").append(list);
                },
                error: function () {
                    LoadDistribuidorErro();
                }
            });
        },
        error: function () {
            LoadDistribuidorErro();
        }
    });
}

function LoadDistribuidorErro() {
    $.ajax({
        url: '/Reserva/LoadDistribuidor',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var list = "";
            // alert(data);
            console.log(data);
            var atendente;
            for (var i = 0; i < data.length; i++) {
                //list += "<option dt='teste' value='" + data[i].v_id_pessoa + "'>" + data[i].v_nome_pessoa + "</option>";
                list += data[i].v_nome_pessoa;
                atendente = data[i].v_id_pessoa;
            }
            $("#DLStates").append(list);
            $('#DLAtendente').empty();
            $.ajax({
                url: '/Reserva/LoadAtendente',
                type: 'POST',
                data: JSON.stringify({ idpessoa: atendente }),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var list = " <option>Selecione</option>";
                    for (var i = 0; i < data.Atendentes.length; i++) {
                        list += "<option dt='teste' value='" + data.Atendentes[i].v_id_pessoa + "'>" + data.Atendentes[i].v_nome_pessoa + "</option>";
                    }
                    $("#DLAtendente").append(list);
                },
                error: function () {
                    alert("Problema ao carregar atendente.");
                }
            });
        },
        error: function () {
            alert("Problema ao carregar distribuidor.s");
        }
    });
}

function validate(comp) {
    if (comp.value == '') {
        $('#' + comp.id).addClass('warning');
    }
    else {
        $('#' + comp.id).removeClass('warning');
    }
}

function ckBox(comp) {
    //if ($("#" + comp.id).attr('checked') == "checked") {
    //    //$("#" + comp.id).attr('checked', null);
    //}
    //else {
    //    $("#" + comp.id).attr('checked', 'true');
    //}
}

function validateFields(validarPagamento) {
    var valido = true;
    var Quartos = $(".validar");
    for (var i = 0; i < Quartos.length ; i++) {
        if (Quartos[i].value == '') {
            $('#' + Quartos[i].id).addClass('warning');
            valido = false;
        }
        else {
            $('#' + Quartos[i].id).removeClass('warning');
        }
    }

    var valido = names();


    var validoIdade = true;
    var validaDataValida = true;

    var datas = $(".IdadeCrianca");
    for (var i = 0; i < datas.length ; i++) {
        if (datas[i].value.indexOf("/") != 2) {
            validaDataValida = false;
        }
    }
    if (!validaDataValida) {
        alert('Informe uma data válida!');
        valido = false;
    } else {
        for (var i = 0; i < datas.length ; i++) {
            if (ValidaDataInfantChd(datas[i].value, datas[i].id)) {
                validoIdade = false;
            }
        }
    }

    if (!valido) {
        alert('Verifique a idade das crianças!');
    }

    if (!validoIdade) {
        alert('Favor informar todos os campos obrigatório!');
    }
    if (!validoIdade)
        valido = false;

    if (validarPagamento && !$('#qtdPagamentos').val()) {
        valido = false;
        alert('Favor informar os dados do pagamento!');
    }
    if (validarValorTotal()) {
        valido = false;
        alert('O Valor total do pagamento diverge do valor total da venda!');
    }
    if (validarCondutor()) {
        valido = false;
        alert('Favor selecione um condutor!');
    }
    if ($("#ckLiAceito").attr('checked') != "checked") {
        valido = false;
        alert('Favor ler e aceitar as Condições Gerais!');
    }
    if (valido) {
        $('.loadingDefault').fadeIn();
    }


    return valido;
}


function names() {
    var words = new Array();
    words[0] = "TESTE";
    words[1] = "PRUEBA";
    words[2] = "TEST";
    words[3] = "TST";
    words[4] = "TSTS";
    words[5] = "FLY";
    words[6] = "FLYTOUR";
    words[7] = "SUPORTE";
    words[8] = "TESTES";
    words[9] = "PASSAGEIRO";
    words[10] = "PASSAGEIROS";
    words[11] = "RESERVA";
    words[12] = "NOME";
    words[13] = "DEFINIR";
    words[14] = "INDEFINIDO";

    var counter = 0;

    for (var i = 0; i < $(".validname").length; i++) {
        $($(".validname")[i]).val(removeAcento($(".validname")[i].value));
        for (var j = 0; j < words.length; j++) {
            if ($(".validname")[i].value.toUpperCase() == words[j]) {
                $($(".validname")[i]).addClass("warning");
                counter++;
            }
        }
    }

    if (counter > 0) {
        alert("Informe um nome válido.");
        return false;
    }
    else {
        return true;
    }
}

function validarNumTxt(dom, tipo) {
    switch (tipo) {
        case 'num': var regex = /[A-Za-z]/g; break;
        case 'text': var regex = /\d/g; break;
    }
    dom.value = dom.value.replace(regex, '');
}

function removeAcento(strToReplace) {
    str_acento = "áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ";
    str_sem_acento = "aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC";
    var nova = "";
    for (var i = 0; i < strToReplace.length; i++) {
        if (str_acento.indexOf(strToReplace.charAt(i)) != -1) {
            nova += str_sem_acento.substr(str_acento.search(strToReplace.substr(i, 1)), 1);
        } else {
            nova += strToReplace.substr(i, 1);
        }
    }
    return nova;
}

function validarCondutor() {

    if ($('.condutor').length == 0)
        return false;

    var valido = true;
    for (var i = 0; i < $('.condutor').length; i++) {
        if ($('.condutor')[i].checked)
            valido = false;
    }
    return valido;

}

function validarValorTotal() {

    var valorTotal = 0;
    for (var i = 0; i < $('.valorPagamento').length; i++) {
        valorTotal += parseFloat($('.valorPagamento')[i].value.replace('.', '').replace(',', '.'));
    }
    if (parseFloat($('#hdnValorTotal').val().replace('.', '').replace(',', '.')) != valorTotal)
        return true;
    else
        return false;
}

function validateFieldsBtoB() {
    var valido = true;
    var Quartos = $(".validar");
    for (var i = 0; i < Quartos.length ; i++) {
        if (Quartos[i].value == '') {
            $('#' + Quartos[i].id).addClass('warning');
            valido = false;
        }
        else {
            $('#' + Quartos[i].id).removeClass('warning');
        }
    }

    var valido = names();

    var validoIdade = true;
    var validaDataValida = true;

    var datas = $(".IdadeCrianca");
    for (var i = 0; i < datas.length ; i++) {
        if (datas[i].value.indexOf("/") != 2) {
            validaDataValida = false;
        }
    }
    if (!validaDataValida) {
        alert('Informe uma data válida!');
        valido = false;
    } else {
        for (var i = 0; i < datas.length ; i++) {
            if (ValidaDataInfantChd(datas[i].value, datas[i].id)) {
                validoIdade = false;
            }
        }
    }

    var datasValidas = $(".DataNascimento");

    for (var j = 0; j < datasValidas.length; j++) {
        if (datasValidas[0].value == "" || datasValidas[0].value == null) {
            valido = false;
            alert('Verifique as datas de nascimento!');
        }
    }

    if (!validoIdade) {
        alert('Verifique a idade das crianças!');
    }
    if (!valido) {
        alert('Favor informar todos os campos obrigatório!');
    }
    if (!validoIdade)
        valido = false;
    if ($("#ckLiAceito").attr('checked') != "checked") {
        valido = false;
        alert('Favor ler e aceitar as Condições Gerais');
    }
    if (validarCondutor()) {
        valido = false;
        alert('Favor selecione um condutor!');
    }
    if (valido) {
        $('.loadingDefault').fadeIn();
    }
    return valido;
}

$(document).ready(function () {
    $(".DataNascimento").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
});

function DatePickerNew(CounterPax) {
    //$('#txtNome' + CounterPax).filter_input({ regex: '[a-zA-Z]' });
    //$('#txtSobrenome' + CounterPax).filter_input({ regex: '[a-zA-Z]' });
    //$('#txtCPF' + CounterPax).mask("999.999.999-99");
    //$('#txtDataNascimento' + CounterPax).mask("99/99/9999");
    //$('#txtValidade' + CounterPax).mask("99/99/9999");
    //$('#txtRg' + CounterPax).mask("99.999.999-9");
}

function CarregarCarrinho() {
    $("#gridCarrinho").show();
    $("#gridCarrinho").load("Carrinho/LoadCarrinho", {
    });
    $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', width: '400px' }).css("font-size", "8pt");
}

function CarregaTotalCarrinho() {

    $("#lblTotal").text("0");

    if ($("#lblTotAereo").text() != "") {
        $("#lblTotal").text(parseFloat($("#lblTotal").text().replace(/,/g, '.')) + parseFloat($("#lblTotAereo").text().replace(/,/g, '.')));
    }

    if ($("#lblTotCarro").text() != "") {
        $("#lblTotal").text(parseFloat($("#lblTotal").text().replace(/,/g, '.')) + parseFloat($("#lblTotCarro").text().replace(/,/g, '.')));
    }

    if ($("#lblTotPacote").text() != "") {
        $("#lblTotal").text(parseFloat($("#lblTotal").text().replace(/,/g, '.')) + parseFloat($("#lblTotPacote").text().replace(/,/g, '.')));
    }

    if ($("#lblTotHotel").text() != "") {
        $("#lblTotal").text(parseFloat($("#lblTotal").text().replace(/,/g, '.')) + parseFloat($("#lblTotHotel").text().replace(/,/g, '.')));
    }
}

function Registrar(flytour) {
    var args = new Object();
    args = flytour;
    $.ajax({
        url: '/Reserva/RegistrarReserva',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert("Reserva realizada com Sucesso ! - CODIGO" + JSON.stringify(data));
        },
        error: function () {
            alert("Problema ao registrar.");
        }
    });
}

function ValidaDataInfantChd(data, id) {
    var invalido = false;
    var ids = (id).substr(17, 5);
    var idade = $('#pax-' + ids).val();

    if (data.length != 10) {
        $('#lblDataInvalida-' + ids).css('display', 'block');
        $('#' + id).addClass('warning');
        invalido = true;
    } else {

        $('#lblDataInvalida-' + ids).css('display', 'none');
        $('#' + id).removeClass('warning');



        var args = new Object();
        args = { data: data, idade: idade };

        $.ajax({
            url: '/Reserva/ValidaDataInfantChd',
            type: 'POST',
            async: false,
            data: JSON.stringify(args),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (!data) {
                    $('#lblDataInvalida-' + ids).css('display', 'block');
                    $('#' + id).addClass('warning');
                    invalido = true;
                }
                else {
                    $('#lblDataInvalida-' + ids).css('display', 'none');
                    $('#' + id).removeClass('warning');
                }
            },
            error: function () {
                //alert('data invalida');
            }
        });
    }
    return invalido;
}

function CarregarPagamentos() {
    $("#gridPagamento").show();
    $("#gridPagamento").load("Pagamento/LoadPagamentos", {
        ValorTotal: $("#hdnTotal").val()
    });
}

$(document).ready(function () {
    CarregarCarrinho();
    LoadDistribuidor();
    adicionarCartao();
    $('.DataNascimento').change(function () {
        ValidaDataInfantChd($(this).val(), this.id);
    });

});

function CondutorCheckado(comp) {

}

function removerCartao() {
    var quant = $("#cartoesAdicionais li.liAdd").length;
    $('#li-' + quant).remove();
    quant--;
    for (var i = 1; i < quant + 1; i++) {
        atualizarModalidade(quant, i, null);
    }
}

function verCondicoes() {
    $("#div_condicoes").modal();
    $("#div_condicoes").attr('display', 'block');
}

function adicionarCartao() {

    var quant = $("#cartoesAdicionais li.liAdd").length + 1;
    var args = new Object();

    if (quant > 4)
        $('#divAddCartao').fadeOut();

    args = {
        indice: quant,
    };

    $.ajax({
        async: false,
        url: '/Pagamento/LoadPagamentos',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#cartoesAdicionais").append(data);
            for (var i = 1; i < quant + 1; i++) {
                atualizarModalidade(quant, i, null);
            }
            $('#valor-' + (quant)).val($('#hdnValDif-' + (quant)).val());
        },
        error: function (data) {
            //alert(data);
        }
    });
    $('#Divtitulo').attr('style', 'display:block');

    //$('#trechosAdicionais').append('<div class="BuscadorSet">            <div>                <label class="buscadorlabel">Origem</label>                <div id="divOrigem">                    <span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span><input class="watermark ui-autocomplete-input marked" data-val="true" data-val-autocompletevalidation="Campo obrigatório" data-val-autocompletevalidation-nomediv="divOrigem" data-val-autocompletevalidation-textopradao="Informe seu destino" id="Origem" name="Origem" title="Informe sua origem" type="text" value="" autocomplete="off"></input><br>                    <span class="field-validation-valid" data-valmsg-for="Origem" data-valmsg-replace="true"></span>                </div>            </div>            <label class="buscadorlabel">Destino</label>            <div id="divDestino">                <span role="status" aria-live="polite" class="ui-helper-hidden-accessible"></span><input class="watermark marked ui-autocomplete-input" data-val="true" data-val-autocompletevalidation="Campo obrigatório" data-val-autocompletevalidation-nomediv="divDestino" data-val-autocompletevalidation-textopradao="Informe seu destino" id="Destino" name="Destino" title="Informe seu destino" type="text" value="" autocomplete="off"></input><br>                <span class="field-validation-valid" data-valmsg-for="Destino" data-valmsg-replace="true"></span>            </div>        </div> <span class="addTrecho" onclick="Trecho()">-remover trecho</span>');
}

function atualizarModalidade(quant, i, valorOper) {

    argsMod = {
        indice: quant,
        id: i,
        valorOper: valorOper
    };
    $('.loadingPartial').show();
    $.ajax({
        url: '/Pagamento/Modalidade',
        type: 'POST',
        data: JSON.stringify(argsMod),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#modalidade-" + (i)).html(data);
            $('.loadingPartial').hide();
            $('.ui-tabs-active').css('border', 'none');
        },
        error: function (data) {
            //alert(data);
        }
    });

}


function selectPlano(idx, codigo, data) {
    $('#qtdPagamentos').val($("#cartoesAdicionais li.liAdd").length);
    $('#hdnData-' + idx).val(codigo.split('-')[1]);
    $('#CodigoPlanoParcela-' + idx).val(codigo.split('-')[0]);
}
