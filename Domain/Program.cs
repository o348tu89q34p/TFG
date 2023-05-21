using Domain;

namespace StrategyTest {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine(new FrenchSuit().Data.Length);
            NaturalField<FrenchSuit> fr = new NaturalField<FrenchSuit>(3);

            for (int i = 0; i < 10; i++) {
                Console.WriteLine($"Created the suit: {fr.ToString()}");
                fr.Prev();
            }

            Console.WriteLine($"Created the suit: {fr.ToString()}");
        }
    }
}
