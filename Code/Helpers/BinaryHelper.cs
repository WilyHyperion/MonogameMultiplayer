
using System;
using System.Diagnostics;
using System.IO;
using Game.Player;
using Microsoft.Xna.Framework;

namespace Game.Helpers;


public static class BinaryHelpers {
    public static void Write(this BinaryWriter bw, Vector2 obj){
        bw.Write(obj.X);
        bw.Write(obj.Y);
    }
    public static Vector2 ReadVector2(this BinaryReader br){
        float x = br.ReadSingle();
        float y = br.ReadSingle();
        return new Vector2(x, y);
    }
    public static void Write(this BinaryWriter bw, RectangleF rect ){
        bw.Write(rect.X);
        bw.Write(rect.Y);
        bw.Write(rect.Width);
        bw.Write(rect.Height);
    }
    public static RectangleF ReadRectangleF(this BinaryReader b){
        var r = new RectangleF(b.ReadSingle(), b.ReadSingle(), b.ReadSingle(), b.ReadSingle());
        return r;
    }
    public static void Write(this BinaryWriter bw, PlayerEntity p){
        bw.Write(p.name);
        bw.Write(p.Bounds);
        bw.Write(p.maxHP);
        bw.Write(p.ID);
        BinaryHelpers.Write(bw, p.Velocity);
    }
    public static PlayerEntity ReadPlayer(this BinaryReader b) {
        string name = b.ReadString();
        RectangleF bounds = b.ReadRectangleF();
        int maxHP = b.ReadInt32();
        PlayerEntity p = new PlayerEntity(name, bounds, maxHP);
        p.ID = b.ReadInt32();
        return p;
    }
}