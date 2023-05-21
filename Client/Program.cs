using Domain;
using Game;

namespace Client {
    class Program {
        static void Main(string[] args) {
            /*
            ArrayHand<FrenchSuit, FrenchRank> arr = ICard<FrenchSuit, FrenchRank>.BuildDecks(1, 2);
            var hand = new ArrayHand<FrenchSuit, FrenchRank>(5);
            for (int i = 0; i < 5; i++) {
                hand.Append(arr.GetAt(i));
            }
            hand.Print();
            var r = new Rules<FrenchSuit, FrenchRank>(
                (new FrenchSuit()).Data.Length,
                (new FrenchRank()).Data.Length,
                3, 1, 4, 7, false, true, true, false, 3, 3);
            MeldRun<FrenchSuit, FrenchRank> mr = new MeldRun<FrenchSuit, FrenchRank>(hand, r.MeldR);
            mr.Print();

            // -------------------------------

            int suitL = (new FrenchSuit()).Data.Length;
            int rankL = (new FrenchRank()).Data.Length;

            var r = new Rules<FrenchSuit, FrenchRank>(
                suitL, rankL, 3, 1, 4, 7, false, true, true, false, 3, 3, true);
            var g = new CoreLoop<FrenchSuit, FrenchRank>(r);
            g.Play();
            */
            GameLoop game = new GameLoop(1024, 768, "Remigio Online");
            game.Run(60);
        }
    }
}
