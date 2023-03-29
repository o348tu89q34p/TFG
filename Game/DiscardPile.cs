using System;
using System.Collections.Generic;

namespace Game;

public class DiscardPile<T> where T : class {
    private Stack<T> _pile;

    public DiscardPile(int n) {
        try {
            this._pile  = new Stack<T>(n);
        } catch (ArgumentOutOfRangeException) {
            this._pile  = new Stack<T>();
        }
    }

    private Stack<T> GetPile() {
        return this._pile;
    }

    /// <returns>True if the pile is empty. False otherwise.</returns>
    public bool IsEmpty() {
        return this.GetPile().Count == 0;
    }

    /// <returns>If the pile is not empty, take the upcard of the discard pile. It is removed from the pile.</returns>
    public T? TakeUpcard() {
        try {
            return this.GetPile().Pop();
        } catch (InvalidOperationException) {
            return null;
        }
    }

    /// <param name="card">True</param>
    /// <summary>Add card as the upcard of the discard pile.</summary>
    public void PutUpcard(T card) {
        this.GetPile().Push(card);
    }

    /// <returns>The number of cards found in the deck.</returns>
    public int Size() {
        return this.GetPile().Count;
    }
}
