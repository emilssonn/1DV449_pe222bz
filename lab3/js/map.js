"use strict";

var MapApp = {
	
	mapCenter : new google.maps.LatLng(62.197631,15.358887),

	map: null,

	infowindow: new google.maps.InfoWindow(),

	markers: new Array(),

	initialize: function () {
		var that = this;

		var mapOptions = {
	    	zoom: 5,
	    	center: that.mapCenter
	  	};

	  	that.map = new google.maps.Map(
	  		document.getElementById('map-canvas'),
	    	mapOptions
	    );

		that.getTraficInfo();
		
		$('#filterButton').on('click', function() {
			that.filterMarkers();
		})
	},

	getTraficInfo: function() {
		var that = this;
		$.ajax({
			type: "GET",
		  	url: "?traficInfo"
		}).done(function(data) {
			data = JSON.parse(data);
			
			if (data.messages !== null) {
				data.messages.forEach(function(entry) {
					that.addMarker(entry);
				});
			}

		});
	},

	addMarker: function(message) {
		var that = this;
		var latlng = new google.maps.LatLng(message.latitude,message.longitude);
		var icon = that.getIconColor(message.priority);

		var marker = new google.maps.Marker({
    		position: latlng,
    		title: message.title,
    		icon: icon,
    		cat: message.category,
    		tid: message.id 
		});

		this.addInfoWindow(marker, message);

		marker.setMap(that.map);

		that.markers.push(marker);
	},



	//marker.setVisible(false);list-trafic
	addInfoWindow: function(marker, message) {
		var that = this;
		var content = $('<p></p>').append(
			document.createTextNode(message.title)).append(
			"<br />").append(
			document.createTextNode(message.description)).append(
			"<br />" + that.getCategoryText(message.category)).append(
			"<br />" + that.getMessageTimeText(message.createddate));

		var mc = $('<p></p>').attr("id", message.id);

		var title = $('<span class="markerTrigger" style="color: blue;"></span>');
		title.append(document.createTextNode(message.title));
		$(title).on('click', function() {
			that.infowindow.setContent(content.get(0));
   			that.infowindow.open(that.map,marker);
		});

		mc.append(title);

		mc.append("<br />").append(
			document.createTextNode(message.description)).append(
			"<br />" + that.getCategoryText(message.category)).append(
			"<br />" + that.getMessageTimeText(message.createddate));

		$("#list-trafic").prepend(mc);

		google.maps.event.addListener(marker, 'click', function() {
			that.infowindow.setContent(content.get(0));
   			that.infowindow.open(that.map,marker);
  		});
	},

	filterMarkers: function() {
		var that = this;
		var checkedValues = $('input[name="cat"]:checked').map(function() {
    		return this.value;
		}).get();

		var result = $.grep(that.markers, function(e){ 
			var flag = true;
			checkedValues.forEach(function(entry) {
				if (e.cat == entry)
					flag = false;
			});
			return flag;
		});

		var result2 = $.grep(that.markers, function(e){ 
			var flag = false;
			checkedValues.forEach(function(entry) {
				if (e.cat == entry)
					flag = true;
			});
			return flag;
		});
		var list = $("#list-trafic");
		result.forEach(function(entry) {
			entry.setVisible(false);
			list.find('#' + entry.tid).hide();
		});
		result2.forEach(function(entry) {
			entry.setVisible(true);
			list.find('#' + entry.tid).show();
		});
	},

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
		}
	},

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
		}
	},

	getMessageTimeText: function(timestamp) {
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
	}
}


$(function() {
	MapApp.initialize();
});



