# CountIt 📊🎵

**CountIt** ist eine moderne, plattformübergreifende Zähler-App für Windows und Android, entwickelt in C# und .NET. Sie ermöglicht das verwalten von Punkteständen in verschiedenen Abschnitten/Phasen – ideal für Spiele, Tracker, Audits oder Event-Phasen.

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Plattformen](https://img.shields.io/badge/Plattformen-Windows%20%7C%20Android-brightgreen)
![Architektur](https://img.shields.io/badge/Architektur-MVVM-blue)

---

## ✨ Features

* **Multi-Abschnitts-System:** Erstelle neue Abschnitte/Phasen. Der Gesamtpunktestand wird fortlaufend übernommen.
* **Abschnitts-Zähler:** Jeder Zähler zeigt neben dem Gesamtpunktestand auch die Differenz im aktuellen Abschnitt an `(+3)`.
* **Sound-Feedback:** Weise einzelnen Zählern eigene Sounds (`.mp3`, `.wav`) mit individueller Lautstärkeregelung zu.
* **JSON-Im- und Export:** Speichere und lade deine Stände flexibel über JSON-Dateien.
* **Saubere MVVM-Architektur:** Geschäftslogik ist in eine wiederverwendbare Core-Bibliothek (`CountIt.Core`) ausgelagert.
* **Cross-Platform:** Läuft als native Desktop-App (WPF) oder als mobile/Desktop App via .NET MAUI.

---

## 🏗 Projektstruktur

Das Repository ist nach dem **MVVM-Muster (Model-View-ViewModel)** strukturiert:

```text
CountIt/
├── CountIt.Core/          # Geschäftslogik, Models, ViewModels & Interfaces (Plattformunabhängig)
├── CountIt.WPF/           # Windows Desktop-UI (WPF, Hotkeys, SoundPlayer)
└── CountIt.Maui/          # Cross-Platform UI für Android & Windows (.NET MAUI)
