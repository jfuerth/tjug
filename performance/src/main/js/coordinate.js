/* coordinate.js */

function Coordinate(row, hole) {

	if (hole < 1) {
		throw new IllegalArgumentException("Illegal hole number: " + hole + " < 1");
	}
	if (hole > row) {
		throw new IllegalArgumentException("Illegal hole number: " + hole + " on row " + row);
	}

	this.getRow = function() { return row; }
	this.getHole = function() { return hole; }
	
	this.possibleMoves = function(rowCount) {
		var moves = [];

		// upward (needs at least 2 rows above)
		if (row >= 3) {

			// up-left
			if (hole >= 3) {
				moves.push(new Move(
						this,
						new Coordinate(row - 1, hole - 1),
						new Coordinate(row - 2, hole - 2)));
			}

			// up-right
			if (row - hole >= 2) {
				moves.push(new Move(
						this,
						new Coordinate(row - 1, hole),
						new Coordinate(row - 2, hole)));
			}
		}

		// leftward (needs at least 2 pegs to the left)
		if (hole >= 3) {
			moves.push(new Move(
					this,
					new Coordinate(row, hole - 1),
					new Coordinate(row, hole - 2)));
		}

		// rightward (needs at least 2 holes to the right)
		if (row - hole >= 2) {
			moves.push(new Move(
					this,
					new Coordinate(row, hole + 1),
					new Coordinate(row, hole + 2)));
		}

		// downward (needs at least 2 rows below)
		if (rowCount - row >= 2) {

			// down-left (always possible when there are at least 2 rows below)
			moves.push(new Move(
					this,
					new Coordinate(row + 1, hole),
					new Coordinate(row + 2, hole)));

			// down-right (always possible when there are at least 2 rows below)
			moves.push(new Move(
					this,
					new Coordinate(row + 1, hole + 1),
					new Coordinate(row + 2, hole + 2)));
		}

		return moves;
	}

	this.toString = function() {
        return "r" + row + "h" + hole;
	}
};
