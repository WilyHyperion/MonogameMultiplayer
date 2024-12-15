



using System;
using System.Numerics;
using Game.Abstract.Entities;
using Game.Entities.Projectiles;
using Game.Helpers;

namespace Game.Entities.Units;

public class BasicShooter : Unit
{
    public override void SetDefaults()
    {
        this.strength = 50;
    }
    public int counter;
    public override void AI()
    {
        counter++;
        Unit e = this.getNearestEnemy();
        if(e != null){
            this.Velocity =  e.Position - this.Position;
            float dist = this.Velocity.LengthSquared();
            this.Velocity.Normalize();
            float factor = 2f;
            if(dist < 50000){
                factor *= -1;
            }
            this.Velocity *= factor;
            if(counter % 15 == 0){
                Projectile.NewProjectile<Bullet>(this.Bounds.Middle, this.Velocity * factor, this.team);
            }
        }
    }
}