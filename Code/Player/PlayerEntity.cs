using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UnamedGame.Abstract;
using UnamedGame;
using UnamedGame.Helpers;
using UnamedGame.GameSystem.UI;
using UnamedGame.UI;
namespace UnamedGame.Player;

public class PlayerEntity : RectangleEntity
{
    public int HP = 100;
    public int maxHP = 100;
    public Texture2D Texture;
    public Texture2D guntexture;
    public PlayerEntity( Vector2 position)
    {
        var i = UnamedGame.Instance;
        Texture = i.Content.Load<Texture2D>("unit");
        guntexture = i.Content.Load<Texture2D>("gun");
        Position = position;
        Width = Texture.Width;
        Height = Texture.Height;
        UIManager.AddElement(new HPBar());
    }
    public float Speed = 1;
    public float Friction = 0.9f;
    PlayerInput input;
    public override void Update()
    {
        input = new PlayerInput();
        Position += Velocity;
        if (input.IsKeyDown(Keys.W))
        {
            Velocity.Y += -1;
        }
        else if (input.IsKeyDown(Keys.S))
        {
            Velocity.Y += 1;
        }
        if (input.IsKeyDown(Keys.A))
        {
            Velocity.X += -1;
        }
        else if (input.IsKeyDown(Keys.D))
        {
            Velocity.X += 1;
        }
        if (Velocity.Length() > Speed)
        {
            Velocity.Normalize();
            Velocity *= Speed;
        }
        if(input.IsKeyDown(Keys.Enter)){
            HP -= 1;
        }
        if(input.MouseState.LeftButton == ButtonState.Pressed && animationTick == -1){
            animationTick = 0;
        }
        else if (animationTick >= guntexture.Height / 8) {
            animationTick = -1;
        }
        else if (animationTick > -1){
            animationTick++;
        }
        Velocity *= Friction;
        if (Velocity.Length() < 0.1)
        {
            Velocity = Vector2.Zero;
        }
    }
    int animationTick = -1;

    public override void Draw(SpriteBatch spriteBatch)
    {

        var mousepos = UnamedGame.Instance.camera.ScreenToWorld(new Vector2(input.MouseState.X, input.MouseState.Y));
        var direction = mousepos - Position;
        var rotation = (float)Math.Atan2(direction.Y + 0.0f, direction.X + 0.0f) + MathHelper.ToRadians(90f);
        spriteBatch.Draw(Texture, Position, null, Color.White, rotation, new Vector2(Texture.Width/2, Texture.Height/2), 1.0f, SpriteEffects.None, 0.0f);
        Vector2 gunOffset = new Vector2(Texture.Width, Texture.Height) * 0.5f;
        gunOffset.Rotate(rotation - MathHelper.ToRadians(90f));
        spriteBatch.Draw(guntexture, Position + gunOffset , DrawHelpers.sampleAnimationFrame( guntexture, 8, 3, animationTick) , Color.White, rotation, new Vector2(Texture.Width/2, Texture.Height/2), 1.0f, SpriteEffects.None, 0.0f);
    }

}