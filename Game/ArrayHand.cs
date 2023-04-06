using System;

namespace Game;

/// <summary>
/// Hand optimised for direct access to positions by index.
/// </summary>
public class ArrayHand<T> {
    private List<T> _hand;

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
        this._hand = new List<T>(n);
    }

    private List<T> GetHand() {
        return this._hand;
    }

    /// <summary>
    /// Get the number of cards in the hand.
    /// </summary>
    public int GetSize() {
        return this.GetHand().Count;
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

    /// <summary>
    /// Make elem be the element in position pos in the hand.
    /// </summary>
    /// <param name="pos">
    /// 0 <= pos < this.GetSize().
    /// </param>
    /// <param name="elem">
    /// True.
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// pos < 0.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// pos >= this.GetSize().
    /// </exception>
    private void InsertAt(int pos, T elem) {
        if (pos < 0) {
            throw new NegativeIndexException("insert at");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("insert at");
        }

        // this.GetHand().Insert(pos, elem);
        this.GetHand()[pos] = elem;
    }

    /// <summary>
    /// Add the element provided to the right of the contents
    /// of the hand.
    /// </summary>
    /// <param name="elem">
    /// True.
    /// </param>
    /// <exception cref="FullHandException">
    /// The hand is at its maximum capacity before adding the
    /// element.
    /// </exception>
    public void Append(T elem) {
        if (this.IsFull()) {
            throw new FullHandException("append");
        }

        this.GetHand().Add(elem);
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
    public T CheckAt(int pos) {
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

        this.GetHand().RemoveAt(pos);
    }

    /// <returns>
    /// Returns the element in position pos after replacing it
    /// by elem.
    /// </returns>
    /// <param name="elem">
    /// True.
    /// </param>
    /// <param name="pos">
    /// 0 <= pos < this.GetSize().
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// pos < 0.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// pos >= this.GetSize().
    /// </exception>
    public T ReplaceAt(T elem, int pos) {
        if (pos < 0) {
            throw new NegativeIndexException("replace at");
        } else if (pos >= this.GetSize()) {
            throw new IndexOverflowException("replace at");
        }

        T aux = this.CheckAt(pos);
        this.InsertAt(pos, elem);

        return aux;
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
            aux = this.CheckAt(r);
            this.InsertAt(r, this.CheckAt(i));
            this.InsertAt(i, aux);
        }
    }

    public void Sort() {
        this.GetHand().Sort();
    }
}
