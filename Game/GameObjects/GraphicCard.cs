using SFML.Graphics;
using SFML.System;

namespace GameObjects;

public class GraphicCard {
    private Sprite FrontSprite { get; }

    public GraphicCard(Sprite frontSprite, Vector2f pos) {
        this.FrontSprite = frontSprite;
        this.FrontSprite.Position = pos;
    }

    public void Render(RenderWindow window) {
        window.Draw(this.FrontSprite);
    }
}
