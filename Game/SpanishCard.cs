namespace Game;

public class SpanishCard : Card {
    private SpanishRank? _rank;
    private SpanishSuit? _suit;
    private bool _isJoker;

    /// <param name=rank>Human readable name for a rank or "Joker".</param>
    /// <param name=suit>Human readable name for a suit or "Joker".</param>
    /// <returns>Card suit that represents the suit indicated by the name received or the joker if both values are "Joker".</returns>
    /// <exception cref="InvalidArgumentException">The names provided do not represent a valid rank, suit or joker in the Spanish suit.</exception>
    public SpanishCard(string rank, string suit) {
        if (rank.Equals("Joker") && suit.Equals("Joker")) {
            _rank = null;
            _suit = null;
            _isJoker = true;
        } else {
            _rank = new SpanishRank(rank);
            _suit = new SpanishSuit(suit);
            _isJoker = false;
        }
    }

    /// <summary>Private getter for the attribute rank.</summary>
    /// <returns>The value of the internal representation of a card's rank.</summary>
    private SpanishRank? GetRank() {
        return this._rank;
    }

    /// <summary>Private getter for the attribute suit.</summary>
    /// <returns>The value of the internal representation of a card's suit.</summary>
    private SpanishSuit? GetSuit() {
        return this._suit;
    }

    public Card? NextInPack() {
        if (this.IsJoker() ||
            this.GetRank() == null ||
            this.GetSuit() == null) {
            return null;
        }

        /*
        SpanishRank thisRank = this.GetRank();
        SpanishSuit thisSuit = this.GetSuit();
        SpanishRank nextRank = thisRank.NextRank();
        SpanishSuit nextSuit = thisSuit.NextSuit();
        SpanishRank newRank;
        SpanishSuit newSuit;

        if (nextRank == null) {
            if (nextSuit == null) {
                return null;
            }

            // newRank = SpanishRank.FirstRank();
            newSuit = nextSuit;
            // Return new card.
        }
        // Maybe rank and suit need a generate first method.
        if (thisRank.IsLastRank()) {
            if (thisSuit.IsLastSuit()) {
                return null;
            }
            newRank = thisRank.NextRank();
        }

        newSuit = thisSuit.NextSuit()
        */

        return this;
    }

    public Card? NextInPackWrap() {

        return this;
    }

    public Card? NextInSuit() {
        return this;
    }

    public Card? NextInSuitWrap() {
        return this;
    }

    public int? CompareTo(Card c) {
        return 1;
    }

    public int? CompareSuit(Card c) {
        return 1;
    }

    public int? CompareRank(Card c) {
        return 1;
    }

    public bool? IsLastInSuit() {
        return 3 == 2;
    }

    public bool? IsFirstInSuit() {
        return 3 == 2;
    }

    public bool IsJoker() {
        return _isJoker;
    }

    // rage adds
}
