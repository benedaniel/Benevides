function getCities(id) {
    insertOptionsToCidade('<option value="">Carregando..</option>', true);

    $.ajax({
        url: "/SitesGrupo/GetCidades",
        data: { id: id },
    dataType: "json",
    type: "POST",
    error: function () {
        alert("Ocorreu algum erro ao tentar buscar as cidades.");
        insertOptionsToCidade('<option value="">Selecione..</option>');
    },
    success: function (data) {
        var items = '<option value="">Selecione..</option>';
        $.each(data, function (i, item) {
            items += "<option value=\"" + item.NM_CIDADE + "\">" + item.NM_CIDADE + "</option>";
        });

        insertOptionsToCidade(items);
    }
});
}

function insertOptionsToCidade(html, disable){
    $("#Cidade").html(html);

    if(disable)
        $("#Cidade").attr('disabled', 'disabled');
    else
        $("#Cidade").removeAttr('disabled');
}


$(document).ready(function () {
    $("#Estado").change(function () {
        var valor = $("#Estado").val(),
            id, valores = valor.split('|');
        id = valores.length > 1 ? parseInt(valores[0]) : 0;

        if (id > 0)
            getCities(id);
        else
            insertOptionsToCidade('<option value="">Selecione..</option>');
    });
});