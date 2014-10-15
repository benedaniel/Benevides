function AlteraStatusUsuario(ID_PESSOA, NM_PESSOA, ID_EMPRESA) {
    var args = new Object();
    args = {
        ID_PESSOA: ID_PESSOA,
        NM_PESSOA: NM_PESSOA,
        ID_EMPRESA: ID_EMPRESA
    }
    $.ajax({
        url: "/PainelControle/AlteraStatusUsuario",
        type: "POST",
        beforeSend: function () { showdivLoad(); },
        complete: function () { hidedivLoad(); },
        data: JSON.stringify(args),
        datatype: 'html',
        contentType: 'application/json',
        success: function (data) {
            showMensagem("Lista de Usuários", data, 500);
        },
        error: function (data) {
            showMensagem("Lista de Usuários", data, 500);
        }
    });

    $("#btnMsgOk").click(function () {
        window.location.reload(true);
    });

}