using System;
using System.Collections.Generic;

namespace Game;

public class Stock<T> where T : class {
    private Stack<T> _stock;

    public Stock(int n) {
        try {
            this._stock = new Stack<T>(n);
        } catch (ArgumentOutOfRangeException) {
            this._stock = new Stack<T>();
        }
    }

    private Stack<T> GetStock() {
        return this._stock;
    }

    /// <returns>True if the stock is empty. False otherwise.</returns>
    public bool IsEmpty() {
        return this.GetStock().Count == 0;
    }

    /// <returns>If the stok is not empty, return the topcard from the stock removing it from the stock.</returns>
    public T? TakeTopcard() {
        try {
            return this.GetStock().Pop();
        } catch (InvalidOperationException) {
            return null;
        }
    }

    /// <summary>Build a new stock pile from the contents of the given discard pile in reverse order.</summary>
    public void FromDiscardPile(DiscardPile<T> dp) {
        this._stock = new Stack<T>(dp.Size());
        T? aux;

        while (!dp.IsEmpty()) {
            aux = dp.TakeUpcard();
            if (aux != null) {
                this.GetStock().Push(aux);
            }
        }
    }
}
