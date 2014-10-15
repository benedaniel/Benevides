$(document).ready(function () {
    try {
        document.body.style.zoom = 1.0;
    } catch (e) {

    }


});


$(function () {
    $('#login').click(function () {
        var checklogin = $('#hdLogin').val();
        if (checklogin == "0") {
            openlogin();
        }
        else {
            fecharlogin();
        }
    });

    try {
        document.body.style.zoom = 1.0;
    } catch (e) {

    }

    $.fn.imagesLoaded = function (callback) {
        var elems = this.filter('img'),
            len = elems.length,
            blank = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";

        elems.bind('load.imgloaded', function () {
            if (--len <= 0 && this.src !== blank) {
                elems.unbind('load.imgloaded');
                //callback.call(elems, this);
            }
        }).each(function () {
            // cached images don't fire load sometimes, so we reset src.
            if (this.complete || this.complete === undefined) {
                var src = this.src;
                // webkit hack from http://groups.google.com/group/jquery-dev/browse_thread/thread/eee6ab7b2da50e1f
                // data uri bypasses webkit log warning (thx doug jones)
                this.src = blank;
                this.src = src;
            }
        });

        return this;
    };

});


function validarDataRetorno(DataEmbarque, DataRetorno) {
    if (!validarData(DataEmbarque, DataRetorno)) {
        $("span[data-valmsg-for='" + DataRetorno + "']").html("<span>Data de retorno não pode ser igual ou inferior a data de embarque</span>");
        $("span[data-valmsg-for='" + DataRetorno + "']").attr("class", "field-validation-error");
    }
    else {
        $("span[data-valmsg-for='" + DataRetorno + "']").html("");
        $("span[data-valmsg-for='" + DataRetorno + "']").attr("class", "field-validation-valid");
    }
}

function validarData(DataEmbarque, DataRetorno) {
    var dateEmbarque = $('#' + DataEmbarque).datepicker('getDate');
    var dateRetorno = $('#' + DataRetorno).datepicker('getDate');
    if (dateEmbarque > dateRetorno)
        return false;
    else
        return true;
}


$(function () {
    $('#logout').click(function () {
        if ($('#hdlogout').val() == 0) {
            $('.configFullTemplate').fadeIn(500);
            $('#hdlogout').val(1);
        }
        else {
            $('.configFullTemplate').fadeOut(500);
            $('#hdlogout').val(0);
        }
    });
    $('.watermark').watermark();
});

function openlogin() {
    $('.login').css("background", "url(../Images/top_login_des.gif) no-repeat top #EA6209");
    $("#login").animate({
        height: "42px"
    }, 'medium', function () {
        // Animation complete.
    });
    $('.logincompleto').load('/Autenticar/_logar');
    $('.logincompleto').fadeIn(500);
    $('#hdLogin').val("1");
}

function alterarloading(produto, tipo) {
    var args = new Object();
    args = { produto: produto, tipo: tipo };
    $.ajax({
        async: false,
        url: '/Home/AlterarLoading',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            //if (data != null && data != '') {
            //    $("#imgload").one("load", function () {
            //        // image loaded here
            //    }).attr("src", data);
            //}
            //$('.loadingDefault').show();
        },
        error: function (data) {
        }
    });
}


function limparCarrinho() {

    $.ajax({
        url: '/Carrinho/LimparCarrinho',
        type: 'POST',
        data: null,
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#CarrinhoTotal').html(data);

        },
        error: function () {
            alert("erro limpando o carrinho");
        }
    });
    alert('Limpo com sucesso!');
}

function AtualizarCarrinho() {
    $.ajax({
        async: false,
        url: '/Carrinho/AtualizarCarrinho',
        type: 'POST',
        data: null,
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#CarrinhoTotal').html(data);
            $('html, body').animate({ scrollTop: 0 }, 'slow');
            $('#seta').fadeIn();
            $('#seta').fadeOut(9000);
        },
        error: function (data) {
            alert("Erro Atualizando o carrinho");
        }
    });
}

function finalizarCompra(somenteCarrinho) {
    args = {
        SomenteCarrinho: somenteCarrinho,
    };

    $.ajax({
        async: false,
        url: '/Carrinho/FinalizarCompra',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            window.location = '/Reserva';
        },
        error: function (data) {
            alert("Erro ao finalizar a Compra!");
        }
    });
}

