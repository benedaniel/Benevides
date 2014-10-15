$(function () {
    $(".rdbPerfil").click(function () {
        var valSelect = $(this).val();
        if (valSelect == 159) {
            $(".divradios[title='Agencia'] fieldset").hide()
        }
        else {
            $(".divradios[title='Agencia'] fieldset").show()
        }
    });
});