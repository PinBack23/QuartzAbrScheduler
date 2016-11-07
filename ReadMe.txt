Idee
---------------------------------------

Was wir gebraucht und warum
--------------------------------
1. Windows Service
	- Hier läuft der Scheduler und führt die Jobs anhand der Trigger aus
	- Dynamisches laden der Jobs
	- Hinzufügen/Entfernen von Jobs und Trigger
2. Web Oberfläche
	- Zum anzeigen der aktuellen Jobs und Trigger.
	- Hochladen neuer Jobs
	- Erzeugen neuer Trigger
3. Framework zum erstellen neue Jobs

Was wird benutzt
--------------------------------
1. Quartz.Net für den Scheduler
2. MEF zum lesen der Jobs
3. Nancy zum Hosten der Web-Oberfläche im Service
4. angular zum erstellen der Web-Oberfläche
5. Logging mit Common.Logging (Quartz) und NLog
6. TopShelf zuerst mal zum test und zum installieren des Service

Quartz Infos
--------------------------------
- Ein Trigger kann nur einem Job zugewiesen sein.
- Ein Job kann aber mehrere Trigger haben
- Jobs und Trigger werden anhand eines Namen und einer Gruppe identifiziert

Jobs
--------------------------------
1. Die eigentlichen Jobs sind in Dll's definiert die über den Service hochgeladen werden
2. Ein Job wird über seinen Namen und seine Gruppe eindeutigt definiert.
3. Es wird Jobs geben die mit verschiedener Konfiguration mehrmals dem Scheduler zugeordnet werden
	z.B. Aufruf eine Rest-API Check Funktion mit verschiedenen URL's
4. Es gibt Jobs die eine bestimmte Konfiguration benötigen. Diese Konfiguration muss veränderbar sein.
	Die Konfiguration wird dem Job bei der Ausführung über die JobDataMap geliefert. Siehe: http://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/more-about-jobs.html
	Da nur serialisierbare Objekte in einer JobDataMap gespeichert werden könnne, wird die Konfiguration im JSON Format gespeichert und erzeugt.
5. Der Scheduler kann bestimmte Einträge, z.B. Verbindungseinstellungen zur DB, global bereitstellen und allen Jobs über die JobDataMap bereitstellen.

MEF
--------------------------------
Die Dll's im Jobverzeichnis können nicht ohne weiteres gelöscht bzw. erneuert werden.
Da die Dll's geblockt sind. Ein Lösungsversuch wird hier beschrieben:
http://www.codeproject.com/Articles/633140/MEF-and-AppDomain-Remove-Assemblies-On-The-Fly

Web-Api Doku mit Swagger
--------------------------------
http://www.softwarearchitekt.at/post/2015/06/15/swashbuckle-zur-generierung-von-swagger-dokumentationen-fur-web-apis-konfigurieren.aspx