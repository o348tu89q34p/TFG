using SFML.Graphics;
using SFML.System;

namespace GameObjects;

public class GraphicPiles {
    private RectangleShape Background { get; }
    private Sprite EmptyPile { get; }
    private Sprite Pick { get; set; }
    private int PickDepth { get; set; }
    private Stack<Sprite> Disc { get; }

    public GraphicPiles(RenderWindow window, (Sprite, Sprite, Sprite) sprites, int pickDepth) {
        float bgWidth = (sprites.Item1.GetGlobalBounds().Width + 30.0f);
        float bgHeight = (sprites.Item1.GetGlobalBounds().Height +
                          sprites.Item2.GetGlobalBounds().Height + 40.0f);
        float posX = 30.0f;
        float posY = ((float)window.Size.Y)*3.0f/4.0f - bgHeight/2.0f;
        this.Background = new RectangleShape(new Vector2f(bgWidth, bgHeight)) {
            Position = new Vector2f(posX, posY),
            FillColor = new Color(0, 0, 0, 80)
        };

        this.Pick = sprites.Item1;
        float pickX = this.Background.Position.X + bgWidth/2.0f - this.Pick.GetGlobalBounds().Width/2.0f;
        float pickY = this.Background.Position.Y + bgHeight/4.0f - this.Pick.GetGlobalBounds().Height/2.0f;
        this.Pick.Position = new Vector2f(pickX, pickY);
        this.PickDepth = pickDepth;

        this.Disc = new Stack<Sprite>();
        this.Disc.Push(sprites.Item2);
        float discX = this.Background.Position.X + bgWidth/2.0f - this.Disc.Peek().GetGlobalBounds().Width/2.0f;
        float discY = this.Background.Position.Y + bgHeight*3.0f/4.0f - this.Disc.Peek().GetGlobalBounds().Height/2.0f;
        this.Disc.Peek().Position = new Vector2f(discX, discY);

        this.EmptyPile = sprites.Item3;
    }

    public Vector2f StockPos() {
        return this.Pick.Position;
    }

    public Vector2f DiscardPos() {
        return this.Disc.Peek().Position;
    }

    public void PopDisc() {
        Vector2f pos = this.Disc.Peek().Position;
        this.Disc.Pop();
        if (Disc.Count == 0) {
            this.Disc.Push(this.EmptyPile);
            this.Disc.Peek().Position = pos;
        }
    }

    public void PopPick() {
        if (this.PickDepth <= 0) {
            throw new Exception("Stock mismanagement.");
        }

        Vector2f pos = this.Pick.Position;
        this.PickDepth--;
        if (this.PickDepth == 0) {
            this.Pick = EmptyPile;
            this.Pick.Position = pos;
        }
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Background);
        window.Draw(this.Pick);
        window.Draw(this.Disc.Peek());
    }
}
