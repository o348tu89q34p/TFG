using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Domain;
using Game;

namespace GameObjects;

public class GraphicPiles {
    private RectangleShape Background { get; }
    private Texture EmptyTexture { get; }
    private Texture StockTexture { get; }
    private Vector2f StockPosition { get; }
    private Vector2f DiscardPosition { get; }
    private Sprite Stock { get; set; }
    private Sprite Discard { get; set; }

    private bool HoverStock;
    private bool HoverDiscard;

    // Stock texture, empty texture  and discard sprite.
    public GraphicPiles(RenderWindow window, (Texture, Texture) sprites) {
        this.StockTexture = sprites.Item1;
        this.EmptyTexture = sprites.Item2;
        // Background
        float bgWidth = (TextureUtils.CardWidth + 30.0f);
        float bgHeight = (TextureUtils.CardHeight + TextureUtils.CardHeight + 40.0f);
        float posX = 30.0f;
        float posY = window.Size.Y - bgHeight - 50.0f;
        this.Background = new RectangleShape(new Vector2f(bgWidth, bgHeight)) {
            Position = new Vector2f(posX, posY),
            FillColor = new Color(0, 0, 0, 80)
        };

        // Stock
        float stockX = this.Background.Position.X + bgWidth/2.0f - TextureUtils.CardWidth/2.0f;
        float stockY = this.Background.Position.Y + bgHeight/4.0f - TextureUtils.CardHeight/2.0f;
        this.StockPosition = new Vector2f(stockX, stockY);
        this.Stock = new Sprite(this.StockTexture);

        // Discard
        float discX = this.Background.Position.X + bgWidth/2.0f - TextureUtils.CardWidth/2.0f;
        float discY = this.Background.Position.Y + bgHeight*3.0f/4.0f - TextureUtils.CardHeight/2.0f;
        this.DiscardPosition = new Vector2f(discX, discY);
        this.Discard = new Sprite(this.StockTexture);

        // Hovers
        this.HoverStock = false;
        this.HoverDiscard = false;
    }

    public void UpdateChanges(bool stock, Sprite? discard) {
        if (stock) {
            this.Stock = new Sprite(this.StockTexture);
        } else {
            this.Stock = new Sprite(this.EmptyTexture);
        }
        this.Stock.Position = this.StockPosition;

        if (discard == null) {
            this.Discard = new Sprite(this.EmptyTexture);
        } else {
            this.Discard = discard;
        }
        this.Discard.Position = this.DiscardPosition;
    }

    public Vector2f StockPos() {
        return this.StockPosition;
    }

    public Vector2f DiscardPos() {
        return this.DiscardPosition;
    }

    public void Highlight() {
        this.Background.OutlineColor = Color.Yellow;
        this.Background.OutlineThickness = 3.0f;
    }

    public void Lowlight() {
        this.Background.OutlineColor = Color.Black;
        this.Background.OutlineThickness = 0.0f;
    }

    public ResultMove<int> PileClicked() {
        if (this.HoverStock) {
            return new ResultMove<int>(MoveKind.STOCK, new List<int>(), null, null);
        } else if (this.HoverDiscard) {
            return new ResultMove<int>(MoveKind.DISCARD, new List<int>(), null, null);
        } else {
            return new ResultMove<int>(MoveKind.EMPTY, new List<int>(), null, null);
        }
    }

    public void Update(RenderWindow window, Step step) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect stockBounds = this.Stock.GetGlobalBounds();
        FloatRect discardBounds = this.Discard.GetGlobalBounds();

        if (step == Step.HUM_PICK) {
            this.Highlight();
        } else {
            this.Lowlight();
        }

        this.HoverStock = stockBounds.Contains(mouse.X, mouse.Y);
        this.HoverDiscard = discardBounds.Contains(mouse.X, mouse.Y);
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Background);
        window.Draw(this.Stock);
        window.Draw(this.Discard);
    }
}
