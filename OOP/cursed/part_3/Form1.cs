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

        public Form1()
        {
            InitializeComponent();
            Text = "Checkers";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chessBoard.Initialize(Checkers.Color.White);
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
            DrawPieces(chessBoard);
        }

        private void DrawPieces(ChessBoard board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
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
                    if (board[y, x] != null)
                    {
                        Piece checker = board[y, x];
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

        private async void Click_Board(object s, EventArgs e)
        {
            ResetBoardButtons(chessBoard);
            if (!(s is Button)) return;
            Button button = (Button)s;
            TableLayoutPanelCellPosition pos = boardLayoutPanel.GetPositionFromControl((Control)s);

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
                            DrawPieces(chessBoard);
                            pieceSelected = chessBoard[target[0], target[1]];
                            pieceSelectedCoords = target;
                            button.FlatStyle = FlatStyle.Standard;
                            foreach (int[] capture in captures_available)
                            {
                                Button actionButton = (Button)boardLayoutPanel.GetControlFromPosition(capture[1], capture[0]);
                                actionButton.FlatStyle = FlatStyle.Standard;
                            }
                            return;
                        }

                        chessBoard.FlipCurrentMoveColor();
                        DrawPieces(chessBoard);

                        // bot move
                        Move move = bot.GenerateMove(chessBoard);
                        chessBoard.Move(move.Source, move.Target);
                        await Task.Delay(1000);
                        DrawPieces(chessBoard);

                        // capture chain
                        captures_available = chessBoard[move.Target[0], move.Target[1]].AvailableCaptures(chessBoard, move.Target);
                        while (chessBoard.LastMoveTakes && captures_available.Any())
                        {
                            move = bot.GenerateMove(chessBoard);
                            chessBoard.Move(move.Source, move.Target);
                            await Task.Delay(500);
                            DrawPieces(chessBoard);
                            captures_available = chessBoard[move.Target[0], move.Target[1]].AvailableCaptures(chessBoard, move.Target);
                        }
                        chessBoard.FlipCurrentMoveColor();
                    }
                    DrawPieces(chessBoard);
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
                foreach (Move move in moves)
                {
                    Button actionButton = (Button)boardLayoutPanel.GetControlFromPosition(move.Target[1], move.Target[0]);
                    actionButton.FlatStyle = FlatStyle.Standard;
                }
            }
        }

    }
}

