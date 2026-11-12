# Projektablauf: Drone Inventory

Stand: 11. November 2026. Diese Dokumentation dient als Grundlage für die
SYP-Abgabe und Präsentation. Sie beschreibt die Vorbereitung und die vorhandenen
Grundgerüste, keine bereits fertige Drohnenverwaltung.

## Quellen und Grenzen der Nachweise

Ausgewertet wurden die Git-Historie, die README.md, die Projektdateien,
AGENTS.md, openspec/config.yaml, die lokale Werkzeugumgebung und lesende
GitHub-CLI-Abfragen. Die Commit-Zeiten unten sind in Europe/Vienna (UTC+1)
angegeben. Sie belegen den Zeitpunkt des Commits, nicht die genaue Arbeitsdauer
oder den Zeitpunkt eines Pushs.

- **Verifiziert:** durch Dateien, Git-Metadaten oder aktuelle Befehlsausgaben bestätigt.
- **In README dokumentiert:** frühere Durchführung beschrieben, aber ohne
  gespeicherte Rohprotokolle; Ergebnis bei dieser Recherche nicht erneut ausgeführt.
- **Nicht verifiziert:** Ablauf, Zeitpunkt oder Ergebnis nicht ausreichend belegt.

Die Reihenfolge von Installation, Anmeldung und Project-Erstellung lässt sich
nicht vollständig rekonstruieren. Die ersten drei Abschnitte ordnen diese
Vorbereitungen sachlich ein; nur belegte Zeitpunkte werden als solche genannt.
Ein separater SYP-Projektauftrag liegt nicht im Repository vor.

## 1. Entwicklungsumgebung vorbereiten

Für das Projekt werden eine Laufzeit für das Backend, Werkzeuge für das Frontend
und eine Versionsverwaltung benötigt. Die aktuelle Umgebung wurde mit folgenden
Befehlen geprüft; daraus folgt nicht, wann oder mit welchem Installationsbefehl
die Werkzeuge ursprünglich installiert wurden.

| Werkzeug | Zweck | Nachweis am Dokumentationsstand |
| --- | --- | --- |
| CachyOS | Linux-Arbeitsumgebung | `/etc/os-release`: CachyOS, Arch-basiert, Rolling Release |
| .NET | ASP.NET Core 10 Backend bauen und starten | SDK 10.0.114; lokal nur Microsoft.NETCore.App 10.0.14 registriert |
| Node.js und npm | Angular-Werkzeuge und Pakete ausführen | Node.js v26.13.0, npm 12.2.1 |
| Angular CLI | Frontend erzeugen, bauen und testen | Frontend-README nennt Generator 22.2.6; package.json enthält lokale CLI `^22.2.6` |
| Git | Änderungen lokal speichern und übertragen | Git 2.56.0 |
| GitHub CLI | GitHub-Anmeldung und Project-Abfragen | gh 2.102.1 |
| Codex | Unterstützung bei Repository-Arbeiten | codex-cli 0.159.2; konkrete frühere Installationsschritte nicht verifiziert |
| OpenSpec | Anforderungen vor der Implementierung planen | OpenSpec 1.16.0; Einrichtung später in Abschnitt 7 |

Wichtige Prüfbefehle:

```bash
cat /etc/os-release
dotnet --info
dotnet --list-runtimes
node --version
npm --version
git --version
gh --version
codex --version
openspec --version
```

Eine globale Angular CLI ist laut README nicht erforderlich. Das Frontend nutzt
die in `frontend/drone-inventory-ui/package.json` deklarierte CLI über npm.
Eine frühere globale Installation ist **nicht verifiziert**. Ebenso fehlen
Installationsprotokolle für CachyOS und die übrigen Werkzeuge; konkrete
Paketmanager- oder Downloadbefehle werden deshalb nicht nachträglich behauptet.

## 2. GitHub-Anmeldung und Project-Berechtigungen

GitHub speichert das entfernte Repository und die Aufgabenplanung. Die GitHub CLI
ermöglicht den Zugriff aus dem Terminal. Die erfolgreiche aktuelle Prüfung mit
`gh auth status` bestätigt die Anmeldung als **FeReiter**, Speicherung im Keyring
und HTTPS als Git-Protokoll. Der Token besitzt unter anderem die Bereiche
`repo`, `project` und `read:org`; zusätzlich wurden `gist` und `workflow` angezeigt.
Tokenwerte werden nicht dokumentiert.

Der Bereich `project` erlaubt dem Werkzeug den entsprechenden Project-Zugriff;
zusätzlich gelten die Rechte des angemeldeten Kontos. Das Project gehört
FeReiter. Das Lesen seiner Felder und Einträge war erfolgreich. Änderungen an
Berechtigungen wurden für diese Recherche nicht vorgenommen oder getestet.

Typische Befehle zur Einrichtung wären `gh auth login` und
`gh auth refresh -s project`. **Nicht verifiziert:** ob und wann diese Befehle
ursprünglich ausgeführt wurden, welches Anmeldeverfahren genutzt wurde und ob
Project-Rechte nachträglich ergänzt werden mussten.

Bei der Recherche schlug die Anmeldungskontrolle zunächst in der Sandbox fehl.
Mit freigegebenem Netzwerkzugriff war dieselbe Anmeldung gültig. Es wurde keine
Neuanmeldung vorgenommen; die erste Meldung belegt keinen tatsächlich ungültigen Token.

## 3. Öffentliches Repository und GitHub Project

