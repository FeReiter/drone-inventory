# Tasks

Alle Tasks sind geplant und unimplementiert. Vor jedem Implementierungstask
werden die unten verknüpften Issues gelesen. Am 12.11.2026 wurden auf ausdrücklichen
Auftrag alle 26 Tasks jeweils einem GitHub Issue im Project Drone Inventory
mit Status Backlog zugeordnet. PR #3 ist gemergt; die Workflow-Skills wurden beim Abgleich mit main
unverändert übernommen. Je Task eigener Branch
und eigene PR, passende Tests, git diff und git diff --check sowie Dokumentation.

Bestehende Task-IDs bleiben stabil. 1.1 wurde in Einrichtung (1.1), Schema (1.3)
und Uhr/Fehlervertrag (1.4) zerlegt. 3.2 liefert die Eingabevalidierung vor 3.1;
3.6 ergänzt den übergreifenden Rollback-Nachweis; 5.6 prüft Rückgabe-Rollback und konkurrierende Rückgaben separat. Die Reihenfolge unten ist eine
mögliche Ausführungsreihenfolge; maßgeblich sind die expliziten Abhängigkeiten.
Jeder Funktionstask enthält eigene Tests. Alle neuen Negativszenarien sind Teil
der betroffenen Funktionstasks, keine erst am Projektende nachgeholten Prüfungen.

## 1. Backend-Grundlage

