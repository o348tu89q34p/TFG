using Game;

namespace Client;

class Program {
    static void Main(string[] args) {
        try {
            OrderedEnum<SpanishSuit> s1 = OrderedEnum<SpanishSuit>.FirstEnum();
            OrderedEnum<SpanishSuit> s2 = OrderedEnum<SpanishSuit>.LastEnum();
            OrderedEnum<SpanishRank> s3 = OrderedEnum<SpanishRank>.LastEnum();

            Console.WriteLine($"{s1.GetName()}");
            s1.NextEnum();
            Console.WriteLine($"{s1.GetName()}");
            s1.NextEnum();
            Console.WriteLine($"{s1.GetName()}");
            s1.NextEnum();
            if (s1.IsLastEnum()) {
                Console.WriteLine("last suit");
            }
            Console.WriteLine($"{s2.GetName()}");
            Console.WriteLine($"{s3.GetName()}");
        } catch (ArgumentException) {
            Console.WriteLine("Invalid rank name.");
        }
    }
}
