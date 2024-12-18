
using System.IO;

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
}