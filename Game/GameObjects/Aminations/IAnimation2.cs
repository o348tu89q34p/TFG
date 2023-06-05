using SFML.Graphics;
using SFML.System;

namespace Animation;

public interface IAnimation2 {
    bool RunAnimation();
    void Render(RenderWindow window);

    public static Vector2f ComputeVelocity(Vector2f start, Vector2f finish, float times) {
        float diffX = finish.X - start.X;
        float diffY = finish.Y - start.Y;
        float sqrX = diffX*diffX;
        float sqrY = diffY*diffY;
        float d = (float)Math.Sqrt((double)(sqrX + sqrY));

        float distanceToMove = d/times;

        float ux = diffX/d;
        float uy = diffY/d;

        return new Vector2f(distanceToMove*ux, distanceToMove*uy);
    }

}
