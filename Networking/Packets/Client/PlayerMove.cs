

using System;
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
        using var ms = new MemoryStream();
        using (var writer = new BinaryWriter(ms))
        {
            Console.WriteLine(UnamedGame.Instance.player.Bounds);
            writer.Write(Game.UnamedGame.Instance.player.Velocity);
            writer.Write(UnamedGame.Instance.player.Bounds);
        }
        return ms.ToArray();
    }
    public override void ServerReceive(ServerPlayer sender)
    {
        using MemoryStream ms = new(this.data);
        using BinaryReader b = new(ms);
        Vector2 vel = b.ReadVector2();
        RectangleF bounds = b.ReadRectangleF();
        sender.player.Bounds = bounds;
        sender.player.Velocity = vel;
        Server.Server.TriggerMovementSync(sender.id, sender.player.Bounds, sender.player.Velocity);
    }
}