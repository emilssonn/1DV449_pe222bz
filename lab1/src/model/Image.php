<?php

namespace model;

class Image {

	/**
	 * @var string
	 */
	private $externalSrc;

	/**
	 * @var string
	 */
	private $name;

	/**
	 * @param string $externalSrc 
	 * @param string $name        
	 */
	public function __construct($externalSrc , $name = null) {
		$this->externalSrc = $externalSrc;
		$this->name = $name;
	}

	/**
	 * @return string
	 */
	public function getExternalSrc() {
		return $this->externalSrc;
	}

	/**
	 * @return string
	 */
	public function getName() {
		return $this->name;
	}
}