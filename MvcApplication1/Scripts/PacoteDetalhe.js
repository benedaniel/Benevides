//Funcções da Página de Detalhe do Pacote
//$("#ddlAdultos").change(function () {
//    $.post('<%=Url.Action("Pacote", "LoadDetalhePacoteMatriz")%>', {
//        nome: "Teste",
//        id: 1390,
//        data: "2013-9-1",
//        origem: "9668",
//        qtdAdulto: 2
//    }, function (data) {
//        refreshDiv($("div#chamada"), data);
//    });
//});

$(function () {
    $("#tabs").tabs();
});
// it will return base domain name only. e.g. yahoo.co.in
function GetBaseUrl() {
    try {
        var url = location.href;

        var start = url.indexOf('//');
        if (start < 0)
            start = 0
        else
            start = start + 2;

        var end = url.indexOf('/', start);
        if (end < 0) end = url.length - start;

        var baseURL = url.substring(start, end);
        return baseURL;
    }
    catch (arg) {
        return null;
    }
}

function VerVoo(div, preco) {
    $("." + div).show();
    $("." + div).dialog({});

}

function VerPreco(div) {
    $("." + div + "-Preco").show();
}

function FecharPreco(div, preco) {
    $("." + div + "-Preco").hide();
}

function VerPrecoParcelas(div) {
    $("." + div + "-Parcelas").show();
    args = {
        CodigoPacote: div
    };
    $.ajax({
        url: '/Pacote/FormasdePagamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("." + div + "-Parcelas div").html(data);
        },
        error: function () {
            $("." + div + "-Parcelas div").html("Tente novamente mais tarde.");
            $("." + div + "-Parcelas").hide();
        }
    });
}

function FecharPrecoParcelas(div, preco) {
    $("." + div + "-Parcelas").hide();
}

function FecharVoo(div) {
    $("." + div).hide();
}

function EsconderVoo(div) {
    $("." + div).hide();
}

function AdicionarCarrinho(CodigoPacote, comprar) {

    var valido = CalcularPrecoPacote(CodigoPacote);

    if (!valido) {
        alert('Indisponível para a quantidade de passageiros informados!');
    }
    else {

        $('.loadingDefault').fadeIn();
        args = {
            CodigoPacote: CodigoPacote,
            Comprar: comprar
        };

        $.ajax({
            url: '/Carrinho/AdicionarPacoteCarrinho',
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
                alert("Problema ao adicionar item no carrinho.");
            }
        });
    }
}

//function AdicionarOrcamento(CodigoPacote) {
//   
//    var args = new Object();
//    args = {
//        CodigoPacote: CodigoPacote
//    };
//    $.ajax({
//        url: '/Orcamento/AdicionarPacoteOrcamento',
//        type: 'POST',
//        data: JSON.stringify(args),
//        dataType: 'json',
//        contentType: 'application/json',
//        success: function (data) {
//            alert("Orçamento adicionado");
//            $('html, body').animate({ scrollTop: 0 }, 'fast');
//        },
//        error: function () {
//            alert("Problema ao adicionar item no orçamento.");
//        }
//    });
//}

function AdicionarOrcamento(CodigoPacote) {
    $('.loadingDefault').show();
    //alert("Entrei: " + CodigoPacote);
    //CalcularPrecoPacote(CodigoPacote);
    var args = new Object();
    args = {
        CodigoPacote: CodigoPacote
    };
    $.ajax({
        url: '/Orcamento/AdicionarPacoteOrcamento',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            //alert("Entrei show: " + data);
            ShowOrcamento(data);
            $('html, body').animate({ scrollTop: 0 }, 'fast');
        },
        error: function () {
            alert("Problema ao adicionar item no orçamento.");
        }
    });
}

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
            alert("Erro ao exibir o orçamento");
        }
    });
}

function RegistrarVenda(codigo, ingresso) {
    var args = new Object();
    // args = Atividade;
    $.ajax({
        url: '/Pacote/DetalhePacote',
        type: 'POST',
        //   data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $.post("/Pacote/LoadDetalhePacoteMatriz", { teste: "" });
        },
        error: function () {
            alert("Problema ao registrar venda.");
        }
    });
}

