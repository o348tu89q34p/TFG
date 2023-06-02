using SFML.Graphics;

namespace Animation;

public class EmptyAnimation<T> : IAnimation<T> {
    private T? V { get; }

    public EmptyAnimation() {
        this.V = default(T);
    }

    public bool RunAnimation() {
        return true;
    }

    public T GetNewState() {
        if (this.V == null) {
            throw new Exception();
        }

        return this.V;
    }

    public void Render(RenderWindow window) {
    }
}
