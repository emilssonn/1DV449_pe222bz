<?php

namespace model;

require_once("./Producer.php");
require_once("./Url.php");

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
		curl_setopt($this->ch, CURLOPT_CONNECTTIMEOUT, 2);
		$locationUrl;

		try {
			$loginLink = $this->getLoginLink();		
			$locationUrl = $this->postLogin($loginLink);
		} catch (\Exception $e) {
			exit("Login Failed");
		}
		
		try {
			$producers = $this->getProducers($locationUrl);
			/**
			 * Debug code
			 */
			foreach ($producers as $value) {
				var_dump($value);
			}
		} catch (\Exception $e) {
			
		}

		curl_close($this->ch);

		/*
		Commented for dev
		if(file_exists($this->cookieFilePath)) {
		    unlink($this->cookieFilePath);
		}
		*/
	}

	/**
	 * @param  string $locationUrl
	 * @return array of \model\Producer
	 * @throws \Exception If                
	 */
	private function getProducers($locationUrl) {
		curl_setopt($this->ch, CURLOPT_HTTPGET, true);
		try {
			$producersArray = array();
			$producersItems = $this->getProducersLinks($locationUrl);
			
			foreach ($producersItems as $item) {
				try {
					$producersArray[] = $this->getProducer($item);
				} catch (\Exception $e) {
					//Do nothing
				}	
			}
			return $producersArray;
		} catch (\Exception $e) {
			throw $e;
		}
	}

	/**
	 * @param  DOMNode $link 
	 * @return \model\Producer
	 */
	private function getProducer($link) {
		curl_setopt($this->ch, CURLOPT_COOKIEFILE, $this->cookieFilePath);
		$xpath;
		$city = null;
		$url = null;
		
		$producerId = (int)preg_replace('/\D/', '', $link->getAttribute("href"));
		$producerName = $link->nodeValue;
		
		$link = $this->baseUrl . "secure/" . $link->getAttribute("href");
		curl_setopt($this->ch, CURLOPT_URL, $link);
		
		try {
			$xpath = $this->curlExec(false);
		} catch (\Exception $e) {
			return \model\Producer::createWithError($producerId, $producerName); 
		}

		try {
			$city = $this->getCity($xpath);	
		} catch (\Exception $e) {}
		try {
			$url = $this->getUrl($xpath);
		} catch (\Exception $e) {}

		return \model\Producer::createSimple($producerId, $producerName, $url, $city); 
	}

	/**
	 * @param  \DOMXPath $xpath
	 * @return \model\Url
	 * @throws \Exception if no Url is found
	 */
	private function getUrl($xpath) {
		$items = $xpath->query('//div[@class = "hero-unit"]//p[last()]//a');

		if ($items === false || $items->length === 0 ||
			 $items->item(0)->getAttribute("href") === "#") {
			throw new \Exception();
		}
		$url = $items->item(0)->getAttribute("href");

		curl_setopt($this->ch, CURLOPT_COOKIEFILE, "");
		curl_setopt($this->ch, CURLOPT_URL, $url);

		$data = curl_exec($this->ch);
		$httpResponse = curl_getinfo($this->ch, CURLINFO_HTTP_CODE);

		if ($httpResponse === 0 || $httpResponse >= 400) {
			return new \model\Url($url, $httpResponse);
		}
		return new \model\Url($url);
	}

	/**
	 * @param  \DOMXPath $xpath
	 * @return string
	 * @throws \Exception If no city is found
	 */
	private function getCity($xpath) {
		$items = $xpath->query('//div[@class = "hero-unit"]//span[@class = "ort"]');
		if ($items === false || $items->length === 0)
			throw new \Exception();
		$s = $items->item(0)->nodeValue;
		$pos = strpos(strtolower($s), "ort:");
		return trim(substr($s, $pos+4));
	}

	/**
	 * @param  string $locationUrl 
	 * @return DOMNodeList
	 * @throws \Exception
	 */
	private function getProducersLinks($locationUrl) {	
		curl_setopt($this->ch, CURLOPT_URL, $locationUrl);
		
		try {
			$xpath = $this->curlExec();
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
			$xpath = $this->curlExec();
			$items = $xpath->query('//form[@class = "form-signin"]/@action');

			if (count($items) === 1)
				return $items->item(0)->nodeValue;
			else
				throw new \Exception("Error getting login post link");
		} catch (\Exception $e) {
			throw $e;
		}
	}

	/**
	 * @param  boolean $throw, throw Exceptions or not
	 * @return \DOMXPath
	 */
	private function curlExec($throw = true) {
		$data = curl_exec($this->ch);
		$dom = new \DOMDocument();

		if (!$throw) {
			libxml_use_internal_errors(true);
			$dom->loadHTML('<?xml encoding="UTF-8">' . $data);
			libxml_use_internal_errors(false);
		} else {
			$dom->loadHTML('<?xml encoding="UTF-8">' . $data);
		}
		return new \DOMXPath($dom);
	}
}

/*
curl_setopt($this->ch, CURLOPT_HEADER, true);
curl_setopt($this->ch, CURLINFO_HEADER_OUT, true);
 */