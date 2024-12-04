
namespace UnamedGame.Abstract.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class UIElement {
    public Vector2 Position;
    public abstract void Draw(SpriteBatch s);
    public virtual void Update(){

    }
    public void UpdateElement(){
        if(this is ClickableElement s){
            s.Clicked = s.CheckClicked();
        }
        Update();
    }
    public static Texture2D texture;
    public static void LoadTexture(Texture2D t){
        texture = t;
    }
}