using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Domain;
using Gui;
using Assets;

namespace Game {
    class MatchState<T, U> : GameState
        where T : Scale, new()
        where U : Scale, new()
    {
        private StateManager GSManager;
        private CoreLoop<T, U> GameData { get; }
        private NaturalCard<T, U> TheCard { get; }
        private Label AbandonButton { get; }

        public MatchState(StateManager gsManager, RenderWindow window, Rules<T, U> rules) {
            this.TheCard = new NaturalCard<T, U>(NaturalField<T>.First(), NaturalField<U>.First());
            this.GSManager = gsManager;

            // Game data.
            this.GameData = new CoreLoop<T, U>(window, rules);

            // Abandon button.
            Font buttonsFont = new Font(FontUtils.TextFont1);
            if (buttonsFont == null) {
                throw new Exception($"Failed to load font: {buttonsFont}");
            }
            Vector2f buttonPos = new Vector2f(30, 30);
            Color buttonColor = new Color(0, 70, 255);
            Color hoverColor = new Color(0, 200, 50);
            this.AbandonButton = new Label("Abandona", buttonPos, Origin.TOPLEFT, 30, buttonsFont, buttonColor, hoverColor, buttonColor);

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
        }

        public void OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
            if (sender == null) {
                return;
            }
            RenderWindow window = (RenderWindow)sender;
            if (e.Button == Mouse.Button.Left) {
                if (this.AbandonButton.MouseOn) {
                    this.GSManager.ChangeState(window, new MenuState(this.GSManager, window));
                }
                this.TheCard.OnClick(window);
            }
        }

        public override void Update(RenderWindow window) {
            this.AbandonButton.Update(window);
        }

        public override void Draw(RenderWindow window) {
            this.AbandonButton.Render(window);
            this.TheCard.Render(window);
        }
    }
}
