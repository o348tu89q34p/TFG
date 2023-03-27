namespace Game;

public class SpanishCard : Card {
    private OrderedEnum<SpanishRank>? _rank;
    private OrderedEnum<SpanishSuit>? _suit;
    private bool _isJoker;

    /// <param name=rank>Human readable name for a rank or "Joker".</param>
    /// <param name=suit>Human readable name for a suit or "Joker".</param>
    /// <returns>Card suit that represents the suit indicated by the name received or the joker if both values are "Joker".</returns>
    /// <exception cref="InvalidArgumentException">The names provided do not represent a valid rank, suit or joker in the OrderedEnum<Spanish suit.</exception>
    public SpanishCard(string rank, string suit) {
        if (rank.Equals("Joker") && suit.Equals("Joker")) {
            _rank = null;
            _suit = null;
            _isJoker = true;
        } else {
            _rank = new OrderedEnum<SpanishRank>(rank);
            _suit = new OrderedEnum<SpanishSuit>(suit);
            _isJoker = false;
        }
    }

    /// <summary>Private getter for the attribute rank.</summary>
    /// <returns>The value of the internal representation of a card's rank.</summary>
    private OrderedEnum<SpanishRank>? GetRank() {
        return this._rank;
    }

    /// <summary>Private getter for the attribute suit.</summary>
    /// <returns>The value of the internal representation of a card's suit.</summary>
    private OrderedEnum<SpanishSuit>? GetSuit() {
        return this._suit;
    }

    public Card? NextInPack() {
        if (this.IsJoker() ||
            this.GetRank() == null ||
            this.GetSuit() == null) {
            return null;
        }

        /*
        OrderedEnum<SpanishRank> thisRank> = this.GetRank>();
        OrderedEnum<SpanishSuit> thisSuit> = this.GetSuit>();
        OrderedEnum<SpanishRank> nextRank> = thisRank>.NextRank>();
        OrderedEnum<SpanishSuit> nextSuit> = thisSuit>.NextSuit>();
        OrderedEnum<SpanishRank> newRank>;
        OrderedEnum<SpanishSuit> newSuit>;

        if (nextRank> == null) {
            if (nextSuit> == null) {
                return null;
            }

            // newRank> = OrderedEnum<SpanishRank>.FirstRank>();
            newSuit> = nextSuit>;
            // Return new card.
        }
        // Maybe rank and suit need a generate first method.
        if (thisRank>.IsLastRank>()) {
            if (thisSuit>.IsLastSuit>()) {
                return null;
            }
            newRank> = thisRank>.NextRank>();
        }

        newSuit> = thisSuit>.NextSuit>()
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
