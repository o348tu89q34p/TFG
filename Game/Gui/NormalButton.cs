using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Game;

namespace Gui;

/*
  https://www.youtube.com/watch?v=xtBNgDncRnU
  Button done following the tutorial in this video (and the previous).
  So coded everything in the first video and I'm 6 minutes into the second.
 */
public class NormalButton {
    private Text Contents { get; }
    private RectangleShape BaseShape { get; }
    private Color IdleColor { get; }
    private Color HoverColor { get; }
    private Color ActiveColor { get; }

    private ButtonState State { get; set; }

    public NormalButton(string message, float x, float y, float w, float h, Color idle, Color hover, Color active) {
        this.BaseShape = new RectangleShape(new Vector2f(w, h)) {
            Position = new Vector2f(w, h),
            FillColor = idle
        };

        this.Contents = new Text(message, FontUtils.ButtonFont, 30) {
            FillColor = Color.White,
        };
        this.Contents.Position = new Vector2f(this.BaseShape.Position.X/2.0f - this.Contents.GetGlobalBounds().Width/2.0f,
                                              this.BaseShape.Position.Y/2.0f - this.Contents.GetGlobalBounds().Height/2.0f);

        this.IdleColor = idle;
        this.HoverColor = hover;
        this.ActiveColor = active;

        this.State = ButtonState.BTN_IDLE;
    }

    public void Move(Vector2f position) {
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect bounds = this.BaseShape.GetGlobalBounds();

        this.State = ButtonState.BTN_IDLE;
        if (bounds.Contains(mouse.X, mouse.Y)) {
            this.State = ButtonState.BTN_HOVER;
            if (Mouse.IsButtonPressed(Mouse.Button.Left)) {
                this.State = ButtonState.BTN_ACTIVE;
            }
        }

        switch (this.State) {
            case ButtonState.BTN_IDLE:
                this.BaseShape.FillColor = this.IdleColor;
                break;
            case ButtonState.BTN_HOVER:
                this.BaseShape.FillColor = this.IdleColor;
                break;
            case ButtonState.BTN_ACTIVE:
                this.BaseShape.FillColor = this.IdleColor;
                break;
            default:
                break;
        }
    }

    public void Render(RenderTarget window) {
        window.Draw(this.BaseShape);
        /*
        window.Draw(this.Body);
        window.Draw(this.CornerBL);
        window.Draw(this.CornerBR);
        window.Draw(this.CornerTL);
        window.Draw(this.CornerTR);
        window.Draw(this.LongitudinalB);
        window.Draw(this.LongitudinalT);
        window.Draw(this.LateralL);
        window.Draw(this.LateralR);
        */
    }
}
