<?php

require_once(dirname(__FILE__)."/../config/lab1.php");
require_once("./model/Scrape.php");
require_once("./model/DbConnection.php");

$dbConnection = \model\DbConnection::getInstance();
try {
	$dbConnection->connect($dbServer, $dbUser, $dbPassword, $db);
} catch (\Exception $e) {
	echo "Fatal database error.";
	exit();
}

$scrape = new \model\Scrape("http://vhost3.lnu.se:20080/~1dv449/scrape/");

$scrape->run();

$dbConnection->close();