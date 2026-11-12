---
name: openspec-apply-with-github
description: Bearbeitet genau ein GitHub Issue mit einem OpenSpec-Task auf eigenem Branch, prüft die Umsetzung und erstellt eine Pull Request ohne Merge.
---

# Genau ein Issue umsetzen

## Umfang und Vorbereitung

Arbeite im Repository-Stamm. Lies `AGENTS.md`, `openspec/config.yaml`,
`docs/requirements.md`, `docs/ddd.md` und den Projektauftrag unter `docs/source/`.
Beachte Domänensprache, Fachregeln und Out of Scope. OpenSpec bestimmt das
vereinbarte Verhalten; Technik steht im Design. Erfinde keine Anforderungen.

1. Verlange genau eine Issue-Nummer. Lies das Issue samt Akzeptanzkriterien
   und Diskussion mit `gh issue view <nummer> --repo FeReiter/drone-inventory --comments`.
   Bestimme den verknüpften Change und genau einen Task. Fehlt die eindeutige
   Zuordnung, enthält das Issue mehrere Tasks oder ist es bereits geschlossen,
   kläre den Umfang vor der Implementierung. Keine anderen Issues erstellen.
2. Prüfe `gh auth status`, Repository-Ziel, Project Nr. 1 von FeReiter
   („Drone Inventory“), Statusfeld und Rechte. Lies vollständig Proposal,
   Design, Delta-Specs, Hauptspecs und Tasks anhand von
   `openspec status --change "<change>" --json` und
   `openspec instructions apply --change "<change>" --json`.
   Nutze `openspec list --json` zur Root-Kontrolle; bei Store-Auswahl dessen
   Pfade und `--store <id>` konsistent verwenden. Bei blockiertem Zustand,
   fehlenden Artefakten oder widersprüchlichen Anforderungen stoppen.
   Prüfe `openspec validate "<change>" --strict --no-interactive`.
   Keine fachliche Implementierung ohne geprüfte OpenSpec-Artefakte.
3. Prüfe Arbeitsbaum und bestehende Branches/PRs für dieses Issue. Fremde
   Änderungen nicht übernehmen oder verwerfen. Aktualisiere `origin/main` und
   erstelle davon `feat/<nummer>-<kurzname>` (für Dokumentation `docs/<nummer>-<kurzname>`).
   Einen bestehenden eindeutig zugehörigen Branch/PR nach Prüfung fortsetzen;
   keine doppelte PR und kein Force-Push. Sind erforderliche Spec-Verknüpfungen
   noch nicht auf `main`, zuerst deren Integration abwarten.
4. Prüfe Project-Mitgliedschaft, ergänze das vorhandene Issue bei Bedarf mit
   `gh project item-add 1 --owner FeReiter --url "<issue-url>" --format json`.
   Setze vor der Umsetzung
   `gh project item-edit 1 --owner FeReiter --url "<issue-url>" --field Status --value "In Progress"`.
   Verifiziere den Status lesend. Bei Fehlern nicht als begonnen ausgeben und
   nicht weiter implementieren, bis die Project-Zuordnung funktioniert.

## Umsetzung und Nachweis

5. Nutze `.agents/skills/openspec-apply-change/SKILL.md` als OpenSpec-Verfahren,
   begrenze dessen Schleife ausdrücklich auf **diesen einen Task**.
   Dessen Aufforderung, mit weiteren Tasks fortzufahren, gilt hier nicht.
   Implementiere nur das spezifizierte Verhalten und notwendige Tests.
   Fehlen Vorarbeiten oder braucht es eine Verhaltensänderung: stoppen,
   Blocker bzw. erforderliche Spec-Anpassung erklären, keinen anderen Task anfangen.
6. Prüfe jedes betroffene GIVEN/WHEN/THEN-Szenario mit passenden Tests,
   einschließlich relevanter Fehlerfälle und Fachregeln. Nutze die aktuellen
   Projektbefehle aus README und Projektdateien. Derzeit sind vorgesehen:
   `dotnet build backend/DroneInventory.Api/DroneInventory.Api.csproj`,
   Tests eines tatsächlich vorhandenen Backend-Testprojekts und im Frontend
   `npm run build` sowie `npm test -- --watch=false`.
   `dotnet test` auf dem API-Projekt ohne Tests ist kein Testnachweis.
   Bei reiner Dokumentation passende Struktur-/Linkprüfungen verwenden.
   Erfasse Befehl, Ergebnis, Testanzahl, Szenario-Zuordnung und Einschränkungen.
   Fehler oder nicht ausführbare Pflichtprüfungen nicht als Erfolg darstellen.
7. Hake ausschließlich den zugehörigen Task ab, wenn sein vollständiges
   Verhalten und alle Akzeptanzkriterien nachweislich erfüllt und die passenden
   Prüfungen erfolgreich sind. Teilumsetzung oder bloßer CLI-Status genügt nicht.
   Die Checkbox bedeutet geprüft implementiert; sie beweist noch keinen Merge.
   Alle anderen Checkboxen unverändert lassen.
8. Lies vor jedem Commit `git diff`, `git diff --check` sowie den staged Diff
   (`git diff --cached`, `git diff --cached --check`). Prüfe Umfang und sensible
   Inhalte. Committe gezielt nur die zugehörigen Dateien nach erfolgreichen
   Prüfungen und pushe den Issue-Branch.
9. Erstelle eine PR mit explizitem `--base main` und `--head <branch>`;
   verwende `gh pr create --repo FeReiter/drone-inventory --base main --head "<branch>" --title "<titel>" --body-file "<datei>"`.
   Beschreibe Problem, Verhalten, Change, Task-ID, Spec-/REQ-Verweise und
   tatsächliche Prüfergebnisse. Verwende `Closes #<nummer>` nur bei vollständig
   erfüllten Kriterien, sonst `Refs #<nummer>` und benenne den Restumfang.
   Verknüpfe PR, Issue und Task gegenseitig, etwa durch einen Issue-Kommentar
   mit PR-Link und einen PR-Verweis am betreffenden Task.
   Jede nachträgliche Dateiänderung vor dem Commit erneut passend prüfen.
10. Setze nach erfolgreicher PR-Erstellung das Issue im Project auf Review
    und verifiziere es. Berichte PR-Link, Tests und eventuelle Blocker.
    Nicht mergen, kein Auto-Merge aktivieren, Issue nicht vorzeitig schließen,
    nicht auf Done setzen und Change nicht archivieren.

## Aufruf in Codex

`Lies .github/skills/openspec-apply-with-github/SKILL.md und bearbeite ausschließlich Issue #<nummer> nach diesem Skill.`

Die Datei ist eine Agent-Anleitung. Hinweise zu Discovery und Kurzaufrufen
stehen in `docs/projektablauf.md`, Abschnitt 10.
