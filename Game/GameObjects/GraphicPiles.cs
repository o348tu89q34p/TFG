using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class GraphicPiles {
    private RectangleShape Background { get; }
    private Texture EmptyTexture { get; }
    private Texture StockTexture { get; }
    private Sprite Stock { get; set; }
    private int StockDepth { get; set; }
    private Stack<Sprite> Disc { get; }
    private Vector2f DiscPos { get; }

    private bool PressStock;
    private bool PressDiscard;

    public GraphicPiles(RenderWindow window, (Texture, Texture, Sprite?) sprites, int pickDepth) {
        this.StockTexture = sprites.Item1;
        this.Stock = new Sprite(this.StockTexture);
        this.EmptyTexture = sprites.Item2;
        float bgWidth = (this.Stock.GetGlobalBounds().Width + 30.0f);
        float bgHeight = (this.Stock.GetGlobalBounds().Height +
                          this.Stock.GetGlobalBounds().Height + 40.0f);
        float posX = 30.0f;
        float posY = ((float)window.Size.Y)*3.0f/4.0f - bgHeight/2.0f;
        this.Background = new RectangleShape(new Vector2f(bgWidth, bgHeight)) {
            Position = new Vector2f(posX, posY),
            FillColor = new Color(0, 0, 0, 80)
        };

        float pickX = this.Background.Position.X + bgWidth/2.0f - this.Stock.GetGlobalBounds().Width/2.0f;
        float pickY = this.Background.Position.Y + bgHeight/4.0f - this.Stock.GetGlobalBounds().Height/2.0f;
        this.Stock.Position = new Vector2f(pickX, pickY);
        this.StockDepth = pickDepth;

        this.Disc = new Stack<Sprite>();
        if (sprites.Item3 == null) {
            this.Disc.Push(new Sprite(this.EmptyTexture));
        } else {
            this.Disc.Push(sprites.Item3);
        }
        float discX = this.Background.Position.X + bgWidth/2.0f - this.Disc.Peek().GetGlobalBounds().Width/2.0f;
        float discY = this.Background.Position.Y + bgHeight*3.0f/4.0f - this.Disc.Peek().GetGlobalBounds().Height/2.0f;
        this.DiscPos = new Vector2f(discX, discY);
        this.Disc.Peek().Position = this.DiscPos;

        this.PressStock = false;
        this.PressDiscard = false;
    }

    public Vector2f StockPos() {
        return this.Stock.Position;
    }

    public Vector2f DiscardPos() {
        return this.DiscPos;
    }

    private void PopDisc() {
        this.Disc.Pop();
        if (Disc.Count == 0) {
            this.Disc.Push(new Sprite(this.EmptyTexture));
            this.Disc.Peek().Position = this.DiscPos;
        }
    }

    public void PopPick() {
        if (this.StockDepth < 0) {
            throw new Exception("Stock mismanagement.");
        }

        this.StockDepth--;
        if (this.StockDepth <= 0) {
            this.Stock = new Sprite(this.EmptyTexture) {
                Position = this.Stock.Position
            };
        }
    }

    public void Highlight() {
        this.Background.OutlineColor = Color.Yellow;
        this.Background.OutlineThickness = 3.0f;
    }

    public void Lowlight() {
        this.Background.OutlineColor = Color.Black;
        this.Background.OutlineThickness = 0.0f;
    }

    // Remove a sprite from one of the piles.
    public void PopCard(PileType pt) {
        switch (pt) {
            case PileType.STOCK:
                this.PopPick();
                break;
            case PileType.DISCARD:
                this.PopDisc();
                break;
            default:
                throw new Exception("Invalid pile to pop.");
        }
    }

    // Get the position from where to start or end an animation.
    public Vector2f GetPilePoint(PileType pt) {
        switch (pt) {
            case PileType.STOCK:
                return this.Stock.Position;
            case PileType.DISCARD:
                return this.Disc.Peek().Position;
            default:
                throw new Exception("Invalid value for the pile.");
        }
    }

    // Remove the sprite on top of the stock pile.
    private Sprite StockSprite() {
        if (this.StockDepth <= 0) {
            throw new Exception("Can't get the empty sprite from pick");
        }

        this.StockDepth--;
        /*
        if (this.StockDepth == 0) {
            return new Sprite(this.EmptyTexture) {
                Position = this.Stock.Position
            };
        }
        */

        return new Sprite(this.StockTexture) {
            Position = this.Stock.Position
        };
    }

    // Remove the sprite on the top of the discard pile.
    private Sprite DiscardSprite() {
        if (this.Disc.Count <= 0) {
            throw new Exception("Discard cannot get empty");
        }

        Sprite top = this.Disc.Peek();
        if (this.Disc.Count == 0) {
            this.Disc.Push(new Sprite(this.EmptyTexture));
        }

        return top;
    }

    // Get the sprite on top of the pile inquired.
    public Sprite GetSprite(PileType pt) {
        switch (pt) {
            case PileType.STOCK:
                return this.StockSprite();
            case PileType.DISCARD:
                return this.DiscardSprite();
            default:
                throw new Exception("Invalid sprite type.");
        }
    }

    public void DiscardCard(Sprite sprite) {
        sprite.Position = this.Disc.Peek().Position;
        this.Disc.Push(sprite);
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect stockBounds = this.Stock.GetGlobalBounds();
        FloatRect discardBounds = this.Disc.Peek().GetGlobalBounds();

        this.PressStock = stockBounds.Contains(mouse.X, mouse.Y);
        this.PressDiscard = discardBounds.Contains(mouse.X, mouse.Y);

        if (this.StockDepth <= 0) {
            this.StockDepth = this.Disc.Count - 1;
            Sprite s = this.Disc.Pop();
            this.Disc.Clear();
            this.Disc.Push(s);
            this.Stock = new Sprite(this.StockTexture) {
                Position = this.Stock.Position
            };
        }
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
