

using System;
using System.IO;
using System.Net;
using Game;
using Game.Helpers;
using Game.Player;
using Microsoft.Xna.Framework;

namespace Server.Packets;

public class Connect : ClientOrigniatingPacket
{
    public Connect()
    {
    }

    public Connect(byte[] data) : base(data)
    {
    }

    public Connect(byte[] data, IPEndPoint from) : base(data, from)
    {
    }

    public override byte[] Send()
    {
        Console.WriteLine("ran");
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter b = new BinaryWriter(ms))
            {
                b.Write(UnamedGame.Instance.player.Bounds);
                b.Write(Console.ReadLine());
            }
            return ms.ToArray();
        }
    }
    public override void ServerReceive(ServerPlayer sender)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            ms.Write(data);
            ms.Position = 0;
            BinaryReader b = new BinaryReader(ms);
            RectangleF bounds = b.ReadRectangleF();
            PlayerEntity p = new PlayerEntity();
            p.Bounds = bounds;
            p.name = b.ReadString();
            sender.player = p;

            b.Dispose();
        }
    }
}
