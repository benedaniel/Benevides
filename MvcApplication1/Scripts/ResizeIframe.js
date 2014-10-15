
function AjusteHeightFrame(frame) {
    $("#" + frame).height(0);
    var x = document.getElementById(frame);
    var y = (x.contentWindow || x.contentDocument);
    if (y.document) y = y.document;
    var newHeight = y.body.scrollHeight;
    $("#" + frame).height(newHeight + 5);
}

function AjusteHeightFrameParent(frame, altura) {
    var newHeight = 0;
    $("#" + frame).height(newHeight);
    //newHeight = $("#"+frame, window.parent.document).height();
    var x = window.parent.document.getElementById(frame);
    var y = (x.contentWindow || x.contentDocument);
    if (y.document) y = y.document;
    newHeight = y.body.scrollHeight;
    $("#" + frame, window.parent.document).height(newHeight + altura);
}

function AjusteHeightFrameParentParent(primeiro, segundo, altura) {
    var novoHeight = 0;
    var antesHeight = $("#" + primeiro, window.parent.document).height();
    $("#" + primeiro).height(novoHeight);

    var x = window.parent.document.getElementById(primeiro);
    var y = (x.contentWindow || x.contentDocument);
    if (y.document) y = y.document;
    novoHeight = y.body.scrollHeight;

    var heightAdicionado = novoHeight - antesHeight;

    $("#" + primeiro, window.parent.document).height(novoHeight + altura);
    if ($("#" + segundo, window.parent.parent.document) != undefined) {
        $("#" + segundo, window.parent.parent.document).height($("#" + segundo, window.parent.parent.document).height() + heightAdicionado + altura);
    }
}
