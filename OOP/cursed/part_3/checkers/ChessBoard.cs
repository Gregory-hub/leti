using System;
using System.Linq;
using System.Collections.Generic;


public struct Move
{
    public int[] Source;
    public int[] Target;
}


public class ChessBoard
{
    private Piece[,] boardArray = new Piece[8, 8];
    public Checkers.Color Orientation;
    public Checkers.Color CurrentMoveColor;
    public bool LastMoveTakes = false;

    public void Initialize(Checkers.Color orientation)
    {
        CurrentMoveColor = Checkers.Color.White;
        Orientation = orientation;
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 == 1 && y != 3 && y != 4)
                {
                    Checkers.Color opponent_color = (CurrentMoveColor == Checkers.Color.White) ? Checkers.Color.Black : Checkers.Color.White;
                    Checkers.Color color = CurrentMoveColor;
                    if (y < 3) color = opponent_color;
                    boardArray[y, x] = new Checker(color);
                }
                else boardArray[y, x] = null;
            }
    }

    public Piece this[int x, int y]
    {
        get { return boardArray[x, y]; }
    }

    public int GetLength(int l)
    {
        return boardArray.GetLength(l);
    }

    public List<Move> AvailableMoves()
    {
        List<Move> moves = AvailableCaptures();
        if (moves.Count > 0) return moves;

        Move move;
        moves = new List<Move>();
        for (int y = 0; y < GetLength(0); y++)
        {
            for (int x = 0; x < GetLength(1); x++)
            {
                if (boardArray[y, x] != null && boardArray[y, x].Color == CurrentMoveColor)
                {
                    int[] source = new int[2] { y, x };
                    List<int[]> targets = boardArray[source[0], source[1]].AvailableMoves(this, source);
                    for (int i = 0; i < targets.Count; i++)
                    {
                        move = new Move() { Source = source, Target = targets[i] };
                        moves.Add(move);
                    }
                }
            }
        }
        return moves;
    }

    private List<Move> AvailableCaptures()
    {
        Move move;
        List<Move> moves = new List<Move>();
        for (int y = 0; y < GetLength(0); y++)
        {
            for (int x = 0; x < GetLength(1); x++)
            {
                if (boardArray[y, x] != null && boardArray[y, x].Color == CurrentMoveColor)
                {
                    int[] source = new int[2] { y, x };
                    List<int[]> targets = boardArray[source[0], source[1]].AvailableCaptures(this, source);
                    for (int i = 0; i < targets.Count; i++)
                    {
                        move = new Move() { Source = source, Target = targets[i] };
                        moves.Add(move);
                    }
                }
            }
        }
        return moves;
    }

    private bool CanMove(Piece piece, int[] source, int[] target)
    {
        List<Move> moves = AvailableMoves()
            .Where(move => move.Source[0] == source[0] && move.Source[1] == source[1])
            .Where(move => move.Target[0] == target[0] && move.Target[1] == target[1])
            .ToList<Move>();

        if (moves.Any()) return true;
        else return false;
    }

    private bool CanTake(Piece piece, int[] source, int[] target)
    {
        List<Move> moves = AvailableCaptures()
            .Where(move => move.Source[0] == source[0] && move.Source[1] == source[1])
            .Where(move => move.Target[0] == target[0] && move.Target[1] == target[1])
            .ToList<Move>();

        if (moves.Any()) return true;
        else return false;
    }

    private void Take(int[] source, in int[] target)
    {
        int y0 = target[0];
        int x0 = target[1];
        int y_sign = Math.Sign(source[0] - target[0]);
        int x_sign = Math.Sign(source[1] - target[1]);
        for(int i = 0; i < Math.Abs(source[0] - target[0]); i++)
        {
            boardArray[y0 + i * y_sign, x0 + i * x_sign] = null;
        }
        LastMoveTakes = true;
    }

    public bool Move(int[] source, int[] target)
    {
        Piece piece = boardArray[source[0], source[1]];
        if (CanMove(piece, source, target))
        {
            LastMoveTakes = false;
            if (CanTake(piece, source, target)) Take(source, target);
            boardArray[source[0], source[1]] = null;
            boardArray[target[0], target[1]] = piece;

            if (target[0] == 0 && CurrentMoveColor == Checkers.Color.White) boardArray[target[0], target[1]] = new King(Checkers.Color.White);
            else if (target[0] == 7 && CurrentMoveColor == Checkers.Color.Black) boardArray[target[0], target[1]] = new King(Checkers.Color.Black);

            return true;
        }
        // invalid move
        return false;
    }

    public void FlipCurrentMoveColor()
    {
        if (CurrentMoveColor == Checkers.Color.White) CurrentMoveColor = Checkers.Color.Black;
        else CurrentMoveColor = Checkers.Color.White;
    }
}

