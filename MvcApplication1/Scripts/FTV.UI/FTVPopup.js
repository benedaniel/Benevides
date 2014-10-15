(function ( $ ) {
	
	var popupStatus = 0;
	var content;
	var gerarLog = false;
	
	function Logar(msg){
		if(gerarLog){
			console.log(msg);
		}
	}
	
	
	function loadPopup(){
		if(popupStatus==0){
			$("#backgroundPopup").css({
				"opacity": "0.7"
			});
			$("#backgroundPopup").fadeIn("slow");
			$("#ModalPopup").fadeIn("slow");
			$(content).show();
			popupStatus = 1;
		}
	}
	
	function disablePopup(){
		Logar("popupStatus:" + popupStatus);
		if(popupStatus==1){
			$("#backgroundPopup").fadeOut("slow");
			$("#ModalPopup").fadeOut("slow");
			popupStatus = 0;
		}
	}

	function centerPopup(){
		var windowWidth = document.documentElement.clientWidth;
		var windowHeight = document.documentElement.clientHeight;
		var popupHeight = $("#ModalPopup").height();
		var popupWidth = $("#ModalPopup").width();
		$("#ModalPopup").css({
			"position": "absolute",
			"top": windowHeight/2-popupHeight/2,
			"left": windowWidth/2-popupWidth/2
		});
		
		//only need force for IE6		
		$("#backgroundPopup").css({
			"height": windowHeight
		});
		
	}
	
	function CleanUp(){
		Logar("chamou o cleanUP");
		$(document.body).append(content);
		$(content).hide();		
		$("#ModalPopup").remove();
		$("#backgroundPopup").remove();
	}
	
	function AddEventListner(){
		$("#popupContactClose").click(function(e){
			$(this).CloseModalPopup();
		});
		
		$(document).keypress(function(e){
			if(e.keyCode==27 && popupStatus==1){
				Logar("pressionou esc");
				$(this).CloseModalPopup();
			}
		});
		
		$("#backgroundPopup").click(function(e){
			Logar("clicou fora");
			$(this).CloseModalPopup();
		});
	}
	
	$.fn.CloseModalPopup = function() {
		disablePopup();
		CleanUp();
		return this;
	}


	$.fn.ShowModalPopup = function( options ) {
	
		var settings = $.extend({
			minHeight: 635,
			minWidth: 735,
			closeText: "[X]",
			closeImage: "/Content/Images/btnfechar.png"
		}, options );
		Logar("chamou o modal");
		this.filter( "div" ).each(function() { Logar("entrou no div");
			var div = $( this );
			content = div;
			$(content).hide();
			$(document.body).append( " <div id='backgroundPopup' />" );
			$(document.body).append( " <div id='ModalPopup' />");
			$("#ModalPopup").append(" <div><a href='#' id='popupContactClose' ><img src='" + settings.closeImage + "' alt='" + settings.closeText + "'></a><br /></div>");
			$("#ModalPopup").append(div);
			
			$("#ModalPopup").height(settings.minHeight);
			$("#ModalPopup").width(settings.minWidth);
			
			centerPopup();
			loadPopup();
			AddEventListner();
		});
		
		return this;
	};
}( jQuery ));


// Exempo de uso
//$(document).ready(function(){	
//	$("#popupContact").hide();
	
//	$("#button").click(function(){
//		$("#popupContact").ShowModalPopup({
//            minHeight: 635,
//            minWidth: 735
//        });
//	});
//});