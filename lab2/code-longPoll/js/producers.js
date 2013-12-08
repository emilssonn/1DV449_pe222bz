$( document ).ready( 	
	function() {

		$("#logout").bind( "click", function() {
			  	window.location = "logout.php";
	 	});
	
		
		$("#add_btn").bind( "click", function() {	
			var name_val = $('#name_txt').val();
			var message_val = $('#message_ta').val();
			var pid =  $('#mess_inputs').val();
			var postToken = $("#postToken").val();
			$.ajax({
				type: "POST",
			  	url: "functions.php",
			  	data: {function: "add", name: name_val, message: message_val, pid: pid, postToken: postToken},
			  	timeout: 22000
			}).done(function(data) {
			  alert(data);
			});
		  
	  });
	}
);
//Current message request, global variable--bad
var request = null;

//This script is running to get the data for the producers

// Called when we click on a producer link - gets the id for the producer 
function changeProducer(pid) {	
	//Abort the current message request
	if (request !== null)
		request.abort();
			
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

		//With time
		if (j.messages !== null) {
			j.messages.forEach(function(entry) {
				var p = $("<p class='message_container'></p>");
				p.append(document.createTextNode(entry.message)).append(
					"<br />Skrivet av: ").append(
					document.createTextNode(entry.name)).append(
					"<br />" + getMessageTimeText(entry.time));
				messContainer.find( "#mess_p_mess" ).append(p);
			});
		}
	});
	
	
	
	// show the div if its unvisible
	messContainer.show("slow");
	
	//Start long polling
	getNewMessages(pid);
}



//Long poll new messages for the specific producer
function getNewMessages(pid) {
    request = $.ajax({
        type: "GET",
            url: "functions.php",
            data: { 
            	function: "newMessages", 
            	pid: pid,
            	time: Math.round(Date.now()/1000)
            }
    }).done(function(data) {
        if (data != "false") {
            var messContainer = $("#mess_container");
            var j = JSON.parse(data);

            //Prepend the new messages to the message container
            //With time
            j.forEach(function(entry) {
                var p = $("<p class='message_container'></p>");
                p.append(document.createTextNode(entry.message)).append(
                    "<br />Skrivet av: ").append(
                     document.createTextNode(entry.name)).append(
                     "<br />" + getMessageTimeText(entry.time));
                messContainer.find( "#mess_p_mess" ).prepend(p);
            });
        }
        request = null;
        //New long poll
        getNewMessages(pid);
    });
}

//Returns the time when the message was sent
function getMessageTimeText(timestamp) {
	var date = new Date(timestamp*1000);
	var minutes = date.getMinutes();
	var day = date.getDate();
	var month = date.getMonth() + 1;
	if (minutes < 10) {
		minutes = "0" + minutes;
	}
	if (day < 10) {
		day = "0" + day;
	}
	if (month < 10) {
		month = "0" + month;
	}
	return date.getFullYear() + "-" + month + "-" + day + " " + date.getHours() + ":" + minutes + ":" + date.getSeconds();
}