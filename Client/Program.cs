using System;

using Domain;
using GameLogic;

namespace Client {
    class Program {
        static void Main(string[] args) {
            Game game = new Game(1024, 768, "Remigio Online");
            game.Run(60);
        }
    }
}
