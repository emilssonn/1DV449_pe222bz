<?php

namespace model;

require_once("./Producer.php");

class Scrape {

	/**
	 * @var string
	 */
	private $baseUrl;

	/**
	 * @var cURL handle
	 */
	private $ch;

	/**
	 * @var string
	 */
	private $cookieFilePath;

	/**
	 * @param string $baseUrl
	 */
	public function __construct($baseUrl) {
		$this->ch = curl_init();
		if ($this->ch === false)
			exit("Fatal error, stopping script");
		$this->baseUrl = $baseUrl;
		$this->cookieFilePath = dirname(__FILE__) . "/data/temp/cookie.txt";
	}

	public function run() {
		curl_setopt($this->ch, CURLOPT_RETURNTRANSFER, true);
		$locationUrl;

		try {
			$loginLink = $this->getLoginLink();		
			$locationUrl = $this->postLogin($loginLink);
		} catch (\Exception $e) {
			exit("Login Failed");
		}
		
		try {
			$this->getProducers($locationUrl);
		} catch (\Exception $e) {
			
		}
	}

	private function getProducers($locationUrl) {
		try {
			$producersArray = array();
			$producersItems = $this->getProducersLinks($locationUrl);
			
			foreach ($producersItems as $item) {
				$producersArray[] = $this->getProducer($item);
			}
		} catch (\Exception $e) {
			throw $e;
		}
	}

	private function getProducer($link) {
		/**
		 * @todo get all information for one producer
		 */
	}

	/**
	 * @param  string $locationUrl 
	 * @return DOMNodeList
	 * @throws \Exception
	 */
	private function getProducersLinks($locationUrl) {
		curl_setopt($this->ch, CURLOPT_HTTPGET, true);
		curl_setopt($this->ch, CURLOPT_URL, $locationUrl);
		curl_setopt($this->ch, CURLOPT_COOKIEFILE, $this->cookieFilePath);

		try {
			$data = curl_exec($this->ch);
			$dom = new \DOMDocument();
			$dom->loadHTML($data);
			$xpath = new \DOMXPath($dom);
			$items = $xpath->query('//table[@class = "table table-striped"]//tr//td/a');
			
			return $items;
		} catch (\Exception $e) {
			throw $e;
		}
	}

	/**
	 * If this function fails the script should not continue with scraping
	 * @param  string $loginPostUrl url to post to
	 * @return string, url to continue to
	 * @throws \Exception If login fails
	 */
	private function postLogin($loginPostUrl) {
		$loginUrl = $this->baseUrl . $loginPostUrl;
		curl_setopt($this->ch, CURLOPT_POST, true);
		curl_setopt($this->ch, CURLOPT_URL, $loginUrl);
		curl_setopt($this->ch, CURLOPT_COOKIEJAR, $this->cookieFilePath);
		
		$postArray = array(
			"username" => "admin",
			"password" => "admin"
		);
		curl_setopt($this->ch, CURLOPT_POSTFIELDS, $postArray);

		try {
			$data = curl_exec($this->ch);
			$locationUrl = curl_getinfo($this->ch, CURLINFO_REDIRECT_URL);
			return $locationUrl;
		} catch (\Exception $e) {
			throw $e;
		}
	}

	/**
	 * If this function fails the script should not continue with scraping
	 * @return string, login link
	 * @throws \Exception If link is not found
	 */
	private function getLoginLink() {
		curl_setopt($this->ch, CURLOPT_URL, $this->baseUrl);
		try {
			$data = curl_exec($this->ch);
			$dom = new \DOMDocument();

			$dom->loadHTML($data);
			$xpath = new \DOMXPath($dom);
			$items = $xpath->query('//form[@class = "form-signin"]/@action');

			if (count($items) === 1)
				return $items->item(0)->nodeValue;
			else
				throw new \Exception("Error getting login post link");
		} catch (\Exception $e) {
			throw $e;
		}
	}
}

/*
curl_setopt($this->ch, CURLOPT_HEADER, true);
curl_setopt($this->ch, CURLINFO_HEADER_OUT, true);
 */