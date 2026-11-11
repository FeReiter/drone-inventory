# Drone Inventory

Schulprojekt zur Verwaltung von Drohnen. Dieses Repository enthält zunächst nur
zwei getrennte, startfähige Grundgerüste:

- `backend/DroneInventory.Api/`: ASP.NET Core 10 Web API mit C#.
- `frontend/drone-inventory-ui/`: Angular 22 mit TypeScript, CSS und Routing.

Die spätere Kommunikation soll über REST/JSON erfolgen. Entity Framework Core
und SQLite sind für einen späteren Schritt vorgesehen und noch nicht installiert.
Es gibt noch keine Drohnen-Models, Controller, Datenbank, Geschäftslogik oder
OpenSpec-Spezifikationen. Das Wetterbeispiel der .NET-Vorlage wurde entfernt;
das Frontend zeigt die unveränderte Angular-Begrüßungsseite. Beide Projekte sind
noch nicht miteinander verbunden.

## Voraussetzungen

- .NET 10 SDK **einschließlich ASP.NET Core 10 Runtime**. Kontrolle mit
  `dotnet --info` und `dotnet --list-runtimes`: Neben `Microsoft.NETCore.App`
  muss auch `Microsoft.AspNetCore.App 10.0.x` vorhanden sein.
- Node.js passend zu Angular 22: `^22.22.3`, `^24.15.0` oder `^26.0.0`.
  Siehe [Angular-Versionsübersicht](https://angular.dev/reference/versions).
- npm (mit Node.js installiert). Geprüft mit Node.js 26.13.0 und npm 12.2.1.
- Eine globale Angular-CLI-Installation ist nicht erforderlich.

Alle folgenden Befehle beginnen im Stammordner dieses Repositorys.
Backend und Frontend laufen in **zwei separaten Terminals**.

## Backend starten

```bash
cd backend/DroneInventory.Api
dotnet restore
dotnet run --launch-profile http
```

Die API läuft unter **http://localhost:5250**. Im Entwicklungsmodus ist das
OpenAPI-Dokument unter **http://localhost:5250/openapi/v1.json** erreichbar.
Es enthält noch keine fachlichen Endpunkte; `/` liefert deshalb HTTP 404.
Eine Swagger-Oberfläche ist nicht eingerichtet.

Optional mit HTTPS:

```bash
dotnet dev-certs https --trust
dotnet run --launch-profile https
```

Das HTTPS-Profil verwendet **https://localhost:7293** und leitet HTTP auf HTTPS
um. Je nach Betriebssystem ist für das Vertrauen des Entwicklungszertifikats ein
zusätzlicher manueller Schritt nötig.

## Frontend starten

```bash
cd frontend/drone-inventory-ui
npm ci
npm start
```

Im Browser **http://localhost:4200** öffnen. `npm ci` installiert die durch
`package-lock.json` festgelegten Pakete. Der Entwicklungsserver lädt Änderungen
automatisch neu. Beide Server lassen sich mit `Strg+C` beenden.

## Build und Tests

Backend:

```bash
dotnet build backend/DroneInventory.Api/DroneInventory.Api.csproj
dotnet test backend/DroneInventory.Api/DroneInventory.Api.csproj
```

Ein eigenes Backend-Testprojekt ist noch nicht vorhanden. `dotnet test` führt
hier daher keine Tests aus.

Frontend:

```bash
cd frontend/drone-inventory-ui
npm run build
npm test -- --watch=false
```

`npm run build` erstellt den Produktionsbuild unter
`dist/drone-inventory-ui/`. `npm test` ohne zusätzliche Optionen startet den
Test-Watch-Modus. Die Vorlage enthält zwei Komponententests mit Vitest.
Ein separater Lint- oder Ende-zu-Ende-Testbefehl ist nicht eingerichtet.

### Prüfung in der Erstellungsumgebung

- Backend-Build erfolgreich, ohne Warnungen oder Fehler.
- Backend-Start und Abruf des OpenAPI-Dokuments geprüft.
- Frontend-Produktionsbuild erfolgreich; beide Tests erfolgreich.
- Frontend-Entwicklungsserver gestartet und per HTTP abgerufen (HTTP 200).

Die lokale .NET-10.0.114-Installation dieser Umgebung enthält keine
ASP.NET-Core-Runtime und unvollständige Prune-Metadaten. Für den Build wurde
vorübergehend `dotnet build backend/DroneInventory.Api/DroneInventory.Api.csproj
-p:AllowMissingPrunePackageData=true` verwendet. Für die Startprüfung wurde eine
ASP.NET-Core-10.0.14-Runtime unter `/tmp` bereitgestellt. Diese Hilfsdateien und
die temporäre Build-Option sind kein Bestandteil der Projektkonfiguration.
Bei einer vollständigen .NET-10-Installation gelten die normalen Befehle oben.

## Wichtige Dateien

| Datei | Aufgabe |
| --- | --- |
| `backend/DroneInventory.Api/Program.cs` | Startet die API und richtet OpenAPI sowie HTTPS-Weiterleitung ein. |
| `backend/DroneInventory.Api/DroneInventory.Api.csproj` | Legt .NET 10 und das OpenAPI-Paket fest. |
| `backend/DroneInventory.Api/appsettings.json` | Gemeinsame Einstellungen, zunächst für Logging und erlaubte Hosts. |
| `backend/DroneInventory.Api/appsettings.Development.json` | Gemeinsame Logging-Einstellungen für die Entwicklung. |
| `backend/DroneInventory.Api/Properties/launchSettings.json` | Gemeinsame Startprofile mit lokalen Entwicklungsports. |
| `frontend/drone-inventory-ui/src/main.ts` | Startet die Angular-Anwendung. |
| `frontend/drone-inventory-ui/src/app/app.ts` | Wurzelkomponente der Oberfläche. |
| `frontend/drone-inventory-ui/src/app/app.html` | HTML der Angular-Begrüßungsseite. |
| `frontend/drone-inventory-ui/src/app/app.routes.ts` | Platz für spätere Seitenrouten, derzeit leer. |
| `frontend/drone-inventory-ui/src/app/app.spec.ts` | Zwei mitgelieferte Komponententests. |
| `frontend/drone-inventory-ui/angular.json` | Konfiguration für Angular-Build, Server und Tests. |
| `frontend/drone-inventory-ui/package.json` | Abhängigkeiten und npm-Befehle. |
| `frontend/drone-inventory-ui/package-lock.json` | Reproduzierbare Paketversionen für `npm ci`. |
| `.gitignore` | Schließt Build-Ausgaben, `node_modules`, Caches, Datenbanken und lokale Einstellungen aus. |

Gemeinsame Vorlagenkonfigurationen werden versioniert; persönliche Einstellungen,
Zugangsdaten und `.env`-Dateien gehören nicht ins Repository.
