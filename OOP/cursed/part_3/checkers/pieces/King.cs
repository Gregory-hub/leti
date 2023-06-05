using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class King : Piece
{
    public King(Checkers.Color color)
    {
        Color = color;

        string path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\images\\king_";
        if (color == Checkers.Color.White) path += "w.png";
        else path += "b.png";

        Img = Image.FromFile(path);
    }

    public override List<int[]> AvailableMoves(ChessBoard chessboard, int[] coords)
    {
        List<int[]> moves = new List<int[]>();

        int y = coords[0];
        int x = coords[1];
        for (int i = 1; i <= Math.Min(y, x); i++)
        {
            if (chessboard[y - i, x - i] == null) moves.Add(new int[2] { y - i, x - i });
            else break;
        }
        for (int i = 1; i <= Math.Min(7 - y, 7 - x); i++)
        {
            if (chessboard[y + i, x + i] == null) moves.Add(new int[2] { y + i, x + i });
            else break;
        }
        for (int i = 1; i <= Math.Min(y, 7 - x); i++)
        {
            if (chessboard[y - i, x + i] == null) moves.Add(new int[2] { y - i, x + i });
            else break;
        }
        for (int i = 1; i <= Math.Min(7 - y, x); i++)
        {
            if (chessboard[y + i, x - i] == null) moves.Add(new int[2] { y + i, x - i });
            else break;
        }

        moves.AddRange(AvailableCaptures(chessboard, coords));

        return moves;
    }

    public override List<int[]> AvailableCaptures(ChessBoard chessboard, int[] coords)
    {
        List<int[]> moves = new List<int[]>();

        int y0 = coords[0];
        int x0 = coords[1];

        int y = y0 - 1;
        int x = x0 - 1;
        while (y > 0 && x > 0)
        {
            if (chessboard[y, x] == null)
            {
                y--; x--;
                continue;
            }
            if (chessboard[y, x].Color == Color) break;

            y--; x--;
            while (y >= 0 && x >= 0 && chessboard[y, x] == null)
            {
                moves.Add(new int[] { y, x });
                y--; x--;
            }

            y--; x--;
        }

        y = y0 + 1;
        x = x0 - 1;
        while (y < 7 && x > 0)
        {
            if (chessboard[y, x] == null)
            {
                y++; x--;
                continue;
            }
            if (chessboard[y, x].Color == Color) break;

            y++; x--;
            while (y <= 7 && x >= 0 && chessboard[y, x] == null)
            {
                moves.Add(new int[] { y, x });
                y++; x--;
            }

            y++; x--;
        }

        y = y0 - 1;
        x = x0 + 1;
        while (y > 0 && x < 7)
        {
            if (chessboard[y, x] == null)
            {
                y--; x++;
                continue;
            }
            if (chessboard[y, x].Color == Color) break;

            y--; x++;
            while (y >= 0 && x <= 7 && chessboard[y, x] == null)
            {
                moves.Add(new int[] { y, x });
                y--; x++;
            }

            y--; x++;
        }

        x = x0 + 1;
        y = y0 + 1;
        while (y < 7 && x < 7)
        {
            if (chessboard[y, x] == null)
            {
                y++; x++;
                continue;
            }
            if (chessboard[y, x].Color == Color) break;

            y++; x++;
            while (y <= 7 && x <= 7 && chessboard[y, x] == null)
            {
                moves.Add(new int[] { y, x });
                y++; x++;
            }

            y++; x++;
        }

        return moves;
    }
}

