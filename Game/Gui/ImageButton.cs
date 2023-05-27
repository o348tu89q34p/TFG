using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Game;

namespace Gui;

public class ImageButton {
    private Text Contents { get; }
    private Sprite CurrentSprite { get; set; }
    private ButtonState State { get; set; }

    public ImageButton(string contents, Vector2f pos) {
        this.Contents = new Text(contents, FontUtils.ButtonFont, 30) {
            FillColor = Color.Black
        };

        this.CurrentSprite = new Sprite(TextureUtils.DisabledTexture);
        this.CurrentSprite.Position = PosOps.ComputeOrigin(pos, this.CurrentSprite.GetGlobalBounds(), Origin.BOTTOMRIGHT);

        this.State = ButtonState.BTN_IDLE;

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

    public void Enable() {
        this.State = ButtonState.BTN_IDLE;
    }

    public void Disable() {
        this.State = ButtonState.BTN_DISABLED;
    }

    private bool IsActive() {
        return this.State != ButtonState.BTN_DISABLED;
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect bounds = this.CurrentSprite.GetGlobalBounds();

        if (this.IsActive()) {
            this.State = ButtonState.BTN_IDLE;
            if (bounds.Contains(mouse.X, mouse.Y)) {
                this.State = ButtonState.BTN_HOVER;
                if (Mouse.IsButtonPressed(Mouse.Button.Left)) {
                    this.State = ButtonState.BTN_ACTIVE;
                }
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
