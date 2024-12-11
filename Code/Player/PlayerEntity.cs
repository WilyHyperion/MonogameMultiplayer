using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game.Abstract;
using Game;
using Game.Helpers;
using Game.GameSystem.UI;
using Game.UI;
using Game.Abstract.Entites;
using Game.System.Collision;
namespace Game.Player;

public class PlayerEntity : SoildEntity

{
    public bool remote = false;
    public string name = "";
    public int HP = 100;
    public int maxHP = 100;
    public static Texture2D Texture;
    public static Texture2D guntexture;
    public PlayerEntity(Vector2 position)
    {
        var i = UnamedGame.Instance;
        gridSystem = i.collisionManager.gridSystem;

        Texture = i.Content.Load<Texture2D>("unit");
        guntexture = i.Content.Load<Texture2D>("gun");
        Bounds = new RectangleF(position.X, position.Y, Texture.Width, Texture.Height);
        UIManager.AddLow(new HPBar());
    }
    public PlayerEntity(Vector2 position, GridSystem s)
    {
        var i = UnamedGame.Instance;
        gridSystem = s;
        Texture = i.Content.Load<Texture2D>("unit");
        guntexture = i.Content.Load<Texture2D>("gun");
        Bounds = new RectangleF(position.X, position.Y, Texture.Width, Texture.Height);
        UIManager.AddLow(new HPBar());

    }
    public float Speed = 3f;
    PlayerInput input;
    public override void Update()
    {
        if (true)//(!remote)
        {
            input = new PlayerInput();

            Vector2 addition = Vector2.Zero;
            if (input.IsKeyDown(Keys.W))
            {
                addition.Y += -1f;
            }
            else if (input.IsKeyDown(Keys.S))
            {
                addition.Y += 1f;
            }
            if (input.IsKeyDown(Keys.A))
            {
                addition.X += -1f;
            }
            else if (input.IsKeyDown(Keys.D))
            {
                addition.X += 1f;
            }
            if (addition.Length() != 0)
            {
                addition.Normalize();
                addition *= Speed;
            }
            Velocity += addition;
            if (input.MouseState.LeftButton == ButtonState.Pressed && animationTick == -1)
            {
                animationTick = 0;
            }
            else if (animationTick >= guntexture.Height / 8)
            {
                animationTick = -1;
            }
            else if (animationTick > -1)
            {
                animationTick++;
            }
        }
    }
    public float Friction = 0.1f;
    public override void PostUpdate()
    {
        this.Velocity *= this.Friction;
    }
    int animationTick = -1;
    public override void Draw(SpriteBatch spriteBatch)
    {
        Helpers.DrawHelpers.DrawRectangle(spriteBatch, OldBounds, Color.White);
        var mousepos = UnamedGame.Instance.camera.ScreenToWorld(new Vector2(input.MouseState.X, input.MouseState.Y));
        var direction = mousepos - this.Position;
        var rotation = (float)Math.Atan2(direction.Y + 0.0f, direction.X + 0.0f) + MathHelper.ToRadians(90f);
        spriteBatch.Draw(Texture, Position + new Vector2(Bounds.Width / 2, Bounds.Height / 2), null, Color.White, rotation, new Vector2(Bounds.Width / 2, Bounds.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
        Vector2 gunOffset = new Vector2(Texture.Width, Texture.Height) * 0.5f;
        gunOffset.Rotate(rotation - MathHelper.ToRadians(90f));
        spriteBatch.Draw(guntexture, Position + gunOffset + new Vector2(Bounds.Width / 2, Bounds.Height / 2), DrawHelpers.sampleAnimationFrame(guntexture, 8, 1, animationTick), Color.White, rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
    }
    public override bool ShouldCollide(Collidable other)
    {
        Logger.Log($"  ${OldBounds}   and ${Bounds} equal {OldBounds == Bounds}");
        return true;
    }

}