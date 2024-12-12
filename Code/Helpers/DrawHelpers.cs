
namespace Game.Helpers;

using System;
using global::System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public static class DrawHelpers
{
   
    static Texture2D pixel = new Texture2D(UnamedGame.Instance.GraphicsDevice, 1, 1);

    public static void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        pixel.SetData(new[] { Color.White });
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
    }
    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        pixel.SetData(new[] { Color.White });
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height), color);
    }
    public static Rectangle sampleAnimationFrame(Texture2D texture, int numAnimationFrames, int frameDuration, int animationTick, bool repeat = false)
    {
        if (animationTick == -1)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height / numAnimationFrames);
        }
        else
        {
            int frame = (int)(animationTick / frameDuration);
            if (animationTick > frameDuration * numAnimationFrames)
            {
                if (repeat)
                {
                    animationTick %=  frameDuration * numAnimationFrames;
                }
                else
                {
                    return new Rectangle(0, 0, texture.Width, texture.Height / numAnimationFrames);
                }
            }
            return new Rectangle(0, frame * (texture.Height / numAnimationFrames), texture.Width, texture.Height / numAnimationFrames);

        }
    }
}