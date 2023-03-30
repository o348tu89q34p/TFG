namespace Game;

public class Card<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private S _suit;
    private R _rank;
    private bool _isJoker;
    /*
      For the score we could have something on player that asks
      for the type of the card to this class and determines
      what the score is there but there is nothing explicitly
      stored in here.
     */

    /// <param name=rank>Human readable name for a rank or "Joker".</param>
    /// <param name=suit>Human readable name for a suit or "Joker".</param>
    /// <returns>Card suit that represents the suit indicated by the name received or the joker if both values are "Joker".</returns>
    /// <exception cref="InvalidArgumentException">The names provided do not represent a valid rank, suit or joker in the suit.</exception>
    public Card(string suit, string rank) {
        if (suit.Equals("Joker") && rank.Equals("Joker")) {
            _suit = (S)new OrderedEnum<T>(0);
            _rank = (R)new OrderedEnum<U>(0);
            _isJoker = true;
        } else {
            _suit = (S)new OrderedEnum<T>(suit);
            _rank = (R)new OrderedEnum<U>(rank);
            _isJoker = false;
        }
    }

    /// <param name=nDecks>Number of full decks to generate.</param>
    /// <param name=nJokers>Number of joker cards to add to the deck.</param>
    /// <returns>Returns a nDecks decks with their cards ordered and one deck after the other and nJokers added at the end. Returns null if any argument is a negative number or if there is a problem creating a card.</returns>
    public static Card<S, R, T, U>[]? buildDecks(int nDecks, int nJokers) {
        if (nDecks < 0 || nJokers < 0) {
            return null;
        }

        int nSuits = OrderedEnum<T>.GetNumEnums();
        int nRanks = OrderedEnum<U>.GetNumEnums();

        Card<S, R, T, U>[] all = new Card<S, R, T, U>[nDecks*nSuits*nRanks + nJokers];
        int pos = 0;
        string suitName;
        string rankName;

        for (int i = 0; i < nDecks; i++) {
            for (int j = 0; j < nSuits; j++) {
                suitName = (new OrderedEnum<T>(j)).GetName();
                for (int k = 0; k < nRanks; k++) {
                    try {
                        rankName = (new OrderedEnum<U>(k)).GetName();
                        all[pos] = new Card<S, R, T, U>(suitName, rankName);
                        pos++;
                    } catch (ArgumentException) {
                        return null;
                    }
                }
            }
        }

        for (int i = 0; i < nJokers; i++) {
            all[pos] = new Card<S, R, T, U>("Joker", "Joker");
            pos++;
        }

        return all;
    }

    private bool GetJoker() {
        return this._isJoker;
    }

    private S? GetSuit() {
        return this._suit;
    }

    private R? GetRank() {
        return this._rank;
    }

    private S? SetSuit(S suit) {
        return this._suit = suit;
    }

    private R? SetRank(R rank) {
        return this._rank = rank;
    }

    /// <summary>Change this card into the next in the natural order. If it's the last on the deck or the joker it stays the same.</summary>
    public void Next() {
        var thisRank = this.GetRank();
        var thisSuit = this.GetSuit();

        if (this.IsJoker() || thisRank == null || thisSuit == null) {
            return;
        }

        if (thisRank.IsLast() && thisSuit.IsLast()) {
            return;
        }

        if (thisRank.IsLast()) {
            thisSuit.NextWrap();
        }

        // It's safe to call wrap because we just checked for last.
        thisRank.NextWrap();
    }

    /// <summary>Change this card into the next in the natural order wrapping around to the beginning. if it's the joker it stays the same.</summary>
    public void NextWrap() {
        S? thisSuit = this.GetSuit();
        R? thisRank = this.GetRank();

        if (this.IsJoker() || thisSuit == null || thisRank == null) {
            return;
        }

        thisRank.NextWrap();
        thisRank.NextWrap();
    }

    /// <summary>Increases the rank of the card by one. If the rank is the highest or the card is a joker, nothing happens.</summary>
    public void NextInSuit() {
        R? thisRank = this.GetRank();

        if (this.IsJoker() || thisRank == null) {
            return;
        }

        thisRank.Next();
    }

    /// <summary>Increases the rank of the card by one. If the rank is the highest it is set to the lowest. If the card is a joker nothing happens.</summary>
    public void NextInSuitWrap() {
        R? thisRank = this.GetRank();

        if (this.IsJoker() || thisRank == null) {
            return;
        }

        thisRank.NextWrap();
    }

    /// <param name=c>True.</param>
    /// <returns>
    /// Returns 0 if this and c represent the same card.
    /// Returns a negative number if this has a suit that is
    /// less than c or if the suit is the same, this' rank is
    /// lower than c's. Returns a positive number in any other
    /// combination of suit and rank. Returns null when
    /// comparing a joker with a regular card.
    /// </returns>
    public int? CompareTo(Card<S, R, T, U> c) {
        if (this.IsJoker() && c.IsJoker()) {
            return 0;
        }

        S? thisSuit = this.GetSuit();
        R? thisRank = this.GetRank();
        S? thatSuit = c.GetSuit();
        R? thatRank = c.GetRank();

        if (this.IsJoker() || c.IsJoker() ||
            thisSuit == null || thisRank == null ||
            thatSuit == null || thatRank == null)
        {
            return null;
        }

        int cmp = thisSuit.CompareTo(thatSuit);
        if (cmp == 0) {
            return thisRank.CompareTo(thatRank);
        } else {
            return cmp;
        }
    }

    /// <param name=c>True.</param>
    /// <returns>
    /// Returns 0 if this and c have the same suit. Returns a negative value if the suit in this is smaller than c's suit and 1 if its greater. Returns null when involving jokers.
    /// </returns>
    public int? CompareSuit(Card<S, R, T, U> c) {
        if (this.IsJoker() || c.IsJoker()) {
            return null;
        }

        S? thisSuit = this.GetSuit();
        S? thatSuit = c.GetSuit();

        if (thisSuit == null || thatSuit == null) {
            return null;
        }

        return thisSuit.CompareTo(thatSuit);
    }

    /// <param name=c>True.</param>
    /// <returns>
    /// Returns 0 if this and c have the same rank. Returns a negative value if the rank in this is smaller than c's rank and 1 if its greater. Returns null when involving jokers.
    /// </returns>
    public int? CompareRank(Card<S, R, T, U> c) {
        if (this.IsJoker() || c.IsJoker()) {
            return null;
        }

        R? thisRank = this.GetRank();
        R? thatRank = c.GetRank();

        if (thisRank == null || thatRank == null) {
            return null;
        }

        return thisRank.CompareTo(thatRank);
    }

    /// <returns>Null if the card is the joker, true if the card has the highest rank within its suit.</returns>
    public bool? IsLastInSuit() {
        R? thisRank = this.GetRank();

        if (thisRank == null) {
            return null;
        }

        return thisRank.IsLast();
    }

    /// <returns>Null if the card is the joker, true if the card has the lowest rank within its suit.</returns>
    public bool? IsFirstInSuit() {
        S? thisSuit = this.GetSuit();

        if (thisSuit == null) {
            return null;
        }

        return thisSuit.IsLast();
    }

    /// <returns>True if the card represents the joker, false otherwise.</returns>
    public bool IsJoker() {
        return this.GetJoker();
    }

    /// <returns>String representation of the card.</returns>
    public override string ToString() {
        if (this.IsJoker()) {
            return "Joker";
        }

        S? thisSuit = this.GetSuit();
        R? thisRank = this.GetRank();
        if (thisSuit == null || thisRank == null) {
            return "Malformed card";
        }
        else {
            return thisRank.GetName() + " of " + thisSuit.GetName();
        }
    }
}