function LoadEmbarque(ef) {
    $('#ddlCidadesEmbarque').empty();
    $('#ddlCidadesEmbarque').hide();
    $.ajax({
        url: '/Pacote/LoadEmbarque',
        type: 'POST',
        data: JSON.stringify({ embarqueFiltro: ef }),
        dataType: 'json',
        onLoading: jQuery(".loaderEmbarque").html('<img src="../images/ajax-loader.gif" />').show(),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            jQuery(".loaderEmbarque").hide();
            $('#ddlCidadesEmbarque').show();
            var list = "";
            var varpaisAnterior
            for (var i = 0; i < data.length; i++) {
                if (i === 0) {
                    list += "<optgroup id='teste' label='" + data[i].NomePais + "'>";
                    varpaisAnterior = data[i].NomePais;
                }
                if (data[i].NomePais != varpaisAnterior) {
                    list += "</optiongroup><optgroup id='teste' label='" + data[i].NomePais + "'>";
                }
                list += "<option dt='teste' value='" + data[i].ID_CIDADE + "'optiongroup='" + data[i].NomePais + "'>" + data[i].NM_CIDADE + "</option>";

                varpaisAnterior = data[i].NomePais;
            }
            list += "</optiongroup>";
            $("#ddlCidadesEmbarque").append(list);
            $("#ddlCidadesEmbarque").val($("#hdnEmbarque").val());
        },
        error: function () {
        }
    });
}

function LoadEmbarquePacDest(ef) {
    $("#ddlCidadesEmbarquePacDest").val($("#hdnCidadesEmbarquePacDest").val());
}

function BuscaAtividades() {
    $("#buttonPacote").click(function () {
        $('#lblPacotesPara').text($("#ddlCidadesDestino option:selected").text());
        $('#lblSaindo').text($("#ddlCidadesEmbarque option:selected").text());
        $('#lblDatasaida').text($("#ddlDataVIagem option:selected").text());
        $('#GridPacoteMatriz').load('/Pacote/GridPacoteMatriz', { DataViagem: $("#ddlDataVIagem").val(), CidadeDestino: $("#ddlCidadesDestino").val(), CidadeEmbarque: $("#ddlCidadesEmbarque").val(), OrdenarPor: "1" });
    });
}

function LoadDiaDefault() {
    $('#ddlDataViagemDia').empty();
    $('#ddlDataViagemDia').hide();
    $('.pacotes-detalhes-comprar').css('display', 'none');
    $('.pacotes-detalhes-solicitar').css('display', 'none');
    $('.pacotes-detalhes-lotado').css('display', 'none');
    var codigoCidadeDestino1;
    var selectedVal = $("#DivRadio input:radio:checked").val();
    codigoCidadeDestino1 = $("#hdnCidadesDestinoPacDest").val();
    var codigoCidadeOrigem1;
    codigoCidadeOrigem1 = $("#ddlCidadesEmbarquePacDest").val();
    mesConsulta = $("#hdnMesDefault").val();
    $.ajax({
        url: '/Pacote/LoadDiaViagem',
        type: 'POST',
        data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1, indicadorAgrupMes: mesConsulta }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        onLoading: jQuery(".loaderData").html('<img src="../images/ajax-loader.gif" />').show(),
        success: function (data) {
            jQuery(".loaderData").hide();
            $('#ddlDataViagemDia').show();
            var list = null
            var varpaisAnterior
            for (var i = 0; i < data.length; i++) {

                list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].DiaSaida + "/" + data[i].MesSaida + "/" + data[i].AnoSaida + "</option>";
            }
            $("#ddlDataViagemDia").append(list);
            $("#ddlDataViagemDia").val($("#dataGet").val());
        },
        error: function () {
            alert("Problema ao carregar dia da viagem.");
        }
    });
}

function LoadDataDefault() {
    $('.pacotes-detalhes-comprar').css('display', 'none');
    $('.pacotes-detalhes-solicitar').css('display', 'none');
    $('#ddlDataViagem').empty();
    var codigoCidadeDestino1;
    var selectedVal = $("#DivRadio input:radio:checked").val();
    codigoCidadeDestino1 = $("#hdnCidadesDestinoPacDest").val();
    var codigoCidadeOrigem1;
    codigoCidadeOrigem1 = $("#hdnCidadesEmbarquePacDest").val();
    $.ajax({
        url: '/Pacote/LoadDataViagemDestino',
        type: 'POST',
        data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1, indicadorAgrupMes: 'S' }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            var list = null
            var varpaisAnterior
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {

                    list += "<option dt='teste' value='" + data[i].Mes + "'>" + data[i].NomeMes + "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataViagem").append(list);
                $('#ddlDataViagem').val($("#hdnMesDefault").val());
            } else {
                debugger;
                window.location("/Pacote/LoadPacoteMatriz");
            }
        },
        error: function () {
            alert("Problema ao carregar data.");
        }
    });
}

