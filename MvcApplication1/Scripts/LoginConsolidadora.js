﻿//var urlPageLoad = 'http://www.e-flytour.com.br/entryPoint.php?class=FlytourAjax&method=newLogin';
var urlPageLoad = 'https://ta.desktopdeviagens.com.br/webcommon/Handlers/GlobalAuthentication.ashx';
var url_corporate = 'http://www.wtsportal.com.br/wtscorporate/login/integracao.aspx';


if (isIE()) {
    urlPageLoad = '/entryPoint.php?class=FlytourAjax&method=newLogin';
}

function AllURLSystem() {
    var _urls = {
        '0': 'https://ta.desktopdeviagens.com.br/',
        '1': 'http://corp.desktopdeviagens.com.br/'
        //'1': 'http://flytour.corporatego.com.br/'
    };
    return _urls;
}


function abrirLogin() {
    if ($('.logincompleto').is(':visible'))
        $('.logincompleto').css('display', 'none');
    else
        $('.logincompleto').css('display', 'block');
}

function isIE() {
    var returnValue = undefined;
    if (navigator.appName == 'Microsoft Internet Explorer') {
        var ua = navigator.userAgent;
        var ie = ua.indexOf('MSIE')
        returnValue = parseInt(ua.substring(ie + 5, ua.indexOf('.', ie)));
    }
    return returnValue;
}

function recuperarSenhEFlytour() {
    var usuario = $("#Usuario").val();
    window.location.href = "http://tools.flytour.com.br/Login.aspx?topo=false&usuario=" + usuario + "&btnLembrarSenha=true";
}

function recuperarSenhaDesktop() {
    var authForm = window.document.getElementById("frmLogin");
    var hForgotPassword = window.document.getElementById("hForgotPassword");
    authForm.action = "https://www.desktopdeviagens.com.br" + hForgotPassword.value;
    var hUserName = window.document.getElementById("hUserName");
    hUserName.value = $('#Usuario').val(); // = userName;
    authForm.submit();
}

function recuperarSenhaUnificado() {
    if ($("#ckbEFlytour").is(":checked")) {
        recuperarSenhEFlytour();
    } else if ($("#ckbDesktop").is(":checked")) {
        recuperarSenhaDesktop();
    }
}

function doLoginNew(email, password) {
    // Codifica a senha corretamente.
    if (!isIE()) {
        password = encodeURIComponent(password);
    }

    // Parâmetros para requisição.
    var params = 'login=' + email + '&password=' + password;

    // Efetua a requisição em ajax.
    $.ajax({
        url: urlPageLoad,
        type: 'GET',
        cache: false,
        data: params,
        //dataType: 'json',
        success: function (response, textStatus, XMLHttpRequest) {
            if (isIE()) {
                if ($(response).find('success').text()) {
                    response = ($(response).find('success').text());
                }
            }
            // Recupera a resposta..
            //if ($.parseJSON(response)) {
            //    response = $.parseJSON(response);
            //}
            // Caso a resposta seja válida.
            if (response && response.length) {
                if (response[3]) {
                    var urlList = AllURLSystem();
                    var _lnkSystem = urlList[(response[2].length) ? '1' : '0'];
                    _lnkSystem = _lnkSystem + "Authenticate/ChangePassword?Login=" + login.value;
                    var wasOpen = true;
                    if (!window.open(_lnkSystem)) {
                        wasOpen = false;
                    }
                    var message = (wasOpen) ? ("Sua senha precisa ser alterada uma nova janela serÃ¡ aberta") : ("Sua senha precisa ser alterada, por favor desabilite seu bloqueador de popup e tente novamente");
                    alert(message);
                } else if (response[2] && response[2].length) {
                    var companies = response[2];
                    // Caso exista apenas 1 empresa.
                    if (companies.length == 1) {
                        systemRedirect(response);
                        return;
                    }
                    // Remove os itens do "select".
                    $('#inputCompany').find('option').remove().end();
                    // Caso tenha mais do que uma empresa é efetuado
                    //o loop sobre a lista de empresas.
                    for (var i = 0; i < companies.length; i++) {
                        var opt = $('<option value=' + i + '>' + companies[i]['Name'] + '</option>');
                        // Adiciona a opção.
                        $('#inputCompany').append(opt);
                    }
                    $('#inputCompany').selectbox();
                    // Mostra a caixa de seleÃ§Ã£o.
                    $('#box-seleciona-empresa').fadeIn();
                    // Adiciona o ouvinte para o clique.
                    $('.empresa_bt').click(function () {
                        // Efetua o redirecionamento
                        systemRedirect(response);
                    });
                } else {
                    systemRedirect(response);
                    return;
                }
            } else {
                alert('E-mail ou Senha inválidos');
            }
            // Reabilita as informações.
            $('input[name=usuario]', target).attr('disabled', false);
            $('input[name=senha]', target).attr('disabled', false);
            $('[type=submit]', target).attr('disabled', false).css('alpha', 1.0);
            return;
        },
        error: function (success) {
            alert(success.responseText);
            alert('Ocorreu um erro durante o login, por favor tente novamente mais tarde');
        }
    });
}

