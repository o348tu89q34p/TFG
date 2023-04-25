using SFML.Graphics;
using SFML.Window;
using SFML.System;

using GameStateManager;
using Gui;

namespace GameStates
{
    class CreateState : GameState {
        private Label _title;
        private GuiComponent _menu;
        private StateManager _gameStateManager;

        public CreateState(StateManager gameStateManager, RenderWindow window) {
            this._gameStateManager = gameStateManager;

            var titlePos = new Vector2f(window.Size.X*50.0f/100.0f, window.Size.Y*10.0f/100.0f);
            Font titleFont = new Font(FontUtils.TitleFont);
            this._title = new Label("Crea la teva partida", titlePos, 80, titleFont, Color.Red);
            this._menu = new Label("Crea la teva partida", titlePos, 80, titleFont, Color.Red);
        }

        public override void BindEvents(RenderWindow window) {
            
        }

        public override void UnbindEvents(RenderWindow window) {
            
        }

        public override void Update(RenderWindow window) {
            
        }

        public override void Draw(RenderWindow window) {
            
        }
    }
}
