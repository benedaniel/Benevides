var MapaAtual = null,
ImageHome = { url: '/Images/home_mapa.png', size: new google.maps.Size(32, 32) },
ImageNaoPreferencial = { url: '/Images/beachflag.png', size: new google.maps.Size(55, 74) },
ImagePreferencial = { url: '/Images/beachflag-preferencial.png', size: new google.maps.Size(67, 74) },
ImageFranquia = { url: '/Images/beachflagFranquia.png', size: new google.maps.Size(67, 74) },
ShapeFranquia = { coords: [0, 0, 55, 55], type: 'rect' },
ShapeHome = { coords: [0, 0, 32, 32], type: 'rect' },
ShapeNaoPreferencial = { coords: [0, 0, 55, 55], type: 'rect' },
ShapePreferencial = { coords: [0, 0, 67, 55], type: 'rect' },
InfoWindow = null,
MarcadorHome = null,
markers = new Array(),
TodasAgenciasNoMapa = new Array(),
TodosIndicesDasAgenciasNoMapa = new Array(),
TotalAgenciasDaUltimaChamada = 0,
ContadorDeCarregamento = 0,
ContadorDeMarker = 0,
EstaCarregandoAjax = false,
VerificarSeEExecutarTimers = new Array(),
changeTabTo = function (id) {
    $('div.agencias-pesquisa').hide();
    $('div#' + id).show();
},
verificarSeEExecutar = function (se, executar, seArgumentos, executarArgumentos, tempoDoIntervalo) {

    if (se == null || typeof se != 'function' || executar == null || typeof executar != 'function')
        return;

    if (VerificarSeEExecutarTimers == null || !(VerificarSeEExecutarTimers instanceof Array))
        VerificarSeEExecutarTimers = new Array();

    if (typeof tempoDoIntervalo != 'number' || tempoDoIntervalo <= 0)
        tempoDoIntervalo = 300;

    if (seArgumentos == null || !(seArgumentos instanceof Array))
        seArgumentos = [seArgumentos];

    if (executarArgumentos == null || !(executarArgumentos instanceof Array))
        executarArgumentos = [executarArgumentos];


    var fn = function () {
        if (se.apply(undefined, seArgumentos)) {
            clearInterval(VerificarSeEExecutarTimers.shift());
            executar.apply(undefined, executarArgumentos);
        }
    };

    var timer = setInterval(fn, tempoDoIntervalo);
    VerificarSeEExecutarTimers.push(timer);
},
formataDistancia = function (distanciaEmMetros) {
    if (!isNaN(distanciaEmMetros)) {
        var medida = 'm';
        distanciaEmMetros = parseFloat(distanciaEmMetros);

        if (distanciaEmMetros >= 1000) {
            medida = 'km';
            distanciaEmMetros /= 1000;
        }

        if (distanciaEmMetros > 0)
            return (distanciaEmMetros + ' ' + medida).replace(/\./g, ',');
    }

    return 'Indisponível';
},
listaAgenciasNaTelaENoMapa = function (json) {
    TotalAgenciasDaUltimaChamada = json.length;
    console.log('início da função');
    setTimeout((function (json) {
        var agencias = new Array(), index, agencia;
        var localizacaoCEP = null;
        console.log('início do setTimeout');

        for (var i = 0; i < json.length; i++) {
            index = TodosIndicesDasAgenciasNoMapa[json[i].Latitude + '-' + json[i].Longitude];
            agencia = TodasAgenciasNoMapa != null && index < TodasAgenciasNoMapa.length ? TodasAgenciasNoMapa[index] : null;
            // Localização do usuário pelo CEP ou endereço
            if (json[i].DistanciaMetros == -999) {
                if (localizacaoCEP == null)
                    localizacaoCEP = json[i];
                continue;
            }

            if (agencia != null) {
                agencia.DistanciaMetros = json[i].DistanciaMetros;
                agencias.push(agencia);
            }
        }

        listaAgenciasNaTela(agencias);
        console.log(localizacaoCEP);

        if (agencias != null && agencias.length > 0 && MapaAtual != null && localizacaoCEP == null) {
            var index = parseInt(agencias.length / 2),
                zoom = agencias.length <= 3 ? 13 : agencias.length <= 10 ? 12 : 10;

            MapaAtual.setCenter(new google.maps.LatLng(agencias[index].Latitude, agencias[index].Longitude));
            MapaAtual.setZoom(zoom);
        }// Localização do usuário pelo CEP ou endereço (centraliza no mapa)
        else if (localizacaoCEP != null) {
            var lat = parseFloat(localizacaoCEP.Latitude), lng = parseFloat(localizacaoCEP.Longitude);
            console.log('lat: ' + lat + ' - lng: ' + lng);

            if (lat != 0 && lng != 0) {
                verificarSeEExecutar(
                    /*SE*/
                    function () {
                        return !EstaCarregandoAjax;
                    },
                    /*EXECUTAR*/
                    function (localizacaoCEP) {
                        console.log('inserir marcador');
                        inserirMarcadorNoMapa(localizacaoCEP, ImageHome, ShapeHome, 1, true);
                        console.log('centraliza em: ' + localizacaoCEP.Latitude + ' : ' + localizacaoCEP.Longitude);
                        centralizarEmLatLng(localizacaoCEP.Latitude, localizacaoCEP.Longitude);
                    },
                    /*SE ARGUMENTS*/
                    null,
                    /*EXECUTAR ARGUMENTS*/
                    localizacaoCEP
                );
            }
        }

        $('#agencias-resultado-mapa').show();
        $('#agencias-resultado-sem-resultados').hide();

    }).bind(undefined, json), 10);//*/
},
listaAgenciasNaTela = function (json) {
    var template = '<div class="agencias-resultado-agencia">' +
                       '<div class="agencias-resultado-left"><ul>' +
                           '<li class="agencias-resultado-nome">{0}</li>' +
                           '{1}' +
                           '{2}' +
                           '{3}' + /*
                           '<li class="agencias-resultado-email"><strong>E-mail: </strong>Indisponível</li>' +*/
                        '</ul></div>' +
                        '<div class="agencias-resultado-right"><ul>' +
                            '<li class="agencias-resultado-loja-mapa"><a href="javacript:void(\'Mostrar a agência no mapa\');" onclick="mostrarLatLngNoMapa(\'{4}\', \'{5}\');">Ver Loja no Mapa</a></li>' +
                            '<li style="display:none;" class="agencias-resultado-loja-como-chegar"><a href="">Como chegar</a></li>' +
                        '</ul></div><div class="clear"></div>' +
                    '</div>' +
                    '<hr class="agencias-resultado-agencia" />',
        liDistancia = '<li class="agencias-resultado-distancia">Distância: {0}</li>',
        liEndereco = '<li class="agencias-resultado-endereco">{0}, {1} - {2}</li>',
        liTelefone = '<li class="agencias-resultado-telefone"><strong>Telefone: </strong>{0}</li>';
    idDivP = '#agencias-resultado-agencias-preferenciais',
    idDivNP = '#agencias-resultado-agencias-nao-preferenciais',
    idDivFR = '#agencias-resultado-agencias-franquias';

    TotalAgenciasDaUltimaChamada = json.length;
    ContadorDeCarregamento = 0;
    $('div' + idDivP + ', div' + idDivNP + ', div' + idDivFR).html(' ');

    // Esconde o menu de carregamento e mostra todo o resto e desbloqueia os campos para próxima requisição
    if (TotalAgenciasDaUltimaChamada <= 0)
        mostrarCarregando(false);
    else {
        var quantidadeMaxima = 30,
            contadorPreferencial = 0,
            contadorNaoPreferencial = 0;
        contadorFranquia = 0;
        var siteName = getSiteName();
        for (var i = 0, timeToShow = 0;
            i < json.length && (contadorPreferencial <= quantidadeMaxima || contadorNaoPreferencial <= quantidadeMaxima) ;/* limitador de agências */
            i++) {

            var agencia = json[i], idDiv;

            if (ehFranquia(agencia, siteName)) {
                idDiv = idDivFR;

                if (contadorFranquia > quantidadeMaxima)
                    continue;
                else
                    contadorFranquia++;
            }
            else if (ehPreferencial(agencia)) {
                idDiv = idDivP;

                if (contadorPreferencial > quantidadeMaxima)
                    continue;
                else
                    contadorPreferencial++;
            } else {
                idDiv = idDivNP;

                if (contadorNaoPreferencial > quantidadeMaxima)
                    continue;
                else
                    contadorNaoPreferencial++;
            }

            setTimeout((function (agencia, template, liDistancia, liEndereco, liTelefone, idDiv) {
                var conteudo = $.format(template,
                                        agencia.Titulo,
                                        agencia.DistanciaMetros > 0 ? $.format(liDistancia, formataDistancia(agencia.DistanciaMetros)) : '',
                                        $.format(liEndereco, agencia.Endereco, agencia.Cidade, agencia.Estado),
                                        $.format(liTelefone, agencia.Telefone),
                                        agencia.Latitude, agencia.Longitude);

                $('div' + idDiv).append(conteudo);

                //console.log(conteudo + "   -----------------    " + ContadorDeCarregamento);

                ContadorDeCarregamento++;
            }).bind(undefined, agencia, template, liDistancia, liEndereco, liTelefone, idDiv), timeToShow);

            if (i > 0 && (i % 10) == 0 && timeToShow <= 500)
                timeToShow += 100;
        }

        verificarSeEExecutar(
            /*SE*/
            function (todosCarregados) {
                return ContadorDeCarregamento >= todosCarregados;
            },
            /*EXECUTAR*/
            function (countPreferencial, countNPreferencial) {
                var preferenciais = '#agencias-resultado-agencias-preferenciais',
                    naoPreferenciais = '#agencias-resultado-agencias-nao-preferenciais',
                    naoEncontrado = '#agencias-resultado-nao-encontrado',
                    franquias = '#agencias-resultado-agencias-franquias';

                if (countPreferencial <= 0)
                    $(preferenciais).html('<div class="agencias-resultado-nao-encontrado">' + $(naoEncontrado).html() + '</div>');

                if (countNPreferencial <= 0)
                    $(naoPreferenciais).html('<div class="agencias-resultado-nao-encontrado">' + $(naoEncontrado).html() + '</div>');

                if (contadorFranquia <= 0)
                    $(franquias).html('<div class="agencias-resultado-nao-encontrado">' + $(naoEncontrado).html() + '</div>');

                mostrarCarregando(false);
            },
            /*SE ARGUMENTS*/
            contadorPreferencial + contadorNaoPreferencial,
            /*EXECUTAR ARGUMENTS*/
            [contadorPreferencial, contadorNaoPreferencial]
        );
    }
},
ehFranquia = function (agencia, siteName) {

    if (agencia.IndicadorFranquia == 'S')
        return true;
    else if (siteName == 'viagens-a-negocios' || siteName == 'consolidadora' || siteName == 'eventos-e-incentivo' || siteName == 'franchising')
        return true;
}
ehPreferencial = function (agencia) {
    var codPreferencial = typeof agencia.CodigoPreferencial == 'number' ?
                        agencia.CodigoPreferencial :
                        !isNaN(agencia.CodigoPreferencial) ? parseInt(agencia.CodigoPreferencial) : 0;
    return codPreferencial == 1 || codPreferencial == 3;
},
fnNaoFoi = function (txt, json) {
    alert('Ocorreu algum erro ao tentar buscar a agência\nPor favor tente novamente mais tarde!');
    mostrarCarregando(false);
},
centralizarEmLatLng = function (latitude, longitude) {
    if (MapaAtual != null && !EstaCarregandoAjax) {
        MapaAtual.setCenter(new google.maps.LatLng(latitude, longitude));
        MapaAtual.setZoom(15);

        var x = $('#agencias-resultado-mapa').offset().top - 50;
        $('html,body').animate({ scrollTop: x }, 500);
    }
    else
        console.log('não pode centralizar por que estava carregando');
},
inicializarMapa = function () {
    var centerObj = { 'Latitude': '-23.5528462', 'Longitude': '-46.6590849' };
    var mapOptions = {
        zoom: 15,
        flat: false,
        center: new google.maps.LatLng(centerObj.Latitude, centerObj.Longitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        overviewMapControl: true,
        overviewMapControlOptions: { opened: true }
    }
    MapaAtual = new google.maps.Map(document.getElementById("mapa"), mapOptions);
    //google.maps.visualRefresh = false;
    inserirAgenciasNoMapa(centerObj);
},
inserirAgenciasNoMapa = function (agencias) {

    if (agencias != null && !(agencias instanceof Array))
        agencias = new Array(agencias);

    //agencias.sort(function (a, b) {
    //    return a.codPreferencial == 1 || a.codPreferencial == 3;
    //});

    //agencias.sort(function (a, b) {
    //    return a.IndicadorFranquia < b.IndicadorFranquia;
    //});

    //agencias.reverse();

    var image, shape, isPreferencial, timeToShow = 0;
    var siteName = getSiteName();
    for (var i = 0; i < agencias.length; i++) {
        isPreferencial = ehPreferencial(agencias[i]);
        isFranquia = ehFranquia(agencias[i], siteName);
        var zindexAg = i;
        if (isFranquia) {
            image = ImageFranquia;
            shape = ShapeFranquia;
            zindexAg = zindexAg + 100000;

        } else if (isPreferencial) {
            image = ImagePreferencial;
            shape = ShapePreferencial;
            zindexAg = zindexAg + 50000;
        }
        else {
            image = ImageNaoPreferencial;
            shape = ShapeNaoPreferencial;
        }

        setTimeout((function (agencia, image, shape, zindexAg, length) {

            inserirMarcadorNoMapa(agencia, image, shape, zindexAg);

            if ((++ContadorDeMarker) >= length) {
                try {
                    var mcOptions = { gridSize: 20, maxZoom: 13 };
                    var mc = new MarkerClusterer(MapaAtual, markers, mcOptions);
                } catch (e) { }
            }
        }).bind(undefined, agencias[i], image, shape, zindexAg, agencias.length), timeToShow);

        if (i > 0 && (i % 10) == 0 && timeToShow <= 2000)
            timeToShow += 100;
    }
},
inserirMarcadorNoMapa = function (agencia, image, shape, zIndex, naoMostrarConteudoAoClicar) {
    var myLatLng = new google.maps.LatLng(agencia.Latitude, agencia.Longitude);

    var marker = new google.maps.Marker({
        position: myLatLng,
        map: MapaAtual,
        //shadow: shadow,
        icon: image,
        shape: shape,
        title: agencia.Titulo != null ? agencia.Titulo : '',
        zIndex: zIndex
    });

    agencia.marker = marker;

    if (!naoMostrarConteudoAoClicar)
        google.maps.event.addListener(marker, 'click', (function (agencia) {
            return function () {
                mostrarConteudoNoMapa(agencia);
            };
        })(agencia));

    markers.push(marker);
},
mostrarConteudoNoMapa = function (agencia) {
    var hasContent = agencia.Titulo != null && agencia.Endereco != null;

    if (hasContent) {
        var content = !hasContent ? '' : ('<div class="map-content">' +
                        '<h3 style="False">{0}</h3>' +
                        '{5}' +
                        '<span class="main-details-category">' +
                            '<span style="display: none">63</span>' +
                            '<p>{1}, {2}, {3} - {4}</p>' +
                            '<span class="hotel-localization"></span>' +
                        '</span>' +
                      '</div>');
        //content += '<a class="see-details" onclick="VerDetalheHotel(' + locationsi[0][7] + ',' + locationsi[0][8] + ',' + locationsi[0][9] + ',' + localizacao[0][10] + ',' + localizacao[0][11] + ',' + localizacao[0][12] + ')" href="#">Ver detalhes</a>';
        content = $.format(content, [agencia.Titulo, agencia.Endereco, agencia.Cidade, agencia.Estado,
                                    (agencia.Telefone || ''),
                                    agencia.Logo != null && agencia.Logo != '' ? '<img  style="margin:auto" src="http://extranet.flytourviagens.com.br/pls/flytour/arquivos/' + agencia.Logo + '" />' : '']);

        InfoWindow.setContent(content);
        InfoWindow.open(MapaAtual, agencia.marker);
    }
},
mostrarAgenciaNoMapa = function (agencia) {
    if (agencia != null) {
        if (agencia.marker != null)
            mostrarConteudoNoMapa(agencia);

        if (agencia.Latitude != 0 && agencia.Longitude != 0)
            centralizarEmLatLng(agencia.Latitude, agencia.Longitude);
    }
},
mostrarLatLngNoMapa = function (latitude, longitude) {
    var index = TodosIndicesDasAgenciasNoMapa[latitude + '-' + longitude];
    var agencia = TodasAgenciasNoMapa != null && index < TodasAgenciasNoMapa.length ?
            TodasAgenciasNoMapa[index] :
            { Latitude: (latitude || 0), Longitude: (longitude || 0) };

    mostrarAgenciaNoMapa(agencia);
},
preencherCidades = function (json) {
    var cidades = new Array();
    cidades.push('<option value="">Selecione uma cidade</option>');

    $.each(json, function (key, val) {
        cidades.push('<option value="' + val + '">' + val + '</option>');
    });

    $('#cidade').html(cidades.join(''));
    $('#cidade').removeAttr('disabled');
},
getSiteName = function () {
    return location.href.replace(/^http[s]*\:\/\/[a-zA-Z0-9\:\.\/]+\/([a-zA-Z\-]+)\/institucional\-onde\-estamos[\/]*$/i, '$1');
},
preencheArrayAgencias = function (agencias) {
    if (agencias != null && agencias.length > 0) {

        for (var i = 0; i < agencias.length; i++) {

            var agencia = agencias[i];
            TodosIndicesDasAgenciasNoMapa[agencia.Latitude + '-' + agencia.Longitude] = i;
            TodasAgenciasNoMapa[i] = agencia;
        }
    } else
        TodasAgenciasNoMapa = new Array();
},
mostrarCarregando = function (sim) {
    var camposBloqueados = '.agencias-pesquisa input, .agencias-pesquisa select',
        mapa = '#agencias-resultado-mapa',
        semResultados = '#agencias-resultado-sem-resultados',
            naoEncontrado = '#agencias-resultado-nao-encontrado',
            loader = '#agencias-resultado-loader',
        tabs = '#tabs',
            preferenciais = '#agencias-resultado-agencias-preferenciais',
            naoPreferenciais = '#agencias-resultado-agencias-nao-preferenciais';

    EstaCarregandoAjax = sim;

    if (sim) {
        $(camposBloqueados).css('background-color', '#ccc').attr('disabled', 'disabled');
        $([mapa, tabs, naoEncontrado].join(', ')).hide();
        $([semResultados, loader].join(', ')).show();
    } else {
        $(camposBloqueados).css('background-color', '').removeAttr('disabled');

        if (TotalAgenciasDaUltimaChamada > 0) {
            $(semResultados).hide();
            $([mapa, tabs].join(', ')).show('fast', (function (mapa, tabs) {
                var x = $(mapa).offset().top - 50;
                $('html,body').animate({ scrollTop: x }, 500);
                $(tabs).tabs("option", "active", 0);
            }).bind(undefined, mapa, tabs));

        } else {
            $([semResultados, naoEncontrado].join(', ')).show();
            $([mapa, tabs, loader].join(', ')).hide();
        }
    }
},
mostrarCarregandoTrue = function () { mostrarCarregando(true) };

$(document).ready(function () {
    InfoWindow = new google.maps.InfoWindow();

    $('#estado').change(function () {
        var siteName = getSiteName();

        $('#cidade').html('<option value="">Caregando...</option>');
        $('#cidade').attr('disabled', 'disabled');

        if ($(this).val() != '')
            $.getJSON('/OndeEstamos/GetCidadesPorEstado/', { 'estado': this.value, 'siteName': siteName }, preencherCidades);
        else
            preencherCidades(new Array());
    });

    $('.link-pesquisa').click(function () {
        if (EstaCarregandoAjax)
            return;

        changeTabTo('pesquisa-por-' + this.rel);
        $('.link-pesquisa').removeClass('active');
        $('.link-pesquisa[rel=' + this.rel + ']').addClass('active');
    });

    $('.link-pesquisa-mostra-todos').click(function () {
        if (EstaCarregandoAjax)
            return;

        if (TodasAgenciasNoMapa != null && TodasAgenciasNoMapa.length > 0) {
            MapaAtual.setZoom(3);

            if (TotalAgenciasDaUltimaChamada != TodasAgenciasNoMapa.length) {
                mostrarCarregandoTrue();
                $('#cEPOuEndereco, #cidade, #estado, #nome').val('');
                $('#agencias-resultado-mapa').show('fast', function () {
                    var x = $('#agencias-resultado-loader').offset().top - 50;
                    $('html,body').animate({ scrollTop: x }, 500);
                });
                listaAgenciasNaTela(TodasAgenciasNoMapa);
            }
        } else {
            TotalAgenciasDaUltimaChamada = 0;
            mostrarCarregando(false);
        }
    });
    $("#tabs").tabs();
    mostrarCarregandoTrue();
    //*
    $.getJSON('/OndeEstamos/GetTodasAgenciasNoMapa/', { 'siteName': getSiteName() }, function (json) {

        if (json.length > 0) {
            inicializarMapa();

            $('#agencias-resultado-mapa').show('slow', function () {
                var x = $('#agencias-resultado-loader').offset().top - 50;
                //$('html,body').animate({ scrollTop: x }, 500);
            });
            inserirAgenciasNoMapa(json);
            MapaAtual.setZoom(3);
            preencheArrayAgencias(json);
            listaAgenciasNaTela(json);
        } else {
            preencheArrayAgencias(new Array());
            listaAgenciasNaTela(new Array());
        }
    });

    //*/
});