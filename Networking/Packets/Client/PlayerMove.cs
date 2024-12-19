

using System.IO;
using System.Net;
using Game;
using Game.Helpers;
using Microsoft.Xna.Framework;
using Server;

public class PlayerMove : ClientOrigniatingPacket
{
    public PlayerMove()
    {
    }

    public PlayerMove(byte[] data) : base(data)
    {
    }

    public PlayerMove(byte[] data, IPEndPoint from) : base(data, from)
    {
    }

    public override byte[] Send()
    {
        using (var ms = new MemoryStream())
        {
            using (var writer = new BinaryWriter(ms))
            {
                writer.Write(Game.UnamedGame.Instance.player.Velocity);
                writer.Write(UnamedGame.Instance.player.Position);
            }
            return ms.ToArray();
        }
    }
    public override void ServerReceive(ServerPlayer sender)
    {
         using (MemoryStream ms = new MemoryStream(this.data))
        {
            using (BinaryReader b = new BinaryReader(ms))
            {
                Vector2 vel = b.ReadVector2();
                Vector2 pos = b.ReadVector2();
                sender.player.Position = pos;
                sender.player.Velocity = vel;
                
            }
        }
    }
}