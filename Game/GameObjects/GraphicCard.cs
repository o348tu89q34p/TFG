using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class GraphicCard {
    private Sprite Sprite { get; }
    private int PosInHand { get; set; }

    private RectangleShape Selector { get; set; }
    private float Visible { get; set; }
    private bool IsLast { get; set; }
    private bool Taken { get; set; }

    private Sprite? Cross { get; set; }

    public bool HoveredCross { get; private set; }
    public bool IsHovered { get; private set; }

    public GraphicCard(Sprite sprite, float visible) {
        this.Sprite = sprite;
        this.Selector = this.BuildSelector();
        this.Visible = visible;
        this.IsLast = false;
        this.IsHovered = false;
        this.Taken = false;
    }

    public GraphicCard(Sprite frontSprite, Vector2f pos) {
        this.Sprite = frontSprite;
        this.Sprite.Position = pos;
        this.Selector = this.BuildSelector();
        this.Taken = false;
    }

    public GraphicCard(Sprite sprite, int posInHand) {
        this.Sprite = sprite;
        this.Selector = this.BuildSelector();
        this.PosInHand = posInHand;
    }

    public Vector2f GetPosition() {
        return this.Sprite.Position;
    }

    public Sprite GetSprite() {
        return this.Sprite;
    }

    public void UpdatePosition(Vector2f pos) {
        this.Sprite.Position = pos;
    }

    public void DeHover() {
        this.IsHovered = false;
    }

    public void Take() {
        this.Taken = true;
    }

    public void UnTake() {
        this.Taken = false;
    }

    public bool IsTaken() {
        return this.Taken;
    }

    private RectangleShape BuildSelector() {
        float h = this.Sprite.GetGlobalBounds().Height;
        float w;
        if (this.IsLast) {
            w = this.Sprite.GetGlobalBounds().Width;
        } else {
            w = this.Visible - 2.7f;
        }

        RectangleShape rs = new RectangleShape(new Vector2f(w, h)) {
            FillColor = new Color(0, 0, 0, 0),
            OutlineColor = Color.Red,
            OutlineThickness = 2.0f
        };

        float x = this.GetPosition().X;
        float y = this.GetPosition().Y;
        rs.Position = new Vector2f(x, y);

        return rs;
    }

    public void Update(RenderWindow window, float visible, Step step, bool isLast) {
        this.Visible = visible;
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));

        if (this.IsTaken()) {
            this.Cross = new Sprite(TextureUtils.CrossTexture);
            float x = this.Sprite.Position.X + TextureUtils.CardWidth - this.Cross.GetGlobalBounds().Width - 4.0f;
            float y = this.Sprite.Position.Y + 4.0f;
            this.Cross.Position = new Vector2f(x, y);
            this.HoveredCross = this.Cross.GetGlobalBounds().Contains(mouse.X, mouse.Y);
            return;
        } else {
            this.Cross = null;
            this.HoveredCross = false;
        }

        switch (step) {
            case Step.HUM_RUN:
            case Step.HUM_SET:
            case Step.HUM_LAYOFF:
            case Step.HUM_REPLACE:
            case Step.HUM_DISC:
                this.IsLast = isLast;
                if (this.IsLast) {
                    FloatRect bounds = this.Sprite.GetGlobalBounds();
                    this.IsHovered = bounds.Contains(mouse.X, mouse.Y);
                } else {
                    FloatRect bounds = this.Selector.GetGlobalBounds();
                    this.IsHovered = bounds.Contains(mouse.X, mouse.Y);
                }

                this.Selector = this.BuildSelector();
                break;
        }
    }

    private Vector2f GetSize() {
        return new Vector2f(this.Sprite.GetGlobalBounds().Width,
                            this.Sprite.GetGlobalBounds().Height);
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Sprite);
        if (this.IsHovered) {
            window.Draw(this.Selector);
        }
        if (this.Cross != null) {
            window.Draw(this.Cross);
        }
    }
}
