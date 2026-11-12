# Spec Delta

## Purpose

Loans mit genau einer Drone und einer StudentGroup samt Zubehörmengen sicher erfassen, zurückgeben und dauerhaft nachvollziehen.

## ADDED Requirements

### Requirement: REQ-004 – Drone ausleihen

Quelle: Seiten 11, 13, 15 und 18.

Das System SHALL eine verfügbare Drone an eine bestehende StudentGroup
über eine aktive Loan ausleihen können. Die Loan SHALL genau diese Drone und
StudentGroup sowie LoanDate und Status Active festhalten.

#### Scenario: Verfügbare Drone ausleihen

- GIVEN „TELLO-07“ ist Available und die StudentGroup „Group 3“ existiert.
- WHEN „TELLO-07“ am 19.08.2026 an „Group 3“ ausgeliehen wird.
- THEN besteht eine Loan für diese Drone und StudentGroup mit Status Active und LoanDate 19.08.2026.
- AND die abgeleitete Verfügbarkeit von „TELLO-07“ ist Loaned.


Bestätigte Präzisierungen vom 12.11.2026: Das Backend SHALL LoanDate automatisch
als aktuellen Kalendertag in Europe/Vienna setzen, ohne manuelle Datumseingabe.
Datumswerte SHALL als YYYY-MM-DD übertragen werden; ReturnDate SHALL bei Active
null sein. Eine Loan ohne LoanItems SHALL erlaubt sein. Loan und sämtliche
LoanItems SHALL gemeinsam vollständig gespeichert oder vollständig verworfen
werden. Bei unbekannter Drone oder StudentGroup SHALL HTTP 404 ohne gespeicherte
Teil-Loan zurückgegeben werden. Fehlerantworten SHALL Problem-JSON mit status,
title und detail sowie bei Feldfehlern errors enthalten. Ungültige Eingaben
SHALL HTTP 400 liefern; technische Fehler SHALL nicht pauschal als Fachkonflikt
ausgegeben werden.


#### Scenario: Datum aus der Backend-Uhr und Loan ohne Zubehör

- GIVEN eine verfügbare Drone, eine bestehende StudentGroup und die Backend-Uhr zeigt 2026-08-18T22:30:00Z, also den 19.08.2026 in Europe/Vienna.
- WHEN eine Loan ohne LoanItems und ohne manuelle Datumsangabe erstellt wird.
- THEN besitzt die aktive Loan LoanDate "2026-08-19", ReturnDate null und keine LoanItems.

#### Scenario: Unbekannte Referenzen ablehnen

- GIVEN eine verfügbare Drone und eine bestehende StudentGroup.
- WHEN eine Loan mit einer unbekannten DroneId oder einer unbekannten StudentGroupId angefordert wird.
- THEN antwortet das Backend jeweils mit HTTP 404 und Problem-JSON.
- AND es werden weder Loan noch LoanItems gespeichert.

#### Scenario: Speicherfehler beim Erstellen verwirft die gesamte Loan

- GIVEN eine verfügbare Drone, eine bestehende StudentGroup und gültige Zubehörpositionen.
- WHEN während der Speicherung nach Beginn der Loan-Erstellung ein technischer Fehler auftritt.
- THEN bleiben nach erneutem Laden weder die neue Loan noch ihre LoanItems gespeichert und die Drone bleibt Available.
- AND die technische Störung wird nicht als fachlicher HTTP-409-Konflikt ausgegeben.

### Requirement: REQ-005 – Keine doppelte Loan

Quelle: Seiten 6, 12–13 und 15.

Das System SHALL höchstens eine aktive Loan je Drone zulassen und jede weitere
Loan für eine bereits ausgeliehene Drone im Backend mit HTTP 409 und Problem-JSON ablehnen.

#### Scenario: Zweite aktive Loan ablehnen

- GIVEN „TELLO-07“ hat bereits eine aktive Loan an „Group 3“.
- WHEN eine andere bestehende StudentGroup dieselbe Drone ausleihen möchte.
- THEN wird die neue Loan mit HTTP 409 und Problem-JSON abgelehnt.
- AND es besteht weiterhin nur die ursprüngliche aktive Loan für „TELLO-07“.

#### Scenario: Gleichzeitige Versuche erhalten die Fachregel

- GIVEN „TELLO-07“ ist Available und zwei StudentGroups existieren.
- WHEN beide StudentGroups gleichzeitig mit gültigen Angaben versuchen, „TELLO-07“ auszuleihen und keine unabhängige technische Störung vorliegt.
- THEN gelingt genau ein Versuch und genau eine aktive Loan entsteht.
- AND der andere Versuch erhält HTTP 409 und Problem-JSON und hinterlässt weder Loan noch LoanItems.

Das zweite Szenario prüft dieselbe Fachregel auch bei konkurrierenden Aktionen;
es legt keinen technischen Sperrmechanismus fest.

