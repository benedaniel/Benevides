$.validator.addMethod("datacheckin", function (value, element, params) {

    var dataSeparada = value.split("/");

    var dataFormatada = new Date(dataSeparada[2], dataSeparada[1] - 1, dataSeparada[0]);

    if (!/Invalid|NaN/.test(dataFormatada)) {

        var dataAtual = new Date();

        var comparacao = dataFormatada > dataAtual;

        return comparacao;
    }
    else {
        return false;
    }
});
$.validator.unobtrusive.adapters.add("datacheckin", ["field1"], function (options) {
    var params = {
        field1: options.params.field1
    };

    options.rules["datacheckin"] = params;

    if (options.message) {
        options.messages['datacheckin'] = options.message;
    }
});

$.validator.addMethod("datacheckout", function (value, element, params) {

    var dataCheckIn = $(params).val();
    var dataSeparadaCheckin = dataCheckIn.split("/");
    var dataCheckinFormatada = new Date(dataSeparadaCheckin[2], dataSeparadaCheckin[1] - 1, dataSeparadaCheckin[0]);
    var dataSeparadaCheckout = value.split("/");
    var dataCheckoutFormatada = new Date(dataSeparadaCheckout[2], dataSeparadaCheckout[1] - 1, dataSeparadaCheckout[0]);

    return new Date(dataCheckoutFormatada) > new Date(dataCheckinFormatada);
});
$.validator.unobtrusive.adapters.add("datacheckout", ["otherpropertyname"], function (options) {
    options.rules["datacheckout"] = "#" + options.params.otherpropertyname;
    options.messages["datacheckout"] = options.message;
});