function systemRedirect(response) {
    //GET Parameters
    if (response && response[0].Key) {
        (function () {
            var urlList = AllURLSystem();
            var ga = document.createElement('form');
            ga.name = "travelling";
            ga.method = "post";
            ga.action = urlList[response[1] + ""];
            //ga.target = "_blank";
            var securityInput = document.createElement('input');
            securityInput.type = "hidden";
            securityInput.name = "Key";
            securityInput.value = response[0].Key;
            var gateway = document.createElement('input');
            gateway.type = "hidden";
            gateway.name = "Gateway";
            gateway.value = location.href;
            if (response[2] && response[2].length && response[2][0]["Name"]) {
                var company = document.createElement('input');
                company.type = "hidden";
                company.name = "company";
                if (response[2].length > 1) {
                    var companySelected = $('option:selected', '#inputCompany').val();
                    var optValue = JSON2.stringifyWcf(response[2][companySelected]);
                    company.value = optValue;
                    //company.value = JSON2.stringifyWcf(response[2][0]);
                } else {
                    company.value = JSON2.stringifyWcf(response[2][0]);
                }
                ga.appendChild(company);
            }
            ga.appendChild(securityInput);
            ga.appendChild(gateway);
            // Adiciona as informações ao
            var s = document.getElementsByTagName('body')[0];
            s.appendChild(ga);
            // Efetua a submissão do formulário criado dinamicamente.
            return document.travelling.submit();
        })();
    } else {
        alert('Usuário/Senha inválidos!');
    }
}

function submitDinacimo() {

    if (validarUsuarioSenha()) {
        if ($("#ckbEFlytour").is(":checked")) {

            LogarEFlytour();

        } else if ($("#ckbDesktop").is(":checked")) {

            LogarDestktop();

        } else if ($("#ckbBTravel").is(":checked")) {

            LogarBTravel();

        }

        $("#Usuario").val("");
        $("#Senha").val("");

    }
    else {
        alert("Informar usuário e senha.");
    }

}

function validarUsuarioSenha() {
    if ($("#Usuario").val() == "" || $("#Senha").val() == "") {
        return false;
    } else {
        return true;
    }
}

function LogarEFlytour() {

    $("#frmLogin").attr("action", "http://tools.flytour.com.br/Login.aspx");
    $("#frmLogin").submit();
}

function LogarDestktop() {

    var email = $("#Usuario").val();
    var password = $("#Senha").val();

    doLoginNew(email, password);
}

function LogarBTravel() {

    enviarFormularioWts();
}


//Função para criptografar as informações
function criptografar(plaintext) {
    var key = stringToByteArray('w3btr4v3lzzzxxxc'); //hexToByteArray(genkey());
    var mode = 'ECB'; // ECB or CBC
    var ciphertext = byteArrayToString(rijndaelEncrypt(plaintext, key, mode));
    var encodedtext = encode(ciphertext)
    return encodedtext;
}

//Função para enviar os dados
function enviarFormularioWts() {
    var formularioWts = document.getElementById("frmLogin");
    var key1 = document.getElementById("key1");
    var key2 = document.getElementById("key2");
    var login = document.getElementById("Usuario");
    var senha = document.getElementById("Senha");
    formularioWts.action = url_corporate;
    key1.value = criptografar(login.value);
    key2.value = criptografar(senha.value);
    login.value = '';
    senha.value = '';
    formularioWts.submit();
}

