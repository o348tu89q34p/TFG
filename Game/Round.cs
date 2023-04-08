using System;

namespace Game;

public class Round<S, R, T, U>
    where T: struct, System.Enum
    where U: struct, System.Enum
    where S: OrderedEnum<T>
    where R: OrderedEnum<U>
{
    private ServerPlayer<S, R, T, U>[] _players;
    private StackHand<S, R, T, U> _stock;
    private StackHand<S, R, T, U> _discardPile;
    private Rules _rules;
    private Meld<S, R, T, U>[] _melds;
    int _turn;

    public Round(Rules rules) {
        this._turn = 0;
        this._rules = rules;
        this._players = new ServerPlayer<S, R, T, U>[rules.NumPlayers()];
        // The size is some informed expectation of how many melds per player there could be.
        this._melds = new Meld<S, R, T, U>[rules.NumPlayers()*3];

        // create the pile
        var deck = Card<S, R, T, U>.buildDecks(rules.NumDecks(), rules.NumJokers());

        // mix the cards
        deck.Suffle();
        this._stock = new StackHand<S, R, T, U>(deck);

        // deliver the cards to the players
        for (int i = 0; i < rules.CardsPerPlayer(); i++) {
            for (int j = 0; j < rules.NumPlayers(); j++) {
                this._players[j].ToAdd(_stock.TakeCard());
            }
        }

        this._discardPile = new StackHand<S, R, T, U>(this._stock.GetSize());
        // set the discard pile
        if (!rules.NeedsOpen()) {
            this._discardPile.PutCard(this._stock.TakeCard());
        }
        // the stock is set
    }
}
