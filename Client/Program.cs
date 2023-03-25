using Game;

namespace Client;

class Program {
    static void Main(string[] args) {
        try {
            SpanishSuit s1 = SpanishSuit.FirstSuit();
            SpanishSuit s2 = SpanishSuit.LastSuit();

            Console.WriteLine($"{s1.GetName()}");
            s1.NextSuit();
            Console.WriteLine($"{s1.GetName()}");
            s1.NextSuit();
            Console.WriteLine($"{s1.GetName()}");
            s1.NextSuit();
            if (s1.IsLastSuit()) {
                Console.WriteLine("last suit");
            }
            Console.WriteLine($"{s2.GetName()}");
        } catch (ArgumentException) {
            Console.WriteLine("Invalid rank name.");
        }
    }
}
