

using System;
using System.IO;
using System.Linq;
using System.Net;
using Game;
using Game.Helpers;
using Game.Player;

namespace Server.Packets.ServerSided
{
    public class ReSyncPacket : ServerOriginatingPacket
    {
        public ReSyncPacket()
        {
        }

        public ReSyncPacket(byte[] data) : base(data)
        {
        }

        public ReSyncPacket(byte[] data, IPEndPoint from) : base(data, from)
        {
        }

        public override void ClientReceive()
        {
            using (MemoryStream ms = new())
            {
                ms.Write(data);

                using (BinaryReader b = new(ms))
                {
                    b.BaseStream.Seek(0, SeekOrigin.Begin);
                    int amount = b.ReadInt32();
                    for (int i = 0; i < amount; i++)
                    {
                        Console.WriteLine($"  bytes left at {i}   {b.BaseStream.Length - b.BaseStream.Position} ");
                        var id = b.ReadInt32();
                         if(id == UnamedGame.Instance.MyID){
                            Console.WriteLine("self player, checking desync");
                            b.ReadRectangleF();
                            b.ReadVector2();
                         }
                         else {
                            Console.WriteLine("Resyncing remote player");
                            var rect  = b.ReadRectangleF();
                            var vel = b.ReadVector2();
                            if(UnamedGame.Instance.ConnectedPlayers.TryGetValue(id, out GamePlayer p)){
                                p.player.Bounds = rect;
                                p.player.Velocity = vel;
                            }
                            else {
                                UnamedGame.Instance.ConnectedPlayers.Add(id, new GamePlayer(id, new PlayerEntity(rect.Position), false));
                            }
                         }
                    }
                }
            }
        }

        public override byte[] Send()
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    Server s = Server.Instance;
                    writer.Write((int)s.connected.Count);
                    for (int i = 0; i < s.connected.Count; i++)
                    {
                        ServerPlayer player = s.connected.ElementAt(i).Value;
                        writer.Write(player.id);
                        writer.Write(player.player.Bounds);
                        writer.Write(player.player.Velocity);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}