// to create an empty game state, pass in 0 for row count
function GameState(rowCount, emptyHole) {

	this.rowCount = rowCount;
	
	// You MUST NOT modify this list or its coordinates
	// Only intended for internal use.
	this.occupiedHoles = [];

	// Returns true if array contains an object equal to the one given.
	// Comparisons are done using the equals() method of the objects.
	// If the objects don't have equals() methods, this operation fails.
	this.occupiedHoles.contains = function(coord) {
		var len = this.length;
		for (var i = 0; i < len; i++) {
			var coord2 = this[i];
			if (coord.equals(coord2)) {
				return true;
			}
		}
		return false;
	};

	// Removes the first object equal to the one given, returning true.
	// If no such object exists, returns false and leaves array unmodified.
	// Comparisons are done using the equals() method of the objects.
	// If the objects don't have equals() methods, this operation fails.
	this.occupiedHoles.remove = function(coord) {
		var len = this.length;
		for (var i = 0; i < len; i++) {
			var coord2 = this[i];
			if (coord.equals(coord2)) {
				this.splice(i, 1);
				return true;
			}
		}
		return false;
	};

	// top-secret constructor overload:
	// if rowCount is actually a game state and emptyHole is a move,
	// then apply the move
	if (rowCount instanceof GameState) {
		var oldgs = rowCount;
		var applyMe = emptyHole;
		
		this.rowCount = oldgs.rowCount;
		
		var len = oldgs.pegsRemaining();
		for (var i = 0; i < len; i++) {
			this.occupiedHoles[i] = oldgs.occupiedHoles[i];
		}
		
		if (!this.occupiedHoles.remove(applyMe.getFrom())) {
			throw new Error(
					"Move is not consistent with game state: 'from' hole was unoccupied.");
		}
		if (!this.occupiedHoles.remove(applyMe.getJumped())) {
			throw new Error(
					"Move is not consistent with game state: jumped hole was unoccupied.");
		}
		if (this.occupiedHoles.contains(applyMe.getTo())) {
			throw new Error(
					"Move is not consistent with game state: 'to' hole was occupied.");
		}
		if (applyMe.getTo().getRow() > this.rowCount || applyMe.getTo().getRow() < 1) {
			throw new Error(
					"Move is not legal because the 'to' hole does not exist: " + applyMe.getTo());
		}
		this.occupiedHoles.push(applyMe.getTo());
		
	} else {
		
		// normal public constructor
		for (var row = 1; row <= rowCount; row++) {
			for (var hole = 1; hole <= row; hole++) {
				var peg = new Coordinate(row, hole);
				if (!peg.equals(emptyHole)) {
					this.occupiedHoles.push(peg);
				}
			}
		}
	}

	/**
	 * Creates a new game state from this one, with the given move applied.
	 */
	this.applyMove = function(applyMe) {
		var afterMove = new GameState(this, applyMe);
//		console.log("afterMove=" + afterMove);
//		console.log("afterMove.occupiedHoles=" + afterMove.occupiedHoles);
//		console.log("afterMove.pegsRemaining=" + afterMove.pegsRemaining());
		return afterMove;
	}
	
	this.legalMoves = function() {
		var legalMoves = [];
		var ohLen = this.occupiedHoles.length;
		for (var i = 0; i < ohLen; i++) {
			var c = this.occupiedHoles[i];
			var possibleMoves = c.possibleMoves(this.rowCount);
			var pmLen = possibleMoves.length;
			for (var j = 0; j < pmLen; j++) {
				var m = possibleMoves[j];
				if (this.occupiedHoles.contains(m.getJumped()) && !this.occupiedHoles.contains(m.getTo())) {
					legalMoves.push(m);
				}
			}
		}
		return legalMoves;
	}
	
	this.pegsRemaining = function() {
		return this.occupiedHoles.length;
	}
	
	this.toString = function() {
		var sb = new Array;
        sb.push("Game with " + this.rowCount + " rows and " + this.pegsRemaining() + " pegs:\n");
        for (var row = 1; row <= this.rowCount; row++) {
            var indent = this.rowCount - row;
            for (var i = 0; i < indent; i++) {
                sb.push(" ");
            }
            for (var hole = 1; hole <= row; hole++) {
                if (this.occupiedHoles.contains(new Coordinate(row, hole))) {
                    sb.push(" *");
                } else {
                    sb.push(" O");
                }
            }
            sb.push("\n");
        }
        return sb.join("");
    }
}