var BS = 128; var BB = 128; var RA = [, , , , [, , , , 10, , 12, , 14], , [, , , , 12, , 12, , 14], , [, , , , 14, , 14, , 14]]; var SO = [, , , , [, 1, 2, 3], , [, 1, 2, 3], , [, 1, 3, 4]]; var RC = [0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef, 0xc5, 0x91]; var SB = [99, 124, 119, 123, 242, 107, 111, 197, 48, 1, 103, 43, 254, 215, 171, 118, 202, 130, 201, 125, 250, 89, 71, 240, 173, 212, 162, 175, 156, 164, 114, 192, 183, 253, 147, 38, 54, 63, 247, 204, 52, 165, 229, 241, 113, 216, 49, 21, 4, 199, 35, 195, 24, 150, 5, 154, 7, 18, 128, 226, 235, 39, 178, 117, 9, 131, 44, 26, 27, 110, 90, 160, 82, 59, 214, 179, 41, 227, 47, 132, 83, 209, 0, 237, 32, 252, 177, 91, 106, 203, 190, 57, 74, 76, 88, 207, 208, 239, 170, 251, 67, 77, 51, 133, 69, 249, 2, 127, 80, 60, 159, 168, 81, 163, 64, 143, 146, 157, 56, 245, 188, 182, 218, 33, 16, 255, 243, 210, 205, 12, 19, 236, 95, 151, 68, 23, 196, 167, 126, 61, 100, 93, 25, 115, 96, 129, 79, 220, 34, 42, 144, 136, 70, 238, 184, 20, 222, 94, 11, 219, 224, 50, 58, 10, 73, 6, 36, 92, 194, 211, 172, 98, 145, 149, 228, 121, 231, 200, 55, 109, 141, 213, 78, 169, 108, 86, 244, 234, 101, 122, 174, 8, 186, 120, 37, 46, 28, 166, 180, 198, 232, 221, 116, 31, 75, 189, 139, 138, 112, 62, 181, 102, 72, 3, 246, 14, 97, 53, 87, 185, 134, 193, 29, 158, 225, 248, 152, 17, 105, 217, 142, 148, 155, 30, 135, 233, 206, 85, 40, 223, 140, 161, 137, 13, 191, 230, 66, 104, 65, 153, 45, 15, 176, 84, 187, 22]; var SBI = [82, 9, 106, 213, 48, 54, 165, 56, 191, 64, 163, 158, 129, 243, 215, 251, 124, 227, 57, 130, 155, 47, 255, 135, 52, 142, 67, 68, 196, 222, 233, 203, 84, 123, 148, 50, 166, 194, 35, 61, 238, 76, 149, 11, 66, 250, 195, 78, 8, 46, 161, 102, 40, 217, 36, 178, 118, 91, 162, 73, 109, 139, 209, 37, 114, 248, 246, 100, 134, 104, 152, 22, 212, 164, 92, 204, 93, 101, 182, 146, 108, 112, 72, 80, 253, 237, 185, 218, 94, 21, 70, 87, 167, 141, 157, 132, 144, 216, 171, 0, 140, 188, 211, 10, 247, 228, 88, 5, 184, 179, 69, 6, 208, 44, 30, 143, 202, 63, 15, 2, 193, 175, 189, 3, 1, 19, 138, 107, 58, 145, 17, 65, 79, 103, 220, 234, 151, 242, 207, 206, 240, 180, 230, 115, 150, 172, 116, 34, 231, 173, 53, 133, 226, 249, 55, 232, 28, 117, 223, 110, 71, 241, 26, 113, 29, 41, 197, 137, 111, 183, 98, 14, 170, 24, 190, 27, 252, 86, 62, 75, 198, 210, 121, 32, 154, 219, 192, 254, 120, 205, 90, 244, 31, 221, 168, 51, 136, 7, 199, 49, 177, 18, 16, 89, 39, 128, 236, 95, 96, 81, 127, 169, 25, 181, 74, 13, 45, 229, 122, 159, 147, 201, 156, 239, 160, 224, 59, 77, 174, 42, 245, 176, 200, 235, 187, 60, 131, 83, 153, 97, 23, 43, 4, 126, 186, 119, 214, 38, 225, 105, 20, 99, 85, 33, 12, 125]; function cSL(TA, PO) { var T = TA.slice(0, PO); TA = TA.slice(PO).concat(T); return TA; } var Nk = BS / 32; var Nb = BB / 32; var Nr = RA[Nk][Nb]; function XT(P) { P <<= 1; return ((P & 0x100) ? (P ^ 0x11B) : (P)); } function GF(x, y) { var B, R = 0; for (B = 1; B < 256; B *= 2, y = XT(y)) { if (x & B) R ^= y; } return R; } function bS(SE, DR) { var S; if (DR == "e") S = SB; else S = SBI; for (var i = 0; i < 4; i++) for (var j = 0; j < Nb; j++) SE[i][j] = S[SE[i][j]]; } function sR(SE, DR) { for (var i = 1; i < 4; i++) if (DR == "e") SE[i] = cSL(SE[i], SO[Nb][i]); else SE[i] = cSL(SE[i], Nb - SO[Nb][i]); } function mC(SE, DR) { var b = []; for (var j = 0; j < Nb; j++) { for (var i = 0; i < 4; i++) { if (DR == "e") b[i] = GF(SE[i][j], 2) ^ GF(SE[(i + 1) % 4][j], 3) ^ SE[(i + 2) % 4][j] ^ SE[(i + 3) % 4][j]; else b[i] = GF(SE[i][j], 0xE) ^ GF(SE[(i + 1) % 4][j], 0xB) ^ GF(SE[(i + 2) % 4][j], 0xD) ^ GF(SE[(i + 3) % 4][j], 9); } for (var i = 0; i < 4; i++) SE[i][j] = b[i]; } } function aRK(SE, RK) { for (var j = 0; j < Nb; j++) { SE[0][j] ^= (RK[j] & 0xFF); SE[1][j] ^= ((RK[j] >> 8) & 0xFF); SE[2][j] ^= ((RK[j] >> 16) & 0xFF); SE[3][j] ^= ((RK[j] >> 24) & 0xFF); } } function YE(Y) { var EY = []; var T; Nk = BS / 32; Nb = BB / 32; Nr = RA[Nk][Nb]; for (var j = 0; j < Nk; j++) EY[j] = (Y[4 * j]) | (Y[4 * j + 1] << 8) | (Y[4 * j + 2] << 16) | (Y[4 * j + 3] << 24); for (j = Nk; j < Nb * (Nr + 1) ; j++) { T = EY[j - 1]; if (j % Nk == 0) T = ((SB[(T >> 8) & 0xFF]) | (SB[(T >> 16) & 0xFF] << 8) | (SB[(T >> 24) & 0xFF] << 16) | (SB[T & 0xFF] << 24)) ^ RC[Math.floor(j / Nk) - 1]; else if (Nk > 6 && j % Nk == 4) T = (SB[(T >> 24) & 0xFF] << 24) | (SB[(T >> 16) & 0xFF] << 16) | (SB[(T >> 8) & 0xFF] << 8) | (SB[T & 0xFF]); EY[j] = EY[j - Nk] ^ T; } return EY; } function Rd(SE, RK) { bS(SE, "e"); sR(SE, "e"); mC(SE, "e"); aRK(SE, RK); } function iRd(SE, RK) { aRK(SE, RK); mC(SE, "d"); sR(SE, "d"); bS(SE, "d"); } function FRd(SE, RK) { bS(SE, "e"); sR(SE, "e"); aRK(SE, RK); } function iFRd(SE, RK) { aRK(SE, RK); sR(SE, "d"); bS(SE, "d"); } function encrypt(bk, EY) { var i; if (!bk || bk.length * 8 != BB) return; if (!EY) return; bk = pB(bk); aRK(bk, EY); for (i = 1; i < Nr; i++) Rd(bk, EY.slice(Nb * i, Nb * (i + 1))); FRd(bk, EY.slice(Nb * Nr)); return uPB(bk); } function decrypt(bk, EY) { var i; if (!bk || bk.length * 8 != BB) return; if (!EY) return; bk = pB(bk); iFRd(bk, EY.slice(Nb * Nr)); for (i = Nr - 1; i > 0; i--) iRd(bk, EY.slice(Nb * i, Nb * (i + 1))); aRK(bk, EY); return uPB(bk); } function byteArrayToString(bA) { var R = ""; for (var i = 0; i < bA.length; i++) R += String.fromCharCode(bA[i]); return R; } function byteArrayToHex(bA) { var R = ""; if (!bA) return; for (var i = 0; i < bA.length; i++) R += ((bA[i] < 16) ? "0" : "") + bA[i].toString(16); return R; } function hexToByteArray(hS) { var bA = []; if (hS.length % 2) return; if (hS.indexOf("0x") == 0 || hS.indexOf("0X") == 0) hS = hS.substring(2); for (var i = 0; i < hS.length; i += 2) bA[Math.floor(i / 2)] = parseInt(hS.slice(i, i + 2), 16); return bA; } function pB(OT) { var SE = []; if (!OT || OT.length % 4) return; SE[0] = []; SE[1] = []; SE[2] = []; SE[3] = []; for (var j = 0; j < OT.length; j += 4) { SE[0][j / 4] = OT[j]; SE[1][j / 4] = OT[j + 1]; SE[2][j / 4] = OT[j + 2]; SE[3][j / 4] = OT[j + 3]; } return SE; } function uPB(PK) { var R = []; for (var j = 0; j < PK[0].length; j++) { R[R.length] = PK[0][j]; R[R.length] = PK[1][j]; R[R.length] = PK[2][j]; R[R.length] = PK[3][j]; } return R; } function fPT(PT) { var bpb = BB / 8; var i; if (typeof PT == "string" || PT.indexOf) { PT = PT.split(""); for (i = 0; i < PT.length; i++) PT[i] = PT[i].charCodeAt(0) & 0xFF; } for (i = bpb - (PT.length % bpb) ; i > 0 && i < bpb; i--) PT[PT.length] = 0; return PT; } function gRB(hM) { var i; var bt = []; for (i = 0; i < hM; i++) bt[i] = Math.round(Math.random() * 255); return bt; } function rijndaelEncrypt(PT, Y, M) { var EY, i, abk; var bpb = BB / 8; var ct; if (!PT || !Y) return; if (Y.length * 8 != BS) return; if (M == "CBC") ct = gRB(bpb); else { M = "ECB"; ct = []; } PT = fPT(PT); EY = YE(Y); for (var bk = 0; bk < PT.length / bpb; bk++) { abk = PT.slice(bk * bpb, (bk + 1) * bpb); if (M == "CBC") for (var i = 0; i < bpb; i++) abk[i] ^= ct[bk * bpb + i]; ct = ct.concat(encrypt(abk, EY)); } return ct; } function rijndaelDecrypt(CT, Y, M) { var EY; var bpb = BB / 8; var pt = []; var abk; var bk; if (!CT || !Y || typeof CT == "string") return; if (Y.length * 8 != BS) return; if (!M) M = "ECB"; EY = YE(Y); for (bk = (CT.length / bpb) - 1; bk > 0; bk--) { abk = decrypt(CT.slice(bk * bpb, (bk + 1) * bpb), EY); if (M == "CBC") for (var i = 0; i < bpb; i++) pt[(bk - 1) * bpb + i] = abk[i] ^ CT[(bk - 1) * bpb + i]; else pt = abk.concat(pt); } if (M == "ECB") pt = decrypt(CT.slice(0, bpb), EY).concat(pt); return pt; } function stringToByteArray(st) { var bA = []; for (var i = 0; i < st.length; i++) bA[i] = st.charCodeAt(i); return bA; } function genkey() { var j = ""; while (1) { var i = Math.random().toString(); j += i.substring(i.lastIndexOf(".") + 1); if (j.length > 31) return j.substring(0, 32); } }
// This code was written by Tyler Akins and has been placed in the
// public domain. It would be nice if you left this header intact.
// Base64 code from Tyler Akins -- http://rumkin.com
var keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
function encode(input) {
    var output = new StringMaker();
    var chr1, chr2, chr3;
    var enc1, enc2, enc3, enc4;
    var i = 0;

    while (i < input.length) {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);

        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;

        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }

        output.append(keyStr.charAt(enc1) + keyStr.charAt(enc2) + keyStr.charAt(enc3) + keyStr.charAt(enc4));
    }

    return output.toString();
}

