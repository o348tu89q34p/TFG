using SFML.Window;
using SFML.Graphics;
using SFML.System;

// Make player action be from game.
using Domain;
using Game;
using Gui;

namespace GameObjects;

public class PlayerView {
    private Vector2f Point { get; }
    private Vector2f Dimentions { get; }

    private List<GraphicCard> Hand { get; }

    private RectangleShape StatusLine { get; }
    public Text Status { private get; set; }

    private List<ImageButton> Buttons { get; }

    /*
      Make run
      Make set
      Replace*
      Discard
      Sort by suit
      Sort by rank
     */

    // pass a class "gameConfig" that has if we need replace button and other stuff
    public PlayerView(RenderWindow window, List<GraphicCard> hand, string message, List<(string, PlayerAction)> buttons) {
        // Top left coner of the player's view.
        this.Point = new Vector2f(0.0f, window.Size.Y - window.Size.Y*0.35f);
        // Area.
        this.Dimentions = new Vector2f(window.Size.X, window.Size.Y - window.Size.Y*0.35f);

        this.Hand = hand;

        var slDims = new Vector2f(600.0f, 30.0f);
        this.StatusLine = new RectangleShape(slDims) {
            FillColor = new Color(248, 204, 75),
            OutlineColor = new Color(248, 148, 20),
            OutlineThickness = 2
        };
        this.StatusLine.Position = PosOps.RelativeTo(this.Point, this.Dimentions, slDims, Origin.TOPCENTER);

        this.Status = new Text(message, FontUtils.StatusFont, 20) {
            FillColor = Color.Black
        };
        var statusSize = new Vector2f(this.Status.GetGlobalBounds().Width, this.Status.GetGlobalBounds().Height + 10);
        this.Status.Position = PosOps.RelativeTo(this.StatusLine.Position, this.StatusLine.Size, statusSize, Origin.CENTER);

        this.Buttons = new List<ImageButton>(buttons.Count);
        for (int i = 0; i < buttons.Count; i++) {
            //this.Buttons.Add(ActionButton item);
        }
    }

    public void Update(RenderTarget window) {
        // call all the updates.
    }

    public void Render(RenderTarget window) {
        window.Draw(this.StatusLine);
        window.Draw(this.Status);
    }
}
