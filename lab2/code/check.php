<?php
require_once("sec.php");

// check tha POST parameters
$u = $_POST['username'];
$p = $_POST['password'];

$u = trim($u);
$u = strip_tags($u);

// Check if user is OK
if(isUser($u, $p)) {
	// set the session
	sec_session_start();

	//Added to prevent session hijacking
	$_SESSION["remoteAddr"] = $_SERVER["REMOTE_ADDR"];
	$_SESSION["httpUserAgent"] = $_SERVER["HTTP_USER_AGENT"];

	$_SESSION['user'] = $u;
	header("Location: mess.php");
}
else {
	// To bad
	header('HTTP/1.1 401 Unauthorized');
}
