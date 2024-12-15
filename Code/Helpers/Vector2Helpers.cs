using System;
using Microsoft.Xna.Framework;

namespace Game.Helpers;

public static class Vector2Helpers {
    public static float ToRotation(this Vector2 v){
        return (float)Math.Atan2(v.Y + 0.0f, v.X + 0.0f);
    }
}