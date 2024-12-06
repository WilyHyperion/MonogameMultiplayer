


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game.Abstract.UI;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;
namespace Game.Abstract.UI;
public class Preformance : UIText
{
    public override void Update()
    {
        Text = "UpdateTime: " + Game.UnamedGame.Instance.updateTime + "\nDrawTime: " + Game.UnamedGame.Instance.drawTime + "\nNumEntites: " + Game.UnamedGame.Instance.entities.Count;
        if(Game.UnamedGame.Instance.updateTime > 1/60f)
        {
            color = Color.Red;
        }
        else
        {
            color = Color.White;
        }
        if(Keyboard.GetState().IsKeyDown(Keys.F1) && UnamedGame.Instance.oldState.IsKeyUp(Keys.F1)) 
        {
            this.Visible = !this.Visible;
        }
    }

    public Preformance( SpriteFont font) : base("", new Vector2(0, 0))
    {
        this.Font = font;
    }
}