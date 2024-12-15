using Game.Abstract.Entities;
using Microsoft.Xna.Framework;

namespace Game.Entities.Projectiles;


public class Bullet : Projectile {
    public override void SetDefaults()
    {
        this.Bounds.Width = 5;
        this.Bounds.Height = 5;
        this.Damage = 3;
        this.RotationOffset = MathHelper.ToRadians(90);
    }
}