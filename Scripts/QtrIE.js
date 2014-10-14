function MaisHotel(eu) {

    var qua = $('#' + eu).val();
    if(qua < 9){
        $('#' + eu).val(parseInt($('#' + eu).val(), 10) + 1);
    }
    if ($('#DivPai2') != null) {
        $('#DivPai2').html('');
    }

}
function MenosHotel(eu) {
    if (parseInt($('#' + eu).val()) - 1 >= 1) {
        $('#' + eu).val(parseInt($('#' + eu).val(), 10) - 1);
    }
    if ($('#DivPai2') != null) {
        $('#DivPai2').html('');
    }

}
$(document).ready(function () {
    $("a#single_image").fancybox();
    $("a#inline").fancybox({
        'hideOnContentClick': true
    });
    $("a.group").fancybox({ 'transitionIn': 'elastic', 'transitionOut': 'elastic', 'speedIn': 600, 'speedOut': 200, 'overlayShow': false });
});
function aumentarIdadeHotel(eu, sinal, quartoid) {
     var qua = $('#' + eu).val();
      {
         var qtd;
         qtd = $('#' + eu).val();
         if (sinal == 'mais') {
             if (qua < 9) {
                 qtd = parseInt(qtd) + 1;
                 $('#' + eu).val(parseInt($('#' + eu).val(), 10) + 1);
             }
         } else {
             qtd = parseInt(qtd) - 1;
             if (parseInt($('#' + eu).val()) - 1 >= 0) {
                 $('#' + eu).val(parseInt($('#' + eu).val(), 10) - 1);
             }
         }
         for (var i = 0; i < 99; i++) {
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
         if (qtd > 0) {
             $('<label/>', {
                 text: 'Idades: ',
                 id: 'lblidades' + quartoid,
                 name: 'lblidades' + quartoid,
                 style: 'line-height: 4.7em;'
             }).attr('class', 'lblqtdcri').appendTo('#agrpQuarto' + quartoid);
         }
         if (parseInt($('#' + eu).val()) >= 0) {
             for (var i = 0; i < qtd; i++) {
                 $('<select/>', {
                     id: eu + i,
                     name: eu + i,
                     style: "border: solid 1px #d6d6d6;width: 55px;margin-top: 8px;margin-left: 5px;"
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
                 $('<br/>').appendTo('#' + eu + i);
                 $('<br/>').appendTo('#lbl' + quartoid);
             }
         }
     }
      if ($('#DivPai2') != null) {
          $('#DivPai2').html('');
      }
    
}

function removerHotel() {
    var elem3 = document.getElementById('hrQuarto' + $("#qtdQuartos").val());
    if (elem3 != null)
        elem3.parentNode.removeChild(elem3);
    var elem = document.getElementById('agrpQuarto' + $("#qtdQuartos").val());
    if (elem != null && $("#qtdQuartos").val() != 1) {
        elem.parentNode.removeChild(elem);
        $("#qtdQuartos").val(parseInt($("#qtdQuartos").val()) - 1);
    }
    try {
        AjusteHeightFrameParentParent("iframeBuscadorHotel", "iframes", 13);
    } catch (e) {
    }
    if ($('#DivPai2') != null) {
        $('#DivPai2').html('');
    }
}
function mudarHotel() {
    $('.pacotes-detalhes-comprar').css('display', 'none');
    $('.pacotes-detalhes-solicitar').css('display', 'none');
    $('.pacotes-detalhes-lotado').css('display', 'none');
    var qtdQ = parseInt($("#qtdQuartos").val()) + 1;
    $('<hr/>', {
        style: "height: 1px; width: 375px; background-color: none;", id: "hrQuarto" + qtdQ,
        name: "hrQuarto" + qtdQ
    }).appendTo('#chamadaRight');
    $('<div/>', { id: "agrpQuarto" + qtdQ, name: "agrpQuarto" + qtdQ }).attr('class', 'lblquantquartos').appendTo('#chamadaRight');
    $('#chamadaRight').attr("class", "agrpQuarto1");
    $('<label/>', { text: 'Quarto 0' + qtdQ, id: 'QUARTO' + qtdQ, style: 'font-weight:bold;' }).appendTo('#agrpQuarto' + qtdQ);
    $('<br/>').appendTo('#' + 'QUARTO' + qtdQ);
    $('<div/>', { id: "quants" + qtdQ, name: "quants" + qtdQ }).appendTo('#agrpQuarto' + qtdQ).attr("class", "quants quantsQuartosAereo");
    $('#agrpQuarto' + qtdQ);
    $('<label/>', { text: 'Adultos ' }).appendTo('#quants' + qtdQ);
    $('<div/>', { id: "quantsAux" + qtdQ, name: "quantsAux" + qtdQ }).appendTo('#quants' + qtdQ);
    $('<a/>', { href: 'javascript:', onclick: "MaisHotel('ddlQ" + qtdQ + "Adulto')" }).appendTo('#quantsAux' + qtdQ).attr('class', 'plusHotel');
    $('#quantsAux' + qtdQ).attr("class","plusHotel");
    $('<input/>', {
        type: "text",
        value: "2",
        id: 'ddlQ' + qtdQ + 'Adulto',
        name: 'ddlQ' + qtdQ + 'Adulto'
    }).appendTo('#quantsAux' + qtdQ);
    $('<a/>', { href: 'javascript:', onclick: "MenosHotel('ddlQ" + qtdQ + "Adulto')" }).appendTo('#quantsAux' + qtdQ).attr('class', 'minusHotel');
    $('#quantsAux' + qtdQ).attr("class","minusHotel");
    $('<div/>', {
        id: "quantsCri" + qtdQ,
        name: "quantsCri" + qtdQ
    }).appendTo('#agrpQuarto' + qtdQ).attr("class", "quants quantsQuartosAereo");
    $('#agrpQuarto' + qtdQ);
    $('<label/>', {
        text: 'Crianças '
    }).appendTo('#quantsCri' + qtdQ);
    $('<div/>', {
        id: "quantsCriAux" + qtdQ,
        name: "quantsCriAux" + qtdQ
    }).appendTo('#quantsCri' + qtdQ).attr("class", "quants");
    $('<a/>', { href: 'javascript:', onclick: "aumentarIdadeHotel('ddlQ" + qtdQ + "Crianca','mais'," + qtdQ + ")" }).appendTo('#quantsCriAux' + qtdQ).attr('class', 'plusHotel');
    $('#quantsCriAux' + qtdQ).attr("class","plusHotel");
    $('<input/>', {
        type: "text",
        value: "0",
        id: 'ddlQ' + qtdQ + 'Crianca',
        name: 'ddlQ' + qtdQ + 'Crianca'
    }).appendTo('#quantsCriAux' + qtdQ);
    $('<a/>', { href: 'javascript:', onclick: "aumentarIdadeHotel('ddlQ" + qtdQ + "Crianca','menos'," + qtdQ + ")" }).appendTo('#quantsCriAux' + qtdQ).attr('class', 'minusHotel');
    $('#quantsCriAux' + qtdQ).attr("class","minusHotel");
    $('<br/>').appendTo('#agrpQuarto' + qtdQ);
    $("#qtdQuartos").val(parseInt($("#qtdQuartos").val()) + 1);
    try {
        AjusteHeightFrameParentParent("iframeBuscadorHotel", "iframes", 13);
    } catch (e) {
    }
    if($('#DivPai2') != null) {
        $('#DivPai2').html('');
    }
}