function Mais(eu) {
    esconderPacotes();
    $('#' + eu).val(parseInt($('#' + eu).val(), 10) + 1);
}

function Menos(eu) {
    esconderPacotes();
    if (parseInt($('#' + eu).val()) - 1 >= 1) {
        $('#' + eu).val(parseInt($('#' + eu).val(), 10) - 1);
    }
}

function LoadGridPacote() {
    $('.loadingDefault').fadeIn();
    $.ajax({
        url: '/Pacote/GridDetalhePacoteMatriz',
        type: 'POST',
        data: {
            Nome: $("#nomeGet").val(), id: $("#idGet").val(), dataget: $("#dataGet").val(), origem: $("#origemGet").val()
        },
        success: function (result) {
            $("#GridDetalhePacoteMatriz").html(result);
            $('#mensagemRefazer').css('display', 'none');
            $('.loadingDefault').fadeOut();
        }
    });
}

$(document).ready(function () {
    $('#btnSubmit').click(function () {
        var qtdQuartos = $("#qtdQuartos").val();
        for (var i = 1; i <= qtdQuartos; i++) {
            var qtdCriancas = $("#ddlQ" + i + "crianca").val();
            if (qtdCriancas > 0) {
                if ($("#agrpQuarto" + i + " select").length < qtdCriancas) {
                    alert("Informe a idade das crianças do quarto " + i + "!");
                    aumentarIdade("ddlQ" + i + "crianca", 'mais', 1);
                    return false;
                }
            }
        }

        if (($('#ddlDataViagem').val() == "Selecione") || ($('#ddlDataddlDataViagemDiaVIagem').val() == "Data")) {
            alert('Favor selecionar o Mes e o Dia antes de Refazer a busca !');
            return false;
        }
        var obj = $("#frmFiltros").serializeArray();
        $("#DataSaidaDestaque").text($('select[name="ddlDataViagemDia"] option:selected').text().substring(0, 2) + ' de ' + $('select[name="ddlDataViagem"] option:selected').text());
        $("#LocalSaidaDestaque").text($('select[name="ddlCidadesEmbarquePacDest"] option:selected').text());
        $('.loadingDefault').fadeIn();
        $.ajax({
            url: '/Pacote/GridDetalhe',
            type: 'POST',
            data: obj,
            success: function (result) {
                $("#GridDetalhePacoteMatriz").html(result);
                $('#mensagemRefazer').css('display', 'none');
                $('.loadingDefault').fadeOut();
            }
        });
    });

    $(".plus").click(function () {
        //var text = $(this).next(":text");
        //text.val(parseInt(text.val(), 10) + 1);
    });

    $(".minus").click(function () {
        var text = $(this).prev(":text");
        if (text.val() != 0) {
            //       text.val(parseInt(text.val(), 10) - 1);
        }
    });

    $('select[name="combo-ordenar"]').change(function () {
        $('#GridPacoteMatriz').load('/Pacote/GridPacoteMatriz', { DataViagem: $("#hdnDataSaida").val(), CidadeDestino: $("#hdnCidadeDestino").val(), CidadeEmbarque: $("#hdnEmbarque").val(), OrdenarPor: $("#combo-ordenar").val() });
    });

    BuscaAtividades();
    LoadGridPacote();
    LoadEmbarquePacDest('S');
    LoadEmbarque('S');
    $('#DivRadio input:radio').change(function () {
        var selectedVal = $("#DivRadio input:radio:checked").val();
        LoadEmbarque(selectedVal);
    });

    LoadDataDefault();
    LoadDiaDefault();
    $('select[name="ddlCidadesDestino"]').change(function () {
        $('#ddlDataVIagem').empty();
        $('#ddlDataVIagem').style.display = none;
        var codigoCidadeDestino1;
        var selectedVal = $("#DivRadio input:radio:checked").val();
        codigoCidadeDestino1 = $(this).val();
        var codigoCidadeOrigem1;
        codigoCidadeOrigem1 = $("#ddlCidadesEmbarque").val();
        $.ajax({
            url: '/Pacote/LoadDataViagem',
            type: 'POST',
            data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1, indicadorAgrupMes: 'S' }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            onLoading: jQuery(".loaderData").html('<img src="../images/ajax-loader.gif" />').show(),
            success: function (data) {
                jQuery(".loaderData").hide();
                $('#ddlDataVIagem').show();
                var list = null
                var varpaisAnterior

                for (var i = 0; i < data.length; i++) {

                    list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].NomeMes + "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataVIagem").append(list);
            },
            error: function () {
                alert("Problema ao carregar aereo.");
            }
        });
    });

    $('select[name="ddlDataViagem"]').change(function () {
        $('#ddlDataViagemDia').empty();
        $('#ddlDataViagemDia').hide();
        $('.pacotes-detalhes-comprar').css('display', 'none');
        $('.pacotes-detalhes-solicitar').css('display', 'none');
        $('.pacotes-detalhes-lotado').css('display', 'none');
        var codigoCidadeDestino1;
        var selectedVal = $("#DivRadio input:radio:checked").val();
        codigoCidadeDestino1 = $("#hdnCidadesDestinoPacDest").val();
        var codigoCidadeOrigem1;
        codigoCidadeOrigem1 = $("#ddlCidadesEmbarquePacDest").val();
        mesConsulta = $("#ddlDataViagem").val();


        $.ajax({
            url: '/Pacote/LoadDiaViagem',
            type: 'POST',
            data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1, indicadorAgrupMes: mesConsulta }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            onLoading: jQuery(".loaderData").html('<img src="../images/ajax-loader.gif" />').show(),
            success: function (data) {
                jQuery(".loaderData").hide();
                $('#ddlDataViagemDia').show();
                var list = null
                var varpaisAnterior
                for (var i = 0; i < data.length; i++) {

                    list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].DiaSaida + "/" + data[i].MesSaida + "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataViagemDia").append(list);
            },
            error: function () {
                alert("Problema ao carregar aereo.");
            }
        });
    });

    $('select[class="selectPacoteNumerico"]').change(function () {
        $('.pacotes-detalhes-solicitar').css('display', 'none');
        $('.pacotes-detalhes-comprar').css('display', 'none');
    });

    $('select[class="selectPacote"]').change(function () {
        $('.pacotes-detalhes-comprar').css('display', 'none');
        $('.pacotes-detalhes-solicitar').css('display', 'none');
    });

    $('select[name="ddlCidadesEmbarquePacDest"]').change(function () {
        $('.pacotes-detalhes-comprar').css('display', 'none');
        $('.pacotes-detalhes-solicitar').css('display', 'none');
        $('#ddlDataViagem').empty();
        var codigoCidadeDestino1;
        var selectedVal = $("#DivRadio input:radio:checked").val();
        codigoCidadeDestino1 = $("#hdnCidadesDestinoPacDest").val();
        var codigoCidadeOrigem1;
        codigoCidadeOrigem1 = $(this).val();
        $.ajax({
            url: '/Pacote/LoadDataViagemDestino',
            type: 'POST',
            data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1, indicadorAgrupMes: 'S' }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var list = null
                var varpaisAnterior
                list += "<option dt='teste' value='Selecione'>Selecione</option>";

                for (var i = 0; i < data.length; i++) {
                    list += "<option dt='teste' value='" + data[i].Mes + "'>" + data[i].NomeMes + "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataViagem").append(list);

                $('#ddlDataViagemDia')
                .find('option')
                .remove()
                .end()
                .append('<option value="Data">Selecione Mês</option>')
                .val('Data')
                ;
            },
            error: function () {
                alert("Problema ao carregar destino.");
            }
        });
    });
    $('select[name="ddlCidadesEmbarque"]').change(function () {
        $('#ddlCidadesDestino').empty();
        $('#ddlCidadesDestino').hide();
        var codigoCidadeOrigem1;
        var selectedVal = $("#DivRadio input:radio:checked").val();
        codigoCidadeOrigem1 = $(this).val();
        $.ajax({
            url: '/Pacote/LoadDestino',
            type: 'POST',
            data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, embarqueFiltro: selectedVal }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            onLoading: jQuery(".loader").html('<img src="../images/ajax-loader.gif" />').show(),
            success: function (data) {
                jQuery(".loader").hide();
                $('#ddlCidadesDestino').show();
                var list = null
                var varpaisAnterior
                for (var i = 0; i < data.length; i++) {
                    if (i === 0) {
                        list += "<optgroup id='teste' label='" + data[i].NomePais + "'>";
                        varpaisAnterior = data[i].NomePais;
                    }
                    if (data[i].NomePais != varpaisAnterior) {
                        list += "</optiongroup><optgroup id='teste' label='" + data[i].NomePais + "'>";
                    }

                    list += "<option dt='teste' value='" + data[i].ID_CIDADE + "'optiongroup='" + data[i].NomePais + "'>" + data[i].NM_CIDADE + "</option>";

                    varpaisAnterior = data[i].NomePais;
                }
                list += "</optiongroup>";
                $("#ddlCidadesDestino").append(list);

                $('#ddlDataVIagem')
                .find('option')
                .remove()
                .end()
                .append('<option value="Data">Data</option>')
                .val('Data')
                ;
            },
            error: function () {
                alert("Problema ao carregar destino.");
            }
        });
    });

    /* This is basic - uses default settings */

    $("a#single_image").fancybox();

    /* Using custom settings */

    $("a#inline").fancybox({
        'hideOnContentClick': true
    });

    /* Apply fancybox to multiple items */

    $("a.group").fancybox({ 'transitionIn': 'elastic', 'transitionOut': 'elastic', 'speedIn': 600, 'speedOut': 200, 'overlayShow': false });
});

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
            //do nothing
        },
        error: function () {
            alert("Problema ao registrar.");
        }
    });
}

