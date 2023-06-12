using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class HoverHand {
    private List<GraphicCard> Cards { get; }
    private List<int> Positions { get; }

    private Vector2f Canvas { get; }
    private Vector2f SpriteDims { get; }

    public HoverHand(Vector2f canvas, Vector2f spriteDims) {
        this.Cards = new List<GraphicCard>();
        this.Positions = new List<int>();

        this.Canvas = canvas;
        this.SpriteDims = spriteDims;
    }

    public void Add(GraphicCard card, int n) {
        card.Take();
        card.DeHover();
        this.Cards.Add(card);
        this.Positions.Add(n);
        this.Rearrange();
    }

    public void Clear() {
        this.Cards.Clear();
        this.Positions.Clear();
    }

    public bool IsActive() {
        return this.Cards.Count > 0;
    }

    public List<GraphicCard> GetCards() {
        return this.Cards;
    }

    public int Size() {
        return this.Cards.Count;
    }

    public List<int> GetPositions() {
        return this.Positions;
    }

    private void Rearrange() {
        int n = this.Cards.Count;
        float offset = 5.0f;
        float middle = this.Canvas.X/2.0f;
        float allWidth = (this.SpriteDims.X*n + offset*(n - 1));
        float start = middle - (allWidth/2.0f);
        float height = this.Canvas.Y - TextureUtils.CardHeight*2.0f - 60.0f;
        for (int i = 0; i < n; i++) {
            this.Cards[i].UpdatePosition(new Vector2f(start + (this.SpriteDims.X + offset)*i, height));
        }
    }

    public void Demote(int pos) {
        for (int i = 0; i < this.Cards.Count; i++) {
            if (pos == this.Positions[i]) {
                this.Cards[i].UnTake();
                this.Cards.RemoveAt(i);
                this.Positions.RemoveAt(i);
                this.Rearrange();
                return;
            }
        }
    }

    public void Render(RenderWindow window) {
        foreach (GraphicCard c in this.Cards) {
            c.Render(window);
        }
    }
}
