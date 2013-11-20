<?php

require_once(dirname(__FILE__)."/../config/lab1.php");
require_once("./src/model/DbConnection.php");
require_once("./src/controller/Scrape.php");

$dbConnection = \model\DbConnection::getInstance();
try {
	$dbConnection->connect($dbServer, $dbUser, $dbPassword, $db);
} catch (\Exception $e) {
	exit("Fatal database error.");
}

$scrapeController = new \controller\Scrape();

$html = $scrapeController->run();

$dbConnection->close();

echo $html;