function esconderPacotes()
{    
    $('#GridDetalhePacoteMatriz').html('');
    $('#mensagemRefazer').css('display', '');
}

function aumentarIdade(eu, sinal, quartoid) {
    esconderPacotes();
    for (var i = 0; i < 99 ; i++) {
        var elem = document.getElementById(eu + i);
        if (elem != null)
            elem.parentNode.removeChild(elem);
    }

    var elem2 = document.getElementById('lblidades' + quartoid);
    if (elem2 != null)
        elem2.parentNode.removeChild(elem2);


    var elem3 = document.getElementById('divIdadecrianca' + quartoid);
    if (elem3 != null)
        elem3.parentNode.removeChild(elem3);

    var qtd;
    qtd = $('#' + eu).val();

    if (sinal == 'mais') {
        qtd = parseInt(qtd) + 1;
        $('#' + eu).val(parseInt($('#' + eu).val(), 10) + 1);
    } else {
        qtd = parseInt(qtd) - 1;
        if (parseInt($('#' + eu).val()) - 1 >= 0) {
            $('#' + eu).val(parseInt($('#' + eu).val(), 10) - 1);
        }
    }

    if (qtd > 0) {
        $('<label/>', {
            text: 'Idades ',
            id: 'lblidades' + quartoid,
            name: 'lblidades' + quartoid,
            style: 'color: #666666;margin-left: 20px;margin-right: 66px;'
        }).appendTo('#agrpQuarto' + quartoid);
    }

    if (parseInt($('#' + eu).val()) >= 0) {
        for (var i = 0; i < qtd ; i++) {
            $('<select/>', {
                id: eu + i,
                name: eu + i,
                onchange: "esconderPacotes()",
                style: "margin-top:8px;margin-left:72px;border: solid 1px #d6d6d6;"
            }).appendTo('#agrpQuarto' + quartoid);

            $('<option/>', {
                value: "0",
                text: "0"
            }).appendTo('#' + eu + i);

            $('<option/>', {
                value: "1",
                text: "1"
            }).appendTo('#' + eu + i);

            $('<option/>', {
                value: "2",
                text: "2"
            }).appendTo('#' + eu + i);

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

            $('<br/>').appendTo('#' + eu + i);
            $('<br/>').appendTo('#lbl' + quartoid);
        }

    }
}

