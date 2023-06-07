using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public struct Move
{
    public int[] Source;
    public int[] Target;
}


public class ChessBoard
{
    public string Path;
    private Piece[,] boardArray = new Piece[8, 8];
    public Checkers.Color Orientation;
    public Checkers.Color CurrentMoveColor;
    public bool LastMoveTakes = false;

    public ChessBoard()
    {
        Path = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\protocol.txt";
    }

    public void Initialize(Checkers.Color orientation)
    {
        CurrentMoveColor = Checkers.Color.White;
        Orientation = orientation;
        for (int y = 0; y < 8; y++)
            for (int x = 0; x < 8; x++)
            {
                if ((x + y) % 2 == 1 && y != 3 && y != 4)
                {
                    Checkers.Color color = Checkers.Color.White;
                    if (y < 3) color = Checkers.Color.Black;
                    boardArray[y, x] = new Checker(color);
                }
                else boardArray[y, x] = null;
            }
    }

    public Piece this[int x, int y]
    {
        get { return boardArray[x, y]; }
    }

     public List<Move> AvailableMoves()
    {
        List<Move> moves = AvailableCaptures();
        if (moves.Count > 0) return moves;

        Move move;
        moves = new List<Move>();
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
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
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
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

    private void Take(int[] source, int[] target)
    {
        int y0 = target[0];
        int x0 = target[1];
        int y_sign = Math.Sign(source[0] - target[0]);
        int x_sign = Math.Sign(source[1] - target[1]);
        for(int i = 0; i < Math.Abs(source[0] - target[0]); i++)
        {
            boardArray[y0 + i * y_sign, x0 + i * x_sign] = null;
        }
    }

    public bool Move(int[] source, int[] target)
    {
        Piece piece = boardArray[source[0], source[1]];
        if (CanMove(piece, source, target))
        {
            LastMoveTakes = false;
            if (CanTake(piece, source, target))
            {
                Take(source, target);
                LastMoveTakes = true;
            }
            boardArray[source[0], source[1]] = null;
            boardArray[target[0], target[1]] = piece;

            if (target[0] == 0 && CurrentMoveColor == Checkers.Color.White) boardArray[target[0], target[1]] = new King(Checkers.Color.White);
            else if (target[0] == 7 && CurrentMoveColor == Checkers.Color.Black) boardArray[target[0], target[1]] = new King(Checkers.Color.Black);

            LogMove(source, target);

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

    public void InitLogger()
    {
        using (StreamWriter sw = new StreamWriter(Path, append: false))
        {
            sw.Write("Starting logger <");
            sw.WriteLine($"{DateTime.Now}>");
            sw.WriteLine();
        }
    }

    public void LogInitGame()
    {
        using (StreamWriter sw = new StreamWriter(Path, append: true))
        {
            sw.Write("\nNew game <");
            sw.WriteLine($"{DateTime.Now}>");
            sw.WriteLine("Human plays {0}", (Orientation == Checkers.Color.White) ? "white" : "black");
            sw.WriteLine("Bot plays {0}", (Orientation == Checkers.Color.Black) ? "white" : "black");
        }
    }

    private void LogMove(int[] source, int[] target)
    {
        using (StreamWriter sw = new StreamWriter(Path, append: true))
        {
            sw.Write("\nplayer: {0}, move: ", (CurrentMoveColor == Checkers.Color.White) ? "white" : "black");
            sw.Write("{0}{1}", "abcdefgh"[source[1]], 8 - source[0]);
            if (LastMoveTakes) sw.Write(':'); 
            else sw.Write('-');
            sw.Write("{0}{1}", "abcdefgh"[target[1]], 8 - target[0]);
        }
    }

    public void LogGameResults(string result)
    {
        using (StreamWriter sw = new StreamWriter(Path, append: true))
        {
            sw.WriteLine();
            switch (result.ToLower())
            {
                case "win":
                    sw.Write("Human wins");
                    break;
                case "lose":
                    sw.Write("Bot wins");
                    break;
                default:
                    throw new InvalidDataException();
            }
            sw.WriteLine();
        }
    }

}

