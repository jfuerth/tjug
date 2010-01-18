#!/usr/bin/ruby

class Move
	def initialize(from, jumped, to)
		print("Creating new move #{from} -> #{jumped} -> #{to}\n")
		@from = from
		@jumped = jumped
		@to = to
	end
	
	def to_s
		return "#{from} -> #{jumped} -> #{to}"
	end
	
	attr_reader :from, :jumped, :to
	
end

class Coordinate
	def initialize(row, hole)
        raise "Illegal hole number: #{hole} < 1" if (hole < 1)
        raise "Illegal hole number: #{hole} on row #{row}" if (hole > row)
		@row = row
		@hole = hole
	end
	
    def possibleMoves(rowCount)
	    moves = []
        
        # upward (needs at least 2 rows above)
        if (@row >= 3)
            
            # up-left
            if (@hole >= 3)
                moves.push(Move.new(
                        self,
                        Coordinate.new(@row - 1, @hole - 1),
                        Coordinate.new(@row - 2, @hole - 2)))
            end
            
            # up-right
            if (@row - @hole >= 2)
                moves.push(Move.new(
                        self,
                        Coordinate.new(@row - 1, @hole),
                        Coordinate.new(@row - 2, @hole)))
            end
        end
        
        # leftward (needs at least 2 pegs to the left)
        if (@hole >= 3)
            moves.push(Move.new(
                    self,
                    Coordinate.new(@row, @hole - 1),
                    Coordinate.new(@row, @hole - 2)))
        end
        
        # rightward (needs at least 2 holes to the right)
        if (@row - @hole >= 2)
            moves.push(Move.new(
                    self,
                    Coordinate.new(@row, @hole + 1),
                    Coordinate.new(@row, @hole + 2)))
        end

        # downward (needs at least 2 rows below)
        if (rowCount - @row >= 2)
            
            # down-left (always possible when there are at least 2 rows below)
            moves.push(Move.new(
                    self,
                    Coordinate.new(@row + 1, @hole),
                    Coordinate.new(@row + 2, @hole)))
            
            # down-right (always possible when there are at least 2 rows below)
            moves.push(Move.new(
                    self,
                    Coordinate.new(@row + 1, @hole + 1),
                    Coordinate.new(@row + 2, @hole + 2)))
        end
        
        return moves
    end

	def to_s()
		return "r#{@row}h#{@hole}"
	end
	
	def eql?(other)
		@row == other.row && @hole == other.hole
	end

	def ==(other)
		eql?(other)
	end
	
	attr_reader :row, :hole
	
end

class GameState

	def initialize(rows, emptyHole)
        @rowCount = rows
        @occupiedHoles = []
        for row in 1..rows
            for hole in 1..row
                def peg = Coordinate.new(row, hole)
                if !(peg == emptyHole)
                    occupiedHoles.push peg
                end
            end
        end
    end

    private GameState(GameState initialState, Move applyMe) {
        rowCount = initialState.rowCount;
        occupiedHoles = new ArrayList<Coordinate>(initialState.occupiedHoles);
        if (!occupiedHoles.remove(applyMe.getFrom())) {
            throw new IllegalArgumentException(
                    "Move is not consistent with game state: 'from' hole was unoccupied.");
        }
        if (!occupiedHoles.remove(applyMe.getJumped())) {
            throw new IllegalArgumentException(
                    "Move is not consistent with game state: jumped hole was unoccupied.");
        }
        if (occupiedHoles.contains(applyMe.getTo())) {
            throw new IllegalArgumentException(
                    "Move is not consistent with game state: 'to' hole was occupied.");
        }
        if (applyMe.getTo().getRow() > rowCount || applyMe.getTo().getRow() < 1) {
            throw new IllegalArgumentException(
                    "Move is not legal because the 'to' hole does not exist: " + applyMe.getTo());
        }
        occupiedHoles.add(applyMe.getTo());
    }
    
    public List<Move> legalMoves() {
        List<Move> legalMoves = new ArrayList<Move>();
        for (Coordinate c : occupiedHoles) {
            Collection<Move> possibleMoves = c.possibleMoves(rowCount);
            Iterator<Move> it = possibleMoves.iterator();
            while (it.hasNext()) {
                Move m = it.next();
                if (occupiedHoles.contains(m.getJumped()) && !occupiedHoles.contains(m.getTo())) {
                    legalMoves.add(m);
                }
            }
        }
        return legalMoves;
    }
    
    public GameState apply(Move move) {
        return new GameState(this, move);
    }

    public int pegsRemaining() {
        return occupiedHoles.size();
    }

    /**
     * Returns the full board state in a multiline string arranged to resemble a
     * real board. '*' characters signify occupied holes, and 'O' characters
     * signify empty ones.
     */
    @Override
    public String toString() {
        final String nl = System.getProperty("line.separator");
        StringBuilder sb = new StringBuilder();
        sb.append("Game with " + pegsRemaining() + " pegs:" + nl);
        for (int row = 1; row <= rowCount; row++) {
            int indent = rowCount - row;
            for (int i = 0; i < indent; i++) {
                sb.append(" ");
            }
            for (int hole = 1; hole <= row; hole++) {
                if (occupiedHoles.contains(new Coordinate(row, hole))) {
                    sb.append(" *");
                } else {
                    sb.append(" O");
                }
            }
            sb.append(nl);
        }
        return sb.toString();
    }
end

c1 = Coordinate.new(2,1)
c2 = Coordinate.new(2,1)

print "c1: #{c1}\n"
print "c1 moves: #{c1.possibleMoves(5)}\n"
print "c2: #{c2}\n"

print "c2 == c1: #{c2 == c1}\n"
print "c2.eql?(c1): #{c2.eql?(c1)}\n"

a = [c1, c2]
print a
