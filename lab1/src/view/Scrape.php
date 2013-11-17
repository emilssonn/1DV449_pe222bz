<?php

namespace view;

class Scrape {
	
	public function scrape() {
		return strtolower($_SERVER['REQUEST_METHOD']) == "post";
	}

	public function goToIndex() {
		header("Location: .");
	}

	public function getHTML($producersArray) {
		$html = "";

		$html .= $this->getTopHTML();

		$html .= '<div class="container">';

		$html .= $this->getProducersHTML($producersArray);

    	$html .= '</div> <!-- /container -->';

    	$html .= $this->getBottomHTML();

		return $html;
	}

	private function getProducersHTML($producersArray) {
		$html = "<ul class='list-unstyled'>";

		foreach ($producersArray as $producer) {
			$name = $producer->getName();
			$pId = $producer->getpId();
			$linkError = $producer->getLinkError();
			$url = $producer->getUrl();
			$city = $producer->getCity();
			$image = $producer->getImage();
			$timesScraped = $producer->getTimesScraped();
			$lastScraped = $producer->getLastScraped();

			$html .= "
				<li>
					<div class='panel panel-default'>
	  					<div class='panel-heading'>
	  						<h3 class='panel-title'># $pId: $name</h3>
	  					</div>
	 					<div class='panel-body'>
	 					<div class='col-md-6'>";

	 		//If producer link failed
	 		if ($linkError !== null) {
	 			$eUrl = $linkError->getUrl();
	 			$eNr = $linkError->getErrorNr();
	 			$html .= "<h4>Failed to connect to link</h4>
	 					<p>Link: $eUrl</p>
	 					<p><span class='label label-danger'>Error Code:</span> $eNr</p>
	 					<p>Times scraped: $timesScraped</p>
	 					<p>Last scraped: $lastScraped</p>";
	 		} else {
	 			$html .= "<p>City: $city</p>";

	 			//Url
	 			if ($url !== null && $url->getErrorNr() === null) {
	 				$tUrl = $url->getUrl();
	 				$html .= "<p>Url: $tUrl</p>";
	 			} else if ($url !== null && $url->getErrorNr() !== null) {
	 				$tUrl = $url->getUrl();
	 				$eNr = $url->getErrorNr();
	 				$html .= "<p>Url: Failed to connect: $tUrl</p>
	 						<p><span class='label label-danger'>Error Code:</span> $eNr</p>";
	 			} else {
	 				$html .= "<p><span class='label label-warning'>Url:</span> None</p>";
	 			}
	 			$html .= "<p>Times scraped: $timesScraped</p>
	 					<p>Last scraped: $lastScraped</p>
	 					</div>
	 					<div class='col-md-6'>";

	 			//Image
	 			if ($image !== null && $image->getName() !== null) {
	 				$imageName = $image->getName();
	 				$html .= "<img src='./src/data/images/$imageName'>";
	 			} else if ($image !== null && $image->getName() === null) {
	 				$externalSrc = $image->getExternalSrc();
	 				$html .= "<h5><span class='label label-danger'>Error</span></h5>
	 							<p>Failed to retrive image from: $externalSrc";
	 			} else {
	 				$html .= "<p><span class='label label-warning'>No Image</span></p>";
	 			}
	 		}

	 		$html .= "	</div>
	 					</div>
	 				</div>
	 			</li>";
		}

		$html .= "</ul>";

		return $html;
	}

	private function getBottomHTML() {
		return '<!-- Bootstrap core JavaScript
						================================================== -->
						<!-- Placed at the end of the document so the pages load faster -->
						<script src="https://code.jquery.com/jquery-1.10.2.min.js"></script>
						<script src="js/bootstrap.min.js"></script>
						<script src="js/spin.min.js"></script>
						<script src="js/ladda.min.js"></script>
						

						<script>
							Ladda.bind( "button[type=submit]" );
						</script>
  					</body>
				</html>';
	}

	private function getTopHTML() {
		return 	'<!DOCTYPE html>
				<html lang="en">
  					<head>
    					<meta charset="utf-8">
    					<meta http-equiv="X-UA-Compatible" content="IE=edge">
    					<meta name="viewport" content="width=device-width, initial-scale=1.0">

    					<title>1DV449 Lab 1</title>

    					<!-- Bootstrap core CSS -->
    					<link href="css/bootstrap.min.css" rel="stylesheet">
    					<link href="css/ladda-themeless.min.css" rel="stylesheet">
    					<style type="text/css">
    						body { padding-top: 70px; }
    						img { max-width: 100%; max-height: 300px;}
    					</style>

    					<!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
   		 				<!--[if lt IE 9]>
      						<script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      						<script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
    					<![endif]-->
  					</head>

  					<body>

   						<!-- Fixed navbar -->
    					<div class="navbar navbar-default navbar-fixed-top" role="navigation">
      						<div class="container">
       							<div class="navbar-header">
          							<button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
							            <span class="sr-only">Toggle navigation</span>
							            <span class="icon-bar"></span>
							            <span class="icon-bar"></span>
							            <span class="icon-bar"></span>
          							</button>
          							<a class="navbar-brand" href="#">1DV449 Lab 1</a>
        						</div>
        						<div class="navbar-collapse collapse">
	        						<form class="navbar-form navbar-left" method="post">
	  									<button type="submit" class="btn btn-default ladda-button"
	  									data-style="zoom-in" data-spinner-color="#FF0000"><span class="ladda-label">Trigger Scrape</span></button>
									</form>
        						</div><!--/.nav-collapse -->
      						</div>
    					</div>';
	}
}