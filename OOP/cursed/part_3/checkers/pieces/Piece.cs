using System;
using System.Collections.Generic;
using System.Drawing;

public abstract class Piece
{
    public Checkers.Color Color;
    public Image Img { get; set; }
    public abstract List<int[]> AvailableMoves(ChessBoard chessboard, int[] coords);
    public abstract List<int[]> AvailableCaptures(ChessBoard chessboard, int[] coords);
}

