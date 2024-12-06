


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Abstract.UI;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
namespace Game.Abstract.UI;
public class UILogText : UIText
{
    public override void Update()
    {
        if(Keyboard.GetState().IsKeyDown(Keys.F2) && UnamedGame.Instance.oldState.IsKeyUp(Keys.F2)) 
        {
            this.Visible = !this.Visible;
        }
    }

    public UILogText( SpriteFont font) : base("", new Vector2(0, 0))
    {
        this.Font = font;
    }
}