using System;

namespace Game;

public abstract class Meld<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    protected LinkedList<Card<S, R, T, U>> _cards;
    protected bool _multipleWc;

    protected Meld(ArrayHand<S, R, T, U> cards,
                   bool multipleWc,
                   int minSize)
    {
        if (cards.GetSize() < minSize) {
            throw new ArgumentException("bad hand size");
        }

        if (!multipleWc && cards.GetNumWilds() > 1) {
            throw new ArgumentException("multiple jokers");
        }

        this._cards = new LinkedList<Card<S, R, T, U>>();
        this._multipleWc = multipleWc;
    }

    protected LinkedList<Card<S, R, T, U>> GetCards() {
        return this._cards;
    }

    protected abstract int MaxSize();
    protected abstract int GetSize();
    protected abstract int GetNumWilds();

    protected bool CanMultWc() {
        return this._multipleWc;
    }

    public abstract void Add(ArrayHand<S, R, T, U> arr);

    /*
     * Reasons why we don't take a hand but just a card:
     * - The user will click on a card from a meld on the deck
     *   to replace it. So there will only be one card selected.
     * - Multiple replacements will be carried out sequentially
     *   one after the other.
     * - This is the best solution for runs, since not doing it
     *   like this it still forces us to do multiple linear
     *   complexity to figure it out.
     */
    public abstract void Replace(Card<S, R, T, U> arr, int pos);
}
