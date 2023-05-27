using SFML.Graphics;
using SFML.System;

namespace Gui;

public static class PosOps {
    /*
      Given a position in the plane, the dimentions of an object and a corner value,
      return the point that the top left corner of the object should be set so the corner value
      given sits on top of the original point.
     */
    public static Vector2f ComputeOrigin(Vector2f point, FloatRect bounds, Origin name) {
        float x;
        float y;
        switch (name) {
            case Origin.TOPLEFT:
                x = point.X;
                y = point.Y;
                break;
            case Origin.TOPRIGHT:
                x = point.X - bounds.Width;
                y = point.Y;
                break;
            case Origin.BOTTOMLEFT:
                x = point.X;
                y = point.Y - bounds.Height;
                break;
            case Origin.BOTTOMRIGHT:
                x = point.X - bounds.Width;
                y = point.Y - bounds.Height;
                break;
            case Origin.CENTER:
                x = point.X - bounds.Width/2.0f;
                y = point.Y - bounds.Height/2.0f;
                break;
            case Origin.TOPCENTER:
                x = point.X - bounds.Width/2.0f;
                y = point.Y;
                break;
            case Origin.BOTTOMCENTER:
                x = point.X + bounds.Width/2.0f;
                y = point.Y + bounds.Height;
                break;
            default:
                throw new Exception("Missing imlementation for origin value.");
        }

        return new Vector2f(x, y);
    }

    /*
      Given a point, a container dimention, an object dimention and a position, compute the position that corresponds
      to placing the object inside the container in the position specified.
     */
    public static Vector2f RelativeTo(Vector2f point, Vector2f cont, Vector2f size, Origin name) {
        float x;
        float y;
        switch (name) {
            case Origin.TOPLEFT:
                x = point.X;
                y = point.Y;
                break;
            case Origin.TOPRIGHT:
                x = point.X + cont.X - size.X;
                y = point.Y;
                break;
            case Origin.BOTTOMLEFT:
                x = point.X;
                y = point.Y + cont.Y - size.Y;
                break;
            case Origin.BOTTOMRIGHT:
                x = point.X + cont.X - size.X;
                y = point.Y + cont.Y - size.Y;
                break;
            case Origin.CENTER:
                x = point.X + cont.X/2.0f - size.X/2.0f;
                y = point.Y + cont.Y/2.0f - size.Y/2.0f;
                break;
            case Origin.TOPCENTER:
                x = point.X + cont.X/2.0f - size.X/2.0f;
                y = point.Y;
                break;
            case Origin.BOTTOMCENTER:
                x = point.X + cont.X/2.0f - size.X/2.0f;
                y = point.Y + cont.Y - size.Y;
                break;
            case Origin.LEFTCENTER:
                x = point.X;
                y = point.Y + cont.Y/2.0f - size.Y/2.0f;
                break;
            case Origin.RIGHTCENTER:
                x = point.X + cont.X - size.X;
                y = point.Y + cont.Y/2.0f - size.Y/2.0f;
                break;
            default:
                throw new Exception("Missing imlementation for origin value.");
        }

        return new Vector2f(x, y);
    }

    public static Vector2f CenterDims(Vector2f pos, FloatRect bounds) {
        return new Vector2f(pos.X + bounds.Width/2.0f, pos.Y + bounds.Height/2.0f);
    }
}
