# Proposal

## Why

Die Verwaltung auf einem Zettel bietet keine dauerhafte Nachvollziehbarkeit von
Drones, StudentGroups, Loans und Zubehörmengen. Vor der Implementierung werden
REQ-001 bis REQ-010 als prüfbares Verhalten festgehalten, damit Umsetzung und
Tests auf denselben vereinbarten Anforderungen beruhen.

## What Changes

Ziel ist eine kleine, vollständig nachvollziehbare Drohnenverwaltung:

- Drones anlegen und mit abgeleiteter Verfügbarkeit anzeigen (REQ-001, REQ-002).
- StudentGroups anlegen (REQ-003).
- Genau eine verfügbare Drone an genau eine StudentGroup ausleihen und doppelte
  aktive Loans verhindern (REQ-004, REQ-005).
- Battery und Propeller ausschließlich als ganzzahlige, nichtnegative Mengen in
  LoanItems erfassen (REQ-006).
- Aktive Loans samt Zuordnungen, Zubehörmengen und LoanDate anzeigen (REQ-007).
- Loans zurückgeben, ReturnDate speichern und die Drone wieder freigeben
  (REQ-008, REQ-009).
- Zurückgegebene Loans samt unveränderten LoanItems dauerhaft anzeigen (REQ-010).

Dieser Auftrag erstellt nur die Planung. Keine dieser Funktionen wird hier
implementiert; alle Umsetzungstasks bleiben offen.

## Capabilities

### New Capabilities

- `drone`: Anlegen und Anzeigen von Drones, einschließlich ausschließlich aus
  aktiven Loans abgeleiteter Verfügbarkeit (REQ-001, REQ-002).
- `student-group`: Anlegen von StudentGroups (REQ-003).
- `loan`: Erstellen, Prüfen, Anzeigen und Zurückgeben von Loans einschließlich
  LoanItems, Datum und dauerhafter Historie (REQ-004 bis REQ-010).

### Modified Capabilities

Keine; die Hauptspezifikationen enthalten bisher nur einen Platzhalter.

## Impact

Die spätere Umsetzung betrifft ASP.NET Core 10 im Backend, Angular/TypeScript
im Frontend, REST/JSON sowie EF Core und SQLite. Geschäftsregeln werden im
Backend durchgesetzt. Die technischen Entscheidungen stehen im Design.

Quellen: `docs/requirements.md`, `docs/ddd.md` und die committed Originalversion
von `docs/source/INF4_SYP_19_Projektauftrag_Drohnenverwaltung.pdf` aus Commit
`16bf8a5` (43 Seiten, SHA-256
`ae7a1582059b765d6fb3a1c399084846479cf85fe1f9e1ba114e790b745851b5`).
Die lokal veränderte PDF ist keine Quelle dieser Planung und bleibt unberührt.

Die fünf Detailentscheidungen aus dem Spec-Review wurden am 12.11.2026 vom
Benutzer bestätigt: Namensvalidierung, automatische Kalendertage, erneute
Rückgabe als Konflikt, HTTP-Fehlervertrag und normalisierte Zubehörpositionen.
Design und Specs halten diese Ergänzungen getrennt von den PDF-Vorgaben fest.

PR #3 ist laut GitHub seit 12.11.2026 um 13:40:24 UTC gemergt:
https://github.com/FeReiter/drone-inventory/pull/3 . Die drei Workflow-Skills wurden beim Abgleich des Planungsbranches mit
main unverändert übernommen. Issue-Zuordnungen sind noch nicht verifiziert und vor Implementierung
nachzuführen. Es werden keine Nummern erfunden und keine Issues angelegt.

## Out of Scope

Login, Benutzerkonten, Benutzerrollen, einzelne Akku-IDs oder andere
Zubehörgeräte-Identitäten, Reservierungen, Schadensverwaltung,
Rückgabedifferenzen, Auswertungen, Statistiken und Exporte sind ausgeschlossen.
Class, Members, ResponsibleStudent und ein eigenes SSID-Feld werden nicht
eingeführt. Description bleibt Freitext ohne automatische Ausleihsperre.
Spätere Change-Request-Beispiele aus dem PDF gehören nicht zu diesem Change.
