#!/usr/bin/ruby

class Move
	def initialize(from, jumped, to)
		print("Creating new move #{from} -> #{jumped} -> #{to}\n")
		@from = from
		@jumped = jumped
		@to = to
		
		freeze
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
		
		freeze
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

	attr_reader :rowCount

	def initialize(rows, emptyHole, initialState=nil, applyMe=nil)
	
		if initialState && applyMe
			# top-secret "apply move" initializer
			@rowCount = initialState.rowCount
			@occupiedHoles = initialState.occupiedHoles.dup
			if !occupiedHoles.delete applyMe.from
				raise "Move is not consistent with game state: 'from' hole was unoccupied."
			end
			
			if !occupiedHoles.delete applyMe.jumped
				raise "Move is not consistent with game state: jumped hole was unoccupied."
			end
			
			if occupiedHoles.member? applyMe.to
				raise "Move is not consistent with game state: 'to' hole was occupied."
			end
        
			if (applyMe.to.row > @rowCount || applyMe.to.row < 1)
				raise "Move is not legal because the 'to' hole does not exist: #{applyMe.to}"
			end
			
			occupiedHoles.push applyMe.to

		else
			# standard public initializer: fill board except for one hole
			@rowCount = rows
			@occupiedHoles = []
			for row in 1..rows
				for hole in 1..row
					peg = Coordinate.new(row, hole)
					if !(peg == emptyHole)
						@occupiedHoles.push peg
					end
				end
			end
		end
		
		occupiedHoles.freeze
		freeze
    end
    
    def legalMoves()
        legalMoves = []
        for c in @occupiedHoles
            possibleMoves = c.possibleMoves(@rowCount)
            
			for m in possibleMoves
                if @occupiedHoles.contains(m.jumped) && !@occupiedHoles.contains(m.to)
                    legalMoves.push m
                end
            end
        end
        return legalMoves;
    end
    
    def apply(move)
        return GameState.new(nil, nil, self, move)
    end

    def pegsRemaining()
        return @occupiedHoles.length
	end

	def to_s()
        sb = ""
        sb.concat("Game with #{pegsRemaining} pegs:\n")
        for row in 1..@rowCount
            indent = @rowCount - row
            for i in 0...indent
                sb.concat(" ")
            end
            for hole in 1..row
                if (@occupiedHoles.member? Coordinate.new(row, hole))
                    sb.concat(" *")
                else
                    sb.concat(" O")
                end
            end
            sb.concat("\n")
        end
        return sb
    end
	
	protected
	
	# the top-secret copy constructor needs access to the original game state's
	# list of occupied holes
	def occupiedHoles
		@occupiedHoles
	end
end

c1 = Coordinate.new(4,1)
c2 = Coordinate.new(3,1)
c3 = Coordinate.new(2,1)

gs = GameState.new(5, Coordinate.new(2,1))
puts
puts gs

m = Move.new(c1, c2, c3)
gs2 = gs.apply(m)
puts
puts gs2

puts "original:"
puts gs

puts gs.occupiedHoles
