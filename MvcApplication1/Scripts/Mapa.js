//Criar mapa.

//1º chamada da Busca ex: BuscaHoteisMapa(Hotel);  OBS: Hotel é um object ou Json
//2º Preencher dados para ser exibido ao clicar no icone do endereço EX: PreencheHoteis
//3º Initialize EXPECIFICO 
//4º SetMarkers Expecifico

var latitudoExemplo = -23.553867,
    longitudeExemplo = -46.660632;
function BuscaHoteisMapa(Hotel) {
    PreencheHoteis(Hotel, 0);
    showModal(0);
    initialize();
}

function BuscaAgenciasMapa(arr, cep) {
    PreencheAgencia(arr);
    showModal(2);
    initializeAgencia(cep);
}

function BuscaHoteisMapaComZoom(Hotel, long, lat, indice) {
    PreencheHoteis(Hotel, indice);
    showModal(0);
    initializeComZoom(lat, long);
}

function BuscaHoteisMapaComStreetView(Hotel, long, lat) {
    PreencheHoteis(Hotel, 0);
    showModal(1);
    initializeStreetView(lat, long);
}

function BuscaHoteisMapaMP(Hotel) {
    PreencheHoteis(Hotel, 0);
    showModal(3);
    initialize();
}

function BuscaHoteisMapaComZoomMP(Hotel, long, lat, indice) {
    PreencheHoteis(Hotel, indice);
    showModal(3);
    initializeComZoom(lat, long);
}

function showModal(indice) {
    if (indice == 1) {
        $('.hoteisMapaStreet').modal({
            minHeight: 635,
            minWidth: 735
        });
    } else if (indice == 2) {
        $('.agenciasMapa').modal({
            minHeight: 635,
            minWidth: 735
        });
    } else if (indice == 3) {
        $('.hoteisMapa').ShowModalPopup({
            minHeight: 635,
            minWidth: 735            
        });
    } else {
        $('.hoteisMapa').modal({
            minHeight: 635,
            minWidth: 735
        });
    }

}

var map = null;
var infowindow = null;
var hoteis = new Array();
var markers = [];

function PreencheHoteis(ListaHoteis, indicelista) {
    hoteis = [];
    markers = [];
    if (indicelista == 0) {
        for (var i = 0; i < ListaHoteis.length; i++) {
            hoteis[i] = new Array(ListaHoteis[i].Titulo, ListaHoteis[i].Latidude, ListaHoteis[i].Longitude, ListaHoteis[i].Order, ListaHoteis[i].ValorDiaria, ListaHoteis[i].Endereco, ListaHoteis[i].NumeroEstrelas, ListaHoteis[i].CodigoHotel, ListaHoteis[i].Checkin, ListaHoteis[i].Checkout, ListaHoteis[i].CodigoCidade, ListaHoteis[i].QuantCriancas, ListaHoteis[i].QuantAdultos, ListaHoteis[i].Foto);
        }
    }
    else {
        hoteis[0] = new Array(ListaHoteis.NomeHotel, ListaHoteis.Latitude, ListaHoteis.Longitude, 1, ListaHoteis.ValorFinalMenorPreco, ListaHoteis.Endereco, ListaHoteis.NumeroEstrelas, ListaHoteis.CodigoHotel, ListaHoteis.Checkin, ListaHoteis.Checkout, ListaHoteis.CodigoCidade, ListaHoteis.QuantCriancas, ListaHoteis.QuantAdultos, ListaHoteis.Foto);
    }

}

function PreencheAgencia(ListaAgencias) {
    agencias = [];
    markers = [];
    for (var i = 0; i < ListaAgencias.length; i++) {
        agencias[i] = new Array(ListaAgencias[i].Titulo, ListaAgencias[i].Latidude, ListaAgencias[i].Longitude, ListaAgencias[i].Order, ListaAgencias[i].CodigoAgencia, ListaAgencias[i].Endereco, ListaAgencias[i].NomeCidade, ListaAgencias[i].NomeEstado, ListaAgencias[i].Logo);
    }
}
function BuscarNoMapa(end) {

    $.ajax({
        url: 'http://maps.googleapis.com/maps/api/geocode/json?address=' + end + '&sensor=true',
        method: "POST",
        success: function (data) {
            latitude = data.results[0].geometry.location.lat;
            longitude = data.results[0].geometry.location.lng;
            //alert("Lat = " + latitude + "- Long = " + longitude);
            if (latitude != '' && longitude != '') {
                if (agencias.length > 0) {
                    var mapOptions = {
                        zoom: 15,
                        center: new google.maps.LatLng(latitude, longitude),
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        overviewMapControl: true,
                        overviewMapControlOptions: { opened: true }
                    }
                    var map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
                    setMarkersAgencia(map, agencias);
                }
            }
        }
    });
}

