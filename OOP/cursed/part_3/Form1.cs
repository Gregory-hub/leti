using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace part_3
{
    public partial class Form1 : Form
    {
        private ChessBoard chessBoard = new ChessBoard();
        private Piece pieceSelected = null;
        private int[] pieceSelectedCoords = new int[2];
        private Bot bot = new Bot();
        private bool gameStarted = false;
        private bool gameEnded = false;

        public Form1()
        {
            InitializeComponent();
            Text = "Checkers";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chessBoard.Initialize(Checkers.Color.White);
            chessBoard.InitLogger();
            for (int x = 0; x < boardLayoutPanel.ColumnCount; x++)
            {
                for (int y = 0; y < boardLayoutPanel.RowCount; y++)
                {
                    Button button = new Button();
                    button.Dock = DockStyle.Fill;
                    button.Margin = new Padding(0);
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    if ((x + y) % 2 == 1) button.BackColor = System.Drawing.Color.FromArgb(116, 102, 59);  // dark
                    else button.BackColor = System.Drawing.Color.FromArgb(211, 188, 141);                  // light
                    boardLayoutPanel.Controls.Add(button);
                    button.Click += Click_Board;
                }
            }
            DrawPieces();
        }

        private void DrawPieces()
        {
            for (int i = 0; i < chessBoard.GetLength(0); i++)
            {
                for (int j = 0; j < chessBoard.GetLength(1); j++)
                {
                    int x;
                    int y;
                    if (chessBoard.Orientation == Checkers.Color.White)
                    {
                        y = i;
                        x = j;
                    }
                    else
                    {
                        y = 7 - i;
                        x = 7 - j;
                    }

                    Button button = (Button)boardLayoutPanel.GetControlFromPosition(x, y);
                    button.FlatStyle = FlatStyle.Flat;
                    if (chessBoard[i, j] != null)
                    {
                        Piece checker = chessBoard[i, j];
                        button.Tag = checker;

                        Size size = button.Size;
                        size.Width -= size.Width / 10;
                        size.Height -= size.Height / 10;
                        button.Image = (Image)(new Bitmap(checker.Img, size));
                        button.ImageAlign = ContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        button.Image = null;
                        button.Tag = null;
                    }
                }
            }
        }

        private void ResetBoardButtons(ChessBoard board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    int x;
                    int y;
                    if (chessBoard.Orientation == Checkers.Color.White)
                    {
                        y = i;
                        x = j;
                    }
                    else
                    {
                        y = 7 - i;
                        x = 7 - j;
                    }

                    Button button = (Button)boardLayoutPanel.GetControlFromPosition(x, y);
                    button.FlatStyle = FlatStyle.Flat;
                }
        }

        private void Click_Board(object s, EventArgs e)
        {
            if (!gameStarted || gameEnded) return;

            ResetBoardButtons(chessBoard);
            if (!(s is Button)) return;
            Button button = (Button)s;
            TableLayoutPanelCellPosition pos = boardLayoutPanel.GetPositionFromControl((Control)s);
            if (chessBoard.Orientation == Checkers.Color.Black)
            {
                pos.Row = 7 - pos.Row;
                pos.Column = 7 - pos.Column;
            }

            if (!(button.Tag is Piece)) // empty square
            {
                if (pieceSelected != null)
                {
                    // move
                    int[] target = new int[] { pos.Row, pos.Column };
                    bool move_is_valid = chessBoard.Move(pieceSelectedCoords, target);
                    if (move_is_valid)
                    {
                        // capture chain
                        List<int[]> captures_available = pieceSelected.AvailableCaptures(chessBoard, target);
                        if (chessBoard.LastMoveTakes && captures_available.Any())
                        {
                            DrawPieces();
                            pieceSelected = chessBoard[target[0], target[1]];
                            pieceSelectedCoords = target;
                            button.FlatStyle = FlatStyle.Standard;

                            foreach (int[] capture in captures_available) HighlightSquare(capture);
                            return;
                        }

                        chessBoard.FlipCurrentMoveColor();
                        List<Move> moves = chessBoard.AvailableMoves();
                        if (moves.Count == 0)
                        {
                            WinGame();
                            return;
                        }
                        DrawPieces();

                        MakeBotMove();
                        chessBoard.FlipCurrentMoveColor();
                        moves = chessBoard.AvailableMoves();
                        if (moves.Count == 0)
                        {
                            LoseGame();
                            return;
                        }
                    }
                    DrawPieces();
                    pieceSelected = null;
                }
                return;
            }

            Piece piece = (Piece)button.Tag;

            if (pieceSelected != null && pieceSelected.Color != piece.Color)  // piece is already selected and colors are different
            {
                pieceSelected = null;
            }
            else if (piece.Color == chessBoard.Orientation)
            {
                pieceSelected = piece;
                pieceSelectedCoords[0] = pos.Row;
                pieceSelectedCoords[1] = pos.Column;

                List<Move> moves = chessBoard.AvailableMoves()
                    .Where(move => move.Source[0] == pieceSelectedCoords[0] && move.Source[1] == pieceSelectedCoords[1])
                    .ToList<Move>();

                if (moves.Any()) button.FlatStyle = FlatStyle.Standard;
                foreach (Move move in moves) HighlightSquare(move.Target);
            }
        }

        private void HighlightSquare(int[] sq)
        {
            int y = (chessBoard.Orientation == Checkers.Color.White) ? sq[0] : 7 - sq[0];
            int x = (chessBoard.Orientation == Checkers.Color.White) ? sq[1] : 7 - sq[1];
            Button actionButton = (Button)boardLayoutPanel.GetControlFromPosition(x, y);
            actionButton.FlatStyle = FlatStyle.Standard;
        }

        public void MakeBotMove(bool wait = true)
        {
            // bot move
            Move move = bot.GenerateMove(chessBoard);
            chessBoard.Move(move.Source, move.Target);
            if (wait) Wait(1000);
            DrawPieces();

            // capture chain
            List<int[]> captures_available = chessBoard[move.Target[0], move.Target[1]].AvailableCaptures(chessBoard, move.Target);
            while (chessBoard.LastMoveTakes && captures_available.Any())
            {
                move = bot.GenerateMove(chessBoard);
                chessBoard.Move(move.Source, move.Target);
                Wait(500);
                DrawPieces();
                captures_available = chessBoard[move.Target[0], move.Target[1]].AvailableCaptures(chessBoard, move.Target);
            }
        }

        private void WinGame()
        {
            gameEnded = true;
            label1.Text = "YOU WIN!";
            DrawPieces();
            chessBoard.LogGameResults("win");
        }

        private void LoseGame()
        {
            gameEnded = true;
            label1.Text = "YOU LOSE!";
            DrawPieces();
            chessBoard.LogGameResults("lose");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // change your color
            if (gameStarted) return;
            Checkers.Color orientation = (chessBoard.Orientation == Checkers.Color.White) ? Checkers.Color.Black : Checkers.Color.White;
            chessBoard.Initialize(orientation);
            DrawPieces();

            button1.Text = "Play as ";
            button1.Text += (chessBoard.Orientation == Checkers.Color.Black) ? "white" : "black";

            label1.Text = "You play ";
            label1.Text += (chessBoard.Orientation == Checkers.Color.Black) ? "black" : "white";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // start game
            if (gameStarted) return;
            gameStarted = true;
            label1.Text = "Game started";
            chessBoard.LogInitGame();
            if (chessBoard.Orientation == Checkers.Color.Black)
            {
                MakeBotMove(false);
                chessBoard.FlipCurrentMoveColor();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // reset
            chessBoard = new ChessBoard();
            chessBoard.Initialize(Checkers.Color.White);
            gameStarted = false;
            gameEnded = false;
            pieceSelected = null;
            DrawPieces();
            ResetFormControls();
            label1.Text = "Game reset";
        }

        private void ResetFormControls()
        {
            button1.Text = "Play as black";
            label1.Text = "";
        }

        public void Wait(int milliseconds)
        {
            var timer = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            timer.Interval = milliseconds;
            timer.Enabled = true;
            timer.Start();

            timer.Tick += (s, e) =>
            {
                timer.Enabled = false;
                timer.Stop();
            };

            while (timer.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
}

