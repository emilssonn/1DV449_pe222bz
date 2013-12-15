<?php

namespace view;

class AppView {

	public function getHTML() {
		return '
			<!DOCTYPE html>
			<html>
			  <head>
			  	<meta charset="UTF-8">
			    <title></title>
			    <meta name="viewport" content="width=device-width, initial-scale=1.0">
			    <!-- Bootstrap -->
			    <link href="css/bootstrap.min.css" rel="stylesheet">

			    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
			    <!--[if lt IE 9]>
			      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
			      <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
			    <![endif]-->

			    <script type="text/javascript"
	      			src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBt7yjVsyTrfUzSWi-8P3FzJKArOfQQQGg&sensor=false">
	    		</script>
			  </head>
			  <body>
			  	<div class="container">
			    	<h1>Test</h1>
			    	<button id="test">Test</button>
			    	<div id="map-canvas" style="min-width: 100%; min-height:500px"></div>
			    </div>
			    <!-- jQuery (necessary for Bootstraps JavaScript plugins) -->
			    <script src="https://code.jquery.com/jquery.js"></script>
			    <!-- Include all compiled plugins (below), or include individual files as needed -->
			    <script src="js/bootstrap.min.js"></script>
			    <script src="js/map.js"></script>
			  </body>
			</html>	
		';
	}
}