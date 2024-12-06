

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Abstract.UI;
namespace Game.Abstract.UI;
public class UIText : UIElement
{

    public Color color = Color.White;
    public Vector2 Size;
    public override void Draw(SpriteBatch s)
    {
        s.DrawString(Font, Text, Position, this.color);
    }
    public string Text;
    public SpriteFont Font;

    public UIText(string text, Vector2 position, Vector2 size, SpriteFont font)
    {
        Text = text;
        Position = position;
        Size = size;
        Font = font;
    }
    public UIText(string text, Vector2 position, Vector2 size)
    {
        Text = text;
        Position = position;
        Size = size;
    }
    public UIText(string text, Vector2 position)
    {
        Text = text;
        Position = position;
    }

}