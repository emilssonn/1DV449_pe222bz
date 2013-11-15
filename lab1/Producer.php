<?php

namespace model;

class Producer {

	private $id;
	
	private $name;

	private $link404;

	/**
	 * @var \model\Url
	 */
	private $url;

	private $city;

	private $imageUrl;

	private $timesScraped;

	private $lastScraped;

	private function __construct($id, $name, $link404, $url = null, $city = null, $imageUrl = null, $timesScraped = null, $lastScraped = null) {
		$this->id = $id;
		$this->name = $name;
		$this->link404 = $link404;
		$this->url = $url;
		$this->city = $city;
		$this->imageUrl = $imageUrl;
		$this->timesScraped = $timesScraped;
		$this->lastScraped = $lastScraped;
	}

	public static function createSimple($id, $name, $url, $city) {
		return new \model\Producer($id, $name, false, $url, $city);
	}

	public static function createWithError($id, $name) {
		return new \model\Producer($id, $name, true);
	}

	public static function createFull($id, $name, $link404, \model\Url $url, $city, $imageUrl, $timesScraped, $lastScraped) {
		return new \model\Producer($id, $name, $link404, $url, $city, $imageUrl, $timesScraped, $lastScraped);
	}

	public function setUrl(\model\Url $url) {
		$this->url = $url;
	}

	public function setCity($city) {
		$this->city = $city;
	}

	public function setImageUrl($imageUrl) {
		$this->imageUrl = $imageUrl;
	}

}