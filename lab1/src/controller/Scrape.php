<?php

namespace controller;

require_once("./src/model/Scrape.php");
require_once("./src/view/Scrape.php");
require_once("./src/model/ProducerDAL.php");

class Scrape {

	/**
	 * @var \view\Scrape
	 */
	private $scrapeView;

	/**
	 * @var \model\ProducerDAL
	 */
	private $producerDAL;

	public function __construct() {
		$this->scrapeView = new \view\Scrape();
		$this->producerDAL = new \model\ProducerDAL();
	}

	/**
	 * @return string HTML
	 */
	public function run() {
		if ($this->scrapeView->scrape()) {
			try {
				$scrapeModel = new \model\Scrape();
				$scrapeModel->run("http://vhost3.lnu.se:20080/~1dv449/scrape/");
			} catch (\Exception $e) {
				
			}	
			$this->scrapeView->goToIndex();
		} else {
			try {
				$producers = $this->producerDAL->getProducers();
				return $this->scrapeView->getHTML($producers);
			} catch (\Exception $e) {
				return $this->scrapeView->getErrorHTML();
			}	
		}
	}
}