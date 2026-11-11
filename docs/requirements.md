# Anforderungen: Drohnenverwaltung

## Quelle und Geltungsbereich

Verbindliche Quelle ist der vollständig gelesene, 43-seitige
[Projektauftrag](source/INF4_SYP_19_Projektauftrag_Drohnenverwaltung.pdf).
Seitenangaben beziehen sich auf die PDF-Seiten, einschließlich Titelblatt.
Die REQ-Kennungen und Zuordnung stammen aus Seite 13; die Details aus Seiten
14–18. Die folgenden SHALL-Sätze und Szenarien formulieren diese Vorgaben
prüfbar aus. Sie beschreiben den geplanten Sollzustand, keine implementierte
Funktion und noch keine OpenSpec-Spezifikation. Die Dokumente sind Input für
den späteren OpenSpec-Workflow (Seite 13).

Die gemeinsame Sprache umfasst ausschließlich die Entities Drone, StudentGroup,
Loan und LoanItem; siehe [Domänenmodell](ddd.md). Battery und Propeller sind
Werte für Zubehörtypen, keine zusätzlichen Entities.

## REQ-001 – Drone anlegen

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

## REQ-002 – Drones anzeigen

Quelle: Seiten 6, 13–14.

Das System SHALL Drones in einer Liste mit Name, Beschreibung und dem
abgeleiteten Status Available oder Loaned anzeigen.

#### Scenario: Verfügbarkeit in der Liste erkennen

- GIVEN „TELLO-02“ besitzt keine aktive Loan und „TELLO-07“ besitzt eine aktive Loan.
- WHEN die Liste der Drones angezeigt wird.
- THEN erscheinen beide Drones mit ihrem Namen und ihrer Beschreibung.
- AND „TELLO-02“ wird als Available und „TELLO-07“ als Loaned angezeigt.

## REQ-003 – StudentGroup anlegen

Quelle: Seiten 11, 13–14.

Das System SHALL das Anlegen einer StudentGroup mit Id und Name ermöglichen.

#### Scenario: StudentGroup anlegen

- GIVEN eine StudentGroup „Group 3“ wurde noch nicht angelegt.
- WHEN eine StudentGroup mit Name „Group 3“ angelegt wird.
- THEN ist eine StudentGroup mit einer Id und Name „Group 3“ vorhanden.

## REQ-004 – Drone ausleihen

Quelle: Seiten 11, 13, 15 und 18.

Das System SHALL eine verfügbare Drone an eine bestehende StudentGroup
über eine aktive Loan ausleihen können. Die Loan SHALL genau diese Drone und
StudentGroup sowie LoanDate und Status Active festhalten.

#### Scenario: Verfügbare Drone ausleihen

- GIVEN „TELLO-07“ ist Available und die StudentGroup „Group 3“ existiert.
- WHEN „TELLO-07“ am 19.08.2026 an „Group 3“ ausgeliehen wird.
- THEN besteht eine Loan für diese Drone und StudentGroup mit Status Active und LoanDate 19.08.2026.
- AND die abgeleitete Verfügbarkeit von „TELLO-07“ ist Loaned.

## REQ-005 – Keine doppelte Loan

Quelle: Seiten 6, 12–13 und 15.

Das System SHALL höchstens eine aktive Loan je Drone zulassen und jede weitere
Loan für eine bereits ausgeliehene Drone im Backend ablehnen.

#### Scenario: Zweite aktive Loan ablehnen

- GIVEN „TELLO-07“ hat bereits eine aktive Loan an „Group 3“.
- WHEN eine andere bestehende StudentGroup dieselbe Drone ausleihen möchte.
- THEN wird die neue Loan abgelehnt.
- AND es besteht weiterhin nur die ursprüngliche aktive Loan für „TELLO-07“.

#### Scenario: Gleichzeitige Versuche erhalten die Fachregel

- GIVEN „TELLO-07“ ist Available und zwei StudentGroups existieren.
- WHEN beide StudentGroups gleichzeitig versuchen, „TELLO-07“ auszuleihen.
- THEN entsteht höchstens eine aktive Loan für „TELLO-07“.

Das zweite Szenario prüft dieselbe Fachregel auch bei konkurrierenden Aktionen;
es legt keinen technischen Sperrmechanismus fest.

