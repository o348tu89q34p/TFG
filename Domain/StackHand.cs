using System;
using System.Collections.Generic;

namespace Domain {

/// <summary>
/// Hand optimised for direct access to the top card.
/// </summary>
public class StackHand<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private Stack<Card<S, R, T, U>> _pile;

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
        this._pile = new Stack<Card<S, R, T, U>>(n);
    }

    /// <summary>
    /// Build a hand out of the contents of a hand array.
    /// </summary>
    /// <param name="cards">
    /// The cards to insert in the stack hand.
    /// </param>
    public StackHand(ArrayHand<S, R, T, U> cards) {
        int size = cards.GetSize();
        this._pile = new Stack<Card<S, R, T, U>>(size);

        for (int i = 0; i < size; i++) {
            this.GetPile().Push(cards.CheckAt(i));
        }
    }

    private Stack<Card<S, R, T, U>> GetPile() {
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
    public Card<S, R, T, U> TakeCard() {
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
    public Card<S, R, T, U> CheckTop() {
        try {
            return this.GetPile().Peek();
        } catch (InvalidOperationException) {
            throw new EmptyHandException("peeking upcard");
        }
    }

    /// <summary>
    /// Place the card given at the top of the stack.
    /// </summary>
    public void PutCard(Card<S, R, T, U> elem) {
        this.GetPile().Push(elem);
    }
}
}
