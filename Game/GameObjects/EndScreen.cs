using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Gui;
using Game;

namespace GameObjects;

public class EndScreen {
    private RectangleShape Bg { get; }
    private RectangleShape TopBar { get; }
    private Text Title { get; }
    private List<CardHolder> Rows { get; }
    private RegularButton CloseButton { get; }
    private ConvexShape CollapseArrow { get; }

    private bool HoverTop { get; set; }
    private bool DrawBody { get; set; }

    public EndScreen(RenderWindow window, string winner, List<(string, List<Sprite>)> info) {
        int n = info.Count;
        float rowHeight = TextureUtils.CardHeight/2.5f + 15.0f;
        float space = 5.0f;
        float allHeight = rowHeight*n + space*(n - 1);

        float bgWidth = window.Size.X*0.8f;
        float bgHeight = 20.0f + 30.0f + allHeight + 50.0f + 20.0f;
        // Make its vertical alignment fixed.
        this.Bg = new RectangleShape(new Vector2f(bgWidth, bgHeight)) {
            Position = new Vector2f(window.Size.X/2.0f - bgWidth/2.0f, 50.0f),
            FillColor = new Color(118, 182, 52),
        };

        this.TopBar = new RectangleShape(new Vector2f(bgWidth, 20.0f)) {
            Position = this.Bg.Position
        };

        this.Title = new Text(winner + " won the game.", FontUtils.StatusFont, 20) {
            Position = new Vector2f(this.Bg.Position.X + 10.0f, this.Bg.Position.Y + 30.0f),
            FillColor = Color.Black
        };

        Vector2f size = new Vector2f(bgWidth*0.9f, rowHeight);
        this.Rows = new List<CardHolder>(info.Count);
        for (int i = 0; i < info.Count; i++) {
            Vector2f p = new Vector2f(this.Bg.Position.X + bgWidth*0.05f, (this.Title.Position.Y + this.Title.GetGlobalBounds().Height*2) + rowHeight*i);
            this.Rows.Add(new CardHolder(p, size, info[i].Item1, info[i].Item2));
        }

        // Close Button
        Vector2f buttonPos = new Vector2f(this.Bg.Position.X + bgWidth/2.0f - TextureUtils.RegularTexture.Size.X/2.0f,
                                          this.Bg.Position.Y + bgHeight - TextureUtils.RegularTexture.Size.Y - 10.0f);
        this.CloseButton = new RegularButton("Finish", buttonPos);
        // Collapse arrow
        this.CollapseArrow = new ConvexShape() {
            FillColor = new Color(200, 160, 70),
            Position = new Vector2f(this.Bg.Position.X + bgWidth - 20.0f - 10.0f, this.Bg.Position.Y + 5.0f)
        };
        this.CollapseArrow.SetPointCount(3);
        this.CollapseArrow.SetPoint(0, new Vector2f(0.0f, 10.0f));
        this.CollapseArrow.SetPoint(1, new Vector2f(20.0f, 10.0f));
        this.CollapseArrow.SetPoint(2, new Vector2f(10.0f, 0.0f));

        this.HoverTop = false;
        this.DrawBody = true;
    }

    public void ToggleTop() {
        if (this.HoverTop) {
            Vector2f pos = this.CollapseArrow.Position;
            if (this.DrawBody) {
                this.CollapseArrow.Scale = new Vector2f(1.0f, -1.0f);
                this.CollapseArrow.Position = new Vector2f(pos.X, pos.Y + 10.0f);
            } else {
                this.CollapseArrow.Scale = new Vector2f(1.0f, 1.0f);
                this.CollapseArrow.Position = new Vector2f(pos.X, pos.Y - 10.0f);
            }
            this.DrawBody = !this.DrawBody;
        }
    }

    public bool Close() {
        return this.CloseButton.IsHovered;
    }

    public void Update(RenderWindow window) {
        Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));

        this.HoverTop = this.TopBar.GetGlobalBounds().Contains(mouse.X, mouse.Y);
        this.CloseButton.Update(window);
    }

    public void Render(RenderWindow window) {
        if (this.DrawBody) {
            window.Draw(this.Bg);
            window.Draw(this.Title);
            this.CloseButton.Render(window);

            for (int i = 0; i < this.Rows.Count; i++) {
                this.Rows[i].Render(window);
            }
        }
        window.Draw(this.TopBar);
        window.Draw(this.CollapseArrow);
    }
}
