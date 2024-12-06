namespace UnamedGame.Abstract.Entites;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collision;
using Abstract;
using System;
using global::System;
using Entites;
using global::UnamedGame.Entites;

public abstract class SoildEntity : Collidable
{
    public int strength = 0;

    public override void OnCollision(Collidable other)
    {
        if (other.node.ID == this.node.ID){
            return;
        }
        if (ShouldCollide(other) && (other is SoildEntity || other == null))
        {
            if(this is StageGeometry) {
                return;
            }
            if(other != null && other is SoildEntity){
                SoildEntity otherSoild = (SoildEntity)other;
                if (otherSoild.strength < this.strength && !(other is StageGeometry))
                {
                    return;
                }
            }
            //3 cases -either its x should be snapped or the y should be snapped, or it looks like both, so we need to do some math to find out where it would have colided along the step
            //if its the x, then the y value of the last bounds should be inside the rectangle
            //and vice versa
            if(OldBounds.Right < other.Bounds.Left){
                //right
                Bounds.X = other.Bounds.Left - Bounds.Width;
            }
            else if(OldBounds.Left > other.Bounds.Right){
                //left
                Bounds.X = other.Bounds.Right;
            }
            else if(OldBounds.Bottom < other.Bounds.Top){
                //bottom
                Bounds.Y = other.Bounds.Top - Bounds.Height;
            }
            else if(OldBounds.Top > other.Bounds.Bottom){
                //top
                Bounds.Y = other.Bounds.Bottom;
            }
            else if (OldBounds.Intersects(other.Bounds)) {
            }
            else{
                Console.WriteLine("No collision");
            }

            


        }
    }
    /// <summary>
    /// Should also be used to run on collide code
    /// </summary>
    /// <param name="other">object touching</param>
    /// <returns>if the normal soild collider code should run. Will still only run on other soild objects </returns>
    public virtual bool ShouldCollide(Collidable other)
    {
        return true;
    }
}