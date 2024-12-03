

using Microsoft.Xna.Framework;

namespace UnamedGame.Abstract;

public abstract class RectangleEntity : Entity
{
    public override bool IsColliding(Entity other) {
        if (other is RectangleEntity) {
            RectangleEntity otherRectangle = (RectangleEntity)other;
            return  Bounds.Intersects(otherRectangle.Bounds);
        }
        return false;
    }
    public Rectangle Bounds {
        get {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }
        set {
            Position = new Vector2(value.X, value.Y);
            Width = value.Width;
            Height = value.Height;
        }
    }
    public int Width;
    public int Height;
}