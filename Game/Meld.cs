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
    protected bool _consecutiveWc;
    protected int _numWc;

    protected Meld(ArrayHand<S, R, T, U> cards,
                   bool consecutiveWc,
                   bool multipleWc,
                   int minSize)
    {
        if (cards.GetSize() < minSize) {
            throw new ArgumentException("bad hand size");
        }

        if (!consecutiveWc && cards.HasConsecutiveJokers()) {
            throw new ArgumentException("consecutive jokers");
        }

        int numWc = cards.GetNumJokers();
        if (!multipleWc && numWc > 1) {
            throw new ArgumentException("multiple jokers");
        }

        this._cards = new LinkedList<Card<S, R, T, U>>();
        this._multipleWc = multipleWc;
        this._consecutiveWc = consecutiveWc;
        this._numWc = numWc;
    }
}
