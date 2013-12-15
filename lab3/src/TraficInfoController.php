<?php

namespace controller;

class TraficInfoController {

	private $url = "http://api.sr.se/api/v2/traffic/messages";

	private $format = "json";

	private $size = "100";

	private $page = "1";

	public function getMessages() {

		$url = $this->url . "?format=" . $this->format . "&size=" . $this->size . "&page=" . $this->page;

		$ch = curl_init();

		curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);

		curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

		curl_setopt($ch, CURLOPT_URL,$url);

		$result = curl_exec($ch);

		curl_close($ch);

		return $result;
	}
}