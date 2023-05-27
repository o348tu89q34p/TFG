using Domain;
using Game;

namespace Client {
    class Program {
        static void Main(string[] args) {
            GameLoop game = new GameLoop(1024, 768, "Remigio Online");
            game.Run(60);
        }
    }
}
