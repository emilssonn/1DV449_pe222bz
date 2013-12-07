<?php
require_once("get.php");
require_once("add.php");
require_once("sec.php");
sec_session_start();
checkUser();

/*
* It's here all the ajax calls goes
*/ 
if (isset($_POST['function'])) {
  if($_POST['function'] == 'add') {
       
    checkToken(sanitizeString($_POST['postToken']));
    $name = $_POST["name"];
    $message = $_POST["message"];
    $pid = $_POST["pid"];
    
    addToDB($name, $message, $pid);
    echo "Det gick fint! Ladda om sidan för att se ditt meddelande!";
  }
}
else if(isset($_GET['function'])) {
  if($_GET['function'] == 'producers') {
  	$pid = $_GET["pid"];
   	echo(json_encode(getFullProducer($pid)));
  } 
}