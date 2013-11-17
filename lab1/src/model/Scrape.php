<?php

namespace model;

require_once("./src/model/Producer.php");
require_once("./src/model/Url.php");
require_once("./src/model/Image.php");
require_once("./src/model/ScrapeDAL.php");

class Scrape {

	/**
	 * @var string
	 */
	private $baseUrl;

	/**
	 * @var cURL handle
	 */
	private $ch = null;

	/**
	 * @var string
	 */
	private $cookieFilePath;

	/**
	 * @var string
	 */
	private $imagesFilePath = "./src/data/images/";

	/**
	 * @param string $baseUrl
	 */
	public function __construct() {
		$this->initCurl();
		$this->cookieFilePath = dirname(__FILE__) . "/../data/temp/cookie.txt";
	}

	public function run($baseUrl) {
		$this->baseUrl = $baseUrl;
		$locationUrl;

		try {
			$loginLink = $this->getLoginLink();		
			$locationUrl = $this->postLogin($loginLink);
		} catch (\Exception $e) {
			exit("Login Failed");
		}
		
		try {
			$producers = $this->getProducers($locationUrl);
			
			curl_close($this->ch);
			if(file_exists($this->cookieFilePath)) {
			    unlink($this->cookieFilePath);
			}
			$scrapeDAL = new \model\ScrapeDAL();
			$scrapeDAL->saveProducers($producers);
		} catch (\Exception $e) {
			
		}
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
		curl_setopt($this->ch, CURLOPT_NOBODY, false);
		$city = null;
		$url = null;
		$image = null;
		
		$producerId = (int)preg_replace('/\D/', '', $link->getAttribute("href"));
		$producerName = $this->sanitizeString($link->nodeValue);
		
		$link = $this->baseUrl . "secure/" . $link->getAttribute("href");
		curl_setopt($this->ch, CURLOPT_URL, $link);
		
		$xpath = $this->curlExec(false);
		
		if (curl_getinfo($this->ch, CURLINFO_HTTP_CODE) >= 400) {
			$failedUrl = new \model\Url($link, curl_getinfo($this->ch, CURLINFO_HTTP_CODE));
			return \model\Producer::createWithError($producerId, $producerName, $failedUrl); 
		}

		//If fail, variable = null
		try {
			$city = $this->getCity($xpath);	
		} catch (\Exception $e) {}
		try {
			$image = $this->getImage($xpath);
		} catch (\Exception $e) {}
		try {
			$url = $this->getUrl($xpath);
		} catch (\Exception $e) {}
		
		return \model\Producer::createSimple($producerId, $producerName, $url, $city, $image); 
	}

	/**
	 * @param  \DomXPath $xpath 
	 * @return \model\Image
	 * @throws \Exception        
	 */
	private function getImage($xpath) {
		$items = $xpath->query('//div[@class = "hero-unit"]/img');
		if ($items === false || $items->length === 0 ||
			 $items->item(0)->getAttribute("src") === "") {
			throw new \Exception();
		}

		$imageSrc = $items->item(0)->getAttribute("src");
		$imageName = $this->sanitizeString(substr($imageSrc, strrpos($imageSrc, "/") + 1));
		$url = $this->baseUrl . "secure/" . $imageSrc;
		$internalUrl = "$this->imagesFilePath$imageName";
		$fp = fopen($internalUrl, 'wb');

		curl_setopt($this->ch, CURLOPT_URL, $url);
		curl_setopt($this->ch, CURLOPT_FILE, $fp);
		curl_setopt($this->ch, CURLOPT_HEADER, false);
		curl_exec($this->ch);
		fclose($fp);
		$httpResponse = curl_getinfo($this->ch, CURLINFO_HTTP_CODE);
		$this->initCurl();

		if ($httpResponse === 0 || $httpResponse >= 400) {
			return new \model\Image($this->sanitizeString($url));
		}
		return new \model\Image($this->sanitizeString($url), $imageName);
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
		curl_setopt($this->ch, CURLOPT_NOBODY, true);
		$data = curl_exec($this->ch);
		$httpResponse = curl_getinfo($this->ch, CURLINFO_HTTP_CODE);

		if ($httpResponse === 0 || $httpResponse >= 400) {
			return new \model\Url($this->sanitizeString($url), $httpResponse);
		}
		return new \model\Url($this->sanitizeString($url));
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
		return $this->sanitizeString(substr($s, $pos+4));
	}

	/**
	 * @param  string $locationUrl 
	 * @return DOMNodeList
	 * @throws \Exception
	 */
	private function getProducersLinks($locationUrl) {	
		curl_setopt($this->ch, CURLOPT_URL, $locationUrl);
		
		$xpath = $this->curlExec();
		return $xpath->query('//table[@class = "table table-striped"]//tr//td/a');
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

		$data = curl_exec($this->ch);
		$locationUrl = curl_getinfo($this->ch, CURLINFO_REDIRECT_URL);
		return $locationUrl;
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
	 * @param  boolean $throw, bubble Exceptions or not
	 * @return \DOMXPath
	 */
	private function curlExec($throw = true) {
		$data = curl_exec($this->ch);
		$data = utf8_decode($data);
		$dom = new \DOMDocument();
		if (!$throw) {
			libxml_use_internal_errors(true);
			$dom->loadHTML($data);
			libxml_use_internal_errors(false);
		} else {
			$dom->loadHTML($data);
		}
		return new \DOMXPath($dom);
	}

	/**
	 * Reset cURL
	 */
	private function initCurl() {
		if ($this->ch !== null)
			curl_close($this->ch);
		$this->ch = curl_init();
		if ($this->ch === false)
			exit("Fatal error, stopping script");
		curl_setopt($this->ch, CURLOPT_RETURNTRANSFER, true);
		curl_setopt($this->ch, CURLOPT_CONNECTTIMEOUT, 2);
	}

	/**
	 * @param  string $string 
	 * @return string         
	 */
	private function sanitizeString($string) {
		$string = strip_tags($string);
		$string = trim($string);
		return $string;
	}
}