namespace Game;

public class BreaksRuleException : Exception {
    public BreaksRuleException() {
    }

    public BreaksRuleException(string i) :
        base(String.Format("Invalid rule for: {0}", i))
    {
    }
}

public class Rules {
    /* The player has to have a meld on the board to be able to pick a card form the discard pile. */
    private bool _needsOpen;
    /* The player can only get rid of their last card via discarding it, but not by melding or laying off. */
    private bool _discardLast;
    /* A player can lay off the corresponding card in place of a joker to pick up the joker. */
    private bool _canTakeJoker;
    // The game can't be won on the first turn.
    private bool _hasHonorRound;

    private int _numPlayers;
    private int _numDecks;
    private int _numJokers;
    private int _cardsPerPlayer;
    private int _maxTurns;
    //private int _maxRounds;


    public Rules() {
        _needsOpen = true;
        _discardLast = true;
        _canTakeJoker = true;

        _numPlayers = 1;
        _numDecks = 1;
        _numJokers = 1;
        _cardsPerPlayer = 1;

        _hasHonorRound = false;
        _maxTurns = -1;
    }

    private int GetNumPlayers() {
        return this._numPlayers;
    }

    private int GetNumDecks() {
        return this._numDecks;
    }

    private int GetNumJokers() {
        return this._numJokers;
    }

    private int GetCardsPerPlayer() {
        return this._cardsPerPlayer;
    }

    private bool GetNeedsOpen() {
        return this._needsOpen;
    }

    private int GetMaxTurns() {
        return this._maxTurns;
    }

    public int NumPlayers() {
        return this.GetNumPlayers();
    }

    public int NumDecks() {
        return this.GetNumDecks();
    }

    public int NumJokers() {
        return this.GetNumJokers();
    }

    public int CardsPerPlayer() {
        return this.GetCardsPerPlayer();
    }

    public bool NeedsOpen() {
        return this.GetNeedsOpen();
    }

    public bool HasHonorRound() {
        return this._hasHonorRound;
    }

    private bool PassedMaxTurns(int turns) {
        if (turns < 0) {
            return false;
        }

        return turns < this.GetMaxTurns();
    }

    public bool RoundOver(int turn) {
        // random code.
        _discardLast = turn < 12;
        _canTakeJoker = turn < 14;
        return PassedMaxTurns(turn);
    }
}
