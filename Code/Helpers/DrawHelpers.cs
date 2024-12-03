
namespace UnamedGame.Helpers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
public static class DrawHelpers {
    static Texture2D pixel = new Texture2D(UnamedGame.Instance.GraphicsDevice, 1, 1);
        
    public static void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, Color color) {
        pixel.SetData(new[] { Color.White });
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
    }
    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color) {
        pixel.SetData(new[] { Color.White });
        spriteBatch.Draw(pixel, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height), color);
    }
}