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
    echo "Det gick fint!";
  }
}
else if(isset($_GET['function'])) {
  if($_GET['function'] == 'producers') {
    $pid = $_GET["pid"];
    echo(json_encode(getFullProducer($pid)));

  } else if ($_GET['function'] == 'newMessages') {
    $time;

    //If the timestamp is set, otherwise generate the current timestamp
    if (isset($_GET['time']))
      $time = $_GET['time'];
    else
      $time = time();

    $pid = $_GET["pid"];
    echo(json_encode(getNewMessages($pid, $time)));

  }
}