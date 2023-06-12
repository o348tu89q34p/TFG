using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Gui;
using Domain;

namespace Game;

class LearnState : GameState
{
    private Sprite Background { get; }
    private Sprite Information { get; }

    private Label StateTitle { get; }
    private RegularButton PlayButton { get; }
    private RegularButton CancelButton { get; }
    private StateManager GSManager { get; set; }

    public LearnState(StateManager gsManager, RenderWindow window) {
        this.GSManager = gsManager;

        this.Background = new Sprite(TextureUtils.BackgroundTexture);
        this.Information = new Sprite(TextureUtils.InformationTexture) {
            Position = new Vector2f(35.0f, 75.0f)
        };

        // Title.
        Font titleFont = new Font(FontUtils.TitleFont);
        if (titleFont == null) {
            throw new Exception($"Failed to load font: {titleFont}");
        }
        Vector2f titlePos = new Vector2f(window.Size.X / 2, 100);
        Color titleColor = new Color(225, 100, 20);

        this.StateTitle = new Label("Tutorial", titlePos, Origin.CENTER, 60, titleFont, titleColor, titleColor, titleColor);

        Vector2f playPos = new Vector2f(window.Size.X - TextureUtils.IdleTexture.Size.X - 20.0f,
                                        window.Size.Y - TextureUtils.IdleTexture.Size.Y - 20.0f);
        this.PlayButton = new RegularButton(TextureUtils.IdleTexture, TextureUtils.HoverTexture, "Try it", playPos, 30);

        Vector2f cancelPos = new Vector2f(20.0f, window.Size.Y - TextureUtils.RegularTexture.Size.Y - 20.0f);
        this.CancelButton = new RegularButton(TextureUtils.RegularTexture, TextureUtils.RegularHoverTexture, "cancel", cancelPos, 20);

        this.BindEvents(window);
    }

    private Label NewMessage(RenderWindow window, string newMessage) {
        Color messageColor = new Color(255, 0, 0);
        return new Label(newMessage, new Vector2f(window.Size.X/2, window.Size.Y - 150.0f), Origin.CENTER, 30, FontUtils.StatusFont, messageColor, messageColor, messageColor);
    }

    public override void BindEvents(RenderWindow window) {
        window.Closed += new EventHandler(OnWindowClose);
        window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPress);
        window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnMouseButtonPress);
    }

    public override void UnbindEvents(RenderWindow window) {
        window.Closed -= new EventHandler(OnWindowClose);
        window.KeyPressed -= new EventHandler<KeyEventArgs>(OnKeyPress);
        window.MouseButtonPressed -= new EventHandler<MouseButtonEventArgs>(OnMouseButtonPress);
    }

    public void OnWindowClose(object? sender, EventArgs e) {
        if (sender == null) {
            return;
        }

        RenderWindow window = (RenderWindow)sender;
        window.Close();
    }

    public void OnKeyPress(object? sender, KeyEventArgs e) {
    }

    public void OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
        if (sender == null) {
            return;
        }
        RenderWindow window = (RenderWindow)sender;

        if (e.Button == Mouse.Button.Left) {
            if (this.CancelButton.IsHovered) {
                this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
            }
            if (this.PlayButton.IsHovered) {
                this.GSManager.ChangeState(window, new CreateState(this.GSManager, window));
            }
        }
    }

    public override void Update(RenderWindow window) {
        this.StateTitle.Update(window);
        this.PlayButton.Update(window);
        this.CancelButton.Update(window);
    }

    public override void Draw(RenderWindow window) {
        window.Draw(this.Background);
        window.Draw(this.Information);
        this.StateTitle.Render(window);
        this.PlayButton.Render(window);
        this.CancelButton.Render(window);
    }
}
