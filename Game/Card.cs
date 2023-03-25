namespace Game;

public interface Card {
    Card? NextInPack();
    Card? NextInPackWrap();
    Card? NextInSuit();
    Card? NextInSuitWrap();

    int? CompareTo(Card c);
    int? CompareSuit(Card? c);
    int? CompareRank(Card? c);

    bool? IsLastInSuit();
    bool? IsFirstInSuit();

    bool IsJoker();
}
