using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameObjects;

public class GraphicHand {
    private List<GraphicCard> Cards { get; }

    public GraphicHand(RenderWindow window, List<Sprite> sprites, int cardWidth, int cardHeight) {
        this.Cards = new List<GraphicCard>(sprites.Count);
        float maxPeek = 40.0f;
        float handWidth = ((float)window.Size.X)*0.68f;
        // The amount of space available to each peeking card.
        float peekAvailable = (handWidth - (float)cardWidth)/((float)(sprites.Count - 1));
        float peekWidth;
        if (peekAvailable > maxPeek) {
            peekWidth = maxPeek;
        } else {
            peekWidth = peekAvailable;
        }
        float actualWidth = peekWidth*((float)(sprites.Count - 1)) + (float)cardWidth;
        float firstPos = window.Size.X/2.0f - actualWidth/2.0f;
        float height = (float)window.Size.Y - (float)cardHeight;
        for (int i = 0; i < sprites.Count - 1; i++) {
            this.Cards.Add(new GraphicCard(sprites[i], new Vector2f(firstPos + peekWidth*i, height)));
        }
        this.Cards.Add(new GraphicCard(sprites[sprites.Count - 1], new Vector2f(firstPos + peekWidth*(sprites.Count - 1), height)));
    }

    public void Render(RenderWindow window) {
        foreach (var c in this.Cards) {
            c.Render(window);
        }
    }
}

