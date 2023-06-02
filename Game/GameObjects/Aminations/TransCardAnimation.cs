using SFML.Graphics;
using SFML.System;

namespace Animation;

public class TransCardAnimation<T> : IAnimation<T> {
    private Sprite CardSprite { get; }
    private Vector2f EndPos { get; }
    private Vector2f Velocity { get; }
    private T NewValue { get; }

    public TransCardAnimation(Sprite sprite, Vector2f initPos, Vector2f endPos, float times, T newValue) {
        this.CardSprite = sprite;
        this.CardSprite.Position = initPos;
        this.EndPos = endPos;
        this.Velocity = IAnimation<T>.GetVelocity(initPos, endPos, times);
        this.NewValue = newValue;
    }

    private float EuclideanDistance(Vector2f p1, Vector2f p2) {
        float diffX = p1.X - p2.X;
        float diffY = p1.Y - p2.Y;
        float sqrX = diffX*diffX;
        float sqrY = diffY*diffY;
        return (float)Math.Sqrt((double)(sqrX + sqrY));
    }

    private float ManhattanDistance(Vector2f p1, Vector2f p2) {
        float diffX = Math.Abs(p1.X - p2.X);
        float diffY = Math.Abs(p1.Y - p2.Y);
        return diffX + diffY;
    }

    private bool AreClose(Vector2f p1, Vector2f p2) {
        return this.EuclideanDistance(p1, p2) < 10.0f;
    }

    public bool RunAnimation() {
        if (!this.AreClose(this.CardSprite.Position, this.EndPos)) {
            this.CardSprite.Position = this.CardSprite.Position + Velocity;
            return true;
        } else {
            return false;
        }
    }

    public T GetNewState() {
        return this.NewValue;
    }

    public void Render(RenderWindow window) {
        window.Draw(this.CardSprite);
    }
}
