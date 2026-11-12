# Design

## Context

Siehe `proposal.md` für Motivation und Umfang. Vorhanden sind eine ASP.NET-Core-10-
Web-API ohne fachliche Endpunkte und ein Angular-22-Grundgerüst mit leeren Routen.
EF Core, SQLite und ein Backend-Testprojekt sind noch nicht eingerichtet. Die
bestehenden Angular-Tests prüfen nur das Grundgerüst. Dieses Design plant die
spätere Umsetzung; es führt weder Pakete noch Code oder Datenbanken ein.

Die fachliche Quelle bilden `docs/requirements.md`, `docs/ddd.md` und das im
Proposal bezeichnete committed Original-PDF, insbesondere Seiten 7–19.
Die drei Delta-Spezifikationen beschreiben das prüfbare Verhalten.

## Goals / Non-Goals

**Goals:** Fachregeln zentral erzwingen, konkurrierende Loan-Versuche absichern,
Historie dauerhaft speichern und jede Funktion auf REQ und Szenarien zurückführen.

**Non-Goals:** Keine zusätzliche Domänenentity, kein direkter Datenbankzugriff aus
Angular und kein unabhängig gespeicherter Drone-Verfügbarkeitsstatus. Fachliche
Ausschlüsse stehen im Proposal. Keine allgemeine Repository-Abstraktionsschicht
oder verteilte Architektur für dieses kleine Schulprojekt einführen.

## Decisions

### Architektur und Schnittstellen

Angular → REST/JSON → ASP.NET Core 10 → EF Core → SQLite entspricht dem Auftrag.
Fachliche Operationen werden im Backend gebündelt; HTTP-Endpunkte nehmen
Eingabedaten entgegen und liefern explizite JSON-Antworten. Separate Transportdaten
verhindern, dass interne Persistenzdetails den HTTP-Vertrag bestimmen. Angular
zeigt Backend-Fehler an; Formularprüfungen ersetzen keine Backend-Validierung.
Eine reine Frontend-Validierung könnte direkte oder gleichzeitige API-Aufrufe
nicht absichern und wird deshalb nicht als Durchsetzung der Regeln verwendet.

Geplante Routen: POST/GET `/api/drones`, POST/GET `/api/student-groups`, POST
`/api/loans`, GET `/api/loans/active`, GET `/api/loans/history` und POST
`/api/loans/{id}/return`. GET StudentGroups dient der Auswahl beim Erstellen einer
Loan; es ist keine zusätzliche Verwaltungsfunktion. Es werden keine Bearbeitungs-
oder Löschfunktionen ergänzt. Exakte JSON-Feldnamen der Erfolgsantworten werden vor
dem jeweiligen API-Task im Design konkretisiert, ohne das spezifizierte Verhalten
zu ändern. Ein fehlgeschlagener Loan-Versuch darf keine Teil-Loan hinterlassen.

### Domänenmodell und Grenzen

| Entity | Attribute |
| --- | --- |
| Drone | Id, Name, Description |
| StudentGroup | Id, Name |
| Loan | Id, DroneId, StudentGroupId, LoanDate, ReturnDate, Status |
| LoanItem | Id, LoanId, Type, Quantity |

Eine Loan referenziert genau eine Drone und eine StudentGroup; beide können über
die Zeit mehrere Loans besitzen. Eine Loan enthält 0 bis viele LoanItems und
bildet mit ihnen ein Aggregat. LoanItem.Id bezeichnet die Zubehörposition, kein
Gerät. Type ist Battery oder Propeller. Quantity ist ganzzahlig und nichtnegativ.
Status der Loan ist Active oder Returned. Drone-Verfügbarkeit wird bei jeder
Abfrage aus der Existenz einer aktiven Loan abgeleitet. Ein eigenes Statusfeld
auf Drone würde zwei fachliche Wahrheiten schaffen und wird verworfen.
Eine StudentGroup wird nicht auf eine einzige aktive Loan beschränkt.

### Datenbank und Konsistenz