### Requirement: REQ-006 – Zubehör ausleihen

Quelle: Seiten 11–13, 16 und 18.

Das System SHALL Zubehör einer Loan als LoanItems mit Type Battery oder
Propeller und ganzzahliger Quantity größer oder gleich 0 erfassen können.
Das Backend SHALL negative Mengen, nichtganzzahlige Mengen und andere Typen
ablehnen. Zubehör SHALL ausschließlich als Menge ohne Einzelgeräte-Identitäten
geführt werden; eine LoanItem-Id identifiziert die Position.

#### Scenario: Zubehörmengen zuordnen

- GIVEN „TELLO-07“ ist Available und „Group 3“ existiert.
- WHEN diese Drone mit 4 Battery und 8 Propeller an diese StudentGroup ausgeliehen wird.
- THEN enthält die Loan eine LoanItem mit Type Battery und Quantity 4 sowie eine LoanItem mit Type Propeller und Quantity 8.
- AND es werden keine einzelnen Akkus oder Propeller inventarisiert.

Das System SHALL Quantity 0 akzeptieren, aber keine Nullposition speichern.
Fehlende Positionen SHALL in aktiver Liste und Historie als Menge 0 erscheinen.
Pro Loan SHALL höchstens eine eingereichte Position je Type erlaubt sein;
doppelte Typen SHALL auch bei Quantity 0 mit HTTP 400 abgelehnt und nicht
summiert werden. Bei ungültigem Zubehör SHALL die gesamte Anfrage mit HTTP 400
und Problem-JSON samt Feldfehlern abgelehnt werden, ohne Loan oder LoanItems
zu speichern.


#### Scenario: Menge null zulassen

- GIVEN eine verfügbare Drone und eine bestehende StudentGroup.
- WHEN eine Loan mit Quantity 0 für Type Battery und Quantity 0 für Type Propeller erfasst wird.
- THEN wird die Loan angelegt, ohne Nullpositionen zu speichern.
- AND aktive Liste und spätere Historie zeigen beide Mengen als 0.

#### Scenario: Ungültige Zubehörangaben ablehnen

- GIVEN eine verfügbare Drone und eine bestehende StudentGroup.
- WHEN eine Loan mit einer der Angaben Battery: -1, Propeller: 1,5 oder Type Cable angefordert wird.
- THEN lehnt das Backend die jeweilige ungültige Angabe ab.
- AND HTTP 400 mit Problem-JSON samt Feldfehlern wird geliefert.
- AND es werden weder die Loan noch LoanItems gespeichert, auch wenn weitere Positionen gültig waren.

#### Scenario: Doppelte Zubehörtypen ablehnen

- GIVEN eine verfügbare Drone und eine bestehende StudentGroup.
- WHEN eine Loan zwei Battery-Positionen mit Mengen 2 und 3 oder mit Mengen 0 und 3 enthält.
- THEN wird die gesamte Anfrage jeweils mit HTTP 400 und Problem-JSON samt Feldfehlern abgelehnt.
- AND es werden weder Loan noch LoanItems gespeichert; die Mengen werden nicht summiert.


### Requirement: REQ-007 – Aktive Loans anzeigen

Quelle: Seiten 13 und 16.

Das System SHALL aktive Loans mit StudentGroup, Drone, Battery-Menge,
Propeller-Menge und LoanDate anzeigen. Zurückgegebene Loans SHALL nicht in
dieser Liste erscheinen.

#### Scenario: Aktive Loan mit Zubehör anzeigen

- GIVEN eine aktive Loan von „TELLO-07“ an „Group 3“ vom 19.08.2026 mit 4 Battery und 8 Propeller sowie eine zurückgegebene Loan existieren.
- WHEN die aktiven Loans angezeigt werden.
- THEN erscheint die aktive Loan mit „Group 3“, „TELLO-07“, den Mengen 4 und 8 sowie LoanDate 19.08.2026.
- AND die zurückgegebene Loan erscheint dort nicht.


### Requirement: REQ-008 – Loan zurückgeben

Quelle: Seiten 12–13 und 17–18.

Das System SHALL bei Rückgabe einer aktiven Loan deren Status von Active auf
Returned ändern. Die Drone SHALL dadurch wieder Available werden. Die Loan und
ihre LoanItems SHALL erhalten bleiben.

#### Scenario: Rückgabe gibt die Drone frei

- GIVEN „TELLO-07“ ist über eine aktive Loan an „Group 3“ ausgeliehen.
- WHEN diese Loan zurückgegeben wird.
- THEN hat sie Status Returned und „TELLO-07“ ist wieder Available.
- AND die ursprünglichen Zuordnungen, LoanDate und LoanItems bleiben erhalten.

