

using Microsoft.Xna.Framework;

namespace UnamedGame.System.Collision;
public class CollisionManager(){
    GridSystem gridSystem = new GridSystem();
}

interface ICollidable{
    Rectangle bounds {get;}
    void OnCollision(ICollidable other);

}