function remover() {
    esconderPacotes();
    var elem3 = document.getElementById('hrQuarto' + $("#qtdQuartos").val());

    if (elem3 != null)
        elem3.parentNode.removeChild(elem3);

    var elem = document.getElementById('agrpQuarto' + $("#qtdQuartos").val());
    if (elem != null && $("#qtdQuartos").val() != 1) {
        elem.parentNode.removeChild(elem);
        $("#qtdQuartos").val(parseInt($("#qtdQuartos").val()) - 1);
    }
}

function mudar() {
    esconderPacotes();
    $('.pacotes-detalhes-comprar').css('display', 'none');
    $('.pacotes-detalhes-solicitar').css('display', 'none');
    $('.pacotes-detalhes-lotado').css('display', 'none');

    var qtdQ = parseInt($("#qtdQuartos").val()) + 1;

    if (qtdQ <= 6) {

        $('<hr/>', {
            style: "height: 1px; margin: 20px; background-color: #d6d6d6;", id: "hrQuarto" + qtdQ,
            name: "hrQuarto" + qtdQ
        }).appendTo('#chamadaRight');

        $('<div/>', {
            id: "agrpQuarto" + qtdQ,
            name: "agrpQuarto" + qtdQ
        }).appendTo('#chamadaRight');

        $('<label/>', {
            text: 'Quarto 0' + qtdQ,
            id: 'QUARTO' + qtdQ,
            style: 'color:#00387b;margin-left:20px;'
        }).appendTo('#agrpQuarto' + qtdQ);

        $('<br/>').appendTo('#' + 'QUARTO' + qtdQ);

        $('<div/>', {
            id: "quants" + qtdQ,
            name: "quants" + qtdQ,
            class: 'quants'
        }).appendTo('#agrpQuarto' + qtdQ);

        $('<label/>', {
            text: 'Adultos ',
            style: 'color: #666666;margin-left: 20px;'
        }).appendTo('#quants' + qtdQ);


        $('<div/>', {
            id: "quantsAux" + qtdQ,
            name: "quantsAux" + qtdQ
        }).appendTo('#quants' + qtdQ);

        $('<a/>', { href: 'javascript:', class: 'plus', onclick: "Mais('ddlQ" + qtdQ + "Adulto')" }).appendTo('#quantsAux' + qtdQ);


        $('<input/>', {
            type: "text",
            value: "2",
            id: 'ddlQ' + qtdQ + 'Adulto',
            name: 'ddlQ' + qtdQ + 'Adulto'
        }).appendTo('#quantsAux' + qtdQ);

        $('<a/>', { href: 'javascript:', class: 'minus', onclick: "Menos('ddlQ" + qtdQ + "Adulto')" }).appendTo('#quantsAux' + qtdQ);

        $('<div/>', {
            id: "quantsCri" + qtdQ,
            name: "quantsCri" + qtdQ,
            class: 'quants'
        }).appendTo('#agrpQuarto' + qtdQ);

        $('<label/>', {
            text: 'Crianças ',
            style: 'color: #666666;margin-left: 20px;'
        }).appendTo('#quants' + qtdQ);


        $('<div/>', {
            id: "quantsCriAux" + qtdQ,
            name: "quantsCriAux" + qtdQ
        }).appendTo('#quantsCri' + qtdQ);

        $('<a/>', { href: 'javascript:', class: 'plus', onclick: "aumentarIdade('ddlQ" + qtdQ + "Crianca','mais'," + qtdQ + ")" }).appendTo('#quantsCriAux' + qtdQ);


        $('<input/>', {
            type: "text",
            value: "0",
            id: 'ddlQ' + qtdQ + 'Crianca',
            name: 'ddlQ' + qtdQ + 'Crianca'
        }).appendTo('#quantsCriAux' + qtdQ);

        $('<a/>', { href: 'javascript:', class: 'minus', onclick: "aumentarIdade('ddlQ" + qtdQ + "Crianca','menos'," + qtdQ + ")" }).appendTo('#quantsCriAux' + qtdQ);

        $("#qtdQuartos").val(parseInt($("#qtdQuartos").val()) + 1);
    } else {
        return false;
    }
}

