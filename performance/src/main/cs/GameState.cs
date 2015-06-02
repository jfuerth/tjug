/*
 * C# implementation of the peg solitaire solver.
 * C# translation by Jeffrey Mo
 * Resturctured for dnx by Jonathan Fuerth
 * 
 * License from the original Java version:
 *
 * Copyright (c) 2010, Jonathan Fuerth
 * 
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in
 *       the documentation and/or other materials provided with the
 *       distribution.
 *     * Neither the name of Jonathan Fuerth nor the names of other
 *       contributors may be used to endorse or promote products derived
 *       from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
 * OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Represents a state of the board in a game of "peg solitaire." The board is an
/// equilateral triangle arranged thusly:
/// <pre>
/// Row
/// 1     *
/// 2    * *
/// 3   * * *    (this diagram depicts a 5-row board)
/// 4  * * * *
/// 5 * * * * *
/// </pre>
/// 
/// Each asterisk represents a "hole" which can be in the "occupied" (contains a
/// peg) or "unoccupied" (no peg) state. Note that each row has the same number
/// of holes as its rank: row 1 has 1 hole, row 2 has 2 holes, and so on.
/// <p>
/// Given a board state, there are zero or more legal moves by which a different
/// board state can be reached.
/// </summary>
public class GameState
{
    private readonly int rowCount;
    private readonly List<Coordinate> occupiedHoles;

    /// <summary>
    /// Creates a new game state with all holes occupied except the one given. 
    /// On a board with 5 rows, row 3 hole 2 is the traditional choice for the
    /// empty hole.
    /// </summary>
    /// <param name="rows"/>
    /// <param name="emptyHole"/>
    public GameState(int rows, Coordinate emptyHole)
    {
        this.rowCount = rows;
        occupiedHoles = new List<Coordinate>();
        for (int row = 1; row <= rows; row++)
        {
            for (int hole = 1; hole <= row; hole++)
            {
                Coordinate peg = new Coordinate(row, hole);
                if (!peg.Equals(emptyHole))
                {
                    occupiedHoles.Add(peg);
                }
            }
        }
    }

    /// <summary>
    /// Creates a new game state by applying the given move to the given starting
    /// state. Verifies the move's validity, throwing an exception if the move is
    /// illegal.
    /// </summary>
    /// <param name="initialState"></param>
    /// <param name="applyMe"></param>
    private GameState(GameState initialState, Move applyMe)
    {
        rowCount = initialState.rowCount;
        occupiedHoles = new List<Coordinate>(initialState.occupiedHoles);
        if (!occupiedHoles.Remove(applyMe.GetFrom()))
        {
            throw new ArgumentException(
                    "Move is not consistent with game state: 'from' hole was unoccupied.");
        }
        if (!occupiedHoles.Remove(applyMe.GetJumped()))
        {
            throw new ArgumentException(
                    "Move is not consistent with game state: jumped hole was unoccupied.");
        }
        if (occupiedHoles.Contains(applyMe.GetTo()))
        {
            throw new ArgumentException(
                    "Move is not consistent with game state: 'to' hole was occupied.");
        }
        if (applyMe.GetTo().GetRow() > rowCount || applyMe.GetTo().GetRow() < 1)
        {
            throw new ArgumentException(
                    "Move is not legal because the 'to' hole does not exist: " + applyMe.GetTo());
        }
        occupiedHoles.Add(applyMe.GetTo());
    }

    public List<Move> LegalMoves()
    {
        List<Move> legalMoves = new List<Move>();
        foreach (Coordinate c in occupiedHoles)
        {
            ICollection<Move> possibleMoves = c.PossibleMoves(rowCount);

            foreach (Move m in possibleMoves)
            {
                if (occupiedHoles.Contains(m.GetJumped()) && !occupiedHoles.Contains(m.GetTo()))
                {
                    legalMoves.Add(m);
                }
            }
        }
        return legalMoves;
    }

    public GameState Apply(Move move)
    {
        return new GameState(this, move);
    }

    public int PegsRemaining()
    {
        return occupiedHoles.Count;
    }

    /// <summary>
    /// Returns the full board state in a multiline string arranged to resemble a
    /// real board. '*' characters signify occupied holes, and 'O' characters
    /// signify empty ones.
    /// </summary>
    public override String ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Game with " + PegsRemaining() + " pegs:" + "\n");
        for (int row = 1; row <= rowCount; row++)
        {
            int indent = rowCount - row;
            for (int i = 0; i < indent; i++)
            {
                sb.Append(" ");
            }
            for (int hole = 1; hole <= row; hole++)
            {
                if (occupiedHoles.Contains(new Coordinate(row, hole)))
                {
                    sb.Append(" *");
                }
                else
                {
                    sb.Append(" O");
                }
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }

}
