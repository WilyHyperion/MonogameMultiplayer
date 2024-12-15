



using System;
using System.Collections.Generic;
using System.Numerics;
using Game.Abstract.Entities;
using Game.Entities.Projectiles;
using Game.Helpers;
using Microsoft.Xna.Framework;

namespace Game.Entities.Units;

public class BasicShooter : Unit
{
    public Unit Target;
    public override Dictionary<string, object> GetDebugInformation()
    {
        if(this.Target == null){
            return new Dictionary<string, object>();
        }
        return new Dictionary<string, object>(){
            {"target", this.Target.whoAmi}
        };
    }
    public override void SetDefaults()
    {
        this.strength = 50;
    }
    public int counter;
    public override void AI()
    {
        counter++;
        Unit e = this.getNearestEnemy();
        this.Target = e;
        if(e != null){
            this.Velocity =  e.Position - this.Position;
            float dist = this.Velocity.LengthSquared();
            this.Velocity.Normalize();
            float factor = 2f;
            if(dist < 50000){
                factor *= -1;
            }
            else {
                this.Velocity.Rotate(MathHelper.ToDegrees(50f));
            }
            this.Velocity *= factor;
            if(counter % 15 == 0){
                Projectile.NewProjectile<Bullet>(this.Bounds.Middle, this.Velocity * factor, this.team);
            }
        }
    }
    public override bool OnHit(int damage, Projectile projectile)
    {
        return base.OnHit(damage, projectile);
    }
}