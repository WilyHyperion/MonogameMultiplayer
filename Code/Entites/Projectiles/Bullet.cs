using Game.Abstract.Entities;

namespace Game.Entities.Projectiles;


public class Bullet : Projectile {
    public override void SetDefaults()
    {
        this.Bounds.Width = 5;
        this.Bounds.Height = 5;
        this.Damage = 3;
    }
}