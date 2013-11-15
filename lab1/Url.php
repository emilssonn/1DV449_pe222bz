<?php

namespace model;

class Url {

	/**
	 * @var string
	 */
	private $url;

	/**
	 * @var int
	 */
	private $errorNr;

	/**
	 * @param string $url     
	 * @param int $errorNr 
	 */
	public function __construct($url, $errorNr = null) {
		$this->url = $url;
		$this->errorNr = $errorNr;
	}

	/**
	 * @return string
	 */
	public function getUrl() {
		return $this->url;
	}

	/**
	 * @return int
	 */
	public function getErrorNr() {
		return $this->errorNr;
	}
}