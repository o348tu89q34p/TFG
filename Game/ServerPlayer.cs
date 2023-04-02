namespace Game;

public class Player<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private ArrayHand<Card<S, R, T, U>> _hand;
    private bool _hasComeOut;

    public Player(ArrayHand<Card<S, R, T, U>> hand) {
        this._hand = hand;
        this.SetHasComeOut(false);
    }

    private ArrayHand<Card<S, R, T, U>> GetHand() {
        return this._hand;
    }

    private bool GetHasComeOut() {
        return this._hasComeOut;
    }

    private void SetHasComeOut(bool b) {
        this._hasComeOut = b;
    }

    /// <param name="card">
    /// The card to add to the player's hand.
    /// </param>
    /// <returns>
    /// Returns true if the card could be added to the player's
    /// hand and false otherwise.
    /// </returns>
    /// <exception cref="FullHandException">
    /// The player's hand is full.
    /// </exception>
    public bool ToAdd(Card<S, R, T, U> card) {
        if (this.GetHand().IsFull()) {
            return false;
        }

        this.GetHand().Append(card);

        return true;
    }

    /// <summary>
    /// The cards are not removed from the payer's hand.
    /// </summary>
    /// <param name="pos">
    /// List with the indices of the cards to lay off.
    /// </param>
    /// <returns>
    /// An array with all the cards specified in the positions
    /// or null if any of the positions are invalid.
    /// </returns>
    public Card<S, R, T, U>[] ToLayOff(int[] pos) {
        // TODO: Decide whether to return an array or hand.
        var arr = new Card<S, R, T, U>[pos.Length];
        int i = 0;

        foreach (int p in pos) {
            arr[i] = this.GetHand().Check(p);
            i++;
        }

        return arr;
    }

    /// <param name="pos">
    /// The position of the card chosen by the playet to shed.
    /// </param>
    /// <returns>
    /// The card chosen by the player to discard.
    /// </returns>
    /// <exception cref="NegativeIndexException">
    /// The index provided is a negative number.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// The index provided refers to a position greater than
    /// hand size.
    /// </exception>
    public Card<S, R, T, U> ToShed(int pos) {
        return this.GetHand().Check(pos);
    }

    /// <summary>
    /// The player sheds the card from it's hand at position pos.
    /// </summary>
    /// <param name="pos">
    /// The position of the card to shed.
    /// </param>
    /// <exception cref="NegativeIndexException">
    /// The index provided is a negative number.
    /// </exception>
    /// <exception cref="IndexOverflowException">
    /// The index provided refers to a position greater than
    /// hand size.
    /// </exception>
    public void ShedCard(int pos) {
        this.GetHand().Remove(pos);
    }

    /// <returns>
    /// True if the player has no more cards in their hand.
    /// </returns>
    // TODO: Revisit the naming around coming out and being out.
    public bool IsOut() {
        return this.GetHand().IsEmpty();
    }
}
