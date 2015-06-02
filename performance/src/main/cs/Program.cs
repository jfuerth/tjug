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
/// This is the test driver for the other classes in this package. It creates an
/// initial game state and then searches that board's entire state space,
/// collecting the winning sequences of moves and final board states as it goes.
/// </summary>
public class Program
{
    private long gamesPlayed;
    private List<List<Move>> solutions = new List<List<Move>>();
    private long startTime;
    private long endTime;

    private void search(GameState gs, Stack<Move> moveStack)
    {
        if (gs.PegsRemaining() == 1)
        {
            //            System.out.println("Found a winning sequence. Final state:");
            //            System.out.println(gs);

            solutions.Add(new List<Move>(moveStack));

            gamesPlayed++;

            return;
        }

        List<Move> legalMoves = gs.LegalMoves();

        if (legalMoves.Count == 0)
        {
            gamesPlayed++;
            return;
        }

        foreach (Move m in legalMoves)
        {
            GameState nextState = gs.Apply(m);
            moveStack.Push(m);
            search(nextState, moveStack);
            moveStack.Pop();
        }
    }


    public void run()
    {
        startTime = DateTime.Now.Ticks; // 1 "Tick" = 100 nanoseconds
        GameState gs = new GameState(5, new Coordinate(3, 2));
        search(gs, new Stack<Move>());
        endTime = DateTime.Now.Ticks;

        Console.WriteLine(String.Format("Games played:    {0,6:d}", gamesPlayed));
        Console.WriteLine(String.Format("Solutions found: {0,6:d}", solutions.Count));
        Console.WriteLine(String.Format("Time elapsed:    {0,6:d}ms", (endTime - startTime) / 10000));
    }

    public static void Main(string[] args)
    {
        new Program().run();
    }
}
