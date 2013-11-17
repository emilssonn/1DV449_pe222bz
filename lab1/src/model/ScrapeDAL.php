<?php

namespace model;

require_once("./src/model/Producer.php");
require_once("./src/model/Url.php");
require_once("./src/model/Image.php");

class ScrapeDAL {
	
	/**
	 * Table name
	 * @var string
	 */
	private static $producerTable = "producer";

	/**
	 * Table name
	 * @var string
	 */
	private static $imageTable = "image";

	/**
	 * Table name
	 * @var string
	 */
	private static $urlTable = "url";

	/**
	 * @var \model\DbConnection
	 */
	private $dbConnection;

	public function __construct() {
		$this->dbConnection = \model\DbConnection::getInstance();
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
        		$image = $this->getImage($object["id"]);
	        	$url = $this->getUrl($object["id"]);
	        	$linkError = $this->getLinkError($object["id"]);

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
        		
        	}
        }
        $statement->free_result();
        return $producersArray;
	}

	/**
	 * @param  int $id 
	 * @return \model\Url or null //bad     
	 */
	private function getLinkError($id) {
		$sql = "SELECT
				url,
				errorNr
				FROM " . self::$urlTable . "
				WHERE pId = ? 
				AND absolutePath = 0";

		$statement = $this->dbConnection->runSql($sql, array($id), "i");

		$result = $statement->bind_result($url, $errorNr);
		if ($result == FALSE) {
			throw new \Exception("bind of [$sql] failed " . $statement->error);
		}

		if ($statement->fetch()) {
			return new \model\Url($url, $errorNr);
		} else {
			return null;
		}
	}

	/**
	 * @param  int $id 
	 * @return \model\Image or null //bad     
	 */
	private function getImage($id) {
		$sql = "SELECT
				externalSrc,
				name
				FROM " . self::$imageTable . "
				WHERE pId = ?";

		$statement = $this->dbConnection->runSql($sql, array($id), "i");

		$result = $statement->bind_result($externalSrc, $name);
		if ($result == FALSE) {
			throw new \Exception("bind of [$sql] failed " . $statement->error);
		}

		if ($statement->fetch()) {
			return new \model\Image($externalSrc, $name);
		} else {
			return null;
		}
	}

	/**
	 * @param  int $id
	 * @return \model\Url or null //bad
	 */
	private function getUrl($id) {
		$sql = "SELECT
				url,
				errorNr
				FROM " . self::$urlTable . "
				WHERE pId = ? 
				AND absolutePath = 1";

		$statement = $this->dbConnection->runSql($sql, array($id), "i");

		$result = $statement->bind_result($url, $errorNr);
		if ($result == FALSE) {
			throw new \Exception("bind of [$sql] failed " . $statement->error);
		}

		if ($statement->fetch()) {
			return new \model\Url($url, $errorNr);
		} else {
			return null;
		}
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
			$this->insertImage($producer->getImage(), $id);
			$this->insertUrl($producer->getUrl(), $id);
			$this->insertLinkError($producer->getLinkError(), $id);
		} catch (\Exception $e) {
			
		}	
	}

	/**
	 * @param  \model\Url $url 
	 * @param  int $id       
	 */
	private function insertLinkError(\model\Url $url = null, $id) {
		var_dump($url);
		if ($url !== null) {
			$sql = "INSERT INTO " . self::$urlTable . "
					(
						pId,
						url,
						errorNr,
						absolutePath
					)
					VALUES(?, ?, ?, ?)";

			$statement = $this->dbConnection->runSql($sql, 
				array($id, $url->getUrl(), $url->getErrorNr(), 0), 
				"isii");
			
			$statement->free_result();
		}
	}

	/**
	 * @param  \model\Image $image 
	 * @param  int $id    
	 */
	private function insertImage(\model\Image $image = null, $id) {
		if ($image !== null) {
			$sql = "INSERT INTO " . self::$imageTable . "
					(
						pId,
						externalSrc,
						name
					)
					VALUES(?, ?, ?)";

			$statement = $this->dbConnection->runSql($sql, 
				array($id, $image->getExternalSrc(), $image->getName()), 
				"iss");
			
			$statement->free_result();
		}
	}

	/**
	 * @param  \model\Url $url 
	 * @param  int $id       
	 */
	private function insertUrl(\model\Url $url = null, $id) {
		if ($url !== null) {
			$sql = "INSERT INTO " . self::$urlTable . "
					(
						pId,
						url,
						errorNr
					)
					VALUES(?, ?, ?)";

			$statement = $this->dbConnection->runSql($sql, 
				array($id, $url->getUrl(), $url->getErrorNr()), 
				"isi");
			
			$statement->free_result();
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

			$this->updateImage($producer->getImage(), $producer->getId());
			$this->updateUrl($producer->getUrl(), $producer->getId());
			$this->updateLinkError($producer->getUrl(), $producer->getId());
		} catch (\Exception $e) {
				
		}	
	}

	/**
	 * @param  \model\Url $url 
	 * @param  int $id     
	 */
	private function updateLinkError(\model\Url $url = null, $id) {
		if ($url !== null) {
			$sql = "UPDATE " . self::$urlTable . "
					SET
						url = ?,
						errorNr = ?
					WHERE pId = ? 
					AND absoultePath = 0";

			$statement = $this->dbConnection->runSql($sql, 
				array($url->getUrl(), $url->getErrorNr(), $id), 
				"sii");
			
			$statement->free_result();
		} else {
			$sql = "DELETE
				FROM " . self::$urlTable . " 
				WHERE pId = ? 
				AND absoultePath = 0";

			try {
				$this->dbConnection->runSql($sql, array($id), "i");
			} catch (\Exception $e) {
				
			}		
		}
	}

	/**
	 * @param  \model\Image $image 
	 * @param  int $id            
	 */
	private function updateImage(\model\Image $image = null, $id) {
		if ($image !== null) {
			$sql = "UPDATE " . self::$imageTable . "
					SET
						externalSrc = ?,
						name = ?
					WHERE pId = ?";

			$statement = $this->dbConnection->runSql($sql, 
				array($image->getExternalSrc(), $image->getName(), $id), 
				"ssi");
			
			$statement->free_result();
		}
	}

	/**
	 * @param  \model\Url $url 
	 * @param  int $id      
	 */
	private function updateUrl(\model\Url $url = null, $id) {
		if ($url !== null) {
			$sql = "UPDATE " . self::$urlTable . "
					SET
						url = ?,
						errorNr = ?
					WHERE pId = ?";

			$statement = $this->dbConnection->runSql($sql, 
				array($url->getUrl(), $url->getErrorNr(), $id), 
				"sii");
			
			$statement->free_result();
		}
	}

	/**
	 * @param  \model\Producer $producer              
	 */
	private function deleteProducer(\model\Producer $producer) {
		$sql = "DELETE
				FROM " . self::$producerTable . " 
				WHERE id = ?";

		$this->dbConnection->runSql($sql, array($producer->getId()), "i");

		//Delete Image
	}
}