## REQ-006 – Zubehör ausleihen

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

#### Scenario: Menge null zulassen

- GIVEN eine verfügbare Drone und eine bestehende StudentGroup.
- WHEN eine Loan mit Quantity 0 für Type Battery und Quantity 0 für Type Propeller erfasst wird.
- THEN wird die Loan nicht wegen dieser Mengen abgelehnt.

#### Scenario: Ungültige Zubehörangaben ablehnen

- GIVEN eine verfügbare Drone und eine bestehende StudentGroup.
- WHEN eine Loan mit einer der Angaben Battery: -1, Propeller: 1,5 oder Type Cable angefordert wird.
- THEN lehnt das Backend die jeweilige ungültige Angabe ab.
- AND es wird keine LoanItem mit dieser ungültigen Angabe gespeichert.

## REQ-007 – Aktive Loans anzeigen

Quelle: Seiten 13 und 16.

Das System SHALL aktive Loans mit StudentGroup, Drone, Battery-Menge,
Propeller-Menge und LoanDate anzeigen. Zurückgegebene Loans SHALL nicht in
dieser Liste erscheinen.

#### Scenario: Aktive Loan mit Zubehör anzeigen

- GIVEN eine aktive Loan von „TELLO-07“ an „Group 3“ vom 19.08.2026 mit 4 Battery und 8 Propeller sowie eine zurückgegebene Loan existieren.
- WHEN die aktiven Loans angezeigt werden.
- THEN erscheint die aktive Loan mit „Group 3“, „TELLO-07“, den Mengen 4 und 8 sowie LoanDate 19.08.2026.
- AND die zurückgegebene Loan erscheint dort nicht.

## REQ-008 – Loan zurückgeben

Quelle: Seiten 12–13 und 17–18.

Das System SHALL bei Rückgabe einer aktiven Loan deren Status von Active auf
Returned ändern. Die Drone SHALL dadurch wieder Available werden. Die Loan und
ihre LoanItems SHALL erhalten bleiben.

#### Scenario: Rückgabe gibt die Drone frei

- GIVEN „TELLO-07“ ist über eine aktive Loan an „Group 3“ ausgeliehen.
- WHEN diese Loan zurückgegeben wird.
- THEN hat sie Status Returned und „TELLO-07“ ist wieder Available.
- AND die ursprünglichen Zuordnungen, LoanDate und LoanItems bleiben erhalten.

## REQ-009 – ReturnDate speichern

Quelle: Seiten 11, 13 und 17–18.

Das System SHALL bei Rückgabe einer Loan deren ReturnDate dauerhaft speichern.

#### Scenario: Rückgabedatum bleibt erhalten

- GIVEN eine aktive Loan vom 19.08.2026 existiert.
- WHEN ihre Rückgabe mit dem Rückgabedatum 20.08.2026 erfolgt und die gespeicherte Loan anschließend erneut geladen wird.
- THEN ist ihr ReturnDate 20.08.2026 und ihr Status Returned.
- AND ihr LoanDate bleibt 19.08.2026.

Das Szenario bestimmt weder eine manuelle Datumseingabe noch die Verwendung
einer Systemuhr; dies ist im PDF nicht entschieden.

## REQ-010 – Ausleihhistorie anzeigen

Quelle: Seiten 5–6, 12–13 und 17–18.

Das System SHALL zurückgegebene Loans mit ihren LoanItems dauerhaft aufbewahren
und als Historie anzeigen können. Eine Rückgabe SHALL keine Löschung auslösen.

#### Scenario: Historie nach Rückgabe und erneutem Laden anzeigen

- GIVEN eine Loan von „TELLO-07“ an „Group 3“ vom 19.08.2026 mit 4 Battery und 8 Propeller wurde am 20.08.2026 zurückgegeben.
- WHEN die gespeicherten Daten erneut geladen werden und die Historie angezeigt wird.
- THEN ist dieselbe Loan mit Status Returned, ihren Zuordnungen, LoanDate, ReturnDate und den unveränderten LoanItems weiterhin nachvollziehbar.

Die Anzeige dient der im Auftrag geforderten Nachvollziehbarkeit. Ein konkretes
Tabellenlayout, Filter oder Sortierungen sind nicht festgelegt.

## Domain Rules