$.validator.addMethod("datacheckoutaereo", function (value, element, params) {
    var selectedVal = $("#radio2:checked").val();
    if (selectedVal == "IdaeVolta") {

        var dataCheckIn = $(params).val();
        var dataSeparadaCheckin = dataCheckIn.split("/");
        var dataCheckinFormatada = new Date(dataSeparadaCheckin[2], dataSeparadaCheckin[1] - 1, dataSeparadaCheckin[0]);

        var dataSeparadaCheckout = value.split("/");

        var dataCheckoutFormatada = new Date(dataSeparadaCheckout[2], dataSeparadaCheckout[1] - 1, dataSeparadaCheckout[0]);

        return new Date(dataCheckoutFormatada) > new Date(dataCheckinFormatada);
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.add("datacheckoutaereo", ["otherpropertyname"], function (options) {
    options.rules["datacheckoutaereo"] = "#" + options.params.otherpropertyname;
    options.messages["datacheckoutaereo"] = options.message;
});

$.validator.addMethod("localwatermark", function (value, element, params) {
    if (value == params || value == "") {
        return false;
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.add("localwatermark", ["param"], function (options) {
    options.rules["localwatermark"] = options.params.param;
    options.messages["localwatermark"] = options.message;
});

$.validator.addMethod("autocompletevalidation", function (value, element, params) {

    var nomeDivConcatenado = "#txt" + params.nomediv;

    if (value == params.textopradao || value == "") {
        return false;
    }
    else {
        return true;
    }
});
$.validator.unobtrusive.adapters.add("autocompletevalidation", ['textopradao', 'nomediv'], function (options) {

    var textopradao = options.textopradao;
    var nomediv = options.nomediv;

    options.rules["autocompletevalidation"] = options.params;
    options.messages["autocompletevalidation"] = options.message;
});

$.validator.addMethod("autocompletedependcheckbox", function (value, element, params) {
    if (!$("#" + params.idcheckbox).prop('checked')) {
        return true;
    }
    else {
        if (value == params.textopadrao || value == "") {
            return false;
        }
    }

    return true;
});
$.validator.unobtrusive.adapters.add("autocompletedependcheckbox", ['idcheckbox', 'textopadrao'], function (options) {

    options.rules["autocompletedependcheckbox"] = options.params;
    options.messages["autocompletedependcheckbox"] = options.message;
});

$.validator.addMethod("requireddependcombobox", function (value, element, params) {
    if ($("#" + params.idcombobox).val() == params.valorparametrovalidacao) {
        if (value == "") {
            return false;
        }
    }

    return true;
});
$.validator.unobtrusive.adapters.add("requireddependcombobox", ['idcombobox', 'valorparametrovalidacao'], function (options) {

    options.rules["requireddependcombobox"] = options.params;
    options.messages["requireddependcombobox"] = options.message;
});


$.validator.addMethod("requiredflytour", function (value, element, params) {
    if (value == "") {
        return false;
    }

    return true;
});
$.validator.unobtrusive.adapters.add("requiredflytour", [''], function (options) {

    options.rules["requiredflytour"] = options.params;
    options.messages["requiredflytour"] = options.message;
});


// JavaScript Document
//adiciona mascara de cnpj
function MascaraCNPJ(cnpj, event) {

    return formataCampo(cnpj, '00.000.000/0000-00', event);
}

//adicona mascara de cpf e cnpj
function MascaraCpfCnpj(cpfcnpj, event) {
    exp = /\-|\.|\/|\(|\)| /g
    campoSoNumeros = $(cpfcnpj).val().toString().replace(exp, "");
    if (campoSoNumeros.length <= 11) {
        return formataCampo(cpfcnpj, '000.000.000-00', event);
    } else {
        return formataCampo(cpfcnpj, '00.000.000/0000-00', event);
    }
}

//adiciona mascara de cep
function MascaraCep(cep, event) {

    return formataCampo(cep, '00000-000', event);
}

//adiciona mascara de hora
function MascaraHora(hora, event) {
    return formataCampo(hora, '00:00', event);
}

//adiciona mascara de data
function MascaraData(data, event) {

    return formataCampo(data, '00/00/0000', event);
}

//adiciona mascara ao telefone
function MascaraTelefone(tel, event) {

    return formataCampo(tel, '(00) 0000-0000', event);
}

//adiciona mascara ao telefone Celular
function MascaraTelefoneCelular(tel, event) {

    return formataCampo(tel, '(00) 00000-0000', event);
}

//adiciona mascara ao CPF
function MascaraCPF(cpf, event) {

    return formataCampo(cpf, '000.000.000-00', event);
}

//valida telefone
function ValidaTelefone(tel) {
    exp = /\(\d{2}\)\ \d{4}\-\d{4}/
    if (!exp.test(tel.value))
        alert('Numero de Telefone Invalido!');
}

//valida CEP
function ValidaCep(cep) {
    exp = /\d{2}\.\d{3}\-\d{3}/
    if (!exp.test(cep.value))
        alert('Numero de Cep Invalido!');
}

//valida o CPF digitado
function ValidarCPF(Objcpf) {
    var cpf = Objcpf.value;
    exp = /\.|\-/g
    cpf = cpf.toString().replace(exp, "");
    var digitoDigitado = eval(cpf.charAt(9) + cpf.charAt(10));
    var soma1 = 0, soma2 = 0;
    var vlr = 11;

    for (i = 0; i < 9; i++) {
        soma1 += eval(cpf.charAt(i) * (vlr - 1));
        soma2 += eval(cpf.charAt(i) * vlr);
        vlr--;
    }
    soma1 = (((soma1 * 10) % 11) == 10 ? 0 : ((soma1 * 10) % 11));
    soma2 = (((soma2 + (2 * soma1)) * 10) % 11);

    var digitoGerado = (soma1 * 10) + soma2;
    if (digitoGerado != digitoDigitado)
        alert('CPF Invalido!');
}

//valida numero inteiro com mascara
function mascaraInteiro(event) {
    if (event.keyCode < 48 || event.keyCode > 57) {
        event.preventDefault();
        return false;
    }
    return true;
}

//valida o CNPJ digitado
function ValidarCNPJ(ObjCnpj) {
    var cnpj = ObjCnpj.value;
    var valida = new Array(6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2);
    var dig1 = new Number;
    var dig2 = new Number;

    exp = /\.|\-|\//g
    cnpj = cnpj.toString().replace(exp, "");
    var digito = new Number(eval(cnpj.charAt(12) + cnpj.charAt(13)));

    for (i = 0; i < valida.length; i++) {
        dig1 += (i > 0 ? (cnpj.charAt(i - 1) * valida[i]) : 0);
        dig2 += cnpj.charAt(i) * valida[i];
    }
    dig1 = (((dig1 % 11) < 2) ? 0 : (11 - (dig1 % 11)));
    dig2 = (((dig2 % 11) < 2) ? 0 : (11 - (dig2 % 11)));

    if (((dig1 * 10) + dig2) != digito)
        alert('CNPJ Invalido!');

}

//formata de forma generica os campos
function formataCampo(campo, Mascara, evento) {
    var boleanoMascara;
    var Digitato;
    if (evento == null) {
        Digitato = 9;
    }
    else {
        Digitato = evento.keyCode;
    }
    exp = /\:|\-|\.|\/|\(|\)| /g
    campoSoNumeros = $(campo).val().toString().replace(exp, "");

    var indexMascara = 0;
    var isSetMascara = false;
    var NovoValorCampo = "";
    var TamanhoMascara = campoSoNumeros.length;

    if (TamanhoMascara != 0) { 
        for (i = 0; i <= TamanhoMascara-1; i++) {

            if (i == Mascara.split("0").length-1)
                break;

            boleanoMascara = ((Mascara.charAt(indexMascara) == ":") || (Mascara.charAt(indexMascara) == "-") || (Mascara.charAt(indexMascara) == ".") || (Mascara.charAt(indexMascara) == "/"))
                            || ((Mascara.charAt(indexMascara) == "(") || (Mascara.charAt(indexMascara) == ")") || (Mascara.charAt(indexMascara) == " "));

            if (boleanoMascara) {
                NovoValorCampo += Mascara.charAt(indexMascara);
                    i--;
               
            } else {
                NovoValorCampo += campoSoNumeros.charAt(i);
            }
            indexMascara++;
        }
        $(campo).val(NovoValorCampo);
        return true;
    } else {
        return true;
    }
}


function validaForm(nameObjeto) {
    $(nameObjeto).validate();

    if ($(nameObjeto).valid()) {
        return true;
    }
    else {
        return false;
    }
}

function checkMail(mail) {
    var er = new RegExp(/^[A-Za-z0-9_\-\.]+@[A-Za-z0-9_\-\.]{2,}\.[A-Za-z0-9]{2,}(\.[A-Za-z0-9])?/);
    if (typeof (mail) == "string") {
        if (er.test(mail)) { return true; }
    } else if (typeof (mail) == "object") {
        if (er.test(mail.value)) {
            return true;
        }
    } else {
        return false;
    }
}

///recebe como parametro o nome do Id (com '#' no começo) ou o nome da class (com '.' no começo)e retorna true ou false para a validação da data.
function validaData(data) {
    var patternValidaData = /^(((0[1-9]|[12][0-9]|3[01])([-.\/])(0[13578]|10|12)([-.\/])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-.\/])(0[469]|11)([-.\/])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-.\/])(02)([-.\/])(\d{4}))|((29)(\.|-|\/)(02)([-.\/])([02468][048]00))|((29)([-.\/])(02)([-.\/])([13579][26]00))|((29)([-.\/])(02)([-.\/])([0-9][0-9][0][48]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][2468][048]))|((29)([-.\/])(02)([-.\/])([0-9][0-9][13579][26])))$/;
    var result = patternValidaData.test($(data).val());
    return result;
}

function numeros(ie, ff) {
    if (ie) {
        tecla = ie;
    } else {
        tecla = ff;
    }

    /**
    * 13 = [ENTER]
    * 8  = [BackSpace]
    * 9  = [TAB]
    * 16 = [SHIFT]
    * 46 = [Delete]
    * 48 a 57 = São os números
    * 35 = [END]
    * 36 = [HOME]
    * 37 = [SETA-ESQUERDA]
    * 39 = [SETA-DIREITA]
    */
    if (tecla == 0 || tecla == 9 || tecla == 8 || tecla == 16 || (tecla > 47 && tecla < 58) || (tecla > 95 && tecla < 106) ||
        tecla == 35 || tecla == 36 || tecla == 37 || tecla == 39 || tecla == 46) {
        return true;
    }
    else {
        return false;
    }
}

function somenteLeitura(event) {
    if (event.keyCode == 9) {
        return true;
    }
    else {
        return false;
    }
}