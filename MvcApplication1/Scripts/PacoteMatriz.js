function CarregarCarrinho() {
    /* $("#gridCarrinho").load("http://" + GetBaseUrl() + "/Carrinho/LoadCarrinho", {
     });
     $("#gridCarrinho").dialog({ position: 'right', title: 'MINHA VIAGEM', border: '1px solid !important' }).css("border:'1px solid';font-size:8pt");
     */
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

function VerVoo(div) {
    $("." + div).show();
}

function AdicionarCarrinho(Atividade) {
    var args = new Object();
    args = Atividade;
    $.ajax({
        url: '/Carrinho/AdicionarPacoteCarrinho',
        type: 'POST',
        data: JSON.stringify(args),
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $("#gridCarrinho").show();
            $("#gridCarrinho").load("http://" + GetBaseUrl() + "/Carrinho/LoadCarrinho", { teste: "" });
            $("#gridCarrinho").dialog();
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
            alert("Problema ao registrar venda. ");
        }
    });

}

function LoadEmbarque(ef) {
    $('#ddlCidadesEmbarque').empty()
    $('#ddlCidadesEmbarque').hide();
    $.ajax({
        url: '/Pacote/LoadEmbarque',
        async: false,
        type: 'POST',
        data: JSON.stringify({ embarqueFiltro: ef }),
        dataType: 'json',
        onLoading: jQuery(".loaderEmbarque").html('<img width="150px" src="../images/ajax-loader.gif" />').show(),
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
            $("#ddlCidadesEmbarque").val($("#hdnEmbarque").val())
        },
        error: function () {
        }
    });
}

function LoadEmbarquePacDest(ef) {
    $('#ddlCidadesEmbarquePacDest').empty()
    $.ajax({
        url: '/Pacote/LoadEmbarque',
        type: 'POST',
        async: false,
        data: JSON.stringify({ embarqueFiltro: ef }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
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
            $("#ddlCidadesEmbarquePacDest").append(list);
        },
        error: function () {
            alert("Problema ao carregar destaques.");
        }
    });
}

function LoadEmbarqueDefault(cidOrig) {
    $('#ddlCidadesDestino').empty();
    $('#ddlCidadesDestino').hide();
    var codigoCidadeOrigem1;
    var selectedVal = $("#DivRadio input:radio:checked").val();
    codigoCidadeOrigem1 = cidOrig;
    $.ajax({
        url: '/Pacote/LoadDestino',
        type: 'POST',
        async: false,
        data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, embarqueFiltro: selectedVal }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        onLoading: jQuery(".loader").html('<img width="150px" src="../images/ajax-loader.gif" />').show(),
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

            $('#ddlCidadesDestino').val($("#hdnCidadeDestino").val());

        },
        error: function () {
        }
    });


}
function LoadDataDefault(cidDest, cidOrigem) {
    $('#ddlDataVIagem').empty();
    $('#ddlDataVIagem').hide();
    var codigoCidadeDestino1;
    var selectedVal = $("#DivRadio input:radio:checked").val();
    codigoCidadeDestino1 = cidDest;
    var codigoCidadeOrigem1;
    codigoCidadeOrigem1 = cidOrigem;
    $.ajax({
        url: '/Pacote/LoadDataViagem',
        type: 'POST',
        async: false,
        data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1, indicadorAgrupMes: 'S' }),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        onLoading: jQuery(".loaderData").html('<img width="170px" src="../images/ajax-loader.gif" />').show(),
        success: function (data) {
            jQuery(".loaderData").hide();
            $('#ddlDataVIagem').show();
            var list = null
            var varpaisAnterior
            for (var i = 0; i < data.length; i++) {

                list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].NomeMes + "/" + data[i].AnoSaida + "</option>";
            }
            $("#ddlDataVIagem").append(list);

            $('#ddlDataVIagem').val($("#hdnDataSaida").val());
            $(".matriz-verdetalhe").show();
        },
        error: function () {
            alert("Problema ao carregar data.");
        }
    });

}

function BuscaAtividades() {
    $("#buttonPacote").click(function () {

        if (($('#ddlCidadesDestino').val() == "Selecione") || ($('#ddlDataVIagem').val() == "Data")) {
            alert('Favor selecionar um destino antes de refazer a busca !');
            return false;
        }

        if (($('#ddlCidadesDestino').val() == null) || ($('#ddlDataVIagem').val() == null)) {
            alert('Favor selecionar um destino antes de refazer a busca !');
            return false;
        }
        $('.loadingDefault').show();
        $("#hdnDataSaida").val($("#ddlDataVIagem").val());
        $("#hdnCidadeDestino").val($("#ddlCidadesDestino").val());
        $("#hdnEmbarque").val($("#ddlCidadesEmbarque").val())
        $('#lblPacotesPara').text($("#ddlCidadesDestino option:selected").text());
        $('#lblSaindo').text($("#ddlCidadesEmbarque option:selected").text());
        $('#lblDatasaida').text($("#ddlDataVIagem option:selected").text());


        $('#FomMatriz').submit();

        return true;

        //$.ajax({
        //    url: '/Pacote/GridPacoteMatriz',
        //    type: 'POST',
        //    data: { DataViagem: $("#ddlDataVIagem").val(), CidadeDestino: $("#ddlCidadesDestino").val(), CidadeEmbarque: $("#ddlCidadesEmbarque").val(), OrdenarPor: "3", TipoProduto: $("#ddlTipoPrd").val() },
        //    success: function (result) {
        //        $("#GridPacoteMatriz").html(result);
        //        $('.loadingDefault').hide();
        //    }
        //});

    });

}

