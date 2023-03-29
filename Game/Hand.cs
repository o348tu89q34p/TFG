namespace Game;

public class Hand<T> {
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

    /// <summray>Add the given card to the right of the hand if the hand is not at its full capacity.</summary>
    /// <param name="elem">True</param>
    public void Append(T elem) {
        int pos = this.Size();

        if (pos >= this.GetCapacity()) {
            return;
        }

        this.Add(elem, pos);
    }

    /// <summary>Remove the card found in the given position in the hand. If the position is invalid, do nothing.</summray>
    /// <param name="pos">The position of the card to remove in the hand.</param>
    public void Remove(int pos) {
        int top = this.GetSize();
        if (pos < 0 || pos >= top) {
            return;
        }

        T[] hand = this.GetHand();
        for (int i = pos; i < top - 1; i++) {
            hand[i] = hand[i + 1];
        }

        this.SetSize(top - 1);
    }
}