EF Core bildet die vier Entities mit Fremdschlüsseln in SQLite ab. Migrationen
machen das Schema reproduzierbar; lokale Datenbankdateien bleiben unversioniert.
Als technische Wahl werden datenbankgenerierte Integer-IDs verwendet, weil keine
verteilte ID-Vergabe erforderlich ist. Dies ist keine zusätzliche PDF-Anforderung.
Loan.ReturnDate ist vor der Rückgabe nullable. Datumswerte werden als DateOnly modelliert; die bestätigte Datumsermittlung steht unten.

Loan-Erstellung speichert Loan und sämtliche LoanItems in einer Transaktion.
Referenzen, Type und Quantity werden im Backend geprüft. Datenbankbedingungen
für erlaubte Status-/Type-Werte und ganzzahlige, nichtnegative Quantity ergänzen
die Validierung; Tests nutzen echtes SQLite statt eines vereinfachten In-Memory-
Providers. Es gibt keinen Bestand einzelner Zubehörgeräte.

Ein eindeutiger partieller Index auf Loan.DroneId für Status Active schützt
gegen konkurrierende aktive Loans. Eine reine Verfügbarkeitsprüfung vor dem
Speichern hätte ein Zeitfenster für doppelte Loans und reicht nicht aus.
Verletzungen des Index sowie konkurrierende SQLite-Schreibkonflikte werden
kontrolliert behandelt; nach dem konkurrierenden Commit ist der verlierende
Versuch fachlich abzulehnen. Tests prüfen zwei getrennte Verbindungen und den
anschließend gespeicherten Zustand, einschließlich genau einer erfolgreichen Loan und einer Antwort HTTP 409 bei zwei gültigen Versuchen ohne unabhängige technische Störung. Sperrkonflikte werden begrenzt erneut geprüft, bis der Gewinner sichtbar ist; technische Störungen werden nicht pauschal zu HTTP 409 umgedeutet.

Rückgabe ändert Status und ReturnDate zusammen in einer Transaktion. LoanDate,
Zuordnungen und LoanItems bleiben unverändert. Für historische Referenzen werden
keine kaskadierenden Löschungen vorgesehen. Eine fehlgeschlagene Rückgabe darf
keinen teilweise geänderten Zustand hinterlassen. Dauerhaftigkeit wird durch
Schließen und erneutes Öffnen der SQLite-Datei geprüft.

### Oberfläche und Prüfstrategie

Angular erhält getrennte Ansichten/Formulare für Drones, StudentGroups, neue
Loans, aktive Loans und Historie. Die Auswahl einer Drone zeigt ihre abgeleitete
Verfügbarkeit; das Backend entscheidet bei konkurrierenden Versuchen. Nach
Erstellen und Rückgabe werden betroffene Listen erneut über REST geladen.
Zubehör wird ausschließlich über Type und Quantity eingegeben.

API-/SQLite-Integrationstests decken die Persistenz und Fehlerfälle aller zehn
REQ ab, insbesondere Konkurrenz und Rückgabe. Angular-Komponententests prüfen
Eingaben, Anzeigen und Fehlerbehandlung mit simulierten HTTP-Antworten. Ein
abschließender Ablauf mit echter API und SQLite prüft das Beispiel TELLO-07,
Group 3, 4 Battery und 8 Propeller bis zur Historie nach erneutem Laden.

### Bestätigte Review-Entscheidungen vom 12.11.2026

Diese Ergänzungen wurden vom Benutzer bestätigt; sie sind keine aus den
PDF-Beispielen abgeleiteten zusätzlichen Vorgaben.

- Namen: Name von Drone und StudentGroup ist Pflicht, wird außen getrimmt und
  muss danach 1–100 Zeichen enthalten. Keine Zeichenmuster oder Eindeutigkeit.
  Ids unterscheiden gleiche Namen; Auswahl zeigt bei gleichen Namen zusätzlich
  die Id. Backend validiert; Angular spiegelt die Regeln und zeigt Feldfehler.
- Datum: Backend setzt LoanDate beim Erstellen und ReturnDate bei Rückgabe aus
  einer injizierbaren Uhr auf den Kalendertag in Europe/Vienna. Keine manuelle
  Datumseingabe; JSON YYYY-MM-DD, Active hat ReturnDate null. ReturnDate vor
  LoanDate wird mit 400 ohne Zustandsänderung abgelehnt. Tests setzen die Uhr,
  einschließlich UTC-Tagesgrenze und Winter-/Sommerzeit.
