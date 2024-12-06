using Microsoft.Xna.Framework;

namespace Game.Helpers;

public class RectangleF {
    float x;
    float y;
    public float X{
        get{
            return x;
        }
        set{
            x = value;
        }
    }
    public float Y{
        get{
            return y;
        }
        set{
            y = value;
        }
    }
    public float Width;
    public float Height;
    public Vector2 Position{
        get{
            return new Vector2(x, y);
        }
        set{
            x = value.X;
            y = value.Y;
        }
    }
    public Vector2 Size{
        get{
            return new Vector2(Width, Height);
        }
        set{
            Width = value.X;
            Height = value.Y;
        }
    }
    public float Left{
        get{
            return x;
        }
        set{
            x = value;
        }
    }
    public float Right{
        get{
            return x + Width;
        }
        set{
            X = value - Width;
        }
    }
    public float Top{
        get{
            return y;
        }
        set{
            y = value;
        }
    }
    public float Bottom{
        get{
            return y + Height;
        }
        set{
            y = value - Height;
        }
    }
    public Vector2 TopLeft{
        get{
            return new Vector2(Left, Top);
        }
    }
    public RectangleF(){
        x = 0;
        y = 0;
        Width = 0;
        Height = 0;
    }
    public RectangleF(Vector2 position, Vector2 size){
        x = position.X;
        y = position.Y;
        Width = size.X;
        Height = size.Y;
    }
    public RectangleF(RectangleF r){
        x = r.x;
        y = r.y;
        Width = r.Width;
        Height = r.Height;
    }
    public RectangleF(float x, float y, float width, float height){
        this.x = x;
        this.y = y;
        this.Width = width;
        this.Height = height;
    }
    public bool Contains(Vector2 point){
        return point.X >= x && point.X <= x + Width && point.Y >= y && point.Y <= y + Height;
    }
    public bool Contains(float x, float y){
        return x >= this.x && x <= this.x + Width && y >= this.y && y <= this.y + Height;
    }
    public bool Intersects(RectangleF other){
        return x < other.x + other.Width && x + Width > other.x && y < other.y + other.Height && y + Height > other.y;
    }
    public bool Intersects(float x, float y, float width, float height){
        return this.x < x + width && this.x + this.Width > x && this.y < y + height && this.y + this.Height > y;
    }





    public static RectangleF operator +(RectangleF r, Vector2 v){
        return new RectangleF(r.x + v.X, r.y + v.Y, r.Width, r.Height);
    }
    public static RectangleF operator -(RectangleF r, Vector2 v){
        return new RectangleF(r.x - v.X, r.y - v.Y, r.Width, r.Height);
    }
    public static implicit operator RectangleF(Rectangle r){
        return new RectangleF(r.X, r.Y, r.Width, r.Height);
    }
    public static implicit operator Rectangle(RectangleF r){
        return new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height);
    }
    public override string ToString()
    {
        return $"X: {x}, Y: {y}, Width: {Width}, Height: {Height}";
    }
    public override bool Equals(object obj)
    {
        if(obj is RectangleF ){
            RectangleF r = (RectangleF)obj;
            return r.x == x && r.y == y && r.Width == Width && r.Height == Height;
        }
        return false;
    }
}