using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.Xna.Framework;
namespace Game.Helpers;

public static class RandomHelper
{
    public static Color GetRandomColor(this Random r){
        return new Color(r.Next(0, 255), r.Next(0,255), r.Next(0,255));
    }
    public static Vector2 NextVector2(this Random r, int min, int max) {
        return new Vector2(r.Next(min, max), r.Next(min, max));
    }
}