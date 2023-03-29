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

            if (cards == null) {
                return;
            }

            // foreach (Card<OrderedEnum<SpanishSuit>, OrderedEnum<SpanishRank>, SpanishSuit, SpanishRank> c in cards) {
            // Console.WriteLine(c.ToString());
            for (int i = 0; i < 1; i++) {
                Console.WriteLine(cards[i].ToString());
            }
        } catch (ArgumentException) {
            Console.WriteLine("Invalid rank name.");
        }
    }
}
