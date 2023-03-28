using Game;

namespace Client;

class Program {
    static void Main(string[] args) {
        try {
            var s = OrderedEnum<SpanishSuit>.First().GetName();
            var r = OrderedEnum<SpanishRank>.First().GetName();
            var c = new Card<OrderedEnum<SpanishSuit>,
                OrderedEnum<SpanishRank>,
                SpanishSuit,
                SpanishRank>("Swords", "Ace");

            if (c.IsJoker()) {
                Console.WriteLine($"Built a joker.");
            } else {
                Console.WriteLine($"{c.ToString()}");
                c.Next();
                Console.WriteLine($"{c.ToString()}");
            }
        } catch (ArgumentException) {
            Console.WriteLine("Invalid rank name.");
        }
    }
}
