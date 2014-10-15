
$(document).ready(function () {
    removeSomenteLeitura("#IN_NACIONAL");
    if ($("#formUsuario #ID_PESSOA").val() != "") {
        addSomenteLeitura("#NR_CPF_CNPJ");
        addSomenteLeitura("#USUARIO");

        MascaraTelefoneCelular("#NR_TELEFONE_CEL", null);
        MascaraTelefone("#NR_TELEFONE", null);
        validaCep();

        //if ($("#formUsuario #ID_PERFIL").val() == "") {
        //    $("#idperfils").hide();
        //    $("#btnSalvar").hide();
        //}

    }

    if ($("#IN_NACIONAL").val() != "") {
        controleCamposUsuarioNacional($("#IN_NACIONAL").val())
    }
    else {
        controleCamposUsuarioNacional("S");
    }
    isCNPJ();
});

function addSomenteLeitura(campo) {
    $(campo).addClass("ReadyOnly");

    $(campo).attr("onkeypress", "return somenteLeitura(event);");
}

function removeSomenteLeitura(campo) {
    $(campo).removeClass("ReadyOnly");
    $(campo).removeAttr("onkeypress");
}

function validaCampoEmail(mail) {
    if (!checkMail(mail)) {
        $("span[data-valmsg-for='USUARIO']span").html("<span> email inválido! </span>");
        $("span[data-valmsg-for='USUARIO']span").attr("class", "field-validation-error");
    }
    else {
        $("span[data-valmsg-for='USUARIO']span").html("");
        $("span[data-valmsg-for='USUARIO']span").attr("class", "field-validation-valid");
    }
}

function isCNPJ(ocultar) {
    exp = /\-|\.|\/|\(|\)| /g
    campoSoNumeros = $("#NR_CPF_CNPJ").val().toString().replace(exp, "");
    
    if (campoSoNumeros.length > 11) {
        $("#divDT_NASCIMENTO").hide();
        $("#divIN_SEXO").hide();
    }
    else {
        $("#divDT_NASCIMENTO").show();
        $("#divIN_SEXO").show();
    }
}

$(function () {
    $("#IN_NACIONAL").change(function () {
        if ($("#IN_NACIONAL").val() != "") {
            controleCamposUsuarioNacional($("#IN_NACIONAL").val());
        }
        else {
            controleCamposUsuarioNacional("S");
        }
    });
});

function controleCamposUsuarioNacional(S) {
    if (S == "S") {
        if ($("#formUsuario #ID_PESSOA").val() == "") {
            removeSomenteLeitura("#NR_CPF_CNPJ");
        }
        removeSomenteLeitura("#ENDERECO_NR_CEP");
        removeSomenteLeitura("#ENDERECO_NR_ENDERECO");
        removeSomenteLeitura("#ENDERECO_DS_COMPLEMENTO");

        addSomenteLeitura("#ENDERECO_DS_PAIS");
        addSomenteLeitura("#ENDERECO_DS_CIDADE");
        addSomenteLeitura("#ENDERECO_DS_BAIRRO");
        addSomenteLeitura("#ENDERECO_DS_ENDERECO");

        $("span[data-valmsg-for='ENDERECO.DS_ENDERECO']span").html("");
        $("span[data-valmsg-for='ENDERECO.DS_ENDERECO']span").attr("class", "field-validation-valid");
    }
    else {
        if ($("#formUsuario #ID_PESSOA").val() == "") {
            $("#NR_CPF_CNPJ").val("");
        }
        $("#ENDERECO_NR_CEP").val("");
        $("#ENDERECO_NR_ENDERECO").val("");
        $("#ENDERECO_DS_COMPLEMENTO").val("");

        if ($("#formUsuario #ID_PESSOA").val() == "") {
            addSomenteLeitura("#NR_CPF_CNPJ");
        }
        addSomenteLeitura("#ENDERECO_NR_CEP");
        addSomenteLeitura("#ENDERECO_NR_ENDERECO");
        addSomenteLeitura("#ENDERECO_DS_COMPLEMENTO");

        removeSomenteLeitura("#ENDERECO_DS_PAIS");
        removeSomenteLeitura("#ENDERECO_DS_CIDADE");
        removeSomenteLeitura("#ENDERECO_DS_BAIRRO");
        removeSomenteLeitura("#ENDERECO_DS_ENDERECO");

        $("span[data-valmsg-for='NR_CPF_CNPJ']span").html("");
        $("span[data-valmsg-for='NR_CPF_CNPJ']span").attr("class", "field-validation-valid");
        $("span[data-valmsg-for='ENDERECO.DS_ENDERECO']span").html("");
        $("span[data-valmsg-for='ENDERECO.DS_ENDERECO']span").attr("class", "field-validation-valid");
        $("span[data-valmsg-for='ENDERECO.NR_ENDERECO']span").html("");
        $("span[data-valmsg-for='ENDERECO.NR_ENDERECO']span").attr("class", "field-validation-valid");
        $("span[data-valmsg-for='ENDERECO.NR_CEP']span").html("");
        $("span[data-valmsg-for='ENDERECO.NR_CEP']span").attr("class", "field-validation-valid");
    }
}

function autoCompletePais(tipolista, nome) {
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
                $("#ENDERECO_DS_CIDADE").val('');
                $("#ENDERECO_DS_BAIRRO").val('');
                autoCompleteCidade(8, "ENDERECO_DS_CIDADE");
            }
            else {
                event.preventDefault();
                $(this).val('');
            }
        }
    });
}

