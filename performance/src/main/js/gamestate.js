function GameState(rowCount, emptyHole) {
	occupiedHoles = [];
	for (int row = 1; row <= rowCount; row++) {
		for (int hole = 1; hole <= row; hole++) {
			var peg = new Coordinate(row, hole);
			if (!peg.equals(emptyHole)) {
				occupiedHoles.push(peg);
			}
		}
	}

	this.applyMove = function(initialState, applyMe) {
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
	
	this.toString = function() {
		sb = "";
        sb.append("Game with " + pegsRemaining() + " pegs:\n");
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
            sb.append("\n");
        }
        return sb.toString();
    }

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

}