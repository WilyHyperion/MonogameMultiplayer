

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UnamedGame.Abstract.UI;
namespace UnamedGame.Abstract.UI;
public class UIText : UIElement
{
    public override void Draw(SpriteBatch s)
    {
    }
    public string Text;
    public UIText(string text, Vector2 position, Vector2 size)
    {
        Text = text;
    }
    
}