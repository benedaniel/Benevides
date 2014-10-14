$(document).ready(function () {
    //Inicio Buscadores
    //Pacote
    $.ajax({
        url: '/Pacote/PacoteForm',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#BuscaPac").html(data);
        },
        error: function (data) {
            $("#BuscaPac").remove();
             
        }
    });
    //Aero
    $.ajax({
        url: '/Aereo/AereoForm',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#BuscaAereo").html(data);
        },
        error: function (data) {
            $("#BuscaAereo").remove();
             
        }
    });
    ////Hotel
    //$.ajax({
    //    url: '/Hotel/HotelForm',
    //    type: 'POST',
    //    dataType: 'html',
    //    contentType: 'application/json',
    //    success: function (data) {
    //        $("#BuscaHotel").html(data);
    //    },
    //    error: function (data) {
    //        $("#BuscaHotel").remove();
             
    //    }
    //});
    //Carro
    $.ajax({
        url: '/Carro/CarroForm',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#BuscaCar").html(data);
        },
        error: function (data) {
            $("#BuscaCar").remove();
             
        }
    });
    //Atividade
    $.ajax({
        url: '/Atividade/AtividadeForm',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#BuscaAtv").html(data);
        },
        error: function (data) {
            $("#BuscaAtv").remove();
             
        }
    });
    ////Monte Pacote
    //$.ajax({
    //    url: '/MontePacote/MPFormMulti',
    //    type: 'POST',
    //    dataType: 'html',
    //    contentType: 'application/json',
    //    success: function (data) {
    //        $("#BuscaMP").html(data);
    //    },
    //    error: function (data) {
    //        $("#BuscaMP").remove();
             
    //    }
    //});
    //Final Buscadores

    //Destaque Hoteis
    $.ajax({
        url: '/Hotel/HotelDestaque',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#HotelDestakNasc").html(data);
        },
        error: function (data) {
            $("#HotelDestakNasc").remove();
             
        }
    });

    $.ajax({
        url: '/Hotel/HotelDestaqueInternacional',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#HotelDestakInter").html(data);
        },
        error: function (data) {
            $("#HotelDestakInter").remove();
             
        }
    });
    //Final Destaque Hoteis

    //Banner Destaque
    $.ajax({
        url: '/Pacote/PacoteDestaque',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#bannerDestaque").html(data);
        },
        error: function (data) {
            $("#bannerDestaque").remove();
             
        }
    });
    //Final Banner Destaque
  
    //Pacotes
    $.ajax({
        url: '/Pacote/PacoteDestaquesNacionais',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#PacotesNasc").html(data);
        },
        error: function (data) {
            $("#PacotesNasc").remove();
             
        }
    });


    $.ajax({
        url: '/Pacote/PacoteDestaquesInternacionais',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#PacotesInter").html(data);
        },
        error: function (data) {
            $("#PacotesInter").remove();
             
        }
    });
    //Final Pacotes

    //Inferiores
    $.ajax({
        url: '/Pacote/PacoteDestaquesInferior',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#PacoteDestaquesInferior").html(data);
        },
        error: function (data) {
            $("#PacoteDestaquesInferior").remove();
             
        }
    });

    $.ajax({
        url: '/Hotel/HotelDestaqueInferior',
        type: 'POST',
        dataType: 'html',
        contentType: 'application/json',
        success: function (data) {
            $("#HotelDestaqueInferior").html(data);
        },
        error: function (data) {
            $("#HotelDestaqueInferior").remove();
             
        }
    });
    //Final Inferiores
});