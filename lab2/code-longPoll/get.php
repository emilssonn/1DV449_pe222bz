<?php


function getFullProducer($id) {
	$db = null;

	try {
		$db = new PDO("sqlite:producerDB.sqlite");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}
	
	$q = "SELECT * FROM Producers WHERE producerID = ?";
	
	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $id, PDO::PARAM_INT);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Error creating query: " .$e->getMessage());
		return false;
	}
	
	if($result) {
		$messages = getMessagesByProducer($id);
		$result[0]['messages'] = $messages;
		return $result[0];
	}
	else
	 	return false;
}

function getMessagesByProducer($pid) {
	$db = null;

	try {
		$db = new PDO("sqlite:db.db");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}
	
	$q = "SELECT `serial`, message, name, pid, datetime(time, 'unixepoch', 'localtime') as time FROM messages WHERE pid = ?";
	
	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $pid, PDO::PARAM_INT);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Error creating query: " .$e->getMessage());
		return null;
	}
	
	if($result)
		return $result;
	else
	 	return null;
}



function getProducer($id) {
	$db = null;

	try {
		$db = new PDO("sqlite:producerDB.sqlite");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}
	
	$q = "SELECT name, city, url, imageUrl FROM Producers WHERE producerID = ?";
	
	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $id, PDO::PARAM_INT);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Error creating query: " .$e->getMessage());
		return false;
	}
	
	if($result)
		return $result[0];
	else
	 	return false;
}

function getProducers() {
	$db = null;

	try {
		$db = new PDO("sqlite:producerDB.sqlite");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}
	
	$q = "SELECT producerID, name FROM Producers";
	
	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Error creating query: " .$e->getMessage());
		return false;
	}
	
	if($result)
		return $result;
	else
	 	return false;
}

function getNewMessages($pid) {
	session_write_close();
	$db = null;

	try {
		$db = new PDO("sqlite:db.db");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOEception $e) {
		die("Del -> " .$e->getMessage());
	}
	
	
	$timeutTime = 20;
	$timeout = 0;
	$time = time();

    while($timeout < $timeutTime) {
    	$q = "SELECT `serial`, message, name, pid, datetime(time, 'unixepoch', 'localtime') as time FROM messages WHERE pid = ? AND  time > $time";

    	$result = null;
		$stm = null;
		try {
			$stm = $db->prepare($q);
			$stm->bindParam(1, $pid, PDO::PARAM_INT);
			$stm->execute();
			$result = $stm->fetchAll(PDO::FETCH_ASSOC);
		}
		catch(PDOException $e) {
			echo("Error creating query: " .$e->getMessage());
			return false;
		}

		if ($result && count($result) > 0) {
			return $result;
		} else {
	        sleep(5);
	        $timeout += 5;
		}
    }

    return false;
}