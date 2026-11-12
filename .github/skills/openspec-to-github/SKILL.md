---
name: openspec-to-github
description: Überträgt Tasks eines geprüften OpenSpec-Changes nach ausdrücklichem Auftrag in GitHub Issues und das Project Drone Inventory; gleicht vorhandene Issues ab.
---

# OpenSpec-Tasks mit GitHub verbinden

## Voraussetzungen

Arbeite im Repository-Stamm. Lies `AGENTS.md`, `openspec/config.yaml`,
`docs/requirements.md`, `docs/ddd.md` und den Projektauftrag unter `docs/source/`.
OpenSpec ist die Source of Truth. Übernimm REQ-Kennungen aus den Quellen;
erfinde keine Anforderungen, Tasks oder Changes. Beachte die vier Domänenbegriffe
Drone, StudentGroup, Loan und LoanItem sowie die verbindlichen Fachregeln.
Dieser Skill implementiert nichts und hakt keine Tasks ab.

Issue-Erstellung verlangt einen ausdrücklichen Nutzerauftrag. Die bloße
Installation oder automatische Auswahl dieses Skills erlaubt keine neuen Issues.
Ein ausdrücklicher Aufruf zum Übertragen der Tasks genügt als Auftrag.

## Ablauf

1. Prüfe `git status --short`, `gh auth status` und mit
   `gh repo view --json nameWithOwner,url` das Ziel `FeReiter/drone-inventory`.
   Prüfe das Project mit `gh project view 1 --owner FeReiter --format json`
   und `gh project field-list 1 --owner FeReiter --format json`.
   Erwartet werden „Drone Inventory“ und Backlog, Ready, In Progress, Review, Done.
   Bei fehlenden Rechten oder abweichendem Ziel stoppe vor Schreibzugriffen;
   keine Ersatz-Projects oder Statuswerte anlegen.
2. Bestimme den ausdrücklich genannten Change, sonst frage bei Mehrdeutigkeit.
   Nutze `openspec list --json`, `openspec status --change "<change>" --json`
   und `openspec instructions apply --change "<change>" --json`.
   Lies Proposal, Design, Delta-Specs, passende Hauptspecs und die Task-Dateien
   anhand der zurückgegebenen Pfade. Bei fehlender Wurzel, blockiertem Change
   oder fehlenden Artefakten nicht eigenmächtig initialisieren oder ergänzen.
   Prüfe `openspec validate "<change>" --strict --no-interactive`.
   Kläre fachliche Widersprüche vor der Übertragung. Für einen Store gelten
   dieselben aufgelösten Root-Pfade und `--store <id>` bei allen passenden Befehlen.
3. Ordne jedem einzeln testbaren Task genau ein Issue zu. Verwende als stabilen
   Schlüssel Repository, Change-Name und vorhandene Task-ID (z. B. `1.1`).
   Fehlt eine eindeutige ID, kläre sie vor der Erstellung; nicht still umnummerieren.
   Bewahre vorhandene Checkboxen und Task-Texte.
4. Prüfe zuerst eingetragene Issue-Nummern per `gh issue view`. Lies offene
   **und geschlossene** Issues, etwa mit
   `gh issue list --repo FeReiter/drone-inventory --state all --limit 100 --json number,title,body,state,url`.
   100 ist nur eine erste Seite: bei voller Ergebnismenge vollständig paginieren,
   z. B. mit `gh api --paginate 'repos/FeReiter/drone-inventory/issues?state=all&per_page=100'`;
   dabei API-Einträge mit `pull_request` ausschließen.
   Vergleiche Task-Schlüssel, Verweise, REQ-Kennungen und Inhalt, nicht nur Titel.
   Eindeutige Treffer wiederverwenden; bei mehreren Treffern oder widersprüchlich
   geschlossenem Issue klären, nicht neu anlegen oder automatisch wieder öffnen.
5. Nur für nachweislich fehlende Issues: deutschen Titel und Body erstellen.
   Der Body enthält `OpenSpec change: <change>`, `OpenSpec task: <id>`,
   den Pfad zur Task-Datei, Links auf Proposal und Spec-Anforderungen,
   belegte REQ-Kennungen, den einzelnen Task und prüfbare Akzeptanzkriterien
   aus den Szenarien. Keine zusätzlichen Fachentscheidungen treffen.
   Schreibe den Body in eine temporäre Datei und verwende
   `gh issue create --repo FeReiter/drone-inventory --title "<titel>" --body-file "<datei>"`.
   Trage die zurückgegebene Nummer unmittelbar am zugehörigen Task ein,
   z. B. `- [ ] 1.1 <unveränderter Tasktext> (GitHub: #<nummer>)`.
6. Prüfe die Project-Mitgliedschaft aller zugeordneten Issues vollständig,
   inklusive weiterer Ergebnisseiten. Fehlende Einträge ergänzen mit
   `gh project item-add 1 --owner FeReiter --url "<issue-url>" --format json`.
   Neue Einträge auf Backlog setzen:
   `gh project item-edit 1 --owner FeReiter --url "<issue-url>" --field Status --value Backlog`.
   Bestehende Statuswerte bewahren. IDs bei Bedarf aktuell auslesen,
   nicht dauerhaft hart codieren. Project-Updates erneut lesend verifizieren.
7. Bei Teilfehlern erhaltene Nummern sichern, erfolgreiche Schritte berichten
   und stoppen. Nach Timeout vor jeder Wiederholung auf GitHub nachsehen,
   ob die Erstellung bereits erfolgt ist. Beim nächsten Lauf abgleichen statt
   doppelt erstellen. Keine Issues löschen oder Tasks als erledigt markieren.
8. Prüfe `git diff` und `git diff --check`. Berichte Task → Issue → Project,
   wiederverwendete/neu erstellte Issues und offene Fehler. Veröffentliche die
   Task-Verknüpfungen auf einem eigenen Dokumentationsbranch mit Commit, Push
   und PR gegen `main`, soweit vom Auftrag umfasst; niemals automatisch mergen.

## Aufruf in Codex

`Lies .github/skills/openspec-to-github/SKILL.md und führe ihn für Change <change> aus. Erstelle die fehlenden Issues und übertrage sie ins Project.`

Die Datei ist eine Agent-Anleitung, kein ausführbarer CLI-Befehl.
Codex-Discovery und Kurzaufrufe sind in `docs/projektablauf.md`, Abschnitt 10, erklärt.
