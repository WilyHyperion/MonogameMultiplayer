

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Game;
using Game.Helpers;
using Game.Player;
using Microsoft.Xna.Framework;

namespace Server.Packets;
public class ConnectRecive : ServerOriginatingPacket
{
    public ConnectRecive()
    {
    }

    public ConnectRecive(byte[] data) : base(data)
    {
    }

    public ConnectRecive(byte[] data, IPEndPoint from) : base(data, from)
    {
    }

    public override void ClientReceive()
    {
        Console.WriteLine("Received Connect reponse");
        UnamedGame.Instance.MyID = data[0];
        UnamedGame.Instance.ConnectedPlayers.Clear();
        UnamedGame.Instance.ConnectedPlayers[data[0]] = new GamePlayer(data[0], UnamedGame.Instance.player, true);
    }
    public byte ID;
    public override byte[] Send()
    {
        Console.WriteLine("Sending connect packet");
        return [ID];
    }
}

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
        using (MemoryStream ms = new())
        {
            using (BinaryWriter b = new(ms))
            {
                b.Write(UnamedGame.Instance.player.Bounds);
                b.Write("USer" + UnamedGame.random);
            }
            return ms.ToArray();
        }
    }
    public override void ServerReceive(ServerPlayer sender)
    {
        using (MemoryStream ms = new())
        {
            Console.WriteLine("Server received connect packet");
            ms.Write(data);
            ms.Position = 0;
            BinaryReader b = new(ms);
            RectangleF bounds = b.ReadRectangleF();
            PlayerEntity p = new()
            {
                collisionManager = Server.Instance.collisionManager,
                Bounds = bounds,
                name = b.ReadString()
            };
            sender.player = p;
            p.ID = Server.Instance.connected.Count -1;
            b.Dispose();
        }
    }
}
