using SFML.Graphics;
using SFML.System;

namespace Animation;

public class SlideAnimation : IAnimation2 {
    private Sprite Sprite { get; }
    private Vector2f StartPos { get; }
    private Vector2f EndPos { get; }
    private Vector2f Velocity { get; }

    public SlideAnimation(Sprite sprite, Vector2f startPos, Vector2f endPos, float speed) {
        this.Sprite = sprite;
        this.StartPos = startPos;
        this.EndPos = endPos;
        this.Velocity = IAnimation2.ComputeVelocity(startPos, endPos, speed);

        this.Sprite.Position = this.StartPos;
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
        if (!this.AreClose(this.Sprite.Position, this.EndPos)) {
            this.Sprite.Position = this.Sprite.Position + Velocity;
            return true;
        } else {
            return false;
        }
    }

    public void Render(RenderWindow window) {
        window.Draw(this.Sprite);
    }
}