$(function () {
    $(".numeroestrelas").change(function () {
        postarform();
    });
});
function SearchHotelName() {
    var num = $("#txtNome").val().length;
    if (num >= 3 || num == 0) {
        setTimeout(sayHi, 1000);
    }
}
function sayHi() {
    postarform();
}

var busca = false;
var pesquisar = false;
var i = 0;

setInterval(function () {
    if (i > 1 && pesquisar == true) {
        pesquisar = false;
        busca = true;
    }
    if (busca) {
        postarform();
        busca = false;
    }
    i = i + 1;
}, 555);

function SearchHotelName() {
    i = 0;
    pesquisar = true;

}

function postarform() {

    var index = 0;
    var estrela = "";

    for (var i = 0; i < $(".numeroestrelas").length; i++) {
        if ($(".numeroestrelas")[i].checked) {
            estrela += $(".numeroestrelas")[i].value;
            estrela += ",";
        }
    }

    var quantCriancas = $("#QuantCriancas").val();
    var quantAdultos = $("#QuantAdultos").val();
    var ordernacao = $("#selectOrder").val();
    var checkout = $("#Checkout").val();
    var codigocidade = $("#CodigoCidade").val();
    var rangeValor = $("#amount").val();
    var nomehotel = $("#txtNome").val();
    var checkin = $("#Checkin").val();
    var codigocidade = $("#CodigoCidade").val();
    rangeValor = rangeValor.replace("R$", "");
    rangeValor = rangeValor.replace("R$", "");
    rangeValor = rangeValor.replace(" ", "");
    rangeValor = rangeValor.split("-");
    $("#hdnIsFiltro").val("1");

    var args = new Object();
    args = {
        nome: nomehotel,
        menorPreco: rangeValor[0],
        maiorPreco: rangeValor[1],
        estrela: estrela,
        hotelBuscaCheckin: checkin,
        hotelBuscaCheckout: checkout,
        order: ordernacao,
        quantAdultos: quantAdultos,
        quantCriancas: quantCriancas,
        codigocidade: codigocidade
    };

    if (($('#ddlDataViagem').val() == "Selecione") || ($('#ddlDataddlDataViagemDiaVIagem').val() == "Data")) {
        alert('Favor selecionar o Mes e o Dia antes de Refazer a busca !');
        return false;
    }

    var obj = $("#frmFiltros").serializeArray();
    $("#DataSaidaDestaque").text($('select[name="ddlDataViagemDia"] option:selected').text().substring(0, 2) + ' de ' + $('select[name="ddlDataViagem"] option:selected').text());
    $("#LocalSaidaDestaque").text($('select[name="ddlCidadesEmbarquePacDest"] option:selected').text());
    $('.loadingDefault').fadeIn();
    $.ajax({
        url: '/Pacote/GridDetalhe',
        type: 'POST',
        data: obj,
        success: function (result) {
            $("#GridDetalhePacoteMatriz").html(result);
            $('#mensagemRefazer').css('display', 'none');
            $("#hdnIsFiltro").val("0");
            $('.loadingDefault').fadeOut();
        }
    });
}

