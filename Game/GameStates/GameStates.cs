using SFML.Graphics;

namespace Game;

public abstract class GameState
{
    // Make the initializer private so there are no null attributes coming out.
    // public abstract void Initialize(RenderWindow window);
    public abstract void BindEvents(RenderWindow window);
    public abstract void UnbindEvents(RenderWindow window);
    public abstract void Update(RenderWindow window);
    public abstract void Draw(RenderWindow window);
}
