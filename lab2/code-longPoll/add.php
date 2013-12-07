<?php

/**
* Called from AJAX to add stuff to DB
*/
function addToDB($name, $message, $pid) {
	$db = null;

	$name = sanitizeString($name);
	$message = sanitizeString($message);
	$pid = sanitizeString($pid);
	
	try {
		$db = new PDO("sqlite:db.db");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Something went wrong -> " .$e->getMessage());
	}
	$q = "INSERT INTO messages (message, name, pid, time) VALUES(?, ?, ?, strftime('%s','now'))";
	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $message, PDO::PARAM_STR);
		$stm->bindParam(2, $name, PDO::PARAM_STR);
		$stm->bindParam(3, $pid, PDO::PARAM_INT);
		$stm->execute();
	}
	catch(PDOException $e) {
		die("Something went wrong -> " .$e->getMessage());
	}
}

function sanitizeString($string) {
	$string = trim($string);
	$string = strip_tags($string);
	return $string;
}
