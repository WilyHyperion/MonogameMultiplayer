

namespace Game.UI;

using Microsoft.Xna.Framework;
using Abstract.UI;
using Microsoft.Xna.Framework.Graphics;
using Helpers;

public class HPBar : UIElement
{
    public override void Draw(SpriteBatch s)
    {
        s.Draw(texture, Position, Color.White);
        DrawHelpers.DrawRectangle(s, new Rectangle((int)Position.X + 1, (int)Position.Y + 1, (int)(texture.Width  *  ((float)UnamedGame.Instance.player.HP / UnamedGame.Instance.player.maxHP) ), (int)(texture.Height * 0.9)), Color.Black);
    }
}