using Game;

namespace Server;

class Program {
    static void Main(string[] args) {
        var g = new SpanishCard("this", "trash");
        Console.WriteLine($"Card type: {SpanishCard.type}");
    }
}
