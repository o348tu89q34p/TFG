using System;
using System.Collections.Generic;

namespace Game;

/// <summary>
/// Hand optimised for direct access to the top card.
/// </summary>
public class StackHand<T> {
    private Stack<T> _pile;

    /// <summary>
    /// Build a hand stack of size n.
    /// </summary>
    /// <param name="n">
    /// The size of the stack to build.
    /// </param>
    /// <exception cref="NegativeSizseException">
    /// The value of n is negative.
    /// </exception>
    public StackHand(int n) {
        if (n < 0) {
            throw new NegativeSizeException("stack hand");
        }
        this._pile = new Stack<T>(n);
    }

    /// <summary>
    /// Build a hand out of the contents of a hand array.
    /// </summary>
    /// <param name="elems">
    /// The elements to insert in the stack hand.
    /// </param>
    public StackHand(ArrayHand<T> elems) {
        int size = elems.GetSize();
        this._pile = new Stack<T>(size);

        for (int i = 0; i < size; i++) {
            this.GetPile().Push(elems.CheckAt(i));
        }
    }

    private Stack<T> GetPile() {
        return this._pile;
    }

    /// <returns>
    /// True if there are no cards in the stack, false
    /// otherwise.
    /// </returns>
    public bool IsEmpty() {
        return this.GetPile().Count == 0;
    }

    /// <returns>
    /// The number of cards currently in the stack.
    /// </returns>
    public int GetSize() {
        return this.GetPile().Count;
    }

    /// <returns>
    /// The card on top of the stack after removing it.
    /// </returns>
    /// <exception cref="EmptyHandException">
    /// The stack is empty before removing the element.
    /// </exception>
    public T TakeCard() {
        try {
            return this.GetPile().Pop();
        } catch (InvalidOperationException) {
            throw new EmptyHandException("taking upcard");
        }
    }

    /// <returns>
    /// The card on top of the stack without removing it.
    /// </returns>
    /// <exception cref="EmptyHandException">
    /// The stack is empty before peeking into it.
    /// </exception>
    public T CheckTop() {
        try {
            return this.GetPile().Peek();
        } catch (InvalidOperationException) {
            throw new EmptyHandException("peeking upcard");
        }
    }

    /// <summary>
    /// Place the card given at the top of the stack.
    /// </summary>
    public void PutCard(T elem) {
        this.GetPile().Push(elem);
    }
}
