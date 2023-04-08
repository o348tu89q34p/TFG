using System;

namespace Game;

public abstract class MeldRun<S, R, T, U> : Meld<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    protected Card<S, R, T, U> _leftmost;
    protected Card<S, R, T, U> _rightmost;
    private bool _canWrap;

    public MeldRun(ArrayHand<S, R, T, U> cards,
                   bool canWrap,
                   bool consecutiveWc,
                   bool multipleWc,
                   int minSize) :
        base(cards, consecutiveWc, multipleWc, minSize)
    {
        // Wrapping only makes sense on runs.
        this._canWrap = canWrap;

        var (card, pos) = cards.FirstNatural();
        if (card == null) {
            throw new ArgumentException("no natural cards");
        }

        var cardDown = Card<S, R, T, U>.Copy(card);
        for (int i = pos - 1; i >= 0; i--) {
            Card<S, R, T, U> aux = cards.CheckAt(i);
            cardDown.Prev(this._canWrap);
            if (aux.IsNatural() && !cardDown.Equals(aux)) {
                throw new ArgumentException("invalid run");
            }
        }
        this._leftmost = cardDown;

        var cardUp = Card<S, R, T, U>.Copy(card);
        for (int i = pos + 1; i < cards.GetSize(); i++) {
            Card<S, R, T, U> aux = cards.CheckAt(i);
            cardUp.Next(this._canWrap);
            if (aux.IsNatural() && !cardUp.Equals(aux)) {
                throw new ArgumentException("invalid run");
            }
        }
        this._rightmost = cardUp;

        for (int i = 0; i < cards.GetSize(); i++) {
            this._cards.AddLast(cards.CheckAt(i));
        }
    }
}
