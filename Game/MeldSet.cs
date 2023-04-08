using System;

namespace Game;

public abstract class MeldSet<S, R, T, U> : Meld<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    protected Card<S, R, T, U> _leftmost;

    public MeldSet(ArrayHand<S, R, T, U> cards,
                   bool consecutiveWc,
                   bool multipleWc,
                   int minSize) :
        base(cards, consecutiveWc, multipleWc, minSize)
    {
        var (card, pos) = cards.FirstNatural();
        if (card == null) {
            throw new ArgumentException("no natural cards");
        }

        // Index used to swap, not to count wild cards.
        int posWilds = 0;
        Card<S, R, T, U> aux;
        Card<S, R, T, U>? first = null;

        for (int i = 0; i < cards.GetSize(); i++) {
            aux = cards.CheckAt(i);
            if (aux.IsJoker()) {
                cards.Exchange(i, posWilds);
                posWilds++;
            } else if (first == null) {
                first = aux;
                this._cards.AddLast(aux);
            } else if (this._cards.Contains(aux)) {
                throw new ArgumentException("duplicated cards");
            } else if (first != null && first.CompareRank(aux) == 0) {
                this._cards.AddLast(aux);
            } else {
                throw new ArgumentException("mixed ranks");
            }
        }

        if (first == null) {
            throw new ArgumentException("no natural cards");
        }

        this._leftmost = first;
    }
}
