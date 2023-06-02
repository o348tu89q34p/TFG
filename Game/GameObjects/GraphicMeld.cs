using SFML.Graphics;
using SFML.System;

using Game;

namespace GameObjects;

class GraphicMeld {
    private List<Sprite> Meld { get; }
    private float Middle { get; }

    private static float Height = 200.0f;
    private static float HorSpacing = 25.0f;
    private static float VertSpacing = 15.0f;

    // n is the number of melds. x is the position of this meld in the list.
    public GraphicMeld(List<Sprite> meld, float middle, int n, int x) {
        this.Meld = meld;
        this.Middle = middle;

        this.UpdatePositions(middle, n, x);
    }

    public void UpdatePositions(float middle, int n, int x) {
        float cardWidth = (float)TextureUtils.CardWidth;
        if (this.Meld.Count > 0) {
            cardWidth = this.Meld[0].GetGlobalBounds().Width;
        }

        float width = n*cardWidth + (n - 1)*HorSpacing;
        float start = middle - width/2.0f + HorSpacing/2.0f; // - cardWidth/2.0f;
        float step = width/((float) n);

        for (int i = 0; i < this.Meld.Count; i++) {
            this.Meld[i].Position = new Vector2f(start + step*x, Height + VertSpacing*i);
        }
    }

    public void Replace(Sprite newCard, float middle, int n, int x, int i) {
        this.Meld[i] = newCard;
        this.UpdatePositions(middle, n, x);
    }

    public void Prepend(List<Sprite> newCards, float middle, int n, int x) {
        for (int i = 0; i < newCards.Count; i++) {
            this.Meld.Insert(i, newCards.ElementAt(i));
        }
        this.UpdatePositions(middle, n, x);
    }

    public void Append(List<Sprite> newCards, float middle, int n, int x) {
        for (int i = 0; i < newCards.Count; i++) {
            this.Meld.Add(newCards.ElementAt(i));
        }
        this.UpdatePositions(middle, n, x);
    }

    public void Render(RenderWindow window) {
        foreach (var m in this.Meld) {
            window.Draw(m);
        }
    }
}
