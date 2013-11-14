<?php

require_once("./Scrape.php");

$scrape = new \model\Scrape("http://vhost3.lnu.se:20080/~1dv449/scrape/");

$scrape->run();