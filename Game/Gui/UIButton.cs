using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace Gui;

/*
  Button made out of shapes using circles at the corners to give it
  rounded corners.
 */
public class UIButton {
    private RectangleShape BaseShape { get; }
    private RectangleShape LateralL { get; }
    private RectangleShape LateralR { get; }
    private RectangleShape LongitudinalT { get; }
    private RectangleShape LongitudinalB { get; }
    private CircleShape CornerTL { get; }
    private CircleShape CornerTR { get; }
    private CircleShape CornerBL { get; }
    private CircleShape CornerBR { get; }

    private Shape[] Corners { get; }

    private Text Contents { get; }
    private Color IdleColor { get; }
    private Color HoverColor { get; }
    private Color ActiveColor { get; }

    private ButtonState State { get; set; }

    public UIButton(string message, float x, float y, float w, float h, float p, Color idle, Color hover, Color active) {
        this.Corners = new Shape[10];

        uint textSize = 40;
        Font textFont = new Font(FontUtils.ButtonFont);
        this.Contents = new Text(message, textFont, textSize) {
            FillColor = Color.Black
        };

        float margin = 10.0f;
        FloatRect textBounds = this.Contents.GetGlobalBounds();
        this.BaseShape = new RectangleShape(new Vector2f(textBounds.Width, textBounds.Height)) {
            FillColor = idle,
            Position = new Vector2f(margin, margin)
        };

        float r = margin/2.0f;
        this.LateralL = new RectangleShape(new Vector2f(margin, textBounds.Height + margin)) {
            FillColor = idle,
            Position = new Vector2f(0.0f, r)
        };
        this.LateralR = new RectangleShape(new Vector2f(margin, textBounds.Height + margin)) {
            FillColor = idle,
            Position = new Vector2f(margin + this.BaseShape.GetGlobalBounds().Width, r)
        };

        this.LongitudinalT = new RectangleShape(new Vector2f(textBounds.Width + margin, margin)) {
            FillColor = idle,
            Position = new Vector2f(r, 0.0f)
        };
        this.LongitudinalB = new RectangleShape(new Vector2f(textBounds.Width + margin, margin)) {
            FillColor = idle,
            Position = new Vector2f(r, margin + this.BaseShape.GetGlobalBounds().Height)
        };

        this.CornerTL = new CircleShape(r) {
            FillColor = idle,
            Position = new Vector2f(0, 0)
        };
        this.CornerTR = new CircleShape(r) {
            FillColor = idle,
            Position = new Vector2f(margin + this.BaseShape.GetGlobalBounds().Width, 0.0f)
        };
        this.CornerBL = new CircleShape(r) {
            FillColor = idle,
            Position = new Vector2f(0.0f, margin + this.BaseShape.GetGlobalBounds().Height)
        };
        this.CornerBR = new CircleShape(r) {
            FillColor = idle,
            Position = new Vector2f(margin + this.BaseShape.GetGlobalBounds().Width, margin + this.BaseShape.GetGlobalBounds().Height)
        };
    }

    public void Move(Vector2f position) {
    }

    public void Update(RenderWindow window) {
    }

    public void Render(RenderTarget window) {
        window.Draw(this.BaseShape);
        window.Draw(this.CornerBL);
        window.Draw(this.CornerBR);
        window.Draw(this.CornerTL);
        window.Draw(this.CornerTR);
        window.Draw(this.LongitudinalB);
        window.Draw(this.LongitudinalT);
        window.Draw(this.LateralL);
        window.Draw(this.LateralR);
    }
}
