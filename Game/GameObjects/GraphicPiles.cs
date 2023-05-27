using SFML.Graphics;
using SFML.System;

namespace GameObjects;

public class GraphicPiles {
    private RectangleShape Background { get; }
    private Sprite Pick { get; }
    private Sprite Disc { get; }

    public GraphicPiles(RenderWindow window, (Sprite, Sprite) sprites) {
        float bgWidth = (sprites.Item1.GetGlobalBounds().Width + 30.0f);
        float bgHeight = (sprites.Item1.GetGlobalBounds().Height +
                          sprites.Item2.GetGlobalBounds().Height + 40.0f);
        float posX = 30.0f;
        float posY = ((float)window.Size.Y)*3.0f/4.0f - bgHeight/2.0f;
        this.Background = new RectangleShape(new Vector2f(bgWidth, bgHeight)) {
            Position = new Vector2f(posX, posY),
            FillColor = new Color(0, 0, 0, 80)
        };

        this.Pick = sprites.Item1;
        float pickX = this.Background.Position.X + bgWidth/2.0f - this.Pick.GetGlobalBounds().Width/2.0f;
        float pickY = this.Background.Position.Y + bgHeight/4.0f - this.Pick.GetGlobalBounds().Height/2.0f;
        this.Pick.Position = new Vector2f(pickX, pickY);

        this.Disc = sprites.Item2;
        float discX = this.Background.Position.X + bgWidth/2.0f - this.Disc.GetGlobalBounds().Width/2.0f;
        float discY = this.Background.Position.Y + bgHeight*3.0f/4.0f - this.Disc.GetGlobalBounds().Height/2.0f;
        this.Disc.Position = new Vector2f(discX, discY);
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Background);
        window.Draw(this.Pick);
        window.Draw(this.Disc);
    }
}