- Erneute Rückgabe: 409 ohne Änderungen, insbesondere kein neues ReturnDate.
  Eine neue aktive Loan derselben Drone bleibt unberührt. Statusprüfung und
  Änderung erfolgen atomar, auch bei konkurrierenden Rückgaben.
- Fehlervertrag: 400 bei ungültiger Eingabe, Menge, Type oder doppeltem Type;
  404 bei unbekannter Drone, StudentGroup oder Loan; 409 bei Loaned Drone oder
  Returned Loan. JSON nach ProblemDetails mit status, title, detail und bei
  Feldfehlern errors (Feldname auf Meldungsliste). Das Frontend verwendet status
  und errors zur Darstellung. Technische Fehler werden nicht pauschal als
  Fachkonflikt behandelt. Diese Fehlerfälle werden direkt über die API geprüft.
- Zubehör: Quantity 0 wird angenommen und nicht persistiert; fehlender Type wird
  als 0 angezeigt, auch in der Historie. Keine LoanItems ist gültig. Je Anfrage
  höchstens eine Position pro Type, doppelte Typen auch mit Nullposition werden
  vor Normalisierung mit 400 abgelehnt. Keine Summierung. Ein eindeutiger Index
  auf (LoanId, Type) sichert persistierte Positionen ergänzend ab.
- Atomarität: Alle Referenzen, Mengen und Typen werden bereits beim ersten
  Loan-Endpunkt geprüft. Ungültige Eingaben oder Speicherfehler hinterlassen
  weder Loan noch Teilpositionen. Tests provozieren auch einen Speicherfehler
  nach Beginn der Transaktion und vergleichen den Zustand nach Rollback.

## Risks / Trade-offs

- SQLite erlaubt begrenzte parallele Schreibzugriffe → kurze Transaktionen,
  kontrollierte Konfliktbehandlung und Konkurrenztests mit getrennten Verbindungen.
- Verfügbarkeitsanzeige kann zwischen Laden und Absenden veralten → Backend
  validiert erneut, Index schützt Konsistenz, Oberfläche lädt danach neu.
- Historische Anzeigen referenzieren Drone und StudentGroup → derzeit sind
  Änderungen/Löschungen nicht beauftragt; spätere Änderungen benötigen eine
  eigene Entscheidung zum Erhalt historischer Bezeichnungen.
- PR #3 ist laut GitHub seit 12.11.2026 13:40:24 UTC gemergt. Die Skills fehlen noch auf diesem älteren Planungsbranch; vor Implementierung den dann aktuellen main-Stand integrieren und vorhandene Issues zuordnen, keine Nummern erfinden.
- Lokale .NET-Runtime war laut Projektablauf unvollständig → vor Backend-Tests
  vollständige ASP.NET-Core-10-Laufzeit sicherstellen; hier keine Reparatur.

## Migration Plan

Erst nach Review, Klärung betroffener Detailentscheidungen und Issue-Zuordnung
werden die offenen Tasks einzeln umgesetzt. Die erste Migration erstellt das
Schema in einer neuen lokalen SQLite-Datei; es gibt noch keine fachlichen
Bestandsdaten zu migrieren. Vor späteren Schemaänderungen eine vorhandene Datei
sichern und Migration an einer Kopie prüfen. Ein Rücksetzen der ersten Migration
ist nur für leere Testdatenbanken vorgesehen; Nutzdaten nicht stillschweigend
löschen. Dieser Planungsauftrag führt keine Migration aus.

## Open Questions

Die fünf Review-Fragen sind entschieden und in den Specs beobachtbar festgelegt.
Historienlayout bleibt frei; Filter, Sortierung und Seiteneinteilung sind keine
zusätzlichen Abnahmekriterien. Erfolgs-JSON-Feldnamen sind vor dem jeweiligen
API-Task im Design zu konkretisieren. Der auf PDF-Seite 43 erwähnte zusätzliche
Leitfaden liegt weiterhin nicht vor. Issue-Zuordnung erfolgt vor Implementierung;
dieser Auftrag erstellt keine Issues und gibt keine Implementierung frei.
