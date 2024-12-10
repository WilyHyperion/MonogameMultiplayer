namespace Game.Abstract.Entites;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collision;
using Abstract;
using System;
using global::System;
using Entites;
using global::Game.Entites;
using Game.Helpers;

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
                //if the old bounds also overlaps, we cant find out where it came from.
                //instead, move it to the closest side
                float xdif = Math.Abs(Math.Max(OldBounds.Left, other.Bounds.Left) - Math.Min(OldBounds.Right, other.Bounds.Right));
                float ydif = Math.Abs(Math.Max(OldBounds.Top, other.Bounds.Top) - Math.Min(OldBounds.Bottom, other.Bounds.Bottom));
                if (xdif < ydif)
                {
                    float rightdist = OldBounds.Left - other.Bounds.Left;
                    if(rightdist > other.Bounds.Right - OldBounds.Right){
                        this.Bounds.Left = other.Bounds.Right;
                    }
                    else {
                        this.Bounds.Right  = other.Bounds.Left;
                    }
                }
                else
                {
                    float topdist = OldBounds.Top - other.Bounds.Top;
                    if(topdist > other.Bounds.Bottom - OldBounds.Bottom){
                        this.Bounds.Top = other.Bounds.Bottom;
                    }
                    else {
                        this.Bounds.Bottom = other.Bounds.Top;
                    }
                }
            }
            else{
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