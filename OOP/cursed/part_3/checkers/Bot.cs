using System;
using System.Collections.Generic;

public class Bot
{
    private Random Rand;

    public Bot()
    {
        Rand = new Random(DateTime.Now.Millisecond);
    }


    public Move GenerateMove(ChessBoard chessboard)
    {
        List<Move> moves = chessboard.AvailableMoves();
        return moves[Rand.Next(moves.Count)];
    }
}

