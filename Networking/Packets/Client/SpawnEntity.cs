using System.Net;
using Server;
namespace Server.Packets.Client;
public class SpawnEntity : ClientOrigniatingPacket
{
    public SpawnEntity()
    {
    }

    public SpawnEntity(byte[] data) : base(data)
    {
    }

    public SpawnEntity(byte[] data, IPEndPoint from) : base(data, from)
    {
    }

    public override byte[] Send()
    {
        throw new System.NotImplementedException();
    }
}