var ua = navigator.userAgent.toLowerCase();
if (ua.indexOf(" chrome/") >= 0 || ua.indexOf(" firefox/") >= 0 || ua.indexOf(' gecko/') >= 0) {
    var StringMaker = function () {
        this.str = "";
        this.length = 0;
        this.append = function (s) {
            this.str += s;
            this.length += s.length;
        };
        this.prepend = function (s) {
            this.str = s + this.str;
            this.length += s.length;
        };
        this.toString = function () {
            return this.str;
        };
    };
} else {
    var StringMaker = function () {
        this.parts = [];
        this.length = 0;
        this.append = function (s) {
            this.parts.push(s);
            this.length += s.length;
        };
        this.prepend = function (s) {
            this.parts.unshift(s);
            this.length += s.length;
        };
        this.toString = function () {
            return this.parts.join('');
        };
    };
}

function queryString(ji) {
    var hu = window.location.search.substring(1);
    var gy = hu.split("&");
    for (var i = 0; i < gy.length; i++) {
        var ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1];
        }
    }
}



$(document).ready(function () {

    $("#ckbEFlytour").click(function () {
        $("#lnkLembrarSenha").attr('style', 'display:block');
    });

    $("#ckbDesktop").click(function () {
        $("#lnkLembrarSenha").attr('style', 'display:block');
    });

    $("#ckbBTravel").click(function () {
        $("#lnkLembrarSenha").attr('style', 'visibility: hidden');
    });
});