---
name: openspec-archive-with-github
description: Prüft Tasks, Testnachweise und gemergte Pull Requests eines OpenSpec-Changes und archiviert ihn erst bei vollständig erfüllten Abschlussbedingungen.
---

# Einen geprüften und gemergten Change archivieren

## Verbindliche Abschlussbedingungen

Arbeite im Repository-Stamm. Lies `AGENTS.md`, `openspec/config.yaml`,
`docs/requirements.md`, `docs/ddd.md` und den Projektauftrag unter `docs/source/`.
Wähle den genannten Change; bei Mehrdeutigkeit nachfragen. Erstelle keine Issues,
implementiere nichts, hake keine Tasks ab und merge keine PRs. Dieser Skill
erlaubt keine Archivierung mit offenen Tasks, fehlenden Tests oder ungemergten PRs.

1. Prüfe Repository `FeReiter/drone-inventory`, Anmeldung und sauberen Arbeitsbaum.
   Aktualisiere `origin/main`. Prüfe den Change auf diesem Stand und arbeite
   auf einem eigenen Archivierungsbranch, z. B. `docs/archive-<change>`.
   Änderungen eines alten Feature-Branches sind kein Beleg für Integration.
2. Nutze `openspec list --json`,
   `openspec status --change "<change>" --json` und
   `openspec instructions apply --change "<change>" --json`, um Root, Schema,
   Artefakte und sämtliche getrackten Task-Dateien zu bestimmen.
   Berücksichtige bei einem Store dessen aufgelöste Pfade und `--store <id>`.
   Lies Proposal, Design, Specs und Tasks; prüfe
   `openspec validate "<change>" --strict --no-interactive`.
   Alle erforderlichen Artefakte müssen fertig sein. Verlange gültige
   `totalTasks`/`completedTasks` aus dem exakt passenden Listeneintrag und
   vollständige Übereinstimmung mit den gelesenen Aufgaben. Fehlende Dateien,
   ungültige Zähler oder null Tasks liefern keinen Abschlussnachweis: stoppen.
3. Erstelle eine Nachweisliste für **jeden** Task: ID, Issue, Akzeptanzkriterien,
   Spec-Szenarien, PR(s), Merge-Commit und Testnachweis.
   Verlange alle Tasks geprüft abgeschlossen. Eine Checkbox allein genügt nicht.
   Lies verknüpfte Issues und ihre Diskussion; prüfe offene und geschlossene
   sowie weitere zugehörige PRs vollständig (Pagination berücksichtigen).
   Geschlossenes Issue oder Project-Status Done beweist keinen Merge.
4. Prüfe jede zugehörige Umsetzungs-PR mit
   `gh pr view <nummer> --repo FeReiter/drone-inventory --json url,state,mergedAt,mergeCommit,baseRefName,headRefOid,body,statusCheckRollup`.
   Verlange `state=MERGED`, gesetztes `mergedAt`, Zielbranch `main` und
   nachweisbar enthaltenen Merge-Commit auf dem aktualisierten `origin/main`,
   etwa `git merge-base --is-ancestor <merge-sha> origin/main`.
   Geschlossen ohne Merge, Draft, offen oder unbekannt: stoppen.
   Zusätzliche offene Umsetzungs-PRs zum Change müssen vor Abschluss geklärt sein.
5. Prüfe Testnachweise zum tatsächlich gemergten Code und betroffenen Szenarien.
   Werte vorhandene CI-Läufe/Checks aus, z. B. `gh pr checks <nummer>` und
   bei Bedarf `gh run view <lauf-id>`. Fehlende Checks sind kein Erfolg.
   Ausstehende oder fehlgeschlagene relevante Prüfungen blockieren den Abschluss.
   Führe die passenden Builds und Tests auf dem aktualisierten integrierten Stand
   erneut aus; nutze README und tatsächliche Testprojekte. Gibt es keine CI,
   dokumentiere ausdrücklich die lokalen Befehle, Ergebnisse, Testanzahlen und
   die geprüfte Commit-SHA. Ein API-Projekt ohne Tests liefert keinen
   Backend-Testnachweis. Bei Dokumentation genügen passende Dokumentprüfungen.
   Bei fehlenden Nachweisen oder nicht ausführbaren Prüfungen stoppen.

## Archivierung nach bestandener Prüfung

6. Berichte die vollständige Nachweisliste und fahre nur fort, wenn alle Gates
   bestanden sind. Nutze `.agents/skills/openspec-archive-change/SKILL.md`
   für die Artefakt- und Delta-Spec-Prüfung, einschließlich dessen Sync-Analyse
   und erforderlicher Auswahl. **Keine** dort mögliche Ausnahme zum Archivieren
   unvollständiger Tasks/Artefakte nutzen. Fehlende oder blockierte Spec-Synchronisierung
   hier nicht umgehen; Hauptspecs müssen nach Abschluss dem integrierten Verhalten
   entsprechen. Falls bereits synchronisiert, dies durch Inhaltsvergleich belegen.
7. Archiviere über `openspec archive "<change>"` mit regulärer Validierung und
   Spec-Aktualisierung. Kein `--no-validate`, kein ungeprüftes `--skip-specs`
   und kein manuelles Verschieben als Umgehung von CLI-Fehlern.
   Der CLI-Befehl ersetzt den manuellen Move-Schritt des generierten Skills;
   Sync nicht doppelt anwenden. Bei Fehlern aktuellen Zustand prüfen und stoppen.
8. Kontrolliere Archivpfad, Erhalt der Metadaten, Hauptspecs und Tasks.
   Führe `openspec validate --all --strict --no-interactive`,
   `openspec validate --archived --no-interactive` und `openspec list --json` aus.
   Lies `git diff`, `git diff --check` und staged Diff vor dem Commit.
   Veröffentliche die Archivierung als Commit/Push und eigene PR gegen `main`;
   verlinke Change, Issues, Umsetzungs-PRs und Prüfprotokoll. Nicht selbst mergen.
9. Verifiziere im Project „Drone Inventory“ (FeReiter, Nr. 1) den Abschluss
   der zugehörigen Issues; setze erst nach bestätigter vollständiger Umsetzung
   und Merge auf Done. Keine anderen Project-Einträge ändern. Berichte Archivpfad,
   Spec-Sync, Testnachweise und Archivierungs-PR. Bei jedem Blocker präzise
   fehlende Belege nennen und den Change aktiv lassen.

## Aufruf in Codex

`Lies .github/skills/openspec-archive-with-github/SKILL.md und prüfe Change <change>. Archiviere ihn nur, wenn alle Abschlussbedingungen erfüllt sind.`

Die Datei ist eine Agent-Anleitung. Discovery und Kurzaufrufe sind in
`docs/projektablauf.md`, Abschnitt 10, dokumentiert.