Eine erneute Rückgabe derselben Loan SHALL HTTP 409 und Problem-JSON liefern,
ohne ReturnDate oder andere Daten zu verändern. Eine unbekannte Loan SHALL
HTTP 404 und Problem-JSON liefern. Rückgaben SHALL atomar sein: Bei Fehlern
bleiben Status, ReturnDate und alle übrigen gespeicherten Daten unverändert.


#### Scenario: Nach Rückgabe erneut ausleihen

- GIVEN eine Loan wurde zurückgegeben und die zugehörige Drone ist Available.
- WHEN diese Drone erneut an eine bestehende StudentGroup ausgeliehen wird.
- THEN entsteht eine neue aktive Loan.
- AND die frühere Loan bleibt mit Status Returned und ihren LoanItems erhalten.


#### Scenario: Wiederholte Rückgabe ohne neue Loan ablehnen

- GIVEN eine Returned Loan mit gespeichertem ReturnDate und ihre verfügbare Drone.
- WHEN dieselbe Loan erneut zurückgegeben wird.
- THEN antwortet das Backend mit HTTP 409 und Problem-JSON.
- AND ReturnDate, Status, Zuordnungen, LoanDate und LoanItems bleiben unverändert; die Drone bleibt Available.

#### Scenario: Alte Rückgabe beeinflusst neue Loan nicht

- GIVEN eine Returned Loan und eine neue Active Loan derselben Drone existieren.
- WHEN die alte Loan erneut zurückgegeben wird.
- THEN antwortet das Backend mit HTTP 409 und Problem-JSON.
- AND beide Loans samt Daten bleiben unverändert und die Drone bleibt Loaned.

#### Scenario: Unbekannte Loan zurückgeben

- GIVEN die angefragte LoanId existiert nicht.
- WHEN eine Rückgabe für diese Id angefordert wird.
- THEN antwortet das Backend mit HTTP 404 und Problem-JSON, ohne Daten zu verändern.

#### Scenario: Speicherfehler bei Rückgabe erhält den Ausgangszustand

- GIVEN eine Active Loan mit ReturnDate null und gespeicherten LoanItems.
- WHEN beim Speichern ihrer Rückgabe ein technischer Fehler auftritt.
- THEN bleibt nach erneutem Laden die Loan Active mit ReturnDate null und die Drone Loaned.
- AND Zuordnungen, LoanDate und LoanItems bleiben unverändert.

### Requirement: REQ-009 – ReturnDate speichern

Quelle: Seiten 11, 13 und 17–18.

Das System SHALL bei Rückgabe einer Loan deren ReturnDate dauerhaft speichern.
Das Backend SHALL ReturnDate automatisch als aktuellen Kalendertag in
Europe/Vienna ohne manuelle Datumseingabe setzen und als YYYY-MM-DD übertragen.
Liegt dieser Tag vor LoanDate, SHALL die Rückgabe mit HTTP 400 und Problem-JSON
abgelehnt werden, ohne Daten zu verändern.

#### Scenario: Rückgabedatum bleibt erhalten

- GIVEN eine aktive Loan vom 19.08.2026 existiert.
- WHEN ihre Rückgabe bei Backend-Zeit 2026-08-19T22:30:00Z (20.08.2026 in Europe/Vienna) ohne manuelle Datumseingabe erfolgt und die gespeicherte Loan anschließend erneut geladen wird.
- THEN ist ihr ReturnDate 20.08.2026 und ihr Status Returned.
- AND ihr LoanDate bleibt 19.08.2026.

#### Scenario: Rückgabedatum vor LoanDate ablehnen

- GIVEN eine Active Loan mit LoanDate 2026-08-20 und eine Backend-Uhr mit Kalendertag 2026-08-19 in Europe/Vienna.
- WHEN die Loan zurückgegeben werden soll.
- THEN antwortet das Backend mit HTTP 400 und Problem-JSON.
- AND Status bleibt Active, ReturnDate bleibt null und die Drone bleibt Loaned; alle übrigen Daten bleiben unverändert.


### Requirement: REQ-010 – Ausleihhistorie anzeigen

Quelle: Seiten 5–6, 12–13 und 17–18.

Das System SHALL zurückgegebene Loans mit ihren LoanItems dauerhaft aufbewahren
und als Historie anzeigen können. Eine Rückgabe SHALL keine Löschung auslösen.

#### Scenario: Historie nach Rückgabe und erneutem Laden anzeigen

- GIVEN eine Loan von „TELLO-07“ an „Group 3“ vom 19.08.2026 mit 4 Battery und 8 Propeller wurde am 20.08.2026 zurückgegeben.
- WHEN die gespeicherten Daten erneut geladen werden und die Historie angezeigt wird.
- THEN ist dieselbe Loan mit Status Returned, ihren Zuordnungen, LoanDate, ReturnDate und den unveränderten LoanItems weiterhin nachvollziehbar.

Die Anzeige dient der im Auftrag geforderten Nachvollziehbarkeit. Ein konkretes
Tabellenlayout, Filter oder Sortierungen sind nicht festgelegt.
