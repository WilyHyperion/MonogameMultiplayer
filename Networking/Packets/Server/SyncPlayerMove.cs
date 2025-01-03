

using System;
using System.IO;
using System.Linq;
using System.Net;
using Game;
using Game.Helpers;
using Game.Player;
using Microsoft.Xna.Framework;

namespace Server.Packets.ServerSided
{
    public class SyncPlayerMove : ServerOriginatingPacket
    {
        private int id;
        private RectangleF bounds;
        private Vector2 velocity;

        public SyncPlayerMove()
        {
        }

        public SyncPlayerMove(byte[] data) : base(data)
        {
        }

        public SyncPlayerMove(byte[] data, IPEndPoint from) : base(data, from)
        {
        }

        public SyncPlayerMove(int id, RectangleF bounds, Vector2 velocity)
        {
            this.id = id;
            this.bounds = bounds;
            this.velocity = velocity;
        }

        public override void ClientReceive()
        {
            using (MemoryStream ms = new())
            {
                ms.Write(data);

                using (BinaryReader b = new(ms))
                {
                    b.BaseStream.Seek(0, SeekOrigin.Begin);
                    int id = b.ReadInt32();
                    if (UnamedGame.Instance.ConnectedPlayers.TryGetValue(id, out GamePlayer p)){
                        p.player.Bounds = b.ReadRectangleF();
                        p.player.Velocity = b.ReadVector2();
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
                    writer.Write(this.id);
                    writer.Write(this.bounds);
                    writer.Write(this.velocity);
                }
                return ms.ToArray();
            }
        }
    }
}