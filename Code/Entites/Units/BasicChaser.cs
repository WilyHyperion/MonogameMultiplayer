



using System;
using System.Numerics;
using Game.Abstract.Entities;
using Game.Helpers;

namespace Game.Entities.Units;

public class BasicChaser : Unit
{
    public override void SetDefaults()
    {
        this.strength = 50;
    }
    public int counter;
    public override void AI()
    {
        Unit e = this.getNearestEnemy();
        if (e != null)
        {
            if ((this.Position - e.Position).LengthSquared() < 50)
            {
                this.Velocity = Vector2.Zero;
            }
            else
            {
                this.Velocity = e.Position - this.Position;
                this.Velocity.Normalize();
                this.Velocity *= 5f;
            }
        }
    }
}