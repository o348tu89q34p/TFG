using GameStateManager;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace GameStates
{
    class PlayState : GameState {
        private Font _font;
        private Text _titleText;
        private StateManager gameStateManager;

        public PlayState(StateManager gameStateManager, RenderWindow window) {
            this.gameStateManager = gameStateManager;

            this._font = new Font(FontUtils.TitleFont);
            if (this._font == null) {
                throw new Exception($"Failed to load font: {this._font}");
            }

            // Create menu buttons.
            this._titleText = new Text("Gameplay", _font);
            this._titleText.CharacterSize = 80;
            this._titleText.FillColor = new Color(0, 100, 255);

            // Center texts.
            FloatRect titleTextRect = this._titleText.GetLocalBounds();
            this._titleText.Origin = new Vector2f(titleTextRect.Left + titleTextRect.Width / 2, titleTextRect.Top + titleTextRect.Height / 2);
            this._titleText.Position = new Vector2f(window.Size.X / 2, 100);

            window.SetMouseCursorVisible(true);

            this.BindEvents(window);
        }

        public override void BindEvents(RenderWindow window) {
            window.LostFocus += new EventHandler(OnWindowLostFocus);
            window.Closed += new EventHandler(OnWindowClose);
            window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPress);
            window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnMouseButtonPress);
            window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(OnMouseButtonRelease);
        }

        public override void UnbindEvents(RenderWindow window) {
            window.LostFocus -= new EventHandler(OnWindowLostFocus);
            window.Closed -= new EventHandler(OnWindowClose);
            window.KeyPressed -= new EventHandler<KeyEventArgs>(OnKeyPress);
            window.MouseButtonPressed -= new EventHandler<MouseButtonEventArgs>(OnMouseButtonPress);
            window.MouseButtonReleased -= new EventHandler<MouseButtonEventArgs>(OnMouseButtonRelease);
        }

        public void OnWindowLostFocus(object? sender, EventArgs e) {
            if (sender == null) {
                return;
            }

            RenderWindow window = (RenderWindow)sender;
        }

        public void OnWindowClose(object? sender, EventArgs e) {
            if (sender == null) {
                return;
            }

            RenderWindow window = (RenderWindow)sender;

            window.Close();
        }

        public void OnMouseButtonPress(object? sender, MouseButtonEventArgs e) {
            if (sender == null) {
                return;
            }
            RenderWindow window = (RenderWindow)sender;

            if (e.Button == Mouse.Button.Left) {
                // Transform the mouse position from window coordinates to world coordinates.
                Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));

                // Retrieve the bounding boxes of the text objects.
                FloatRect findTextBounds = this._titleText.GetGlobalBounds();
                // Hit tests.
                if (findTextBounds.Contains(mouse.X, mouse.Y)) {
                    gameStateManager.ChangeState(window, new MenuState(gameStateManager, window));
                }
            }
        }

        public void OnMouseButtonRelease(object? sender, MouseButtonEventArgs e) {
            if (sender == null) {
                return;
            }

            RenderWindow window = (RenderWindow)sender;
        }

        public void OnKeyPress(object? sender, KeyEventArgs e) {
            if (sender == null) {
                return;
            }
            RenderWindow window = (RenderWindow)sender;

            if (e.Code == Keyboard.Key.Escape) {
                gameStateManager.ChangeState(window, new MenuState(gameStateManager, window));
            }
        }

        public override void Update(RenderWindow window) {
        }

        public void DrawTexts(RenderWindow window) {
            window.Draw(this._titleText);
        }

        public override void Draw(RenderWindow window) {
            DrawTexts(window);
        }
    }
}