function removerItemCarrinho(tipo, codigo) {

    args = {
        tipoItem: tipo,
        codigoItem: codigo,
    };

    $.ajax({
        async: false,
        url: '/Carrinho/removerItemCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert('Removido com sucesso!');
            $('#CarrinhoTotal').html(data);
        },
        error: function (data) {
            alert("Erro removendo o item do carrinho");
        }
    });
}

function fecharlogin() {
    $('.login').css("background", "url(../Images/top_login.gif) no-repeat top #EA6209");
    $("#login").animate({
        height: "27px"
    }, 'fast', function () {
        // Animation complete.
    });
    $('.logincompleto').fadeOut(300);
    $('#hdLogin').val(0);
}

function autoComplete(tipolista, nome) {
    $("#" + nome).append(
        //"<input type='text' id='txt" + nome + "' name='txt" + nome + "' style='width:100%;' autocomplete='off' data-val-length-max='200' data-val-autocompletevalidation='Campo obrigatório' data-val='true' data-val-autocompletevalidation-nomediv='HotelDestino' data-val-autocompletevalidation-textopradao='Informe seu destino' /> " +
        //"<span class='field-validation-error' data-valmsg-for='txt" + nome + "' data-valmsg-replace='true'>" +
        //"<span for='txt" + nome + "' generated='true' class='field-validation-error'></span> " + 
        //"</span>" +
        " <input type='hidden' id='hdn" + nome + "Codigo' name='hdn" + nome + "Codigo' /><input type='hidden' id='hdn" + nome + "Text' />" +
        "<input type='hidden' id='hdn" + nome + "Nome' name='hdn" + nome + "Nome' />"
      );

    if ($('#hdn' + nome + 'Nome').val() != '') {
        $(this).val($('#hdn' + nome + 'Nome').val());
    }

    if ($('#hdn' + nome + 'Nome').val() == '') {
        if ($('#' + nome).val() != '') {
            $('#hdn' + nome + 'Nome').val($('#' + nome).val());
        }
    }

    $('#' + nome).focusout(function () {
        $('#' + nome).keyup();
    });



    $('#' + nome).keyup(function () {
        if ($(this).val() == '') {
            $('#hdn' + nome + 'Codigo').val('');
            $('#hdn' + nome + 'Nome').val('');
            $('#hdn' + nome + 'Text').val('');
            $(this).removeClass('working');
        }
        //else if ($(this).val().length <= 2) {
        else if ($(this).val() != $('#hdn' + nome + 'Nome').val()) {
            $('#hdn' + nome + 'Codigo').val('');
            $('#hdn' + nome + 'Nome').val('');
            $('#hdn' + nome + 'Text').val('');
        }
    });

    $('#' + nome).focus(function () {

        //alert(this.id + ":" + $(this).val() + "\r\n\r\n" + '#hdn' + nome + 'Nome' + ":" + $('#hdn' + nome + 'Nome').val());

        if (!(/^informe/).test($(this).val().toLowerCase())) {
            if ($(this).val() != $('#hdn' + nome + 'Nome').val()) {
                $(this).autocomplete("search");
            }
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
            }
            else {
                event.preventDefault();
                $(this).val('');
            }
        }
    });
}


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function isLessThanZero(input) {
    if (input.value < 1)
        input.value = 1;
}

function rolarScrollHorDir(objScroll, qtdPixel) {
    $(objScroll).scrollLeft($(objScroll).scrollLeft() + qtdPixel);
}

function rolarScrollHorEsq(objScroll, qtdPixel) {
    $(objScroll).scrollLeft($(objScroll).scrollLeft() - qtdPixel);
}

function popUpNewWindow(url, largura, altura) {
    window.open(url, 'Pagina', 'STATUS=NO, TOOLBAR=NO, LOCATION=NO, DIRECTORIES=NO, RESISABLE=NO, SCROLLBARS=YES, TOP=10, LEFT=10, WIDTH=' + largura + ', HEIGHT=' + altura + '');
}

function showdivLoad() {
    $(".loading").show();
}

function hidedivLoad() {
    $(".loading").hide();
}

