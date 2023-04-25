using GameStates;
using SFML.Graphics;

namespace GameStateManager
{
    class StateManager {
        private List<GameState> _gameStates;
        private int _currentState;

        // New manager containing a main menu state.
        public StateManager(RenderWindow window) {
            this._gameStates = new List<GameState>();

            this._currentState = 0;
            this._gameStates.Add(new MenuState(this, window));
            //this._gameStates[this._currentState].Initialize(window);

            this.GetCurrentStateInfo();
        }

        public void ChangeState(RenderWindow window, GameState gameState) {
            // Unbind state event handlers.
            this._gameStates[this._currentState].UnbindEvents(window);

            // Remove the current state.
            this._gameStates.RemoveAt(this._currentState);

            // Add a state.
            this._currentState = 0;
            this._gameStates.Add(gameState);

            // Initialize the state.
            // TODO: MAKE SURE THAT COMMENTING THIS OUT DOESN'T BREAK ANYTHING.
            // this._gameStates[this._currentState].Initialize(window);

            this.GetCurrentStateInfo();
        }

        public void GetCurrentStateInfo() {
            Console.WriteLine("GameState: " + this._gameStates[this._currentState]);
        }

        public void Update(RenderWindow window) {
            this._gameStates[this._currentState].Update(window);
        }

        public void Draw(RenderWindow window) {
            this._gameStates[this._currentState].Draw(window);
        }
    }
}
