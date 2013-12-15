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
				<nav class="navbar navbar-default" role="navigation">
				  <!-- Brand and toggle get grouped for better mobile display -->
				  <div class="navbar-header">
				    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
				      <span class="sr-only">Toggle navigation</span>
				      <span class="icon-bar"></span>
				      <span class="icon-bar"></span>
				      <span class="icon-bar"></span>
				    </button>
				    <a class="navbar-brand" href="#">Test</a>
				  </div>

				  <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
				    <form class="navbar-form navbar-left" role="search" id="checkBoxFilter">
					  <div class="checkbox inline">
					    <label>
					      <input type="checkbox" value="0" name="cat"> Vägtrafik
					    </label>
					  </div>
					  <div class="checkbox inline">
					    <label>
					      <input type="checkbox" value="1" name="cat"> Kollektivtrafik
					    </label>
					  </div>
					  <div class="checkbox inline">
					    <label>
					      <input type="checkbox" value="2" name="cat"> Planerad störning
					    </label>
					  </div>
					  <div class="checkbox inline">
					    <label>
					      <input type="checkbox" value="3" name="cat"> Övrigt
					    </label>
					  </div>
					  <a class="btn btn-default" id="filterButton">Filter</a>
					</form>

				  </div><!-- /.navbar-collapse -->
				</nav>

			  	<div class="container">
			  		<div class="row">
			  			<h1>Test</h1>
			  			<div class="col-lg-2" id="list-trafic" style="max-height:500px; overflow:scroll;">

			  			</div>
			  			<div class="col-lg-10">
			  				<div id="map-canvas" style="min-width: 100%; min-height:500px"></div>
			  			</div>
			  		</div>
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