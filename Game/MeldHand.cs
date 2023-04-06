using System;

namespace Game;

public class MeldHand<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private LinkedList<Card<S, R, T, U>> _cards;
    private Card<S, R, T, U>? _firstCard;
    private Card<S, R, T, U>? _rightmost;
    private MeldType _type;
    private bool _canWrap;

    public MeldHand(bool canWrap) {
        this._cards = new LinkedList<Card<S, R, T, U>>();
        this._firstCard = null;
        this._rightmost = null;
        this._type = MeldType.Undefined;
        this._canWrap = canWrap;
    }

    private LinkedList<Card<S, R, T, U>> GetCards() {
        return this._cards;
    }

    private Card<S, R, T, U>? GetFirstCard() {
        return this._firstCard;
    }

    private void SetFirstCard(Card<S, R, T, U> firstCard) {
        this._firstCard = firstCard;
    }

    private Card<S, R, T, U>? GetRightmost() {
        return this._rightmost;
    }

    private void SetRightmost(Card<S, R, T, U> rightmost) {
        this._rightmost = rightmost;
    }

    private MeldType GetMeldType() {
        return this._type;
    }

    private void SetMeldType(MeldType type) {
        this._type = type;
    }

    private bool GetCanWrap() {
        return this._canWrap;
    }

    private bool CanAddToSet(Card<S, R, T, U> c) {
        Card<S, R, T, U>? insider = this.GetRightmost();
        if (insider == null) {
            throw new BadOperationException("can add to set");
        }

        if (insider.CompareRank(c) != 0) {
            return false;
        }

        foreach (Card<S, R, T, U> aux in this.GetCards()) {
            if (aux.CompareSuit(c) == 0) {
                return false;
            }
        }

        return true;
    }

    private bool ValidSetLength() {
        return this.GetCards().Count < OrderedEnum<T>.GetNumEnums();
    }

    private bool CanAddToRunLast(Card<S, R, T, U> c) {
        Card<S, R, T, U>? insider = this.GetRightmost();
        if (insider == null) {
            throw new BadOperationException("can add to run last");
        }

        return insider.IsPredecessor(c, this.GetCanWrap());
    }

    private bool CanAddToRunFirst(Card<S, R, T, U> c) {
        Card<S, R, T, U>? insider = this.GetRightmost();
        if (insider == null) {
            throw new BadOperationException("can add to run first");
        }

        return c.IsPredecessor(insider, this.GetCanWrap());
    }

    private bool ValidRunLength() {
        return this.GetCanWrap() ||
            this.GetCards().Count < OrderedEnum<U>.GetNumEnums();
    }

    // check the type of the meld to know if the added card
    // was enough to form a concrete type of meld.
    // the bool tells us if adding c is done because its valid.
    /// <summary>
    /// If adding c in the meld keeps the meld in a consistent
    /// state, add it. Otherwise don't.
    /// </summary>
    /// <returns>
    /// Returns true if adding c in the meld keeps it a valid
    /// meld of either kind.
    /// </returns>
    /// <param name="c">
    /// True.
    /// </param>
    public bool AddLast(Card<S, R, T, U> c) {
        Card<S, R, T, U>? first = this.GetFirstCard();
        // Card is a joker.
        if (c.IsJoker() &&
            (this.ValidSetLength() || this.ValidRunLength()))
        {
            if (first != null) {
                this.GetRightmost().Next();
            }
            this.GetCards().AddLast(c);
            return true;
        }

        // Card is the first non-joker.
        if (first == null) {
            this.SetRightmost(c);
            this.SetFirstCard(c);
            this.GetCards().AddLast(c);
            return false;
        }

        // Card determines the meld will be a set.
        if (this.CanAddToSet(c) &&
            this.ValidSetLength())
        {
            this.SetMeldType(MeldType.Set);
            this.GetCards().AddLast(c);
            return true;
        }

        // Card determines the meld will be a run.
        if (this.CanAddToRunLast(c) &&
            this.ValidRunLength())
        {
            this.GetRightmost().Next();
            this.SetMeldType(MeldType.Run);
            this.GetCards().AddLast(c);
            return true;
        }

        // Adding card would form an invalid meld.
        return false;
    }
}
