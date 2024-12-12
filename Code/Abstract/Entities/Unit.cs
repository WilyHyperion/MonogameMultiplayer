

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Game.Abstract.Entites;
using Game.Helpers;
using Game.System.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Game.Abstract.Entites;
public class StaticUnitInformation {
    public static ContentManager manager;
    public Texture2D texture;
    public int ID;
    public Type type;
    public StaticUnitInformation(Type t,  int id){
        this.ID = id;
        string path = t.Namespace.Remove(0, 5) + t.Name + "";
        Console.WriteLine("t.name" + path);
        try
        {
            texture = manager.Load<Texture2D>(path.Replace(".", "/"));
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to find texture");
            this.texture = manager.Load<Texture2D>("unit");
            Console.WriteLine(texture);
        }
    }
}
public abstract class Unit : SoildEntity
{
    public static Unit SpawnUnit<T>(Vector2 position, Team team) where T : Unit{
        Unit u = (Unit)Activator.CreateInstance(typeof(T));
        u.SetDefaults();
        u.Bounds.X = position.X;
        u.Bounds.Y = position.Y;
        UnamedGame.Instance.SpawnEntity(u);
        u.team = team;
        team.Units.Add(u);
        return u;
    }
    public static Dictionary<Type, StaticUnitInformation> unitTypes = new Dictionary<Type, StaticUnitInformation>();
    public static void InitializeUnits(ContentManager content){
        StaticUnitInformation.manager = content;
        int current = 0;
        var types = Game.UnamedGame.Instance.GetType().Assembly.GetTypes();
        foreach(Type t in types) {
            if(!t.IsAbstract && t.IsSubclassOf(typeof(Unit))){
                unitTypes[t] = new StaticUnitInformation(t, current);
                current++;
            }
        }
    }

    /// <summary>
    /// Returns the closest unit with a team not equal to the units own.
    /// May also return null if none are found within the range
    /// </summary>
    /// <param name="maxRange"></param>
    /// <returns></returns>
    public Unit getNearestEnemy(float maxRange = 9000)
    {
        var nearby = this.gridSystem.GetNodesWithin(Bounds.Middle, maxRange);
        float nearist = maxRange;
        Unit res = null;
        foreach (var p in nearby)
        {
            if (p.collidable is Unit u)
            {
                if (u.team == this.team)
                {
                    continue;
                }
                else
                {
                    float dist = (this.Bounds.Middle - u.Bounds.Middle).LengthSquared();
                    if (nearist > dist)
                    {
                        nearist = dist;
                        res = u;
                    }
                }
            }
            else {
                Logger.Log($"Not a unit  {p.collidable}  {p.collidable.GetType()}   {nearby.Count}  {this.whoAmi}");
            }

        }
        return res;
    }
    public Unit getNearestAlly(float maxRange = 9000)
    {
        var nearby = this.gridSystem.GetNodesWithin(Bounds.Middle, maxRange);
        float nearist = maxRange;
        Unit res = null;
        foreach (var p in nearby)
        {
            if (p.collidable is Unit u)
            {
                if (u.team != this.team)
                {
                    continue;
                }
                else
                {
                    Console.WriteLine("Found ally");
                    float dist = (this.Bounds.Middle - u.Bounds.Middle).LengthSquared();
                    if (nearist > dist)
                    {
                        nearist = dist;
                        res = u;
                    }
                }
            }

        }
        return res;
    }
    public int HP = 100;
    public Texture2D Texture {
        get {
            return unitTypes[GetType()].texture;
        }
        set {
            unitTypes[GetType()].texture = value;
        }
    }
    public virtual void SetDefaults(){
    }
    public virtual void AI(){

    }
    public override void Update()
    {
        this.AI();
    }
    public Team team;
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(this.Texture, this.Position, Color.White);
    }
}