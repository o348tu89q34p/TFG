using System;

namespace Game;

/// <summary>
/// Hand optimised for direct access to positions by index.
/// </summary>
public class ArrayHand<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private List<Card<S, R, T, U>> _hand;
    private int _numWc;
    private bool _consecWc;

    /// <summary>
    /// Buld an empty hand that can hold up to n cards.
    /// </summary>
    /// <param name="n">
    /// n >= 0. The maximum capacity of the hand.
    /// </param>
    /// <exception cref="NegativeSizseException">
    /// n has a negative value.
    /// </exception>
    public ArrayHand(int n) {
        if (n < 0) {
            throw new NegativeSizeException("array hand");
        }
        this._hand = new List<Card<S, R, T, U>>(n);
    }

    private List<Card<S, R, T, U>> GetHand() {
        return this._hand;
    }

    /// <returns>
    /// The number of cards in the hand.
    /// </returns>
    public int GetSize() {
        return this.GetHand().Count;
    }

    /// <returns>
    /// The number of cards in the hand that are jokers.
    /// </returns>
    public int GetNumJokers() {
        return this._numWc;
    }

    private void SetNumJokers(int numWc) {
        this._numWc = numWc;
    }

    private void IncNumJokers() {
        this.SetNumJokers(this.GetNumJokers() + 1);
    }

    private void DecNumJokers() {
        this.SetNumJokers(this.GetNumJokers() - 1);
    }

    /// <returns>
    /// True if at any point of the hand there are two cards
    /// next to each other that are jokers.
    /// </returns>
    public bool HasConsecutiveJokers() {
        return this._consecWc;
    }

    private void SetConsecWc(bool consecWc) {
        this._consecWc = consecWc;
    }

    private int GetCapacity() {
        return this.GetHand().Capacity;
    }

    /// <returns>
    /// True if the number of cards in the hand is the same as
    /// the hand's capacity, false otherwise.
    /// </returns>
    public bool IsFull() {
        return this.GetSize() == this.GetCapacity();
    }

    /// <returns>
    /// True if the hand contains no cards, false otherwise.
    /// </returns>
    public bool IsEmpty() {
        return this.GetSize() == 0;
    }

    private bool CheckIfConsecWc(int p1, int p2) {
        return (this.CheckAt(p1).IsJoker() &&
                this.CheckAt(p2).IsJoker());
    }

    private void UpdateConsecutiveWc(int pos) {
        if (this.GetSize() <= 1) {
            return;
        }

        bool aux = this.HasConsecutiveJokers();
        bool next;
        if (pos == 0) {
            next = aux || this.CheckIfConsecWc(pos, pos + 1);
        } else if (pos == (this.GetSize() - 1)) {
            next = aux || this.CheckIfConsecWc(pos, pos - 1);
        } else {
            next = (aux || this.CheckIfConsecWc(pos, pos - 1) ||
                    this.CheckIfConsecWc(pos, pos + 1));
        }

        this.SetConsecWc(next);
    }

    /// <summary>
    /// Make elem be the element in position pos in the hand.
    /// </summary>
    /// <param name="pos">
    /// 0 <= pos < this.GetSize().
    /// </param>
    /// <param name="c">
    /// True.
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// pos < 0.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// pos >= this.GetSize().
    /// </exception>
    private void InsertAt(int pos, Card<S, R, T, U> c) {
        if (pos < 0) {
            throw new NegativeIndexException("insert at");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("insert at");
        }

        if (!this.CheckAt(pos).IsJoker() && c.IsJoker()) {
            this.IncNumJokers();
        } else if (this.CheckAt(pos).IsJoker() && !c.IsJoker()) {
            this.DecNumJokers();
        }

        UpdateConsecutiveWc(pos);

        this.GetHand()[pos] = c;
    }

    /// <summary>
    /// Add the card provided to the right of the contents
    /// of the hand.
    /// </summary>
    /// <param name="c">
    /// True.
    /// </param>
    /// <exception cref="FullHandException">
    /// The hand is at its maximum capacity before adding the
    /// element.
    /// </exception>
    public void Append(Card<S, R, T, U> c) {
        if (this.IsFull()) {
            throw new FullHandException("append");
        }

        this.GetHand().Add(c);
    }

    /// <returns>
    /// The card from the hand found in position pos counting
    /// from the left.
    /// </returns>
    /// <param name="pos">
    /// 0 <= pos < this.GetSize().
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// pos < 0.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// pos >= this.GetSize().
    /// </exception>
    public Card<S, R, T, U> CheckAt(int pos) {
        if (pos < 0) {
            throw new NegativeIndexException("check at");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("check at");
        }

        return this.GetHand()[pos];
    }

    /// <summary>
    /// The card found in position pos within the player's
    /// hand is removed.
    /// </summary>
    /// <param name="pos">
    /// 0 <= pos < this.GetSize().
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// pos < 0.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// pos >= this.GetSize().
    /// </exception>
    public void RemoveAt(int pos) {
        if (pos < 0) {
            throw new NegativeIndexException("remove at");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("remove at");
        }

        if (this.CheckAt(pos).IsJoker()) {
            this.DecNumJokers();
        }

        this.GetHand().RemoveAt(pos);

        UpdateConsecutiveWc(pos);
    }

    /// <returns>
    /// Returns the element in position pos after replacing it
    /// by elem.
    /// </returns>
    /// <param name="elem">
    /// True.
    /// </param>
    /// <param name="c">
    /// 0 <= pos < this.GetSize().
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// pos < 0.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// pos >= this.GetSize().
    /// </exception>
    public Card<S, R, T, U> ReplaceAt(int pos, Card<S, R, T, U> c) {
        if (pos < 0) {
            throw new NegativeIndexException("replace at");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("replace at");
        }

        Card<S, R, T, U> aux = this.CheckAt(pos);
        this.InsertAt(pos, c);

        return aux;
    }

    /// <summray>
    /// Randomize the position of each card in the hand.
    /// </summray>
    public void Suffle() {
        int n = this.GetSize();
        int r;
        Random rnd = new Random();

        for (int i = 0; i < n; i++) {
            r = rnd.Next(i, n);
            this.Exchange(i, r);
            if (!this.HasConsecutiveJokers() && i > 0) {
                this.SetConsecWc(CheckIfConsecWc(i, i - 1));
            }
        }
    }

    public void Sort() {
        this.GetHand().Sort();
    }

    public void Exchange(int a, int b) {
        int max = this.GetSize();
        if (a < 0 || b < 0 || a >= max || b >= max) {
            throw new ArgumentOutOfRangeException("exchange");
        }

        Card<S, R, T, U> aux;
        aux = this.CheckAt(a);
        this.InsertAt(a, this.CheckAt(b));
        this.InsertAt(b, aux);
    }

    public (Card<S, R, T, U>?, int) FirstNatural() {
        if (this.GetNumJokers() == this.GetSize()) {
            return (null, 0);
        }

        Card<S, R, T, U>? first = null;
        int pos = 0;

        while (first == null && pos < this.GetSize()) {
            Card<S, R, T, U> current = this.CheckAt(pos);
            if (current.IsNatural()) {
                first = current;
            }
            pos++;
        }

        return (first, pos - 1);
    }
}
