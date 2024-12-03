

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UnamedGame.Abstract
{
public abstract class Entity {
    public int ID;
    public Vector2 Position;
    public Vector2 Velocity;
    public bool Active = true;
    public int whoAmi;

    public abstract Boolean IsColliding(Entity other);
    public abstract void Update();
    public abstract void Draw ( SpriteBatch spriteBatch );
}

}