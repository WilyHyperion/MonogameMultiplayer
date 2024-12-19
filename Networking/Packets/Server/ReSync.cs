

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
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(data);

                using (BinaryReader b = new BinaryReader(ms))
                {
                    b.BaseStream.Seek(0, SeekOrigin.Begin);
                    int amount = b.ReadInt32();
                    for (int i = 0; i < amount; i++)
                    {
                         PlayerEntity p = b.ReadPlayer();
                         if(p.ID == UnamedGame.Instance.MyID){
                            Console.WriteLine("self player, checking desync");
                         }
                         else {
                            Console.WriteLine("Resyncing remote player");
                            UnamedGame.Instance.ReSyncPlayer(p);
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
                        writer.Write(s.connected.Values.ElementAt(i).player);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}