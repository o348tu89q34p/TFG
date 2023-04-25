using SFML.Graphics;
using SFML.System;
using SFML.Window;

using Gui;
using GameStateManager;

namespace GameLogic {
    class Game {
        private RenderWindow _window;
        private StateManager _gameStateManager;

        public Game(uint width, uint height, string name) {
            // Create the window.
            var mode = new VideoMode(width, height);
            this._window = new RenderWindow(mode, name);
            this._window.KeyPressed += Window_KeyPressed;
            this._window.Closed += (s, a) => this._window.Close();

            // Initial state for the game.
            this._gameStateManager = new StateManager(this._window);
        }

        public void Run_MinimumTimeStep(int min_fps) {
            Clock clock = new Clock();
            Time timeSinceLastUpdate;
            Time timePerFrame = Time.FromSeconds(1.0F/min_fps);

            while (this._window.IsOpen) {
                ProcessEvents();
                timeSinceLastUpdate = clock.Restart();

                while (timeSinceLastUpdate > timePerFrame) {
                    timeSinceLastUpdate -= timePerFrame;
                    Update(timePerFrame);
                }

                Update(timeSinceLastUpdate);
                Render();
            }
        }

        public void Run(int fps) {
            Run_MinimumTimeStep(fps);
        }

        private void ProcessEvents() {
            this._window.DispatchEvents();
        }

        private void Update(Time deltaTime) {
            this._gameStateManager.Update(this._window);
        }

        private void DebugBackground() {
            uint width = this._window.Size.X;
            uint height = this._window.Size.Y;

            float wl = 0.0f;
            float wc = width/2.0f;
            float wr = (float)width;
            float ht = 0.0f;
            float hc = height/2.0f;
            float hb = (float)height;

            var size = new Vector2f(wc, hc);
            var poses = new Vector2f[] {
                new Vector2f(wl, ht),
                new Vector2f(wc, ht),
                new Vector2f(wl, hc),
                new Vector2f(wc, hc),
            };
            byte start = 20;
            byte step = 10;

            for (byte i = 0; i < poses.Length; i++) {
                byte next = (byte)(start + step*i);
                var q = new RectangleShape(size) {
                    Position = poses[i],
                    FillColor = new Color(next, next, next)
                };
                this._window.Draw(q);
            }
        }

        private void Render() {
            this._window.Clear();
            this._gameStateManager.Draw(this._window);
            this._window.Display();
        }

        //Window events.
        private void Window_KeyPressed(object? sender, KeyEventArgs e) {
            /*
            // Take care of the case when the object is null.
            if (sender == null) {
                return;
            }

            var window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape) {
                window.Close();
            }
            */
        }
    }
}
