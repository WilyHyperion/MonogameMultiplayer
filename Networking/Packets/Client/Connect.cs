

using System;
using System.IO;
using System.Net;
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
        Console.WriteLine(data[0] + "clientreceive");
        UnamedGame.Instance.MyID = data[0];
        UnamedGame.Instance.ConnectedPlayers.Clear();
        UnamedGame.Instance.ConnectedPlayers[data[0]] = new GamePlayer(data[0], UnamedGame.Instance.player, true);
    }
    public byte ID;
    public override byte[] Send()
    {
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
        using (MemoryStream ms = new MemoryStream())
        {
            using (BinaryWriter b = new BinaryWriter(ms))
            {
                b.Write(UnamedGame.Instance.player.Bounds);
                b.Write("USer" + UnamedGame.random);
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
            p.ID = Server.Instance.connected.Count -1;
            b.Dispose();
        }
    }
}
