$( document ).ready( 	
	function() {

		$("#logout").bind( "click", function() {
			  	window.location = "index.php";
	 	});
	
		
		$("#add_btn").bind( "click", function() {
			  	
			var name_val = $('#name_txt').val();
			var message_val = $('#message_ta').val();
			var pid =  $('#mess_inputs').val();
			// make ajax call to logout
			$.ajax({
				type: "GET",
			  	url: "functions.php",
			  	data: {function: "add", name: name_val, message: message_val, pid: pid}
			}).done(function(data) {
			  alert(data);
			});
		  
	  });
	}
);

//This script is running to get the data for the producers

// Called when we click on a producer link - gets the id for the producer 
function changeProducer(pid) {
					
	// Clear and update the hidden stuff
	$( "#mess_inputs").val(pid);
	$( "#mess_p_mess").empty();
	
	// get all the stuff for the producers
	// make ajax call to functions.php with the data
	$.ajax({
		type: "GET",
	  	url: "functions.php",
	  	data: {function: "producers", pid: pid}
	}).done(function(data) { // called when the AJAX call is ready
		var j = JSON.parse(data);
		
		$("#mess_p_headline").text("Meddelande till " +j.name +", " +j.city);
		
		
		if(j.url !== "") {
			
			$("#mess_p_kontakt").text("Länk till deras hemsida " +j.url);
		}
		else {
			$("#mess_p_kontakt").text("Producenten har ingen webbsida");
		}
		
		if(j.imageURL !== "") {
			$("#p_img_link").attr("href", j.imageURL); 
			$("#p_img").attr("src", j.imageURL); 
		}
		else {
			$("#p_img_link").attr("href", "#"); 
			$("#p_img").attr("src", "img/noimg.jpg"); 
		}
	});
	
	// Get all the messages for the producers through functions.php
	$.ajax({
		type: "GET",
	  	url: "functions.php",
	  	data: {function: "getMessages", pid: pid}	
	}).done(function(data) {
		var j = JSON.parse(data);
		
		if (j.length > 0) {
			j.reverse().forEach(function(entry) {
				$( "#mess_p_mess" ).append( "<p class='message_container'>" +entry.message +"<br />Skrivet av: " +entry.name +"</p>");
			});
		}
	});
	
	// show the div if its unvisible
	$("#mess_container").show("slow");
	
}