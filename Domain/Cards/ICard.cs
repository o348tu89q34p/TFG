namespace Domain;

public interface ICard<T, U> where T : Scale, new() where U : Scale, new()
{
    bool IsFirst();
    bool IsLast();
    bool IsWithin(NaturalCard<T, U> a, NaturalCard<T, U> b);

    bool IsNatural();
    bool IsWild();

    NaturalField<T> GetSuit();
    NaturalField<U> GetRank();

    void Next(bool wrap, bool inSuit);
    void Prev(bool wrap, bool inSuit);

    int CompareTo(ICard<T, U> c);
    int CompareRank(ICard<T, U> c);
    int CompareSuit(ICard<T, U> c);
    int SortCompareRank(ICard<T, U> c);
    int SortCompareSuit(ICard<T, U> c);
    bool Equals(ICard<T, U> c);

    (int, int) Coords();

    public static ICard<T, U> Copy(ICard<T, U> c) {
        if (c is WildCard<T, U>) {
            return new WildCard<T, U>();
        } else if (c is NaturalCard<T, U>) {
            return new NaturalCard<T, U>(NaturalField<T>.Copy(c.GetSuit()), NaturalField<U>.Copy(c.GetRank()));
        } else {
            throw new Exception("Copy not defined for this implementation of card.");
        }
    }

    public static ArrayHand<T, U> BuildDecks(int nDecks, int nWilds) {
        int n = (NaturalField<T>.LenData() *
                 NaturalField<U>.LenData() *
                 nDecks) + nWilds;
        var hand = new ArrayHand<T, U>(n);
        var c = new NaturalCard<T, U>(NaturalField<T>.First(), NaturalField<U>.First());

        for (int i = 0; i < nDecks; i++) {
            for (int j = 0; j < NaturalField<T>.LenData(); j++) {
                for (int k = 0; k < NaturalField<U>.LenData(); k++) {
                    hand.Append(ICard<T, U>.Copy(c));
                    c.Next(true, false);
                }
            }
        }

        return hand;
    }

    public static Stack<ICard<T, U>> BuildStock(int nDecks, int nWilds) {
        int n = (NaturalField<T>.LenData() *
                 NaturalField<U>.LenData() *
                 nDecks) + nWilds;
        ArrayHand<T, U> hand = ICard<T, U>.BuildDecks(nDecks, nWilds);
        for (int i = hand.Size(); i < n; i++) {
            hand.Append(new WildCard<T, U>());
        }

        hand.Shuffle();
        var stack = new Stack<ICard<T, U>>(hand.Size());
        for (int i = 0; i < hand.Size(); i++) {
            stack.Push(hand.GetAt(i));
        }

        return stack;
    }

    void Print();
}
