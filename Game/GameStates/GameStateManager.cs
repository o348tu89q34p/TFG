using SFML.Graphics;

namespace Game;

class StateManager
{
    private GameState State { get; set; }

    public StateManager(RenderWindow w) {
        this.State = new MenuState(this, w);

        // Print the state at every change.
        this.GetCurrentStateInfo();
    }

    public void ChangeState(RenderWindow w, GameState gameState) {
        // Unbind state event handlers.
        this.State.UnbindEvents(w);

        // Remove the current state.
        this.State = gameState;

        this.GetCurrentStateInfo();
    }

    public void GetCurrentStateInfo() {
        //Console.WriteLine("GameState: " + this.State);
    }

    public void Update(RenderWindow w) {
        this.State.Update(w);
    }

    public void Draw(RenderWindow w) {
        this.State.Draw(w);
    }
}
