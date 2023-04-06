using System;
using Game;

namespace Client;

class Program {
    static void Main(string[] args) {
        try {
            var s = OrderedEnum<SpanishSuit>.First().GetName();
            var r = OrderedEnum<SpanishRank>.First().GetName();
            var cards = Card<OrderedEnum<SpanishSuit>,
                OrderedEnum<SpanishRank>,
                SpanishSuit,
                SpanishRank>.buildDecks(2, 4);

            // Create a smaller deck.
            int size = 10;
            var deck = new ArrayHand<Card<OrderedEnum<SpanishSuit>,
                OrderedEnum<SpanishRank>,
                SpanishSuit,
                SpanishRank>>(size);

            // Put in it a few cards from a full hand.
            for (int i = 0; i < size; i++) {
                deck.Append(cards.CheckAt(i));
            }

            // Shuffle the cards in the hand.
            deck.Suffle();

            // Display the suffled cards.
            for (int i = 0; i < size; i++) {
                Console.WriteLine(deck.CheckAt(i).ToString());
            }

        } catch (ArgumentException) {
            Console.WriteLine("Invalid rank name.");
        }
    }
}
