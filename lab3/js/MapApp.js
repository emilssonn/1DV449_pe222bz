"use strict";

var MapApp = {
	
	mapCenter : new google.maps.LatLng(62.197631,15.358887),

	map: null,

	infowindow: new google.maps.InfoWindow(),

	markers: new Array(),

	cacheLocation: "SRTraficMessages",

	/**
	 * Seconds
	 * @type {Number}
	 */
	cacheTime: 60,

	/**
	 * Start up
	 */
	initialize: function () {
		var that = this,
			mapOptions = {
	    	zoom: 5,
	    	center: that.mapCenter
	  	};

	  	that.map = new google.maps.Map(
	  		document.getElementById('map-canvas'),
	    	mapOptions
	    );

		that.getTraficInfo();
		
		$(".filterCheckbox").each(function() {
			//Set checked as default, fix for FF reload not resetting
			$(this).find('input').prop('checked', true);
			//Mouseup event instead if click to prevent label from triggering checkbox click
			$(this).on('mouseup', function() {
				var val = $(this).find('input').val();
				that.filterMarkers(val);
			})
		});	

		$("#updateFrequency").text(that.cacheTime);
	},

	/**
	 * Check for valid messages in localstorage, otherwise fetch from SR.se
	 */
	getTraficInfo: function() {
		var that = this;
		if (that.checkLocalStorage()) {
			var messageCache = JSON.parse(localStorage.getItem(that.cacheLocation));
			if (messageCache !== null) {
				//Get timestamp and check if cache is to old
				var timestamp = messageCache.shift();
				if (timestamp < new Date().getTime()) {
					that.fetchTraficInfo();
					return;
				} else {
					$("#lastUpdated").text(that.getTimeText(timestamp, true));
					that.manageMessages(messageCache);
					return;
				}
			}
		}
		that.fetchTraficInfo();
	},

	/**
	 * Get the 100 latest trafic messages from SR.se
	 * Get by JSONP
	 * Cache messages in localstorage
	 */
	fetchTraficInfo: function() {
		var that = this;
		$.ajax({
			type: "GET",
			cache: true,
		  	url: "http://api.sr.se/api/v2/traffic/messages",
		  	dataType: "jsonp",
		  	data: { 
		  		format: "json",
            	size: 100
            }
		}).done(function(data) {
			//Cache, add timestamp, save to localstorage, remove timestamp
			if (that.checkLocalStorage()) {
				var timestamp = new Date().getTime() + (that.cacheTime * 1000);
				data.messages.unshift(timestamp);
				localStorage.setItem(that.cacheLocation, JSON.stringify(data.messages));
				data.messages.shift();
				$("#lastUpdated").text(that.getTimeText(timestamp, true));
			}
			that.manageMessages(data.messages);	
		});
	},

	/**
	 * Add messages to the list and map
	 * @param  {Array} messages
	 */
	manageMessages: function(messages) {
		var that = this;
		if (messages != "undefined" && messages !== null) {
			messages.forEach(function(entry) {
				that.addMarker(entry);
			});
		}
	},

	/**
	 * Add a marker to the map
	 * @param {Object} message
	 */
	addMarker: function(message) {
		var that = this,
			latlng = new google.maps.LatLng(message.latitude,message.longitude),
			icon = that.getIconColor(message.priority),

			marker = new google.maps.Marker({
	    		position: latlng,
	    		title: message.title,
	    		icon: icon,
	    		cat: message.category,
	    		tId: message.id 
			});

		that.addInfoWindow(marker, message);
		marker.setMap(that.map);

		that.markers.push(marker);
	},

	/**
	 * @param {google.maps.Marker} marker
	 * @param {Object} message
	 */
	addInfoWindow: function(marker, message) {
		var that = this,
		//Info Window
			content = $('<p></p>').addClass('infoWindow').append(
				$('<strong></strong>').append(
					document.createTextNode(message.title))).append(
				"<br />").append(
				document.createTextNode(message.description)).append(
				"<br /> Kategori: " + that.getCategoryText(message.category)).append(
				"<br /> Tid: " + that.getTimeText(message.createddate));

		//Show content when the marker gets clicked
		google.maps.event.addListener(marker, 'click', function() {
			that.infowindow.setContent(content.get(0));
   			that.infowindow.open(that.map,marker);
  		});

		//List element
		var listMessage = $('<li></li>').attr("id", message.id),
			h5 = $('<h5></h5>'),
			title = $('<a href="#map-canvas"></a>');

		title.append(document.createTextNode(message.title));
		$(title).on('click', function() {
			that.infowindow.setContent(content.get(0));
   			that.infowindow.open(that.map,marker);
		});
		h5.append(title)
		listMessage.append(h5);
		listMessage.append(
			document.createTextNode(message.description)).append(
			"<br /> Kategori: " + that.getCategoryText(message.category)).append(
			"<br /> Tid: " + that.getTimeText(message.createddate)).append('<hr />');

		$("#list-trafic").prepend(listMessage);
	},

	/**
	 * Hide/Show a specific trafic message category
	 * @param  {Number} val
	 */
	filterMarkers: function(val) {
		var list = $("#list-trafic");
		this.markers.forEach(function(entry) {
			if (entry.cat == val) {
				entry.setVisible(!entry.getVisible());
				list.find('#' + entry.tId).toggle();
			}
		});
	},

	/**
	 * Return a url for a google maps marker icon
	 * If no matching url is found, returns null
	 * @param  {Number} prio
	 * @return {string} Url  
	 */
	getIconColor: function(prio) {
		switch(prio) {
			case 1:
				return "http://maps.google.com/mapfiles/ms/icons/red-dot.png";
			case 2:
				return "http://maps.google.com/mapfiles/ms/icons/yellow-dot.png";
			case 3:
				return "http://maps.google.com/mapfiles/ms/icons/purple-dot.png";
			case 4:
				return "http://maps.google.com/mapfiles/ms/icons/green-dot.png";
			case 5:
				return "http://maps.google.com/mapfiles/ms/icons/blue-dot.png";
			default:
				return null;
		}
	},

	/**
	 * Return the corresponding string for a category number
	 * If no matching category is found, returns null
	 * @param  {Number} prio
	 * @return {string} Category  
	 */
	getCategoryText: function(cat) {
		switch (cat) {
			case 0:
				return 'Vägtrafik';
			case 1:
				return 'Kollektivtrafik';
			case 2:
				return 'Planerad störning';
			case 3:
				return 'Övrigt';
			default:
				return null;
		}
	},

	/**
	 * @param  {json timestamp} timestamp
	 * @return {string} Ex: 2013-12-18 11:33
	 */
	getTimeText: function(timestamp, flag) {
		var date;
		if (flag)
			var date = new Date(timestamp);
		else
			var date = new Date(parseInt(timestamp.substr(6)));
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
		return date.getFullYear() + "-" + month + "-" + day + " " + date.getHours() + ":" + minutes;
	},

	/**
	 * Check localStorage support
	 * @source http://diveintohtml5.info/detect.html#storage
	 * @return {bool}
	 */
	checkLocalStorage: function() {
  		try {
    		return 'localStorage' in window && window['localStorage'] !== null;
  		} catch(e){
    		return false;
  		}
	}
}

$(function() {
	MapApp.initialize();
});