$(document).ready(function () {
    $(".matriz-verdetalhe").hide();


    var $radios = $('input:radio[name=rdbOrigem]');
    if ($("#hdnOrigem").val() == "S") {
        $radios.filter('[value=S]').prop('checked', true);
    }
    else {
        $radios.filter('[value=N]').prop('checked', true);
    }

    $('select[name="combo-ordenar"]').change(function () {
        $('.loadingDefault').fadeIn(1000);
        $.ajax({
            url: '/Pacote/GridPacoteMatriz',
            type: 'POST',
            data: { DataViagem: $("#hdnDataSaida").val(), CidadeDestino: $("#hdnCidadeDestino").val(), CidadeEmbarque: $("#hdnEmbarque").val(), OrdenarPor: $("#combo-ordenar").val(), TipoProduto: $("#hdnPrd").val() },
            success: function (result) {
                $("#GridPacoteMatriz").html(result);
                $('.loadingDefault').fadeOut(1000);
            }
        });
    });

    CarregarCarrinho();
    BuscaAtividades();
    LoadEmbarqueDefault($("#hdnEmbarque").val());
    LoadDataDefault($("#hdnCidadeDestino").val(), $("#hdnEmbarque").val());
    LoadEmbarquePacDest('S');
    LoadEmbarque('S');

    $('#ddlTipoPrd').val($('#hdnPrd').val());
    

    $('#DivRadio input:radio').change(function () {
        var selectedVal = $("#DivRadio input:radio:checked").val();
        $('#ddlCidadesDestino').empty();
        $('#ddlDataVIagem').empty();

        LoadEmbarque(selectedVal);
        LoadEmbarqueDefault($("#hdnEmbarque").val());
        if ($("#hdnOrigem").val() == selectedVal) {
            
            LoadDataDefault($("#hdnCidadeDestino").val(), $("#hdnEmbarque").val());
        }

    });

    $('select[name="ddlCidadesDestino"]').change(function () {
        $('#ddlDataVIagem').empty();
        $('#ddlDataVIagem').hide();
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
            onLoading: jQuery(".loaderData").html('<img width="170px" src="../images/ajax-loader.gif" />').show(),
            success: function (data) {
                jQuery(".loaderData").fadeOut();
                $('#ddlDataVIagem').show();
                var list = null
                var varpaisAnterior
                for (var i = 0; i < data.length; i++) {

                    list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].NomeMes +  "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataVIagem").append(list);
                
            },
            error: function () {
            }
        });
    });

    $('select[name="ddlCidadesEmbarquePacDest"]').change(function () {
        $('#ddlDataViagem').empty()
        var codigoCidadeDestino1;
        var selectedVal = $("#DivRadio input:radio:checked").val();
        codigoCidadeDestino1 = "1347";
        var codigoCidadeOrigem1;
        codigoCidadeOrigem1 = $(this).val();
        $.ajax({
            url: '/Pacote/LoadDataViagem',
            type: 'POST',
            data: JSON.stringify({ codigoCidadeOrigem: codigoCidadeOrigem1, codigoCidadeDestino: codigoCidadeDestino1 }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                var list = null
                var varpaisAnterior
                for (var i = 0; i < data.length; i++) {

                    list += "<option dt='teste' value='" + data[i].AnoSaida + "-" + data[i].MesSaida + "-" + data[i].DiaSaida + "'>" + data[i].NomeMes + "/" + data[i].AnoSaida + "</option>";
                }
                $("#ddlDataViagem").append(list);
                
            },
            error: function () {
                alert("Problema ao carregar data.");
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
            onLoading: jQuery(".loader").html('<img width="150px" src="../images/ajax-loader.gif" />').show(),
            success: function (data) {
                jQuery(".loader").hide();
                $('#ddlCidadesDestino').show();
                var list = null
                var varpaisAnterior

                list += "<option dt='teste' value='Selecione'>Selecione</option>";
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
                $("#ddlCidadesDestino").val("Selecione");

                $('#ddlDataVIagem')
                .find('option')
                .remove()
                .end()
                .append('<option value="Data">Selecione Destino</option>')
                .val('Data')
                ;

            },
            error: function () {
                alert("Problema ao carregar destino.");
            }
        });
    });

    $('#lblPacotesPara').text($("#ddlCidadesDestino option:selected").text());
    $('#lblSaindo').text($("#ddlCidadesEmbarque option:selected").text());
    $('#lblDatasaida').text($("#ddlDataVIagem option:selected").text());
    
});

