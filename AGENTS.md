# Arbeitsregeln für drone-inventory

Dieses SYP-Schulprojekt verwaltet Drohnen und deren Ausleihe. Grundlage sind die
Vorgaben des Auftraggebers und README.md. Ein separater SYP-Projektauftrag liegt
nicht im Repository vor; fehlende Details nicht als beschlossene Anforderungen ausgeben.

## Architektur

- `backend/DroneInventory.Api/`: ASP.NET Core 10 Web API mit C#.
- `frontend/drone-inventory-ui/`: Angular mit TypeScript.
- Kommunikation über REST/JSON.
- Entity Framework Core und SQLite sind für die Persistenz vorgesehen.
- Das Backend setzt Fachregeln durch; das Frontend stellt die Bedienung bereit.

Derzeit existieren nur Grundgerüste. Im Rahmen der OpenSpec-Einrichtung keine
Geschäftslogik oder Datenbank erstellen. Bestehende Projekte nur bei Bedarf ändern.

## Domänensprache und Regeln

Verwende ausschließlich diese vier Domänenbegriffe in Spezifikationen, Code und
Issues. Keine alternativen Namen oder zusätzlichen Domänenentitäten einführen.

| Begriff | Bedeutung |
| --- | --- |
| Drone | Eine verwaltete Drohne. |
| StudentGroup | Eine Gruppe, die Drohnen ausleiht. |
| Loan | Eine Ausleihe an eine StudentGroup, aktiv bis zur Rückgabe. |
| LoanItem | Zuordnung einer Drone zu einer Loan mit Zubehörmengen. |

Verbindliche Fachregeln:

- Eine Drone darf höchstens einer aktiven Loan zugeordnet sein.
- Nur verfügbare Drones dürfen ausgeliehen werden.
- Rückgaben beenden die aktive Loan und machen deren Drones wieder verfügbar.
- Zubehörmengen dürfen nicht negativ sein.
- Zurückgegebene Loans und ihre LoanItems bleiben gespeichert.
- Zubehör wird ausschließlich als Menge verwaltet, ohne eigene Identitäten.
- Verfügbarkeit ergibt sich aus aktiven Loans; keinen unabhängig gepflegten
  Verfügbarkeitszustand als fachliche Quelle einführen.

## OpenSpec vor der Implementierung

Vor jeder fachlichen Implementierung zuerst `openspec/config.yaml`, vorhandene
Spezifikationen und zugehörige Issues lesen. Den OpenSpec-Workflow verwenden:
Change mit Proposal, Spezifikationen, Design und Aufgaben ausarbeiten und prüfen,
dann implementieren. Keine fachliche Implementierung ohne zugehörige
OpenSpec-Artefakte beginnen. Nach Umsetzung passende Prüfungen ausführen und den
Change über den OpenSpec-Workflow abschließen und archivieren.

Spezifikationen beschreiben beobachtbares Verhalten, keine Klassen, Tabellen oder
Algorithmen. Technische Entscheidungen gehören ins Design. Jede Anforderung
verwendet SHALL/MUST und mindestens ein `#### Scenario:` mit expliziten
GIVEN/WHEN/THEN-Schritten (Ausgangslage, Aktion, Ergebnis). Artefakttexte sind
Deutsch; OpenSpec-Strukturüberschriften bleiben Englisch.

Codex verwendet die generierten Skills unter `.agents/skills/openspec-*/SKILL.md`.
OpenSpec-Befehle im Repository-Stamm ausführen. Nach einem frischen Checkout bei
Bedarf `openspec init --tools codex --language German --no-animation` ausführen.

## GitHub-Issue-Workflow

1. Vorhandenes Issue und Akzeptanzkriterien lesen; REQ-Kennungen aus dem
   Projektauftrag übernehmen. Fehlende Inhalte nicht erfinden.
2. Einen nachvollziehbaren Branch verwenden, etwa `feat/<issue-nummer>-<kurzname>`
   oder `docs/<kurzname>` für reine Dokumentation.
3. Vor fachlicher Umsetzung den OpenSpec-Change mit dem vorhandenen Issue
   verknüpfen; Issue, Change und spätere Pull Request gegenseitig referenzieren.
4. Vereinbarten Umfang umsetzen, passende Prüfungen ausführen und Ergebnisse in
   der Pull Request dokumentieren. Änderungen vor dem Commit prüfen.
5. Die Pull Request referenziert das Issue. `Closes #<nummer>` nur bei vollständig
   erfüllten Akzeptanzkriterien verwenden; Issues erst nach Abschluss schließen.

Keine Issues automatisch anlegen; neue Issues nur bei ausdrücklichem Auftrag.
Für die aktuelle Einrichtung weder Issues noch einen OpenSpec-Change anlegen
oder vorschlagen. REQ-001 bis REQ-010 bleiben unimplementiert.

## Out of Scope

Login, Benutzerrollen, einzelne Akku-IDs, Reservierungen, Schadensverwaltung,
Statistiken und Exporte gehören nicht zum Projektumfang.
