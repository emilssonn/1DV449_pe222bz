<?php

require_once("./src/AppController.php");

$appController = new \controller\AppController();

echo $appController->run();