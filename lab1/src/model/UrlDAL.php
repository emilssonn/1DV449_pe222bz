<?php

namespace model;

require_once("./src/model/Url.php");

class UrlDAL {

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
	 * @param  int $id
	 * @return \model\Url or null //bad
	 */
	public function getUrl($id, $error = false) {
		$sql = "SELECT
				url,
				errorNr
				FROM " . self::$urlTable . "
				WHERE pId = ? 
				AND absolutePath = ?";

		$error = $error ? 0 : 1;

		$statement = $this->dbConnection->runSql($sql, array($id, $error), "ii");

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
	 * @param  \model\Url $url 
	 * @param  int $id       
	 */
	public function insertUrl(\model\Url $url = null, $id, $error = false) {
		if ($url !== null) {
			$sql = "INSERT INTO " . self::$urlTable . "
					(
						pId,
						url,
						errorNr,
						absolutePath
					)
					VALUES(?, ?, ?, ?)";

			$error = $error ? 0 : 1;

			$statement = $this->dbConnection->runSql($sql, 
				array($id, $url->getUrl(), $url->getErrorNr(), $error), 
				"isii");
			
			$statement->free_result();
		}
	}

	/**
	 * @param  \model\Url $url 
	 * @param  int $id     
	 */
	public function updateLinkError(\model\Url $url = null, $id) {
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
	 * @param  \model\Url $url 
	 * @param  int $id      
	 */
	public function updateUrl(\model\Url $url = null, $id) {
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
}