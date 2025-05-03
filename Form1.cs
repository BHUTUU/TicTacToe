//<<<-----------Imports----------->>>
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        //<<<-----------UI Components----------->>>
        private readonly Label title;
        private readonly Label turnLabel;
        private readonly CheckBox playWithComputer;
        private readonly Button undoButton;
        private readonly Button redoButton;

        //<<<-----------Game State Variables----------->>>
        private string currentPlayer = "X";
        private string[,] board = new string[3, 3];
        private Rectangle[,] cellRects = new Rectangle[3, 3];
        private readonly Stack<string[,]> undoStack = new();
        private readonly Stack<string[,]> redoStack = new();
        private int gridSize = 300;
        private int cellSize;
        private int gridLeft, gridTop;

        //<<<-----------Constructor----------->>>
        public Form1()
        {
            InitializeComponent();
            this.Text = "Tic Tac Toe";
            this.Width = 900;
            this.Height = 550;
            this.BackColor = Color.White;

            //<<<-----------Event Handlers----------->>>
            this.Resize += Form1_Resize;
            this.Paint += Form1_Paint;
            this.MouseClick += Form1_MouseClick;
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            //<<<-----------Title Label----------->>>
            title = new Label
            {
                Text = "TIC TAC TOE",
                Font = new Font("Comic Sans MS", 28, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Black
            };
            this.Controls.Add(title);

            //<<<-----------Turn Label----------->>>
            turnLabel = new Label
            {
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(turnLabel);

            //<<<-----------Computer Mode Checkbox----------->>>
            playWithComputer = new CheckBox
            {
                Text = "Play with Computer",
                Font = new Font("Arial", 12),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(playWithComputer);

            //<<<-----------Undo Button----------->>>
            undoButton = new Button
            {
                Text = "Undo (Ctrl+Z)",
                Font = new Font("Arial", 10),
                Location = new Point(this.ClientSize.Width - 180, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            undoButton.Click += (s, e) => Undo();
            this.Controls.Add(undoButton);

            //<<<-----------Redo Button----------->>>
            redoButton = new Button
            {
                Text = "Redo (Ctrl+Y)",
                Font = new Font("Arial", 10),
                Location = new Point(this.ClientSize.Width - 90, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            redoButton.Click += (s, e) => Redo();
            this.Controls.Add(redoButton);

            //<<<-----------Initialize Game----------->>>
            ResetGame();
            CenterUI();
        }

        //<<<-----------Keyboard Shortcut Handling----------->>>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z) Undo();
            else if (e.Control && e.KeyCode == Keys.Y) Redo();
        }

        //<<<-----------Resize Handler----------->>>
        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
            CenterUI();
        }

        //<<<-----------Center UI Elements----------->>>
        private void CenterUI()
        {
            title.Location = new Point((this.ClientSize.Width - title.Width) / 2, 20);
            turnLabel.Text = $"Turn: {currentPlayer}";
            turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
            gridLeft = (this.ClientSize.Width - gridSize) / 2;
            gridTop = (this.ClientSize.Height - gridSize) / 2 + 20;
            cellSize = gridSize / 3;

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    cellRects[row, col] = new Rectangle(gridLeft + col * cellSize, gridTop + row * cellSize, cellSize, cellSize);
        }

        //<<<-----------Draw Grid and Symbols----------->>>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            using Pen pen = new Pen(Color.Black, 6)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };

            for (int i = 1; i < 3; i++)
            {
                g.DrawLine(pen, gridLeft + i * cellSize, gridTop, gridLeft + i * cellSize, gridTop + gridSize);
                g.DrawLine(pen, gridLeft, gridTop + i * cellSize, gridLeft + gridSize, gridTop + i * cellSize);
            }

            Font drawFont = new Font("Comic Sans MS", 36, FontStyle.Bold);
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (!string.IsNullOrEmpty(board[row, col]))
                        g.DrawString(board[row, col], drawFont, Brushes.Black, cellRects[row, col], sf);
        }

        //<<<-----------Handle Mouse Clicks----------->>>
        private async void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (cellRects[row, col].Contains(e.Location) && string.IsNullOrEmpty(board[row, col]))
                    {
                        SaveUndoState();
                        board[row, col] = currentPlayer;
                        this.Invalidate();

                        if (CheckWin(currentPlayer)) { ShowWinner(currentPlayer); return; }
                        if (IsDraw()) { MessageBox.Show("It's a draw!", "Draw", MessageBoxButtons.OK, MessageBoxIcon.Information); ResetGame(); return; }

                        currentPlayer = (currentPlayer == "X") ? "O" : "X";
                        turnLabel.Text = $"Turn: {currentPlayer}";
                        turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);

                        if (playWithComputer.Checked && currentPlayer == "O")
                        {
                            await Task.Delay(30);
                            ComputerMove();
                        }
                        return;
                    }
        }

        //<<<-----------Save Current Board for Undo----------->>>
        private void SaveUndoState()
        {
            undoStack.Push(CloneBoard(board));
            redoStack.Clear();
        }

        //<<<-----------Undo Logic----------->>>
        private void Undo()
        {
            if (undoStack.Count == 0) return;

            if (undoStack.Count >= 2)
            {
                redoStack.Push(CloneBoard(board));
                redoStack.Push(CloneBoard(undoStack.Peek()));
                board = undoStack.Pop();
                board = undoStack.Pop();
            }
            else
            {
                redoStack.Push(CloneBoard(board));
                board = undoStack.Pop();
            }

            currentPlayer = "X";
            turnLabel.Text = $"Turn: {currentPlayer}";
            turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
            this.Invalidate();
        }

        //<<<-----------Redo Logic----------->>>
        private void Redo()
        {
            if (redoStack.Count == 0) return;

            if (redoStack.Count >= 2)
            {
                undoStack.Push(CloneBoard(board));
                undoStack.Push(CloneBoard(redoStack.Peek()));
                board = redoStack.Pop();
                board = redoStack.Pop();
            }
            else
            {
                undoStack.Push(CloneBoard(board));
                board = redoStack.Pop();
            }

            currentPlayer = "X";
            turnLabel.Text = $"Turn: {currentPlayer}";
            turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
            this.Invalidate();
        }

        //<<<-----------Clone Board for Stack----------->>>
        private string[,] CloneBoard(string[,] src)
        {
            string[,] clone = new string[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    clone[i, j] = src[i, j];
            return clone;
        }

        //<<<-----------Computer Move Logic----------->>>
        private void ComputerMove()
        {
            SaveUndoState();

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (string.IsNullOrEmpty(board[row, col]))
                    {
                        board[row, col] = "O";
                        if (CheckWin("O")) { this.Invalidate(); ShowWinner("O"); return; }
                        board[row, col] = null;
                    }

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (string.IsNullOrEmpty(board[row, col]))
                    {
                        board[row, col] = "X";
                        if (CheckWin("X")) { board[row, col] = "O"; FinalizeComputerTurn(); return; }
                        board[row, col] = null;
                    }

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (string.IsNullOrEmpty(board[row, col]))
                    {
                        board[row, col] = "O";
                        FinalizeComputerTurn();
                        return;
                    }
        }

        //<<<-----------Finalize Computer Turn----------->>>
        private void FinalizeComputerTurn()
        {
            this.Invalidate();
            if (CheckWin("O")) { ShowWinner("O"); return; }
            if (IsDraw()) { MessageBox.Show("It's a draw!", "Draw", MessageBoxButtons.OK, MessageBoxIcon.Information); ResetGame(); return; }
            currentPlayer = "X";
            turnLabel.Text = $"Turn: {currentPlayer}";
            turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
        }

        //<<<-----------Win Check----------->>>
        private bool CheckWin(string player)
        {
            for (int i = 0; i < 3; i++)
                if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
                    (board[0, i] == player && board[1, i] == player && board[2, i] == player))
                    return true;

            return (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
                   (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player);
        }

        //<<<-----------Draw Check----------->>>
        private bool IsDraw()
        {
            foreach (var cell in board)
                if (string.IsNullOrEmpty(cell))
                    return false;
            return true;
        }

        //<<<-----------Show Win Message----------->>>
        private void ShowWinner(string winner)
        {
            DialogResult result = MessageBox.Show($"{winner} wins!", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
                ResetGame();
        }

        //<<<-----------Reset the Game----------->>>
        private void ResetGame()
        {
            currentPlayer = "X";
            board = new string[3, 3];
            undoStack.Clear();
            redoStack.Clear();
            turnLabel.Text = $"Turn: {currentPlayer}";
            turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
            this.Invalidate();
        }
    }
}
