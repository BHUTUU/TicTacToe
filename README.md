# 🎮 Tic Tac Toe - Windows Forms Application

A modern, fully-featured **Tic Tac Toe** game built using **C#** and **Windows Forms**. It offers a polished UI, intelligent undo/redo and optional computer opponent — making it more than just the classic version.

---

## 🖼️ Features

* ✨ Smooth, centered 3x3 grid UI (dynamically resizes with window)
* 🔄 Turn indicator label for current player (`X` or `O`)
* 🖱️ Mouse click to make moves
* ✅ Win and draw detection with popup announcements
* 🔁 Automatic restart after game ends
* 💻 Optional **"Play with Computer"** mode (basic AI)
* ⏪ **Undo/Redo** functionality with buttons and shortcuts (`Ctrl+Z`, `Ctrl+Y`)

---

## 💻 Technologies Used

* **C#**
* **Windows Forms (.NET)**
* **GDI+** for custom drawing (grid, X, O)

---

## 🚀 Getting Started

### Prerequisites (For Source Build)

* Visual Studio with **.NET Desktop Development** workload
* .NET Framework or SDK installed

### Running from Source

1. Clone or download this repository.
2. Open the solution in **Visual Studio**.
3. Press `F5` to build and run.

---

## 📦 Installation

You can download the latest setup file from the [Releases](../../releases) page.

🔽 **[Download: TicTacToe\_v1.0.2\_x64\_Setup.exe](https://github.com/BHUTUU/TicTacToe/releases/download/TicTacToe_v1.0.2/TicTacToe_v1.0.2_x64_Setup.exe)**

> Simply run the installer and launch the game from the Start menu or desktop shortcut.

---

## 🎮 How to Play

* Game starts with player **X**.
* Click on a cell to make a move.
* Player with 3 consecutive marks (row/column/diagonal) wins.
* If all cells are filled and no winner, it's a **draw**.
* **Optional:** Check the "Play with Computer" box to play against a simple AI.
* Game auto-restarts after a win or draw.
* Use **Undo/Redo** via:

  * Buttons at the top-right corner
  * Keyboard shortcuts: `Ctrl+Z` (Undo), `Ctrl+Y` (Redo)

---

## 🧠 Logic Summary

* The board is stored as a 2D array `string[,] board`.
* Each cell click triggers game logic:

  * Save state for undo
  * Update board
  * Check win/draw
  * Switch turns
* The **computer AI**:

  * First tries to win
  * Then blocks opponent's win
  * Otherwise picks first available cell
* **Undo/Redo** stacks track board history
* Grid and symbols drawn using **GDI+**
* Score and game logs stored and optionally displayed

---

## 🛠️ Future Improvements

* Add difficulty levels for AI
* Save/load scoreboard and game history between sessions
* Sound toggle option in UI
* Add multiplayer over LAN

---

## 📄 License

This project is open source and available under the [MIT License](https://github.com/BHUTUU/TicTacToe/blob/main/LICENSE).
