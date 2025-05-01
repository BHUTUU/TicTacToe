# 🎮 Tic Tac Toe - Windows Forms Application

This is a classic **Tic Tac Toe** game implemented using **C#** and **Windows Forms**. It features a neat centered UI with dynamic resizing and a turn indicator. The game automatically detects wins and draws, and shows a popup message when the game ends.

## 🖼️ Features

- Smooth, centered 3x3 grid UI
- Turn indicator label showing whose move it is (`X` or `O`)
- Click-based cell selection
- Win and draw detection
- Winner popup with automatic restart
- Fully resizable window layout

## 💻 Technologies Used

- **C#**
- **.NET Windows Forms**
- **GDI+ Drawing API** for custom grid rendering

## 🚀 Getting Started

### Prerequisites

- Visual Studio (with .NET Desktop Development workload)
- .NET Framework or .NET SDK installed

### Running the App

1. Clone or download the repository.
2. Open the solution/project in **Visual Studio**.
3. Build and run (`F5`).

### How to Play

- The game starts with **X**.
- Players take turns clicking on the grid.
- If a player gets 3 in a row (horizontally, vertically, or diagonally), a popup will announce the winner.
- If all cells are filled and no one wins, it's a draw.
- The game restarts automatically after win or draw.

## 🧠 Logic Summary

- Grid and symbols are drawn manually using GDI+.
- The board is stored as a 2D `string[,]` array.
- Turn is switched after every valid move.
- Win and draw logic checks are performed after each move.


## 🛠️ Future Improvements

- Add a **Restart** button in the popup instead of auto-reset
- Add score tracking
- Add option to play against the computer (AI)

## 📄 License

This project is open source and available under the [MIT License](https://chatgpt.com/c/LICENSE).
