

using System;

namespace Server.Packets.Server {
    public class ReSyncPacket : ServerOriginatingPacket
    {
        public override void ClientReceive()
        {
        }

        public override byte[] Send()
        {throw new NotImplementedException();
        }
    }
}