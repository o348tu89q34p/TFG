using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Gui;

namespace Game {
    class MenuState : GameState {
        private Label StateTitle { get; }
        private Label NewGameOption { get; }
        private Label QuitOption { get; }
        private StateManager GSManager;

        public MenuState(StateManager gsManager, RenderWindow window) {
            this.GSManager = gsManager;

            // Title.
            Font titleFont = new Font(FontUtils.TitleFont);
            if (titleFont == null) {
                throw new Exception($"Failed to load font: {titleFont}");
            }
            Vector2f titlePos = new Vector2f(window.Size.X / 2, 100);
            Color titleColor = new Color(0, 70, 255);

            this.StateTitle = new Label("Remigio Challenge", titlePos, Origin.CENTER, 80, titleFont, titleColor, titleColor, titleColor);

            // Options.
            Font optionsFont = new Font(FontUtils.TextFont1);
            if (optionsFont == null) {
                throw new Exception($"Failed to load font: {optionsFont}");
            }

            Vector2f optionPos = new Vector2f(window.Size.X/2, window.Size.Y/2);
            Color hoverColor = new Color(200, 0, 12);
            this.NewGameOption = new Label("Partida nova", optionPos, Origin.CENTER, 60, optionsFont, titleColor, hoverColor, titleColor);
            optionPos = new Vector2f(optionPos.X, optionPos.Y + 100);
            this.QuitOption = new Label("Sortir", optionPos, Origin.CENTER, 60, optionsFont, titleColor, hoverColor, titleColor);

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
            // Maybe add something for bg music.
            // No key events are considered.
            /*
            RenderWindow window = (RenderWindow)sender;

            if (e.Code == Keyboard.Key.Escape) {
                window.Close();
            }
            if (e.Code == Keyboard.Key.W) {
                GSManager.ChangeState(window, new CreateState(GSManager));
            }
            if (e.Code == Keyboard.Key.K) {
                GSManager.ChangeState(window, new LoseState(GSManager));
            }
            */
        }

        public void OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
            if (sender == null) {
                return;
            }
            RenderWindow window = (RenderWindow)sender;

            if (e.Button == Mouse.Button.Left) {
                if (this.NewGameOption.MouseOn) {
                    this.GSManager.ChangeState(window, new CreateState(this.GSManager, window));
                }

                if (this.QuitOption.MouseOn) {
                    window.Close();
                }
            }
        }

        public override void Update(RenderWindow window) {
            this.StateTitle.Update(window);
            this.NewGameOption.Update(window);
            this.QuitOption.Update(window);
        }

        public override void Draw(RenderWindow window) {
            this.StateTitle.Render(window);
            this.NewGameOption.Render(window);
            this.QuitOption.Render(window);
            //window.Draw();
            /*
            if (!mouseOnFindButton) {
                this._findText.FillColor = new Color(0, 70, 255);
            } else {
                this._findText.FillColor = new Color(255, 0, 0);
            }

            if (!mouseOnCreateButton) {
                this._createText.FillColor = new Color(100, 70, 255);
            } else {
                this._createText.FillColor = new Color(255, 0, 0);
            }

            if (!mouseOnQuitButton) {
                this._quitText.FillColor = new Color(0, 70, 255);
            } else {
                this._quitText.FillColor = new Color(255, 0, 0);
            }
            */

            /*
            window.Draw(this._findText);
            window.Draw(this._createText);
            window.Draw(this._quitText);
            */
        }
    }
}