- [ ] 1.1 EF Core/SQLite und Backend-Testprojekt einrichten (REQ-001 bis REQ-010). Prüfung: Backend bauen, Testprojekt ausführen und temporäre SQLite-Datei öffnen/schließen; Befehle dokumentieren. (GitHub: #5)
- [ ] 1.3 Schema und erste Migration für die vier Entities einrichten (REQ-001, REQ-003, REQ-004, REQ-006, REQ-009, REQ-010; nach 1.1). Prüfung: Migration auf leerer temporärer Datei anwenden, Daten einschließlich DateOnly/null speichern und nach erneutem Öffnen laden; Fremdschlüssel, Status-/Type-Bedingungen, ganzzahlige nichtnegative Mengen und eindeutiges (LoanId, Type) prüfen. Migration und Prüfungen dokumentieren. (GitHub: #6)
- [ ] 1.2 Eindeutigen partiellen Index für aktive Loans pro Drone einrichten (REQ-005; nach 1.3). Prüfung: mehrere Returned Loans und höchstens eine Active Loan je Drone; aktive Loans verschiedener Drones erlaubt. Index und Prüfbefehl dokumentieren. (GitHub: #7)
- [ ] 1.4 Backend-Uhr und gemeinsames Problem-JSON vorbereiten (REQ-001, REQ-003, REQ-004, REQ-008, REQ-009; nach 1.1). Prüfung: gesteuerte Uhr liefert Europe/Vienna-Kalendertag an UTC-Tagesgrenze sowie in Winter-/Sommerzeit; Fehlerdarstellung enthält status/title/detail und bei Feldfehlern errors. Technische Fehler werden nicht pauschal zu 409. Datums-/Fehlervertrag dokumentieren. (GitHub: #8)

## 2. Drones und StudentGroups

- [ ] 2.1 Drone-Anlegen im Backend bereitstellen (REQ-001; nach 1.3, 1.4). Prüfung: API-Test speichert Name und Description aus dem Spec-Beispiel unverändert und erhält eine Id; keine eingegebene Verfügbarkeit. Namen trimmen; fehlend/null/leer/101 Zeichen mit 400 und Feldfehler ohne Speicherung ablehnen; 1/100 Zeichen, Sonderzeichen und doppelte Namen mit verschiedenen Ids akzeptieren. Endpunkt dokumentieren. (GitHub: #9)
- [ ] 2.2 Drone-Liste mit abgeleiteter Verfügbarkeit bereitstellen (REQ-002; nach 2.1, 1.2). Prüfung: API-/SQLite-Test zeigt Name, Description, Available ohne aktive und Loaned mit aktiver Loan; Returned allein ergibt Available. Endpunkt dokumentieren. (GitHub: #10)
- [ ] 2.3 StudentGroup-Anlegen und Auswahlabfrage im Backend bereitstellen (REQ-003, REQ-004; nach 1.3, 1.4). Prüfung: Group 3 anlegen, erneut laden und zur Auswahl erhalten; Namen trimmen und dieselben Namensgrenzen/400-Fälle wie 2.1 testen; doppelte Namen mit verschiedenen Ids erlauben, keine Begrenzung auf eine Loan je StudentGroup. Auswahl liefert Id und Name. Endpunkte dokumentieren. (GitHub: #11)
- [ ] 2.4 Angular-Formular zum Anlegen einer Drone umsetzen (REQ-001; nach 2.1). Prüfung: HTTP-Komponententest überträgt Name/Description ohne Verfügbarkeitsfeld und zeigt Erfolg oder Backend-Feldfehler und prüft Pflichtfeld sowie Namensgrenzen. Bedienung dokumentieren. (GitHub: #12)
- [ ] 2.5 Angular-Drone-Liste umsetzen (REQ-002; nach 2.2, 2.4). Prüfung: Komponententest zeigt beide Verfügbarkeiten mit Name/Description und lädt nach erfolgreichem Anlegen neu. Bedienung dokumentieren. (GitHub: #13)
- [ ] 2.6 Angular-Formular zum Anlegen einer StudentGroup umsetzen (REQ-003; nach 2.3). Prüfung: HTTP-Komponententest legt Group 3 an und prüft Pflichtfeld/Namensgrenzen und zeigt Backend-Feldfehler nachvollziehbar. Bedienung dokumentieren. (GitHub: #14)

## 3. Neue Loans und Zubehör

- [ ] 3.2 Backend-Validierung der Loan-Eingaben bereitstellen (REQ-004, REQ-006; nach 1.3, 1.4). Prüfung: Validierungstests akzeptieren 0 und leere Positionsliste, normalisieren Nullpositionen weg und lehnen -1, 1,5, Type Cable sowie doppelte Battery-/Propeller-Typen auch mit 0 ab; keine Summierung. Fehlermeldungen und Normalisierung dokumentieren. API-Durchsetzung wird in 3.1 geprüft. (GitHub: #15)
- [ ] 3.1 Backend-Operation für eine Loan mit vollständiger Validierung und Transaktion bereitstellen (REQ-004, REQ-005, REQ-006; nach 1.2, 1.4, 2.2, 2.3, 3.2). Prüfung: API-/SQLite-Tests speichern genau eine Drone/StudentGroup, automatisches LoanDate, ReturnDate null, Active und 4 Battery/8 Propeller; dieselbe StudentGroup darf eine weitere Drone erhalten. Loaned liefert 409, unbekannte Referenzen 404; sämtliche ungültigen Mengen/Typen aus 3.2 liefern 400 samt Feldfehler ohne Loan oder Teilpositionen. Nullmengen und keine Positionen sind gültig und werden nicht als Nullposition gespeichert. API einschließlich Erfolgs-JSON und Atomarität dokumentieren. (GitHub: #16)
- [ ] 3.3 API-/SQLite-Konkurrenztest mit getrennten Verbindungen ergänzen und Konfliktbehandlung absichern (REQ-004, REQ-005; nach 3.1). Prüfung: zwei gleichzeitige Versuche für dieselbe Drone liefern ohne unabhängige technische Störung genau einen Erfolg, eine Antwort 409 samt Problem-JSON und genau eine Active Loan ohne Teilpositionen des Verlierers; sequentieller Zweitversuch wird abgelehnt. Testablauf dokumentieren. (GitHub: #17)
- [ ] 3.6 API-/SQLite-Rollback bei Speicherfehler prüfen (REQ-004, REQ-006; nach 3.1). Prüfung: Fehler nach Beginn der Loan-Speicherung provozieren, Datei neu öffnen und nachweisen, dass weder Loan noch Teilpositionen bestehen und die Drone Available bleibt; technische Fehler nicht als fachlichen 409 ausgeben. Testablauf dokumentieren. (GitHub: #18)
- [ ] 3.4 Angular-Loan-Formular mit Drone-/StudentGroup-Auswahl umsetzen (REQ-004, REQ-005; nach 3.1, 2.5, 2.6). Prüfung: HTTP-Komponententests übertragen genau eine Zuordnung je Loan, zeigen bei gleichnamigen Drones/StudentGroups zusätzlich die Id, übermitteln keine manuellen Datumsangaben, zeigen 400/404/409 verständlich an und laden nach Erfolg Verfügbarkeit neu. Bedienung dokumentieren. (GitHub: #19)
- [ ] 3.5 Zubehörmengen im Angular-Loan-Formular ergänzen (REQ-006; nach 3.4, 3.2). Prüfung: HTTP-Komponententests übertragen 4 Battery/8 Propeller sowie gültige Nullmengen ohne Geräte-IDs und prüfen genau ein Eingabefeld je Type, leere Zubehörliste, negative/nichtganzzahlige Eingaben und Backend-Feldfehler; keine Geräte-IDs und keine Nullpositionen voraussetzen. Bedienung dokumentieren. (GitHub: #20)

## 4. Aktive Loans

- [ ] 4.1 Backend-Abfrage aktiver Loans bereitstellen (REQ-007; nach 3.1). Prüfung: API-Test zeigt StudentGroup, Drone, beide Mengen und LoanDate für Active und schließt Returned aus; fehlende Zubehörtypen erscheinen mit 0 und Datum als Kalendertag. Endpunkt dokumentieren. (GitHub: #21)
- [ ] 4.2 Angular-Liste aktiver Loans umsetzen (REQ-007; nach 4.1, 3.5). Prüfung: Komponententest zeigt alle Spec-Angaben und aktualisiert die Liste nach einer neuen Loan. Bedienung dokumentieren. (GitHub: #22)

## 5. Rückgabe und Historie

- [ ] 5.1 Backend-Rückgabe mit dauerhafter Speicherung von ReturnDate bereitstellen (REQ-008, REQ-009; nach 4.1). Prüfung: API-/SQLite-Test ändert Active zu Returned, speichert ReturnDate und erhält LoanDate, Zuordnungen und LoanItems; Drone ist Available. Uhrgesteuerte API-Tests prüfen Europe/Vienna/Datumsformat, ReturnDate vor LoanDate mit 400, unbekannte Loan mit 404, erneute Rückgabe mit 409 (mit und ohne neue aktive Loan derselben Drone). Fehler lassen Status, Daten, LoanItems und neue Loan unverändert; Endpunkt dokumentieren. Übergreifende Fehler- und Konkurrenzprüfung folgt in 5.6. (GitHub: #23)
- [ ] 5.6 Atomarität und Konkurrenz der Rückgabe mit echter SQLite-Datei absichern (REQ-008, REQ-009; nach 5.1). Prüfung: zwei gleichzeitige Rückgaben über getrennte Verbindungen ergeben ohne unabhängige technische Störung genau einen Erfolg und einmal 409; ReturnDate wird nicht überschrieben. Speicherfehler nach Beginn der Rückgabe provozieren und nach erneutem Öffnen unveränderte Active Loan, ReturnDate null, LoanItems und Loaned Drone nachweisen. Technische Störungen nicht als fachlichen 409 behandeln. Testablauf dokumentieren. (GitHub: #24)
- [ ] 5.2 Backend-Historienabfrage bereitstellen (REQ-010; nach 5.1). Prüfung: API-Test zeigt Returned Loans mit Zuordnungen, beiden Daten und unveränderten Zubehörmengen nach erneutem Öffnen der SQLite-Datei; fehlende Typen als 0 anzeigen. Endpunkt dokumentieren. (GitHub: #25)
- [ ] 5.3 Angular-Rückgabeaktion umsetzen (REQ-008, REQ-009; nach 5.1, 4.2). Prüfung: HTTP-Komponententest löst Rückgabe aus, entfernt erfolgreiche Rückgabe aus aktiven Loans, lädt Drone-Verfügbarkeit neu und zeigt 400/404/409 verständlich, ohne bei Fehlern die Loan als erfolgreich zurückgegeben darzustellen. Keine manuelle Datumseingabe. Bedienung dokumentieren. (GitHub: #26)
- [ ] 5.4 Angular-Historie umsetzen (REQ-010; nach 5.2, 5.3). Prüfung: Komponententest zeigt dieselbe zurückgegebene Loan mit StudentGroup, Drone, Status, LoanDate, ReturnDate und Zubehörmengen; erneutes Laden erhält die Anzeige. Bedienung dokumentieren. (GitHub: #27)
- [ ] 5.5 SQLite-Integrationstest für Rückgabe und erneute Loan ergänzen (REQ-002, REQ-004, REQ-008, REQ-009, REQ-010; nach 5.2). Prüfung: Rückgabe, Verbindung schließen, erneut öffnen, neue Loan derselben Drone erstellen; alte Loan/LoanItems und ReturnDate bleiben unverändert, neue Loan ist Active. Testablauf dokumentieren. (GitHub: #28)

## 6. Gesamtablauf prüfen

- [ ] 6.1 Browser-/API-Gesamtablauf gegen temporäres SQLite prüfen (REQ-001 bis REQ-010; nach sämtlichen Tasks der Gruppen 1–5 einschließlich 3.3, 3.6 und 5.6). Prüfung: TELLO-07 und Group 3 anlegen, 4 Battery/8 Propeller ausleihen, zweiten Versuch ablehnen, aktive Liste prüfen, zurückgeben und nach erneutem Laden Historie sowie Verfügbarkeit prüfen; Ergebnis dokumentieren. (GitHub: #29)
- [ ] 6.2 Szenarien- und Testzuordnung abschließend prüfen (REQ-001 bis REQ-010; nach 6.1). Prüfung: jedes Spec-Szenario hat einen tatsächlich ausgeführten Test mit Ergebnis; Backend-/Frontend-Builds und Tests, OpenSpec-Validierung sowie git diff --check ausführen und konkrete Ergebnisse samt verbleibenden Grenzen dokumentieren. (GitHub: #30)

## Traceability

| Requirement | Spec | Tasks |
| --- | --- | --- |
| REQ-001 | drone | 1.1, 1.3, 1.4, 2.1, 2.4, 6.1, 6.2 |
| REQ-002 | drone | 2.2, 2.5, 5.5, 6.1, 6.2 |
| REQ-003 | student-group | 1.1, 1.3, 1.4, 2.3, 2.6, 6.1, 6.2 |
| REQ-004 | loan | 1.1, 1.3, 1.4, 3.2, 3.1, 3.3, 3.6, 3.4, 5.5, 6.1, 6.2 |
| REQ-005 | loan | 1.2, 3.1, 3.3, 3.4, 6.1, 6.2 |
| REQ-006 | loan | 1.1, 1.3, 3.2, 3.1, 3.6, 3.5, 6.1, 6.2 |
| REQ-007 | loan | 4.1, 4.2, 6.1, 6.2 |
| REQ-008 | loan | 1.4, 5.1, 5.3, 5.5, 5.6, 6.1, 6.2 |
| REQ-009 | loan | 1.1, 1.3, 1.4, 5.1, 5.3, 5.5, 5.6, 6.1, 6.2 |
| REQ-010 | loan | 1.1, 1.3, 5.2, 5.4, 5.5, 6.1, 6.2 |

| Domain Rule | Abdeckung |
| --- | --- |
| DR-01 | REQ-005; Tasks 1.2, 3.1, 3.3 |
| DR-02 | REQ-004/005; Tasks 2.2, 3.1, 3.4 |
| DR-03 | REQ-008; Tasks 5.1, 5.3, 5.5, 5.6 |
| DR-04 | REQ-006; Tasks 1.3, 3.2, 3.1, 3.5, 3.6 |
| DR-05 | REQ-008/009/010; Tasks 5.1, 5.2, 5.5, 5.6 |
| DR-06 | REQ-006; Tasks 3.1, 3.2, 3.5 |
| DR-07 | REQ-002/004/008; Tasks 2.2, 3.1, 5.1, 5.5 |

## Workflow follow-up

- Nach Umsetzung aller Tasks und erfolgreichem Review über den OpenSpec-Workflow
  abschließen, Hauptspecs synchronisieren und archivieren; dieser Planungsauftrag
  führt diese Schritte nicht aus.
