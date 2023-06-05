using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace GameObjects;

public class GraphicCard {
    private Sprite FrontSprite { get; }

    private RectangleShape Selector { get; set; }
    private float Visible { get; set; }
    private bool IsLast { get; set; }
    private bool Taken { get; set; }

    public bool IsHovered { get; private set; }

    public GraphicCard(Sprite sprite, float visible) {
        this.FrontSprite = sprite;
        this.Selector = this.BuildSelector();
        this.Visible = visible;
        this.IsLast = false;
        this.IsHovered = false;
        this.Taken = false;
    }

    public GraphicCard(Sprite frontSprite, Vector2f pos) {
        this.FrontSprite = frontSprite;
        this.FrontSprite.Position = pos;
        this.Selector = this.BuildSelector();
        this.Taken = false;
    }

    public Vector2f GetPosition() {
        return this.FrontSprite.Position;
    }

    public Sprite GetSprite() {
        return this.FrontSprite;
    }

    public void UpdatePosition(Vector2f pos) {
        this.FrontSprite.Position = pos;
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
        float h = this.FrontSprite.GetGlobalBounds().Height;
        float w;
        if (this.IsLast) {
            w = this.FrontSprite.GetGlobalBounds().Width;
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
            return;
        }

        switch (step) {
            case Step.HUM_RUN:
            case Step.HUM_SET:
            case Step.HUM_LAYOFF:
            case Step.HUM_REPLACE:
            case Step.HUM_DISC:
                this.IsLast = isLast;
                if (this.IsLast) {
                    FloatRect bounds = this.FrontSprite.GetGlobalBounds();
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
        return new Vector2f(this.FrontSprite.GetGlobalBounds().Width,
                            this.FrontSprite.GetGlobalBounds().Height);
    }

    public void Render(RenderWindow window) {
        window.Draw(this.FrontSprite);
        if (this.IsHovered) {
            window.Draw(this.Selector);
        }
    }
}