function autoCompleteCidade(tipolista, nome) {

    $("#" + nome).append(
        " <input type='hidden' id='hdn" + nome + "Codigo' name='hdn" + nome + "Codigo' />" +
        " <input type='hidden' id='hdn" + nome + "Text' />" +
        " <input type='hidden' id='hdn" + nome + "Nome' name='hdn" + nome + "Nome' />"
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
    });

    $('#' + nome).autocomplete({
        source: "/AutoComplete/Repository.ashx?lista=" + tipolista + "&id=" + $("#hdnENDERECO_DS_PAISCodigo").val(),
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
            }
            else {
                event.preventDefault();
                $(this).val('');
            }
        }
    });
}

function consultaCep() {
    var args = new Object();
    args = { cep: $("#ENDERECO_NR_CEP").val() };

    $.ajax({
        url: '/PainelControle/ConsultaCep',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.Status == "S") {
                $("#ENDERECO_DS_ENDERECO").val(data.Descricao);
                $("#ENDERECO_DS_PAIS").val("Brasil");
                $("#ENDERECO_DS_CIDADE").val(data.Cidade);
                $("#ENDERECO_DS_BAIRRO").val(data.Bairro);

                addSomenteLeitura("#ENDERECO_DS_PAIS");
                addSomenteLeitura("#ENDERECO_DS_CIDADE");
                addSomenteLeitura("#ENDERECO_DS_BAIRRO");

                $("#ENDERECO_NR_ENDERECO").focus();

                $("span[data-valmsg-for='ENDERECO.NR_CEP']span").html("");
                $("span[data-valmsg-for='ENDERECO.NR_CEP']span").attr("class", "field-validation-valid");
            }
            else {
                $("span[data-valmsg-for='ENDERECO.NR_CEP']span").html("<span>CEP não encontrado</span>");
                $("span[data-valmsg-for='ENDERECO.NR_CEP']span").attr("class", "field-validation-error");
            }

        },
        error: function () {
            alert(data);
        }
    });
}

//$(function () {

//    $("#ENDERECO_NR_CEP").blur(function () {
//        consultaCep()

//    });
//});



$(function () {
    $("#ENDERECO_NR_CEP").keyup(function () {
        validaCep();
    })
});

function validaCep() {
    var cep = $("#ENDERECO_NR_CEP").val().replace('-', '').length;

    if (cep > 8) {
        $("#ENDERECO_NR_CEP").val($("#ENDERECO_NR_CEP").val().substring(0, 9));
        return;
    }
    if (cep == 8) {
        consultaCep();

    } else if (cep >= 1) {
        addSomenteLeitura("#ENDERECO_DS_PAIS");
        addSomenteLeitura("#ENDERECO_DS_CIDADE");
        addSomenteLeitura("#ENDERECO_DS_BAIRRO");

        $("#ENDERECO_DS_ENDERECO").val("");
        $("#ENDERECO_NR_ENDERECO").val("");
        $("#ENDERECO_DS_COMPLEMENTO").val("");
        $("#ENDERECO_DS_PAIS").val("");
        $("#ENDERECO_DS_CIDADE").val("");
        $("#ENDERECO_DS_BAIRRO").val("");

        $("span[data-valmsg-for='ENDERECO.NR_CEP']span").html("");
        $("span[data-valmsg-for='ENDERECO.NR_CEP']span").attr("class", "field-validation-valid");
    }
    //else {
    //    removeSomenteLeitura("#ENDERECO_DS_PAIS");
    //    removeSomenteLeitura("#ENDERECO_DS_CIDADE");
    //    removeSomenteLeitura("#ENDERECO_DS_BAIRRO");
    //}
}

function validaCampoSenha()
{
    if ($("#formUsuario #ID_PERFIL").val() == "" && ($("#REDEFINIR_SENHA").val() == "" || $("#SENHA").val() == "" )) {
        $("span[data-valmsg-for='REDEFINIR_SENHA']span").html("<span> Campo Obrigatório! </span>");
        $("span[data-valmsg-for='REDEFINIR_SENHA']").attr("class", "field-validation-error");

        $("span[data-valmsg-for='SENHA']span").html("<span> Campo Obrigatório! </span>");
        $("span[data-valmsg-for='SENHA']").attr("class", "field-validation-error");
        return false;
        
    }
    else {
        $("span[data-valmsg-for='REDEFINIR_SENHA']span").html("");
        $("span[data-valmsg-for='REDEFINIR_SENHA']").attr("class", "field-validation-valid");

        $("span[data-valmsg-for='SENHA']span").html("");
        $("span[data-valmsg-for='SENHA']").attr("class", "field-validation-valid");
        return true;
    }
}

function validaForm() {
    var senhaValido = validaCampoSenha();
    var dataValida = validaDataNascimento();
    $("#formUsuario").validate();

    if ($("#formUsuario").valid() && senhaValido && dataValida) {
        return true;
    }
    else {
        return false;
    }
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

function validaDataNascimento() {
    if (validaData("#DT_NASCIMENTO")) {
        $("span[data-valmsg-for='DT_NASCIMENTO']span").html("");
        $("span[data-valmsg-for='DT_NASCIMENTO']").attr("class", "field-validation-valid");
        return true;
    }
    else {
        $("span[data-valmsg-for='DT_NASCIMENTO']span").html("<span> Data inválida! </span>");
        $("span[data-valmsg-for='DT_NASCIMENTO']").attr("class", "field-validation-error");
        return false;
    }
}