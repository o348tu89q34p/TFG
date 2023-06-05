using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Game;

namespace Gui;

public class ImageButton {
    private Text Contents { get; }
    private Sprite CurrentSprite { get; set; }
    private ButtonState State { get; set; }
    public bool Hovering { get; private set; }

    public ImageButton(string contents, Vector2f pos) {
        this.Contents = new Text(contents, FontUtils.ButtonFont, 30) {
            FillColor = Color.Black
        };

        this.CurrentSprite = new Sprite(TextureUtils.DisabledTexture);
        this.CurrentSprite.Position = PosOps.ComputeOrigin(pos, this.CurrentSprite.GetGlobalBounds(), Origin.BOTTOMRIGHT);

        this.State = ButtonState.BTN_IDLE;
        this.Hovering = false;

        this.ContentsPosition();
    }

    private Vector2f TextSize() {
        return new Vector2f(this.Contents.GetGlobalBounds().Width, this.Contents.GetGlobalBounds().Height);
    }

    private Vector2f ButtonSize() {
        return new Vector2f(this.CurrentSprite.GetGlobalBounds().Width, this.CurrentSprite.GetGlobalBounds().Height);
    }

    private void ContentsPosition() {
        this.Contents.Position = PosOps.RelativeTo(this.CurrentSprite.Position, this.ButtonSize(), this.TextSize(), Origin.CENTER);
        this.Contents.Position = this.Contents.Position - (new Vector2f(0.0f, this.Contents.GetGlobalBounds().Height/2.0f));
    }

    private void UpdateSprite(Texture t) {
        Vector2f pos = this.CurrentSprite.Position;
        this.CurrentSprite = new Sprite(t) {
            Position = pos
        };
    }

    public void Activate() {
        this.State = ButtonState.BTN_ACTIVE;
    }

    public void Deactivate() {
        this.State = ButtonState.BTN_IDLE;
    }

    public void Enable() {
        if (!this.IsActivated()) {
            this.State = ButtonState.BTN_IDLE;
        }
    }

    public void Disable() {
        this.State = ButtonState.BTN_DISABLED;
    }

    public bool IsEnabled() {
        return this.State != ButtonState.BTN_DISABLED;
    }

    public bool IsActivated() {
        return this.State == ButtonState.BTN_ACTIVE;
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect bounds = this.CurrentSprite.GetGlobalBounds();

        this.Hovering = bounds.Contains(mouse.X, mouse.Y);
        if (this.IsEnabled()) {
            if (this.IsActivated()) {
                this.State = ButtonState.BTN_ACTIVE;
            } else if (!this.IsActivated() && this.Hovering) {
                this.State = ButtonState.BTN_HOVER;
            } else {
                this.State = ButtonState.BTN_IDLE;
            }
        } else {
            this.State = ButtonState.BTN_DISABLED;
        }

        switch (this.State) {
            case ButtonState.BTN_IDLE:
                this.UpdateSprite(TextureUtils.IdleTexture);
                break;
            case ButtonState.BTN_HOVER:
                this.UpdateSprite(TextureUtils.HoverTexture);
                break;
            case ButtonState.BTN_ACTIVE:
                this.UpdateSprite(TextureUtils.ActiveTexture);
                break;
            case ButtonState.BTN_DISABLED:
                this.UpdateSprite(TextureUtils.DisabledTexture);
                break;
            default:
                break;
        }
    }

    public void Render(RenderTarget window) {
        window.Draw(this.CurrentSprite);
        window.Draw(this.Contents);
    }
}
