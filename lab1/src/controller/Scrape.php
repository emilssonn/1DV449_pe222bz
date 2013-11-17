<?php

namespace controller;

require_once("./src/model/Scrape.php");
require_once("./src/view/Scrape.php");
require_once("./src/model/ScrapeDAL.php");

class Scrape {

	private $scrapeView;

	private $scrapeDAL;

	public function __construct() {
		$this->scrapeView = new \view\Scrape();
		$this->scrapeDAL = new \model\ScrapeDAL();
	}

	public function run() {
		if ($this->scrapeView->scrape()) {
			$scrapeModel = new \model\Scrape();
			$scrapeModel->run("http://vhost3.lnu.se:20080/~1dv449/scrape/");
			$this->scrapeView->goToIndex();
		} else {
			$producers = $this->scrapeDAL->getProducers();
			return $this->scrapeView->getHTML($producers);
		}
	}
}