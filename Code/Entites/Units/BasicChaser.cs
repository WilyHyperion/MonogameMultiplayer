



using System;
using System.Numerics;
using Game.Abstract.Entites;
using Game.Helpers;

namespace Game.Entites.Units;

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
        if(e != null){
            this.Velocity = this.Position - e.Position;
            this.Velocity.Normalize();
            this.Velocity *= 5f;
        }
    }
}