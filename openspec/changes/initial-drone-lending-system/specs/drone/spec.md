# Spec Delta

## Purpose

Drones mit Name und Beschreibung erfassen und ihre aus aktiven Loans abgeleitete Verfügbarkeit nachvollziehbar anzeigen.

## ADDED Requirements

### Requirement: REQ-001 – Drone anlegen

Quelle: Seiten 11, 13–14.

Das System SHALL das Anlegen einer Drone mit Id, Name und Description als
Freitext ermöglichen. Der angezeigte Verfügbarkeitsstatus SHALL aus den aktiven
Loans entstehen und nicht beim Anlegen eingegeben werden.

#### Scenario: Drone mit Beschreibung anlegen

- GIVEN eine Drone mit dem Namen „TELLO-07“ wurde noch nicht angelegt.
- WHEN eine Drone mit Name „TELLO-07“ und Description „SSID: TELLO-9FA21B, rechter Propellerschutz beschädigt“ angelegt wird.
- THEN ist eine Drone mit einer Id und genau diesen Textangaben vorhanden.
- AND ihre abgeleitete Verfügbarkeit ist Available, solange keine aktive Loan besteht.

Das Beispiel dokumentiert Freitext; daraus entsteht keine automatische
Schadensprüfung oder ein separates SSID-Feld.


Die am 12.11.2026 bestätigte Präzisierung ergänzt den PDF-Auftrag: Das Backend
SHALL Name als Pflichtfeld behandeln, äußere Leerzeichen entfernen und danach
1 bis 100 Zeichen zulassen. Gleiche Namen SHALL zulässig sein; unterschiedliche
Drones SHALL anhand ihrer Id unterscheidbar bleiben. Die Auswahl SHALL bei
Namensgleichheit zusätzlich die Id anzeigen. Ungültige Namen SHALL HTTP 400
mit Problem-JSON (status, title, detail und bei Feldfehlern errors) liefern,
ohne eine Drone anzulegen.

#### Scenario: Namen normalisieren und doppelte Namen zulassen

- GIVEN eine Drone mit Name „TELLO-07“ existiert.
- WHEN eine weitere Drone mit Name „  TELLO-07  “ angelegt wird.
- THEN besitzt sie Name „TELLO-07“ und eine andere Id.
- AND eine Auswahl gleichnamiger Drones zeigt zusätzlich ihre Ids.

#### Scenario: Ungültige Namen ablehnen

- GIVEN eine Anfrage zum Anlegen einer Drone.
- WHEN Name fehlt, null ist, nur aus Leerzeichen besteht oder nach Entfernen äußerer Leerzeichen 101 Zeichen enthält.
- THEN antwortet das Backend jeweils mit HTTP 400 und Problem-JSON samt Feldfehler.
- AND es wird keine Drone gespeichert.

#### Scenario: Gültige Namensgrenzen akzeptieren

- GIVEN eine Anfrage zum Anlegen einer Drone.
- WHEN Name nach Entfernen äußerer Leerzeichen 1 oder 100 Zeichen enthält, auch mit Umlauten oder Sonderzeichen.
- THEN wird die Drone mit diesem Namen angelegt.

### Requirement: REQ-002 – Drones anzeigen

Quelle: Seiten 6, 13–14.

Das System SHALL Drones in einer Liste mit Name, Beschreibung und dem
abgeleiteten Status Available oder Loaned anzeigen.

#### Scenario: Verfügbarkeit in der Liste erkennen

- GIVEN „TELLO-02“ besitzt keine aktive Loan und „TELLO-07“ besitzt eine aktive Loan.
- WHEN die Liste der Drones angezeigt wird.
- THEN erscheinen beide Drones mit ihrem Namen und ihrer Beschreibung.
- AND „TELLO-02“ wird als Available und „TELLO-07“ als Loaned angezeigt.
