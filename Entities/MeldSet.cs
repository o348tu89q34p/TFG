using System;

namespace Entities {

public abstract class MeldSet<S, R, T, U> : Meld<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    Card<S, R, T, U> _first;
    private LinkedList<Card<S, R, T, U>> _jokers;

    public MeldSet(ArrayHand<S, R, T, U> cards,
                   bool multipleWc,
                   int minSize) :
        base(cards, multipleWc, minSize)
    {
        int nCards = cards.GetSize();
        if (nCards > this.MaxSize()) {
            throw new ArgumentException("too many cards");
        }

        var (first, _) = cards.FirstNatural();
        if (first == null) {
            throw new ArgumentException("no natural cards");
        }

        this._first = first;
        this._jokers = new LinkedList<Card<S, R, T, U>>();

        for (int i = 0; i < nCards; i++) {
            Card<S, R, T, U> aux = cards.CheckAt(i);
            if (aux.IsJoker()) {
                this._jokers.AddLast(aux);
            } else if (first.CompareRank(aux) != 0) {
                throw new ArgumentException("mixed ranks");
            } else if (this._cards.Contains(aux)) {
                throw new ArgumentException("duplicated cards");
            } else {
                this._cards.AddLast(aux);
            }
        }
    }

    protected override int MaxSize() {
        return OrderedEnum<T>.GetNumEnums();
    }

    private LinkedList<Card<S, R, T, U>> GetJokers() {
        return this._jokers;
    }

    protected override int GetSize() {
        return this.GetCards().Count + this.GetJokers().Count;
    }

    protected override int GetNumWilds() {
        return this.GetJokers().Count;
    }

    public bool IsFull() {
        return this.GetSize() == this.MaxSize();
    }

    private bool CanAddHand(ArrayHand<S, R, T, U> hand) {
        int handLen = hand.GetSize();
        if (this.GetSize() + handLen > this.MaxSize()) {
            return false;
        }

        if (!this.CanMultWc() &&
            (hand.GetNumWilds() + this.GetNumWilds()) > 1)
        {
            return false;
        }

        for (int i = 0; i < handLen; i++) {
            Card<S, R, T, U> aux = hand.CheckAt(i);
            if (aux.IsJoker()) {
                continue;
            }

            if (aux.CompareRank(this._first) != 0) {
                return false;
            }

            for (int j = i + 1; j < handLen; j++) {
                if (aux.CompareTo(hand.CheckAt(j)) == 0) {
                    return false;
                }
            }

            if (this.GetCards().Contains(aux)) {
                return false;
            }
        }

        return true;
    }

    // throws argument exception for mixed ranks and dupes.
    public override void Add(ArrayHand<S, R, T, U> arr) {
        if (!this.CanAddHand(arr)) {
            throw new ArgumentException("invalid addition to set");
        }

        for (int i = 0; i < arr.GetSize(); i++) {
            Card<S, R, T, U> c = arr.CheckAt(i);
            if (c.IsJoker()) {
                this.GetJokers().AddLast(c);
            } else {
                this.GetCards().AddLast(c);
            }
        }
    }

    private bool CanReplaceHand(ArrayHand<S, R, T, U> hand) {
        int handLen = hand.GetSize();

        if (handLen > this.GetNumWilds()) {
            return false;
        }

        if (hand.GetNumWilds() > 0) {
            return false;
        }

        for (int i = 0; i < handLen; i++) {
            Card<S, R, T, U> c = hand.CheckAt(i);
            if (c.CompareRank(this._first) != 0) {
                return false;
            }

            for (int j = i + 1; j < handLen; j++) {
                if (c.CompareTo(hand.CheckAt(j)) == 0) {
                    return false;
                }
            }

            if (this.GetCards().Contains(c)) {
                return false;
            }
        }

        return true;
    }

    public override void Replace(Card<S, R, T, U> arr, int _) {
        /*
        if (!this.CanReplaceHand(arr)) {
            throw new ArgumentException("bad hand");
        }

        for (int i = 0; i < arr.GetSize(); i++) {
            this.GetCards().AddLast(arr.CheckAt(i));
            this.GetJokers().RemoveLast();
        }
        */
    }
}
}
