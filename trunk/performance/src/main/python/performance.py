#!/usr/bin/python

class Move:
    def __init__(self, fromh, jumped, to):
        self.fromh = fromh
        self.jumped = jumped
        self.to = to

    def get_fromh(self):
        return self.fromh

    def get_jumped(self):
        return self.jumped

    def get_to(self):
        return self.to

    fromh = property(get_fromh)
    jumped = property(get_jumped)
    to = property(get_to)

    def __str__(self):
        return str(self.fromh) + " -> " + str(self.jumped) + " -> " + str(self.to)


class Coordinate:
    def __init__(self, row, hole):
        if (hole < 1):
            raise RuntimeError, "Illegal hole number: " + hole + " < 1"
        if (hole > row):
            raise RuntimeError, "Illegal hole number: " + hole + " on row " + row
        self.hole = hole
        self.row = row

    def get_hole(self):
        return self.hole

    def get_row(self):
        return self.row

    hole = property(get_row)
    row = property(get_hole)

    def possibleMoves(self, rowCount):
        moves = []
        
        # upward (needs at least 2 rows above)
        if (self.row >= 3):
            
            # up-left
            if (self.hole >= 3):
                moves.append(Move(
                        self,
                        Coordinate(self.row - 1, self.hole - 1),
                        Coordinate(self.row - 2, self.hole - 2)))
            
            # up-right
            if (self.row - self.hole >= 2):
                moves.append(Move(
                        self,
                        Coordinate(self.row - 1, self.hole),
                        Coordinate(self.row - 2, self.hole)))
        
        # leftward (needs at least 2 pegs to the left)
        if (self.hole >= 3):
            moves.append(Move(
                    self,
                    Coordinate(self.row, self.hole - 1),
                    Coordinate(self.row, self.hole - 2)))
        
        # rightward (needs at least 2 holes to the right)
        if (self.row - self.hole >= 2):
            moves.append(Move(
                    self,
                    Coordinate(self.row, self.hole + 1),
                    Coordinate(self.row, self.hole + 2)))

        # downward (needs at least 2 rows below)
        if (rowCount - self.row >= 2):
            
            # down-left (always possible when there are at least 2 rows below)
            moves.append(Move(
                    self,
                    Coordinate(self.row + 1, self.hole),
                    Coordinate(self.row + 2, self.hole)))
            
            # down-right (always possible when there are at least 2 rows below)
            moves.append(Move(
                    self,
                    Coordinate(self.row + 1, self.hole + 1),
                    Coordinate(self.row + 2, self.hole + 2)))
        
        return moves
    
    def __str__(self):
        return "r" + str(self.row) + "h" + str(self.hole)

    def __eq__(self, other):
        return self.row == other.row and self.hole == other.hole


class GameState:

    def __init__(self, rows, emptyHole, initialState=None, applyMe=None):

        if initialState != None:
            # top-secret constructor overload for applying a move
            self.rowCount = initialState.rowCount
            self.occupiedHoles = []
            self.occupiedHoles.extend(initialState.occupiedHoles)
            
            self.occupiedHoles.remove(applyMe.fromh)
            self.occupiedHoles.remove(applyMe.jumped)

            try:
                self.occupiedHoles.index(applyMe.to)
                raise RuntimeError, "Move is not consistent with game state: 'to' hole was occupied."
            except ValueError:
                # this is the desired case ("to" hole unoccupied)
                pass

            if (applyMe.to.row > self.rowCount or applyMe.to.row < 1):
                raise RuntimeError, "Move is not legal because the 'to' hole does not exist: " + str(applyMe.to)

            self.occupiedHoles.append(applyMe.to)

        else:
            # normal constructor that sets up board
            self.rowCount = rows;
            self.occupiedHoles = []
            for row in range(1, rows + 1):
                for hole in range(1, row + 1):
                    peg = Coordinate(row, hole)
                    if (not peg == emptyHole):
                        self.occupiedHoles.append(peg)

    def legalMoves(self):
        legalMoves = []
        for c in self.occupiedHoles:
            possibleMoves = c.possibleMoves(self.rowCount);
            for m in possibleMoves:
                containsJumped = False
                try:
                    self.occupiedHoles.index(m.jumped)
                    containsJumped = True
                except ValueError:
                    pass

                containsTo = False
                try:
                    self.occupiedHoles.index(m.to)
                    containsTo = True
                except ValueError:
                    pass

                if containsJumped and not containsTo:
                    legalMoves.append(m)
                
        return legalMoves
    
    
    def applyMove(self, move):
        return GameState(None, None, self, move)

    def pegsRemaining(self):
        return len(self.occupiedHoles)

    def __str__(self):
        sb = []
        sb.append("Game with " + str(self.pegsRemaining()) + " pegs:\n")
        for row in range(1, self.rowCount + 1):
            indent = self.rowCount - row
            for i in range(0, indent):
                sb.append(" ")
            for hole in range(1, row + 1):
                try:
                    self.occupiedHoles.index(Coordinate(row, hole))
                    sb.append(" *")
                except ValueError:
                    sb.append(" O")
            sb.append("\n")
        return "".join(sb)


gamesPlayed = 0
solutions = []
    
def search(gs, moveStack):
    global gamesPlayed
    global solutions
    
    if (gs.pegsRemaining() == 1):
        #print("Found a winning sequence. Final state:")
        #print(gs);

        solutionCopy = []
        solutionCopy.extend(moveStack)
        solutions.append(solutionCopy)
        
        gamesPlayed += 1
        
        return
    
    legalMoves = gs.legalMoves()
    
    if (len(legalMoves) == 0):
        gamesPlayed += 1
        return
    
    for m in legalMoves:
        nextState = gs.applyMove(m)
        moveStack.append(m)
        search(nextState, moveStack)
        moveStack.pop()

from time import time

startTime = time()
gs = GameState(5, Coordinate(3, 2))
search(gs, [])
endTime = time()

print "Games played:    %6d" % (gamesPlayed)
print "Solutions found: %6d" % (len(solutions))
print "Time elapsed:    %6dms" % ((endTime - startTime) * 1000)
