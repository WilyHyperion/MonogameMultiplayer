
using System;
using System.Collections.Generic;
using System.Threading;
using Game.Abstract.Entities;
using Game.Helpers;
using Game.System.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Game.Abstract.Entities;

public abstract class Projectile : Collidable
{
    public bool RotateToVelocity = true;
    public static Projectile NewProjectile<T>(Vector2 pos, Vector2 Vel, Team t) where T : Projectile
    {
        Projectile spawner = (Projectile)Activator.CreateInstance(typeof(T));
        spawner.SetDefaults();
        spawner.Bounds.BottomRight = pos;
        spawner.Velocity = Vel;
        spawner.team = t;
        Game.UnamedGame.Instance.SpawnEntity(spawner);
        return spawner;
    }

    public virtual void SetDefaults()
    {
    }

    public Team team = Team.Any;
    public static Projectile NewProjectile<T>(Vector2 pos, Vector2 vel) where T : Projectile
    {
        return NewProjectile<T>(pos, Vector2.Zero, Team.Any);
    }
    public static Projectile NewProjectile<T>(Vector2 pos) where T : Projectile
    {
        return NewProjectile<T>(pos, Vector2.Zero);
    }

    public static Dictionary<Type, StaticInformation> unitTypes = new Dictionary<Type, StaticInformation>();

    public Texture2D Texture
    {
        get
        {
            return unitTypes[this.GetType()].texture;
        }
    }
    public int Damage = 1;
    public override void OnCollision(Collidable other)
    {
        if (other is Unit u)
        {
            if (u.team != this.team || this.team == Team.Any)
            {
                if (u.OnHit(Damage, this))
                {
                    u.HP -= this.Damage;

                }
            }
        }
    }

    public static void InitializeUnits(ContentManager content)
    {
        StaticInformation.manager = content;
        int current = 0;
        var types = Game.UnamedGame.Instance.GetType().Assembly.GetTypes();
        foreach (Type t in types)
        {
            if (!t.IsAbstract && t.IsSubclassOf(typeof(Projectile)))
            {
                unitTypes[t] = new StaticInformation(t, current);
                current++;
            }
        }
    }
    public float RotationOffset = 0f;
    public override void Draw(SpriteBatch spriteBatch)
    {
        if(RotateToVelocity){
            spriteBatch.Draw(this.Texture, this.Bounds, null, this.team.TeamColor, this.Velocity.ToRotation() + this.RotationOffset, new Vector2(Bounds.Width / 2, Bounds.Height / 2) , SpriteEffects.None, 0f );
        }
        else{
        spriteBatch.Draw(this.Texture, this.Position, this.team.TeamColor);
        }
    }
}