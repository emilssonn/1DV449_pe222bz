<?php


function getFullProducer($id) {
	$db = null;

	try {
		$db = new PDO("sqlite:producerDB.sqlite");
		$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	}
	catch(PDOException $e) {
		die("Database error");
		//Debug
		//die("Del -> " . $e->getMessage());	
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
		echo("Database error");
		//Debug
		//echo("Error creating query: " .$e->getMessage());
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
	catch(PDOException $e) {
		die("Database error");
		//Debug
		//die("Del -> " . $e->getMessage());	
	}
	
	$q = "SELECT `serial`, message, name, pid, time 
		FROM messages WHERE pid = ?
		ORDER BY time DESC";
	
	$result;
	$stm;	
	try {
		$stm = $db->prepare($q);
		$stm->bindParam(1, $pid, PDO::PARAM_INT);
		$stm->execute();
		$result = $stm->fetchAll(PDO::FETCH_ASSOC);
	}
	catch(PDOException $e) {
		echo("Database error");
		//Debug
		//echo("Error creating query: " .$e->getMessage());
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
	catch(PDOException $e) {
		die("Database error");
		//Debug
		//die("Del -> " . $e->getMessage());	
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
		echo("Database error");
		//Debug
		//echo("Error creating query: " .$e->getMessage());
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
	catch(PDOException $e) {
		die("Database error");
		//Debug
		//die("Del -> " . $e->getMessage());	
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
		echo("Database error");
		//Debug
		//echo("Error creating query: " .$e->getMessage());
		return false;
	}
	
	if($result)
		return $result;
	else
	 	return false;
}

//Look for new messages for the specific producer
function getNewMessages($pid, $time) {
	//End the session, making it available for other request from same client
	//http://www.php.net/session_write_close
	session_write_close();
	
	//20 seconds per requests, then return even if no result.
	//20 seconds because of the webbsite structur.
	//Average time spent looking on one producer will not be high.
	//Better to send multiple request than having one long.
	//This will make the server load smaller, the chance of the server doing work that the client dont want anymore is lower.
	$timeutTime = 20;
	//Current time since start
	$timeout = 0;
	$db = null;
	//Check for new message in the given intervall
    while($timeout < $timeutTime) {
    	try {
			$db = new PDO("sqlite:db.db");
			$db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		}
		catch(PDOException $e) {
			die("Database error");
			//Debug
			//die("Del -> " . $e->getMessage());	
		}

		//Select only messages that has been created after the request
    	$q = "SELECT `serial`, message, name, pid, time
    			FROM messages WHERE pid = ? AND  time > ?
    			ORDER BY time DESC";

    	$result = null;
		$stm = null;
		try {
			$stm = $db->prepare($q);
			$stm->bindParam(1, $pid, PDO::PARAM_INT);
			$stm->bindParam(2, $time, PDO::PARAM_INT);
			$stm->execute();
			$result = $stm->fetchAll(PDO::FETCH_ASSOC);
		}
		catch(PDOException $e) {
			echo("Database error");
			//Debug
			//echo("Error creating query: " .$e->getMessage());
			return false;
		}

		//If a message was found, return
		if ($result && count($result) > 0) {
			return $result;
		} else {
			//Close connection
			//Not sure if it should be closed for each iteration or not, performance?
			$db = null;

			//Sleep for 4 seconds then check again for new messages
	        sleep(4);
	        $timeout += 4;
		}
    }

    //No messages were found
    return false;
}