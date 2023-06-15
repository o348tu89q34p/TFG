using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Gui;

namespace Game;

class MenuState : GameState
{
    private Sprite Background { get; }

    private Sprite Title { get; }
    private RectangleShape ButtonsBg { get; }
    private RegularButton PlayButton { get; }
    private RegularButton LearnButton { get; }
    private RegularButton QuitButton { get; }
    private StateManager GSManager;

    public MenuState(StateManager gsManager, RenderWindow window) {
        this.GSManager = gsManager;

        this.Background = new Sprite(TextureUtils.BackgroundTexture);

        // Title.
        Vector2f titlePos = new Vector2f(window.Size.X / 2, 100);
        Color titleColor = new Color(0, 70, 255);

        //this.StateTitle = new Label("Remigio Challenge", titlePos, Origin.CENTER, 80, titleFont, titleColor, titleColor, titleColor);
        this.Title = new Sprite(TextureUtils.TitleTexture) {
            Position = new Vector2f(0.0f, 60.0f)
        };

        float rectW = TextureUtils.IdleTexture.Size.X + 40.0f;
        float rectH = TextureUtils.IdleTexture.Size.Y*3 + 20.0f*2 + 2*30.0f;
        float rectX = window.Size.X/2.0f - rectW/2.0f;
        float rectY = window.Size.Y - rectH - 30.0f;
        this.ButtonsBg = new RectangleShape(new Vector2f(rectW, rectH)) {
            Position = new Vector2f(rectX, rectY),
            FillColor = new Color(0, 0, 0, 100),
            OutlineColor = new Color(0, 200, 0)
        };

        Texture idle = TextureUtils.IdleTexture;
        Texture hover = TextureUtils.HoverTexture;
        float buttonX = rectX + this.ButtonsBg.GetGlobalBounds().Width/2.0f - idle.Size.X/2.0f;
        float buttonY = rectY + 20.0f;
        float buttonH = TextureUtils.IdleTexture.Size.Y;

        Vector2f playPos = new Vector2f(buttonX, buttonY);
        this.PlayButton = new RegularButton(idle, hover, "play", playPos, 30);
        Vector2f learnPos = new Vector2f(buttonX, playPos.Y + idle.Size.Y + 30.0f);
        this.LearnButton = new RegularButton(idle, hover, "tutorial", learnPos, 30);
        Vector2f quitPos = new Vector2f(buttonX, learnPos.Y + idle.Size.Y + 30.0f);
        this.QuitButton = new RegularButton(idle, hover, "quit", quitPos, 30);


        window.SetMouseCursorVisible(true);

        this.BindEvents(window);
    }

    // The events that can occur on a main menu.
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
        if (sender == null) {
            return;
        }
    }

    public void OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
        if (sender == null) {
            return;
        }
        RenderWindow window = (RenderWindow)sender;

        if (e.Button == Mouse.Button.Left) {
            if (this.PlayButton.IsHovered) {
                this.GSManager.ChangeState(window, new CreateState(this.GSManager, window));
            }
            if (this.LearnButton.IsHovered) {
                this.GSManager.ChangeState(window, new LearnState(this.GSManager, window));
            }
            if (this.QuitButton.IsHovered) {
                window.Close();
            }
        }
    }

    public override void Update(RenderWindow window) {
        this.PlayButton.Update(window);
        this.LearnButton.Update(window);
        this.QuitButton.Update(window);
    }

    public override void Draw(RenderWindow window) {
        window.Draw(this.Background);
        window.Draw(this.Title);
        window.Draw(this.ButtonsBg);
        this.PlayButton.Render(window);
        this.LearnButton.Render(window);
        this.QuitButton.Render(window);
    }
}
