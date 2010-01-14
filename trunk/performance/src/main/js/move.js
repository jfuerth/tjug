function Move(from, jumped, to) {
	
	this.getFrom = function() { return from; }
	this.getJumped = function() { return jumped; }
	this.getTo = function() { return to; }
	this.toString = function() {
        return from + " -> " + jumped + " -> " + to;
	}
};
