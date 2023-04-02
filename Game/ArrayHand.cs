using System;

namespace Game;

/// <summary>
/// Hand optimised direct access to positions by index.
/// Used as a player's hand.
/// </summary>
public class ArrayHand<T> {
    private T[] _hand;
    private int _size;

    /// <summary>
    /// Buld a hand that can hold up to n cards.
    /// </summary>
    /// <param name="n">
    /// The maximum capacity of the hand.
    /// </param>
    /// <exception cref="NegativeSizseException">
    /// The value of n is negative.
    /// </exception>
    public ArrayHand(int n) {
        if (n < 0) {
            throw new NegativeSizeException("array hand");
        } else {
            this._hand = new T[n];
        }
        this.SetSize(0);
    }

    private T[] GetHand() {
        return this._hand;
    }

    /// <summary>
    /// Get the number of cards in the hand.
    /// </summary>
    public int GetSize() {
        return this._size;
    }

    private void SetSize(int size) {
        this._size = size;
    }

    private int GetCapacity() {
        return this.GetHand().Length;
    }

    /// <returns>
    /// True if the hand does not admit any more cards, false
    /// otherwise.
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

    private void Put(T elem, int pos) {
        if (pos < 0) {
            throw new NegativeIndexException("adding");
        } else if (pos >= this.GetCapacity()) {
            throw new IndexOverflowException("adding");
        }

        this.GetHand()[pos] = elem;
    }

    /// <summary>
    /// Add the element provided to the right of the contents
    /// of the hand.
    /// </summary>
    /// <exception cref="FullHandException">
    /// The hand is at its maximum capacity before adding the
    /// element.
    /// </exception>
    public void Append(T elem) {
        if (this.IsFull()) {
            throw new FullHandException("append");
        }

        int pos = this.GetSize();
        this.Put(elem, pos);
        this.SetSize(pos + 1);
    }

    /// <returns>
    /// The card from the hand found in position pos starting
    /// from the left.
    /// </returns>
    /// <exception cref="NegativeIndexException">
    /// The index provided is a negative number.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// The index provided refers to a position greater than
    /// hand size.
    /// </exception>
    public T Check(int pos) {
        if (pos < 0) {
            throw new NegativeIndexException("checking");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("checking");
        }

        return this.GetHand()[pos];
    }

    /// <param name="pos">
    /// The position in the hand of the card to check.
    /// </param>
    /// <summary>
    /// The card found in position pos within the player's
    /// hand is removed.
    /// </summary>
    /// <exception cref="NegativeIndexException">
    /// The index provided is a negative number.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// The index provided refers to a position greater than
    /// hand size.
    /// </exception>
    public void Remove(int pos) {
        if (pos < 0) {
            throw new NegativeIndexException("removing");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("removing");
        }

        T[] hand = this.GetHand();
        int top = this.GetSize();

        for (int i = pos; i < top - 1; i++) {
            hand[i] = hand[i + 1];
        }

        this.SetSize(top - 1);
    }

    /// <summray>
    /// Randomize the position of each card in the hand.
    /// </summray>
    public void Suffle() {
        int n = this.GetSize();
        int r;
        Random rnd = new Random();
        T aux;

        for (int i = 0; i < n; i++) {
            r = rnd.Next(i, n);
            aux = this.GetHand()[r];
            this.GetHand()[r] = this.GetHand()[i];
            this.GetHand()[i] = aux;
        }
    }
}
