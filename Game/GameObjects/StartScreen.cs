using SFML.Graphics;
using SFML.System;

using Game;

public class StartScreen {
    private Text Message { get; }
    private RectangleShape Box { get; }

    public StartScreen(RenderWindow window, string name) {
        this.Message = new Text(name + " goes first!", FontUtils.StatusFont, 20) {
            FillColor = Color.Black
        };
        Vector2f textPos = new Vector2f(window.Size.X/2.0f - this.Message.GetGlobalBounds().Width/2.0f,
                                        window.Size.Y/2.0f - this.Message.GetGlobalBounds().Height/2.0f);
        this.Message.Position = textPos;

        Vector2f boxPos = new Vector2f(window.Size.X/2.0f - 100.0f, window.Size.Y/2.0f - 50.0f);
        this.Box = new RectangleShape(new Vector2f(200.0f, 100.0f)) {
            Position = boxPos,
            FillColor = new Color(118, 182, 52),
            OutlineColor = new Color(255, 247, 21),
            OutlineThickness = 4.0f
        };
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Box);
        window.Draw(this.Message);
    }
}
