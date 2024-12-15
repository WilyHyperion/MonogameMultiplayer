

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.Abstract;
using Game.Abstract.Entities;
using Game.GameSystem;
using Game.Helpers;
using Game.System.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.System;
public static class Debug
{
    public static bool DebugMode = false;
    public static void ToggleDebug()
    {
        DebugMode = !DebugMode;
    }
    const int gridamount = 15;
    public static void DrawDebugGrid(SpriteBatch s, GridSystem gridSystem){
        Vector2 position = UnamedGame.Instance.camera.Position;
        position.X = position.X - position.X % GridSystem.GridSize;
        position.Y = position.Y - position.Y % GridSystem.GridSize;
        for (int i = -gridamount; i < gridamount; i++)
        {
            for (int j = -gridamount    ; j < gridamount; j++)
            {
                RectangleF r = new RectangleF(i * GridSystem.GridSize + position.X, j * GridSystem.GridSize + position.Y, GridSystem.GridSize, GridSystem.GridSize);
                DrawHelpers.DrawRectangleOutline(s, r, Color.Red);
            }
        }
    }
    public static void DrawDebugInformation(Entity e, SpriteBatch s)
    {
        SpriteFont f = Game.UnamedGame.font;
        Dictionary<String, Object> draws = new Dictionary<String, object>()
        {
            {"Active", e.Active.ToString()},
            {"ID", e.ID}
        };
        e.GetDebugInformation().ToList().ForEach(x => draws.Add(x.Key, x.Value));

        if (e is Collidable c)
        {
            if(e is Projectile ){
                return;
            }
            draws.Add("Bounds", c.Bounds);
            draws.Add("Velocity", e.Velocity);
            draws.Add("WhoAmI", e.whoAmi);
            draws.Add("node", c.node);
            string output = "";
            foreach (var key in draws)
            {
                 output += $"{key.Key}:{key.Value.ToString()}\n";
            }
                s.DrawString(f,output , new Vector2(c.Bounds.Left, c.Bounds.Top + draws.Count* -20) , Color.Beige );
        }

    }
}