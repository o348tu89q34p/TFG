using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class GraphicPiles {
    private RectangleShape Background { get; }
    private Sprite EmptyPile { get; }
    private Sprite Stock { get; set; }
    private int StockDepth { get; set; }
    private Stack<Sprite> Disc { get; }
    private Vector2f DiscPos { get; }
    private Sprite NextDisc { get; set; }

    private bool PressStock;
    private bool PressDiscard;

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

        this.Stock = sprites.Item1;
        float pickX = this.Background.Position.X + bgWidth/2.0f - this.Stock.GetGlobalBounds().Width/2.0f;
        float pickY = this.Background.Position.Y + bgHeight/4.0f - this.Stock.GetGlobalBounds().Height/2.0f;
        this.Stock.Position = new Vector2f(pickX, pickY);
        this.StockDepth = pickDepth;

        this.Disc = new Stack<Sprite>();
        this.Disc.Push(sprites.Item2);
        float discX = this.Background.Position.X + bgWidth/2.0f - this.Disc.Peek().GetGlobalBounds().Width/2.0f;
        float discY = this.Background.Position.Y + bgHeight*3.0f/4.0f - this.Disc.Peek().GetGlobalBounds().Height/2.0f;
        this.DiscPos = new Vector2f(discX, discY);
        this.Disc.Peek().Position = this.DiscPos;

        this.EmptyPile = sprites.Item3;
        this.NextDisc = sprites.Item2;

        this.PressStock = false;
        this.PressDiscard = false;
    }

    public Vector2f StockPos() {
        return this.Stock.Position;
    }

    public Vector2f DiscardPos() {
        return this.DiscPos;
    }

    public void PopDisc() {
        this.Disc.Pop();
        if (Disc.Count == 0) {
            this.Disc.Push(this.EmptyPile);
            this.Disc.Peek().Position = this.DiscPos;
        }
    }

    public void PopPick() {
        if (this.StockDepth <= 0) {
            throw new Exception("Stock mismanagement.");
        }

        Vector2f pos = this.Stock.Position;
        this.StockDepth--;
        if (this.StockDepth == 0) {
            this.Stock = EmptyPile;
            this.Stock.Position = pos;
        }
    }

    public void SetNextDisc(Sprite nextDisc) {
        this.NextDisc = nextDisc;
    }

    public void ChangeNextDisc() {
        this.NextDisc.Position = this.DiscPos;
        this.Disc.Push(this.NextDisc);
    }

    public void Highlight() {
        this.Background.OutlineColor = Color.Yellow;
        this.Background.OutlineThickness = 3.0f;
    }

    public void Lowlight() {
        this.Background.OutlineColor = Color.Black;
        this.Background.OutlineThickness = 0.0f;
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect stockBounds = this.Stock.GetGlobalBounds();
        FloatRect discardBounds = this.Disc.Peek().GetGlobalBounds();

        this.PressStock = stockBounds.Contains(mouse.X, mouse.Y);
        this.PressDiscard = discardBounds.Contains(mouse.X, mouse.Y);
    }

    public PileType OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
        if (sender == null) {
            return PileType.NONE;
        }

        RenderWindow window = (RenderWindow)sender;
        if (e.Button == Mouse.Button.Left) {
            if (this.PressStock) {
                return PileType.STOCK;
            } else if (this.PressDiscard) {
                return PileType.DISCARD;
            }
        }

        return PileType.NONE;
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Background);
        window.Draw(this.Stock);
        window.Draw(this.Disc.Peek());
    }
}