Quelle: Seite 12; Präzisierungen zu Typen, Mengen und Rückgabe auf Seiten 16–18.

| Regel | Verbindliches Verhalten | Abgedeckt durch |
| --- | --- | --- |
| DR-01 | Eine Drone SHALL höchstens eine aktive Loan besitzen. | REQ-005 |
| DR-02 | Nur Available Drones SHALL ausgeliehen werden dürfen. | REQ-004, REQ-005 |
| DR-03 | Eine Rückgabe SHALL die Loan beenden und die Drone wieder Available machen. | REQ-008 |
| DR-04 | Quantity SHALL ganzzahlig und nichtnegativ sein; das Backend SHALL ungültige Mengen ablehnen. | REQ-006 |
| DR-05 | Zurückgegebene Loans und ihre LoanItems SHALL dauerhaft gespeichert bleiben. | REQ-008 bis REQ-010 |
| DR-06 | Battery und Propeller SHALL ausschließlich als Mengen ohne eigene Geräteidentität geführt werden. | REQ-006 |
| DR-07 | Verfügbarkeit SHALL aus aktiven Loans abgeleitet werden: Active vorhanden → Loaned; sonst Available. Ein unabhängig gepflegter Verfügbarkeitszustand ist keine zweite fachliche Quelle. | REQ-002, REQ-004, REQ-008 |

Geschäftslogik und Validierung liegen im ASP.NET-Core-10-Backend. Angular mit
TypeScript kommuniziert ausschließlich über REST/JSON; Entity Framework Core
und SQLite sind für die Persistenz vorgegeben (Seiten 7–8). Diese Architektur
ersetzt keine fachliche Anforderung und fügt keine weitere REQ-Kennung hinzu.

## Out of Scope

Quelle: Seite 19. Ausdrücklich ausgeschlossen sind:

- Login, Benutzerkonten und Benutzerrollen.
- Einzelinventarisierung von Akkus oder anderem Zubehör, insbesondere Akku-IDs.
- Reservierungen im Voraus.
- Schadensverwaltung und Rückgabedifferenzen, insbesondere fehlendes Zubehör.
- Auswertungen, Statistiken und Exporte.

Die späteren Change-Request-Beispiele auf Seiten 37–38 sind keine jetzt
beauftragten Funktionen. Class, Members und ResponsibleStudent (Seite 14) sowie
SSID als eigenes Feld (Seite 37) gehören ebenfalls nicht zum aktuellen Modell.
Eine Schadensnotiz im Freitext Description begründet keine Ausleihsperre.

## Unklarheiten und noch offene Entscheidungen

Diese Punkte sind im PDF nicht eindeutig festgelegt und werden hier nicht als
zusätzliche Anforderungen beschlossen:

- Id-Datentyp und Vergabe; Pflichtfelder, Textlängen, leere Namen sowie Namenseindeutigkeit.
- Genauigkeit, Zeitzone und Ermittlung von LoanDate/ReturnDate; Datumsvalidierung und Darstellung einer noch nicht erfolgten Rückgabe.
- Verhalten bei mehrfacher Rückgabe derselben Loan und bei nicht existierenden Referenzen; konkrete API-Fehlercodes.
- Ob eine Menge 0 als LoanItem gespeichert oder durch eine fehlende Position dargestellt wird; ob pro Loan und Type genau eine Position zulässig ist. Seite 18 zeigt zwei Positionen als Beispiel, schreibt aber keine allgemeine Eindeutigkeit vor.
- Genaue Darstellung der Historie, über die nachvollziehbaren Angaben aus dem Beispiel hinaus; Filterung, Sortierung und Seiteneinteilung sind nicht beauftragt.
- Seite 43 verweist zusätzlich auf einen SYP-Leitfaden „Drohnenverwaltung“. Dieser liegt nicht vor; als verbindliche Quelle gilt hier das vom Auftraggeber bereitgestellte PDF.

Die PDF-Definition von LoanItem widerspricht der bisherigen Projektkonfiguration:
Seiten 10–11 definieren eine Zubehörposition mit Type und Quantity; DroneId
gehört zur Loan. Die Definition in AGENTS.md und openspec/config.yaml wird anhand
der nun verfügbaren Quelle korrigiert. Es werden keine weiteren Entities eingeführt.