function initializeAgencia(zipcode) {
    var latitude = '';
    var longitude = '';
    $.ajax({
        url: "http://maps.googleapis.com/maps/api/geocode/json?components=postal_code:" + zipcode + "&sensor=false",
        method: "POST",
        success: function (data) {
            //CENTRAL (onde o mapa vai abrir centralizado).
            latitude = data.results[0].geometry.location.lat;
            longitude = data.results[0].geometry.location.lng;
            if (latitude != '' && longitude != '') {
                if (agencias.length > 0) {
                    var mapOptions = {
                        zoom: 15,
                        center: new google.maps.LatLng(latitude, longitude),
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        overviewMapControl: true,
                        overviewMapControlOptions: { opened: true }
                    }
                    var map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
                    //SetMarkers é onde será adicionado cada endereço novo
                    setMarkersAgencia(map, agencias);
                }
            }
            else {
                if (agencias.length > 0) {
                    var mapOptions = {
                        zoom: 9,
                        center: new google.maps.LatLng(agencias[0][1], agencias[0][2]),
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        overviewMapControl: true,
                        overviewMapControlOptions: { opened: true }
                    }
                    var map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
                    setMarkersAgencia(map, agencias);
                }
            }
        }

    });

}

function initialize() {
    if (hoteis.length > 0) {
        var mapOptions = {
            zoom: 9,
            center: new google.maps.LatLng(hoteis[0][1], hoteis[0][2]),
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            overviewMapControl: true,
            overviewMapControlOptions: { opened: true }
        }
        var map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
        setMarkers(map, hoteis);
    }
    else {
        var mapOptions = {
            zoom: 9,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            overviewMapControl: true,
            overviewMapControlOptions: { opened: true }
        }

        var map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
        setMarkers(map, hoteis);
    }
}

function initializeComZoom(lat, long) {
    var mapOptions = {
        zoom: 18,
        center: new google.maps.LatLng(lat, long),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        overviewMapControl: true,
        overviewMapControlOptions: { opened: true }
    }
    var map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
    setMarkers(map, hoteis);
}

