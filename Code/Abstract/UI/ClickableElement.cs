
namespace UnamedGame.Abstract.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public  interface ClickableElement
{
    public Rectangle Bounds { get; set; }
    public bool Clicked { get; set; }
    public bool CheckClicked(){
        return CheckClicked(new Vector2(Mouse.GetState().X, Mouse.GetState().Y ) );
    }
    public bool CheckClicked(Vector2 mousePos){
        return Bounds.Contains(mousePos) && Mouse.GetState().LeftButton == ButtonState.Pressed;
    }

}