# Spec Delta

## Purpose

StudentGroups mit einer nachvollziehbaren Identität und einem Namen als Grundlage für die Zuordnung von Loans erfassen.

## ADDED Requirements

### Requirement: REQ-003 – StudentGroup anlegen

Quelle: Seiten 11, 13–14.

Das System SHALL das Anlegen einer StudentGroup mit Id und Name ermöglichen.

#### Scenario: StudentGroup anlegen

- GIVEN eine StudentGroup „Group 3“ wurde noch nicht angelegt.
- WHEN eine StudentGroup mit Name „Group 3“ angelegt wird.
- THEN ist eine StudentGroup mit einer Id und Name „Group 3“ vorhanden.

Die am 12.11.2026 bestätigte Präzisierung ergänzt den PDF-Auftrag: Das Backend
SHALL Name als Pflichtfeld behandeln, äußere Leerzeichen entfernen und danach
1 bis 100 Zeichen zulassen. Gleiche Namen SHALL zulässig sein; unterschiedliche
StudentGroups SHALL anhand ihrer Id unterscheidbar bleiben. Die Auswahl SHALL bei
Namensgleichheit zusätzlich die Id anzeigen. Ungültige Namen SHALL HTTP 400
mit Problem-JSON (status, title, detail und bei Feldfehlern errors) liefern,
ohne eine StudentGroup anzulegen.

#### Scenario: Namen normalisieren und doppelte Namen zulassen

- GIVEN eine StudentGroup mit Name „Group 3“ existiert.
- WHEN eine weitere StudentGroup mit Name „  Group 3  “ angelegt wird.
- THEN besitzt sie Name „Group 3“ und eine andere Id.
- AND eine Auswahl gleichnamiger StudentGroups zeigt zusätzlich ihre Ids.

#### Scenario: Ungültige Namen ablehnen

- GIVEN eine Anfrage zum Anlegen einer StudentGroup.
- WHEN Name fehlt, null ist, nur aus Leerzeichen besteht oder nach Entfernen äußerer Leerzeichen 101 Zeichen enthält.
- THEN antwortet das Backend jeweils mit HTTP 400 und Problem-JSON samt Feldfehler.
- AND es wird keine StudentGroup gespeichert.

#### Scenario: Gültige Namensgrenzen akzeptieren

- GIVEN eine Anfrage zum Anlegen einer StudentGroup.
- WHEN Name nach Entfernen äußerer Leerzeichen 1 oder 100 Zeichen enthält, auch mit Umlauten oder Sonderzeichen.
- THEN wird die StudentGroup mit diesem Namen angelegt.