function initializeStreetView(lat, long) {
    var fenway = new google.maps.LatLng(long, lat);
    var mapOptions = {
        center: fenway,
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(
        document.getElementById('map-canvas'), mapOptions);
    var panoramaOptions = {
        position: fenway,
        pov: {
            heading: 34,
            pitch: 10
        }
    };
    var panorama = new google.maps.StreetViewPanorama(document.getElementById('pano'), panoramaOptions);
    map.setStreetView(panorama);
}

function selecionarAgencia(agencia, codigo) {
    $('#txtAgencia').val(agencia);
    if (!$('#txtAgencia').val()) {
        $('#txtAgencia').addClass('warning');
    }
    else {
        $('#txtAgencia').removeClass('warning');
    }
    $('#hdnAgencia').val(codigo);
    $.modal.close();
}

function setMarkersAgencia(map, locations) {

    var image = new google.maps.MarkerImage('../images/beachflag.png',
       new google.maps.Size(55, 74),
        new google.maps.Point(0, 0),
        new google.maps.Point(0, 0));

    var shadow = new google.maps.MarkerImage('../images/beachflag_shadow.png',
        new google.maps.Size(68, 35),
        new google.maps.Point(0, 0),
        new google.maps.Point(0, 0));

    var shape = {
        //coord: [1, 1, 1, 20, 18, 20, 18, 1],
        coord: [0, 0, 55, 55],
        type: 'circle'
    };

    var infowindow = new google.maps.InfoWindow();
    var aspas = "'";
    //ListaAgencias[0].Titulo
    //ListaAgencias[1].Latidude
    //ListaAgencias[2].Longitude
    //ListaAgencias[3].Order
    //ListaAgencias[4].CodigoAgencia
    //ListaAgencias[5].Endereco
    //ListaAgencias[6].NomeCidade
    //ListaAgencias[7].NomeEstado
    for (var i = 0; i < locations.length; i++) {
        var myLatLng = new google.maps.LatLng(locations[i][1].replace(',', '.'), locations[i][2].replace(',', '.'));
        var content = '<div class="map-content">';
        content += '<h3 style="False">' + locations[i][0] + '</h3>';
        if (locations[i][8] != "") {
            content += '<img  style="margin:auto" src="http://extranet.flytourviagens.com.br/pls/flytour/arquivos/' + locations[i][8] + '" />';
        }
        content += '<span class="main-details-category">';
        content += '<span style="display: none">63</span>';
        content += '<p>' + locations[i][5] + ', ' + locations[i][6] + ', ' + locations[i][7] + '</p>';
        content += '<span class="hotel-localization"></span>';
        content += '<p class="button"> <input type="submit" style="background-color: #00AE29; color: white; cursor:pointer; border-radius: 0px; border: none; font-weight: bold; width: 111px; line-height: 1.5em; font-family: verdana; height: 40px; font-size: 16px;" value="Selecionar" onclick="selecionarAgencia(' + aspas + locations[i][0] + aspas + ', ' + aspas + locations[i][4] + aspas + ')"> </p>';
        //content += '<a class="see-details" onclick="VerDetalheHotel(' + locationsi[0][7] + ',' + locationsi[0][8] + ',' + locationsi[0][9] + ',' + localizacao[0][10] + ',' + localizacao[0][11] + ',' + localizacao[0][12] + ')" href="#">Ver detalhes</a>';
        content += '</div>';

        var marker = new google.maps.Marker({
            position: myLatLng,
            map: map,
            shadow: shadow,
            icon: image,
            shape: shape,
            title: locations[i][0],
            zIndex: locations[i][3]
        });


        google.maps.event.addListener(marker, 'click', (function (marker, content) {
            return function () {
                infowindow.setContent(content);
                infowindow.open(map, marker);
            }
        })(marker, content));

        markers.push(marker);
    }
    var mcOptions = { gridSize: 20, maxZoom: 13 };
    var mc = new MarkerClusterer(map, markers, mcOptions);
}

function setMarkers(map, locationsi) {
    //console.log("Iniciando setMarkers");
    var locations = new Array();

    //var image = new google.maps.MarkerImage('../images/HotelFlag.png',
    //    new google.maps.Size(55, 74),
    //    new google.maps.Point(0, 0),
    //    new google.maps.Point(0, 74));


    var image = {
        url: '/images/HotelFlag.png',
        size: new google.maps.Size(55, 74),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(25, 74)
    };
    
    var shadow = new google.maps.MarkerImage('/images/beachflag_shadow.png',
        new google.maps.Size(55, 74),
        new google.maps.Point(0, 0),
        new google.maps.Point(25, 74));


    var shape = {
        //coord: [1, 1, 1, 20, 18, 20, 18, 1],
        coord: [0, 0, 55, 55],
        type: 'circle'
    };

    var infowindow = new google.maps.InfoWindow();

    if (locationsi.length == 1) {
        if (locationsi[0][1] == "0" || locationsi[0][2] == "") {
            //console.log("LongLat não informado.");
        } else {
            var myLatLng = new google.maps.LatLng(locationsi[0][1], locationsi[0][2]);
            var content = '<div class="map-content">';
            content += '<h3 style="False">' + locationsi[0][0] + '</h3>';
            if (locationsi[0][13] == "" || locationsi[13] == null)
                content += '<img src="/Images/no-image.png" class="imgMapa" />';
            else
                content += '<img src="' + localizacao[0][13] + '" class="imgMapa" />';
            content += '<span class="main-details-category">';
            content += '<span style="display: none">63</span>';
            content += '<span class="main-details-category-stars main-details-category-stars-4"> </span>';
            content += locationsi[0][6] + ' Estrelas';
            content += '</span>';
            content += '<p>' + locationsi[5] + '</p>';
            content += '<span class="hotel-localization"></span>';
            content += '<div class="hotel-details-price">';
            content += '<p><b> diária a partir de</b></p>';
            content += '<span class="the-currency">R$</span>';
            content += '<span class="the-price"><b>' + locationsi[0][4] + '</b></span>';
            content += '</div>';
            content += '<div>';
            content += '<a class="see-details" onclick="VerDetalheHotel(' + locationsi[0][7] + ',' + locationsi[0][8] + ',' + locationsi[0][9] + ',' + locationsi[0][10] + ',' + locationsi[0][11] + ',' + locationsi[0][12] + ')" href="#">Ver detalhes</a>';
            content += '</div>';
            content += '</div>';

            //var marker = new google.maps.Marker({
            //    position: myLatLng,
            //    map: map,
            //    shadow: shadow,
            //    icon: image,
            //    shape: shape,
            //    title: "R$ " + Math.round(locationsi[0][4]),
            //    zIndex: locationsi[0][3]
            //});

            var marker = new markerwithlabel({
                position: mylatlng,
                map: map,
                shadow: shadow,
                icon: image,
                shape: shape,
                title: locationsi[0][0],
                zindex: locationsi[0][3],
                draggable: false,
                raiseondrag: true,
                labelcontent: "R$ " + math.round(locationsi[0][4]),
                labelanchor: new google.maps.point(19, 57),
                labelclass: "mapa-labels", // the css class for the label
                labelinbackground: false
            });

            google.maps.event.addListener(marker, 'click', (function (marker, content) {
                return function () {
                    infowindow.setContent(content);
                    infowindow.open(map, marker);
                    infowindow.minHeight(300);
                }
            })(marker, content));
            markers.push(marker);
        }
    }
    else {
        locations = locationsi;
        for (var i = 0; i < locations.length; i++) {
            //locations[0].Titulo
            //locations[1].Latidude
            //locations[2].Longitude
            //locations[3].Order
            //locations[4].ValorDiaria
            //locations[5].Endereco
            //locations[6].NumeroEstrelas
            //locations[7].CodigoHotel
            //locations[8].Checkin
            //locations[9].Checkout
            //locations[10].CodigoCidade
            //locations[11].QuantCriancas
            //locations[12].QuantAdultos
            //locations[13].Foto

            var localizacao = locations[i];

            if ((localizacao[1] == "0" || localizacao[2] == "0") || (localizacao[1] == "" || localizacao[2] == "") || (localizacao[1] == null || localizacao[2] == null)) {
                //console.log("LongLat não encontrado.");
            } else {
                var myLatLng = new google.maps.LatLng(localizacao[1], localizacao[2]);
                var content = '<div class="map-content">';
                content += '<h3 style="False">' + localizacao[0] + '</h3>';
                content += '<br />';
                if (localizacao[13] == "" || localizacao[13] == null)
                    content += '<img src="/Images/no-image.png" class="imgMapa" />';
                else
                    content += '<img src="' + localizacao[13] + '" class="imgMapa" />';               
                content += '<br />';
                content += '<span class="main-details-category">';
                content += '<span style="display: none">63</span>';
                content += '<span class="main-details-category-stars main-details-category-stars-4"> </span>';
                content += localizacao[6] + ' Estrelas';
                content += '</span>';
                content += '<p>' + localizacao[5] + '</p>';
                content += '<span class="hotel-localization"></span>';
                content += '<div class="hotel-details-price">';
                content += '<p><b> diária a partir de</b></p>';
                content += '<span class="the-currency">R$</span>';
                content += '<span class="the-price"><b>' + localizacao[4] + '</b></span>';
                content += '</div>';
                content += '<div>';
                content += '<a class="see-details" onclick="VerDetalheHotel(' + +localizacao[7] + ',' + "'" + localizacao[8] + "'" + ',' + "'" + localizacao[9] + "'" + ',' + localizacao[10] + ',' + localizacao[11] + ',' + localizacao[12] + ')" href="#">Ver detalhes</a>';
                content += '</div>';
                content += '</div>';

                //var marker = new google.maps.Marker({
                //    position: myLatLng,
                //    map: map,
                //    shadow: shadow,
                //    icon: image,
                //    shape: shape,
                //    title: "R$ " + Math.round(localizacao[4]),
                //    zIndex: localizacao[3]
                //});
                //console.log(localizacao[0] + " : " + localizacao[1] + " , " + localizacao[2]);
                var marker = new MarkerWithLabel({
                    position: myLatLng,
                    map: map,
                    shadow: shadow,
                    icon: image,
                    shape: shape,
                    title: localizacao[0],
                    zIndex: localizacao[3],
                    draggable: false,
                    raiseOnDrag: true,
                    labelContent: "R$ " + Math.round(localizacao[4]), // + ":" + i,
                    labelAnchor: new google.maps.Point(19, 57),
                    labelClass: "mapa-labels",  //the CSS class for the label
                    labelInBackground: false
                });
                
                google.maps.event.addListener(marker, 'click', (function (marker, content) {
                    return function () {
                        infowindow.setContent(content);
                        infowindow.open(map, marker);
                        infowindow.minHeight(300);
                    }
                })(marker, content));
                markers.push(marker);
            }
        }
    }

    var mcOptions = { gridSize: 20, maxZoom: 13 };
    var mc = new MarkerClusterer(map, markers, mcOptions);
}

//function loadScript() {
//    var script = document.createElement("script");
//    script.type = "text/javascript";
//    script.src = "http://maps.googleapis.com/maps/api/js?key=AIzaSyDmjUPBIHCXMF49oBKYY7uoaHzUswt55SE&sensor=false&callback=initialize";
//    document.body.appendChild(script);
//}

//window.onload = loadScript;

