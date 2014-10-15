function MenuExibicaoB2C(b2c) {
    if (b2c == "s") {
        
        $("#lnkReserva").attr("href", "/Venda");
        $("#divMenuPainelControle li ul").css("height", "118");
        $("#divReservas a").html("Minhas Reservas");
    }
    else {
        $("#divReservas a").html("Reservas da Agência");
    }
}

// perfil N = Atendente, S = Master e "" = Cliente
function ControleExibicaoItenMenu(perfil) {
    if (perfil == "N") {
        $(".A").show();
    } else if (perfil == "S") {
        $(".M").show();
    }
    else {
        $(".C").show();
    }
}

function ControleExibicaoMenu() {
    var lstLinkds = new Array();
    lstLinkds.push("/PainelControle/DadosUsuario");
    lstLinkds.push("/PainelControle/UsuarioAgencia");
    lstLinkds.push("/PainelControle/UsuarioNovo");
    lstLinkds.push("/PainelControle/ListaUsuario");
    lstLinkds.push("/PainelControle/Reserva");
    lstLinkds.push("/PainelControle/Extrato");
    lstLinkds.push("/Orcamento/ListarOrcamento");
    lstLinkds.push("/Venda");

    var lstClass = new Array();
    lstClass.push("meucadastro");
    
    lstClass.push("dadosdaagencia");
    lstClass.push("adicionarusuario");
    lstClass.push("listadeusuarios");
    lstClass.push("reservasdocliente");
    lstClass.push("extrato");
    lstClass.push("orcamento");
    lstClass.push("minhasreservas");

    $.each(lstLinkds, function (key, value) {
            
        var args = new Object();
        args = { url:value };
        $.ajax({
            url: "/PainelControle/GetPermissaoPagina",
            type: "POST",
            beforeSend: function () {}, //showdivLoad(); Estava dando conflito com outras funcionalidades, qqr coisa me fala , Stringher },
            complete: function () {},// hidedivLoad(); Estava dando conflito com outras funcionalidades, qqr coisa me fala , Stringher},
            data: JSON.stringify(args),
            datatype: 'html',
            contentType: 'application/json',
            success: function (data) {
                if(data){
                    $("." + lstClass[key]).show();
                }
            },
            error: function (data) {
                // Estava dando conflito com outras funcionalidades, qqr coisa me fala , Stringher   showMensagem("Lista de Usuários", data, 500);
            }
        });

    });
}

