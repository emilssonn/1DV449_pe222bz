<?php
require_once("get.php");
require_once("add.php");
require_once("sec.php");
sec_session_start();
checkUser();
/*
* It's here all the ajax calls goes
*/ 
if(isset($_GET['function'])) {
	
  if($_GET['function'] == 'add') {
       
    $name = $_GET["name"];
		$message = $_GET["message"];
		$pid = $_GET["pid"];
		
		addToDB($name, $message, $pid);
		echo "Det gick fint! Ladda om sidan för att se ditt meddelande!";

  } else if($_GET['function'] == 'producers') {

  	$pid = $_GET["pid"];
   	echo(json_encode(getFullProducer($pid)));

  } else if ($_GET['function'] == 'newMessages') {

    $pid = $_GET["pid"];
    echo(json_encode(getNewMessages($pid)));

  }
}