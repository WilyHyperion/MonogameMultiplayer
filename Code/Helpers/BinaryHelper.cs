
using System.Diagnostics;
using System.IO;
using Game.Player;

namespace Game.Helpers;


public static class BinaryHelpers {
    public static void Write(this BinaryWriter bw, RectangleF rect ){
        bw.Write(rect.X);
        bw.Write(rect.Y);
        bw.Write(rect.Width);
        bw.Write(rect.Height);
    }
    public static RectangleF ReadRectangleF(this BinaryReader b){
        return new RectangleF((float)b.ReadSingle(), b.ReadSingle(), b.ReadSingle(), b.ReadSingle());
    }
    public static void Write(this BinaryWriter bw, PlayerEntity p){
        bw.Write(p.name);
        bw.Write(p.Bounds);
        bw.Write(p.maxHP);
    }
    public static PlayerEntity ReadPlayer(this BinaryReader b) {
        string name = b.ReadString();
        RectangleF bounds = b.ReadRectangleF();
        int maxHP = b.ReadInt32();
        PlayerEntity p = new PlayerEntity(name, bounds, maxHP);
        p.remote = true;
        return p;
    }
}