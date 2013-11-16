<?php

namespace model;

class Producer {

	/**
	 * @var int
	 */
	private $id;

	/**
	 * @var int
	 */
	private $pId;
	
	/**
	 * @var string
	 */
	private $name;

	/**
	 * @var bool
	 */
	private $link404;

	/**
	 * @var \model\Url
	 */
	private $url;

	/**
	 * @var string
	 */
	private $city;

	/**
	 * @var \model\Image
	 */
	private $image;

	/**
	 * @var int
	 */
	private $timesScraped;

	/**
	 * @var string
	 */
	private $lastScraped;

	private function __construct($pId, $name, $link404, \model\Url $url = null, $city = null, \model\Image $image = null, $timesScraped = null, $lastScraped = null, $id = null) {
		$this->pId = $pId;
		$this->name = $name;
		$this->link404 = $link404;
		$this->url = $url;
		$this->city = $city;
		$this->image = $image;
		$this->timesScraped = $timesScraped;
		$this->lastScraped = $lastScraped;
		$this->id = $id;
	}

	public static function createSimple($pId, $name, \model\Url $url = null, $city, \model\Image $image = null) {
		return new \model\Producer($pId, $name, false, $url, $city, $image);
	}

	public static function createWithError($pId, $name) {
		return new \model\Producer($pId, $name, true);
	}

	public static function createFull($id, $pId, $name, $link404, \model\Url $url = null, $city, \model\Image $image = null, $timesScraped, $lastScraped) {
		return new \model\Producer($pId, $name, $link404, $url, $city, $image, $timesScraped, $lastScraped, $id);
	}

	public function getId() {
		return $this->id;
	}

	public function getpId() {
		return $this->pId;
	}

	public function getName() {
		return $this->name;
	}

	public function getLink404() {
		return $this->link404;
	}

	public function getUrl() {
		return $this->url;
	}

	public function getCity() {
		return $this->city;
	}

	public function getImage() {
		return $this->image;
	}

	public function getTimesScraped() {
		return $this->timesScraped;
	}

	public function getLastScraped() {
		return $this->lastScraped;
	}

	public function setId($id) {
		if ($this->id !== null ||
			!is_numeric($id))
			throw new \Exception();
		$this->id = $id;		
	}

}