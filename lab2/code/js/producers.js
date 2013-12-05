$( document ).ready( 	
	function() {

		$("#logout").bind( "click", function() {
			  	window.location = "logout.php";
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
				
	var messContainer = $("#mess_container");	
	// Clear and update the hidden stuff
	messContainer.find( "#mess_inputs").val(pid);
	messContainer.find( "#mess_p_mess").empty();
	
	// get all the stuff for the producers
	// make ajax call to functions.php with the data
	$.ajax({
		type: "GET",
	  	url: "functions.php",
	  	data: {function: "producers", pid: pid}
	}).done(function(data) { // called when the AJAX call is ready
		var j = JSON.parse(data);
		
		messContainer.find("#mess_p_headline").text("Meddelande till " +j.name +", " +j.city);
		
		
		if(j.url !== "") {
			
			messContainer.find("#mess_p_kontakt").text("Länk till deras hemsida " +j.url);
		}
		else {
			messContainer.find("#mess_p_kontakt").text("Producenten har ingen webbsida");
		}
		
		if(j.imageURL !== "") {
			messContainer.find("#p_img_link").attr("href", j.imageURL); 
			messContainer.find("#p_img").attr("src", j.imageURL); 
		}
		else {
			messContainer.find("#p_img_link").attr("href", "#"); 
			messContainer.find("#p_img").attr("src", "img/noimg.jpg"); 
		}

		if (j.messages !== null) {
			j.messages.reverse().forEach(function(entry) {
				var p = $("<p class='message_container'></p>");
				p.append(document.createTextNode(entry.message)).append(
					"<br />Skrivet av: ").append(
					document.createTextNode(entry.name));
				messContainer.find( "#mess_p_mess" ).append(p);
			});
		}
	});
	
	
	
	// show the div if its unvisible
	messContainer.show("slow");
	
}