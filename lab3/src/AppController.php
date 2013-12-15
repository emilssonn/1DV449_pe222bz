<?php

namespace controller;

require_once("./src/TraficInfoController.php");
require_once("./src/AppView.php");

class AppController {

	private $traficInfoGET = 'traficInfo';

	public function run() {
		if (isset($_GET[$this->traficInfoGET])) {
			$traficInfoController = new \controller\TraficInfoController();
			return $traficInfoController->getMessages();
		} else {
			$appView = new \view\AppView();
			return $appView->getHTML();
		}
	}
}