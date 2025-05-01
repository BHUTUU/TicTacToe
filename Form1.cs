using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tic_Tac_Toe
{
    public partial class Form1 : Form
    {
        //<<<-----------UI Elements and Game Variables----------->>>
        private Label title;
        private Label turnLabel;
        private string currentPlayer = "X";
        private string[,] board = new string[3, 3];
        private Rectangle[,] cellRects = new Rectangle[3, 3];
        private int gridSize = 300;
        private int cellSize;
        private int gridLeft, gridTop;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Tic Tac Toe";
            this.Width = 900;
            this.Height = 550;
            this.BackColor = Color.White;
            this.Resize += Form1_Resize;
            this.Paint += Form1_Paint;
            this.MouseClick += Form1_MouseClick;
            //<<<-----------Title Label----------->>>
            title = new Label
            {
                Text = "TIC TAC TOE",
                Font = new Font("Comic Sans MS", 28, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Black
            };
            this.Controls.Add(title);
            //<<<-----------Turn Indicator Label----------->>>
            turnLabel = new Label
            {
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.DarkBlue
            };
            this.Controls.Add(turnLabel);
            //<<<-----------Initialize Game----------->>>
            ResetGame();
            CenterUI();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            //<<<-----------Redraw UI on Resize----------->>>
            this.Invalidate();
            CenterUI();
        }
        private void CenterUI()
        {
            //<<<-----------Center Title and Turn Label----------->>>
            title.Location = new Point((this.ClientSize.Width - title.Width) / 2, 20);
            turnLabel.Text = $"Turn: {currentPlayer}";
            turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
            //<<<-----------Calculate Grid and Cell Rectangles----------->>>
            gridLeft = (this.ClientSize.Width - gridSize) / 2;
            gridTop = (this.ClientSize.Height - gridSize) / 2 + 20;
            cellSize = gridSize / 3;
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    cellRects[row, col] = new Rectangle(gridLeft + col * cellSize, gridTop + row * cellSize, cellSize, cellSize);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //<<<-----------Draw Grid----------->>>
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 6)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            for (int i = 1; i < 3; i++)
            {
                g.DrawLine(pen, gridLeft + i * cellSize, gridTop, gridLeft + i * cellSize, gridTop + gridSize);
                g.DrawLine(pen, gridLeft, gridTop + i * cellSize, gridLeft + gridSize, gridTop + i * cellSize);
            }
            //<<<-----------Draw X and O Symbols----------->>>
            Font drawFont = new Font("Comic Sans MS", 36, FontStyle.Bold);
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    if (!string.IsNullOrEmpty(board[row, col]))
                        g.DrawString(board[row, col], drawFont, Brushes.Black, cellRects[row, col], sf);
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //<<<-----------Handle Cell Click----------->>>
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (cellRects[row, col].Contains(e.Location) && string.IsNullOrEmpty(board[row, col]))
                    {
                        board[row, col] = currentPlayer;
                        this.Invalidate();
                        //<<<-----------Check for Win----------->>>
                        if (CheckWin(currentPlayer))
                        {
                            ShowWinner(currentPlayer);
                            return;
                        }
                        //<<<-----------Check for Draw----------->>>
                        if (IsDraw())
                        {
                            MessageBox.Show("It's a draw!", "Draw", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetGame();
                            return;
                        }
                        //<<<-----------Switch Turn----------->>>
                        currentPlayer = (currentPlayer == "X") ? "O" : "X";
                        turnLabel.Text = $"Turn: {currentPlayer}";
                        turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
                        return;
                    }
                }
            }
        }
        private bool CheckWin(string player)
        {
            //<<<-----------Check Rows and Columns----------->>>
            for (int i = 0; i < 3; i++)
                if ((board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) ||
                    (board[0, i] == player && board[1, i] == player && board[2, i] == player))
                    return true;
            //<<<-----------Check Diagonals----------->>>
            return (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) ||
                   (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player);
        }
        private bool IsDraw()
        {
            //<<<-----------Return True if All Cells Filled----------->>>
            foreach (var cell in board)
                if (string.IsNullOrEmpty(cell))
                    return false;
            return true;
        }
        private void ShowWinner(string winner)
        {
            //<<<-----------Show Winner Message and Reset----------->>>
            DialogResult result = MessageBox.Show($"{winner} wins!", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
                ResetGame();
        }
        private void ResetGame()
        {
            //<<<-----------Reset Game State----------->>>
            currentPlayer = "X";
            board = new string[3, 3];
            if (turnLabel != null)
            {
                turnLabel.Text = $"Turn: {currentPlayer}";
                turnLabel.Location = new Point((this.ClientSize.Width - turnLabel.Width) / 2, this.ClientSize.Height - 50);
            }
            this.Invalidate();
        }
    }
}
