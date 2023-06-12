using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Game;

namespace Gui;

public class RegularButton {
    private Texture BgNormal { get; }
    private Texture BgHover { get; }
    private Text Contents { get; }
    private Sprite BgSprite { get; set; }
    private Vector2f Position { get; }
    public bool IsHovered { get; private set; }

    public RegularButton(Texture bgNormal, Texture bgHover, string contents, Vector2f position, uint size) {
        this.BgNormal = bgNormal;
        this.BgHover = bgHover;
        this.IsHovered = false;
        this.Position = position;
        this.BgSprite = this.SetSprite(this.BgNormal);

        this.Contents = new Text(contents, FontUtils.ButtonFont, size) {
            FillColor = Color.Black
        };

        float cw = this.Contents.GetGlobalBounds().Width/2.0f;
        float ch = this.Contents.GetGlobalBounds().Height/2.0f;
        float bw = this.BgSprite.GetGlobalBounds().Width/2.0f;
        float bh = this.BgSprite.GetGlobalBounds().Height/2.0f;

        this.Contents.Position = new Vector2f(this.Position.X + bw - cw, this.Position.Y + bh - ch*1.90f);
    }

    public RegularButton(Texture bgNormal, Texture bgHover, string contents, Vector2f position) :
        this(TextureUtils.RegularTexture, TextureUtils.RegularHoverTexture, contents, position, 15) {
    }

    public RegularButton(string contents, Vector2f position) :
        this(TextureUtils.RegularTexture, TextureUtils.RegularHoverTexture, contents, position) {
    }

    private Sprite SetSprite(Texture texture) {
        Sprite sprite = new Sprite(texture) {
            Position = this.Position
        };

        return sprite;
    }

    public Vector2f GetPosition() {
        return this.BgSprite.Position;
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));
        FloatRect bounds = this.BgSprite.GetGlobalBounds();

        this.IsHovered = bounds.Contains(mouse.X, mouse.Y);
        if (this.IsHovered) {
            this.BgSprite = this.SetSprite(this.BgHover);
        } else {
            this.BgSprite = this.SetSprite(this.BgNormal);
        }
    }

    public void Render(RenderTarget window) {
        window.Draw(this.BgSprite);
        window.Draw(this.Contents);
    }
}
