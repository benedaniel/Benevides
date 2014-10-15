
//$('#newsletterForm').validate({
//    rules: {
//        txtNomeNews: {
//            required: {
//                depends: function (element) {
//                    return $('#txtNomeNews').val() != "Nome" || $('#txtNomeNews').val() != ""
//                }
//            }
//        },
//        txtMailNews: {
//            required: true,
//            email: true
//        }
//    },
//    messages: {
//        txtNomeNews: 'Por favor selecionar um local de embarque',
//        txtMailNews: {
//            required: 'Por favor preencher o email',
//            email: 'Informar email válido'
//        }
//    }
//});

function sendNews() {

    var email = $("#txtMailNews").val();
    var nome = $("#txtNomeNews").val();

    if (nome == null || nome == "" || nome == "Nome" || nome == "Campo obrigatório") {
        $('.watermark').watermark('clearWatermarks');
        $("#txtNomeNews").attr("style", "border:solid red 1px; color:red");
        $("#txtNomeNews").attr("title", "Campo obrigatório");

        $('.watermark').watermark();

        nome = "";
    }

    if (email == null || email == "" || email == "E-mail" || email == "Campo obrigatório" || email == "E-mail inválido" || checkmail(email)) {
        $('.watermark').watermark('clearWatermarks');
        $("#txtMailNews").attr("style", "border:solid red 1px; color:red");
        $("#txtMailNews").attr("title", "Campo obrigatório");

        $('.watermark').watermark();

        email = "";
    }

    if (nome != "" && email != "") {

        var args = new Object();
        args = {
            nome: nome,
            email: email
        };

        $.ajax({
            url: '/Home/SendEmail',
            type: 'POST',
            data: args,
            success: function (result) {
                alert("E-mail cadastrado com sucesso.");
            },
            error: function () {
                alert("Problema ao cadastrar.");
            }
        });
    }
}

function checkmail(email) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    if (!pattern.test(email)) {
        $("#txtMailNews").val("");
        $('.watermark').watermark('clearWatermarks');
        $("#txtMailNews").attr("style", "border:solid red 1px; color:red");
        $("#txtMailNews").attr("title", "E-mail inválido");
        $('.watermark').watermark();
        return true;
    }
    else { return false; }
}