function CalcularPrecoPacote(pacote) {
    var ok = false;
    $('#divcalcular-' + pacote).html('<img src="../../../../Images/loading_transparent.gif" />');

    var criancas = Array();
    var agechild = Array();

    var Apartamentos = Array();
    var Quartos = $("#qtdQuartos").val();

    for (var i = 1; i <= Quartos ; i++) {
        var Apartamento = new Object();
        Apartamento.Passageiros = Array();
        Apartamento.CodigoApartamento = i;
        for (var k = 0; k < $('#ddlQ' + i + 'Adulto').val() ; k++) {
            Apartamento.Passageiros.push({ IdadePassageiro: 30 });
        }

        for (var j = 0; j < $('#agrpQuarto' + i + ' select').length ; j++) {
            {
                Apartamento.Passageiros.push({ IdadePassageiro: $('#ddlQ' + i + 'crianca' + j).val() });
            }

        }
        Apartamentos.push(Apartamento);
    }

    $.ajax({
        url: '/Pacote/CalcularPrecoPacote',
        type: 'POST',
        data: JSON.stringify({ CodigoPacote: pacote, apartamentos: JSON.stringify(Apartamentos) }),
        dataType: 'json',
        async: false,
        contentType: 'application/json',
        success: function (data) {
            if (data.Valor != "0,00") {
                $('#divcalcular-' + pacote).html('<a style="color:#7B7B7B"> Pacote: ' + data.SiglaMoeda + ' ' + data.Valor + '</a><br/><a style="color:#7B7B7B"> Taxa: ' + data.SiglaMoeda + ' ' + data.Taxa + '</a>' + '</a><br/><a style="font-weight: bold; color:#7B7B7B"> Total: ' + data.SiglaMoeda + ' ' + data.ValorTotal + '</a>');
                ok = true;
            }
            else {
                $('#divcalcular-' + pacote).html('<div style="width:105px; color: #F05C18;"><a style="color: #F05C18;">Indisponível para a quantidade de passageiros informados!<\a><\div>');
                ok = false;
            }
        },
        error: function () {
            alert("Problemas ao calcular.");
        }
    });
    return ok;
}






