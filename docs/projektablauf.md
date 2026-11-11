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

Die aktuelle Arbeit ergänzt ausschließlich `docs/projektablauf.md`; Anwendungscode
bleibt unverändert. Es wurden keine GitHub Issues oder Project-Einträge angelegt.

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
