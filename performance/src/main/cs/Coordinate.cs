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

public class Coordinate
{
    private readonly int hole;
    private readonly int row;

    /// <summary>
    /// Creates a new coordinate instance. Arguments are checked to ensure they
    /// make sense according to the board's structure.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="hole"></param>
    /// <exception cref="System.ArgumentException">
    /// if <tt>hole &lt; 1</tt> or <tt>hole &gt; row</tt>.
    /// </exception>
    public Coordinate(int row, int hole)
    {
        if (hole < 1)
        {
            throw new System.ArgumentException("Illegal hole number: " + hole + " < 1");
        }
        if (hole > row)
        {
            throw new System.ArgumentException("Illegal hole number: " + hole + " > " + row);
        }
        this.hole = hole;
        this.row = row;
    }

    public int GetRow()
    {
        return row;
    }

    public int GetHole()
    {
        return hole;
    }

    /// <summary>
    /// Returns all possible moves from this coordinate, regardless of board 
    /// state.
    /// </summary>
    /// <param name="rowCount">
    /// the number of rows on the board to which the moves will be
    /// applied. This is needed in order to avoid returning moves
    /// that go past the bottom of your board.
    /// </param>

    public ICollection<Move> PossibleMoves(int rowCount)
    {
        List<Move> moves = new List<Move>();
        if (row >= 3)
        {
            // up-left
            if (hole >= 3)
            {
                moves.Add(new Move(
                        this,
                        new Coordinate(row - 1, hole - 1),
                        new Coordinate(row - 2, hole - 2)));
            }

            // up-right
            if (row - hole >= 2)
            {
                moves.Add(new Move(
                        this,
                        new Coordinate(row - 1, hole),
                        new Coordinate(row - 2, hole)));
            }
        }

        // leftward (needs at least 2 pegs to the left)
        if (hole >= 3)
        {
            moves.Add(new Move(
                    this,
                    new Coordinate(row, hole - 1),
                    new Coordinate(row, hole - 2)));
        }

        // rightward (needs at least 2 holes to the right)
        if (row - hole >= 2)
        {
            moves.Add(new Move(
                    this,
                    new Coordinate(row, hole + 1),
                    new Coordinate(row, hole + 2)));
        }

        // downward (needs at least 2 rows below)
        if (rowCount - row >= 2)
        {

            // down-left (always possible when there are at least 2 rows below)
            moves.Add(new Move(
                    this,
                    new Coordinate(row + 1, hole),
                    new Coordinate(row + 2, hole)));

            // down-right (always possible when there are at least 2 rows below)
            moves.Add(new Move(
                    this,
                    new Coordinate(row + 1, hole + 1),
                    new Coordinate(row + 2, hole + 2)));
        }

        return moves;
    }

    public override String ToString()
    {
        return "r" + row + "h" + hole;
    }

    public override int GetHashCode()
    {
        const int prime = 31;
        int result = 1;
        result = prime * result + hole;
        result = prime * result + row;
        return result;
    }

    public override bool Equals(Object obj)
    {
        if (this == obj)
            return true;
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        Coordinate other = (Coordinate)obj;
        if (hole != other.hole)
            return false;
        if (row != other.row)
            return false;
        return true;
    }
}
