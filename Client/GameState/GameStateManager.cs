using GameStates;
using SFML.Graphics;

namespace GameStateManager
{
    class StateManager {
        private GameState _state;

        /*
         * Initialize the state manager with the menu being the
         * current state.
         */
        public StateManager(RenderWindow win) {
            this._state = new MenuState(this, win);

            // Print the state at every change.
            this.GetCurrentStateInfo();
        }

        public void ChangeState(RenderWindow win, GameState gameState) {
            // Unbind state event handlers.
            this._state.UnbindEvents(win);

            // Remove the current state.
            this._state = gameState;

            this.GetCurrentStateInfo();
        }

        public void GetCurrentStateInfo() {
            Console.WriteLine("GameState: " + this._state);
        }

        public void Update(RenderWindow win) {
            this._state.Update(win);
        }

        public void Draw(RenderWindow win) {
            this._state.Draw(win);
        }
    }
}
