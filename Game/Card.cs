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
    /// <exception cref="ArgumentException">
    /// The names provided do not represent a valid rank, suit
    /// or joker in the suit.
    /// </exception>
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
            throw new CardBadKindException("get suit");
        } else if (this._suit == null) {
            throw new CardBadStateException();
        }

        return this._suit;
    }

    private R GetRank() {
        if (this.IsJoker()) {
            throw new CardBadKindException("get rank");
        } else if (this._rank == null) {
            throw new CardBadStateException();
        }

        return this._rank;
    }

    private S SetSuit(S suit) {
        if (this.IsJoker()) {
            throw new CardBadKindException("set suit");
        }

        return this._suit = suit;
    }

    private R SetRank(R rank) {
        if (this.IsJoker()) {
            throw new CardBadKindException("set rank");
        }

        return this._rank = rank;
    }

    private bool IsFirstSuit() {
        if (this.IsJoker()) {
            throw new CardBadKindException("is first suit");
        }

        return this.GetSuit().IsFirst();
    }

    private bool IsFirstRank() {
        if (this.IsJoker()) {
            throw new CardBadKindException("is first rank");
        }

        return this.GetRank().IsFirst();
    }

    private bool IsLastSuit() {
        if (this.IsJoker()) {
            throw new CardBadKindException("is last suit");
        }

        return this.GetSuit().IsLast();
    }

    private bool IsLastRank() {
        if (this.IsJoker()) {
            throw new CardBadKindException("is last rank");
        }

        return this.GetRank().IsLast();
    }

    private bool IsFirst() {
        if (this.IsJoker()) {
            throw new CardBadKindException("is first");
        }

        return this.IsFirstSuit() && this.IsFirstRank();
    }

    private bool IsLast() {
        if (this.IsJoker()) {
            throw new CardBadKindException("is last");
        }

        return this.IsLastSuit() && this.IsLastRank();
    }

    /// <summary>
    /// This card becomes the previous value in the cards'
    /// defined natural order. If wrap is true the last card
    /// wraps to the first.
    /// </summary>
    /// <param name="wrap">
    /// Boolean used to indicate if next on the last card makes
    /// it wrap to become the first.
    /// </param>
    /// <exception cref="CardBadKindException">
    /// The card is a joker.
    /// </exception>
    /// <exception cref="CardOperationException">
    /// The card is not a joker, it's last and wrap is false.
    /// </exception>
    public void Next(bool wrap) {
        if (this.IsJoker()) {
            throw new CardBadKindException("next");
        }

        var thisRank = this.GetRank();
        var thisSuit = this.GetSuit();

        if (this.IsLast() || !wrap) {
            throw new CardOperationException("next");
        }

        if (thisRank.IsLast()) {
            thisSuit.Next();
        }

        thisRank.Next();
    }

    /// <summary>
    /// This card becomes the previous value in the cards'
    /// defined natural order. If wrap is true the first card
    /// wraps to the last.
    /// </summary>
    /// <param name="wrap">
    /// Boolean used to indicate if next on the first card makes
    /// it wrap to become the first.
    /// </param>
    /// <exception cref="CardBadKindException">
    /// The card is a joker.
    /// </exception>
    /// <exception cref="CardOperationException">
    /// The card is not a joker, it's first and wrap is false.
    /// </exception>
    public void Prev(bool wrap) {
        if (this.IsJoker()) {
            throw new CardBadKindException("prev");
        }

        var thisRank = this.GetRank();
        var thisSuit = this.GetSuit();

        if (this.IsFirst() || !wrap) {
            throw new CardOperationException("prev");
        }

        if (thisRank.IsFirst()) {
            thisSuit.Prev();
        }

        thisRank.Prev();
    }

    /// <summary>
    /// Increases the rank of the card by one within its
    /// current suit. If warp is true, the last rank wraps to
    /// the first.
    /// </summary>
    /// <param name="wrap">
    /// Boolean used to indicate if next on the last suit
    /// makes it wrap to become the first.
    /// </param>
    /// <exception cref="CardBadKindException">
    /// This card is a joker.
    /// </exception>
    /// <exception cref="CardOperationException">
    /// This card is not a joker and its rank is not the last
    /// within its suit.
    /// </exception>
    public void NextInSuit(bool wrap) {
        if (this.IsJoker()) {
            throw new CardBadKindException("next in suit");
        }

        R thisRank = this.GetRank();

        if (thisRank.IsLast()) {
            throw new CardOperationException("next in suit");
        }

        thisRank.Next();
    }

    /// <summary>
    /// Decreases the rank of the card by one within its
    /// current suit. If warp is true, the first rank wraps to
    /// the last.
    /// </summary>
    /// <param name="wrap">
    /// Boolean used to indicate if previous on the first suit
    /// makes it wrap to become the last.
    /// </param>
    /// <exception cref="CardBadKindException">
    /// This card is a joker.
    /// </exception>
    /// <exception cref="CardOperationException">
    /// This card is not a joker and its rank is not the first
    /// within its suit.
    /// </exception>
    public void PrevInSuit(bool wrap) {
        if (this.IsJoker()) {
            throw new CardBadKindException("prev in suit");
        }

        R thisRank = this.GetRank();

        if (thisRank.IsFirst()) {
            throw new CardOperationException("prev in suit");
        }

        thisRank.Prev();
    }

    /// <param name="c">
    /// True.
    /// </param>
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

    /// <param name="c">
    /// Is not a joker.
    /// </param>
    /// <returns>
    /// Returns 0 if this and c have the same suit. Returns a
    /// negative value if the suit in this is smaller than c's
    /// suit and 1 if its greater.
    /// </returns>
    /// <exception cref="CardBadKindException">
    /// This or c are jokers.
    /// </exception>
    public int CompareSuit(Card<S, R, T, U> c) {
        if (this.IsJoker() || c.IsJoker()) {
            throw new CardBadKindException("compare suit");
        }

        S thisSuit = this.GetSuit();
        S thatSuit = c.GetSuit();

        return thisSuit.CompareTo(thatSuit);
    }

    /// <param name=c>
    /// Is not a joker.
    /// </param>
    /// <returns>
    /// Returns 0 if this and c have the same rank. Returns a
    /// negative value if the rank in this is smaller than c's
    /// rank and 1 if its greater. Returns null when involving
    /// jokers.
    /// </returns>
    /// <exception cref="CardBadKindException">
    /// This or c are jokers.
    /// </exception>
    public int CompareRank(Card<S, R, T, U> c) {
        if (this.IsJoker() || c.IsJoker()) {
            throw new CardBadKindException("compare rank");
        }

        R thisRank = this.GetRank();
        R thatRank = c.GetRank();

        return thisRank.CompareTo(thatRank);
    }

    /// <returns>
    /// True if the card has the highest rank within its suit.
    /// False otherwise.
    /// </returns>
    /// <exception cref="CardBadKindException">
    /// The card is a joker.
    /// </exception>
    public bool IsLastInSuit() {
        if (this.IsJoker()) {
            throw new CardBadKindException("last in suit");
        }

        R thisRank = this.GetRank();

        return thisRank.IsLast();
    }

    /// <returns>
    /// True if the card has the lowest rank within its suit.
    /// False otherwise.
    /// </returns>
    /// <exception cref="CardBadKindException">
    /// The card is a joker.
    /// </exception>
    public bool IsFirstInSuit() {
        if (this.IsJoker()) {
            throw new CardBadKindException("first in suit");
        }

        R thisRank = this.GetRank();

        return thisRank.IsFirst();
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

    /// <returns>
    /// Returns true if c correspons to the card that comes
    /// after this in the natural ordering of a deck. If wrap
    /// is true, the first card is next to the last.
    /// </returns>
    /// <param name="wrap">
    /// Boolean used to indicate that if this card is the last,
    /// the first card of the deck is considered the next.
    /// </param>
    /// <exception cref="CardBadKindException">
    /// This card or c are a joker.
    /// </exception>
    public bool IsNext(Card<S, R, T, U> c, bool wrap) {
        if (this.IsJoker() || c.IsJoker()) {
            throw new CardBadKindException("is next");
        }

        var aux = Card<S, R, T, U>.Copy(this);

        try {
            aux.Prev(wrap);
            return aux.Equals(c);
        } catch {
            return false;
        }
    }

    /// <returns>
    /// Returns true if c correspons to the card that comes
    /// before this in the natural ordering of a deck. If wrap
    /// is true, the last card is next to the first.
    /// </returns>
    /// <param name="wrap">
    /// Boolean used to indicate that if this card is the first,
    /// the last card of the deck is considered the next.
    /// </param>
    /// <exception cref="CardBadKindException">
    /// This card or c are a joker.
    /// </exception>
    public bool IsPrev(Card<S, R, T, U> c, bool wrap) {
        if (this.IsJoker() || c.IsJoker()) {
            throw new CardBadKindException("is next");
        }

        var aux = Card<S, R, T, U>.Copy(this);

        try {
            aux.Next(wrap);
            return aux.Equals(c);
        } catch {
            return false;
        }
    }

    /// <returns>
    /// A copy of c.
    /// </returns>
    /// <param name="c">
    /// True.
    /// </param>
    public static Card<S, R, T, U> Copy(Card<S, R, T, U> c) {
        if (c.IsJoker()) {
            return new Card<S, R, T, U>("Joker", "Joker");
        }

        string s = c.GetSuit().GetName();
        string r = c.GetRank().GetName();

        return new Card<S, R, T, U>(s, r);
    }

    /// <returns>
    /// True if this card and c represent the same card, false
    /// otherwise.
    /// </returns>
    /// <param name="c">
    /// True.
    /// </param>
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
