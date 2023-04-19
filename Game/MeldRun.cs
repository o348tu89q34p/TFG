using System;

namespace Game;

public abstract class MeldRun<S, R, T, U> : Meld<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private Card<S, R, T, U> _first;
    private Card<S, R, T, U> _last;
    private bool _consecutiveWc;
    private bool _canWrap;
    private int _numWc;

    public MeldRun(ArrayHand<S, R, T, U> cards,
                   bool canWrap,
                   bool consecutiveWc,
                   bool multipleWc,
                   int minSize) :
        base(cards, multipleWc, minSize)
    {
        this._canWrap = canWrap;

        var (card, pos) = cards.FirstNatural();
        if (card == null) {
            throw new ArgumentException("no natural cards");
        }

        if (!consecutiveWc && cards.HasConsecutiveJokers()) {
            throw new ArgumentException("consecutive jokers");
        }
        this._consecutiveWc = consecutiveWc;

        var firstCard = Card<S, R, T, U>.Copy(card);
        for (int i = pos - 1; i >= 0; i--) {
            Card<S, R, T, U> aux = cards.CheckAt(i);
            firstCard.Prev(this._canWrap);
            if (aux.IsNatural() && !firstCard.Equals(aux)) {
                throw new ArgumentException("invalid run");
            }
        }
        this._first = firstCard;

        var lastCard = Card<S, R, T, U>.Copy(card);
        for (int i = pos + 1; i < cards.GetSize(); i++) {
            Card<S, R, T, U> aux = cards.CheckAt(i);
            lastCard.Next(this._canWrap);
            if (aux.IsNatural() && !lastCard.Equals(aux)) {
                throw new ArgumentException("invalid run");
            }
        }
        this._last = lastCard;

        for (int i = 0; i < cards.GetSize(); i++) {
            this._cards.AddLast(cards.CheckAt(i));
        }
        this._numWc = cards.GetNumWilds();
    }

    private Card<S, R, T, U> GetFirst() {
        return this._first;
    }

    private Card<S, R, T, U> GetLast() {
        return this._last;
    }

    protected override int MaxSize() {
        return OrderedEnum<U>.GetNumEnums();
    }

    protected override int GetSize() {
        return this.GetCards().Count;
    }

    protected override int GetNumWilds() {
        return this._numWc;
    }

    private (Card<S, R, T, U>?, Card<S, R, T, U>?) RunEdges(ArrayHand<S, R, T, U> hand) {
        var (card, pos) = hand.FirstNatural();
        if (card == null) {
            return (null, null);
        }

        if (!this._consecutiveWc && hand.HasConsecutiveJokers()) {
            return (null, null);
        }

        var firstCard = Card<S, R, T, U>.Copy(card);
        for (int i = pos - 1; i >= 0; i--) {
            Card<S, R, T, U> aux = hand.CheckAt(i);
            firstCard.Prev(this._canWrap);
            if (aux.IsNatural() && !firstCard.Equals(aux)) {
                return (null, null);
            }
        }

        var lastCard = Card<S, R, T, U>.Copy(card);
        for (int i = pos + 1; i < hand.GetSize(); i++) {
            Card<S, R, T, U> aux = hand.CheckAt(i);
            lastCard.Next(this._canWrap);
            if (aux.IsNatural() && !lastCard.Equals(aux)) {
                return (null, null);
            }
        }

        return (firstCard, lastCard);
    }

    private delegate LinkedListNode<Card<S, R, T, U>> Adder(Card<S, R, T, U> c);

    public override void Add(ArrayHand<S, R, T, U> arr) {
        if (this.GetSize() + arr.GetSize() > this.MaxSize()) {
            throw new ArgumentException("hand too big");
        }

        if (!this.CanMultWc() &&
            (this.GetNumWilds() + arr.GetNumWilds()) > 1)
        {
            throw new ArgumentException("hand too many wc");
        }

        var (first, last) = this.RunEdges(arr);
        if (first == null || last == null) {
            throw new ArgumentException("no naturals in hand");
        }

        last.Next(this._canWrap);
        first.Prev(this._canWrap);
        Adder adder;
        if (this.GetFirst().CompareTo(last) == 0) {
            adder = new Adder(this.GetCards().AddFirst);
        } else if (this.GetLast().CompareTo(first) == 0) {
            adder = new Adder(this.GetCards().AddLast);
        } else {
            throw new ArgumentException("hand not contiguous");
        }

        for (int i = 0; i < arr.GetSize(); i++) {
            Card<S, R, T, U> c = arr.CheckAt(i);
            adder(c);
            if (c.IsJoker()) {
                this._numWc =+ 1;
            }
        }
    }

    public override void Replace(Card<S, R, T, U> card, int pos) {
        if (card.IsJoker()) {
            throw new ArgumentException("replacing wild card with another wild card");
        }

        if (pos < 0 || pos >= this.GetSize()) {
            throw new ArgumentException("invalid position");
        }

        int count = 0;
        Card<S, R, T, U> guide = Card<S, R, T, U>.Copy(this.GetFirst());
        LinkedListNode<Card<S, R, T, U>>? pointer = this.GetCards().First;
        if (pointer == null) {
            throw new MeldInvalidStateException("empty meld");
        }

        while (count < pos) {
            guide.Next(this._canWrap);
            pointer = pointer.Next;
            if (pointer == null) {
                throw new MeldInvalidStateException("empty meld");
            }
            count++;
        }

        if (card.CompareTo(guide) != 0) {
            throw new ArgumentException("invalid replacement");
        }

        if (!pointer.Value.IsJoker()) {
            throw new ArgumentException("replacing non joker");
        }

        pointer.Value = card;
    }
}
