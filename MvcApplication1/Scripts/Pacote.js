function CarregarCarrinho() {
    /* $("#gridCarrinho").load("http://" + GetBaseUrl() + "/Carrinho/LoadCarrinho", {
     });
     $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', border: '1px solid !important' }).css("border:'1px solid';font-size:8pt");
     */
}

function PacoteLoadingHide() {
    $('.loadingDefault').fadeOut();
}

function PacoteLoadingShow() {

    if ($('#ddlCidadesEmbarque').val() == "") {
        alert('Favor informe a Origem!');
        return false;
    }

    if ($('#ddlCidadesDestino').val() == "") {
        alert('Favor informe o Destino!');
        return false;
    }

    if ($('#ddlDataVIagem').val() == "") {
        alert('Favor informe a Data da Viagem!');
        return false;
    }

    $("#pacoteForm").validate();

    if ($("#pacoteForm").valid()) {
        $('.loadingDefault').fadeIn();

        return true;
    }
    else {
        return false;
    }
}

function PacotePartialLoadingShow() {
    $("#loadPacoteForm").validate();

    if ($("#loadPacoteForm").valid()) {
        $('.loadingDefault').fadeIn();

        return true;
    }
    else {
        return false;
    }
}

$("#ddlAdultos").change(function () {
    $.post('<%=Url.Action("Pacote", "LoadDetalhePacoteMatriz")%>', {
        nome: "Teste",
        id: 1390,
        data: "2013-9-1",
        origem: "9668",
        qtdAdulto: 2
    }, function (data) {
        refreshDiv($("div#chamada"), data);
    });
});

$(function () {
    $("#tabs").tabs();
});

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

function VerVoo(div) {
    $("." + div).show();

}

function AdicionarCarrinho(Atividade, comprar) {

    $.ajax({
        url: '/Carrinho/AdicionarPacoteCarrinho',
        type: 'POST',
        data: JSON.stringify({ CodigoPacote: Atividade, Comprar: comprar }),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            if (comprar) {
                AtualizarCarrinho();
                $('.loadingDefault').fadeOut();
                alert('Adicionado ao Carrinho!');
            }
        },
        error: function () {
            alert("Problema ao adicionar no carrinho.");
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
            alert("Problema ao Registrar Venda.");
        }
    });

}

function LoadEmbarque(ef) {
        $('#ddlCidadesEmbarque').empty();
        $('.content-pacote-destino-emb').hide();

        var args = new Object();
        args = {
            embarqueFiltro: ef
        };


    $.ajax({
        url: '/Pacote/LoadEmbarque',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        onLoading: jQuery(".loaderEmbarque").html('<img src="../images/ajax-loader.gif" />').show(),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            jQuery(".loaderEmbarque").fadeOut();
            $('.content-pacote-destino-emb').show();
            var list = "";
            var varpaisAnterior
            for (var i = 0; i < data.length; i++) {
                if (i === 0) {
                    list += "<option value>Selecione</option>"
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

        },
        error: function () {
        }
    });
}


$(document).ready(function () {
    CarregarCarrinho();

    LoadEmbarque('S');
    $('#DivRadio input:radio').change(function () {
        var selectedVal = $("#DivRadio input:radio:checked").val();
        $('#ddlCidadesDestino').empty();
        $('#ddlDataVIagem').empty();
        LoadEmbarque(selectedVal);
    });

    $('select[name="ddlDataVIagem"]').change(function () {
        $("#hdnDataViagemTexto").val($("#ddlDataVIagem option:selected").text());
    });

    $('select[name="ddlCidadesDestino"]').change(function () {
        $('#ddlDataVIagem').empty();
        $('#content-pacote-data-home').hide();
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
                $('#content-pacote-data-home').show();
                var list = null
                var varpaisAnterior
                list += "<option value=''>Selecione</option>"
                for (var i = 0; i < data.length; i++) {
                    list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].NomeMes + "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataVIagem").append(list);
                $("#hdnCidadesDestinoTexto").val($("#ddlCidadesDestino option:selected").text());
            },
            error: function () {
                alert("Problema ao carregar pacotes.");
            }
        });
    });

    $('select[name="ddlCidadesEmbarque"]').change(function () {
        $('#ddlCidadesDestino').empty();
        $('#content-pacote-destino-interno').hide();
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
                $('#content-pacote-destino-interno').show();
                var list = null
                var varpaisAnterior
                for (var i = 0; i < data.length; i++) {
                    if (i === 0) {
                        list += "<option value>Selecione o Destino</option>"
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

                $("#hdnCidadesEmbarqueTexto").val($("#ddlCidadesEmbarque option:selected").text());

                $('#ddlDataVIagem')
                .find('option')
                .remove()
                .end()
                .append('<option value>Selecione</option>')
                ;
            },
            error: function () {
                alert("Problema ao carregar pacotes.");
            }
        });
    });

});

$('#pacoteForm').validate({
    rules: {
        ddlCidadesEmbarque: {
            required: true
        },
        ddlCidadesDestino: {
            required: true
        },
        ddlDataVIagem: {
            required: {
                depends: function (element) {
                    return $('#ddlDataVIagem').val() == "presele" || $('#ddlDataVIagem').val() == "" || $('#ddlDataVIagem').val() == "Data" || $('#ddlDataVIagem').val() == "Selecione" || $('#ddlDataVIagem').val() == "Selecione"
                }
            }
        }
    },
    highlight: function (element) {
        $(element).prev('span').addClass("error");
    },
    unhighlight: function (element) {
        $(element).prev('span').removeClass("error");
    },
    messages: {
        ddlCidadesEmbarque: '<label class="erroDrop"></label> Por favor selecionar um local de embarque',
        ddlCidadesDestino: '<label class="erroDrop"></label> Por favor selecionar um local de destino',
        ddlDataVIagem: '<label class="erroDrop"></label> Por favor selecionar uma data para a viagem'
    }
});
