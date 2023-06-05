using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class Checker : Piece
{
    public Checker(Checkers.Color color)
    {
        Color = color;

        string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\images\\checker_";
        if (color == Checkers.Color.White) path += "w.png";
        else path += "b.png";

        Img = Image.FromFile(path);
    }

    public override List<int[]> AvailableMoves(ChessBoard chessboard, int[] coords)
    {
        List<int[]> moves = new List<int[]>();

        int y = coords[0];
        int x = coords[1];
        if (chessboard.CurrentMoveColor == Checkers.Color.Black && y < 7) 
        {
            if (x > 0 && chessboard[y + 1, x - 1] == null) moves.Add(new int[] { y + 1, x - 1 });
            if (x < 7 && chessboard[y + 1, x + 1] == null) moves.Add(new int[] { y + 1, x + 1 });
        }
        else if (chessboard.CurrentMoveColor == Checkers.Color.White && y > 0) 
        {
            if (x > 0 && chessboard[y - 1, x - 1] == null) moves.Add(new int[] { y - 1, x - 1 });
            if (x < 7 && chessboard[y - 1, x + 1] == null) moves.Add(new int[] { y - 1, x + 1 });
        }

        moves.AddRange(AvailableCaptures(chessboard, coords));

        return moves;
    }

    public override List<int[]> AvailableCaptures(ChessBoard chessboard, int[] coords)
    {
        List<int[]> moves = new List<int[]>();
        Piece piece_taken;

        int y = coords[0];
        int x = coords[1];
        if (y < 6) 
        {
            if (x > 1)
            {
                piece_taken = chessboard[y + 1, x - 1];
                if (piece_taken != null && piece_taken.Color != Color && chessboard[y + 2, x - 2] == null)
                    moves.Add(new int[] { y + 2, x - 2 });
            }
            if (x < 6)
            {
                piece_taken = chessboard[y + 1, x + 1];
                if (piece_taken != null && piece_taken.Color != Color && chessboard[y + 2, x + 2] == null)
                    moves.Add(new int[] { y + 2, x + 2 });
            }
        }

        if (y > 1) 
        {
            if (x > 1)
            {
                piece_taken = chessboard[y - 1, x - 1];
                if (piece_taken != null && piece_taken.Color != Color && chessboard[y - 2, x - 2] == null)
                    moves.Add(new int[] { y - 2, x - 2 });
            }
            if (x < 6)
            {
                piece_taken = chessboard[y - 1, x + 1];
                if (piece_taken != null && piece_taken.Color != Color && chessboard[y - 2, x + 2] == null)
                    moves.Add(new int[] { y - 2, x + 2 });
            }
        }

        return moves;
    }
}

