using SFML.Graphics;
using SFML.System;

using Game;

namespace GameObjects;

public class StatusBar {
    private RectangleShape Rect { get; }
    private Text Message { get; set; }

    private const float Width = 0.60f;
    private const float Height = 25.0f;
    private const float Valign = 0.70f;

    public StatusBar(RenderWindow window, string message) {
        float width = (float)window.Size.X*Width;
        float xStart = (float)window.Size.X/2.0f - width/2.0f;
        float yStart = (float)window.Size.Y*Valign;

        this.Rect = new RectangleShape(new Vector2f(width, Height)) {
            Position = new Vector2f(xStart, yStart),
            FillColor = new Color(246, 197, 94),
            OutlineColor = new Color(200, 128, 13),
            OutlineThickness = 2
        };

        uint fontSize = 25;
        this.Message = new Text(message, FontUtils.StatusFont, fontSize) {
            FillColor = Color.Black
        };
        float msgX = this.Rect.Position.X + this.Rect.GetGlobalBounds().Width/2.0f - this.Message.GetGlobalBounds().Width/2.0f;
        float msgY = this.Rect.Position.Y + this.Rect.GetGlobalBounds().Height/2.0f - this.Message.GetGlobalBounds().Height/2.0f - fontSize/2.0f;
        this.Message.Position = new Vector2f(msgX, msgY);
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Rect);
        window.Draw(this.Message);
    }
}