Das öffentliche Repository [FeReiter/drone-inventory](https://github.com/FeReiter/drone-inventory)
wurde laut GitHub am **11.11.2026 um 07:55:18 Uhr** erstellt. Es dient als
zentraler Ablageort für Quellcode, Dokumentation und nachvollziehbare Änderungen.
Der genaue Erstellungsbefehl und der Zeitpunkt der Sichtbarkeitseinstellung sind
**nicht verifiziert**.

Das [GitHub Project „Drone Inventory“](https://github.com/users/FeReiter/projects/1)
ist als Project Nr. 1 unter FeReiter vorhanden, geöffnet und **privat**.
Die Öffentlichkeit des Repositorys gilt also nicht automatisch auch für das
Project. Sein genaues Erstellungsdatum wurde nicht ermittelt. Aktuell enthält es
keine Einträge. Die Repository-Abfrage liefert keine verknüpften Projects;
eine ausdrückliche Verknüpfung mit dem Repository ist damit nicht nachgewiesen.

Das Statusfeld enthält nachweislich diese fünf Werte. Die Bedeutungen erklären
den vorgesehenen Arbeitsablauf, nicht bereits durchlaufene Aufgaben:

| Status | Bedeutung für die Arbeit |
| --- | --- |
| Backlog | Gesammelte, noch nicht vorbereitete Aufgaben |
| Ready | Ausreichend geklärt und bereit zur Bearbeitung |
| In Progress | Aufgabe wird bearbeitet |
| Review | Ergebnis wartet auf Prüfung |
| Done | Aufgabe ist abgeschlossen |

Für die Recherche verwendete lesende Befehle:

```bash
gh auth status
gh repo view FeReiter/drone-inventory --json nameWithOwner,url,visibility,createdAt,projectsV2
gh project list --owner FeReiter --format json
gh project field-list 1 --owner FeReiter --format json
gh project item-list 1 --owner FeReiter --format json
```

## 4. README erstellen und veröffentlichen – fcf27aa

Am **11.11.2026 um 07:57:09 Uhr** entstand der Commit
[`fcf27aa`](https://github.com/FeReiter/drone-inventory/commit/fcf27aab369f8850f522334fd342a34cb90bd428)
mit der Nachricht `Add project README`.

Er enthielt ausschließlich die vierzeilige `README.md`: Titel, Schulprojekt zur
Verwaltung von Drohnen und Hinweis auf die Projektbeschreibung als Ausgangspunkt.
Damit hatte das Repository zunächst eine verständliche Einstiegsseite. Backend
und Frontend waren in diesem Commit noch nicht enthalten.

Der Inhalt ist mit `git show fcf27aa:README.md` überprüfbar. Dass der Commit nach
GitHub übertragen wurde, ist durch die GitHub-Commit-Abfrage belegt. Der genaue
Push-Zeitpunkt und der damalige Push-Befehl sind **nicht verifiziert**.

## 5. Backend und Frontend anlegen – 2f9794f

Am **11.11.2026 um 08:03:52 Uhr** folgte der Commit
[`2f9794f`](https://github.com/FeReiter/drone-inventory/commit/2f9794f2e0d4b171c5a58f4ea90d588e42f6b11c)
mit der Nachricht `chore: scaffold ASP.NET Core API and Angular frontend`.
Er ergänzte zwei getrennte Grundgerüste, die `.gitignore` und eine ausführliche
README mit Start-, Build- und Testanleitung. Der Commit ist auf GitHub im Branch
`main` nachgewiesen; der genaue ursprüngliche Push-Zeitpunkt ist nicht verifiziert.

### Backend

`backend/DroneInventory.Api/DroneInventory.Api.csproj` verwendet
`Microsoft.NET.Sdk.Web`, das Ziel `net10.0` und das Paket
`Microsoft.AspNetCore.OpenApi` in Version 10.0.14.
`Program.cs` startet die API, aktiviert OpenAPI im Entwicklungsmodus und richtet
HTTPS-Weiterleitung ein. Das Wetterbeispiel wurde laut README entfernt; es gibt
keine fachlichen Endpunkte. Das Grundgerüst schafft den späteren Einstiegspunkt
für die Geschäftsregeln und die REST/JSON-Kommunikation.

`Properties/launchSettings.json` legt die Startprofile fest. Laut README sind
HTTP-Port 5250 und HTTPS-Port 7293 vorgesehen. Der genaue ursprüngliche
`dotnet new`-Aufruf ist **nicht verifiziert**.

### Frontend

`frontend/drone-inventory-ui/` enthält Angular 22 mit TypeScript, CSS und Routing.
Die Frontend-README nennt Angular CLI 22.2.6 als Generator. Die npm-Skripte stehen
in `package.json`; `package-lock.json` hält die Paketauflösung fest.
`src/main.ts` startet die Anwendung. `src/app/app.html` enthält die
Angular-Begrüßungsseite, `app.routes.ts` noch keine fachlichen Routen.
Der genaue ursprüngliche `ng new`-Aufruf ist **nicht verifiziert**.

Die Trennung ermöglicht, API und Oberfläche unabhängig zu starten und später
über REST/JSON zu verbinden. Entity Framework Core und SQLite sind vorgesehen,
aber noch nicht eingerichtet. Es gibt keine fachlichen Models, Datenbank oder
Geschäftslogik. Die vereinbarten Domänenbegriffe sind Drone, StudentGroup, Loan
und LoanItem.

Reproduzierbare Startbefehle aus der README, jeweils in getrennten Terminals:

```bash
# Backend, ausgehend vom Repository-Stamm
cd backend/DroneInventory.Api
dotnet restore
dotnet run --launch-profile http
```

```bash
# Frontend, ausgehend vom Repository-Stamm
cd frontend/drone-inventory-ui
npm ci
npm start
```

Die `.gitignore` verhindert unter anderem, dass Build-Ausgaben, `node_modules`,
Datenbanken und persönliche Einstellungen versehentlich versioniert werden.

## 6. Builds, Tests und Probleme der Erstellungsumgebung

Die folgende Tabelle gibt die **in README.md dokumentierten früheren Ergebnisse**
wieder. Es liegen keine versionierten Build- oder Testprotokolle vor. Bei dieser
Dokumentationsarbeit wurden die Anwendungs-Builds, Tests und HTTP-Prüfungen nicht
erneut ausgeführt; ihre früheren Ergebnisse sind deshalb **nicht unabhängig
verifiziert**. Die vorhandenen Konfigurationen und Testdefinitionen wurden gelesen.

| Prüfung | Befehl bzw. Ziel | In README dokumentiertes Ergebnis |
| --- | --- | --- |
| Backend-Build | `dotnet build backend/DroneInventory.Api/DroneInventory.Api.csproj -p:AllowMissingPrunePackageData=true` | Erfolgreich, ohne Warnungen oder Fehler |
| Backend starten/OpenAPI lesen | HTTP-Startprofil; `http://localhost:5250/openapi/v1.json` | Start und Dokumentabruf geprüft; kein genauer Statuscode dokumentiert |
| Frontend-Produktionsbuild | Im Frontend: `npm run build` | Erfolgreich |
| Frontend-Komponententests | Im Frontend: `npm test -- --watch=false` | Beide Tests erfolgreich |
| Frontend-Entwicklungsserver | `npm start`; `http://localhost:4200/` | HTTP 200 |

Die beiden Tests in `frontend/drone-inventory-ui/src/app/app.spec.ts` prüfen,
ob die Wurzelkomponente erzeugt wird und die Begrüßungsüberschrift erscheint.
Sie prüfen noch keine Ausleihregeln. Ein Backend-Testprojekt ist nicht vorhanden.
Der README-Befehl `dotnet test backend/DroneInventory.Api/DroneInventory.Api.csproj`
ist daher kein Nachweis bestandener Backend-Tests. Es gibt auch keine belegten
Ende-zu-Ende- oder Lint-Ergebnisse.

### Unvollständige .NET-Installation und vorübergehende Lösung

Die README beschreibt zwei Probleme der lokalen SDK-Installation 10.0.114:

1. Fehlende ASP.NET-Core-Runtime: Ohne sie kann die Web API nicht regulär starten.
   Aktuell bestätigt `dotnet --list-runtimes` nur `Microsoft.NETCore.App 10.0.14`;
   `Microsoft.AspNetCore.App` fehlt weiterhin in dieser Installation.
2. Unvollständige Prune-Metadaten: Laut README wurde für den Build vorübergehend
   `-p:AllowMissingPrunePackageData=true` verwendet. Diese Option erlaubt den
   Build trotz fehlender Metadaten zur Paketbereinigung. Die frühere konkrete
   Fehlermeldung und eine erneute Reproduktion sind **nicht verifiziert**.

Für die damalige Startprüfung wurde laut README eine ASP.NET-Core-10.0.14-Runtime
unter `/tmp` bereitgestellt. Der genaue Unterordner, Downloadbefehl und die
Laufzeitaktivierung sind **nicht verifiziert**. Die temporäre Runtime und die
Build-Option wurden nicht in die Projektkonfiguration übernommen.
Die Prüfung mit Hilfsmitteln ist somit kein Nachweis einer vollständig reparierten
systemweiten .NET-Installation. Laut README gelten die normalen Start- und
Build-Befehle bei einer vollständigen .NET-10-Installation.

## 7. OpenSpec und verbindliche Arbeitsregeln – 3074b01

Am **11.11.2026 um 08:12:31 Uhr** entstand der Commit
[`3074b01`](https://github.com/FeReiter/drone-inventory/commit/3074b019f48b25f409878aac172a65d60ff06fca)
mit der Nachricht `chore: configure OpenSpec with Codex and SYP project rules`.
Er ist auf dem entfernten Branch `main` nachgewiesen.

OpenSpec wurde mit der Codex-Integration eingerichtet:

```bash
openspec init --tools codex --language German --no-animation
```

Im vorherigen Einrichtungslauf scheiterte die erste Ausführung am geschützten
Verzeichnis `.agents/skills`; nach freigegebenem Schreibzugriff war die Einrichtung
erfolgreich. Diese Beobachtung stammt aus dem vorhandenen Sitzungsverlauf.
Die `.gitignore` wurde gezielt angepasst, damit die OpenSpec-Skills versioniert
werden können und andere persönliche Agent-Einstellungen ausgeschlossen bleiben.

Die Dateien dienen folgenden Zwecken:

- `openspec/config.yaml`: Schema `spec-driven`, Technologie-Kontext, Fachregeln,
  Domänensprache und Vorgaben für verhaltensorientierte Given/When/Then-Szenarien.
- `AGENTS.md`: Architektur, Regeln und GitHub-Issue-Workflow; OpenSpec ist vor
  fachlichen Implementierungen verpflichtend.
- `.agents/skills/openspec-*/SKILL.md`: sechs Codex-Anleitungen für den
  OpenSpec-Workflow; `.openspec-target` kennzeichnet die Codex-Integration.
- `openspec/specs/.gitkeep` und `openspec/changes/archive/.gitkeep`: Platzhalter,
  damit die noch leeren Ordner in Git erhalten bleiben.

Diese Vorbereitung soll Anforderungen und Verhalten vor der Umsetzung klären.
Fachliche Changes und Spezifikationen wurden noch nicht angelegt; REQ-001 bis
REQ-010 wurden nicht implementiert. Login, Benutzerrollen, einzelne Akku-IDs,
Reservierungen, Schadensverwaltung, Statistiken und Exporte sind ausgeschlossen.

Bei der Dokumentationsrecherche erneut ausgeführte OpenSpec-Prüfungen:

```bash
openspec doctor
openspec schema validate spec-driven
openspec list
openspec list --specs
openspec validate --all --strict --no-interactive
```

**Verifizierte Ergebnisse:** Projektwurzel erkannt, Schema gültig, keine aktiven
Changes und keine Spezifikationen. Die Validierung meldet deshalb lediglich
`No items found to validate.`; das ist kein Test fachlicher Anforderungen.

## 8. Stand für die Abgabe und weitere Nachvollziehbarkeit

Vor Erstellung dieser Dokumentation waren die drei genannten Commits auf
`origin/main` vorhanden. GitHub nennt allerdings **master** als Standardbranch;
die späteren Grundgerüste und OpenSpec liegen auf **main**. Für die Bewertung daher
gezielt `main` öffnen. Die Standardbranch-Einstellung wurde nicht verändert.

Die damalige Dokumentationsarbeit ergänzte ausschließlich `docs/projektablauf.md`;
Anwendungscode blieb unverändert. Es wurden keine GitHub Issues oder Project-Einträge angelegt.

Wichtige Befehle zur Nachprüfung:

```bash
git log --all --format=fuller --stat
git show fcf27aa:README.md
git show --stat 2f9794f
git show --stat 3074b01
git ls-remote origin refs/heads/main
gh api 'repos/FeReiter/drone-inventory/commits?sha=main&per_page=10'
git diff --check
```

Für die Präsentation lässt sich der Ablauf so erklären: Zuerst wurden die
Arbeitsgrundlagen und die GitHub-Ablage vorbereitet. Danach folgten eine kurze
Projektbeschreibung und getrennte Grundgerüste für API und Oberfläche. Die README
hält erste technische Prüfungen und die nötigen .NET-Hilfslösungen fest. Anschließend
wurden mit OpenSpec die Regeln für die spätere fachliche Planung eingerichtet.
Die eigentliche Drohnenverwaltung muss auf dieser Grundlage noch entwickelt werden.


## 9. OpenSpec-Einrichtung prüfen und Arbeitsregeln vervollständigen

Bei der erneuten Einrichtung am 11. November 2026 waren `openspec/config.yaml`,
die leeren Verzeichnisse für Spezifikationen und Changes sowie alle sechs
Codex-Skills bereits vorhanden. Deshalb war kein erneutes `openspec init` nötig.
Die bestehende Integration wurde beibehalten, statt generierte Dateien unnötig
neu zu schreiben.

`openspec/config.yaml` verwendet weiterhin `schema: spec-driven`. Der Kontext
nennt nun ausdrücklich Angular mit TypeScript neben ASP.NET Core 10 Web API,
Entity Framework Core, SQLite und REST/JSON. Die vier Domänenbegriffe, alle sieben
Fachregeln und die ausgeschlossenen Funktionen bleiben verbindlich dokumentiert.
Entity Framework Core und SQLite sind weiterhin nur vorgesehen.

`AGENTS.md` und die Konfiguration erklären OpenSpec ausdrücklich zur Source of
Truth für vereinbartes fachliches Verhalten. Geschäftslogik gehört ins Backend;
das Frontend nutzt ausschließlich die REST/JSON-API. Anforderungen erhalten
GIVEN/WHEN/THEN-Szenarien. Je Implementierung wird ein abgegrenzter Task mit
eigenem Branch und eigener Pull Request bearbeitet. GitHub Issues und das
vorhandene Project dienen zur Planung. Vor einem Commit werden passende Tests,
`git diff` und `git diff --check` geprüft. Für diese reine Einrichtung sind
Konfigurationsprüfungen passend; Anwendungs-Builds und Anwendungstests wurden
nicht erneut ausgeführt.

Die Einrichtung wird auf `docs/openspec-arbeitsregeln` versioniert. Die Prüfungen
`openspec doctor` und `openspec schema validate spec-driven` waren erfolgreich.
`openspec list` und `openspec list --specs` bestätigten, dass keine aktiven Changes
und keine Spezifikationen vorhanden sind. `openspec validate --all --strict
--no-interactive` meldete erwartungsgemäß `No items found to validate.`; dies
validiert keine Fachanforderungen. Die YAML-Struktur und die Codex-Skilldateien
wurden zusätzlich geprüft; `git diff --check` meldete keine Formatfehler.

Ein separater SYP-Projektauftrag ist weiterhin nicht im Repository vorhanden.
Deshalb werden keine zusätzlichen Anforderungen oder REQ-Inhalte angenommen.
REQ-001 bis REQ-010 bleiben unimplementiert. Es wurden keine Changes, Issues oder
Project-Einträge angelegt und keine Anwendungscode-Dateien verändert.


## 10. Requirements und DDD aus dem Originalauftrag dokumentieren

Am 11. November 2026 wurde der vollständige Projektauftrag unter
`docs/source/INF4_SYP_19_Projektauftrag_Drohnenverwaltung.pdf` bereitgestellt.
Die früheren Hinweise auf eine fehlende Quelle beschreiben den damaligen Stand;
ab diesem Schritt ist das PDF die verbindliche Quelle. Alle 43 PDF-Seiten wurden
mit `pdftotext -layout` vollständig ausgelesen und gelesen. `pdfinfo` bestätigt
43 Seiten und 305862 Bytes. Die Originaldatei wird unverändert mitversioniert.
SHA-256:
`ae7a1582059b765d6fb3a1c399084846479cf85fe1f9e1ba114e790b745851b5`.

Auf dem Branch `docs/requirements-ddd` entstanden:

- `docs/requirements.md`: alle zehn REQ-Kennungen aus Seite 13, ausgearbeitet mit SHALL und testbaren GIVEN/WHEN/THEN-Szenarien gemäß Seiten 14–18; sieben Fachregeln, Out of Scope und ausdrücklich offene Fragen.
- `docs/ddd.md`: die vier Entities, sämtliche auf Seite 11 vorgegebenen Attribute, Beziehungen, ein Mermaid-Domänenmodell und die fachliche Einordnung von Identität, Mengenwerten und Aggregat.

Die Quelle klärt einen Widerspruch zur bisherigen Vorbereitung: Loan enthält
DroneId und StudentGroupId. LoanItem ist eine Zubehörposition mit Type und
Quantity, keine Zuordnung zwischen Drone und Loan. Deshalb wurden AGENTS.md und
openspec/config.yaml gezielt korrigiert und um den Quellenverweis ergänzt.
Benutzerkonten, Rückgabedifferenzen und Auswertungen wurden gemäß Seite 19 als
weitere ausdrücklich ausgeschlossene Funktionen benannt.

Verfügbarkeit wird aus aktiven Loans abgeleitet. Loan.Status kennt Active und
Returned; Drone zeigt Available oder Loaned. Zubehör hat keine Geräteidentität;
die vorgegebene LoanItem.Id identifiziert eine Position. Loan mit ihren LoanItems
wird begründet als fachliches Aggregat beschrieben. Historische Loans und
LoanItems bleiben erhalten. Kein zusätzliches Statusfeld oder Zubehörmodell
wurde als Implementierung eingeführt.

Unklarheiten werden nicht stillschweigend entschieden: unter anderem ID-Typen,
Namensvalidierung, Datumsermittlung, wiederholte Rückgabe und die Speicherung
von Nullmengen. Der auf Seite 43 erwähnte zusätzliche Leitfaden liegt nicht vor.
Die auf Seiten 37–38 diskutierten künftigen Change Requests sind kein aktueller
Auftrag. Eine Schadensnotiz im Freitext löst keine Ausleihsperre aus.

Prüfnachweise: Die REQ-Zuordnung wurde gegen Seite 13 und jede Ausarbeitung gegen
Seiten 14–18 abgeglichen; Attribute gegen Seite 11, Fachregeln gegen Seite 12,
Ausschlüsse gegen Seite 19. Das Beispiel auf Seite 18 dient als Gegenprobe für
Loan und die zwei LoanItems. Strukturprüfungen kontrollieren alle zehn eindeutigen
REQ-Abschnitte mit SHALL und GIVEN/WHEN/THEN, die Attributlisten, lokalen Links,
YAML-Struktur und PDF-Prüfsumme. Die Mermaid-Beziehungen wurden fachlich gegen
die beschriebenen Kardinalitäten geprüft. Ein grafischer Mermaid-Render ist
kein Bestandteil dieser Prüfung. Der vollständige Diff
und `git diff --check` wurden vor dem Commit geprüft. Anwendungs-Builds und Anwendungstests sind für
diese reine Dokumentationsänderung nicht erforderlich und wurden nicht ausgeführt.

Es wurden keine OpenSpec-Changes, Issues oder Anwendungscode erstellt.
Die Requirements sind noch nicht implementiert. Commit und Push veröffentlichen
die Dokumentation einschließlich Original-PDF; eine Pull Request dient dem
Review und wird nicht automatisch gemergt. Der Branch baut auf der vorherigen,
noch offenen Einrichtungs-PR #1 auf; diese Abhängigkeit wird in der PR benannt.


## 11. Drei GitHub-Workflow-Skills erstellen

Am 11.11.2026 wurden die drei Anleitungen unter `.github/skills/` ergänzt.
Grundlage sind der Projektauftrag unter `docs/source/` (insbesondere PDF-Seiten
28 und 32–36 sowie die Spielregeln auf Seite 39), `docs/requirements.md`,
`docs/ddd.md`, `AGENTS.md` und `openspec/config.yaml`. Die Einrichtung selbst
erstellt keine Issues oder OpenSpec-Changes und implementiert keine REQ.

Der neue Branch `docs-github-workflow-skills` basiert auf dem zu Beginn aktuell
abgerufenen `origin/main`, Commit `16bf8a5a40da1bf6ad41ece3eb398298da280760`.
Backend, Frontend und bestehende OpenSpec-Integration bleiben unverändert.
Der ursprünglich vorgesehene Branchname unter `docs/` war durch fremdbesessene
lokale Git-Verzeichnisse gesperrt; deshalb wurde ein eindeutiger Name ohne
diesen Verzeichnisprefix gewählt. Der Abruf erfolgte mit
`git fetch --no-write-fetch-head origin main`, da `.git/FETCH_HEAD` ebenfalls
einem anderen lokalen Benutzer gehört. Bestehende Dateirechte wurden nicht geändert.
Auch der Commit scheiterte an fremdbesessenen Verzeichnissen in `.git/objects`.
Die Rechtekorrektur war ohne lokale sudo-Authentifizierung nicht möglich.
Commit und Veröffentlichung erfolgen deshalb aus einem isolierten Checkout unter
`/tmp` mit denselben vier geprüften Dateien und derselben main-Basis.

### Entscheidungen und Verhalten

| Skill | Aufgabe | Wesentliche Grenze |
| --- | --- | --- |
| `openspec-to-github` | Pro geprüftem Task ein Issue zuordnen, ins Project aufnehmen und Nummer in der Task-Datei sichern | Erstellung nur auf ausdrücklichen Auftrag; offene und geschlossene Issues vollständig auf Duplikate prüfen |
| `openspec-apply-with-github` | Genau ein Issue auf eigenem Branch umsetzen, Tests und Diff prüfen, PR gegen main erstellen | Keine weiteren Tasks, kein Merge, kein vorzeitiges Done |
| `openspec-archive-with-github` | Tasks, Testnachweise, gemergte PRs und integrierte Commits prüfen, dann Specs aktualisieren und archivieren | Fehlende Belege oder offene Arbeit blockieren die Archivierung |

Die Zuordnung verwendet Change-Name und Task-ID; eine REQ allein ist kein
Task-Schlüssel. Wiederholungen verwenden vorhandene Issues und Project-Einträge.
Bei Teilfehlern werden bereits erzeugte Nummern gesichert und vor einem erneuten
Versuch abgeglichen. Bestehende Checkboxen bleiben beim Übertragen unverändert.
Der Apply-Skill hakt nur den vollständig umgesetzten und erfolgreich geprüften
Task ab. Der Archive-Skill prüft zusätzlich den Merge; eine Checkbox oder ein
geschlossenes Issue genügt nicht. Tests ohne tatsächlich ausgeführte Testfälle
gelten nicht als Nachweis. Project-Statuswerte werden aktuell ausgelesen.

Die generierte Apply-Anleitung wird auf einen Task begrenzt. Beim Archivieren
gelten strengere Abschlussbedingungen als die generierte Anleitung erlaubt:
keine Ausnahme für offene Tasks, keine Umgehung der Validierung. Die Archivierung
verwendet den installierten CLI-Befehl mit Spec-Aktualisierung anstelle eines
manuellen Verschiebens. Die Archivierungsänderung erhält eine eigene PR;
auch diese wird nicht automatisch gemergt.

### Kompatibilität und Aufruf

Geprüfte lokale Versionen: **codex-cli 0.159.2**, **OpenSpec 1.16.0** und
**gh 2.102.1**. Die Skills haben YAML-Frontmatter mit `name` und `description`.
Es sind Anleitungen für den Agenten, keine GitHub Actions oder OpenSpec-Befehle.

Die [offizielle Codex-Dokumentation](https://learn.chatgpt.com/docs/build-skills)
beschreibt `.agents/skills/` als Repository-Suchpfad und `$skill-name` bzw.
`/skills` als Codex-Aufruf. Die im Projektauftrag gezeigten Slash-Aufrufe sind
deshalb nicht unverändert als Codex-Kommandos vorauszusetzen. Die verlangte
Ablage `.github/skills/` wurde beibehalten. Die lokale Diagnose
`codex debug prompt-input` bestätigt: Die drei neuen Skills erscheinen dort
nicht im automatisch geladenen Skill-Katalog, `openspec-apply-change` dagegen
schon. Der Diagnoseaufruf erforderte außerhalb der Sandbox Zugriff auf die
lokale Codex-Laufzeit; es wurde kein Modelllauf zur Umsetzung gestartet.

**Sofort nutzbar im Codex-Chat**, aus dem Repository-Stamm:

```text
Lies .github/skills/openspec-to-github/SKILL.md und führe ihn für Change <change> aus. Erstelle die fehlenden Issues und übertrage sie ins Project.

Lies .github/skills/openspec-apply-with-github/SKILL.md und bearbeite ausschließlich Issue #<nummer> nach diesem Skill.

Lies .github/skills/openspec-archive-with-github/SKILL.md und prüfe Change <change>. Archiviere ihn nur, wenn alle Abschlussbedingungen erfüllt sind.
```

`<change>` und `<nummer>` werden später durch tatsächliche Werte ersetzt.
Diese Beispiele wurden bei der Einrichtung nicht ausgeführt. Wer Kurzaufrufe
nutzen möchte, kann die drei Skill-Ordner zusätzlich unter `.agents/skills/`
verlinken (relative Ziele: `../../.github/skills/<skill-name>`), danach die
Erkennung in `/skills` prüfen und gegebenenfalls Codex neu starten. Erst bei
bestätigter Erkennung sind `$openspec-to-github <change>`,
`$openspec-apply-with-github` mit Issue-Nummer im Prompt und
`$openspec-archive-with-github <change>` sinnvoll. Solche Links wurden hier
nicht angelegt; es gibt keine zweite zu pflegende Kopie der Anleitungen.

### Prüfergebnisse und Grenzen

- Alle drei Skills bestehen `quick_validate.py` aus dem installierten
  `skill-creator` (`Skill is valid!`). Geprüft sind Frontmatter und Skill-Struktur.
- Die lokalen Hilfen bestätigen `openspec instructions` für apply/archive,
  `status`, `list`, strikte Validierung sowie `archive` mit Spec-Aktualisierung
  und `validate --archived`. Die vorhandenen generierten Skills stammen aus 1.16.0.
- `gh project item-add --help` und `item-edit --help` bestätigen die verwendete
  Syntax einschließlich Statusänderung über Projekt, Issue-URL, Feld und Wert.
- Lesende GitHub-Abfragen bestätigen FeReiter/Project Nr. 1 „Drone Inventory“,
  privat und geöffnet, ohne Einträge, mit Backlog, Ready, In Progress, Review,
  Done. Es wurden keine Project-Daten verändert.
- `openspec doctor` erkennt die Projektwurzel; `openspec schema validate
  spec-driven` ist erfolgreich. `openspec list --json` enthält keine Changes.
  `openspec validate --all --strict --no-interactive` meldet
  `No items found to validate.`, `openspec validate --archived --no-interactive`
  meldet `No archived changes found.` Das belegt keine fachliche Umsetzung.
- Die Abläufe wurden anhand des Projektauftrags und der generierten Skills
  inhaltlich geprüft: Wiederholung und Teilfehler beim Export, Scope-Grenze beim
  Apply, offene/ungemergte PRs und fehlende Tests beim Archivieren. Es wurde
  kein vollständiger GitHub-Schreiblauf der Skills durchgeführt, um die Vorgabe
  ohne Issues und Changes einzuhalten. Schreibrechte wurden damit nicht praktisch getestet.
- Vollständiger Diff und `git diff --check` werden vor dem Commit geprüft.
  Anwendungstests sind für diese reine Anleitungsänderung nicht erforderlich.

Die Änderungen werden als Commit und Push veröffentlicht und in einer Pull
Request gegen `main` zur Prüfung bereitgestellt. Ein Merge erfolgt nicht automatisch.


## 12. Schritt 8: vorhandenen OpenSpec-Change vervollständigen

Am 12. November 2026 wurde auf dem bereits vorhandenen Branch
`plan-initial-drone-lending-system` der bestehende Change
`initial-drone-lending-system` vervollständigt. Zu Beginn enthielt er nur
`.openspec.yaml`. Es wurde weder ein neuer Branch noch ein neuer Change angelegt.

Die bereits lokal veränderte Projektauftrag-PDF wurde nicht überschrieben oder
als Anforderungsquelle verwendet. Ihre SHA-256-Prüfsumme blieb
`6691cb598df930ac473ec1f79178f9302d0596681b23aea58cf046e083258b3c`.
Gelesen wurden `docs/requirements.md`, `docs/ddd.md`, `openspec/config.yaml`, die
Projektgrundgerüste und das mit `git show HEAD:docs/source/INF4_SYP_19_Projektauftrag_Drohnenverwaltung.pdf`
ausgelesene Original-PDF. Dessen SHA-256 entspricht weiterhin der in Abschnitt 10
festgehaltenen Original-Prüfsumme; alle 43 Seiten wurden mit pdftotext gelesen.

Erstellt wurden Proposal mit Motivation und Umfang, Design mit Architektur,
Domänenmodell, SQLite-Konzept und begründeten Entscheidungen sowie drei Delta-
Spezifikationen unter `specs/drone/`, `specs/student-group/` und `specs/loan/`.
Sie übernehmen alle zehn REQ aus der dokumentierten Quelle. 14 Szenarien enthalten
jeweils explizite GIVEN/WHEN/THEN-Schritte, einschließlich Konkurrenz,
ungültiger Zubehörmengen, Rückgabe und erneuter Loan. Die 22 Tasks umfassen
Backend, Frontend und Tests; jeder Task nennt REQ-Bezüge, Prüfung und erforderliche
Voraussetzungen. Zwei Zuordnungstabellen dokumentieren REQ-001 bis REQ-010 und
DR-01 bis DR-07. Alle 22 Checkboxen sind offen.

Offen bleiben insbesondere Namensvalidierung, Datumsermittlung/-darstellung,
Antworten bei wiederholter Rückgabe oder ungültigen Referenzen und die konkrete
Darstellung von Nullmengen/mehrfachen Positionen. Diese Punkte sind vor den jeweils
betroffenen Implementierungstasks zu entscheiden; fehlende Anforderungen wurden
nicht stillschweigend ergänzt. Die Planung ist als Review-Grundlage vollständig,
keine Freigabe zur ungeprüften Implementierung.

Tatsächlich ausgeführte Prüfungen:

- `openspec context --json` und `openspec list --json`: bestehende Projektwurzel
  und bestehenden Change erkannt; `openspec list --specs`: noch keine Hauptspecs.
- `openspec instructions` für proposal, specs, design und tasks gelesen; Artefakte
  in Abhängigkeitsreihenfolge erstellt und Status dazwischen kontrolliert.
- `openspec validate initial-drone-lending-system --strict`: erfolgreich.
- `openspec validate --all --strict --no-interactive`: 1 bestanden, 0 fehlgeschlagen.
- `openspec status --change initial-drone-lending-system`: 4/4 Planungsartefakte
  vollständig. Diese Anzeige bezeichnet keine erledigten Umsetzungstasks.
- `openspec doctor`: Projektwurzel erkannt; `openspec schema validate spec-driven`:
  Schema gültig.
- Zusätzliche Python-Strukturprüfung: genau drei Specs, zehn eindeutige REQ mit
  SHALL, 14 Szenarien mit GIVEN/WHEN/THEN, 22 eindeutige offene Task-IDs mit
  Prüfhinweis und REQ-Bezug; lokale und committed PDF-Prüfsummen bestätigt.
- Fachliche Gegenprüfung der Specs gegen Requirements/DDD und Original-PDF sowie
  der Zuordnung aller sieben Domain Rules durchgeführt.
- `git diff`, `git status` und `git diff --check` geprüft; Formatprüfung erfolgreich.
  Die neuen Dateien wurden zusätzlich über `git diff --no-index` gegen
  `/dev/null` geprüft; ein Commit-Diff war wegen des Schreibschutzes nicht möglich. Keine Anwendungstests
  ausgeführt, da nur Planungs- und Dokumentationsdateien geändert wurden.

Die lesenden Abfragen `gh pr view 3` und `gh issue list` scheiterten am
Netzwerkzugriff auf api.github.com. Der aktuelle Merge-Status von PR #3 und
vorhandene Issue-Nummern konnten daher nicht verifiziert werden. Die drei
GitHub-Workflow-Skills fehlen auf diesem Branch; die Planung hält dies als
Voraussetzung für spätere Umsetzung fest. Es wurden keine Issues angelegt,
keine Funktionen implementiert, keine Hauptspecs synchronisiert und nichts
archiviert oder gemergt. Als Nächstes folgen das gemeinsame Spec-Review
(Schritt 9) und die Task-Prüfung (Schritt 10 des PDF-Workflows).

Veröffentlichungsgrenze dieses Laufs: Das explizite `git add` ausschließlich der
sieben OpenSpec-Dateien (einschließlich der bestehenden Metadatei) und dieser
Dokumentation scheiterte mit `Unable to create .git/index.lock: Read-only file
system`. Es wurde nichts gestaged oder committed. Die PDF war nicht Teil der
Dateiliste. Ohne Commit der Planung wurden Push und PR-Erstellung nicht
angestoßen; es gibt keinen neuen PR-Link. Die fertigen Dateien bleiben im
Arbeitsverzeichnis für einen späteren Commit erhalten. Weder Dateirechte noch
Branch oder Checkout wurden geändert.

## 13. Spec-Review entscheiden und Planung aktualisieren

Am 12. November 2026 bestätigte der Benutzer alle fünf Empfehlungen aus dem
Spec-Review unverändert und beauftragte die Aktualisierung sowie Veröffentlichung
auf dem vorhandenen Branch `plan-initial-drone-lending-system` gegen `main`.
Der Change bleibt `initial-drone-lending-system`; Anwendungscode wird nicht
geändert, keine Issues werden angelegt und keine Tasks als erledigt markiert.

Bestätigte Ergänzungen zum PDF-Ausgangsstand:

1. Drone.Name und StudentGroup.Name sind Pflichtfelder. Äußere Leerzeichen werden
   entfernt; danach sind 1–100 Zeichen zulässig, ohne besonderes Zeichenmuster.
   Gleiche Namen sind erlaubt, Ids unterscheiden sie; Auswahl zeigt bei gleichen
   Namen zusätzlich die Id.
2. Das Backend ermittelt LoanDate und ReturnDate automatisch als Kalendertag in
   Europe/Vienna. Keine manuelle Datumseingabe; API YYYY-MM-DD, ReturnDate null
   bei Active. Rückgabe vor LoanDate wird mit 400 ohne Änderung abgelehnt.
3. Erneute Rückgabe liefert 409 ohne Änderungen, auch wenn dieselbe Drone bereits
   eine neue Active Loan besitzt. Das ursprüngliche ReturnDate bleibt erhalten.
4. Fehlervertrag: 400 bei ungültigen Eingaben, 404 bei unbekannten Referenzen,
   409 bei Loaned Drone oder Returned Loan. Einheitliches Problem-JSON mit
   status/title/detail und bei Feldfehlern errors; technische Störungen werden
   nicht pauschal als Fachkonflikt behandelt.
5. Menge 0 ist gültig, wird aber nicht als Position gespeichert. Fehlende Typen
   erscheinen als 0. Doppelte eingereichte Zubehörtypen werden auch mit Menge 0
   mit 400 abgelehnt, ohne Summierung. Eine Loan ohne LoanItems ist gültig.

Design, Proposal und die drei Delta-Specs sind abgeglichen. Requirements und DDD
behalten die ursprüngliche Quellenanalyse und verweisen auf die bestätigten
Präzisierungen im Change. Atomarität umfasst die vollständige Ablehnung einer
ungültigen Loan, Rollback bei Speicherfehlern sowie unveränderte Daten bei einer
fehlgeschlagenen Rückgabe. Zwei gleichzeitige gültige Loan-Anfragen für dieselbe
verfügbare Drone ergeben ohne unabhängige technische Störung genau einen Erfolg,
eine Antwort 409 und keine Teilpositionen des Verlierers.

Die Liste umfasst jetzt **25 offene Tasks** statt 22. Bestehende IDs bleiben
stabil: 1.1 wurde auf Einrichtung, Schema (1.3) und Uhr/Fehlervertrag (1.4)
verteilt. 3.2 liefert die Validierung vor der ersten Loan-Operation 3.1; deren
API-Tests prüfen die vollständige Durchsetzung. 3.6 ergänzt den Rollback-Nachweis.
2.5 hängt nun auch von 2.4 ab. 6.1 setzt sämtliche Tasks der Gruppen 1–5 voraus,
inklusive Konkurrenz- und Rollback-Prüfung; 6.2 folgt danach. Alle zehn REQs und
alle sieben Domain Rules bleiben in den Zuordnungstabellen abgedeckt.

`openspec/config.yaml` enthielt noch den früheren Auftrag „ausschließlich
Requirements- und DDD-Dokumentation, keinen Change vorschlagen“. Dieser Abschnitt
wurde auf die aktuelle Planungs-/Review-Phase des bestehenden Changes angepasst.
Architektur, Fachregeln und ausgeschlossener Funktionsumfang bleiben erhalten.

Tatsächliche Prüfungen dieses Aktualisierungslaufs:

- `gh pr view 3 --json number,state,mergedAt,url,title` mit freigegebenem
  Netzwerkzugriff: **MERGED**, Merge-Zeit **2026-11-12T13:40:24Z**;
  [PR #3](https://github.com/FeReiter/drone-inventory/pull/3). Kein Merge durch
  diesen Lauf. Die Skills waren auf dem älteren Planungsbranch noch nicht vorhanden; sie wurden
  beim anschließenden Abgleich mit main unverändert übernommen.
- `git fetch origin main` scheiterte auch außerhalb der Sandbox an der bestehenden,
  einem anderen Benutzer gehörenden `.git/FETCH_HEAD`. Die anschließende Variante
  `git fetch --no-write-fetch-head origin main` war erfolgreich; keine Dateirechte
  geändert und FETCH_HEAD nicht überschrieben. origin/main wurde auf a98abb3
  aktualisiert. Der anschließende Abgleich des Planungsbranches ist unten dokumentiert.
- Die erste strikte Validierung beanstandete zu lange Anforderungseinleitungen.
  Nach Gliederung der Detailregeln bei den zugehörigen Szenarien:
  `openspec validate initial-drone-lending-system --strict` erfolgreich ohne
  Warnungen; `openspec validate --all --strict --no-interactive`: **1 bestanden,
  0 fehlgeschlagen**.
- Python-Strukturprüfung erfolgreich: drei Specs, zehn eindeutige REQ-Abschnitte,
  29 Szenarien mit GIVEN/WHEN/THEN, 25 eindeutige offene Task-IDs, Prüfhinweise
  und REQ-Bezüge in allen Tasks, Abhängigkeiten in Ausführungsreihenfolge und
  Zuordnungstabellen für DR-01 bis DR-07.
- Fachlicher Abgleich der Szenarien und Tasks: Namensgrenzen, ungültige
  Referenzen, Zubehörmengen/-typen, Nullpositionen, doppelte Typen, erneute
  Rückgabe, Datumsgrenzen, Atomarität und Konkurrenz sind als Prüfungen geplant.
- `git diff --check`: erfolgreich. Anwendungs-Builds und Anwendungstests wurden
  für diese reine Planungsänderung nicht ausgeführt; die geplanten Fachtests
  sind damit ausdrücklich noch nicht bestanden.
- Die SHA-256-Prüfsumme der lokalen PDF wurde vor und nach der Bearbeitung
  verglichen: unverändert gegenüber Abschnitt 12. Sie wird weder gestaged noch
  committed. Der Commit umfasst ausschließlich OpenSpec-Planung und zugehörige
  Textdokumente; der gestagte Diff wird vor dem Commit nochmals geprüft.

Die Veröffentlichung erfolgt als Planungs-PR gegen main ohne automatische
Zusammenführung. Keine Hauptspecs werden synchronisiert und nichts archiviert;
die spätere Implementierung benötigt weiterhin ihre Issue-Zuordnung und den
vorgeschriebenen Task-/Branch-/PR-Workflow.

Abgleich vor Veröffentlichung: Nach dem Planungscommit cdf572b wurde origin/main
(a98abb3) in den bestehenden Planungsbranch integriert. Nur die gleichzeitig
erweiterte `docs/projektablauf.md` hatte einen Konflikt. Beide Einträge wurden
vollständig erhalten und die neuen Abschnitte fortlaufend nummeriert. Die drei
Workflow-Skills aus PR #3 wurden unverändert übernommen. main und PR #3 wurden
dabei nicht verändert. Gegen main enthält der Planungs-PR ausschließlich die
elf Planungs-/Textdateien. Die lokale PDF bleibt unverändert und uncommitted.
Strikte Change-/Gesamtvalidierung sowie `git diff --check` und
`git diff --cached --check` wurden nach Auflösung erneut erfolgreich ausgeführt.


### Abschließende Gegenprüfung und Ergänzung

Die erneute GitHub-Abfrage bestätigt PR #3 weiterhin als MERGED mit
mergedAt 2026-11-12T13:40:24Z. Der bereits vorbereitete Merge von main liegt
lokal mit aufgelösten Konflikten vor und wird mit der Planung abgeschlossen.

Ein explizites Szenario für zwei gleichzeitige Rückgaben ergänzt REQ-008.
Task 5.6 trennt den übergreifenden Rückgabe-Rollback- und Konkurrenznachweis
von der Rückgabeoperation 5.1. Der endgültige Stand umfasst **26 offene Tasks**
und **30 Szenarien**; keine Checkbox wurde abgehakt. Alle zehn REQs und sieben
Domain Rules sind weiterhin zugeordnet. Die vorhandene config.yaml passt nach
der bereits enthaltenen Phasenanpassung; keine weitere Änderung erforderlich.

Erneut tatsächlich ausgeführt: strikte Change-Validierung erfolgreich;
`openspec validate --all --strict --no-interactive`: 1 bestanden, 0 fehlgeschlagen.
Python-Strukturprüfung: drei Specs, zehn eindeutige REQs, 30 Szenarien mit
GIVEN/WHEN/THEN, 26 eindeutige offene Tasks mit Prüfhinweisen/REQ-Bezügen,
Abhängigkeiten in Ausführungsreihenfolge und sieben DR-Zuordnungen bestätigt.
`git diff --check` und `git diff --cached --check` erfolgreich; Diffs geprüft.
Die PDF-Prüfsumme ist weiterhin
6691cb598df930ac473ec1f79178f9302d0596681b23aea58cf046e083258b3c.
Keine Anwendungstests ausgeführt, kein Anwendungscode geändert.

Veröffentlichung: Staging scheiterte erneut am fremdbesessenen .git/objects,
auch außerhalb der Sandbox. Deshalb wird derselbe vorhandene Planungsbranch
aus einem isolierten lokalen Clone unter /tmp/drone-plan-publish-20261112
veröffentlicht. Der Clone enthält die committed Original-PDF; die lokal
veränderte PDF wird nicht kopiert. Der main-Abgleich wird dort mit den
geprüften Textdateien abgeschlossen. Keine Dateirechte wurden verändert.


## 14. Schritt 9: OpenSpec-Tasks nach GitHub übertragen

Am 12.11.2026 wurde auf ausdrücklichen Auftrag der Skill
`.github/skills/openspec-to-github/SKILL.md` für den bestehenden Change
`initial-drone-lending-system` ausgeführt. Die Bezeichnung Schritt 9 folgt dem
aktuellen Arbeitsauftrag; der PDF-Leitfaden bezeichnet den Issue-Export als
Schritt 11. Es wurden keine Fachfunktionen implementiert.

### Sicherung und Synchronisierung

Vor jedem Schreibzugriff wurden Status, lokale Branches und origin/main geprüft.
Neun uncommittete Dateien wurden mit Verzeichnispfaden unter
`/tmp/drone-inventory-backup-20261112-154431/files/` gesichert und per SHA-256
verglichen. Zusätzlich enthält die Sicherung staged.patch, unstaged.patch,
Git-Status, Index und Merge-Metadaten. Die vorhandenen Änderungen und der bereits
begonnene Merge im ursprünglichen Checkout wurden nicht verworfen oder beendet.
Die lokal geänderte PDF bleibt dort erhalten; ihr SHA-256 ist weiterhin
6691cb598df930ac473ec1f79178f9302d0596681b23aea58cf046e083258b3c.
Die Sicherung unter /tmp ist eine zusätzliche lokale Kopie, kein dauerhaftes Archiv.

`git fetch --no-write-fetch-head origin main` war erfolgreich. PR #4 ist laut
GitHub seit 2026-11-12T14:42:44Z gemergt. Der lokale, nicht ausgecheckte main
wurde auf origin/main, d97bfce2efda722de38e4e17019aaabe18c0d75a, gesetzt.
Der Ref-Schreibzugriff erforderte wegen der Sandbox eine Freigabe; anschließend
stimmten beide Commit-IDs überein. Alle neun gesicherten Arbeitsdateien sind
nachweislich unverändert. Ein Branch-Wechsel im ursprünglichen Checkout wurde
vermieden. Im isolierten Checkout `/tmp/drone-issues-step9` wurde main per
Fast-forward auf denselben Commit synchronisiert und der neue Branch
`docs/openspec-task-issues` angelegt. Die drei Workflow-Skills sowie 26 offene
Tasks wurden dort bestätigt; die veränderte PDF wurde nicht kopiert.

### Issue-Export und Project

Requirements, DDD, config.yaml, Proposal, Design, drei Delta-Specs und die
vollständige Task-Datei wurden gelesen; der committed Projektauftrag wurde mit
pdftotext ausgewertet. Es existieren noch keine Hauptspecs. GitHub-Repository
FeReiter/drone-inventory, Anmeldung mit repo-/project-Rechten, Project Nr. 1
„Drone Inventory“ sowie Backlog, Ready, In Progress, Review und Done wurden
verifiziert. Der ausdrückliche aktuelle Auftrag erlaubt den Issue-Export trotz
der früheren, auf die reine Planungsphase beschränkten Nicht-Erstellen-Hinweise.

Die REST-Abfrage aller offenen und geschlossenen Einträge wurde vollständig
paginiert; Pull Requests wurden ausgeschlossen. Vorher gab es keine Issues.
26 Issues wurden neu erstellt, keine wiederverwendet. Jeder Body enthält den
stabilen Schlüssel aus Repository, Change und Task-ID, den unveränderten
Taskumfang mit Abhängigkeiten, REQ-Bezüge und Links zu den passenden
Spec-Anforderungen, Proposal, Design, Akzeptanzkriterien und Testhinweise.
Die Nummer wurde jeweils unmittelbar nach erfolgreicher Erstellung in tasks.md
und zusätzlich in einem lokalen Exportprotokoll gespeichert. Anschließend wurde
das Issue zum bestehenden Project hinzugefügt und auf Backlog gesetzt.

| OpenSpec-Task | GitHub Issue | Project-Status |
| --- | --- | --- |
| 1.1 | [#5](https://github.com/FeReiter/drone-inventory/issues/5) | Backlog |
| 1.3 | [#6](https://github.com/FeReiter/drone-inventory/issues/6) | Backlog |
| 1.2 | [#7](https://github.com/FeReiter/drone-inventory/issues/7) | Backlog |
| 1.4 | [#8](https://github.com/FeReiter/drone-inventory/issues/8) | Backlog |
| 2.1 | [#9](https://github.com/FeReiter/drone-inventory/issues/9) | Backlog |
| 2.2 | [#10](https://github.com/FeReiter/drone-inventory/issues/10) | Backlog |
| 2.3 | [#11](https://github.com/FeReiter/drone-inventory/issues/11) | Backlog |
| 2.4 | [#12](https://github.com/FeReiter/drone-inventory/issues/12) | Backlog |
| 2.5 | [#13](https://github.com/FeReiter/drone-inventory/issues/13) | Backlog |
| 2.6 | [#14](https://github.com/FeReiter/drone-inventory/issues/14) | Backlog |
| 3.2 | [#15](https://github.com/FeReiter/drone-inventory/issues/15) | Backlog |
| 3.1 | [#16](https://github.com/FeReiter/drone-inventory/issues/16) | Backlog |
| 3.3 | [#17](https://github.com/FeReiter/drone-inventory/issues/17) | Backlog |
| 3.6 | [#18](https://github.com/FeReiter/drone-inventory/issues/18) | Backlog |
| 3.4 | [#19](https://github.com/FeReiter/drone-inventory/issues/19) | Backlog |
| 3.5 | [#20](https://github.com/FeReiter/drone-inventory/issues/20) | Backlog |
| 4.1 | [#21](https://github.com/FeReiter/drone-inventory/issues/21) | Backlog |
| 4.2 | [#22](https://github.com/FeReiter/drone-inventory/issues/22) | Backlog |
| 5.1 | [#23](https://github.com/FeReiter/drone-inventory/issues/23) | Backlog |
| 5.6 | [#24](https://github.com/FeReiter/drone-inventory/issues/24) | Backlog |
| 5.2 | [#25](https://github.com/FeReiter/drone-inventory/issues/25) | Backlog |
| 5.3 | [#26](https://github.com/FeReiter/drone-inventory/issues/26) | Backlog |
| 5.4 | [#27](https://github.com/FeReiter/drone-inventory/issues/27) | Backlog |
| 5.5 | [#28](https://github.com/FeReiter/drone-inventory/issues/28) | Backlog |
| 6.1 | [#29](https://github.com/FeReiter/drone-inventory/issues/29) | Backlog |
| 6.2 | [#30](https://github.com/FeReiter/drone-inventory/issues/30) | Backlog |

### Tatsächliche Prüfungen

- Erneute vollständige Abfrage offener/geschlossener Issues: für jede der 26
  Task-IDs exakt ein passendes offenes Issue, keine doppelten Schlüssel oder
  mehrfach verwendeten Issue-Nummern.
- Project-Einträge erneut gelesen; vollständige Ergebnismenge anhand totalCount
  geprüft. Alle 26 verknüpften Issues sind genau einmal im Project auf Backlog.
- Task-Zuordnungsprüfung: 26/26 verknüpft, alle Checkboxen offen. Die ursprünglichen
  Tasktexte bleiben abgesehen vom angehängten GitHub-Verweis unverändert.
- `openspec list`, `status` und `instructions apply` bestätigen den bestehenden
  Change mit vollständiger Planung und 0/26 erledigten Tasks. Apply wurde nur
  zur Statusprüfung gelesen; keine Implementierung gestartet.
- Strikte Change-Validierung und strikte Gesamtvalidierung erfolgreich;
  Gesamtvalidierung: 1 bestanden, 0 fehlgeschlagen.
- `git diff` geprüft; `git diff --check` und `git diff --cached --check` erfolgreich.
  Der Commit enthält ausschließlich tasks.md und diese Dokumentation.
- Keine Anwendungstests ausgeführt: Dieser Schritt ändert nur Aufgabenverweise
  und Textdokumentation. Akzeptanzkriterien der Issues sind geplante Prüfungen,
  keine bereits bestandenen Fachtests.

Veröffentlichung auf dem neuen Dokumentationsbranch mit PR gegen main.
Kein automatischer Merge, kein Schließen von Issues, keine erledigten Tasks,
keine Archivierung und keine Änderung von Anwendungscode oder PDF.
