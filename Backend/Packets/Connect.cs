

using System;
using System.IO;
using Game.Player;
using Microsoft.Xna.Framework;

namespace Server.Packets;

public class Connect : Packet
{
    public Connect(byte[] data) : base(data)
    {
    }
    public Connect() : base(){
        
    }
  

    public override byte[] ServerHandle()
    {
        using MemoryStream res = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(res);
        writer.Write(Server.Instance.players.Count);
        foreach(PlayerEntity p in Server.Instance.players) {
            writer.Write(p.name);
            writer.Write(p.Bounds.X);
            writer.Write(p.Bounds.Y);
        }
        using MemoryStream memoryStream = new MemoryStream(this.data);
        using BinaryReader b = new BinaryReader(memoryStream);
        b.ReadByte();
        string name = b.ReadString();
        Vector2 pos = new Vector2(b.ReadInt16(), b.ReadInt16());
        Server.Instance.players.Add(new Game.Player.PlayerEntity(pos));
        return res.ToArray();
    }

    public override void ClientReceive()
    {
        using MemoryStream memoryStream = new MemoryStream(this.data);
        using BinaryReader b = new BinaryReader(memoryStream);
        b.ReadByte();
        Int16 amount = b.ReadInt16();
        for(int i = 0; i < amount; i++){
            string name = b.ReadString();
            Vector2 pos = new Vector2(b.ReadInt16(), b.ReadInt16());
            Game.UnamedGame.Instance.AddNewRemote(name, pos);
        }
    }

    public override byte[] Send()
    {
        using MemoryStream res = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(res);
        writer.Write(PacketTypes.GetPacketType<Connect>());
        writer.Write(Game.UnamedGame.Instance.player.name);
        writer.Write((Int16)Game.UnamedGame.Instance.player.Bounds.X);
        writer.Write((short)Game.UnamedGame.Instance.player.Bounds.Y);
        return res.ToArray();
    }
}