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

    /// <param name=rank>
    /// Human readable name for a rank or "Joker".
    /// </param>
    /// <param name=suit>
    /// Human readable name for a suit or "Joker".
    /// </param>
    /// <returns>
    /// Card that represents the suit and rank indicated by the
    /// names received or the joker if both values are "Joker".
    /// </returns>
    /// <exception cref="BadCardException">
    /// The names provided do not represent a valid rank, suit
    /// or joker in the suit.
    /// </exception>
    public Card(string suit, string rank) {
        try {
            if (suit.Equals("Joker") && rank.Equals("Joker")) {
                _suit = (S)new OrderedEnum<T>(0);
                _rank = (R)new OrderedEnum<U>(0);
                _isJoker = true;
            } else {
                _suit = (S)new OrderedEnum<T>(suit);
                _rank = (R)new OrderedEnum<U>(rank);
                _isJoker = false;
            }
        } catch (ArgumentException) {
            throw new BadCardException("card constructor");
        }
    }

    /// <param name="nDecks">
    /// Number of full decks to generate.
    /// </param>
    /// <param name="nJokers">
    /// Number of joker cards to add to the deck.
    /// </param>
    /// <returns>
    /// Returns a hand containing nDecks decks with their
    /// cards in ascending ordered and one deck after the
    /// other and nJokers added at the end.
    /// </returns>
    /// <exception cref="NegativeSizeException">
    /// Any of the parameters has a negative value.
    /// </exception>
    public static ArrayHand<Card<S, R, T, U>> buildDecks(int nDecks, int nJokers) {
        if (nDecks < 0 || nJokers < 0) {
            throw new NegativeSizeException("building deck");
        }

        int nSuits = OrderedEnum<T>.GetNumEnums();
        int nRanks = OrderedEnum<U>.GetNumEnums();

        int size = nDecks*nSuits*nRanks + nJokers;
        var deck = new ArrayHand<Card<S, R, T, U>>(size);
        string suitName;
        string rankName;
        Card<S, R, T, U> card;

        for (int i = 0; i < nDecks; i++) {
            for (int j = 0; j < nSuits; j++) {
                suitName = (new OrderedEnum<T>(j)).GetName();
                for (int k = 0; k < nRanks; k++) {
                    rankName = (new OrderedEnum<U>(k)).GetName();
                    card = new Card<S, R, T, U>(suitName, rankName);
                    deck.Append(card);
                }
            }
        }

        for (int i = 0; i < nJokers; i++) {
            card = new Card<S, R, T, U>("Joker", "Joker");
            deck.Append(card);
        }

        return deck;
    }

    private bool GetJoker() {
        return this._isJoker;
    }

    private S GetSuit() {
        if (this.IsJoker()) {
            throw new InvalidCardException("get suit");
        } else if (this._suit == null) {
            // A suit can be null in this implementation but it
            // is an error to call GetSet when that is the case.
            throw new CardBadStateException();
        }

        return this._suit;
    }

    private R GetRank() {
        if (this.IsJoker()) {
            throw new InvalidCardException("get rank");
        } else if (this._rank == null) {
            throw new CardBadStateException();
        }

        return this._rank;
    }

    private S SetSuit(S suit) {
        if (this.IsJoker()) {
            throw new InvalidCardException("set suit");
        }

        return this._suit = suit;
    }

    private R SetRank(R rank) {
        if (this.IsJoker()) {
            throw new InvalidCardException("set rank");
        }

        return this._rank = rank;
    }

    private bool IsFirstSuit() {
        if (this.IsJoker()) {
            throw new CardOperationException("is first suit");
        }

        return this.GetSuit().IsFirst();
    }

    private bool IsFirstRank() {
        if (this.IsJoker()) {
            throw new CardOperationException("is first rank");
        }

        return this.GetRank().IsFirst();
    }

    private bool IsLastSuit() {
        if (this.IsJoker()) {
            throw new CardOperationException("is last suit");
        }

        return this.GetSuit().IsLast();
    }

    private bool IsLastRank() {
        if (this.IsJoker()) {
            throw new CardOperationException("is last rank");
        }

        return this.GetRank().IsLast();
    }

    private bool IsFirst() {
        if (this.IsJoker()) {
            throw new CardOperationException("is first");
        }

        return this.IsFirstSuit() && this.IsFirstSuit();
    }

    private bool IsLast() {
        if (this.IsJoker()) {
            throw new CardOperationException("is last");
        }

        return this.IsLastSuit() && this.IsLastSuit();
    }

    /// <summary>
    /// Change this card into the next in the natural order.
    /// If it's the last on the deck or the joker it stays
    /// the same.
    /// </summary>
    /// <exception cref="CardOperationException">
    /// The card has no next value: it's a joker or is the
    /// last in the ordering.
    /// </exception>
    public void Next() {
        var thisRank = this.GetRank();
        var thisSuit = this.GetSuit();

        if (this.IsJoker() || this.IsLast()) {
            throw new CardOperationException("next");
        }

        if (thisRank.IsLast()) {
            thisSuit.Next();
        }

        thisRank.Next();
    }

    /// <summary>
    /// Change this card into the previous card in the natural
    /// order.
    /// If it's the frist on the deck or the joker it stays
    /// the same.
    /// </summary>
    /// <exception cref="CardOperationException">
    /// The card has no previous value: it's a joker or is the
    /// first in the ordering.
    /// </exception>
    public void Prev() {
        var thisRank = this.GetRank();
        var thisSuit = this.GetSuit();

        if (this.IsJoker() || this.IsLast()) {
            throw new CardOperationException("prev");
        }

        if (thisRank.IsFirst()) {
            thisSuit.Prev();
        }

        // Think about this.
        // and also about next.
        thisRank.PrevWrap();
    }

    /// <summary>
    /// Change this card into the next in the natural order
    /// wrapping around to the beginning. if it's the joker
    /// it stays the same.
    /// </summary>
    /// <exception cref="CardOperationException">
    /// The card has no next value: it's a joker.
    /// </exception>
    public void NextWrap() {
        S thisSuit = this.GetSuit();
        R thisRank = this.GetRank();

        if (this.IsJoker()) {
            throw new CardOperationException("next wrap");
        }

        if (thisRank.IsLast()) {
            thisSuit.NextWrap();
        }

        thisRank.NextWrap();
    }

    /// <summary>
    /// Increases the rank of the card by one.
    /// </summary>
    /// <exception cref="CardOperationException">
    /// The card has no next value: it's a joker.
    /// </exception>
    public void NextInSuit() {
        R thisRank = this.GetRank();

        if (thisRank == null) {
            throw new CardBadStateException("next in suit");
        }

        if (this.IsJoker()) {
            throw new CardOperationException("next in suit");
        }

        thisRank.Next();
    }

    /// <summary>
    /// Increases the rank of the card by one. If the rank is
    /// the highest it is set to the lowest. If the card is a
    /// joker nothing happens.
    /// </summary>
    /// <exception cref="CardOperationException">
    /// The card has no next value: it's a joker.
    /// </exception>
    public void NextInSuitWrap() {
        R thisRank = this.GetRank();

        if (thisRank == null) {
            throw new CardBadStateException("next in suit wrap");
        }

        if (this.IsJoker() || thisRank == null) {
            throw new CardOperationException("next in suit wrap");
        }

        thisRank.NextWrap();
    }

    /// <param name="c">Is not a joker.</param>
    /// <returns>
    /// Returns 0 if this and c represent the same card.
    /// Returns a negative number if this has a suit that is
    /// less than c or if the suit is the same, this' rank is
    /// lower than c's. Returns a positive number in any other
    /// combination of suit and rank. Jokers are the highest
    /// value.
    /// </returns>
    public int CompareTo(Card<S, R, T, U> c) {
        if (this.IsJoker() && c.IsJoker()) {
            return 0;
        }

        if (this.IsJoker()) {
            return 1;
        }

        if (c.IsJoker()) {
            return 0;
        }

        S thisSuit = this.GetSuit();
        R thisRank = this.GetRank();
        S thatSuit = c.GetSuit();
        R thatRank = c.GetRank();

        int cmp = thisSuit.CompareTo(thatSuit);

        if (cmp == 0) {
            return thisRank.CompareTo(thatRank);
        }

        return cmp;
    }

    /// <param name="c">Is not a joker.</param>
    /// <returns>
    /// Returns 0 if this and c have the same suit. Returns a
    /// negative value if the suit in this is smaller than c's
    /// suit and 1 if its greater.
    /// </returns>
    /// <exception cref="CardOperationException">
    /// The card is a joker.
    /// </exception>
    public int CompareSuit(Card<S, R, T, U> c) {
        if (this.IsJoker() || c.IsJoker()) {
            throw new CardOperationException("compare suit");
        }

        S thisSuit = this.GetSuit();
        S thatSuit = c.GetSuit();

        return thisSuit.CompareTo(thatSuit);
    }

    /// <param name=c>Is not a joker.</param>
    /// <returns>
    /// Returns 0 if this and c have the same rank. Returns a
    /// negative value if the rank in this is smaller than c's
    /// rank and 1 if its greater. Returns null when involving
    /// jokers.
    /// </returns>
    /// <exception cref="CardOperationException">
    /// The card is a joker.
    /// </exception>
    public int CompareRank(Card<S, R, T, U> c) {
        if (this.IsJoker() || c.IsJoker()) {
            throw new CardOperationException("compare rank");
        }

        R thisRank = this.GetRank();
        R thatRank = c.GetRank();

        return thisRank.CompareTo(thatRank);
    }

    /// <returns>
    /// True if the card has the highest rank within its suit.
    /// False otherwise.
    /// </returns>
    /// <exception cref="CardOperationException">
    /// The card is a joker.
    /// </exception>
    public bool IsLastInSuit() {
        if (this.IsJoker()) {
            throw new CardOperationException("last in suit");
        }

        R thisRank = this.GetRank();

        return thisRank.IsLast();
    }

    /// <returns>
    /// True if the card has the lowest rank within its suit.
    /// False otherwise.
    /// </returns>
    /// <exception cref="CardOperationException">
    /// The card is a joker.
    /// </exception>
    public bool IsFirstInSuit() {
        if (this.IsJoker()) {
            throw new CardOperationException("first in suit");
        }

        S thisSuit = this.GetSuit();

        return thisSuit.IsFirst();
    }

    /// <returns>
    /// True if the card represents the joker, false otherwise.
    /// </returns>
    public bool IsJoker() {
        return this.GetJoker();
    }

    /// <returns>
    /// String representation of the card.
    /// </returns>
    public override string ToString() {
        if (this.IsJoker()) {
            return "Joker";
        }

        S thisSuit = this.GetSuit();
        R thisRank = this.GetRank();

        return thisRank.GetName() + " of " + thisSuit.GetName();
    }

    public bool IsPredecessor(Card<S, R, T, U> c, bool canWrap) {
        var aux = Card<S, R, T, U>.Copy(this);

        try {
            if (canWrap) {
                aux.NextWrap();
            } else {
                aux.Next();
            }
            return aux.Equals(c);
        } catch {
            return false;
        }
    }

    public static Card<S, R, T, U> Copy(Card<S, R, T, U> c) {
        if (c.IsJoker()) {
            return new Card<S, R, T, U>("Joker", "Joker");
        }

        string s = c.GetSuit().GetName();
        string r = c.GetRank().GetName();

        return new Card<S, R, T, U>(s, r);
    }

    public bool Equals(Card<S, R, T, U> c) {
        if (this.IsJoker() && c.IsJoker()) {
            return true;
        }

        if (this.IsJoker()) {
            return false;
        }

        if (c.IsJoker()) {
            return false;
        }

        return this.GetSuit().Equals(c.GetSuit()) &&
            this.GetRank().Equals(c.GetRank());
    }
}