//elis uma caixa de mensagem personalizada para passar informações decorrentes a ações na tela.
function showMensagem(titulo, texto, largura) {

    $(window).scroll(function () {
        $("#DivMsg").stop().animate({
            // basicamente é utilizado a posição do scroll vertical
            // para definir a posição da div atravéz da propriedade marginTop
            marginTop: $(window).scrollTop() - 100
        });
    });

    $("#DivBgMsg").show();
    $("#DivBgMsg").width($(window).width() - 2);
    $("#DivBgMsg").height($(window).height() - 2);

    $("#DivMsg").width(largura);
    $("#DivMsg").css("margin-left", (($(window).width() - largura) / 2) - $("#DivMsg").css("padding-left").replace("px", ""));

    $("#DivMsgInternaTitulo").html(titulo);
    $("#DivMsgInternaTexto").html("&nbsp;&nbsp;" + texto.replace("\r", "").replace("\n", "").replace(";", " </br> "));
    $(window).scroll();
}
function FechaDivMsg() {
    $('#DivBgMsg').hide();
}


function FormatadaDatas(data) {
    var dia = data.substring(0, 2);
    var mes = data.substring(3, 5);
    var ano = data.substring(6, 10);

    var dataFormatada = new Date(ano, mes - 1, dia);

    return dataFormatada;
}

function getToday() {
    var today = new Date();
    Days = parseInt(2);
    var dia = (today.getDate()).toString();;
    var mes = (today.getMonth() + 1).toString();;
    var ano = (today.getFullYear()).toString();;

    if (dia.substr(0, 1) == "0") { dia = dia.substr(1, 1); }
    if (mes.substr(0, 1) == "0") { mes = mes.substr(1, 1); }

    var d = new Date(parseInt(ano), (parseInt(mes) - 1), parseInt(dia), 1, 0, 0, 0);

    d.setTime(d.getTime() + (parseInt(Days) * 1000 * 60 * 60 * 24));
    dia = d.getDate();
    mes = d.getMonth() + 1;
    ano = d.getFullYear();

    strDia = new String();
    strDia = dia.toString();
    strMes = mes.toString();
    if (strDia.length == 1) { strDia = "0" + strDia; }
    if (strMes.length == 1) { strMes = "0" + strMes; }

    var tomorow = strDia + '/' + strMes + '/' + ano;

    return tomorow;
}

function getTodaySame() {
    var today = new Date();
    Days = parseInt(0);
    var dia = (today.getDate()).toString();;
    var mes = (today.getMonth() + 1).toString();;
    var ano = (today.getFullYear()).toString();;

    if (dia.substr(0, 1) == "0") { dia = dia.substr(1, 1); }
    if (mes.substr(0, 1) == "0") { mes = mes.substr(1, 1); }

    var d = new Date(parseInt(ano), (parseInt(mes) - 1), parseInt(dia), 1, 0, 0, 0);

    d.setTime(d.getTime() + (parseInt(Days) * 1000 * 60 * 60 * 24));
    dia = d.getDate();
    mes = d.getMonth() + 1;
    ano = d.getFullYear();

    strDia = new String();
    strDia = dia.toString();
    strMes = mes.toString();
    if (strDia.length == 1) { strDia = "0" + strDia; }
    if (strMes.length == 1) { strMes = "0" + strMes; }

    var tomorow = strDia + '/' + strMes + '/' + ano;

    return tomorow;
}

function getYesterday() {
    var today = new Date();
    var dd = today.getDate() - 1;
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = dd + '/' + mm + '/' + yyyy;

    return today;
}

function getTomorow() {
    var today = new Date();
    Days = parseInt(1);
    Day = getToday();
    var strData = new String(Day);
    var dia = new String(strData.substr(0, 2));
    var mes = new String(strData.substr(3, 2));
    var ano = new String(strData.substr(6, 4));

    if (dia.substr(0, 1) == "0") { dia = dia.substr(1, 1); }
    if (mes.substr(0, 1) == "0") { mes = mes.substr(1, 1); }

    var d = new Date(parseInt(ano), (parseInt(mes) - 1), parseInt(dia), 1, 0, 0, 0);

    d.setTime(d.getTime() + (parseInt(Days) * 1000 * 60 * 60 * 24));
    dia = d.getDate();
    mes = d.getMonth() + 1;
    ano = d.getFullYear();

    strDia = new String();
    strDia = dia.toString();
    strMes = mes.toString();
    if (strDia.length == 1) { strDia = "0" + strDia; }
    if (strMes.length == 1) { strMes = "0" + strMes; }

    var tomorow = strDia + '/' + strMes + '/' + ano;

    return tomorow;
}

try {
    document.body.style.zoom = 1.0;
} catch (e) {

}
