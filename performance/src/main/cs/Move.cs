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

/// <summary>
/// Represents a single move in the game. No effort is made to ensure the move is
/// </summary>
public class Move
{
    private readonly Coordinate from;
    private readonly Coordinate jumped;
    private readonly Coordinate to;

    /// <summary>
    /// </summary>
    /// <param name="from"></param>
    /// <param name="jumped"></param>
    /// <param name="to"></param>
    public Move(Coordinate from, Coordinate jumped, Coordinate to)
    {
        this.from = from;
        this.jumped = jumped;
        this.to = to;
    }

    public Coordinate GetFrom()
    {
        return from;
    }

    public Coordinate GetJumped()
    {
        return jumped;
    }

    public Coordinate GetTo()
    {
        return to;
    }

    public override String ToString()
    {
        return from + " -> " + jumped + " -> " + to;
    }

    public override int GetHashCode()
    {
        const int prime = 31;
        int result = 1;
        result = prime * result + ((from == null) ? 0 : from.GetHashCode());
        result = prime * result + ((jumped == null) ? 0 : jumped.GetHashCode());
        result = prime * result + ((to == null) ? 0 : to.GetHashCode());
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
        Move other = (Move)obj;
        if (from == null)
        {
            if (other.from != null)
                return false;
        }
        else if (!from.Equals(other.from))
            return false;
        if (jumped == null)
        {
            if (other.jumped != null)
                return false;
        }
        else if (!jumped.Equals(other.jumped))
            return false;
        if (to == null)
        {
            if (other.to != null)
                return false;
        }
        else if (!to.Equals(other.to))
            return false;
        return true;
    }
}
