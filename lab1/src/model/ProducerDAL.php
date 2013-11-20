<?php

namespace model;

require_once("./src/model/Producer.php");
require_once("./src/model/Url.php");
require_once("./src/model/Image.php");
require_once("./src/model/ImageDAL.php");
require_once("./src/model/UrlDAL.php");

class ProducerDAL {
	
	/**
	 * Table name
	 * @var string
	 */
	private static $producerTable = "producer";

	/**
	 * @var \model\ImageDAL
	 */
	private $imageDAL;

	/**
	 * @var \model\UrlDAL
	 */
	private $urlDAL;

	/**
	 * @var \model\DbConnection
	 */
	private $dbConnection;

	public function __construct() {
		$this->dbConnection = \model\DbConnection::getInstance();
		$this->imageDAL = new \model\ImageDAL();
		$this->urlDAL = new \model\UrlDAL();
	}

	/**
	 * @return array of \model\Producer
	 */
	public function getProducers() {
		$producersArray = array();
		$sql = "SELECT
				id,
				pId,
				name,
				city,
				timesScraped,
				lastScraped
				FROM " . self::$producerTable;
		
		$statement = $this->dbConnection->runSql($sql);
		
		$result = $statement->get_result();

        while ($object = $result->fetch_array(MYSQLI_ASSOC)) {
        	try {
        		$image = $this->imageDAL->getImage($object["id"]);
	        	$url = $this->urlDAL->getUrl($object["id"]);
	        	$linkError = $this->urlDAL->getUrl($object["id"], true);

	            $producersArray[] = \model\Producer::createFull(
	            	$object["id"],
	            	$object["pId"],
	            	$object["name"],
	            	$linkError,
	            	$url,
	            	$object["city"],
	            	$image,
	            	$object["timesScraped"],
	            	$object["lastScraped"]);
        	} catch (\Exception $e) {
        		//Accept a fail
        	}
        }
        $statement->free_result();
        return $producersArray;
	}

	/**
	 * @param  array of \model\Producer
	 */
	public function saveProducers($producerArray) {
		$oldProducersArray = $this->getProducers();
		$newProducers = array();
		$updateProducers = array();
		$deleteProducers = array();

		foreach ($oldProducersArray as $oldProducer) {
			$delete = true;
			foreach ($producerArray as $producer) {
				if ($oldProducer->getpId() === $producer->getpId()) {
					$producer->setId($oldProducer->getId());
					$updateProducers[] = $producer;
					$delete = false;
					break;
				}
			}
			if ($delete) 
				$deleteProducers[] = $oldProducer;
		}

		foreach ($producerArray as $producer) {
			$new = true;
			foreach ($oldProducersArray as $oldProducer) {
				if ($oldProducer->getpId() === $producer->getpId()) {
					$new = false;
					break;
				}
			}
			if ($new)
				$newProducers[] = $producer;
		}

		foreach ($newProducers as $producer) {
			$this->insertProducer($producer);
		}

		foreach ($updateProducers as $producer) {
			$this->updateProducer($producer);
		}

		foreach ($deleteProducers as $producer) {
			$this->deleteProducer($producer);
		}
	}

	/**
	 * @param  \model\Producer $producer 
	 */
	private function insertProducer(\model\Producer $producer) {
		try {
			$sql = "INSERT INTO " . self::$producerTable . "
					(
						pId,
						name,
						city
					)
					VALUES(?, ?, ?)";

			$statement = $this->dbConnection->runSql($sql, 
				array($producer->getpId(), $producer->getName(), $producer->getCity()), 
				"iss");

			$statement->free_result();

			$id = $this->dbConnection->getLastInsertedId();
			$this->imageDAL->insertImage($producer->getImage(), $id);
			$this->urlDAL->insertUrl($producer->getUrl(), $id);
			$this->urlDAL->insertUrl($producer->getLinkError(), $id, true);
		} catch (\Exception $e) {
			//Accept a fail
		}	
	}

	/**
	 * @param  \model\Producer $producer                  
	 */
	private function updateProducer(\model\Producer $producer) {
		try {
			$sql = "UPDATE " . self::$producerTable . "
					SET
						name = ?,
						city = ?,
						timesScraped = timesScraped + 1
					WHERE id = ?";

			$statement = $this->dbConnection->runSql($sql, 
				array($producer->getName(), $producer->getCity(), $producer->getId()), 
				"ssi");
			
			$statement->free_result();

			$this->imageDAL->updateImage($producer->getImage(), $producer->getId());
			$this->urlDAL->updateUrl($producer->getUrl(), $producer->getId());
			$this->urlDAL->updateLinkError($producer->getUrl(), $producer->getId());
		} catch (\Exception $e) {
			//Accept a fail	
		}	
	}

	/**
	 * @param  \model\Producer $producer              
	 */
	private function deleteProducer(\model\Producer $producer) {
		try {
			$sql = "DELETE
				FROM " . self::$producerTable . " 
				WHERE id = ?";

			$this->dbConnection->runSql($sql, array($producer->getId()), "i");

			//Remove image from server
			$image = $Producer->getImage();
			$imageName = \model\Scrape::ImagesFilePath . $image->getName();
			if(file_exists($imageName)) {
				unlink($imageName);
			}
		} catch (\Exception $e) {
			//Accept a fail
		}
	}
}