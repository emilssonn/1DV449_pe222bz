<?php

namespace model;

require_once("./src/model/Image.php");

class ImageDAL {

	/**
	 * Table name
	 * @var string
	 */
	private static $imageTable = "image";

	/**
	 * @var \model\DbConnection
	 */
	private $dbConnection;

	public function __construct() {
		$this->dbConnection = \model\DbConnection::getInstance();
	}

	/**
	 * @param  int $id 
	 * @return \model\Image or null //bad     
	 */
	public function getImage($id) {
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
	 * @param  \model\Image $image 
	 * @param  int $id    
	 */
	public function insertImage(\model\Image $image = null, $id) {
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
	 * @param  \model\Image $image 
	 * @param  int $id            
	 */
	public function updateImage(\model\Image $image = null, $id) {
		if ($image !== null) {
			$oldImage = $this->getImage($id);
			$sql = "UPDATE " . self::$imageTable . "
					SET
						externalSrc = ?,
						name = ?
					WHERE pId = ?";

			$statement = $this->dbConnection->runSql($sql, 
				array($image->getExternalSrc(), $image->getName(), $id), 
				"ssi");
			
			$statement->free_result();

			//Remove old image from server
			if ($oldImage->getName() !== $image->getName()) {
				$imageName = \model\Scrape::ImagesFilePath . $oldImage->getName();
				if(file_exists($imageName)) {
					unlink($imageName);
				}
			}
		}
	}
}