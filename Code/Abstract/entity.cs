

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Abstract
{
    public abstract class Entity
    {
        public int ID;
        public Vector2 Velocity;
        public bool Active = true;
        public int whoAmi;
        public virtual void Update() {
            
        }
        public abstract void Draw(SpriteBatch spriteBatch);
        public void Destroy()
        {
            Active = false;
        }
        public virtual void PostUpdate(){

        }
    }

    }