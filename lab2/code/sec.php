<?php

require_once("password.php");

/**
 * Just som simple scripts for session handling
*/
function sec_session_start() {
        $session_name = 'sec_session_id'; // Set a custom session name
        $secure = false; // Set to true if using https.
        $httponly = true; // This stops javascript being able to access the session id. 
        ini_set('session.use_only_cookies', 1); // Forces sessions to only use cookies. 
        $cookieParams = session_get_cookie_params(); // Gets current cookies params.
        session_set_cookie_params(3600, $cookieParams["path"], $cookieParams["domain"], $secure, $httponly); 
        session_name($session_name); // Sets the session name to the one set above.
        session_start(); // Start the php session
        session_regenerate_id(); // regenerated the session, delete the old one.  
}

function checkUser() {
	if(!session_id()) {
		sec_session_start();
	}

	if(!isset($_SESSION["user"])) {header('HTTP/1.1 401 Unauthorized'); die();}
	
	$user = getUser($_SESSION["user"]);
	$un = $user[0]["username"];
	
	//Prevent session hijacking
	if ($_SESSION["remoteAddr"] != $_SERVER["REMOTE_ADDR"] || 
		$_SESSION["httpUserAgent"] != $_SERVER["HTTP_USER_AGENT"]) {
		header('HTTP/1.1 401 Unauthorized'); die();
	}
}

function isUser($u, $p) {
	$db = null;

	try {
		$db = new PDO("sqlite:db.db");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}

	$q = "SELECT id, password FROM users WHERE username = ?";

	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $u, PDO::PARAM_STR);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Error creating query: " .$e->getMessage());
		return false;
	}

	if($result) {
		if (password_verify($p, $result[0]['password'])) {
    		return true;
		} 
	}
	return false;
	
}

function getUser($user) {
	$db = null;

	try {
		$db = new PDO("sqlite:db.db");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}
	$q = "SELECT * FROM users WHERE username = ?";
	
	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $user, PDO::PARAM_STR);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Error creating query: " .$e->getMessage());
		return false;
	}
	
	return $result;
}

function logout() {
	if(!session_id()) {
		sec_session_start();
	}
	session_destroy();
	header('Location: index.php');
}