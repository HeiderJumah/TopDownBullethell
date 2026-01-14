# Top-Down Bullet Hell Multiplayer



## Kurzbeschreibung

Ein 2-Spieler Online Bullet-Hell-Spiel aus der Top-Down-Perspektive.

Das Spiel nutzt ein server-autoritäres Multiplayer-System auf Basis von FishNet.



---



## Technologie

- Unity 6000.0.x

- FishNet Networking

- GitHub Desktop



---



## Spiel starten (Host \& Client)

1. Projekt in Unity öffnen

2. Multiplayer Play Mode aktivieren

3. Instance 1: Host starten

4. Instance 2: Client starten

5. Verbindung erfolgt über FishNetHUD



---



## Technischer Überblick



### Multiplayer

- Server-autoritäres Modell

- FishNet NetworkManager

- Player als NetworkObject



### Verwendete RPCs

- ServerRpc: Spielerbewegung

- ServerRpc: Projektil-Spawn

- ObserversRpc: visuelle Effekte



### SyncVars

- Player Health

- Player Color / Name

- Score



### Bullet-Logik

- Projektile werden serverseitig gespawnt

- Mehrere Bullet-Patterns (z. B. Gerade / Spread)

- Treffererkennung auf dem Server



### Gegner-Logik

- Gegner werden serverseitig gespawnt

- Unterschiedliche Gegnertypen

- Wave-System oder Boss-Mechanik



---



## HUD \& Punkte

- Anzeige von HP und Score

- Synchronisierte Punktevergabe

- Highscore-System (siehe Persistenz)



---



## Persistenz

- Highscore-Speicherung via (PHP \& SQL / JSON / PlayerPrefs)

- Serverseitige Verwaltung



---



## Bonusfeatures

- (wird ergänzt)



---



## Bekannte Bugs / Einschränkungen

- (wird ergänzt)



---



## Projektstatus

🔧 In Entwicklung

