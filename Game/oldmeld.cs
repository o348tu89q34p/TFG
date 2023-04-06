using System;

namespace Game;

public class Meld<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private LinkedList<Card<S, R, T, U>> _cards;
    private MeldType _type;

    public Meld() {
        this._cards = new LinkedList<Card<S, R, T, U>>();
        this.SetMeldType(MeldType.Undefined);
    }

    private LinkedList<Card<S, R, T, U>> GetCards() {
        return this._cards;
    }

    private MeldType GetMeldType() {
        return this._type;
    }

    private void SetMeldType(MeldType type) {
        this._type = type;
    }

    /// <returns>
    /// True if the meld is a set, false otherwise.
    /// </returns>
    public bool IsSet() {
        return this.GetMeldType() == MeldType.Set;
    }

    /// <returns>
    /// True if the meld is a run, false otherwise.
    /// </returns>
    public bool IsRun() {
        return this.GetMeldType() == MeldType.Run;
    }

    /// <returns>
    /// True if the meld is undefined, false otherwise.
    /// </returns>
    public bool IsUndefined() {
        return this.GetMeldType() == MeldType.Undefined;
    }
    /*

    // change visibility
    public Card<S, R, T, U>? GetLast() {
        Card<S, R, T, U> c = null;
        foreach (Card<S, R, T, U> card in this.GetCards()) {
            if (c == null) {
                if (!card.IsJoker()) {
                    c = Card<S, R, T, U>.Copy(card);
                }
            } else {
                c.NextWrap();
            }
        }

        return c;
    }

    /// <summray>
    /// Add the card to the meld as a new member.
    /// </summray>
    public void Append(Card<S, R, T, U> c) {
        Card<S, R, T, U>? prev = this.GetLast();
        if (this.IsEmpty() || c.IsJoker() || prev == null) {
            this.GetCards().AddLast(c);
            return;
        } else if (this.IsUndefined()) {
            if (prev == null) {
                this.GetCards().AddLast(c);
                return;
            } else {
                
            }
        }

        Card<S, R, T, U>? prev = this.GetLast();
    }

    // prepend
    // replace

    // replace a joker.
    // https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-8.0
    */
}
