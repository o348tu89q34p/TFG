using GameStateManager;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

using Gui;

namespace GameStates
{
    class MenuState : GameState {
        //private Label _ui;
        private Font _font;
        private Text _titleText;
        private Text _findText;
        private Text _createText;
        private Text _quitText;
        private StateManager gameStateManager;
        public bool mouseOnFindButton;
        public bool mouseOnCreateButton;
        public bool mouseOnQuitButton;

        public MenuState(StateManager gameStateManager, RenderWindow window) {
            this.gameStateManager = gameStateManager;

            // Set font.
            this._font = new Font(FontUtils.TextFont1);
            if (this._font == null) {
                throw new Exception($"Failed to load font: {this._font}");
            }

            // Create menu buttons.
            this._titleText = new Text("Remigio Online", _font);
            this._titleText.CharacterSize = 80;
            this._titleText.FillColor = new Color(0, 100, 255);

            this._findText = new Text("Buscar partides", _font);
            this._findText.CharacterSize = 60;

            this._createText = new Text("Crear partida", _font);
            this._createText.CharacterSize = 60;

            this._quitText = new Text("Sortir", _font);
            this._quitText.CharacterSize = 60;

            // Center texts.
            FloatRect titleTextRect = this._titleText.GetLocalBounds();
            this._titleText.Origin = new Vector2f(titleTextRect.Left + titleTextRect.Width / 2, titleTextRect.Top + titleTextRect.Height / 2);
            this._titleText.Position = new Vector2f(window.Size.X / 2, 100);

            FloatRect findTextRect = this._findText.GetLocalBounds();
            this._findText.Origin = new Vector2f(findTextRect.Left + findTextRect.Width / 2, findTextRect.Top + findTextRect.Height / 2);
            this._findText.Position = new Vector2f(window.Size.X / 2, 400);

            FloatRect createTextRect = this._createText.GetLocalBounds();
            this._createText.Origin = new Vector2f(createTextRect.Left + createTextRect.Width / 2, createTextRect.Top + createTextRect.Height / 2);
            this._createText.Position = new Vector2f(window.Size.X / 2, 450);

            FloatRect quitTextRect = this._quitText.GetLocalBounds();
            this._quitText.Origin = new Vector2f(quitTextRect.Left + quitTextRect.Width / 2, quitTextRect.Top + quitTextRect.Height / 2);
            this._quitText.Position = new Vector2f(window.Size.X / 2, 500);

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
                gameStateManager.ChangeState(window, new PlayState(gameStateManager));
            }
            if (e.Code == Keyboard.Key.K) {
                gameStateManager.ChangeState(window, new LoseState(gameStateManager));
            }
            */
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
                FloatRect findTextBounds = this._findText.GetGlobalBounds();
                FloatRect createTextBounds = this._createText.GetGlobalBounds();
                FloatRect quitTextBounds = this._quitText.GetGlobalBounds();

                // Hit tests.
                if (findTextBounds.Contains(mouse.X, mouse.Y)) {
                    gameStateManager.ChangeState(window, new PlayState(gameStateManager, window));
                }

                if (createTextBounds.Contains(mouse.X, mouse.Y)) {
                    gameStateManager.ChangeState(window, new PlayState(gameStateManager, window));
                }

                if (quitTextBounds.Contains(mouse.X, mouse.Y)) {
                    window.Close();
                }
            }
        }

        public override void Update(RenderWindow window) {
            // Transform the mouse position from window coordinates to world coordinates.
            Vector2f mouse = window.MapPixelToCoords(Mouse.GetPosition(window));

            // Retrieve the bounding boxes of the text objects.
            FloatRect findTextBounds = this._findText.GetGlobalBounds();
            FloatRect createTextBounds = this._createText.GetGlobalBounds();
            FloatRect quitTextBounds = this._quitText.GetGlobalBounds();

            // Hit tests.
            if (findTextBounds.Contains(mouse.X, mouse.Y)) {
                mouseOnFindButton = true;
            } else {
                mouseOnFindButton = false;
            }

            if (createTextBounds.Contains(mouse.X, mouse.Y)) {
                mouseOnCreateButton = true;
            } else {
                mouseOnCreateButton = false;
            }

            // Press the quit button.
            if (quitTextBounds.Contains(mouse.X, mouse.Y)) {
                mouseOnQuitButton = true;
            } else {
                mouseOnQuitButton = false;
            }
        }

        public override void Draw(RenderWindow window) {
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

            window.Draw(this._titleText);
            window.Draw(this._findText);
            window.Draw(this._createText);
            window.Draw(this._quitText);
        }
    }
}
