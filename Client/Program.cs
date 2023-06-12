using Domain;
using Game;

namespace Client {
    class Program {
        static void Main(string[] args) {
            GameLoop game = new GameLoop(1366, 768, "Remigio Challenge");
            game.Run(60);
        }
    }
}
