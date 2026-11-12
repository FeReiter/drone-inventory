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


## 11. Schritt 8: vorhandenen OpenSpec-Change vervollständigen

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

## 12. Spec-Review entscheiden und Planung aktualisieren

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
  diesen Lauf. Die Skills fehlen auf dem älteren Planungsbranch noch; vor einer
  Implementierung ist der aktuelle main-Stand zu integrieren.
- `git fetch origin main` scheiterte auch außerhalb der Sandbox an der bestehenden,
  einem anderen Benutzer gehörenden `.git/FETCH_HEAD`. Die anschließende Variante
  `git fetch --no-write-fetch-head origin main` war erfolgreich; keine Dateirechte
  geändert und FETCH_HEAD nicht überschrieben. origin/main wurde auf a98abb3
  aktualisiert; kein Merge oder Rebase des Planungsbranches vorgenommen.
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
  verglichen: unverändert gegenüber Abschnitt 11. Sie wird weder gestaged noch
  committed. Der Commit umfasst ausschließlich OpenSpec-Planung und zugehörige
  Textdokumente; der gestagte Diff wird vor dem Commit nochmals geprüft.

Die Veröffentlichung erfolgt als Planungs-PR gegen main ohne automatische
Zusammenführung. Keine Hauptspecs werden synchronisiert und nichts archiviert;
die spätere Implementierung benötigt weiterhin ihre Issue-Zuordnung und den
vorgeschriebenen Task-/Branch-/PR-Workflow.
