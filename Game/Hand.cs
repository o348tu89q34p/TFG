namespace Game;

public class Hand<T> where T : class {
    private T[] _hand;
    private int _capacity;
    private int _size;

    public Hand(int n) {
        this._hand = new T[n];
        this.SetCapacity(n);
        this.SetSize(0);
    }

    private T[] GetHand() {
        return this._hand;
    }

    private void SetHand(T[] hand) {
        this._hand = hand;
    }

    private int GetCapacity() {
        return this._capacity;
    }

    private void SetCapacity(int capacity) {
        this._capacity = capacity;
    }

    private int GetSize() {
        return this._size;
    }

    private void SetSize(int size) {
        this._size = size;
    }

    /// <summray>Get the number of cards in the hand.</summary>
    public int Size() {
        return this.GetSize();
    }

    private void Add(T elem, int pos) {
        this.GetHand()[pos] = elem;
    }

    /// <returns>True if the the number of cards in the hand is the same as the capacity and false if it's less.</returns>
    public bool IsFull() {
        return this.GetSize() >= this.GetCapacity();
    }

    /// <summray>Add the given card to the right of the hand if the hand is not at its full capacity.</summary>
    /// <param name="elem">True</param>
    public void Append(T elem) {
        if (this.IsFull()) {
            return;
        }

        int pos = this.Size();

        this.Add(elem, pos);
        this.SetSize(pos + 1);
    }

    public bool IsEmpty() {
        return this.GetSize() == 0;
    }

    /// <param name="pos">The position of the card in the hand.<param>
    /// <returns>The card found in the position pos or null if pos is an invalid position for the current hand.</returns>
    public T? Check(int pos) {
        if (pos < 0 || pos >= this.GetSize()) {
            return null;
        }

        return this.GetHand()[pos];
    }

    /// <summary>Remove the card found in the given position in the hand. If the position is invalid, do nothing.</summray>
    /// <param name="pos">The position of the card to remove in the hand.</param>
    /// <returns>The card that was removed from the hand or null if the position was invalid.</returns>
    public T? Remove(int pos) {
        T? card = this.Check(pos);

        if (card == null) {
            return null;
        }

        T[] hand = this.GetHand();
        int top = this.GetSize();

        for (int i = pos; i < top - 1; i++) {
            hand[i] = hand[i + 1];
        }

        this.SetSize(top - 1);
        return card;
    }
}
