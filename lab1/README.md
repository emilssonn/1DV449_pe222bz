Laboration 01
=============

##### Url
Med cachning: http://peteremilsson.se/course/1dv449/lab1/

Denna kommer inte uppdatera det som visas direkt, men inom 1-5 min.

Utan cachning: http://peteremilsson.se.preview.citynetwork.se/course/1dv449/lab1/

"Trigger Scrape" knappen borde inte finnas/vara publik i en "riktigt" applikation

Reflektion
----------

#### Ni är fria att välja sätt att läsa in och extrahera data ur webbsidorna. Motivera ditt val!

Jag valde att skriva i PHP för att det är det som jag har använts mest senaste tiden. Hade tänkt att skriva i Node.js men var ett tag sen jag skrev något i det.
Jag valde att använda cURL, XPath och DomDocuemnt för att extrahera data till stor del för att det var det som gicks igenom på föreläsningen, men även för att iallafall cURL och XPath går att använda i mer än PHP.

#### Vad finns det för risker med applikationer som innefattar automatisk skrapning av webbsidor? Nämn minst tre stycken!

* Man måste ta hänsyn till variationer i uppbyggnaden av informationen som ska hämtas. Om man gör en webbskrapa som är fokuserad på bara 1 webbplats och 1 del av den webbplatsen så minskar problemen. Det går inte heller att lita fullt ut på att data alltid kommer vara uppbyggd på samma sätt.
* En resurs kan ha flyttats, kan vara en död länk, server kan ta lång tid på sig att svara på ett anrop.
* Om applikationen gör något mer än att bara skrapa webbsidor, tex presenterar den skapade informationen så kan en stor del av hela applikationen bli beroende av information från en annan webbplats som hämtas på ett sätt som webbplats ägaren kanske inte hade tänkt sig. (Bättre med tex API:er).

#### Tänk dig att du skulle skrapa en sida gjord i ASP.NET WebForms. Vad för extra problem skulle man kunna få då?

* Kan använda viewstate för att kommma ihåg en sidas status.
* Webforms kan autogenerera en del extra HTML.

#### Har du gjort något med din kod för att vara "en god webbskrapare" med avseende på hur du skrapar sidan?

Nej, inte vad jag vet.

#### Vad har du lärt dig av denna uppgift? 

* Enklare cURL, med cookie och fil hantering
* Enkla xpath uttryck
* Problem som kan upptså när man skrapar en webbsida, tekniska och moraliska.